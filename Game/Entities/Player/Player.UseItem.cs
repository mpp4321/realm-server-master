﻿using RotMG.Common;
using RotMG.Game.Logic.ItemEffs;
using RotMG.Networking;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using static RotMG.Game.Logic.LootDef;

namespace RotMG.Game.Entities
{
    public partial class Player
    {
        public const float UseCooldownThreshold = 1.1f;
        public const int MaxAbilityDist = 14;

        public static List<ConditionEffectIndex> NegativeEffects = new List<ConditionEffectIndex>() 
        {
            ConditionEffectIndex.ArmorBroken,
            ConditionEffectIndex.Blind,
            ConditionEffectIndex.Confused,
            ConditionEffectIndex.Drunk,
            ConditionEffectIndex.Stunned,
            ConditionEffectIndex.Bleeding,
            ConditionEffectIndex.CursedLower,
            ConditionEffectIndex.CursedHigher,
            ConditionEffectIndex.Dazed,
            ConditionEffectIndex.Weak,
            ConditionEffectIndex.Quiet,
            ConditionEffectIndex.Sick,
        };

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
            Client.IsReconnecting = true;
            Manager.AddTimedAction(2000, Client.Disconnect);
        }

        public void RemoveNegativeEffects()
        {
            foreach(var eff in NegativeEffects)
            {
                RemoveConditionEffect(eff);
            }
        }

