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
        public static (int, int) FindComponentItem(int slot1, int slot2, ItemDesc s1, ItemDesc s2, Func<ItemDesc, bool> pred)
        {
            return pred(s1) ? (slot1, slot2) : pred(s2) ? (slot2, slot1) : (-1, -1);
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
            var descs = items.Select(a => Resources.Type2Item[(ushort)a]).ToList();

            if(AreAnyComponents(descs))
            {
                DoMixComponents(p, slot1, slot2, descs);
                return;
            } else
            {
                //Crafting
                (int, int) goo = FindComponentItem(slot1, slot2, descs[0], descs[1], x =>
                {
                    return x.ActivateEffects.Count() > 0 && x.ActivateEffects[0].Index == ActivateEffectIndex.MagicCrystal;
                });
                (int, int) havocPiece = FindComponentItem(p, slot1, slot2, 0xcaa);

                CombineAndReroll(p, goo);
                CombineAndTransform(p, havocPiece);

                p.UpdateInventory();
                return;
            }
        }

        private static void CombineAndReroll(Player p, (int, int) itemPair)
        {
            if (itemPair.Item1 == -1 || itemPair.Item2 == -1) return;
            var crystal = Resources.Type2Item[(ushort)p.Inventory[itemPair.Item1]];
            var eff = crystal.ActivateEffects[0];
            int power = eff.Amount;
            float scale = eff.StatScale;
            ItemDataModType typeOfMod = Enum.Parse<ItemDataModType>(crystal.ActivateEffects[0].Id ?? p.Client.Character.ItemDataModifier);
            var item = Resources.Type2Item[(ushort)p.Inventory[itemPair.Item2]];
            var r = item.Roll(new RarityModifiedData(scale, power, true), typeOfMod);

            p.Inventory[itemPair.Item1] = -1;
            p.ItemDatas[itemPair.Item1] = new ItemDataJson();

            p.ItemDatas[itemPair.Item2] = r.Item1 ? r.Item2 : new ItemDataJson();
        }

        private static Dictionary<ushort, ushort> HavocPieces = new Dictionary<ushort, ushort>
        {
            { 0xc24, 0xccd }
        };

        private static void CombineAndTransform(Player p, (int, int) itemPair)
        {

            if (itemPair.Item1 == -1 || itemPair.Item2 == -1) return;

            var item = (ushort) p.Inventory[itemPair.Item2];

            if (!HavocPieces.ContainsKey(item)) return;

            var transformTo = HavocPieces[item];

            p.Inventory[itemPair.Item1] = -1;
            p.ItemDatas[itemPair.Item1] = new ItemDataJson();

            p.ItemDatas[itemPair.Item2] = new ItemDataJson();
            p.Inventory[itemPair.Item2] = transformTo;
        }
    }
}
