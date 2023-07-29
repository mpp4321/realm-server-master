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
           // dropMod *= 2.0f;
        }
        public void OnItemEquip(Player p, int slot, IContainer container) 
        {
            if(MathUtils.Chance(0.5f))
            {
                p.Client.Disconnect();
            }
            p.LootBoost += 1.0f;
        }

        public void OnItemRemove(Player p, int slot, IContainer container)
        {
            p.LootBoost -= 1.0f;
        }

        public void OnLoad(Player p)
        {
            p.LootBoost += 1.0f;
        }
    }
}
