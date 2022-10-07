using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Networking;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.ItemEffs.ItemDB
{
    class ArachGar : IItemHandler
    {

        private Dictionary<int, int> PlayerCooldowns = new Dictionary<int, int>();
        private Dictionary<int, int> HealCooldowns = new Dictionary<int, int>();

        public virtual void OnTick(Player p) 
        {
            PlayerCooldowns[p.AccountId] = Math.Max(0, PlayerCooldowns.GetValueOrDefault(p.AccountId) - Settings.MillisecondsPerTick);
            HealCooldowns[p.AccountId] = Math.Max(0, HealCooldowns.GetValueOrDefault(p.AccountId) - Settings.MillisecondsPerTick);
        }

        public void OnHitByEnemy(Player hit, Entity hitBy, Projectile by)
        {
            var pcd = PlayerCooldowns.GetValueOrDefault(hit.AccountId);
            var hcd = HealCooldowns.GetValueOrDefault(hit.AccountId);

            if(by.Damage > 50 && pcd == 0)
            {
                hit.Parent?.BroadcastPacketNearby(GameServer.Notification
                (
                    hit.Id,
                    "Web!",
                    0xffff2222
                ), hitBy.Position);
                var nearbyEntities = GameUtils.GetNearbyEntities(hit, 3f);
                foreach(var en in nearbyEntities)
                {
                    en.ApplyConditionEffect(ConditionEffectIndex.Slowed, 2000);
                }
                var aoe = GameServer.ShowEffect(ShowEffectIndex.Nova, hit.Id, 0xff2222, new Vector2(3f, 0));
                hit.Parent?.BroadcastPacketNearby(aoe, hit.Position);
                PlayerCooldowns[hit.AccountId] = 5000;
            }

            if(by.Damage > 150 && hcd == 0)
            {
                hit.Parent?.BroadcastPacketNearby(GameServer.Notification
                (
                    hit.Id,
                    "Shed!",
                    0xffff0000
                ), hitBy.Position);

                hit.Heal(hit.MaxHp / 10, false, true);

                HealCooldowns[hit.AccountId] = 7500;
            }
        }

    }
}
