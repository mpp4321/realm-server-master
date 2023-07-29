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
                var scale = 0.001f + pl.GetStatTotal(6) * 0.0001f; 
                pl.Heal( (int)MathF.Ceiling((damageDone * scale)) , false, false);
            }
        }
    }
}
