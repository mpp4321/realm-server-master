using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Networking;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.ItemEffs.ItemDB
{
    class OphGem : IItemHandler
    {
        public void OnAbilityUse(Vector2 position, ItemDesc desc, ItemDataJson itemdata, Player player)
        {
        }

        public void OnEnemyHit(Entity hit, Projectile by)
        {
        }

        public void OnHitByEnemy(Player hit, Entity hitBy, Projectile by)
        {
            if(MathUtils.Next(10) == 0)
            {
                hit.Parent?.BroadcastPacketNearby(GameServer.Notification
                (
                    hitBy.Id,
                    "Retaliaton!",
                    0xff22ff22
                ), hitBy.Position);
                (hitBy as Enemy)?.Damage(hit, by.Damage, new ConditionEffectDesc[0], true, true);
            }
        }

        public void OnTick(Player p)
        {
        }
    }
}
