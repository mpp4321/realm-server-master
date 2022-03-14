﻿using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Utils;

namespace RotMG.Game.Logic.ItemEffs.ItemDB
{
    class ShaitTome : IItemHandler
    {

        public void OnTick(Player p)
        {
            foreach(Player pl in p.GetNearbyPlayers(1.5f))
            {
                if(pl.Id != p.Id)
                    pl?.ApplyConditionEffect(ConditionEffectIndex.Damaging, 1000);
            }
        }
    }
}
