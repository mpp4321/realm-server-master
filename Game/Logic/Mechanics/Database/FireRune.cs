using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Game.Logic.ItemEffs;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.Mechanics.Database
{
    class FireRune : IItemHandler
    {
        public void OnAbilityUse(Vector2 position, ItemDesc desc, ItemDataJson itemdata, Player player)
        {
        }

        public void OnEnemyHit(Entity hit, Projectile by, ref int damageDone)
        {
            if(hit is Enemy en)
            {
                en.Death(by.Owner as Player);
            }
        }

        public void OnHitByEnemy(Player hit, Entity hitBy, Projectile by)
        {
        }

        public void OnTick(Player p)
        {
        }
    }
}
