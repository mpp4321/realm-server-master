using RotMG.Game.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.ItemEffs.RuneEffects
{
    class Vampirism : IItemHandler
    {
        public void OnEnemyHit(Entity hit, Projectile by, ref int damageDone)
        {
            if(by.Owner is Player pl)
            {
                pl.Heal((int)MathF.Ceiling((damageDone * 0.005f)), false, false);
            }
        }
    }
}
