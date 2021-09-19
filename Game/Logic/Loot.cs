using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Game.Logic.Loots;
using RotMG.Utils;

namespace RotMG.Game.Logic
{
    public class LootDef
    {
        public class RarityModifiedData
        {
            public RarityModifiedData() { }
            public RarityModifiedData(float mod) { RarityMod = mod; }
            public RarityModifiedData(float mod, int shift) { RarityMod = mod; RarityShift = shift; }
            public RarityModifiedData(float mod, int shift, bool alwaysRare) { RarityMod = mod; RarityShift = shift; AlwaysRare = alwaysRare; }

            public float RarityMod = 1.0f;
            public int RarityShift = 0;
            public bool AlwaysRare = false;
        }

        public readonly ushort Item;
        public readonly float Threshold;
        public readonly float Chance;
        public readonly int Min;

        public readonly RarityModifiedData RareData;
        public LootDef(string item, float chance = 1, float threshold = 0, int min = 0, RarityModifiedData r = null)
        {
            if(item != null)
                Item = Resources.IdLower2Item[item.ToLower()].Type;
            Threshold = threshold;
            Chance = chance;
            Min = min;
            RareData = r ?? new RarityModifiedData();
        }
    }
    
    public class Loot : ReadOnlyCollection<MobDrop>
    {
        public Loot(params MobDrop[] drops) : base(drops) { }

        public IEnumerable<ushort> GetLoots(int min, int max)
        {
            var possibleItems = new List<LootDef>();
            foreach (var i in this)
                i.Populate(possibleItems);

            //it is possible to get less than the minimum
            var count = MathUtils.NextInt(min, max);
            foreach (var item in possibleItems)
            {
                if (MathUtils.Chance(item.Chance))
                {
                    yield return item.Item;
                    count--;
                }
                if (count <= 0)
                    yield break;
            }
        }

        private List<LootDef> GetPossibleDrops()
        {
            var possibleDrops = new List<LootDef>();
            foreach (var i in this)
            {
                i.Populate(possibleDrops);
            }
            return possibleDrops;
        }

        public void Handle(Enemy enemy, Player killer)
        {
            var possibleDrops = GetPossibleDrops();
            possibleDrops.AddRange(enemy.Parent.WorldLoot.GetPossibleDrops());
            var dropDictionary = possibleDrops.GroupBy(d => d.Item).ToDictionary(drop => drop.Key, drop => drop.First());
            var requiredDrops = possibleDrops.GroupBy(d => d.Item).ToDictionary(drop => drop.Key, drop => drop.Sum(a => a.Min));

            var publicLoot = new List<LootDef>();
            foreach (var drop in possibleDrops)
            {
                if (drop.Threshold <= 0 && MathUtils.Chance(drop.Chance))
                {
                    publicLoot.Add(drop);
                    --requiredDrops[drop.Item];
                }
            }

            var privateLoot = new Dictionary<Player, List<LootDef>>();
            foreach (var (player, damage) in enemy.DamageStorage.OrderByDescending(k => k.Value))
            {
                if (enemy.Desc.Quest)
                {
                    player.HealthPotions = Math.Min(Player.MaxPotions, player.HealthPotions + 1);
                    player.MagicPotions = Math.Min(Player.MaxPotions, player.MagicPotions + 1);
                }
                else
                {
                    if (MathUtils.Chance(.05f))
                        player.HealthPotions = Math.Min(Player.MaxPotions, player.HealthPotions + 1);
                    if (MathUtils.Chance(.05f))
                        player.MagicPotions = Math.Min(Player.MaxPotions, player.MagicPotions + 1);
                }

                if (!player.Equals(killer))
                {
                    player.FameStats.MonsterAssists++;
                    if (enemy.Desc.Cube) player.FameStats.CubeAssists++;
                    if (enemy.Desc.Oryx) player.FameStats.OryxAssists++;
                    if (enemy.Desc.God) player.FameStats.GodAssists++;
                }

                var t = Math.Min(1f, (float) damage / enemy.MaxHp);
                var loot = new List<LootDef>();
                foreach (var drop in possibleDrops)
                {
                    if (drop.Threshold > 0 && t >= drop.Threshold && MathUtils.Chance(drop.Chance * (1f + 1.2f*t)))
                    {
                        loot.Add(drop);
                        --requiredDrops[drop.Item];
                    }
                }

                privateLoot[player] = loot;
            }
            
            foreach (var (drop, count) in requiredDrops.ToArray())
            {
                if (dropDictionary[drop].Threshold <= 0)
                {
                    while (requiredDrops[drop] > 0)
                    {
                        publicLoot.Add(dropDictionary[drop]);
                        --requiredDrops[drop];
                    }
                    continue;
                }

                foreach (var (player, damage) in enemy.DamageStorage.OrderByDescending(k => k.Value))
                {
                    if (requiredDrops[drop] <= 0)
                        break;
                    
                    var t = Math.Min(1f, (float) damage / enemy.MaxHp);
                    if (t < dropDictionary[drop].Threshold)
                        continue;

                    if (privateLoot[player].Select(a => a.Item).Contains(drop))
                        continue;

                    privateLoot[player].Add(dropDictionary[drop]);
                    --requiredDrops[drop];
                }
            }

            AddBagsToWorld(enemy, publicLoot, privateLoot, killer);
        }

        private void AddBagsToWorld(Enemy enemy, List<LootDef> publicLoot, Dictionary<Player, List<LootDef>> playerLoot, Player killer)
        {
            foreach (var (player, loot) in playerLoot)
            {
                ShowBags(enemy, loot, player, player.AccountId);
            }
            ShowBags(enemy, publicLoot, killer, -1);
        }

        private void ShowBags(Enemy enemy, List<LootDef> loot, Player player, int ownerId)
        {
            while (loot.Count > 0)
            {
                var bagType = 1;
                var bagCount = Math.Min(loot.Count, 8);
                for (var k = 0; k < bagCount; k++)
                {
                    var d = Resources.Type2Item[loot[k].Item];
                    if (d.BagType > bagType)
                        bagType = d.BagType;
                }

                if (player != null)
                {
                    if (bagType == 2) player.FameStats.CyanBags++;
                    else if (bagType == 3) player.FameStats.BlueBags++;
                    else if (bagType >= 4) player.FameStats.WhiteBags++;
                }

                var c = new Container(Container.FromBagType(bagType), ownerId, 40000 * bagType);
                for (var k = 0; k < bagCount; k++)
                {
                    ItemDataModType vtype = ItemDataModType.Classical;
                    Enum.TryParse<ItemDataModType>(player?.Client?.Character?.ItemDataModifier, out vtype);

                    var roll = Resources.Type2Item[loot[k].Item].Roll(r: loot[k].RareData, smod: vtype);
                    c.Inventory[k] = loot[k].Item;
                    c.ItemDatas[k] = roll.Item1 ? roll.Item2 : new ItemDataJson() { Meta=-1 };
                    c.UpdateInventorySlot(k);
                }
                loot.RemoveRange(0, bagCount);

                enemy.Parent.AddEntity(c, enemy.Position + MathUtils.Position(0.2f, 0.2f));
            }
        }
    }
}
