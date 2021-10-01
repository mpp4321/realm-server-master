using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RotMG.Game.Logic.Transitions
{
    class TransitionFrom : Transition
    {
        private readonly string IdFrom;

        public TransitionFrom(string idFrom, string idTo) : base(idTo)
        {
            IdFrom = idFrom.ToLower();
            SubIndex = 0;
        }

        public override bool Tick(Entity host)
        {
            var lastState = host.CurrentStates.Last();
            if (lastState == null) return false;
            return lastState.StringId.Equals(IdFrom);
        }

    }
}
