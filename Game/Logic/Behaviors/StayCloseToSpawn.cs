using RotMG.Common;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.Behaviors
{
    class StayCloseToSpawn : Behavior
    {

        //State storage: target position
        //assume spawn=host.StateObject[Id] entry position

        float speed;
        int range;
        public StayCloseToSpawn(float speed, int range = 5)
        {
            this.speed = speed;
            this.range = range;
        }

        public override void Enter(Entity host)
        {
            host.StateObject[Id] = host.Position;
        }

        public override bool Tick(Entity host)
        {
            if (host.HasConditionEffect(ConditionEffectIndex.Paralyzed))
                return false;

            if (!(host.StateObject.GetValueOrDefault(Id) is Vector2))
            {
                host.StateObject[Id] = host.Position;
                return false;
            }

            var vect = (Vector2)host.StateObject[Id];
            if ((vect - host.Position).Length() > range)
            {
                vect -= host.Position;
                vect.Normalize();
                float dist = host.GetSpeed(speed) * Settings.SecondsPerTick;
                host.ValidateAndMove(vect * dist + host.Position);
            }
            return false;
        }

    }
}
