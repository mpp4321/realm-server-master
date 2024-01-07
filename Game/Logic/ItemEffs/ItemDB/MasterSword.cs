using RotMG.Common;
using RotMG.Game.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.ItemEffs.ItemDB
{
    class MasterSword : IItemHandler
    {

        public void OnEnemyHit(Entity hit, Projectile by, ref int damageDone)
        {
            var p = by.Owner as Player;
            if (p == null) return;

            var maxHp = p.SVs.GetValueOrDefault(StatType.MaxHp, 0);

            if(p.Hp == (int)maxHp)
            {
                damageDone = (int)(damageDone * 1.2);
            }
        }

        public void OnTick(Player p)
        {
        }
    }
}
