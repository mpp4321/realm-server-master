using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Game.Logic.ItemEffs;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.ItemEffs.ItemDB
{
    class ObsidianPlatemail : IItemHandler
    {
        public void OnEnemyHit(Entity hit, Projectile by, ref int damageDone)
        {
            if(MathUtils.NextFloat() < 0.105f)
            {
                if (!(hit is Enemy en) || !(by.Owner is Player pl))
                    return;
                var damage = pl.GetStatTotal(3) * 10;
                en.ApplyPoison(pl, new ConditionEffectDesc[] { }, damage / 5, damage, 0xff000000);
            }
        }
    }
}
