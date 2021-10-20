using RotMG.Game.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.ItemEffs.ItemDB
{
    class CryptRing : IItemHandler
    {

        static int t = 0;
        public void OnTick(Player p)
        {
            t++;
            if (t % 8 != 0) return;
            if(p.GetStatTotal(7) > 80)
            {
                var wisIncrease = (p.GetStatTotal(7) - 80) / 2;

                p.AddIdentifiedEffectBoost(new Player.BoostTimer
                {
                    amount = wisIncrease,
                    id = "CryptRing".GetHashCode(),
                    index = 7,
                    timer = 1.0f
                });

                p.UpdateStats();
            }
        }

    }
}
