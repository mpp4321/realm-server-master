using RotMG.Common;
using RotMG.Utils;
using System.Linq;

namespace RotMG.Game.Logic.Behaviors
{
    class Order : Behavior
    {
        //State storage: none

        private readonly float _range;
        private readonly ushort _children;
        private readonly string _targetStateName;

        public Order(float range, string children, string targetState)
        {
            _range = range;
            _children = Resources.Id2Object[children].Type;
            _targetStateName = targetState;
        }

        public override bool Tick(Entity host)
        {
            foreach(var i in host.Parent.EntityChunks.HitTest(host.Position, _range).Where(z => z.GetObjectDefinition().ObjectType == _children))
            {
                if(!i.CurrentStates.Any(a => a.StringId.Equals(_targetStateName)))
                {
                    var s = i.Behavior.States.Values.Select(a => OrderOnDeath.FindNestedState(a, _targetStateName)).FirstOrDefault(a => a != null);

                    if (s != null)
                    {
                        i.CurrentStates.Clear();
                        i.CurrentStates.Add(s);
                    }
                }
            }
            return false;
        }
    }
}
