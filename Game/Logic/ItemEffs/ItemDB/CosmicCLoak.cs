using RotMG.Common;
using RotMG.Game.Entities;
using System;

namespace RotMG.Game.Logic.ItemEffs.ItemDB
{
    class CosmicCloak : IItemHandler
    {
        int t = 0;
        public void OnTick(Player p)
        {
            if (t++ % 8 != 0) return;

            p.MP -= (int)(p.GetMPRegen() * 16 * Settings.SecondsPerTick);
            p.MP = Math.Max(0, p.MP);
            var maxMana = p.GetStatTotal(1);
            var totalPossibleBoost = (int)(maxMana / 10);
            float percentManaLeft = p.MP / (float) maxMana;
            float attackBoost = 1f - percentManaLeft;
            attackBoost *= totalPossibleBoost;
            p.AddIdentifiedEffectBoost(new Player.BoostTimer()
            {
                amount = (int) attackBoost,
                id = "CosmicCloak".GetHashCode(),
                index = 2,
                timer = 2f,
            }, false);
            p.UpdateStats();
        }
    }
}
