using RotMG.Common;
using RotMG.Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RotMG.Game.Logic.Mechanics
{
    class Mix
    {

        public static void DoMix(Player p, int slot1, int slot2)
        {
            var items = new int[] { p.Inventory[slot1], p.Inventory[slot2] };
            var descs = items.Select(a => Resources.Type2Item[(ushort)a]);
            var componentItem = descs.Where(a => a.Component != null).FirstOrDefault();

            if (componentItem == null) return;

            var itemCombined = descs.Where(a => a.Component == null).FirstOrDefault();

            if (itemCombined == null) return;

            var slotCombiningInto = p.Inventory[slot1] == itemCombined.Type ? slot1 : slot2;
            var slotConsuming = slotCombiningInto == slot1 ? slot2 : slot1;

            p.ItemDatas[slotCombiningInto].ItemComponent = componentItem.Component;
            p.Inventory[slotConsuming] = -1;

            p.SendInfo("Success!");
            p.UpdateInventory();
        }

    }
}
