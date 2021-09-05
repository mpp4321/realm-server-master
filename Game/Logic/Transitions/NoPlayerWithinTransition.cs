using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RotMG.Game.Logic.Transitions
{
    class NoPlayerWithinTransition : Transition
    {
        float dist;

        public NoPlayerWithinTransition(float dist, string targetState)
            : base(targetState)
        {
            this.dist = dist;
        }

        public override bool Tick(Entity host)
        {
            return GameUtils.GetNearestPlayer(host, dist) == null;
        }
    }
}
