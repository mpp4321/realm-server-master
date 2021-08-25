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
        public readonly ushort Item;
        public readonly float Threshold;
        public readonly float Chance;
        public readonly int Min;

        public LootDef(string item, float chance = 1, float threshold = 0, int min = 0)
        {
            Item = Resources.IdLower2Item[item.ToLower()].Type;
            Threshold = threshold;
            Chance = chance;
            Min = min;
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
            var requiredDrops = possibleDrops.ToDictionary(drop => drop, drop => drop.Min);

            var publicLoot = new List<ushort>();
            foreach (var drop in possibleDrops)
            {
                if (drop.Threshold <= 0 && MathUtils.Chance(drop.Chance))
                {
                    publicLoot.Add(drop.Item);
                    --requiredDrops[drop];
                }
            }

            var privateLoot = new Dictionary<Player, List<ushort>>();
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
                var loot = new List<ushort>();
                foreach (var drop in possibleDrops)
                {
                    if (drop.Threshold > 0 && t >= drop.Threshold && MathUtils.Chance(drop.Chance))
                    {
                        loot.Add(drop.Item);
                        --requiredDrops[drop];
                    }
                }

                privateLoot[player] = loot;
            }
            
            foreach (var (drop, count) in requiredDrops.ToArray())
            {
                if (drop.Threshold <= 0)
                {
                    while (requiredDrops[drop] > 0)
                    {
                        publicLoot.Add(drop.Item);
                        --requiredDrops[drop];
                    }

                    continue;
                }

                foreach (var (player, damage) in enemy.DamageStorage.OrderByDescending(k => k.Value))
                {
                    if (requiredDrops[drop] <= 0)
                        break;
                    
                    var t = Math.Min(1f, (float) damage / enemy.MaxHp);
                    if (t < drop.Threshold)
                        continue;

                    if (privateLoot[player].Contains(drop.Item))
                        continue;

                    privateLoot[player].Add(drop.Item);
                    --requiredDrops[drop];
                }
            }

            AddBagsToWorld(enemy, publicLoot, privateLoot);
        }

        private void AddBagsToWorld(Enemy enemy, List<ushort> publicLoot, Dictionary<Player, List<ushort>> playerLoot)
        {
            foreach (var (player, loot) in playerLoot)
            {
                ShowBags(enemy, loot, player, player.AccountId);
            }
            ShowBags(enemy, publicLoot, null, -1);
        }

        private void ShowBags(Enemy enemy, List<ushort> loot, Player player, int ownerId)
        {
            while (loot.Count > 0)
            {
                var bagType = 1;
                var bagCount = Math.Min(loot.Count, 8);
                for (var k = 0; k < bagCount; k++)
                {
                    var d = Resources.Type2Item[loot[k]];
                    if (d.BagType > bagType)
                        bagType = d.BagType;
                }

                if (player != null)
                {
                    if (bagType == 2) player.FameStats.CyanBags++;
                    else if (bagType == 3) player.FameStats.BlueBags++;
                    else if (bagType == 4) player.FameStats.WhiteBags++;
                }

                var c = new Container(Container.FromBagType(bagType), ownerId, 40000 * bagType);
                for (var k = 0; k < bagCount; k++)
                {
                    var roll = Resources.Type2Item[loot[k]].Roll();
                    c.Inventory[k] = loot[k];
                    c.ItemDatas[k] = roll.Item1 ? (int) roll.Item2 : -1;
                    c.UpdateInventorySlot(k);
                }
                loot.RemoveRange(0, bagCount);

                enemy.Parent.AddEntity(c, enemy.Position + MathUtils.Position(0.2f, 0.2f));
            }
        }
    }
}
