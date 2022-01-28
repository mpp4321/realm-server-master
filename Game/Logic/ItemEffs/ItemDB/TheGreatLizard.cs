using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotMG.Game.Logic.ItemEffs.ItemDB
{
    public class TheGreatLizard : IItemHandler
    {
        public void OnEnemyHit(Entity hit, Projectile by, ref int damageDone) 
        {
            if(MathUtils.Chance(.1f))
            {
                hit.ApplyConditionEffect(Common.ConditionEffectIndex.ArmorBroken, 1000);
            }
        }

    }
}
