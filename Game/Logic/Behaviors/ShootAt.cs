using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Networking;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RotMG.Game.Logic.Behaviors
{
    class ShootAt : Behavior
    {
        public readonly float Range = 8.0f;
        public readonly byte Count;
        public readonly int Index;
        public readonly int CooldownOffset;
        public readonly int CooldownVariance;
        public readonly int Cooldown;

        public readonly string Target;

        public int? DamageOverride = null;

        public ShootAt(
            string target,
            float range = 5, 
            byte count = 1, 
            int index = 0, 
            int cooldownOffset = 0,
            int cooldownVariance = 0,
            int cooldown = 0
        )
        {
            Target = target;
            Range = range;
            Count = count;
            Index = index;
            CooldownOffset = cooldownOffset;
            CooldownVariance = cooldownVariance;
            Cooldown = cooldown;
        }

        public override void Enter(Entity host)
        {
            host.StateCooldown[Id] = CooldownOffset;
        }

        public override bool Tick(Entity host)
        {
            host.StateCooldown[Id] -= Settings.MillisecondsPerTick;
            if (host.StateCooldown[Id] <= 0)
            {
                if (host.HasConditionEffect(ConditionEffectIndex.Stunned))
                    return false;

                var count = Count;
                if (host.HasConditionEffect(ConditionEffectIndex.Dazed))
                    count = (byte)Math.Ceiling(count / 2f);

                Entity target = null;
                target = host.GetNearbyEntities(Range).Where(a => a.Desc.Id.Equals(Target)).FirstOrDefault();

                if (target != null)
                {
                    var desc = host.Desc.Projectiles[Index + host.Desc.Projectiles.First().Key];
                    float angle = 0;

                    if (target != null)
                    {
                        angle = (float) Math.Atan2(target.Position.Y - host.Position.Y, target.Position.X - host.Position.X);
                    }

                    var damage = DamageOverride ?? desc.Damage;

                    var startId = host.Parent.NextProjectileId;

                    host.Parent.NextProjectileId += count;

                    var projectiles = new List<Projectile>();
                    for (byte k = 0; k < count; k++)
                    {
                        //var p = new Projectile(host, desc, startId + k, Manager.TotalTime, angle, host.Position, damage);
                        var p = host.BuildProjectile(Index, angle, k, damage);
                        projectiles.Add(p);
                    }

                    var packet = GameServer.EnemyShoot(startId, host.Id, desc.BulletType, host.Position, angle, (short)damage, count, angle);
                    
                    foreach (var en in host.Parent.PlayerChunks.HitTest(host.Position, Player.SightRadius))
                    {
                        if (en is Player player)
                        {
                            if (player.Entities.Contains(host))
                            {
                                player.AwaitProjectiles(projectiles);
                                player.Client.Send(packet);
                            }
                        }
                    }
                }

                host.StateCooldown[Id] = Cooldown;
                if (CooldownVariance != 0)
                    host.StateCooldown[Id] += MathUtils.NextIntSnap(-CooldownVariance, CooldownVariance, Settings.MillisecondsPerTick);
                return true;
            }
            return false;
        }

        public override void Exit(Entity host)
        {
            host.StateCooldown.Remove(Id);
        }

    }
}
