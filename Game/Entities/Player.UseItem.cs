using RotMG.Common;
using RotMG.Networking;
using RotMG.Utils;
using System;
using System.Collections.Generic;

namespace RotMG.Game.Entities
{
    public partial class Player
    {
        public const float UseCooldownThreshold = 1.1f;
        public const int MaxAbilityDist = 14;

        public Queue<ushort> ShootAEs;
        public int UseDuration;
        public int UseTime;

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
                case "Attack": return GetStat(2);
                case "Defense": return GetStat(3);
                case "Speed": return GetStat(4);
                case "Dexterity": return GetStat(5);
                case "Vitality": return GetStat(6);
                case "Wisdom": return GetStat(7);
            }
            return GetStat(7);
        }

        public static int StatScaling(int stat, float amount, int min, float scale)
        {
            return (int)(((stat - min) * scale) + amount);
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
            }

            var inRange = Position.Distance(target) <= MaxAbilityDist && Parent.GetTileF(target.X, target.Y) != null;
            Action callback = null;
            foreach (var eff in desc.ActivateEffects)
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
                            return;
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
                                var d = GetNextDamage(desc.Projectile.MinDamage, desc.Projectile.MaxDamage, ItemDatas[slot.SlotId]);
                                var a = t == null ? MathUtils.NextAngle() : Position.Angle(t.Position);
                                var p = new List<Projectile>()
                                {
                                     new Projectile(this, desc.Projectile, startId + i, time, a, Position, d)
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
                            var radius = StatScaling(statForScale, eff.Radius, eff.StatMin, eff.StatRangeScale);
                            var line = GameServer.ShowEffect(ShowEffectIndex.Line, Id, 0xFFFF0000 , target);
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
                                    lifeSucked += dmg;
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
                                    var radius = StatScaling(statForScale, eff.Radius, eff.StatMin, eff.StatRangeScale);
                                    var tdamage = StatScaling(statForScale, eff.TotalDamage, eff.StatMin, eff.StatScale);
                                    Parent.AddEntity(new Trap(this, radius, tdamage, eff.Effects), target);
                                }
                            });
                        }
                        break;
                    case ActivateEffectIndex.Lightning:
                        {
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
                                    lines[i] = GameServer.ShowEffect(ShowEffectIndex.Line, Id, 0xffff0088, new Vector2(x, y), new Vector2(350, 0));
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
                                var dmg = StatScaling(statForScale, eff.TotalDamage, eff.StatMin, eff.StatScale); 
                                (current as Enemy).Damage(this, dmg, eff.Effects, false, true);
                                for (var i = 1; i < eff.MaxTargets + 1; i++)
                                {
                                    pkts.Add(GameServer.ShowEffect(ShowEffectIndex.Lightning, prev.Id, 0xffff0088,
                                        new Vector2(current.Position.X, current.Position.Y),
                                        new Vector2(350, 0)));

                                    if (i == eff.MaxTargets) 
                                        break;

                                    Entity next;
                                    if(current.Parent == null)
                                    {
                                        current.Parent = this.Parent;
                                        next = current.GetNearestEnemy(10, targets);
                                        current.Parent = null;
                                    } else
                                    {
                                        next = current.GetNearestEnemy(10, targets);
                                    }

                                    if (next == null) break;

                                    targets.Add(next);
                                    (next as Enemy).Damage(this, dmg, eff.Effects, false, true);
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
                        break;
                    case ActivateEffectIndex.PoisonGrenade:
                        if (inRange)
                        {
                            var placeholder = new Placeholder();
                            Parent.AddEntity(placeholder, target);

                            var @throw = GameServer.ShowEffect(ShowEffectIndex.Throw, Id, 0xffddff00, target);
                            var nova = GameServer.ShowEffect(ShowEffectIndex.Nova, placeholder.Id, 0xffddff00, new Vector2(eff.Radius, 0));

                            foreach (var j in Parent.PlayerChunks.HitTest(Position, SightRadius))
                                if (j is Player k && (k.Client.Account.Effects || k.Equals(this)))
                                    k.Client.Send(@throw);

                            Manager.AddTimedAction(1500, () =>
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
                            var range = StatScaling(statForScale, eff.Range, eff.StatMin, eff.StatScale);
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
                            var range = StatScaling(statForScale, eff.Range, eff.StatMin, eff.StatScale);
                            var color = eff.Effect == ConditionEffectIndex.Damaging ? 0xffff0000 : 0xffffffff;
                            var nova = GameServer.ShowEffect(ShowEffectIndex.Nova, Id, color, new Vector2(range, 0));
                            foreach (var j in Parent.PlayerChunks.HitTest(Position, Math.Max(range, SightRadius)))
                            {
                                if (j is Player k)
                                {
                                    if (Position.Distance(j) <= eff.Range)
                                        k.ApplyConditionEffect(eff.Effect, StatScaling(statForScale, eff.DurationMS, eff.StatMin, eff.StatDurationScale));
                                    if (k.Client.Account.Effects || k.Equals(this))
                                        k.Client.Send(nova);
                                }
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
                        if (inRange)
                            Teleport(time, target, true);
                        break;
                    case ActivateEffectIndex.Decoy:
                        Parent.AddEntity(new Decoy(this, Position.Angle(target), StatScaling(statForScale, eff.DurationMS, eff.StatMin, eff.StatScale)), Position);
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
                                var d = GetNextDamage(desc.Projectile.MinDamage, desc.Projectile.MaxDamage, ItemDatas[slot.SlotId]);
                                var p = new Projectile(this, desc.Projectile, startId + i, time, angleInc * i, target, d);
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
                            return;
                        }
                            // callback = () =>
                            // {
                            //     // con.Inventory[slot.SlotId].Type = desc.Type;
                            //     // con.UpdateInventorySlot(slot.SlotId);
                            //     SendError("You already have a backpack.");
                            // };
                        // else
                        // {
                        //     HasBackpack = true;
                        //     SendInfo("8 more spaces. Woohoo!");
                        // }
                        HasBackpack = true;
                        SendInfo("8 more spaces. Woohoo!");
                        break;
                    case ActivateEffectIndex.Create:
                        if (!Resources.Id2Object.TryGetValue(eff.Id, out var obj))
                        {
#if DEBUG
                            Program.Print(PrintType.Error, $"{eff.Id} not found for AE Create");
#endif
                            return;
                        }

                        var entity = Resolve(obj.Type);
                        Parent.AddEntity(entity, Position);

                        // if (!(entity is Portal portal))
                        //     return;
                        //
                        // var notif = GameServer.Notification(Id, $"Opened by {Name}", 0xFF00FF00);
                        // var nearbyPlayers = Parent.PlayerChunks.HitTest(Position, SightRadius);
                        // foreach (var player in Parent.Players.Values)
                        // {
                        //     player.SendInfo($"{portal.DungeonName} opened by {Name}");
                        //     if (nearbyPlayers.Contains(player))
                        //         player.Client.Send(notif);
                        // }
                        break;
#if DEBUG
                    default:
                        Program.Print(PrintType.Error, $"Unhandled AE <{eff.Index.ToString()}>");
                        break;
#endif
                }
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
    }
}
