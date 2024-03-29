﻿using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Networking;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;

namespace RotMG.Game.Logic.Behaviors
{
    public class Shoot : Behavior
    {
        public const int PredictNumTicks = 4;

        public readonly float Range = 8.0f;
        public readonly byte Count;
        public readonly float ShootAngle;
        public float? FixedAngle;
        public readonly float? RotateAngle;
        public readonly float AngleOffset;
        public readonly float? DefaultAngle;
        public readonly float OffsetX;
        public readonly float OffsetY;
        public readonly float Predictive;
        public readonly int Index;
        public readonly int CooldownOffset;
        public readonly int CooldownVariance;
        public readonly int Cooldown;
        public int? DamageOverride = null;

        public readonly ConditionEffectIndex[] effect; 
        public readonly int effect_duration;
        public readonly Func<Entity, Player> playerOwner;

        public Action<Entity> Callback;

        public Shoot(
            float range = 5, 
            byte count = 1, 
            float? shootAngle = null, 
            int index = 0, 
            float? fixedAngle = null, 
            float? rotateAngle = null, 
            float angleOffset = 0, 
            float? defaultAngle = null,
            float predictive = 0,
            int cooldownOffset = 0,
            int cooldownVariance = 0,
            int cooldown = 0,
            ConditionEffectIndex[] effect = null,
            int effect_duration = 0,
            float offsetX = 0f,
            float offsetY = 0f,
            Action<Entity> callback = null,
            Func<Entity, Player> playerOwner = null)
        {
            Range = range;
            OffsetX = offsetX;
            OffsetY = offsetX;
            Count = count;
            ShootAngle = count == 1 ? 0 : (shootAngle ?? 360f / count) * MathUtils.ToRadians;
            Index = index;
            FixedAngle = fixedAngle * MathUtils.ToRadians;
            RotateAngle = rotateAngle * MathUtils.ToRadians;
            AngleOffset = angleOffset * MathUtils.ToRadians;
            DefaultAngle = defaultAngle * MathUtils.ToRadians;
            Predictive = predictive;
            CooldownOffset = cooldownOffset;
            CooldownVariance = cooldownVariance;
            Cooldown = cooldown;
            Callback = callback;

            this.effect = effect ?? new ConditionEffectIndex[] { };
            this.effect_duration = effect_duration;
            this.playerOwner = playerOwner;
        }

        public override void Enter(Entity host)
        {
            host.StateCooldown[Id] = CooldownOffset;
            if (RotateAngle != null) 
                host.StateObject[Id] = 0;
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

                bool alwaysShoot = Range < 0.0001f;

                if (playerOwner != null)
                {
                    target = host.GetNearestEnemy(Range);
                }
                else if (Range > 0 || alwaysShoot)
                    target = host.GetNearestPlayer(alwaysShoot ? 99f : Range);


                if (target != null || DefaultAngle != null || FixedAngle != null || alwaysShoot)
                {
                    var desc = host.Desc.Projectiles[Index + host.Desc.Projectiles.First().Key];
                    float angle = 0;

                    if (FixedAngle != null)
                    {
                        angle = (float)FixedAngle;
                    }
                    else if (target != null)
                    {
                        if (Predictive != 0 && Predictive > MathUtils.NextFloat())
                        {
                            Vector2 history;
                            if (target is Decoy) history = target.Position;
                            else history = target.TryGetHistory(1);
                            Vector2 targetPos = target.Position;
                            targetPos -= history;
                            targetPos *= PredictNumTicks;
                            targetPos += target.Position;
                            Vector2 dirVector = targetPos - (host.Position - new Vector2(OffsetX, OffsetY));
                            angle = (float)Math.Atan2(dirVector.Y, dirVector.X);
                        }
                        else
                        {
                            angle = (float)Math.Atan2(target.Position.Y - (host.Position.Y + OffsetY), target.Position.X - (host.Position.X + OffsetX));
                        }
                    }
                    else if (DefaultAngle != null)
                        angle = (float)DefaultAngle;

                    angle += AngleOffset;

                    if (RotateAngle != null)
                    {
                        if (host.StateObject[Id] == null) host.StateObject[Id] = 0;
                        var rotateCount = (int)host.StateObject[Id];
                        angle += (float)RotateAngle * rotateCount;
                        rotateCount++;
                        host.StateObject[Id] = rotateCount;
                    }

                    var damage = DamageOverride ?? desc.Damage;

                    if(host is Enemy enCast)
                    {
                        if (enCast.IsElite) damage = (int)(damage * 1.15f);
                    }

                    if (host.HasConditionEffect(ConditionEffectIndex.Damaging))
                        damage = (int)(damage * 1.5f);

                    if (host.HasConditionEffect(ConditionEffectIndex.Weak))
                        damage = (int)((DamageOverride ?? desc.Damage) * 0.5);

                    var startAngle = angle - ShootAngle * (count - 1) / 2;
                    var startId = host.Parent.NextProjectileId;
                    var owner = playerOwner != null ? playerOwner(host) : host;
                    host.Parent.NextProjectileId += count;

                    var projectiles = new List<Projectile>();

                    var burstCount = 1;
                    if(desc.DoStretchShot)
                    {
                        burstCount = desc.StretchShotCount;
                    }

                    for (byte k = 0; k < count; k++)
                    {
                        var p = new Projectile(owner, desc, startId + k, Manager.TotalTime, startAngle + ShootAngle * k, host.Position, 0f, 0f, damage, (e) => { 
                            if(effect.Length > 0)
                            {
                                foreach(var eff in effect)
                                {
                                    e.ApplyConditionEffect(eff, effect_duration);
                                }
                            }
                            if(Callback != null) Callback(e);
                        });
                       
                        if(desc.DoStretchShot)
                        {
                            for(var i = 0; i < burstCount; i++)
                            {
                                var copyP = new Projectile(p, k + startId + i);
                                copyP.OverrideSpeed = copyP.Desc.Speed - (i / burstCount) * copyP.Desc.Speed;
                                projectiles.Add(copyP);
                            }
                        } else
                        {
                            projectiles.Add(p);
                        }

                        if(owner is Player pl)
                        {
                            //if(pl.ShotProjectiles.ContainsKey(p.Id)) bad
                            pl.ShotProjectiles[p.Id] = p;
                        }
                    }

                    var packet = GameServer.EnemyShoot(startId, host.Id, desc.BulletType, host.Position, startAngle, (short)damage, count, ShootAngle, OffsetX, OffsetY);
                    var isOwnerPlayer = owner is Player;
                    var ownerAccId = isOwnerPlayer ? (owner as Player).AccountId : -1;

                    foreach (var en in host.Parent.PlayerChunks.HitTest(host.Position, Player.SightRadius))
                    {
                        if (en is Player player)
                        {
                            if (player.Entities.Contains(host))
                            {
                                if(!isOwnerPlayer || player.AccountId == ownerAccId)
                                {
                                    player.AwaitProjectiles(projectiles);
                                    player.Client.Send(packet);
                                }
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
            if (RotateAngle != null)
                host.StateObject.Remove(Id);
        }
    }
}
