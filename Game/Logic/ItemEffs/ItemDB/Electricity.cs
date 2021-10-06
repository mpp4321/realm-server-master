using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.ItemEffs.ItemDB
{
    class Electricity : IItemHandler
    {
        public void OnAbilityUse(Vector2 position, ItemDesc desc, ItemDataJson itemdata, Player player)
        {
        }

        public void OnEnemyHit(Entity hit, Projectile by, ref int damageDone)
        {
            if (MathUtils.NextInt(0, 10) != 9)
                return;
            var fire = new Enemy(Resources.Id2Object["Thunder Cloud"].Type);
            fire.PlayerOwner = by.Owner as Player;
            var spawnPoint = GameUtils.RandomPointWithin(hit.Position, 2);
            hit.Parent.AddEntity(fire, spawnPoint);
        }

        public void OnHitByEnemy(Player hit, Entity hitBy, Projectile by)
        {
        }

        public void OnTick(Player p)
        {
        }
    }
}
