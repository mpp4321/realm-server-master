using RotMG.Common;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.Behaviors
{
    class StayAbove : Behavior
    {
        //State storage: none

        float speed;
        int altitude;
        public StayAbove(double speed, int altitude)
        {
            this.speed = (float)speed;
            this.altitude = altitude;
        }

        public override bool Tick(Entity host)
        {
            if (host.HasConditionEffect(ConditionEffectIndex.Paralyzed))
                return false;

            var map = host.Parent.Map;
            var tile = map.Tiles[(int)host.Position.X, (int)host.Position.Y];
            if (tile.Elevation != 0 && tile.Elevation < altitude)
            {
                Vector2 vect;
                vect = new Vector2(map.Width / 2 - host.Position.X, map.Height / 2 - host.Position.Y);
                vect.Normalize();
                float dist = host.GetSpeed(speed) * Settings.MillisecondsPerTick;

                host.ValidateAndMove(new Vector2(host.Position.X + vect.X * dist, host.Position.Y + vect.Y * dist));
                return true;
            }
            return false;
        }
    }
}
