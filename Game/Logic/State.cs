using RotMG.Game.Logic.Transitions;
using System;
using System.Collections.Generic;

namespace RotMG.Game.Logic
{
    public class State : IBehavior
    {
        public string StringId; //Only used for parsing.
        public int Id;

        public State Parent;
        public List<Behavior> Behaviors;
        public List<Transition> Transitions;
        public Dictionary<int, State> States;

        public State(string id, params IBehavior[] behaviors)
        {
            StringId = id.ToLower();
            Id = ++BehaviorDb.NextId;

            Behaviors = new List<Behavior>();
            Transitions = new List<Transition>();
            States = new Dictionary<int, State>();

            foreach (var bh in behaviors)
            {
#if DEBUG
                if (bh is Loot) throw new Exception("Loot should not be initialized in a substate.");
#endif
                if (bh is Behavior) Behaviors.Add(bh as Behavior);
                if (bh is Transition) Transitions.Add(bh as Transition);
                if (bh is State)
                {
                    var state = bh as State;
                    state.Parent = this;
                    States.Add(state.Id, state);
                }
            }
        }

        State FindNthParentState(State s, int n)
        {
            if (n == 0) return s;
            return FindNthParentState(s.Parent, n - 1);
        }

        public void FindStateTransitions(IEnumerable<State> firstLayer)
        {
            foreach (var transition in Transitions)
            {
                var iterState = FindNthParentState(this, transition.SubIndex);;
                foreach (var state in iterState?.States.Values ?? firstLayer)
                {
                    if (state.StringId == transition.StringTargetState)
                        transition.TargetState = state.Id;
                }
            }

            foreach (var state in States.Values)
                state.FindStateTransitions(firstLayer);
        }
    }
}
