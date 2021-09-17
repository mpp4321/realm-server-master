using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Game.Logic.ItemEffs;
using RotMG.Networking;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RotMG.Game.Logic.ItemEffs.ItemDB
{
    class Crumbling : IItemHandler
    {
        public void OnAbilityUse(Vector2 position, ItemDesc desc, ItemDataJson itemdata, Player player)
        {
        }

        public void OnEnemyHit(Entity hit, Projectile by)
        {
        }

        public void OnHitByEnemy(Player hitBy, Entity hit, Projectile by)
        {

            float chance = 0.105f;

            if (hitBy.Stats[3] + hitBy.Boosts[3] + hitBy.GetTemporaryStatBoost(3) > by.Damage) chance = 0.03f;

            if(MathUtils.NextFloat() < chance)
            {
                hitBy.Parent.BroadcastPacketNearby(GameServer.Notification
                (
                    hitBy.Id,
                    "Crumble",
                    0xffAEC6CF
                ), hitBy.Position);

                hitBy.EffectBoosts.Add(new Player.BoostTimer()
                {
                    amount = -10,
                    timer = 5.0f,
                    index = 3
                });

                hitBy.UpdateStats();
            }
        }

        public void OnTick(Player p)
        {
        }
    }
}
