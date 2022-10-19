using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Game.Logic.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static RotMG.Game.Logic.LootDef;

namespace RotMG.Game.Logic.Mechanics
{
    class Mix
    {

        interface IMix
        {
            bool CanMix(ItemDesc item, ItemDataJson json);
        }

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

        public static bool DoMix(Player p, int slot1, int slot2)
        {
            var items = new int[] { p.Inventory[slot1], p.Inventory[slot2] };
            if (items[0] == -1 || items[1] == -1) return false;
            var descs = items.Select(a => Resources.Type2Item[(ushort)a]).ToList();

            //Crafting
            (int, int) goo = FindComponentItem(slot1, slot2, descs[0], descs[1], x =>
            {
                return x.ActivateEffects.Count() > 0 && x.ActivateEffects[0].Index == ActivateEffectIndex.MagicCrystal;
            });

            (int, int) comp = FindComponentItem(slot1, slot2, descs[0], descs[1], x =>
            {
                return x.Component != null;
            });

            if (ItemTransforms.ContainsKey(descs[0].Type))
            {
                CombineAndTransform(p, (slot1, slot2), ItemTransforms[descs[0].Type]);
                p.UpdateInventory();
                return true; 
            }
            else if (ItemTransforms.ContainsKey(descs[1].Type))
            {
                CombineAndTransform(p, (slot2, slot1), ItemTransforms[descs[1].Type]);
                p.UpdateInventory();
                return true;
            }
            else if (comp != (-1, -1))
            {
                ApplyComponent(p, comp);
                p.UpdateInventory();
                return true;
            }
            else if (!(goo.Item1 == -1 || goo.Item2 == -1))
            {
                var success = CombineAndReroll(p, goo);
                p.UpdateInventory();
                return true;
            }

            return false;
        }

        private static Dictionary<string, Action<int, ItemDataJson>> OnApply = new Dictionary<string, Action<int, ItemDataJson>>()
        {
            {"AttackComp", (id, json) => {
                json.ExtraStatBonuses[(ulong) ItemData.Attack] += 6;
            } },
            {"FireRune", (id, json) => {
                var v = json.ExtraStatBonuses.GetValueOrDefault((ulong) ItemData.Attack);
                json.ExtraStatBonuses[(ulong) ItemData.Attack] = v + 1000;
            } }
        };

        private static Dictionary<string, Action<int, ItemDataJson>> OnRemove = new Dictionary<string, Action<int, ItemDataJson>>()
        {
            {"AttackComp", (id, json) => {
                json.ExtraStatBonuses[(ulong) ItemData.Attack] -= 6;
            } },
            {"FireRune", (id, json) => {
                var v = json.ExtraStatBonuses.GetValueOrDefault((ulong) ItemData.Attack);
                json.ExtraStatBonuses[(ulong) ItemData.Attack] = v - 1000;
            } }
        };

        private static void ApplyComponent(Player p, (int, int) itemPair)
        {
            var desc = Resources.Type2Item[(ushort) p.Inventory[itemPair.Item1]].Component;

            p.Inventory[itemPair.Item1] = -1;
            p.ItemDatas[itemPair.Item1] = new ItemDataJson();

            if (OnApply.TryGetValue(desc, out var add))
            {
                add(itemPair.Item2, p.ItemDatas[itemPair.Item2]);
            }

            if (OnRemove.TryGetValue(desc, out var remove))
            {
                remove(itemPair.Item2, p.ItemDatas[itemPair.Item2]);
            }

            p.ItemDatas[itemPair.Item2].ItemComponent = desc;
        }

        private static bool CombineAndReroll(Player p, (int, int) itemPair)
        {
            if (itemPair.Item1 == -1 || itemPair.Item2 == -1) return false;
            var crystal = Resources.Type2Item[(ushort)p.Inventory[itemPair.Item1]];
            var other = Resources.Type2Item[(ushort)p.Inventory[itemPair.Item2]];
            if (other.Consumable) return false;
            var eff = crystal.ActivateEffects[0];
            int power = eff.Amount;
            float scale = eff.StatScale;
            ItemDataModType typeOfMod = Enum.Parse<ItemDataModType>(crystal.ActivateEffects[0].Id ?? p.Client.Character.ItemDataModifier);
            var item = Resources.Type2Item[(ushort)p.Inventory[itemPair.Item2]];
            var r = item.Roll(new RarityModifiedData(scale, power, true), typeOfMod);
            var upgradeOnly = eff.UpgradeOnly;
            if(upgradeOnly)
            {
                var currentLevel = p.ItemDatas[itemPair.Item2].ItemLevel;
                r = item.Roll(new RarityModifiedData(scale, currentLevel, false), typeOfMod);
                if(currentLevel < 1)
                {
                    return false;
                }
            }

            if (!p.ItemDatas[itemPair.Item2].IsLocked)
            {
                p.Inventory[itemPair.Item1] = -1;
                p.ItemDatas[itemPair.Item1] = new ItemDataJson();

                if(upgradeOnly)
                {
                    if(r.Item1)
                        p.ItemDatas[itemPair.Item2].ItemLevel = r.Item2.ItemLevel;
                } else
                {
                    p.ItemDatas[itemPair.Item2] = r.Item1 ? r.Item2 : new ItemDataJson();
                }
            }
            return true;
       }

        private static Dictionary<int, Dictionary<int, int>> ItemTransforms = new Dictionary<int, Dictionary<int, int>> {
            { Resources.Id2Item["Piece of Havoc"].Type, new Dictionary<int, int> { { 0xc24, 0xccd }, { Resources.Id2Item["Cracked Waraxe"].Type, Resources.Id2Item["Paladin's Waraxe"].Type } } },
            { Resources.Id2Item["Golden Demonic Metal"].Type, new Dictionary<int, int> { { Resources.Id2Item["Demon Blade"].Type, Resources.Id2Item["Gilded Demon Blade"].Type } } }
        };

        private static void CombineAndTransform(Player p, (int, int) itemPair, Dictionary<int, int> ItemSet)
        {

            if (itemPair.Item1 == -1 || itemPair.Item2 == -1) return;

            var item = (ushort) p.Inventory[itemPair.Item2];

            if (!ItemSet.ContainsKey(item)) return;

            var transformTo = ItemSet[item];

            p.Inventory[itemPair.Item1] = -1;
            p.ItemDatas[itemPair.Item1] = new ItemDataJson();

            p.ItemDatas[itemPair.Item2] = new ItemDataJson();
            p.Inventory[itemPair.Item2] = transformTo;
        }
    }
}
