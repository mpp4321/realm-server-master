using RotMG.Common;
using RotMG.Game.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.ItemEffs.ItemDB
{
    class ElectricSnake : IItemHandler
    {
        public void OnAbilityUse(Vector2 position, ItemDesc desc, ItemDataJson itemdata, Player player)
        {
            Enemy e = new Enemy(0x5008);
            player.Parent.AddEntity(e, player.Position);
            e.PlayerOwner = player;
        }

        public void OnEnemyHit(Entity hit, Projectile by)
        {
        }

        public void OnHitByEnemy(Player hit, Entity hitBy, Projectile by)
        {
        }

        public void OnTick(Player p)
        {
        }
    }
}
