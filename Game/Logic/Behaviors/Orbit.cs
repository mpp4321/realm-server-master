﻿using RotMG.Common;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.Behaviors
{
    class Orbit : Behavior
    {

        private static Random Random = new Random();

        //State storage: orbit host.StateObject[Id]
        class OrbitState
        {
            public float Speed;
            public float Radius;
            public int Direction;
            public Entity entity = null;
        }

        float speed;
        float acquireRange;
        float radius;
        ushort? target;
        float speedVariance;
        float radiusVariance;
        bool? orbitClockwise;

        public Orbit(float speed, float radius, float acquireRange = 10,
            string target = null, float? speedVariance = null, float? radiusVariance = null,
            bool? orbitClockwise = false)
        {
            this.speed = speed;
            this.radius = radius;
            this.acquireRange = acquireRange;
            this.target = target == null ? null : (ushort?) Resources.Id2Object[target].Type;
            this.speedVariance = (float)(speedVariance ?? speed * 0.1);
            this.radiusVariance = (float)(radiusVariance ?? speed * 0.1);
            this.orbitClockwise = orbitClockwise;
        }

        public override void Enter(Entity host)
        {
            int orbitDir;
            if (orbitClockwise == null)
                orbitDir = (Random.Next(1, 3) == 1) ? 1 : -1;
            else
                orbitDir = ((bool)orbitClockwise) ? 1 : -1;

            host.StateObject[Id] = new OrbitState()
            {
                Speed = speed + speedVariance * (float)(Random.NextDouble() * 2 - 1),
                Radius = radius + radiusVariance * (float)(Random.NextDouble() * 2 - 1),
                Direction = orbitDir
            };
        }

        public override bool Tick(Entity host)
        {
            OrbitState s = (OrbitState)host.StateObject[Id];

            if (host.HasConditionEffect(ConditionEffectIndex.Paralyzed))
                return false;

            if(s.entity == null)
            {
                if (target.HasValue)
                    s.entity = GameUtils.GetNearestEntity(host, acquireRange, target.Value);
                else
                    s.entity = GameUtils.GetNearestEntity(host, acquireRange);
            }

            Entity entity = s.entity;

            if (entity != null)
            {
                float angle;
                if (host.Position == entity.Position) //small offset
                    angle = MathF.Atan2((float)(host.Position.Y - entity.Position.Y + (Random.NextDouble() * 2 - 1)), (float)(host.Position.X - entity.Position.X + (Random.NextDouble() * 2 - 1)));
                else
                    angle = MathF.Atan2(host.Position.Y - entity.Position.Y, host.Position.X - entity.Position.X);
                var angularSpd = s.Direction * host.GetSpeed(s.Speed) / s.Radius;
                angle += angularSpd * Settings.SecondsPerTick;

                float x = entity.Position.X + MathF.Cos(angle) * s.Radius;
                float y = entity.Position.Y + MathF.Sin(angle) * s.Radius;
                Vector2 vect = new Vector2(x, y) - host.Position;
                vect.Normalize();
                vect *= host.GetSpeed(s.Speed) * Settings.SecondsPerTick;

                host.ValidateAndMove(host.Position + vect);
            }

            host.StateObject[Id] = s;
            return true;
        }

    }
}
