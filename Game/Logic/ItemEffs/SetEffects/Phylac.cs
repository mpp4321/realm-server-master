using RotMG.Game.Entities;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotMG.Game.Logic.ItemEffs.SetEffects
{
    public class Phylac : IItemHandler
    {
        public void OnEnemyHit(Entity hit, Projectile by, ref int damageDone)
        {
            if(by.Owner is Player pl)
            {
                if(MathUtils.Chance(0.01f))
                {
                    pl.Heal((int)(damageDone * 0.5f), false, false);
                }
            }
        }


    }
}
