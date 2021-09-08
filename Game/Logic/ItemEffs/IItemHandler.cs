using RotMG.Common;
using RotMG.Game.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.ItemEffs
{
    interface IItemHandler
    {

        abstract void OnEnemyHit(Entity hit, Projectile by);

        abstract void OnHitByEnemy(Player hit, Entity hitBy, Projectile by);

        abstract void OnAbilityUse(Vector2 position, ItemDesc desc, ItemDataJson itemdata);

    }
}
