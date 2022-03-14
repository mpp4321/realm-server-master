using RotMG.Game.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.ItemEffs.ItemDB
{
    class SappingShot : IItemHandler
    {
        public void OnProjectileShoot(Player shotFrom, ref Projectile projectile)
        {
            shotFrom.Heal(-10, false, false);
        }
    }
}
