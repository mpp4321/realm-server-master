using RotMG.Common;
using RotMG.Game.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.ItemEffs.ItemDB
{
    class UniversalPower : IItemHandler
    {
        public void OnAbilityUse(Vector2 position, ItemDesc desc, ItemDataJson itemdata, Player player)
        {
        }

        public void OnEnemyHit(Entity hit, Projectile by, ref int damageDone)
        {
        }

        public void OnHitByEnemy(Player hit, Entity hitBy, Projectile by)
        {
        }

        static int rateLimit = 0;

        public void OnTick(Player p)
        {
            rateLimit = (rateLimit + 1) % 10;
            if(rateLimit != 0)
            {
                return;
            }

            var weakestStat = Int32.MaxValue;
            for(var i = 3; i < 8; i++)
            {
                if(p.Stats[i] < weakestStat)
                {
                    weakestStat = p.GetStatTotal(i);
                }
            }
            p.AddIdentifiedEffectBoost(new Player.BoostTimer
            {
                amount = weakestStat/6,
                id = 1,
                index = 2,
                timer = 1.0f
            });
            p.UpdateStats();
        }
    }
}
