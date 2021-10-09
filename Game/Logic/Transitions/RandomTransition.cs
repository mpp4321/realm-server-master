using RotMG.Utils;

namespace RotMG.Game.Logic.Behaviors
{
    class RandomTransition : Transition
    {

        public readonly Transition[] transitions;

        public RandomTransition(params Transition[] transitions) : base("")
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
            var trans = transitions[MathUtils.Next(transitions.Length)];
            var rt = trans.Tick(host);
            if(rt)
            {
                SubIndex = trans.SubIndex;
                TargetState = trans.TargetState;
            }
            return rt;
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
