using RotMG.Common;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.Behaviors
{
    class Swirl : Behavior
    {
        private static Random Random = new Random();

        //State storage: swirl host.StateObject[Id]
        class SwirlState
        {
            public Vector2 Center;
            public bool Acquired;
            public int RemainingTime;
        }

        float speed;
        float acquireRange;
        float radius;
        bool targeted;
        public Swirl(double speed = 1, double radius = 8, double acquireRange = 10, bool targeted = true)
        {
            this.speed = (float)speed;
            this.radius = (float)radius;
            this.acquireRange = (float)acquireRange;
            this.targeted = targeted;
        }

        public override void Enter(Entity host)
        {
            host.StateObject[Id] = new SwirlState()
            {
                Center = targeted ? Vector2.Zero : new Vector2(host.Position.X, host.Position.Y),
                Acquired = !targeted,
            };
        }

        public override bool Tick(Entity host)
        {
            SwirlState s = (SwirlState)host.StateObject[Id];

            if (host.HasConditionEffect(ConditionEffectIndex.Paralyzed))
                return false;

            var period = (int)(1000 * radius / host.GetSpeed(speed) * (2 * Math.PI));
            if (!s.Acquired &&
                s.RemainingTime <= 0 &&
                targeted)
            {
                var entity = GameUtils.GetNearestPlayer(host, acquireRange);
                if (entity != null && entity.Position.X != host.Position.X && entity.Position.Y != host.Position.Y)
                {
                    //find circle which pass through host and player pos
                    var l = MathUtils.Distance(entity.Position, host);
                    var hx = (host.Position.X + entity.Position.X) / 2;
                    var hy = (host.Position.Y + entity.Position.Y) / 2;
                    var c = Math.Sqrt(Math.Abs(radius * radius - l * l) / 4);
                    s.Center = new Vector2(
                        (float)(hx + c * (host.Position.Y - entity.Position.Y) / l),
                        (float)(hy + c * (entity.Position.X - host.Position.X) / l));

                    s.RemainingTime = period;
                    s.Acquired = true;
                }
                else
                    s.Acquired = false;
            }
            else if (s.RemainingTime <= 0 || (s.RemainingTime - period > 200 && GameUtils.GetNearestPlayer(host, 2) != null))
            {
                if (targeted)
                {
                    s.Acquired = false;
                    var entity = GameUtils.GetNearestPlayer(host, acquireRange);
                    if (entity != null)
                        s.RemainingTime = 0;
                    else
                        s.RemainingTime = 5000;
                }
                else
                    s.RemainingTime = 5000;
            }
            else
                s.RemainingTime -= Settings.MillisecondsPerTick;

            double angle;
            if (host.Position.Y == s.Center.Y && host.Position.X == s.Center.X)//small offset
                angle = Math.Atan2(host.Position.Y - s.Center.Y + (Random.NextDouble() * 2 - 1), host.Position.X - s.Center.X + (Random.NextDouble() * 2 - 1));
            else
                angle = Math.Atan2(host.Position.Y - s.Center.Y, host.Position.X - s.Center.X);

            var spd = host.GetSpeed(speed) * (s.Acquired ? 1 : 0.2);
            var angularSpd = spd / radius;
            angle += angularSpd * Settings.SecondsPerTick;

            double x = s.Center.X + Math.Cos(angle) * radius;
            double y = s.Center.Y + Math.Sin(angle) * radius;
            Vector2 vect = new Vector2((float)x, (float)y) - new Vector2(host.Position.X, host.Position.Y);
            vect.Normalize();
            vect *= (float)spd * Settings.SecondsPerTick;

            host.ValidateAndMove(new Vector2(host.Position.X + vect.X, host.Position.Y + vect.Y));

            host.StateObject[Id] = s;

            return true;
        }

    }
}
