namespace RotMG.Game.Logic.Behaviors
{
    public class Prioritize : Behavior
    {
        public readonly Behavior[] Behaviors;
        public readonly bool negate = false;

        public Prioritize(params Behavior[] behaviors)
        {
            Behaviors = behaviors;
        }

        public Prioritize(bool negate, params Behavior[] behaviors)
        {
            Behaviors = behaviors;
            this.negate = negate;
        }

        public override void Enter(Entity host)
        {
            for (var k = 0; k < Behaviors.Length; k++)
                Behaviors[k].Enter(host);
        }

        public override bool Tick(Entity host)
        {
            for (var k = 0; k < Behaviors.Length; k++)
                if (Behaviors[k].Tick(host) != negate)
                    return !negate;
            return !negate;
        }

        public override void Exit(Entity host)
        {
            for (var k = 0; k < Behaviors.Length; k++)
                Behaviors[k].Exit(host);
        }
    }
}
