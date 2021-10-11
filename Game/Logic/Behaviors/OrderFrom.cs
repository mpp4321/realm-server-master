using RotMG.Common;
using RotMG.Utils;
using System.Collections.Generic;
using System.Linq;

namespace RotMG.Game.Logic.Behaviors
{
    class OrderFrom : Behavior
    {
        //State storage: none

        private readonly float _range;
        private readonly ushort _children;
        private readonly string _initialState;
        private readonly string _targetStateName;

        public OrderFrom(float range, string children, string initialState, string targetState)
        {
            _range = range;
            _children = Resources.Id2Object[children].Type;
            _targetStateName = targetState.ToLower();
            _initialState = initialState.ToLower();
        }

        public override bool Tick(Entity host)
        {
            foreach(var i in host.Parent.EntityChunks.HitTest(host.Position, _range).Where(z => z.GetObjectDefinition().ObjectType == _children))
            {
                var lastState = i.CurrentStates.Last();
                if (lastState == null) return false;
                if(lastState.StringId.Equals(_initialState))
                {
                    OrderOnDeath.ChangeStateTree(i, _targetStateName);
                    return true;
                }
            }
            return false;
        }
    }
}
