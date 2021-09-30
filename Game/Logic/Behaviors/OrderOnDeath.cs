using RotMG.Common;
using RotMG.Utils;
using System.Collections.Generic;
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

        public static void ChangeStateTree(Entity i, string target)
        {
            if (!i.CurrentStates.Any(a => a.StringId.Equals(target)))
            {
                var s = i.Behavior.States.Values.Select(a => {
                    var tree = FindTransverseState(a, target) ?? new List<State>();
                    if (tree.Count == 0) return tree;
                    tree.Insert(0, a);
                    return tree;
                }).FirstOrDefault(a => (a?.Count != 0));

                if (s.Count > 0)
                {
                    i.ExitStates();
                    i.CurrentStates = s;
                    i.EnterStates();
                }
            }
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
        public static List<State> FindTransverseState(State s, string id, List<State> qu = null)
        {
            if (qu == null) qu = new List<State>();
            if(s.StringId.ToLower().Equals(id))
            {
                qu.Add(s);
                return qu;
            }
            foreach(var state in s.States.Values)
            {
                var v = FindTransverseState(state, id, qu);
                if (v != null)
                {
                    v.Add(state);
                    return v;
                }
            }
            return null;
        }

        public static List<State> FindTransverseState(State s, int id, List<State> qu = null)
        {
            if (qu == null) qu = new List<State>();
            if(s.Id == id)
            {
                qu.Add(s);
                return qu;
            }
            foreach(var state in s.States.Values)
            {
                var v = FindTransverseState(state, id, qu);
                if (v != null)
                {
                    v.Add(state);
                    return v;
                }
            }
            return null;
        }

        public static State FindNestedState(State s, int id)
        {
            if(s.Id == id)
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
                ChangeStateTree(a, _stateName);
            }
        }
    }
}
