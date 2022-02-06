using RotMG.Game.Entities;

namespace RotMG.Game.Logic.ItemEffs.RuneEffects
{
    class Elven : IItemHandler
    {
        public void OnEnemyHit(Entity hit, Projectile by, ref int damageDone) 
        {
            if(by.Owner is Player pl)
            {
                if (pl.HasConditionEffect(Common.ConditionEffectIndex.Quiet))
                    return;
                pl.Heal((int)(damageDone * 0.01), true, false);
            }
        }
    }
}
