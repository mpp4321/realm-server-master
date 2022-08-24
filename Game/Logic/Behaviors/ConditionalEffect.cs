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
        Predicate<Entity> reapply;

        public ConditionalEffect(ConditionEffectIndex effect, bool perm = false, int duration = -1, Predicate<Entity> reapply=null)
        {
            this.effect = effect;
            this.perm = perm;
            this.duration = duration;
            this.reapply = reapply;
        }

        public override void Enter(Entity host)
        {
            host.ApplyConditionEffect(
                effect,
                duration
            );
        }

        public override bool Tick(Entity host)
        {
            if (reapply != null)
            {
                if(reapply(host))
                {
                    host.ApplyConditionEffect(effect, duration);
                }
            }
            return base.Tick(host);
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
