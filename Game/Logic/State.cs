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

        public void FindStateTransitions()
        {
            foreach (var transition in Transitions)
                foreach (var state in Parent.States.Values)
                    if (state.StringId == transition.StringTargetState)
                        transition.TargetState = state.Id;

            foreach (var state in States.Values)
                state.FindStateTransitions();
        }
    }
}
