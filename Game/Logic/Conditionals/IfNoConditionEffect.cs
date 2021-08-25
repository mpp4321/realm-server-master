using RotMG.Common;

namespace RotMG.Game.Logic.Conditionals
{
    public class IfNoConditionEffect : Conditional
    {
        public readonly ConditionEffectIndex Effect;

        public IfNoConditionEffect(ConditionEffectIndex effect, params Behavior[] behaviors) : base(behaviors)
        {
            Effect = effect;
        }

        public override bool ConditionMet(Entity host)
        {
            return !host.HasConditionEffect(Effect);
        }
    }
}
