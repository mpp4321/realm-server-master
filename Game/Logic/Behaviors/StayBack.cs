﻿using RotMG.Common;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RotMG.Game.Logic.Behaviors
{
    class StayBack : Behavior
    {
        //State storage: cooldown timer

        float speed;
        float distance;
        string entity;

        public StayBack(float speed, float distance = 8, string entity = null)
        {
            this.speed = speed;
            this.distance = distance;
            this.entity = entity;
        }

        public override void Enter(Entity host)
        {
            host.StateObject[Id] = null;
        }

        public override bool Tick(Entity host)
        {
            var state = host.StateObject[Id];
            int cooldown;
            if (state == null) cooldown = 1000;
            else cooldown = (int)state;

            if (host.HasConditionEffect(ConditionEffectIndex.Paralyzed))
                return false;

            Entity e = entity != null ? 
                GameUtils.GetNearestEntity(host, distance, Resources.Id2Object[entity].Type) : 
                GameUtils.GetNearestPlayer(host, distance);

            var returnRes = false;

            if (e != null)
            {
                Vector2 vect = Vector2.Zero;
                vect = e.Position - host.Position;
                vect.Normalize();
                float dist = host.GetSpeed(speed) * (Settings.MillisecondsPerTick / 1000f);
                if(dist > 0)
                {
                    host.ValidateAndMove(-dist * vect + host.Position);
                    returnRes = true; 
                }

                if (cooldown <= 0)
                {
                    cooldown = 1000;
                }
                else
                {
                    cooldown -= Settings.MillisecondsPerTick;
                }
            }

            host.StateObject[Id] = cooldown;
            return returnRes;
        }
    }
}
