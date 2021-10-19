using RotMG.Game.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.ItemEffs.ItemDB
{
    class WillOfDen : IItemHandler
    {
        static int t = 0;
        public void OnTick(Player p)
        {
            t++;
            if (t % 4 != 0) return;
            if(p.GetStatTotal(0) == p.Hp)
            {
                p.AddIdentifiedEffectBoost(new Player.BoostTimer
                {
                    amount = 15,
                    id = "WillOfDen".GetHashCode(),
                    index = 5,
                    timer = 1.0f
                });

                p.UpdateStats();
            }
        }
    }
}
