using RotMG.Common;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.Behaviors
{
    class OrbitSpawn : Behavior
    {

        private static Random Random = new Random();

        //State storage: orbit host.StateObject[Id]
        class OrbitState
        {
            public float Speed;
            public float Radius;
            public int Direction;
        }

        Vector2? OverPosition;
        float speed;
        float acquireRange;
        float radius;
        float speedVariance;
        float radiusVariance;
        bool? orbitClockwise;

        public OrbitSpawn(float speed, float radius, float acquireRange = 10,
            Vector2? overridePosition = null, float? speedVariance = null, float? radiusVariance = null,
            bool? orbitClockwise = false)
        {
            this.OverPosition = overridePosition;
            this.speed = speed;
            this.radius = radius;
            this.acquireRange = acquireRange;
            this.speedVariance = (float)(speedVariance ?? speed * 0.1);
            this.radiusVariance = (float)(radiusVariance ?? speed * 0.1);
            this.orbitClockwise = orbitClockwise;
        }

        private OrbitState BuildDefaultOrbit()
        {
            int orbitDir;
            if (orbitClockwise == null)
                orbitDir = (Random.Next(1, 3) == 1) ? 1 : -1;
            else
                orbitDir = ((bool)orbitClockwise) ? 1 : -1;

            return new OrbitState()
            {
                Speed = speed + speedVariance * (float)(Random.NextDouble() * 2 - 1),
                Radius = radius + radiusVariance * (float)(Random.NextDouble() * 2 - 1),
                Direction = orbitDir
            };
        }

        public override void Enter(Entity host)
        {
            host.StateObject[Id] = BuildDefaultOrbit();
        }

        public override bool Tick(Entity host)
        {
            OrbitState s = (OrbitState)host.StateObject[Id];

            if (s == null) s = BuildDefaultOrbit();

            if (host.HasConditionEffect(ConditionEffectIndex.Paralyzed))
                return false;

            var Position = this.OverPosition.HasValue ? OverPosition.Value : host.SpawnPoint;

            float angle;
            if (host.Position == Position) //small offset
                angle = MathF.Atan2((float)(host.Position.Y - Position.Y + (Random.NextDouble() * 2 - 1)), (float)(host.Position.X - Position.X + (Random.NextDouble() * 2 - 1)));
            else
                angle = MathF.Atan2(host.Position.Y - Position.Y, host.Position.X - Position.X);
            var angularSpd = s.Direction * host.GetSpeed(s.Speed) / s.Radius;
            angle += angularSpd * Settings.SecondsPerTick;

            float x = Position.X + MathF.Cos(angle) * s.Radius;
            float y = Position.Y + MathF.Sin(angle) * s.Radius;
            Vector2 vect = new Vector2(x, y) - host.Position;
            vect.Normalize();
            vect *= host.GetSpeed(s.Speed) * Settings.SecondsPerTick;

            host.ValidateAndMove(host.Position + vect);

            host.StateObject[Id] = s;
            return true;
        }

    }
}
