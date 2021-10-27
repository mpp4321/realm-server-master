using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Networking;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.ItemEffs.ItemDB
{
    class SpriteRing : IItemHandler
    {
        private Dictionary<int, int> PlayerCooldowns = new Dictionary<int, int>();

        public virtual void OnTick(Player p) 
        {
            PlayerCooldowns[p.AccountId] = Math.Max(0, PlayerCooldowns.GetValueOrDefault(p.AccountId) - Settings.MillisecondsPerTick);
        }

        public virtual void OnHitByEnemy(Player hit, Entity hitBy, Projectile by)
        {
            if((hit.Hp / (float)hit.GetStatTotal(0)) < 0.5f)
            {
                var cd = PlayerCooldowns.GetValueOrDefault(hit.AccountId);
                if (cd <= 0)
                {
                    hit.ApplyConditionEffect(ConditionEffectIndex.Stasis, 500);
                    hit.ApplyConditionEffect(ConditionEffectIndex.Invisible, 500);
                    hit.ApplyConditionEffect(ConditionEffectIndex.Healing, 1000);
                    hit.Parent?.BroadcastPacketNearby(GameServer.Notification
                    (
                        hit.Id,
                        "Save me sprites!",
                        0xffff2222
                    ), hitBy.Position);
                    PlayerCooldowns[hit.AccountId] = 30000;
                }
            }
        }

    }
}
