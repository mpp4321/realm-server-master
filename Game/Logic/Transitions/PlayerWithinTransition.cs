using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RotMG.Game.Logic.Transitions
{
    class PlayerWithinTransition : Transition
    {
        private readonly float _dist;

        public PlayerWithinTransition(float dist, string targetState, bool seeInvis = false)
            : base(targetState)
        {
            _dist = dist;
        }

        public override bool Tick(Entity host)
        {
            return GameUtils.GetNearestPlayer(host, _dist) != null;
        }
    }
}
