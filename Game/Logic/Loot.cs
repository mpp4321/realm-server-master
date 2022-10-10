using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Game.Logic.Loots;
using RotMG.Networking;
using RotMG.Utils;

namespace RotMG.Game.Logic
{
    public class LootDef
    {

        public class Builder
        {
            private string _item = "NONE";
            private float _thresh = 0.0f;
            private float _chance = 0.0001f;
            private int _min = 0;
            
            //How many players can receive this drop; sorted from top damagers
            public int _maxTop = 999;

            public ItemDataJson _override;

            public RarityModifiedData _rareData;

            public LootDef Build()
            {
                return new LootDef(_item, _chance, _thresh, _min, _rareData, _maxTop, _override);
            }

            public Builder Item(string name) { _item = name; return this; }
            public Builder Threshold(float thresh) { _thresh = thresh; return this; }
            public Builder Chance(float chance) { _chance = chance; return this; }
            public Builder Min(int min) { _min = min; return this; }
            public Builder MaxTop(int max) { _maxTop = max; return this; }
            public Builder RareMod(RarityModifiedData RareData) { _rareData = RareData; return this; }
            public Builder OverrideJson(ItemDataJson json) { _override = json; return this; }

        }


        public class RarityModifiedData
        {
            public RarityModifiedData() { }
            public RarityModifiedData(RarityModifiedData r) 
            {
                RarityMod = r.RarityMod;
                RarityShift = r.RarityShift;
                AlwaysRare = r.AlwaysRare;
            }
            public RarityModifiedData(float mod) { RarityMod = mod; }
            public RarityModifiedData(float mod, int shift) { RarityMod = mod; RarityShift = shift; }
            public RarityModifiedData(float mod, int shift, bool alwaysRare) { RarityMod = mod; RarityShift = shift; AlwaysRare = alwaysRare; }

            public float RarityMod = 1.0f;
            public int RarityShift = 0;
            public bool AlwaysRare = false;
            public ItemDataModType? OverrideMod = null;
        }

        public readonly ushort Item;
        public readonly float Threshold;
        public readonly float Chance;
        public readonly int Min;
        
        //How many players can receive this drop; sorted from top damagers
        public readonly int MaxTop = 999;

        public readonly RarityModifiedData RareData;

        public readonly ItemDataJson overrideData = null;

