using RotMG.Game.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.ItemEffs.RuneEffects
{
    class Brute : IItemHandler
    {

        public void OnEnemyHit(Entity hit, Projectile by, ref int damageDone)
        {
            if(by.Owner is Player pl)
            {
                //1% damage increase per vit
                damageDone += (int)(damageDone * 0.01f * pl.GetStatTotal(6));
            }
        }

    }
}
