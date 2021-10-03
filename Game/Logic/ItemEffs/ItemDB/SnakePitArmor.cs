using RotMG.Common;
using RotMG.Game.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.ItemEffs.ItemDB
{
    class SnakePitArmor : IItemHandler
    {
        public void OnAbilityUse(Vector2 position, ItemDesc desc, ItemDataJson itemdata, Player player)
        {
        }

        public void OnEnemyHit(Entity hit, Projectile by)
        {
        }

        public void OnHitByEnemy(Player hit, Entity hitBy, Projectile by)
        {
        }

        public void OnTick(Player p)
        {
            if(p.HasConditionEffect(ConditionEffectIndex.Invisible))
            {
                var defenseDecrease = p.GetStatTotal(3) / 2;

                p.EffectBoosts.Add(new Player.BoostTimer
                {
                    amount = defenseDecrease,
                    id = 1,
                    index = 6,
                    timer = 1000
                });

                p.EffectBoosts.Add(new Player.BoostTimer
                {
                    amount = -defenseDecrease,
                    id = 2,
                    index = 3,
                    timer = 1000
                });
            }
        }
    }
}
