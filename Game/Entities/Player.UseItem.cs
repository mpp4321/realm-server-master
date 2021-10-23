using RotMG.Common;
using RotMG.Networking;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RotMG.Game.Entities
{
    public partial class Player
    {
        public const float UseCooldownThreshold = 1.1f;
        public const int MaxAbilityDist = 14;

        public Queue<ushort> ShootAEs;
        public int UseDuration;
        public int UseTime;

        public Entity Pet = null;

        public void UsePortal(int objectId)
        {
            var entity = Parent.GetEntity(objectId);
            if (!(entity is Portal portal))
            {
#if DEBUG
                Program.Print(PrintType.Error, $"{entity} from UsePortal is not a portal");
#endif
                return;
            }
            
            if (entity.Position.Distance(this) > ContainerMinimumDistance)
            {
#if DEBUG
                Program.Print(PrintType.Error, "Too far away from portal");
#endif
                return;
            }

            var world = portal.GetWorldInstance(Client);
            if (world == null)
            {
                SendError($"{portal.Desc.DungeonName} not yet implemented");
                return;
            }
            
            if (!world.AllowedAccess(Client))
            {
                SendError("Access denied");
                return;
            }
            
            Client.Send(GameServer.Reconnect(world.Id));
            Manager.AddTimedAction(2000, Client.Disconnect);
        }

        public int ParseStatForScale(string val)
        {
            switch(val)
            {
                case "Attack": return GetStatTotal(2);
                case "Defense": return GetStatTotal(3);
                case "Speed": return GetStatTotal(4);
                case "Dexterity": return GetStatTotal(5);
                case "Vitality": return GetStatTotal(6);
                case "Wisdom": return GetStatTotal(7);
            }
            return GetStatTotal(7);
        }

        public static float StatScalingF(int stat, float amount, int min, float scale)
        {
            return (MathF.Max(0, (stat - min)) * scale) + amount;
        }

        public static int StatScaling(int stat, float amount, int min, float scale)
        {
            return (int) StatScalingF(stat, amount, min, scale);
        }

        public void TryUseItem(int time, SlotData slot, Vector2 target)
        {
            if (!ValidTime(time))
            {
#if DEBUG
                Program.Print(PrintType.Error, "Invalid time useitem");
#endif
                //Client.Disconnect();
                return;
            }

            if (slot.SlotId == HealthPotionSlotId)
            {
                if (HealthPotions > 0 && !HasConditionEffect(ConditionEffectIndex.Sick))
                {
                    Heal(100, false);
                    HealthPotions--;
                }
                return;
            }
            else if (slot.SlotId == MagicPotionSlotId)
            {
                if (MagicPotions > 0 && !HasConditionEffect(ConditionEffectIndex.Quiet))
                {
                    Heal(100, true);
                    MagicPotions--;
                }
                return;
            }

            var en = Parent.GetEntity(slot.ObjectId);
            if (slot.SlotId != 1)
                (en as IContainer)?.UpdateInventorySlot(slot.SlotId);
            if (en == null || !(en is IContainer))
            {
#if DEBUG
                Program.Print(PrintType.Error, "Undefined entity");
#endif
                return;
            }

            if (en is Player && !en.Equals(this))
            {
#if DEBUG
                Program.Print(PrintType.Error, "Trying to use items from another players inventory");
#endif
                return;
            }

            if (en is Container c)
            {

                if(en is GiftChest)
                {
#if DEBUG
                    Program.Print(PrintType.Error, "Player attempted to use item from gift chest.");
#endif
                    return;
                }

                if ((en as Container).OwnerId != -1 && (en as Container).OwnerId != AccountId)
                {
#if DEBUG
                    Program.Print(PrintType.Error, "Trying to use items from another players container/bag");
#endif
                    return;
                }

                if (en.Position.Distance(this) > ContainerMinimumDistance)
                {
#if DEBUG
                    Program.Print(PrintType.Error, "Too far away from container");
#endif
                    return;
                }
            }

            var con = en as IContainer;
            ItemDesc desc = null;
            if (con.Inventory[slot.SlotId] != -1)
                desc = Resources.Type2Item[(ushort)con.Inventory[slot.SlotId]];

            if (desc == null)
            {
#if DEBUG
                Program.Print(PrintType.Error, "Invalid use item");
#endif
                return;
            }

            var isAbility = slot.SlotId == 1 && en is Player;
            if (isAbility)
            {
                if (slot.ObjectId != Id)
                {
#if DEBUG
                    Program.Print(PrintType.Error, "Trying to use ability from a container?");
#endif
                    return;
                }

                if (UseTime + UseDuration * (1f / UseCooldownThreshold) > time)
                {
#if DEBUG
                    Program.Print(PrintType.Error, "Used ability too soon");
#endif
                    return;
                }

                if (MP - desc.MpCost < 0)
                {
#if DEBUG
                    Program.Print(PrintType.Error, "Not enough MP");
#endif
                    return;
                }

                var itemId = Inventory[slot.SlotId];
                var itemData = ItemDatas[slot.SlotId];
                var itemDesc = Resources.Type2Item[(ushort)(itemId)];
                var uef = itemDesc.UniqueEffect;

                foreach(var it in BuildAllItemHandlers())
                {
                    it.OnAbilityUse(target, itemDesc, itemData, this);
                }
            }

            var inRange = Position.Distance(target) <= MaxAbilityDist && Parent.GetTileF(target.X, target.Y) != null;
            Action callback = null;
            foreach (var eff in desc.ActivateEffects)
            {
                if (HandleAbilitySwitchStatement(eff, target, desc, time, inRange, slot)) return;
            }

            if (isAbility)
            {
                MP -= desc.MpCost;
                UseTime = time;
                var cooldownMod = ItemDesc.GetStat(ItemDatas[1], ItemData.Cooldown, ItemDesc.CooldownMultiplier);
                var cooldown = desc.CooldownMS;
                cooldown = cooldown + (int)(cooldown * -cooldownMod);
                UseDuration = cooldown;
                FameStats.AbilitiesUsed++;
            }

            if (desc.Potion)
                FameStats.PotionsDrank++;

            if (desc.Consumable)
            {
                con.Inventory[slot.SlotId] = -1;
                con.UpdateInventorySlot(slot.SlotId);
            }

            callback?.Invoke();
        }

        public bool HandleAbilitySwitchStatement(ActivateEffectDesc eff, Vector2 target, ItemDesc desc, int time, bool inRange, SlotData slot)
        {
            var statForScale = ParseStatForScale(eff.StatForScale);
            switch (eff.Index)
            {
                case ActivateEffectIndex.Heal:
                    if (!HasConditionEffect(ConditionEffectIndex.Sick))
                        Heal(StatScaling(statForScale, eff.Amount, eff.StatMin, eff.StatScale), false);
                    break;
                case ActivateEffectIndex.Magic:
                    if (!HasConditionEffect(ConditionEffectIndex.Quiet))
                        Heal(StatScaling(statForScale, eff.Amount, eff.StatMin, eff.StatScale), true);
                    break;
                case ActivateEffectIndex.IncrementStat:
                    if (eff.Stat == -1)
                    {
#if DEBUG
                        Program.Print(PrintType.Error, "Increment stat called without stat declared");
#endif
                        break;
                    }
                    var statMax = Resources.Type2Player[Type].Stats[eff.Stat].MaxValue;
                    if (Stats[eff.Stat] == statMax)
                    {
                        SendInfo($"{desc.Id} not consumed. Already at max");
                        return true;
                    }
                    Stats[eff.Stat] = Math.Min(statMax, Stats[eff.Stat] + eff.Amount);
                    UpdateStats();
                    break;
                case ActivateEffectIndex.Shuriken: //Could be optimized too, it's not great..
                    {
                        var nova = GameServer.ShowEffect(ShowEffectIndex.Nova, Id, 0xffeba134, new Vector2(2.5f, 0));

                        foreach (var j in Parent.EntityChunks.HitTest(Position, 2.5f))
                        {
                            if (j is Enemy k &&
                                !k.HasConditionEffect(ConditionEffectIndex.Invincible) &&
                                !k.HasConditionEffect(ConditionEffectIndex.Stasis))
                            {
                                k.ApplyConditionEffect(ConditionEffectIndex.Dazed, 1000);
                            }
                        }

                        var stars = new List<byte[]>();
                        var seeked = new HashSet<Entity>();
                        var startId = NextAEProjectileId;
                        var amt = StatScaling(statForScale, eff.Amount, eff.StatMin, eff.StatScale);
                        NextAEProjectileId += amt;

                        var angle = Position.Angle(target);
                        var cone = MathF.PI / 8;
                        for (var i = 0; i < amt; i++)
                        {
                            var t = this.GetNearestEnemy(8, angle, cone, target, seeked) ?? this.GetNearestEnemy(6, seeked);
                            if (t != null) seeked.Add(t);
                            var d = GetNextDamage(desc.NextProjectile(startId + i).MinDamage, desc.NextProjectile(startId + i).MaxDamage, ItemDatas[slot.SlotId]);
                            var a = t == null ? MathUtils.NextAngle() : Position.Angle(t.Position);
                            var p = new List<Projectile>()
                                {
                                     new Projectile(this, desc.NextProjectile(startId + i), startId + i, time, a, Position, d)
                                };

                            stars.Add(GameServer.ServerPlayerShoot(startId + i, Id, desc.Type, Position, a, 0, p));
                            AwaitProjectiles(p);
                        }

                        foreach (var j in Parent.PlayerChunks.HitTest(Position, SightRadius))
                        {
                            if (j is Player k)
                            {
                                if (k.Client.Account.Effects || k.Equals(this))
                                    k.Client.Send(nova);
                                if (k.Client.Account.AllyShots || k.Equals(this))
                                    foreach (var s in stars)
                                        k.Client.Send(s);
                            }
                        }
                    }
                    break;
                case ActivateEffectIndex.VampireBlast: //Maybe optimize this...?
                    if (inRange)
                    {
                        var radius = StatScalingF(statForScale, eff.Radius, eff.StatMin, eff.StatRangeScale);
                        radius = MathF.Max(radius, 1f);
                        var line = GameServer.ShowEffect(ShowEffectIndex.Line, Id, 0xFFFF0000, target);
                        var burst = GameServer.ShowEffect(ShowEffectIndex.Burst, Id, 0xFFFF0000, target, new Vector2(target.X + radius, target.Y));
                        var lifeSucked = 0;

                        var enemies = new List<Entity>();
                        var players = new List<Entity>();
                        var flows = new List<byte[]>();

                        foreach (var j in Parent.EntityChunks.HitTest(target, radius))
                        {
                            if (j is Enemy k &&
                                !k.HasConditionEffect(ConditionEffectIndex.Invincible) &&
                                !k.HasConditionEffect(ConditionEffectIndex.Stasis))
                            {
                                var dmg = StatScaling(statForScale, eff.TotalDamage, eff.StatMin, eff.StatScale);
                                k.Damage(this, dmg, eff.Effects, true, true);
                                lifeSucked += eff.TotalDamage;
                                enemies.Add(k);
                            }
                        }

                        foreach (var j in Parent.PlayerChunks.HitTest(Position, radius))
                        {
                            if (j is Player k)
                            {
                                players.Add(k);
                                k.Heal(lifeSucked, false);
                            }
                        }

                        if (enemies.Count > 0)
                        {
                            for (var i = 0; i < 5; i++)
                            {
                                var a = enemies[MathUtils.Next(enemies.Count)];
                                var b = players[MathUtils.Next(players.Count)];
                                flows.Add(GameServer.ShowEffect(ShowEffectIndex.Flow, b.Id, 0xffffffff, a.Position));
                            }
                        }

                        foreach (var j in Parent.PlayerChunks.HitTest(Position, SightRadius))
                        {
                            if (j is Player k)
                            {
                                if (k.Client.Account.Effects)
                                {
                                    k.Client.Send(line);
                                    foreach (var p in flows)
                                        k.Client.Send(p);
                                }

                                if (k.Client.Account.Effects || k.Equals(this))
                                    k.Client.Send(burst);
                            }
                        }
                    }
                    break;
                case ActivateEffectIndex.StasisBlast:
                    if (inRange)
                    {
                        var blast = GameServer.ShowEffect(ShowEffectIndex.Collapse, Id, 0xffffffff,
                            target,
                            new Vector2(target.X + 3, target.Y));
                        var notifications = new List<byte[]>();

                        foreach (var j in Parent.EntityChunks.HitTest(target, 3))
                        {
                            if (j is Enemy k)
                            {
                                if (k.HasConditionEffect(ConditionEffectIndex.StasisImmune))
                                {
                                    notifications.Add(GameServer.Notification(k.Id, "Immune", 0xff00ff00));
                                    continue;
                                }

                                if (k.HasConditionEffect(ConditionEffectIndex.Stasis))
                                    continue;

                                notifications.Add(GameServer.Notification(k.Id, "Stasis", 0xffff0000));
                                k.ApplyConditionEffect(ConditionEffectIndex.Stasis, eff.DurationMS);
                                k.ApplyConditionEffect(ConditionEffectIndex.StasisImmune, eff.DurationMS + 3000);
                            }
                        }

                        foreach (var j in Parent.PlayerChunks.HitTest(Position, SightRadius))
                        {
                            if (j is Player k)
                            {
                                if (k.Client.Account.Effects || k.Equals(this))
                                    k.Client.Send(blast);
                                if (k.Client.Account.Notifications || k.Equals(this))
                                    foreach (var n in notifications)
                                        k.Client.Send(n);
                            }
                        }
                    }
                    break;
                case ActivateEffectIndex.Trap:
                    if (inRange)
                    {
                        var @throw = GameServer.ShowEffect(ShowEffectIndex.Throw, Id, 0xff9000ff, target);
                        foreach (var j in Parent.PlayerChunks.HitTest(Position, SightRadius))
                            if (j is Player k && (k.Client.Account.Effects || k.Equals(this)))
                                k.Client.Send(@throw);

                        Manager.AddTimedAction(1500, () =>
                        {
                            if (Parent != null)
                            {
                                var radius = StatScalingF(statForScale, eff.Radius, eff.StatMin, eff.StatRangeScale);
                                var tdamage = StatScaling(statForScale, eff.TotalDamage, eff.StatMin, eff.StatScale);
                                Parent.AddEntity(new Trap(this, radius, tdamage, eff.Effects), target);
                            }
                        });
                    }
                    break;
                case ActivateEffectIndex.Lightning:
                    {
                        Lightning(target, StatScaling(statForScale, eff.TotalDamage, eff.StatMin, eff.StatScale), eff.MaxTargets, eff.Color.HasValue ? eff.Color.Value : 0xffff3300, eff.Effects);
                    }
                    break;
                case ActivateEffectIndex.StrongLightning:
                    {
                        var lastIndex = 1;
                        Entity lastEntity = null;
                        var dmg = StatScaling(statForScale, eff.TotalDamage, eff.StatMin, eff.StatScale);
                        Func<int, int> _f = i =>
                        {
                            return 400 * (i - 1);
                        };
                        Lightning(target, dmg, eff.MaxTargets, eff.Color.HasValue ? eff.Color.Value : 0xffff3300, eff.Effects, (e, i) =>
                        {
                            lastIndex = i;
                            lastEntity = e;
                        }, -400);
                        var entitiesLeft = eff.MaxTargets - lastIndex;
                        if (entitiesLeft > 0 && lastEntity != null && lastEntity is Enemy enn)
                        {
                            var dmgSum = 0;
                            for (int z = lastIndex + 1; z < eff.MaxTargets + 1; z++)
                            {
                                dmgSum += dmg + _f(z);
                            }
                            enn.Damage(this, dmgSum, eff.Effects, false, true);
                            ApplyConditionEffect(ConditionEffectIndex.Quiet, 1000 * entitiesLeft + 1000);
                        }
                    }
                    break;
                case ActivateEffectIndex.PoisonGrenade:
                    if (inRange)
                    {
                        var placeholder = new Placeholder();
                        Parent.AddEntity(placeholder, target);

                        var @throw = GameServer.ShowEffect(ShowEffectIndex.Throw, Id, 0xffddff00, pos1: target, speed: eff.ThrowtimeMS);
                        var nova = GameServer.ShowEffect(ShowEffectIndex.Nova, placeholder.Id, 0xffddff00, new Vector2(eff.Radius, 0));

                        foreach (var j in Parent.PlayerChunks.HitTest(Position, SightRadius))
                            if (j is Player k && (k.Client.Account.Effects || k.Equals(this)))
                                k.Client.Send(@throw);

                        Manager.AddTimedAction(eff.ThrowtimeMS, () =>
                        {
                            if (placeholder.Parent != null)
                            {
                                if (Parent != null)
                                {
                                    foreach (var j in Parent.PlayerChunks.HitTest(Position, SightRadius))
                                        if (j is Player k && (k.Client.Account.Effects || k.Equals(this)))
                                            k.Client.Send(nova);
                                    foreach (var j in Parent.EntityChunks.HitTest(placeholder.Position, eff.Radius))
                                        if (j is Enemy e)
                                        {
                                            var dmg = StatScaling(statForScale, eff.TotalDamage, eff.StatMin, eff.StatScale);
                                            e.ApplyPoison(this, new ConditionEffectDesc[0], (int)(dmg
                                                / (StatScaling(statForScale, eff.DurationMS, eff.StatMin, eff.StatScale)
                                                / 1000f)), dmg);
                                        }
                                }
                                placeholder.Parent.RemoveEntity(placeholder);
                            }
                        });
                    }
                    break;
                case ActivateEffectIndex.HealNova:
                    {
                        var range = StatScalingF(statForScale, eff.Range, eff.StatMin, eff.StatRangeScale);
                        var nova = GameServer.ShowEffect(ShowEffectIndex.Nova, Id, 0xffffffff, new Vector2(range, 0));
                        foreach (var j in Parent.PlayerChunks.HitTest(Position, Math.Max(range, SightRadius)))
                        {
                            if (j is Player k)
                            {
                                if (Position.Distance(j) <= eff.Range)
                                    k.Heal(StatScaling(statForScale, eff.Amount, eff.StatMin, eff.StatScale), false);
                                if (k.Client.Account.Effects || k.Equals(this))
                                    k.Client.Send(nova);
                            }
                        }
                    }
                    break;
                case ActivateEffectIndex.ConditionEffectAura:
                    {
                        var center = eff.TargetCursor ? target : Position;
                        var range = StatScalingF(statForScale, eff.Range, eff.StatMin, eff.StatRangeScale);
                        var color = eff.Effect == ConditionEffectIndex.Damaging ? 0xffff0000 : 0xffffffff;
                        if (eff.Color.HasValue) color = eff.Color.Value;
                        byte[] nova;
                        if (eff.TargetCursor)
                        {
                            nova = GameServer.ShowEffect(ShowEffectIndex.Collapse, Id, color,
                                center,
                                new Vector2(center.X + range, target.Y));
                        }
                        else
                        {
                            nova = GameServer.ShowEffect(ShowEffectIndex.Nova, Id, color, new Vector2(range, 0));
                        }
                        foreach (var j in Parent.PlayerChunks.HitTest(center, Math.Max(range, SightRadius)))
                        {
                            if (j is Player k)
                            {
                                if (center.Distance(j) <= eff.Range)
                                    k.ApplyConditionEffect(eff.Effect, StatScaling(statForScale, eff.DurationMS, eff.StatMin, eff.StatDurationScale));
                                if (k.Client.Account.Effects || k.Equals(this))
                                    k.Client.Send(nova);
                            }
                        }
                    }
                    break;
                case ActivateEffectIndex.ConditionEffectBlast:
                    {
                        var center = eff.TargetCursor ? target : Position;
                        var range = StatScalingF(statForScale, eff.Range, eff.StatMin, eff.StatRangeScale);
                        var color = eff.Effect == ConditionEffectIndex.Damaging ? 0xffff0000 : 0xffffffff;
                        if (eff.Color.HasValue) color = eff.Color.Value;
                        byte[] nova;
                        if (eff.TargetCursor)
                        {
                            nova = GameServer.ShowEffect(ShowEffectIndex.Collapse, Id, color,
                                center,
                                new Vector2(center.X + range, target.Y));
                        }
                        else
                        {
                            nova = GameServer.ShowEffect(ShowEffectIndex.Nova, Id, color, new Vector2(range, 0));
                        }

                        foreach (var j in Parent.PlayerChunks.HitTest(center, Math.Max(range, SightRadius)))
                        {
                            if (!(j is Player h)) continue;
                            if (h.Client.Account.Effects || h.Equals(this))
                                h.Client.Send(nova);
                        }

                        foreach (var j in Parent.EntityChunks.HitTest(center, Math.Max(range, SightRadius)).OfType<Enemy>())
                        {
                            if (center.Distance(j) <= eff.Range)
                                j.ApplyConditionEffect(eff.Effect, StatScaling(statForScale, eff.DurationMS, eff.StatMin, eff.StatDurationScale));
                        }
                    }
                    break;
                case ActivateEffectIndex.ConditionEffectSelf:
                    {
                        ApplyConditionEffect(eff.Effect, StatScaling(statForScale, eff.DurationMS, eff.StatMin, eff.StatDurationScale));

                        var nova = GameServer.ShowEffect(ShowEffectIndex.Nova, Id, 0xffffffff, new Vector2(1, 0));
                        foreach (var j in Parent.PlayerChunks.HitTest(Position, SightRadius))
                            if (j is Player k && k.Client.Account.Effects)
                                k.Client.Send(nova);
                    }
                    break;
                case ActivateEffectIndex.Dye:
                    if (desc.Tex1 != 0)
                        Tex1 = desc.Tex1;
                    if (desc.Tex2 != 0)
                        Tex2 = desc.Tex2;
                    break;
                case ActivateEffectIndex.Shoot:
                    if (!HasConditionEffect(ConditionEffectIndex.Stunned))
                        ShootAEs.Enqueue(desc.Type);
                    break;
                case ActivateEffectIndex.Teleport:
                    if (eff.Position.X != 0 && eff.Position.Y != 0 && (eff.Map?.Equals(Parent.Name) ?? false))
                    {
                        ApplyConditionEffect(ConditionEffectIndex.Invincible, 5000);
                        ApplyConditionEffect(ConditionEffectIndex.Invisible, 5000);
                        Teleport(time, eff.Position, false);
                    }
                    else if (inRange)
                    { //Less than 0 means its a manual teleport thingy like a scroll
                        Teleport(time, target, true);
                    }
                    break;
                case ActivateEffectIndex.Decoy:
                    Parent.AddEntity(new Decoy(this,
                        Position.Angle(target),
                            StatScaling(
                                statForScale,
                                eff.DurationMS,
                                eff.StatMin,
                                eff.StatScale
                            ),
                        eff.Id == null ? (ushort)1813 : Resources.Id2Object[eff.Id].Type), Position);
                    break;
                case ActivateEffectIndex.BulletNova:
                    if (inRange)
                    {
                        var projs = new List<Projectile>(20);
                        var novaCount = 20;
                        var startId = NextAEProjectileId;
                        var angleInc = MathF.PI * 2 / novaCount;
                        NextAEProjectileId += novaCount;
                        for (var i = 0; i < novaCount; i++)
                        {
                            var d = GetNextDamage(desc.NextProjectile(startId + i).MinDamage, desc.NextProjectile(startId + i).MaxDamage, ItemDatas[slot.SlotId]);
                            var p = new Projectile(this, desc.NextProjectile(startId - i), startId + i, time, angleInc * i, target, d);
                            projs.Add(p);
                        }

                        AwaitProjectiles(projs);

                        var line = GameServer.ShowEffect(ShowEffectIndex.Line, Id, 0xFFFF00AA, target);
                        var nova = GameServer.ServerPlayerShoot(startId, Id, desc.Type, target, 0, angleInc, projs);

                        foreach (var j in Parent.PlayerChunks.HitTest(Position, SightRadius))
                        {
                            if (j is Player k)
                            {
                                if (k.Client.Account.Effects)
                                    k.Client.Send(line);
                                if (k.Client.Account.AllyShots || k.Equals(this))
                                    k.Client.Send(nova);
                            }
                        }
                    }
                    break;
                case ActivateEffectIndex.Backpack:
                    if (HasBackpack)
                    {
                        SendError("You already have a backpack");
                        return true;
                    }
                    HasBackpack = true;
                    SendInfo("8 more spaces. Woohoo!");
                    break;
                case ActivateEffectIndex.Create:
                    if (!Resources.Id2Object.TryGetValue(eff.Id, out var obj))
                    {
#if DEBUG
                        Program.Print(PrintType.Error, $"{eff.Id} not found for AE Create");
#endif
                        return true;
                    }
                    var entity = Resolve(obj.Type);
                    Parent.AddEntity(entity, Position);
                    break;
                case ActivateEffectIndex.ItemDataModifier:
                    SaveToCharacter();
                    Client.Character.ItemDataModifier = eff.StatForScale;
                    Client.Character.Save();
                    SendInfo("Your luck begins to change.");
                    break;
                case ActivateEffectIndex.StatBoostSelf:
                    EffectBoosts.Add(new BoostTimer()
                    {
                        amount = eff.Amount,
                        timer = eff.DurationMS / 1000,
                        index = eff.Stat
                    });
                    UpdateStats();
                    break;
                case ActivateEffectIndex.StatBoostAura:
                    {
                        var range = eff.Range;
                        var color = eff.Color.HasValue ? eff.Color.Value : 0xffffffff;
                        var nova = GameServer.ShowEffect(ShowEffectIndex.Nova, Id, color, new Vector2(range, 0));
                        foreach (var j in Parent.PlayerChunks.HitTest(Position, Math.Max(range, SightRadius)))
                        {
                            if (j is Player k)
                            {
                                k.EffectBoosts.Add(new BoostTimer()
                                {
                                    amount = eff.Amount,
                                    timer = eff.DurationMS / 1000.0f,
                                    index = eff.Stat
                                });
                                k.UpdateStats();
                                k.Client.Send(nova);
                            }
                        }
                    }
                    break;
                case ActivateEffectIndex.PermaPet:
                    if (Pet != null)
                    {
                        Pet.Parent?.RemoveEntity(Pet);
                    }
                    string objId = eff.Id;
                    int petId = Resources.Id2Object[objId].Type;
                    CreateAndAddPet(petId);
                    SendInfo("Enjoy your new pet!");
                    break;
                case ActivateEffectIndex.RuneConsume:
                    if (Client.Character != null)
                    {
                        string runeId = eff.Id;
                        if (Client.Character.SelectedRunes.Any(a => a.Equals(runeId)))
                        {
                            SendInfo("You have that rune already!");
                            return true;
                        }
                        Client.Character.SelectedRunes = Client.Character.SelectedRunes.Concat(new string[] { runeId }).ToArray();
                        SendInfo("Added rune");
                        UpdateRunes();
                    }
                    else
                    {
                        SendInfo("Reconnect and try again.");
                        return true;
                    }
                    break;
                case ActivateEffectIndex.UnlockSkin:
                    {
                        ItemDataJson dt = ItemDatas[slot.SlotId];
                        if (dt != null && dt.SkinId != null)
                        {
                            var directid = Resources.Id2Skin[dt.SkinId.Replace("_", " ")].Type;
                            if (Client.Account.OwnedSkins.Contains(directid))
                            {
                                SendInfo("Already unlocked!");
                                return true;
                            }
                            else
                            {
                                Client.Account.OwnedSkins.Add(directid);
                            }
                            SendInfo("Unlocked!");
                        }
                        else
                        {
                            SendInfo("This unlocker is invalid. Contact an admin.");
                            return true;
                        }
                    }
                    break;
                case ActivateEffectIndex.RemoveFromBag:
                    {
                        ItemDataJson dt = ItemDatas[slot.SlotId];
                        if (dt != null && dt.StoredItems != null)
                        {
                            if (dt.StoredItems.Count > 0)
                            {
                                var topItem = dt.StoredItems[0];
                                var freeSlot = GetFreeInventorySlot();
                                if (freeSlot != -1)
                                {
                                    Inventory[freeSlot] = topItem;
                                    ItemDatas[freeSlot] = new ItemDataJson();
                                    dt.StoredItems.RemoveAt(0);
                                    UpdateInventorySlot(freeSlot);
                                }
                            }
                        }
                        else
                        {
                            SendInfo("This item bag is invalid, contact admin.");
                            return true;
                        }
                    }
                    break;
                case ActivateEffectIndex.MagicCrystal:
                    ItemDataModType typeOfMod = Enum.Parse<ItemDataModType>(eff.Id ?? Client.Character.ItemDataModifier);
                    int power = eff.Amount;
                    float scale = eff.StatScale;
                    for(int i = 0; i < Inventory.Length; i++)
                    {
                        if(Inventory[i] != -1 && ItemDesc.GetRank(ItemDatas[i].Meta) == -1)
                        {
                            ItemDatas[i] = Resources.Type2Item[(ushort)Inventory[i]]
                                .Roll(r: new Logic.LootDef.RarityModifiedData(scale, power, true), typeOfMod).Item2;
                            UpdateInventorySlot(i);
                        }
                    }
                    break;
#if DEBUG
                default:
                    Program.Print(PrintType.Error, $"Unhandled AE <{eff.Index.ToString()}>");
                    break;
#endif
            }
            return false;
        }
        public void Lightning(Vector2 target, int dmg, int targetCount, uint color, ConditionEffectDesc[] effs = null, Action<Entity, int> callback = null, int DamageFallOff = 0)
        {
            effs = effs ?? new ConditionEffectDesc[] { };
            var angle = Position.Angle(target);
            var cone = MathF.PI / 4;
            var start = this.GetNearestEnemy(MaxAbilityDist, angle, cone, target);

            if (start == null)
            {
                var angles = new float[3] { angle, angle - cone, angle + cone };
                var lines = new byte[3][];
                for (var i = 0; i < 3; i++)
                {
                    var x = (int)(MaxAbilityDist * MathF.Cos(angles[i])) + Position.X;
                    var y = (int)(MaxAbilityDist * MathF.Sin(angles[i])) + Position.Y;
                    lines[i] = GameServer.ShowEffect(ShowEffectIndex.Line, Id, color, new Vector2(x, y), new Vector2(350, 0));
                }

                foreach (var j in Parent.PlayerChunks.HitTest(Position, SightRadius))
                {
                    if (j is Player k && k.Client.Account.Effects)
                    {
                        k.Client.Send(lines[0]);
                        k.Client.Send(lines[1]);
                        k.Client.Send(lines[2]);
                    }
                }
            }
            else
            {
                Entity prev = this;
                var current = start;
                var targets = new HashSet<Entity>();
                var pkts = new List<byte[]>();
                targets.Add(current);
                //var dmg = StatScaling(statForScale, eff.TotalDamage, eff.StatMin, eff.StatScale);
                (current as Enemy).Damage(this, dmg, effs, false, true);
                if(callback != null) callback(current, 1);
                for (var i = 1; i < targetCount + 1; i++)
                {
                    pkts.Add(GameServer.ShowEffect(ShowEffectIndex.Lightning, prev.Id, 0xffff0088,
                        new Vector2(current.Position.X, current.Position.Y),
                        new Vector2(350, 0)));

                    if (i == targetCount)
                        break;

                    Entity next;
                    if (current.Parent == null)
                    {
                        current.Parent = this.Parent;
                        next = current.GetNearestEnemy(10, targets);
                        current.Parent = null;
                    }
                    else
                    {
                        next = current.GetNearestEnemy(10, targets);
                    }

                    if (next == null) break;

                    targets.Add(next);
                    (next as Enemy).Damage(this, dmg - (i*DamageFallOff), effs, false, true);
                    if (callback != null) callback(next, i);
                    prev = current;
                    current = next;
                }

                foreach (var j in Parent.PlayerChunks.HitTest(Position, SightRadius))
                    if (j is Player k && k.Client.Account.Effects)
                        foreach (var p in pkts)
                        {
                            Console.WriteLine(p.Length);
                            k.Client.Send(p);
                        }
            }
        }

        public Entity CreateAndAddPet(int id)
        {
            if (id <= 0) return null;
            Entity pet = new Entity((ushort) id);
            pet.PlayerOwner = this;
            Client.Character.PetId = id; 
            Parent.AddEntity(pet, Position);
            Pet = pet;
            return pet;
        }

    }
}
