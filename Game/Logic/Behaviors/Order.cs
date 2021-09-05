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
                    State foundState = i.Behavior.States.Values.Where(
                        a => a.StringId.Equals(_targetStateName)
                    ).FirstOrDefault();
                    if(foundState != default(State))
                    {
                        i.CurrentStates.Clear();
                        i.CurrentStates.Add(
                            foundState
                        );
                    }
                }
            }
            return false;
        }
    }
}
