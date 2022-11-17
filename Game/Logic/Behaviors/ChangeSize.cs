using RotMG.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.Behaviors
{
    class ChangeSize : Behavior
    {

        //State storage: cooldown timer

        int rate;
        int target;

        public ChangeSize(int rate, int target)
        {
            this.rate = rate;
            this.target = target;
        }

        public override void Enter(Entity host)
        {
            host.StateObject[Id] = 0;
        }

        public override bool Tick(Entity host)
        {
            int cool = (int) host.StateObject[Id];

            if (cool <= 0)
            {
                var size = host.Size;
                if (size != target)
                {
                    size += rate;
                    size = Math.Min(Math.Max(size, 0), target);
                    host.Size = size;
                }
                cool = 150;
            }
            else
                cool -= Settings.MillisecondsPerTick;

            host.StateObject[Id] = cool;
            return true;
        }

    }
}
