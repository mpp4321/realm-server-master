using RotMG.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotMG.Game.Logic.Behaviors
{
    public class HealthThreshOrder : Behavior
    {
        //State storage: none

        private readonly float _range;
        private readonly float _threshold;
        private readonly ushort _children;
        private readonly string _targetStateName;

        public HealthThreshOrder(float range, string children, float thresh, string targetState)
        {
            _range = range;
            _children = Resources.Id2Object[children].Type;
            _targetStateName = targetState.ToLower();
            _threshold = thresh;
        }

        public override bool Tick(Entity host)
        {
            var hpp = host.GetHealthPercentage();
            if (hpp > _threshold)
                return false;

            if (host.Parent == null) return false;
            var state = false;
            foreach(var i in host.Parent.EntityChunks.HitTest(host.Position, _range).Where(z => z.GetObjectDefinition().ObjectType == _children))
            {
                OrderOnDeath.ChangeStateTree(i, _targetStateName);
                state = true;
            }
            return state;
        }
    }
}
