using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.Behaviors
{
    class ChargeShoot : Behavior
    {

        private Shoot s;
        private Charge c;

        public ChargeShoot(Shoot s, Charge c)
        {
            this.s = s;
            this.c = c;
        }

        public override void Death(Entity host)
        {
            s?.Death(host);
            c?.Death(host);
        }

        public override void Enter(Entity host)
        {
            s.Enter(host);
            c.Enter(host);
        }

        public override void Exit(Entity host)
        {
            s?.Exit(host);
            c?.Exit(host);
        }

        public override bool Tick(Entity host)
        {
            var rt = c.Tick(host);
            var dir = (host.StateObject[c.Id] as Charge.ChargeState).Direction;
            if(rt)
            {
                s.FixedAngle = MathF.Atan2(dir.Y, dir.X);
                s.Tick(host);
            }
            return rt;
        }

    }
}
