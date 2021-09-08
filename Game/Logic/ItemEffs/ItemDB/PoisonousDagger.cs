using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Game.Logic.ItemEffs;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.ItemEffs.ItemDB
{
    class PoisonousDagger : IItemHandler
    {
        public void OnAbilityUse(Vector2 position, ItemDesc desc, ItemDataJson itemdata)
        {
        }

        public void OnEnemyHit(Entity hit, Projectile by)
        {
            if(MathUtils.NextFloat() < 0.105f)
            {
                if (!(hit is Enemy en) || !(by.Owner is Player pl))
                    return;
                en.ApplyPoison(pl, new ConditionEffectDesc[] { }, 500, 500);
            }
        }

        public void OnHitByEnemy(Player hitBy, Entity hit, Projectile by)
        {
        }
    }
}
