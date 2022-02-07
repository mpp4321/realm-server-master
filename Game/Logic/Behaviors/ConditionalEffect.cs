using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RotMG.Common;

namespace RotMG.Game.Logic.Behaviors
{
    class ConditionalEffect : Behavior
    {
        ConditionEffectIndex effect;
        bool perm;
        int duration;

        public ConditionalEffect(ConditionEffectIndex effect, bool perm = false, int duration = -1)
        {
            this.effect = effect;
            this.perm = perm;
            this.duration = duration;
        }

        public override void Enter(Entity host)
        {
            host.ApplyConditionEffect(
                effect,
                duration
            );
        }

        public override void Exit(Entity host)
        {
            if (!perm)
            {
                host.ApplyConditionEffect(effect, 0);
            }
        }

    }
}
