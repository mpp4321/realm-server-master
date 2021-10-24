using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.Transitions
{
    class NotMovingTransition : Transition
    {

        public NotMovingTransition(string targetState) : base(targetState)
        {
        }

        public override bool Tick(Entity host)
        {
            return host.HasConditionEffect(Common.ConditionEffectIndex.Paralyzed);
        }

    }
}
