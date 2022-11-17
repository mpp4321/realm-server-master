using RotMG.Common;
using RotMG.Game.Logic.ItemEffs;
using RotMG.Networking;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RotMG.Game.Entities
{
    public class AoeAck
    {
        public int Damage;
        public ConditionEffectDesc[] Effects;
        public Vector2 Position;
        public string Hitter;
        public float Radius;
        public int Time;
    }
    public struct ExplosionAck
    {
        public static ExplosionAck Undefined = new ExplosionAck
        {
            Projectile = null,
            Time = -1
        };

        public Projectile Projectile;
        public int Time;

        public override bool Equals(object obj)
        {
#if DEBUG
            if (obj is null)
                throw new Exception("Undefined object");
#endif
            return (obj as ProjectileAck?).Value.Projectile.Id == Projectile.Id;
        }

        public override int GetHashCode()
        {
            return Projectile.Id;
        }
    }

    public struct ProjectileAck
    {
        public static ProjectileAck Undefined = new ProjectileAck
        {
            Projectile = null,
            Time = -1
        };

        public Projectile Projectile;
        public int Time;

        public override bool Equals(object obj)
        {
#if DEBUG
            if (obj is null)
                throw new Exception("Undefined object");
#endif
            return (obj as ProjectileAck?).Value.Projectile.Id == Projectile.Id;
        }

        public override int GetHashCode()
        {
            return Projectile.Id;
        }
    }

    public partial class Player
    {
        private const int TimeUntilAckTimeout = 5000;
        private const int TickProjectilesDelay = 2000;
        private const float RateOfFireThreshold = 1.2f;
        private const float EnemyHitRangeAllowance = 1.7f;
        private const float EnemyHitTrackPrecision = 8;
        private const int EnemyHitHistoryBacktrack = 2;

        public Queue<List<Projectile>> AwaitingProjectiles;
        public Dictionary<int, ProjectileAck> AckedProjectiles;
        public Queue<AoeAck> AwaitingAoes; //Doesn't really belong here... But Player.Aoe.cs???

        public Dictionary<int, Projectile> ShotProjectiles;
        // Projectiles that explode get sent here when shot or AckShot then we receive
        // AckExplosion which spawns the projectiles server side after getting checked here
        public Dictionary<int, ExplosionAck> AckedExplosions;

        public int NextAEProjectileId = int.MinValue; //Goes up positively from bottom (Server sided projectiles)
        public int NextProjectileId; //Goes down negatively (Client sided projectiles)

        public int ShotTime;
        public int ShotDuration;
        public int BurstShotDelay = 0;
        public int BurstShotCount = 0;

        public void TickProjectiles()
        {
            if (Manager.TotalTime % TickProjectilesDelay != 0)
                return;

            foreach (var aoe in AwaitingAoes)
            {
                if (Manager.TotalTime - aoe.Time > TimeUntilAckTimeout)
                {
#if DEBUG
                    Program.Print(PrintType.Error, "Aoe ack timed out");
#endif
                    Client.Disconnect();
                    return;
                }
            }

            foreach (var explode in AckedExplosions)
            {
                var lifetime = explode.Value.Projectile.Desc.LifetimeMS;
                if (Manager.TotalTime - explode.Value.Time > lifetime + TimeUntilAckTimeout)
                {
#if DEBUG
                    Program.Print(PrintType.Error, "explode timed out");
#endif
                    //Client.Disconnect();
                    return;
                }
            }

            foreach (var apList in AwaitingProjectiles)
            {
                foreach (var ap in apList)
                {
                    if (Manager.TotalTime - ap.Time > TimeUntilAckTimeout)
                    {
#if DEBUG
                        Program.Print(PrintType.Error, "Proj ack timed out");
#endif
                        if(!(ap.Owner is Player))
                            Client.Disconnect();
                        return;
                    }
                }
            }
        }

        public int GetNextDamageSeeded(int min, int max, ItemDataJson data, float enchantmentStrength)
        {
            var dmgMod = ItemDesc.GetStat(data, ItemData.Damage, ItemDesc.DamageMultiplier, enchantmentStrength);
            var minDmg = min + (int)(min * dmgMod);
            var maxDmg = max + (int)(max * dmgMod);

            var damage = (int)Client.Random.NextIntRange((uint)minDmg, (uint)maxDmg);
            return damage;
        }

        public int GetNextDamage(int min, int max, ItemDataJson data, float enchantmentStrength=1f)
        {
            var dmgMod = ItemDesc.GetStat(data, ItemData.Damage, ItemDesc.DamageMultiplier, enchantmentStrength);
            var minDmg = min + (int)(min * dmgMod);
            var maxDmg = max + (int)(max * dmgMod);
            return MathUtils.NextInt(minDmg, maxDmg);
        }

        public float GetDynamicRangeAllowance(Projectile proj)
        {
            return (proj.Desc.Size / 200.0f) - 0.5f;
        }

        public void TryHitEnemy(int time, int bulletId, int targetId)
        {
            if (!ValidTime(time))
            {
#if DEBUG
                Program.Print(PrintType.Error, "Invalid time for enemy hit");
#endif
                Client.Disconnect();
                return;
            }

            if (ShotProjectiles.TryGetValue(bulletId, out var p))
            {
#if DEBUG
                Program.Print(PrintType.Info, "Hit with id: " + bulletId);
#endif
                var target = Parent.GetEntity(targetId);
                if (target == null || !target.Desc.Enemy)
                {
                    //Add entity to remove next update call
                    ToRemoveFromClient.Add(targetId);
                    return;
                }
                var elapsed = time - p.Time;
                var steps = (int)Math.Ceiling(p.SpeedAt(elapsed) / 100f * (elapsed * EnemyHitTrackPrecision / 1000f));
                var timeStep = steps == 0 ? 0 : (float)elapsed / steps;

                for (var k = 0; k <= steps; k++)
                {
                    var pos = p.PositionAt(k * timeStep);
                    if (k == steps) //Try hit enemy
                    {
                        if (target.Desc.Static)
                        {
                            if (pos.Distance(target.Position) <= (EnemyHitRangeAllowance + GetDynamicRangeAllowance(p)) && p.CanHit(target))
                            {
                                target.HitByProjectile(p);
                                if (!p.Desc.MultiHit)
                                    ShotProjectiles.Remove(p.Id);
                                return;
                            }
                        }
                        else
                        {
                            for (var j = 0; j <= EnemyHitHistoryBacktrack; j++)
                            {
                                if (pos.Distance(target.TryGetHistory(j)) <= (EnemyHitRangeAllowance + GetDynamicRangeAllowance(p)) && p.CanHit(target))
                                {
                                    target.HitByProjectile(p);
                                    if (!p.Desc.MultiHit)
                                        ShotProjectiles.Remove(p.Id);
                                    return;
                                }
                            }
                        }
#if DEBUG
                    Console.WriteLine(pos);
                    Console.WriteLine(target);
                    Program.Print(PrintType.Error, "Enemy hit aborted, too far away from projectile");
#endif
                    }
                    else //Check collisions to make sure player isn't shooting through walls etc
                    {
                        var tile = Parent.GetTileF(pos.X, pos.Y);

                        if (tile == null || tile.Type == 255 ||
                            tile.StaticObject != null && !tile.StaticObject.Desc.Enemy && (tile.StaticObject.Desc.EnemyOccupySquare || !p.Desc.PassesCover && tile.StaticObject.Desc.OccupySquare))
                        {
#if DEBUG
                            Program.Print(PrintType.Error, "Shot projectile hit wall, removed");
#endif
                            ShotProjectiles.Remove(bulletId);
                            return;
                        }
                    }
                }
            }
            else
            {
#if DEBUG
                Program.Print(PrintType.Error, "Tried to hit enemy with undefined projectile id: " + bulletId);
#endif
            }
        }

        public void AddShotProjectiles(int time, Projectile projectile)
        {
            if(ShotProjectiles.TryAdd(projectile.Id, projectile))
            {
                if(projectile.Desc.ExplodeCount != 0)
                {
                    AckedExplosions.Add(projectile.Id, new Game.Entities.ExplosionAck()
                    {
                        Projectile = projectile,
                        Time = time
                    });
                }
            }
        }

        public void TryShoot(int time, Vector2 pos, float attackAngle, bool ability, int numShots, int expectedProjectileId)
        {
            if (AwaitingGoto.Count > 0)
            {
                Client.Random.Drop(numShots);
                return;
            }

            var startId = NextProjectileId;
            if(expectedProjectileId != NextProjectileId)
            {
                var diff = expectedProjectileId - NextProjectileId;
                Client.Send(GameServer.ShootDesync(diff));
            }
            NextProjectileId -= numShots;

            if (!ValidTime(time))
            {
#if DEBUG
                Program.Print(PrintType.Error, "Invalid time for player shoot");
#endif
                Client.Disconnect();
                return;
            }

            if (!ValidMove(time, pos))
            {
#if DEBUG
                Program.Print(PrintType.Error, "Invalid move for player shoot");
#endif
                if(AwaitingGoto.Count == 0)
                    Client.Disconnect();
                return;
            }

            ItemDesc desc = ability ? GetItem(1) : GetItem(0);
            if (desc == null)
            {
#if DEBUG
                Program.Print(PrintType.Error, "Undefined item descriptor");
#endif
                Client.Random.Drop(numShots * 2);
                return;
            }


            if (numShots != desc.NumProjectiles)
            {
#if DEBUG
                Program.Print(PrintType.Error, "Manipulated num shots");
#endif
                Client.Random.Drop(numShots * 2);
                return;
            }

            if (HasConditionEffect(ConditionEffectIndex.Stunned))
            {
#if DEBUG
                Program.Print(PrintType.Error, "Stunned...");
#endif
                Client.Random.Drop(numShots * 2);
                return;
            }

            if (ability)
            {
                if (ShootAEs.TryDequeue(out var aeItemType))
                {
                    if (aeItemType != desc.Type)
                    {
                        Client.Random.Drop(numShots * 2);
                        return;
                    }

                    var arcGap = desc.ArcGap * MathUtils.ToRadians;
                    var totalArc = arcGap * (numShots - 1);
                    var angle = attackAngle - totalArc / 2f;
                    for (var i = 0; i < numShots; i++)
                    {
                        var damage = GetNextDamageSeeded(desc.NextProjectile(startId - i).MinDamage, desc.NextProjectile(startId - i).MaxDamage, ItemDatas[1], desc.EnchantmentStrength);
                        var didCrit = Client.Random.NextFloat() * 100 < GetStatTotal(9);
                        if (didCrit) damage *= 2;
                        damage = (int)(damage * GetAttackMultiplier());
                        //var uneffs = this.Inventory.Take(4).Select(a => Resources.Type2Item[Convert.ToUInt16(a)].UniqueEffect).Where(a => a != null).ToArray();
                        var projectile = new Projectile(this, desc.NextProjectile(startId - i), startId - i, time, angle + arcGap * i, pos, damage);
                        AddShotProjectiles(time, projectile);
                    }

                    var packet = GameServer.AllyShoot(Id, desc.Type, attackAngle);
                    foreach (var en in Parent.PlayerChunks.HitTest(Position, SightRadius))
                        if (en is Player player && player.Client.Account.AllyShots && !player.Equals(this))
                            player.Client.Send(packet);

                    FameStats.Shots += numShots;
                }
                else
                {
#if DEBUG
                    Program.Print(PrintType.Error, "Invalid ShootAE");
#endif
                    Client.Random.Drop(numShots);
                }
            }
            else
            {
                if (time > ShotTime + ShotDuration + BurstShotDelay)
                {
                    //Reset burst shot delay
                    BurstShotDelay = 0;
                    var arcGap = desc.ArcGap * MathUtils.ToRadians;
                    var totalArc = arcGap * (numShots - 1);
                    var angle = attackAngle - totalArc / 2f;
                    var abilityEffects = BuildAllItemHandlers();
                    for (var i = 0; i < numShots; i++)
                    {
                        var pdesc = desc.NextProjectile(Math.Abs(startId - i));
                        var damage = GetNextDamageSeeded(pdesc.MinDamage, pdesc.MaxDamage, ItemDatas[1], desc.EnchantmentStrength);
                        var didCrit = Client.Random.NextFloat() * 100 < GetStatTotal(9);
                        if (didCrit)
                            damage *= 2;
                        damage = (int)(damage * GetAttackMultiplier());
                        //var compeffs = this.ItemDatas.Take(4).Select(a => a.ItemComponent != null ? a.ItemComponent : null).Where(a => a != null);
                        var uneffs = BuildAllItemHandlers();
                        var projectile = new Projectile(this, pdesc, startId - i, time, angle + arcGap * i, pos, damage, uniqueEff: uneffs.ToArray());
                        foreach (var ae in abilityEffects) ae.OnProjectileShoot(this, ref projectile);
                        AddShotProjectiles(time, projectile);
                    }

                    var packet = GameServer.AllyShoot(Id, desc.Type, attackAngle);

                    foreach (var en in Parent.PlayerChunks.HitTest(Position, SightRadius))
                        if (en is Player player && player.Client.Active && player.Client.Account.AllyShots && !player.Equals(this))
                            player.Client.Send(packet);

                    FameStats.Shots += numShots;
                    var rateOfFireMod = ItemDesc.GetStat(ItemDatas[0], ItemData.RateOfFire, ItemDesc.RateOfFireMultiplier, desc.EnchantmentStrength);
                    var rateOfFire = desc.RateOfFire;
                    rateOfFire *= 1 + rateOfFireMod;
                    ShotDuration = (int)(1f / GetAttackFrequency() * (1f / rateOfFire) * (1f / RateOfFireThreshold));
                    ShotTime = time;

                    BurstShotCount += numShots;
                    if(desc.DoBurst && desc.BurstCount <= BurstShotCount + 1)
                    {
                        BurstShotDelay = desc.BurstDelay;
                        BurstShotCount = 0;
                    }
                }
                else
                {
#if DEBUG
                    Program.Print(PrintType.Error, "Shot too early, ignored");
#endif
                    Client.Random.Drop(numShots * 2);
                }
            }
        }

        public void AwaitAoe(AoeAck aoe)
        {
            AwaitingAoes.Enqueue(aoe);
        }

        public bool CheckProjectiles(int time)
        {
            foreach (var p in ShotProjectiles.ToArray())
            {
                var elapsed = time - p.Value.Time;
                if (elapsed > p.Value.Desc.LifetimeMS)
                {
#if DEBUG
                    //Program.Print(PrintType.Error, "Shot projectile removed");
#endif
                    ShotProjectiles.Remove(p.Key);
                    continue;
                }
            }
            foreach (var p in AckedProjectiles.ToArray()) 
            {
                var elapsed = time - p.Value.Time;
                if (elapsed > p.Value.Projectile.Desc.LifetimeMS)
                {
#if DEBUG
                    //Program.Print(PrintType.Error, "Proj lifetime expired");
#endif
                    AckedProjectiles.Remove(p.Key);
                    AckedExplosions.Remove(p.Key);
                    continue;
                }

                var pos = p.Value.Projectile.PositionAt(elapsed);
                var dx = Math.Abs(Position.X - pos.X);
                var dy = Math.Abs(Position.Y - pos.Y);
                if (dx <= 0.4f && dy <= 0.4f)
                {
                    if (p.Value.Projectile.CanHit(this))
                    {
                        AckedProjectiles.Remove(p.Key);
                        AckedExplosions.Remove(p.Key);
                        if (HitByProjectile(p.Value.Projectile))
                        {
#if DEBUG
                            //Program.Print(PrintType.Error, "Died cause of server collision");
#endif
                            return true;
                        }
#if DEBUG
                        //Program.Print(PrintType.Error, "Collided on server");
#endif
                    }
#if DEBUG
                    else
                    {
                        Program.Print(PrintType.Error, "In range but can't hit...?");
                    }
#endif
                }
            }
            return false;
        }

        public void TryHit(int bulletId)
        {
            if (AckedProjectiles.TryGetValue(bulletId, out var v))
            {
                if (v.Projectile.CanHit(this))
                {
                    HitByProjectile(v.Projectile);
                    AckedProjectiles.Remove(bulletId);
                    if(!v.Projectile.Desc.MultiHit)
                        AckedExplosions.Remove(bulletId);
                }
            }
            else
            {
#if DEBUG
                Program.Print(PrintType.Error, $"Tried to hit with undefined projectile {bulletId}");
#endif
                var diff = bulletId - NextProjectileId;
                Client.Send(GameServer.ShootDesync(diff));
            }
        }

        public IEnumerable<IItemHandler> BuildAllItemHandlers()
        {
            var equips = Inventory.Take(4).Select(a => Resources.Type2Item.GetValueOrDefault((ushort) a))
                .Where(a => a != null && a.UniqueEffect != null)
                .Select( a => 
                    ItemHandlerRegistry.Registry[a.UniqueEffect]
                );
            if (Client.Character != null)
            {
                var runes = Client.Character.SelectedRunes.Select(a => ItemHandlerRegistry.Registry[a]);
                equips = equips.Concat(runes);
            }
            if(UniqueEffectsFromSet.Count > 0)
            {
                equips = equips.Concat(
                    UniqueEffectsFromSet.Select(a => ItemHandlerRegistry.Registry[a])
                );
            }
            return equips;
        }

        public override bool HitByProjectile(Projectile projectile)
        {

            if (projectile.Owner is Player)
                return false;

            if (HasConditionEffect(ConditionEffectIndex.Invincible)
                || HasConditionEffect(ConditionEffectIndex.Stasis))
                return false;

            if(projectile.OnHitDelegate != null)
                projectile.OnHitDelegate(this);

            foreach(var v in BuildAllItemHandlers())
            {
                v.OnHitByEnemy(this, projectile.Owner, projectile);
            }

            return Damage(Resources.Type2Object[projectile.Desc.ContainerType].DisplayId,
                   projectile.Damage, 
                   projectile.Desc.Effects, 
                   projectile.Desc.ArmorPiercing);
        }

        public void AwaitProjectiles(List<Projectile> projectiles)
        {
            AwaitingProjectiles.Enqueue(projectiles);
            var explodingProjectiles = projectiles.Where(a => a.Desc.ExplodeCount != 0)
                .Select(a => new ExplosionAck()
                {
                    Projectile = a,
                    Time = a.Time
                });

            foreach(var a in explodingProjectiles) {
                AckedExplosions.Add(
                    a.Projectile.Id, a
                );
            }
        }

        private int IdByPlayer(bool isPlayer, int start, int k)
        {
            if (isPlayer) return start - k;
            return start + k;
        }

        public void ExplosionAck(int time, int bulletId)
        {
            if (!ValidTime(time))
            {
                Client.Disconnect();
                return;
            }

            if (AckedExplosions.TryGetValue(bulletId, out var ac))
            {
                var count = ac.Projectile.Desc.ExplodeCount;
                var projs = new List<Projectile>(count);
                var owner = ac.Projectile.Owner;
                var isPlayer = owner is Player;
                var startId = NextProjectileId;
                var desc = ac.Projectile.Desc.ExplodeProjectile;
                var Damage = desc.Damage;
                if(isPlayer)
                    NextProjectileId -= count;

                for (var i = 0; i < count; i++)
                {
                    var Angle = 360f / count * i * MathUtils.ToRadians;
                    var p = new Projectile(ac.Projectile.Owner, desc, startId - i, time, Angle, ac.Projectile.PositionAt(
                       time - ac.Projectile.Time
                    ), Damage);
                    projs.Add(p);
                    if (isPlayer)
                    {
                        // Players have already seen there shots since they're procing this
                        // Enemies need to explode server side and dispatch since players don't manage enemy shot ids
                        //   or we just make exploding player shots managed by client? nah
                        //
                        ShotProjectiles[startId - i] = p;
                    } else
                    {
                        // Since we attach the nextprojectileid to map info now we can do this
                        AckedProjectiles[startId - i] = new()
                        {
                            Projectile = p,
                            Time = time
                        };
                    }
                }

                // Nested exploders
                var explodingProjectiles = projs.Where(a => a.Desc.ExplodeCount != 0)
                    .Select(a => new ExplosionAck()
                    {
                        Projectile = a,
                        Time = a.Time
                    });

                foreach(var a in explodingProjectiles) {
                    AckedExplosions.Add(
                        a.Projectile.Id, a
                    );
                }
            } else
            {
            }
        }

        public void TryHitSquare(int time, int bulletId)
        {
            if (!ValidTime(time))
            {
#if DEBUG
                Program.Print(PrintType.Error, "HitSquare invalid time");
#endif
                Client.Disconnect();
                return;
            }

            if (AckedProjectiles.TryGetValue(bulletId, out var ac))
            {
                var pos = ac.Projectile.PositionAt(time - ac.Time);
                var tile = Parent.GetTileF(pos.X, pos.Y);

                if (tile == null || tile.Type == 255 || TileUpdates[(int)pos.X, (int)pos.Y] != Parent.Tiles[(int)pos.X, (int)pos.Y].UpdateCount ||
                    tile.StaticObject != null && (tile.StaticObject.Desc.EnemyOccupySquare || !ac.Projectile.Desc.PassesCover && tile.StaticObject.Desc.OccupySquare))
                {
                    AckedProjectiles.Remove(bulletId);
                    AckedExplosions.Remove(bulletId);
                }
#if DEBUG
                else
                {
                    Program.Print(PrintType.Error, "Manipualted SquareHit?");
                }
#endif
            }
#if DEBUG
            else
            {
                Program.Print(PrintType.Error, $"Tried to hit square with undefined projectile {bulletId}");
            }
#endif
        }

        public void TryAckAoe(int time, Vector2 pos)
        {
            if (!ValidTime(time))
            {
#if DEBUG
                Program.Print(PrintType.Error, "AoeAck invalid time");
#endif
                Client.Disconnect();
                return;
            }

            if (AwaitingAoes.TryDequeue(out var aoe))
            {
                if (!ValidMove(time, pos) && AwaitingGoto.Count == 0)
                {
#if DEBUG
                    Program.Print(PrintType.Error, "INVALID MOVE FOR AOEACK!");
#endif
                    Client.Disconnect();
                    return;
                }

                if (pos.Distance(aoe.Position) < aoe.Radius && !HasConditionEffect(ConditionEffectIndex.Invincible))
                {
                    Damage(aoe.Hitter, aoe.Damage, aoe.Effects, false);
                }
            }
            else
            {
#if DEBUG
                Program.Print(PrintType.Error, "AoeAck desync");
#endif
                Client.Disconnect();
            }
        }

        public void TryShootAck(int time)
        {
            if (!ValidTime(time))
            {
#if DEBUG
                Program.Print(PrintType.Error, "ShootAck invalid time");
#endif
                if(AwaitingGoto.Count() < 1)
                    Client.Disconnect();
                return;
            }

            if (AwaitingProjectiles.TryDequeue(out var projectiles))
            {
                foreach (var p in projectiles)
                {
                    if (p.Owner.Equals(this))
                    {
                        p.Time = time;
                        ShotProjectiles[p.Id] = p;
                    }
                    else
                    {
#if DEBUG
                        if (AckedProjectiles.ContainsKey(p.Id))
                        {
                            Program.Print(PrintType.Warn, "Duplicate ack key");
                        }
#endif
                        var ack = new ProjectileAck { Projectile = p, Time = time };
                        AckedProjectiles[p.Id] = ack;
                    }
                }
            }
            else
            {
#if DEBUG
                Program.Print(PrintType.Error, "ShootAck desync");
#endif
                if(AwaitingGoto.Count() < 1)
                    Client.Disconnect();
            }
        }
    }
}
