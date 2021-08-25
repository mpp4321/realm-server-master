using RotMG.Common;
using RotMG.Utils;

namespace RotMG.Game.Logic.Behaviors
{
    public class WanderState
    {
        public Vector2 Direction;
        public float RemainingDistance;
    }

    public class Wander : Behavior
    {
        public readonly float Speed;

        public Wander(float speed)
        {
            Speed = speed;
        }

        public override void Enter(Entity host)
        {
            host.StateObject[Id] = new WanderState();
        }

        public override bool Tick(Entity host)
        {
            var state = host.StateObject[Id] as WanderState;

            if (host.HasConditionEffect(ConditionEffectIndex.Paralyzed))
                return false;

            if (state.RemainingDistance <= 0)
            {
                state.Direction = new Vector2(MathUtils.PlusMinus(), MathUtils.PlusMinus());
                state.Direction.Normalize();
                state.RemainingDistance = 600 / 1000f;
            }

            var dist = host.GetSpeed(Speed) * Settings.SecondsPerTick;
            host.ValidateAndMove(host.Position + state.Direction * dist);
            state.RemainingDistance -= dist;
            return true;
        }

        public override void Exit(Entity host)
        {
            host.StateObject[Id] = null;
        }

        public override void Death(Entity host) { }
    }
}
