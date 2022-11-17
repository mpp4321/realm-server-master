using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotMG.Game.Logic.Behaviors
{
    public class PredicateBehav : Conditional
    {
        private Func<Entity, bool> Predicate;

        public PredicateBehav(Func<Entity, bool> pred, params Behavior[] behaviors) : base(behaviors)
        {
            Predicate = pred;
        }

        public override bool ConditionMet(Entity host)
        {
            return Predicate(host);
        }
    }
}
