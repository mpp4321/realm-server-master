using RotMG.Common;
using RotMG.Utils;
using System.Linq;

namespace RotMG.Game.Logic.Behaviors
{
    class OrderOnDeath : Behavior
    {
        private readonly float _range;
        private readonly ushort _target;
        private readonly string _stateName;
        private readonly float _probability;

        public OrderOnDeath(float range, string target, string state, double probability = 1)
        {
            _range = range;
            _target = Resources.Id2Object[target].Type;
            _stateName = state;
            _probability = (float) probability;
        }

        public static State FindNestedState(State s, string id)
        {
            if (s.StringId.Equals(id.ToLower()))
                return s;
            foreach(var state in s.States.Values)
            {
                var v = FindNestedState(state, id);
                if (v != null) return v;
            }
            return null;
        }

        public override void Death(Entity host)
        {
            foreach (var a in GameUtils.GetNearbyEntities(host, _range).Where(a => a.Type == _target))
            {
                var s = a.Behavior.States.Values.Select(a => FindNestedState(a, _stateName)).Where(a => a != null).FirstOrDefault();

                if (s != null)
                {
                    a.CurrentStates.Clear();
                    a.CurrentStates.Add(s);
                }
            }
        }
    }
}
