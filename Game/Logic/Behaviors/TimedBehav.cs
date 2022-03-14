using RotMG.Common;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wServer.logic;

namespace RotMG.Game.Logic.Behaviors
{
    public class TimedBehav : Behavior
    {

        private int Offset { get; set; }
        private Behavior[] Behavior { get; set; }

        public TimedBehav(int offset, params Behavior[] behavs)
        {
            Offset = offset;
            Behavior = behavs;
        }


        public override void Enter(Entity host)
        {
            host.StateCooldown[Id] = Offset;
            foreach(var b in Behavior)
                b.Enter(host);
        }

        public override bool Tick(Entity host)
        {
            if(host.StateCooldown[Id] <= 0)
            {
                foreach (var b in Behavior)
                    b?.Tick(host);
                return true;
            }
            host.StateCooldown[Id] -= Settings.MillisecondsPerTick;
            return false;
        }

        public override void Exit(Entity host)
        {
            host.StateCooldown.Remove(Id);
            foreach(var b in Behavior)
                b?.Exit(host);
        }

    }
}
