using RotMG.Game.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.ItemEffs.RuneEffects
{
    class Brute : IItemHandler
    {
        int t = 0;
        public virtual void OnTick(Player host) 
        {
            t++;
            if (t % 8 != 0) return;

            var defOver50 = host.GetStatTotalNotTemporary(3) - 50;
            if(defOver50 > 0)
            {
                host.AddIdentifiedEffectBoost(new Player.BoostTimer
                {
                    amount = defOver50,
                    id = "BruteAtk".GetHashCode(),
                    index = 2,
                    timer = 1.0f
                });
                host.AddIdentifiedEffectBoost(new Player.BoostTimer
                {
                    amount = defOver50,
                    id = "BruteVit".GetHashCode(),
                    index = 6,
                    timer = 1.0f
                });
                host.AddIdentifiedEffectBoost(new Player.BoostTimer
                {
                    amount = -defOver50,
                    id = "BruteDef".GetHashCode(),
                    index = 3,
                    timer = 1.0f
                });
                host.UpdateStats();
            }
        }

    }
}