        public int ParseStatForScale(string val)
        {
            switch(val)
            {
                case "Health": return GetStatTotal(0);
                case "Mana": return GetStatTotal(1);
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
                if (HandleAbilitySwitchStatement(eff, target, desc, time, inRange, slot, con)) return;
            }

            if (isAbility)
            {
                MP -= desc.MpCost;
                UseTime = time;
                var cooldownMod = ItemDesc.GetStat(ItemDatas[1], ItemData.Cooldown, ItemDesc.CooldownMultiplier, desc.EnchantmentStrength);
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


        public bool HandleAbilitySwitchStatement(ActivateEffectDesc eff, Vector2 target, ItemDesc desc, int time, bool inRange, SlotData slot, IContainer con)
        {
            var statForScale = ParseStatForScale(eff.StatForScale);
            switch (eff.Index)
            {
                case ActivateEffectIndex.UnlockPortal:
                    {
                        var nova = GameServer.ShowEffect(ShowEffectIndex.Nova, Id, 0xffffa500, new Vector2(3.0f, 0));
                        foreach(Portal portal in this.GetNearbyEntities(3.0f).OfType<Portal>())
                        {
                            // Locked WC
                            if(portal.Type == 0x0721)
                            {
                                //Unlocked WC
                                Entity new_portal = Entity.Resolve(0x0757);
                                portal.Parent?.AddEntity(new_portal, portal.Position);
                                // TODO popup text, needs temporary entity to carry this text
                                //var notif_text = GameServer.Notification(portal.Id, "Wine Cellar unlocked by " + this.Name, 0xff00ff00);
                                portal.Parent?.RemoveEntity(portal);
                                //GameUtils.ShowEffectRange(this, Parent, Position, 99f, notif_text);
                            }
                        }
                        GameUtils.ShowEffectRange(this, this.Parent, this.Position, 3.0f, nova);
                    }

                    break;
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
                                     new Projectile(this, desc.NextProjectile(startId + i), startId + i, time, a, Position, 0f, 0f, d)
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
                        var nova = GameServer.ShowEffect(ShowEffectIndex.Nova, placeholder.Id, eff.Color.HasValue ? eff.Color.Value : 0xffddff00, new Vector2(eff.Radius, 0));

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
                                        if (j is Player k && (k.Client?.Account.Effects ?? false || k.Equals(this)))
                                            k.Client.Send(nova);
                                    foreach (var j in Parent.EntityChunks.HitTest(placeholder.Position, eff.Radius))
                                        if (j is Enemy e)
                                        {
                                            var dmg = StatScaling(statForScale, eff.TotalDamage, eff.StatMin, eff.StatScale);
                                            e.ApplyPoison(this, new ConditionEffectDesc[0], (int)(dmg
                                                / (eff.DurationMS
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
                        ApplyConditionEffect(ConditionEffectIndex.Invincible, 4000);
                        ApplyConditionEffect(ConditionEffectIndex.Invisible, 4000);
                        ApplyConditionEffect(ConditionEffectIndex.Stunned, 4000);
                        Teleport(Manager.TotalTimeUnsynced, eff.Position, false);
                    }
                    else if (inRange)
                    { //Less than 0 means its a manual teleport thingy like a scroll
                        Teleport(Manager.TotalTimeUnsynced, target, true);
                    }
                    break;
                case ActivateEffectIndex.TeleportQuest:
                    {
                        if (Quest == null)
                        {
                            SendError("No quest to teleport to");
                            return true;
                        }
                        if (!Parent.AllowTeleport)
                        {
                            SendError("You cannot teleport in this area.");
                            return true;
                        }
                        ApplyConditionEffect(ConditionEffectIndex.Invincible, 2000);
                        ApplyConditionEffect(ConditionEffectIndex.Invisible, 2000);
                        ApplyConditionEffect(ConditionEffectIndex.Stunned, 2000);
                        EntityTeleport(_clientTime, Quest.Id, true);
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
                        // Backwards on purpose
                        var center = eff.TargetCursor ? Position : target;
                        var count = desc.NumProjectiles == 1 ? 20 : desc.NumProjectiles;
                        var projs = new List<Projectile>(count);
                        var novaCount = count;
                        var startId = NextAEProjectileId;
                        var angleInc = MathF.PI * 2 / novaCount;
                        NextAEProjectileId += novaCount;
                        for (var i = 0; i < novaCount; i++)
                        {
                            var d = GetNextDamage(desc.NextProjectile(startId + i).MinDamage, desc.NextProjectile(startId + i).MaxDamage, ItemDatas[slot.SlotId]);
                            var p = new Projectile(this, desc.NextProjectile(startId - i), startId + i, time, angleInc * i, center, 0f, 0f, d);
                            projs.Add(p);
                        }

                        AwaitProjectiles(projs);

                        var line = GameServer.ShowEffect(ShowEffectIndex.Line, Id, 0xFFFF00AA, center);
                        var nova = GameServer.ServerPlayerShoot(startId, Id, desc.Type, center, 0, angleInc, projs);

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
                    {
                        if (!Resources.Id2Object.TryGetValue(eff.Id, out var obj))
                        {
#if DEBUG
                        Program.Print(PrintType.Error, $"{eff.Id} not found for AE Create");
#endif
                            return true;
                        }
                        var entity = Resolve(obj.Type);
                        entity.PlayerOwner = this;
                        Parent.AddEntity(entity, Position);
                    }
                    break;
                case ActivateEffectIndex.TossObject:
                    {
                        if (!Resources.Id2Object.TryGetValue(eff.Id, out var obj))
                        {
#if DEBUG
                        Program.Print(PrintType.Error, $"{eff.Id} not found for AE Create");
#endif
                            return true;
                        }
                        if (inRange)
                        {
                            var placeholder = new Placeholder();
                            Parent.AddEntity(placeholder, target);

                            var @throw = GameServer.ShowEffect(ShowEffectIndex.Throw, Id, 0xffffffff, pos1: target, speed: eff.ThrowtimeMS);

                            foreach (var j in Parent.PlayerChunks.HitTest(Position, SightRadius))
                                if (j is Player k && (k.Client.Account.Effects || k.Equals(this)))
                                    k.Client.Send(@throw);

                            Manager.AddTimedAction(eff.ThrowtimeMS, () =>
                            {
                                if (placeholder.Parent != null)
                                {
                                    if (Parent != null)
                                    {
                                        var entity = Resolve(obj.Type);
                                        placeholder.Parent.AddEntity(entity, placeholder.Position);
                                        entity.PlayerOwner = this;
                                    }
                                    placeholder.Parent.RemoveEntity(placeholder);
                                }
                            });
                        }
                        //Parent.AddEntity(entity, Position);
                    }
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
                        const int MAX_FAME_REQ = 500;
                        SaveToCharacter();
                        string runeId = eff.Id;
                        int fameSumTotal = Client.Character.SelectedRunes.Count() == 0 ? 0
                            : Client.Character.SelectedRunes.Select(a => ItemHandlerRegistry.RuneFameCosts[a]).Aggregate((a, b) =>
                            {
                                return a + b;
                            });


                        if (Client.Character.SelectedRunes.Any(a => a.Equals(runeId)))
                        {
                            SendInfo("You have that rune already!");
                            return true;
                        }

                        if(fameSumTotal + ItemHandlerRegistry.RuneFameCosts[runeId] > MAX_FAME_REQ)
                        {
                            SendInfo("You have reached the maximum amount of points you can spend on runes (500)!");
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
                        var allowedItems = new List<int> {
                                                Resources.Id2Item["Potion of Attack"].Type,
                                                Resources.Id2Item["Potion of Dexterity"].Type,
                                                Resources.Id2Item["Potion of Speed"].Type,
                                                Resources.Id2Item["Potion of Life"].Type,
                                                Resources.Id2Item["Potion of Mana"].Type,
                                                Resources.Id2Item["Potion of Vitality"].Type,
                                                Resources.Id2Item["Potion of Wisdom"].Type,
                                                Resources.Id2Item["Potion of Defense"].Type,
                                                Resources.Id2Item["Realm Equipment Crystal"].Type
                                            };
                        if (dt != null && dt.StoredItems != null)
                        {
                            if (dt.StoredItems.Count > 0)
                            {
                                var topItem = dt.StoredItems[0];
                                var freeSlot = GetFreeInventorySlot();
                                if (freeSlot != -1)
                                {
                                    Inventory[freeSlot] = topItem;
                                    ItemDatas[freeSlot] = new ItemDataJson()
                                        {
                                            StoredItems = new List<int>(),
                                            AllowedItems = allowedItems,
                                        };
                                    dt.StoredItems.RemoveAt(0);
                                    UpdateInventorySlot(freeSlot);
                                    UpdateInventorySlot(slot.SlotId);
                                }
                            }
                        }
                        else
                        {

                            con.ItemDatas[slot.SlotId] = new ItemDataJson();
                            con.ItemDatas[slot.SlotId].StoredItems = new ();
                            con.ItemDatas[slot.SlotId].AllowedItems = allowedItems;
                            con.UpdateInventorySlot(slot.SlotId);
                            SendInfo("Bag broken, should be repaired.");
                            return true;
                        }
                    }
                    break;
                case ActivateEffectIndex.MagicCrystal:
                    // Deprecated use Mix.cs
                    return false;
                case ActivateEffectIndex.FishingRod:
                    var nearestSpot = GameUtils.GetNearestEntity(this, 8f, Resources.Id2Object["Fishing Spot"].Type);
                    if(nearestSpot != null && !HasConditionEffect(ConditionEffectIndex.Paralyzed)) 
                    {
                        Entity bob = new Entity(Resources.Id2Object["Fishing Bob"].Type);
                        bob.Id = Parent.GenerateNextObjectId();
                        bob.Position = nearestSpot.Position;
                        //Add fake pirate as temporary bob
                        ClientSideAdds.Add(bob);
                        EntityUpdates.Add(bob.Id, bob.UpdateCount);

                        const int FISH_TIME = 1000;
                        ApplyConditionEffect(ConditionEffectIndex.Paralyzed, FISH_TIME);
                        var worldBefore = Parent.Id;
                        if(MathUtils.Chance(0.01f))
                        {
                            // Break the rod
                            con.Inventory[slot.SlotId] = -1;
                            con.ItemDatas[slot.SlotId] = new();
                            SendInfo("Your fishing rod broke! Time to get a new one :).");
                        }
                        Manager.AddTimedAction(FISH_TIME, () =>
                        {
                            if(Parent != null) { 
                                var itemId = GetFreeInventorySlot();
                                if(itemId != -1)
                                {
                                    Inventory[itemId] = Resources.Id2Item[ FishingLootGenerator.Next() ].Type;
                                    ItemDatas[itemId] = new ItemDataJson();
                                    ToRemoveFromClient.Add(bob.Id);
                                    UpdateInventorySlot(itemId);
                                }
                            }
                        });

                    }
                    break;
                case ActivateEffectIndex.EggBreak:
                    {
                        int overCharge = (con.ItemDatas[slot.SlotId].Meta - 500000) / 100000;
                        overCharge = Math.Max(overCharge, 0);
                        overCharge = Math.Min(overCharge, 6);

                        if(desc.EggType == 1 && ItemDatas[slot.SlotId].Meta > 200000) 
                        {
                            var randomGreen = Player.greenUtItems[MathUtils.NextInt(0, Player.greenUtItems.Count() - 1)];
                            var itemDesc = Resources.Id2Item[randomGreen];
                            con.Inventory[slot.SlotId] = itemDesc.Type;
                            con.ItemDatas[slot.SlotId] = itemDesc.Roll(
                              r: new RarityModifiedData(1.0f, shift: overCharge, true)
                            ).Item2;
                        }
                        else if(desc.EggType == 2 && ItemDatas[slot.SlotId].Meta > 500000)
                        {
                            var randomBlue = Player.blueRtItems[MathUtils.NextInt(0, Player.blueRtItems.Count() - 1)];
                            var itemDesc = Resources.Id2Item[randomBlue];
                            con.Inventory[slot.SlotId] = itemDesc.Type;
                            con.ItemDatas[slot.SlotId] = itemDesc.Roll(
                              r: new RarityModifiedData(1.0f, shift: overCharge, true)
                            ).Item2;
                        }
                        else if(desc.EggType == 3 && ItemDatas[slot.SlotId].Meta > 100000)
                        {
                            var randomBlue = Player.blueRtItems[MathUtils.NextInt(0, Player.blueRtItems.Count() - 1)];
                            var itemDesc = Resources.Id2Item[randomBlue];
                            con.Inventory[slot.SlotId] = itemDesc.Type;
                            con.ItemDatas[slot.SlotId] = itemDesc.Roll(
                              r: new RarityModifiedData(1.0f, shift: overCharge, true)
                            ).Item2;
                        }
                        else
                        {
                            SendInfo("This egg is not ready to hatch yet!");
                        }
                        con.UpdateInventorySlot(slot.SlotId);
                    }
                    break;
                case ActivateEffectIndex.RemoveNegativeConditions:
                    {
                        var range = StatScalingF(statForScale, eff.Range, eff.StatMin, eff.StatRangeScale);
                        foreach (var j in Parent.PlayerChunks.HitTest(Position, Math.Max(range, SightRadius)))
                        {
                            if (j is Player k)
                            {
                                if (Position.Distance(j) <= eff.Range)
                                    k.RemoveNegativeEffects();
                            }
                        }
                    }
                    break;
                case ActivateEffectIndex.RemoveNegativeConditionsSelf:
                    RemoveNegativeEffects();
                    break;
                case ActivateEffectIndex.TransformBag:
                    {
                        ItemDataJson dt = ItemDatas[slot.SlotId];
                        if (dt != null && dt.StoredItems != null)
                        {
                            if (dt.StoredItems.Count > 3)
                            {
                                var allLegendaries = dt.StoredItems.Take(4).Select(v => Resources.Type2Item[(ushort)v]).All(v => v.BagType == 6);
                                if(allLegendaries)
                                {
                                    ItemDatas[slot.SlotId] = new ItemDataJson();
                                    Inventory[slot.SlotId] = Resources.Id2Item[legendaryTransformation[MathUtils.NextInt(0, legendaryTransformation.Count() - 1)]].Type;
                                    SendInfo("Congrats you got a new legendary!");
                                    UpdateInventorySlot(slot.SlotId);
                                    return true;
                                } else
                                {
                                    SendInfo("You must have 4 legendary tier items in this bag to continue.");
                                    return true;
                                }
                            } else
                            {
                                SendInfo("You must have 4 legendary tier items in this bag to continue.");
                                return true;
                            }
                        }
                        else
                        {

                            con.ItemDatas[slot.SlotId] = new ItemDataJson();
                            con.ItemDatas[slot.SlotId].StoredItems = new ();
                            con.ItemDatas[slot.SlotId].AllowedTier = 6;
                            con.UpdateInventorySlot(slot.SlotId);
                            SendInfo("Bag broken, should be repaired.");
                            return true;
                        }
                    }
                    break;
                case ActivateEffectIndex.FameConsume:
                    {
                        Client.Account.Stats.TotalFame += con.ItemDatas[slot.SlotId].MiscIntOne;
                        Client.Account.Stats.Fame += con.ItemDatas[slot.SlotId].MiscIntOne;
                        Fame = Client.Account.Stats.Fame;
                        con.Inventory[slot.SlotId] = -1;
                        con.ItemDatas[slot.SlotId] = new ItemDataJson() { };
                        con.UpdateInventorySlot(slot.SlotId);
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

        public void RandomGlow()
        {
            int r = MathUtils.NextInt(0, 255);
            int g = MathUtils.NextInt(0, 255);
            int b = MathUtils.NextInt(0, 255);
            var color = (r << 16) | (g << 8) | b;
            Client.Character.GlowColor = color;
            Glow = color;
        }

    }
}
