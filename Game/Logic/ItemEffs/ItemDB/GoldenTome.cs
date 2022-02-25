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
    class GoldenTome : IItemHandler
    {

        public void OnTick(Player p)
        {
            foreach(Player pl in p.GetNearbyPlayers(1.5f))
            {
                if(pl.Id != p.Id)
                    pl?.ApplyConditionEffect(ConditionEffectIndex.Healing, 1000);
            }
        }
    }
}
