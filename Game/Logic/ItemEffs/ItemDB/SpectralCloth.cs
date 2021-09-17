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
    class SpectralCloth : IItemHandler
    {
        public void OnAbilityUse(Vector2 position, ItemDesc desc, ItemDataJson itemdata)
        {
        }

        public void OnEnemyHit(Entity hit, Projectile by)
        {
        }

        public void OnHitByEnemy(Player hitBy, Entity hit, Projectile by)
        {
            if(MathUtils.NextFloat() < (0.105f - 0.03f*hitBy.EffectBoosts.Count))
            {
                hitBy.Parent.BroadcastPacketNearby(GameServer.Notification
                (
                    hitBy.Id,
                    "Wis Boost!",
                    0xffAEC6CF
                ), hitBy.Position);

                hitBy.EffectBoosts.Add(new Player.BoostTimer()
                {
                    amount = 12,
                    timer = 5.0f,
                    index = 7
                });
                hitBy.UpdateStats();
            }
        }
    }
}