        public LootDef(string item,
            float chance = 1,
            float threshold = 0,
            int min = 0,
            RarityModifiedData r = null,
            int maxTop = 999,
            ItemDataJson overrideData = null
            )
        {
            if(item != null)
                Item = Resources.IdLower2Item[item.ToLower()].Type;
            Threshold = threshold;
            Chance = chance;
            Min = min;
            RareData = r ?? new RarityModifiedData();
            MaxTop = maxTop;
            this.overrideData = overrideData;
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
            var c = 0;

            // Attempt to stop players from getting multiple loot bags
            HashSet<int> accIdsWithLoot = new HashSet<int>();

            //Sort by damage done total
            foreach (var (player, damage) in enemy.DamageStorage.OrderByDescending(k => k.Value))
            {
                if (accIdsWithLoot.Contains(player.AccountId)) continue;
                //Count top damagers
                c++;

                if (enemy.Desc.Quest || enemy.IsElite)
                {
                    player.HealthPotions = Math.Min(Player.MaxPotions, player.HealthPotions + 1);
                    player.MagicPotions = Math.Min(Player.MaxPotions, player.MagicPotions + 1);
                }
                else
                {
                    if (MathUtils.Chance(.1f))
                        player.HealthPotions = Math.Min(Player.MaxPotions, player.HealthPotions + 1);
                    if (MathUtils.Chance(.1f))
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
                var sentTopDamage = false;

                foreach (var drop in possibleDrops)
                {
                    if (drop.MaxTop < c)
                        continue;
                    //up to 50% lootboost if you did all the damage
                    var baseMod = (1f + 0.5f * t);
                    baseMod += enemy.IsElite ? 1f : 0f;
                    baseMod += player.LootBoost;
                    baseMod += player.Parent?.WorldLB ?? 0.0f;

                    foreach (var ih in player.BuildAllItemHandlers())
                        ih.ModifyDrop(player, drop, ref enemy.DamageStorage, ref baseMod);

                    var chance = drop.Chance * baseMod;
                    if (drop.Threshold > 0 && t >= drop.Threshold && MathUtils.Chance(chance))
                    {
                        if(drop.MaxTop == 1 && !sentTopDamage)
                        {
                            player.SendInfo("Congrats, you were top damage with " + damage + " damage!");
                            sentTopDamage = true;
                        }
                        loot.Add(drop);
                        --requiredDrops[drop.Item];
                    }
                }

                privateLoot[player] = loot;
                accIdsWithLoot.Add(player.AccountId);
            }

            
            foreach (var (drop, count) in requiredDrops.ToArray())
            {
                //All loot SB
                //if (dropDictionary[drop].Threshold <= 0)
                //{
                //    while (requiredDrops[drop] > 0)
                //    {
                //        publicLoot.Add(dropDictionary[drop]);
                //        --requiredDrops[drop];
                //    }
                //    continue;
                //}
                accIdsWithLoot.Clear();
                foreach (var (player, damage) in enemy.DamageStorage.OrderByDescending(k => k.Value))
                {
                    if (accIdsWithLoot.Contains(player.AccountId)) continue;
                    if (requiredDrops[drop] <= 0)
                        break;
                    
                    var t = Math.Min(1f, (float) damage / enemy.MaxHp);
                    if (t < dropDictionary[drop].Threshold)
                        continue;

                    if (privateLoot[player].Select(a => a.Item).Contains(drop))
                        continue;

                    privateLoot[player].Add(dropDictionary[drop]);
                    accIdsWithLoot.Add(player.AccountId);
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
                var significantItems = new List<ItemDesc>();
                ItemDesc topItem = null;
                for (var k = 0; k < bagCount; k++)
                {
                    var d = Resources.Type2Item[loot[k].Item];
                    if (d.BagType > bagType)
                    {
                        bagType = d.BagType;
                    }
                    if(d.BagType > 4 && d.BagType < 7)
                    {
                        significantItems.Add(d);
                        if((topItem?.BagType ?? -1) < d.BagType)
                        {
                            topItem = d;
                        }
                    }
                }

                if (player != null)
                {
                    if (bagType == 2) player.FameStats.CyanBags++;
                    else if (bagType == 3) player.FameStats.BlueBags++;
                    else if (bagType >= 4) player.FameStats.WhiteBags++;
                }

                var precentFormatted = String.Format("{0:0.##}", ((float)enemy.DamageStorage.GetValueOrDefault(player, 0) / enemy.MaxHp) * 100.0);
                if(topItem != null)
                {
                    switch(topItem.BagType)
                    {
                        case 6:
                            player.Client.Send(GameServer.LootNotif(0));
                            break;
                        case 5:
                            player.Client.Send(GameServer.LootNotif(1));
                            break;
                        case 4:
                            player.Client.Send(GameServer.LootNotif(2));
                            break;
                    }
                }
                foreach(var sItem in significantItems)
                {
                    switch (sItem.BagType)
                    {
                        case 6:
                            Manager.Announce("<LOOT> " + player.Name + " just got a legendary item " + sItem.DisplayId + " with " + precentFormatted + "%!");
                            break;
                        case 5:
                            Manager.Announce("<LOOT> " + player.Name + " just got a rare item " + sItem.DisplayId + " with " + precentFormatted  + "%!");
                            break;
                    }
                }

                var c = new Container(Container.FromBagType(bagType), ownerId, 40000 * bagType);
                for (var k = 0; k < bagCount; k++)
                {

                    Enum.TryParse<ItemDataModType>(player?.Client?.Character?.ItemDataModifier, out var vtype);
                    var roll = Resources.Type2Item[loot[k].Item].Roll(r: loot[k].RareData, smod: vtype);

                    //Roll an item twice on elite enemies, take better roll
                    if (enemy.IsElite)
                    {
                        var rd = new LootDef.RarityModifiedData(loot[k].RareData);
                        rd.AlwaysRare = true;

                        var eliteTier = (enemy.MaxHp / 10000) + 1;

                        for (var z = 0; z < eliteTier; z++) {
                            var roll2 = Resources.Type2Item[loot[k].Item].Roll(r: rd, smod: vtype);
                            if (ItemDesc.GetRank(roll2.Item2) > ItemDesc.GetRank(roll.Item2))
                            {
                                roll = roll2;
                            }
                        }
                    }

                    c.Inventory[k] = loot[k].Item;
                    if(loot[k].overrideData != null)
                    {
                        c.ItemDatas[k] = (ItemDataJson) loot[k].overrideData.Clone();
                    } else
                        c.ItemDatas[k] = roll.Item1 ? roll.Item2 : new ItemDataJson() { Meta=-1 };

                    if(player != null)
                    {
                        foreach (var ih in player.BuildAllItemHandlers())
                            ih.ModifyDroppedItemData(player, ref c.ItemDatas[k]);
                    }

                    c.UpdateInventorySlot(k);
                }
                loot.RemoveRange(0, bagCount);

                var position = MathUtils.PlusMinus() * MathUtils.NextFloat() * MathUtils.Position(1.0f, 1.0f);
                enemy.Parent.AddEntity(c, enemy.Position + position);
            }
        }
    }
}
