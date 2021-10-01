using RotMG.Common;
using RotMG.Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static RotMG.Game.Logic.LootDef;

namespace RotMG.Game.Logic.Mechanics
{
    class Mix
    {

        public static bool AreAnyComponents(IEnumerable<ItemDesc> items)
        {
            return items.Any(a => a.Component != null);
        }

        public static (int, int) FindComponentItem(Player p, int slot1, int slot2, int id)
        {
            return p.Inventory[slot1] == id ? (slot1, slot2) : p.Inventory[slot2] == id ? (slot2, slot1) : (-1, -1);
        }

        public static void DoMixComponents(Player p, int slot1, int slot2, IEnumerable<ItemDesc> descs)
        {
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

        public static void DoMix(Player p, int slot1, int slot2)
        {
            var items = new int[] { p.Inventory[slot1], p.Inventory[slot2] };
            var descs = items.Select(a => Resources.Type2Item[(ushort)a]);

            if(AreAnyComponents(descs))
            {
                DoMixComponents(p, slot1, slot2, descs);
                return;
            } else
            {
                //Crafting
                (int, int) goo = FindComponentItem(p, slot1, slot2, 0xcb0);
                if(goo.Item1 != -1 && goo.Item2 != -1)
                {
                    var item = Resources.Type2Item[(ushort) p.Inventory[goo.Item2]];
                    ItemDataModType vtype = ItemDataModType.Classical;
                    Enum.TryParse(p.Client.Character.ItemDataModifier, out vtype);
                    var r = item.Roll(new RarityModifiedData(1.0f, 3, true), vtype);

                    p.Inventory[goo.Item1] = -1;
                    p.ItemDatas[goo.Item1] = new ItemDataJson();

                    p.ItemDatas[goo.Item2] = r.Item1 ? r.Item2 : new ItemDataJson();
                }

                p.UpdateInventory();
                return;
            }
        }

    }
}
