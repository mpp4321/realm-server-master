using RotMG.Common;
using System;
using System.Collections.Generic;
using System.Text;
using wServer.logic;

namespace RotMG.Game.Logic.Behaviors
{
    class PulseFire : Behavior
    {
        private readonly Func<Entity, bool> delg;
        private readonly Cooldown cooldown;

        public PulseFire(Func<Entity, bool> delg, Cooldown cooldown) : base()
        {

            this.cooldown = cooldown;
            this.delg = delg;

        }

        private static Random Random = new Random();

        public override void Enter(Entity host)
        {
            host.StateObject[Id] = cooldown.Next(Random);
        }

        public override bool Tick(Entity host)
        {
            var returnState = false;
            int cd = (int) host.StateObject[Id];

            if(cd <= 0)
            {
                returnState = delg(host);
                cd = cooldown.Next(Random);
            } else
            {
                cd -= Settings.MillisecondsPerTick;
            }

            host.StateObject[Id] = cd;
            return returnState;
        }


    }
}
