using System;
using System.Collections.Generic;
using System.Linq;
using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Game.Logic;
using RotMG.Game.Logic.Loots;
using RotMG.Utils;

namespace RotMG.Game.SetPieces
{
    class Oasis : ISetPiece
    {
        public int Size
        {
            get { return 30; }
        }

        static readonly string Floor = "Light Grass";
        static readonly string Water = "Shallow Water";
        static readonly string Tree = "Palm Tree";

        static readonly Loot chest = new Loot(
                new TierLoot(5, TierLoot.LootType.Weapon, 0.3f),
                new TierLoot(6, TierLoot.LootType.Weapon, 0.2f),
                new TierLoot(7, TierLoot.LootType.Weapon, 0.1f),

                new TierLoot(4, TierLoot.LootType.Armor, 0.3f),
                new TierLoot(5, TierLoot.LootType.Armor, 0.2f),
                new TierLoot(6, TierLoot.LootType.Armor, 0.1f),

                new TierLoot(2, TierLoot.LootType.Ability, 0.3f),
                new TierLoot(3, TierLoot.LootType.Ability, 0.2f),

                new TierLoot(1, TierLoot.LootType.Ring, 0.25f),
                new TierLoot(2, TierLoot.LootType.Ring, 0.15f)
            );

        public void RenderSetPiece(World world, IntPoint pos)
        {
            var outerRadius = 13;
            var waterRadius = 10;
            var islandRadius = 3;
            var border = new List<IntPoint>();

            var t = new int[Size, Size];
            for (var y = 0; y < Size; y++)      //Outer
                for (var x = 0; x < Size; x++)
                {
                    var dx = x - (Size / 2.0);
                    var dy = y - (Size / 2.0);
                    var r = Math.Sqrt(dx * dx + dy * dy);
                    if (r <= outerRadius)
                        t[x, y] = 1;
                }

            for (var y = 0; y < Size; y++)      //Water
                for (var x = 0; x < Size; x++)
                {
                    var dx = x - (Size / 2.0);
                    var dy = y - (Size / 2.0);
                    var r = Math.Sqrt(dx * dx + dy * dy);
                    if (r <= waterRadius)
                    {
                        t[x, y] = 2;
                        if (waterRadius - r < 1)
                            border.Add(new IntPoint(x, y));
                    }
                }

            for (var y = 0; y < Size; y++)      //Island
                for (var x = 0; x < Size; x++)
                {
                    var dx = x - (Size / 2.0);
                    var dy = y - (Size / 2.0);
                    var r = Math.Sqrt(dx * dx + dy * dy);
                    if (r <= islandRadius)
                    {
                        t[x, y] = 1;
                        if (islandRadius - r < 1)
                            border.Add(new IntPoint(x, y));
                    }
                }

            var trees = new HashSet<IntPoint>();
            while (trees.Count < border.Count * 0.5)
                trees.Add(border[MathUtils.Next(border.Count)]);

            foreach (var i in trees)
                t[i.X, i.Y] = 3;
            
            for (var x = 0; x < Size; x++)
                for (var y = 0; y < Size; y++)
                {
                    if (t[x, y] == 1)
                    {
                        world.UpdateTile(x + pos.X, y + pos.Y, Resources.Id2Tile[Floor].Type);
                        world.RemoveStatic(x + pos.X, y + pos.Y);
                    }
                    else if (t[x, y] == 2)
                    {
                        world.UpdateTile(x + pos.X, y + pos.Y, Resources.Id2Tile[Water].Type);
                        world.RemoveStatic(x + pos.X, y + pos.Y);
                    }
                    else if (t[x, y] == 3)
                    {
                        world.UpdateTile(x + pos.X, y + pos.Y, Resources.Id2Tile[Floor].Type);
                        world.UpdateStatic(x + pos.X, y + pos.Y, Resources.Id2Object[Tree].Type);
                        world.GetTile(x + pos.X, y + pos.Y).StaticObject.Size = MathUtils.Chance(.5f) ? 120 : 140;
                    }
                }

            var entity = Entity.Resolve(Resources.Id2Object["Oasis Giant"].Type);
            world.AddEntity(entity, new Vector2(pos.X + 15.5f, pos.Y + 15.5f));

            var c = new Container(0x0501, -1, null);
            var loot = chest.GetLoots(3, 8).ToArray();
            for (var k = 0; k < loot.Length; k++)
            {
                var roll = Resources.Type2Item[loot[k]].Roll();
                c.Inventory[k] = loot[k];
                c.ItemDatas[k] = roll.Item1 ? (int) roll.Item2 : -1;
                c.UpdateInventorySlot(k);
            }

            world.AddEntity(c, new Vector2(pos.X + 15.5f, pos.Y + 15.5f));
        }
    }
}
