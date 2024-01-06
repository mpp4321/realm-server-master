using RotMG.Game.Entities;
using System;

namespace RotMG.Game.Logic.ItemEffs.RuneEffects
{
    class Manasteal : IItemHandler
    {
        public void OnEnemyHit(Entity hit, Projectile by, ref int damageDone) 
        {
            if(by.Owner is Player pl)
            {
                if (pl.HasConditionEffect(Common.ConditionEffectIndex.Quiet))
                    return;
                var scale = pl.GetStatTotal(7) * 0.0001f + 0.001f;
                pl.Heal((int)MathF.Ceiling(damageDone * scale), true, false);
            }
        }
    }
}
