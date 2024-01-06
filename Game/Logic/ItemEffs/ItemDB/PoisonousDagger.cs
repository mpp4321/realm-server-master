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
        public void OnAbilityUse(Vector2 position, ItemDesc desc, ItemDataJson itemdata, Player player)
        {
        }

        public void OnEnemyHit(Entity hit, Projectile by, ref int damageDone)
        {
            if(MathUtils.NextFloat() < 0.15f)
            {
                if (!(hit is Enemy en) || !(by.Owner is Player pl))
                    return;
                en.ApplyPoison(pl, new ConditionEffectDesc[] { }, 100, 300);
            }
        }

        public void OnHitByEnemy(Player hitBy, Entity hit, Projectile by)
        {
        }

        public void OnTick(Player p)
        {
        }
    }
}
