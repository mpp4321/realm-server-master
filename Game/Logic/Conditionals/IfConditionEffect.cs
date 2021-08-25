using RotMG.Common;

namespace RotMG.Game.Logic.Conditionals
{
    public class IfConditionEffect : Conditional
    {
        public readonly ConditionEffectIndex Effect;

        public IfConditionEffect(ConditionEffectIndex effect, params Behavior[] behaviors) : base(behaviors)
        {
            Effect = effect;
        }

        public override bool ConditionMet(Entity host)
        {
            return host.HasConditionEffect(Effect);
        }
    }
}
