using RotMG.Utils;
using System.Collections.Generic;
using System.Linq;

namespace RotMG.Game.Logic.Behaviors
{
    class RandomTransition : Transition
    {

        public readonly Transition[] transitions;

        public RandomTransition(params Transition[] transitions) :
            base(transitions.Select(a => a.TargetStates.Keys).SelectMany(a => a).ToArray())
        {
            this.transitions = transitions;
        }

        public override void Enter(Entity host)
        {
            foreach(var t in transitions)
            {
                t.Enter(host);
            }
        }

        public override bool Tick(Entity host)
        {
            var choices = new List<Transition>();
            foreach(var t in transitions)
            {
                var rt = t.Tick(host);
                if (rt) choices.Add(t);
            }
            if (choices.Count == 0) return false;
            var trans = choices[MathUtils.Next(transitions.Length)];
            CurrentState = trans.TargetStates.Keys.First().ToLower();
            return true;
        }

        public override void Exit(Entity host) 
        {
            foreach(var t in transitions)
            {
                t.Exit(host);
            }
        }

    }
}
