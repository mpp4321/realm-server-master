using RotMG.Common;
using RotMG.Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RotMG.Game.Logic.Behaviors
{
    class Decay : Behavior
    {

        int time;

        public Decay(int time = 10000)
        {
            this.time = time;
        }

        public override void Enter(Entity host)
        {
            host.StateObject[Id] = time;
        }

        public override bool Tick(Entity host)
        {
            int cool = (int) host.StateObject[Id];

            if (cool <= 0)
            {
                // Safe as possible since this could be couldnt be related to other behavior/tick stuff
                Manager.StartOfTickAction(() =>
                {
                    if(host?.Parent != null)
                        host.Parent.RemoveEntity(host);
                });
            }
            else
                cool -= Settings.MillisecondsPerTick;

            host.StateObject[Id] = cool;
            return true;
        }

    }
}
