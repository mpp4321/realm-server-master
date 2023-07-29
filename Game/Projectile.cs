using RotMG.Common;
using RotMG.Game.Logic.ItemEffs;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace RotMG.Game
{
    public class Projectile
    {
        public readonly Action<Entity> OnHitDelegate = null;
        public readonly Entity Owner;
        public readonly ProjectileDesc Desc;
        public readonly int Id;
        public readonly float Angle;
        public readonly float OffsetX;
        public readonly float OffsetY;
        public float OverrideSpeed = -1f;
        public readonly Vector2 StartPosition;
        public int Damage;
        public readonly HashSet<int> Hit;
        public readonly IItemHandler[] UniqueEffects = new IItemHandler[] { };

        public int Time;
        public bool DidCrit = false;

        public Projectile(Entity owner, ProjectileDesc desc, int id, int time, float angle, Vector2 startPos, float offsetX, float offsetY, int damage, Action<Entity> hitDelegate = null, IItemHandler[] uniqueEff = null, float overrideSpeed = -1f)
        {
            Owner = owner;
            Desc = desc;
            Id = id;
            Time = time;
            Angle = MathUtils.BoundToPI(angle);
            StartPosition = startPos;
            Damage = damage;
            Hit = new HashSet<int>();
            OffsetX = offsetX;
            OffsetY = offsetY;
            OnHitDelegate = hitDelegate;
            UniqueEffects = uniqueEff ?? new IItemHandler[] { };
            OverrideSpeed = overrideSpeed;
        }

        // Does not clone hit map
        public Projectile(Projectile p, int newid)
        {
            Owner = p.Owner;
            Desc = p.Desc;
            Id = newid;
            Time = p.Time;
            Angle = p.Angle;
            OffsetX = p.OffsetX;
            OffsetY = p.OffsetY;
            StartPosition = p.StartPosition;
            Damage = p.Damage;
            Hit = new HashSet<int>();
            OnHitDelegate = p.OnHitDelegate;
            UniqueEffects = p.UniqueEffects;
            OverrideSpeed = p.OverrideSpeed;
        }

        public bool CanHit(Entity en)
        {
            if (!Hit.Contains(en.Id))
            {
                Hit.Add(en.Id);
                return true;
            }
            return false;
        }

        public float SpeedAt(float elapsed)
        {
            var speed = OverrideSpeed == -1f ? Desc.Speed : OverrideSpeed;
            var accel = Desc.Accelerate;
            if (Desc.DoAccelerate && elapsed > Desc.AccelerateDelay)
            {
                //var elapsedWithDelay = MathF.Max(0, elapsed - Desc.AccelerateDelay);
                var speedIncreaseByLifeTime = elapsed * (accel / 1000);
                //speed *= elapsed / Desc.LifetimeMS;
                speed += speedIncreaseByLifeTime;
            }

            var hasSpeedClamp = Desc.SpeedClamp > 0;

            if (hasSpeedClamp)
            {
                if (Desc.Speed > Desc.SpeedClamp)
                {

                    if (speed < Desc.SpeedClamp)
                    {
                        speed = Desc.SpeedClamp;
                    }

                }
                else
                {
                    //Desc.Speed < Desc.SpeedClamp
                    if (speed > Desc.SpeedClamp)
                    {
                        speed = Desc.SpeedClamp;
                    }
                }
            }
            return speed;
        }

        public Vector2 PositionAt(float elapsed)
        {
            elapsed = float.IsNaN(elapsed) ? 0.0f : elapsed;
            var p = new Vector2(StartPosition.X + OffsetX, StartPosition.Y + OffsetY);
            //var speed = SpeedAt(elapsed)//////;
            var accel = OverrideSpeed == -1f ? Desc.Accelerate : OverrideSpeed;

            //if (Desc.Decelerate != 0.0f) speed *= 2 - elapsed / Desc.LifetimeMS;

            //var distBeforeAccel = MathF.Min(elapsed, Desc.AccelerateDelay) * (Desc.Speed / 10000f);
            //var distAfterAccel = MathF.Max(0, elapsed - Desc.AccelerateDelay) * (speed / 10000f);
            //var dist = distBeforeAccel + distAfterAccel;

            float dist;
            var elapsedT = elapsed / 1000.0f;
            // time in T for constant velocity before acceleration
            var delayT = Desc.AccelerateDelay / 1000.0f;
            var distDelayT = Desc.Speed * delayT;

            if (Desc.DoAccelerate && elapsedT >= delayT)
            {
                var delta = -Desc.Speed / accel;
                if ((elapsedT - delayT) > delta && delta > 0)
                {
                    dist = (accel * MathF.Pow(delta, 2) / 2.0f) + (Desc.Speed * delta);
                }
                else
                {
                    dist = (accel * MathF.Pow(elapsedT - delayT, 2) / 2.0f) + (Desc.Speed * (elapsedT - delayT));
                }
                //Add the movement before acceleration
                dist += distDelayT;
                dist /= 10.0f;
            }
            else
            {
                dist = elapsed * Desc.Speed / 10000f;
            }

            if(Desc.Rotate)
            {
                var t = Desc.RadialSpeed * elapsed / Desc.LifetimeMS * 2 * MathF.PI;
                if(dist > Desc.MaxRadius)
                {
                    dist = Desc.MaxRadius;
                }
                return new Vector2(p.X + dist * MathF.Cos(t + Angle), p.Y + dist * MathF.Sin(t + Angle));
            }

            //Phase != 1 -> MathF.PI locked else 0 lock
            float phase = Desc.PhaseLock == 1 ? 0 : MathF.PI;

            if (Desc.PhaseLock == -1)
                phase = Id % 2 == 0 ? 0 : MathF.PI;

            var theta = Angle;
            if (Desc.Wavy && Desc.Amplitude == 0)
            {
                var periodFactor = 6 * MathF.PI;
                var amplitudeFactor = MathF.PI / 64.0f;
                theta = theta + amplitudeFactor * MathF.Sin(phase + periodFactor * elapsed / 1000.0f);
                p.X = p.X + dist * MathF.Cos(theta);
                p.Y = p.Y + dist * MathF.Sin(theta);
                return p;
            }

            if (Desc.Parametric)
            {
                var t = elapsed / Desc.LifetimeMS * 2 * MathF.PI;
                var x = MathF.Sin(t) * (Id % 2 == 1 ? 1 : -1);
                var y = MathF.Sin(2 * t) * (Id % 4 < 2 ? 1 : -1);
                var sin = MathF.Sin(theta);
                var cos = MathF.Cos(theta);
                p.X = p.X + (x * cos - y * sin) * Desc.Magnitude;
                p.Y = p.Y + (x * sin + y * cos) * Desc.Magnitude;
                return p;
            }

            if (Desc.Boomerang)
            {
                var halfway = Desc.LifetimeMS * (Desc.Speed / 10000) / 2;
                if (dist > halfway)
                {
                    dist = halfway - (dist - halfway);
                }
            }

            p.X = p.X + dist * MathF.Cos(theta);
            p.Y = p.Y + dist * MathF.Sin(theta);
            if (Desc.Amplitude != 0)
            {
                var ampFactor = Desc.Amplitude;
                if (Desc.Wavy)
                {
                    ampFactor *= MathF.Pow(elapsed / Desc.LifetimeMS, 1.4f);
                }
                var deflection = ampFactor * MathF.Sin(phase + elapsed / Desc.LifetimeMS * Desc.Frequency * 2 * MathF.PI);
                p.X = p.X + deflection * MathF.Cos(theta + MathF.PI / 2);
                p.Y = p.Y + deflection * MathF.Sin(theta + MathF.PI / 2);
            }
            return p;
        }
    }
}
