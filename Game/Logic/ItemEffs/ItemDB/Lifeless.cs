using RotMG.Common;
using RotMG.Game.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.ItemEffs.ItemDB
{
    class Lifeless : IItemHandler
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

        static int t = 0;
        public void OnTick(Player p)
        {
            t++;
            if (t % 8 != 0) return;
            if(p.GetStatTotal(6) < 20)
            {
                p.AddIdentifiedEffectBoost(new Player.BoostTimer
                {
                    amount = 800,
                    id = "Lifeless".GetHashCode(),
                    index = 0,
                    timer = 1.0f
                });
                p.UpdateStats();
            }
        }
    }
}
