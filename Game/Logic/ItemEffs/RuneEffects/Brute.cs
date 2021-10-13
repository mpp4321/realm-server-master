using RotMG.Game.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.ItemEffs.RuneEffects
{
    class Brute : IItemHandler
    {

        public virtual void OnProjectileShoot(Player shotFrom, ref Projectile projectile) {
            projectile.Damage += (int)(projectile.Damage * 0.01f * shotFrom.GetStatTotal(6));
        }

    }
}
