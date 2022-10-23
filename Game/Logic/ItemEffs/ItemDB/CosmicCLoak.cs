using RotMG.Common;
using RotMG.Game.Entities;
using System;

namespace RotMG.Game.Logic.ItemEffs.ItemDB
{
    class CosmicCloak : IItemHandler
    {
        public void OnTick(Player p)
        {
            p.MP -= (int)(p.GetMPRegen() * 2 * Settings.SecondsPerTick);
            p.MP = Math.Max(0, p.MP);
            float percentManaLeft = p.MP / (float) p.GetStatTotal(1);
            float attackBoost = 1 - percentManaLeft;
            attackBoost *= 80;
            p.AddIdentifiedEffectBoost(new Player.BoostTimer()
            {
                amount = (int)attackBoost,
                id = "CosmicCloak".GetHashCode(),
                index = 2,
                timer = 200,
            }, false);
            p.UpdateStats();
        }
    }
}
