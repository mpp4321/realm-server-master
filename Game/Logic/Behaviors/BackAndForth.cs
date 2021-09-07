using RotMG.Common;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.Behaviors
{
    class BackAndForth : Behavior
    {

        float speed;
        int distance;

        public BackAndForth(double speed, int distance = 5)
        {
            this.speed = (float)speed;
            this.distance = distance;
        }

        public override void Enter(Entity host)
        {
            if(!host.StateObject.ContainsKey(Id))
                host.StateObject[Id] = null;
        }

        public override bool Tick(Entity host)
        {
            float dist;
            if (host.StateObject[Id] == null) dist = distance;
            else dist = (float)host.StateObject[Id];

            if (host.HasConditionEffect(ConditionEffectIndex.Paralyzed))
                return false;

            float moveDist = host.GetSpeed(speed) * Settings.SecondsPerTick;
            if (dist > 0)
            {
                host.ValidateAndMove(new Vector2(host.Position.X + moveDist, host.Position.Y));
                dist -= moveDist;
                if (dist <= 0)
                {
                    dist = -distance;
                }
            }
            else
            {
                host.ValidateAndMove(new Vector2(host.Position.X - moveDist, host.Position.Y));
                dist += moveDist;
                if (dist >= 0)
                {
                    dist = distance;
                }
            }

            host.StateObject[Id] = dist;
            return true;
        }

    }
}
