using RotMG.Game.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.ItemEffs.ItemDB
{
    class AmuletOfBackwardsLuck : IItemHandler
    {
        public void ModifyDrop(Player p, LootDef def, ref Dictionary<Player, int> thresholds, ref float dropMod)
        {
            if(thresholds.Count < 4)
            {
                dropMod *= 1.2f;
            }
            else
            {
                dropMod *= .8f;
            }
        }
    }
}
