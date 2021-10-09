namespace RotMG.Game.Logic.Behaviors
{
    class RandomTransition : Transition
    {

        public readonly Transition[] transitions;

        public RandomTransition(params Transition[] transitions) : base("")
        {
            this.transitions = transitions;
        }

    }
}
