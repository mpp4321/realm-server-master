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

        public void OnEnemyHit(Entity hit, Projectile by, ref int damageDone)
        {
        }

        public void OnHitByEnemy(Player hit, Entity hitBy, Projectile by)
        {
            if(MathUtils.Next(5) == 0)
            {
                hit.Parent?.BroadcastPacketNearby(GameServer.Notification
                (
                    hitBy.Id,
                    "Retaliaton!",
                    0xff22ff22
                ), hitBy.Position);
                var protTotal = hit.GetStatTotal(8);
                var damage = by.Damage * (10.0 * (protTotal / 100.0));
                (hitBy as Enemy)?.Damage(hit, (int)damage, new ConditionEffectDesc[0], true, true);
            }
        }

        public void OnTick(Player p)
        {
        }
    }
}
