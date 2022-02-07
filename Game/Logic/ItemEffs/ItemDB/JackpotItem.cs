using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.ItemEffs.ItemDB
{
    class JackpotItem : IItemHandler
    {
        public void ModifyDrop(Player p, LootDef def, ref Dictionary<Player, int> thresholds, ref float dropMod)
        {
            dropMod *= 2.0f;
        }
        public void OnItemEquip(Player p, int slot) 
        {
            //Ring slot obvs we are equipping jackpot
            if(slot == 3)
            {
                if(MathUtils.Chance(0.5f))
                {
                    p.Client.Disconnect();
                }
            }
        }
    }
}
