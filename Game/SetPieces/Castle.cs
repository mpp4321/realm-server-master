using System;
using System.Linq;
using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Game.Logic;
using RotMG.Game.Logic.Loots;
using RotMG.Utils;

namespace RotMG.Game.SetPieces
{
    class Castle : ISetPiece
    {
        public int Size { get { return 40; } }

        static readonly string Floor = "Rock";
        static readonly string Bridge = "Bridge";
        static readonly string WaterA = "Shallow Water";
        static readonly string WaterB = "Dark Water";
        static readonly string WallA = "Grey Wall";
        static readonly string WallB = "Destructible Grey Wall";

        static readonly Loot chest = new Loot(
                new TierLoot(6, TierLoot.LootType.Weapon, 0.3f),
                new TierLoot(7, TierLoot.LootType.Weapon, 0.2f),
                new TierLoot(8, TierLoot.LootType.Weapon, 0.1f),

                new TierLoot(5, TierLoot.LootType.Armor, 0.3f),
                new TierLoot(6, TierLoot.LootType.Armor, 0.2f),
                new TierLoot(7, TierLoot.LootType.Armor, 0.1f),

                new TierLoot(2, TierLoot.LootType.Ability, 0.3f),
                new TierLoot(3, TierLoot.LootType.Ability, 0.2f),
                new TierLoot(4, TierLoot.LootType.Ability, 0.1f),

                new TierLoot(2, TierLoot.LootType.Ring, 0.25f),
                new TierLoot(3, TierLoot.LootType.Ring, 0.15f)
        );
        
        public void RenderSetPiece(World world, IntPoint pos)
        {
            var t = new int[31, 40];

            for (var x = 0; x < 13; x++)    //Moats
                for (var y = 0; y < 13; y++)
                {
                    if ((x == 0 && (y < 3 || y > 9)) ||
                        (y == 0 && (x < 3 || x > 9)) ||
                        (x == 12 && (y < 3 || y > 9)) ||
                        (y == 12 && (x < 3 || x > 9)))
                        continue;
                    t[x + 0, y + 0] = t[x + 18, y + 0] = 2;
                    t[x + 0, y + 27] = t[x + 18, y + 27] = 2;
                }
            for (var x = 3; x < 28; x++)
                for (var y = 3; y < 37; y++)
                {
                    if (x < 6 || x > 24 || y < 6 || y > 33)
                        t[x, y] = 2;
                }

            for (var x = 7; x < 24; x++)    //Floor
                for (var y = 7; y < 33; y++)
                    t[x, y] = MathUtils.Chance(.33f) ? 0 : 1;

            for (var x = 0; x < 7; x++)    //Perimeter
                for (var y = 0; y < 7; y++)
                {
                    if ((x == 0 && y != 3) ||
                        (y == 0 && x != 3) ||
                        (x == 6 && y != 3) ||
                        (y == 6 && x != 3))
                        continue;
                    t[x + 3, y + 3] = t[x + 21, y + 3] = 4;
                    t[x + 3, y + 30] = t[x + 21, y + 30] = 4;
                }
            for (var x = 6; x < 25; x++)
                t[x, 6] = t[x, 33] = 4;
            for (var y = 6; y < 34; y++)
                t[6, y] = t[24, y] = 4;

            for (var x = 13; x < 18; x++)    //Bridge
                for (var y = 3; y < 7; y++)
                    t[x, y] = 6;

            for (var x = 0; x < 31; x++)    //Corruption
                for (var y = 0; y < 40; y++)
                {
                    if (t[x, y] == 1 || t[x, y] == 0) continue;
                    var p = MathUtils.NextFloat();
                    if (t[x, y] == 6)
                    {
                        if (p < 0.4)
                            t[x, y] = 0;
                        continue;
                    }

                    if (p < 0.1)
                        t[x, y] = 1;
                    else if (p < 0.4)
                        t[x, y]++;
                }

            //Boss & Chest
            t[15, 27] = 7;
            t[15, 20] = 8;

            var r = MathUtils.Next(4);
            for (var i = 0; i < r; i++)     //Rotation
                t = SetPieces.RotateCW(t);
            int w = t.GetLength(0), h = t.GetLength(1);
            
            for (var x = 0; x < w; x++)     //Rendering
                for (var y = 0; y < h; y++)
                {
                    if (t[x, y] == 1)
                    {
                        world.UpdateTile(x + pos.X, y + pos.Y, Resources.Id2Tile[Floor].Type);
                        world.RemoveStatic(x + pos.X, y + pos.Y);
                    }

                    else if (t[x, y] == 2)
                    {
                        world.UpdateTile(x + pos.X, y + pos.Y, Resources.Id2Tile[WaterA].Type);
                        world.RemoveStatic(x + pos.X, y + pos.Y);
                    }
                    else if (t[x, y] == 3)
                    {
                        world.UpdateTile(x + pos.X, y + pos.Y, Resources.Id2Tile[WaterB].Type);
                        world.RemoveStatic(x + pos.X, y + pos.Y);
                    }

                    else if (t[x, y] == 4)
                    {
                        world.UpdateTile(x + pos.X, y + pos.Y, Resources.Id2Tile[Floor].Type);
                        world.UpdateStatic(x + pos.X, y + pos.Y, Resources.Id2Object[WallA].Type);
                    }
                    else if (t[x, y] == 5)
                    {
                        world.UpdateTile(x + pos.X, y + pos.Y, Resources.Id2Tile[Floor].Type);
                        world.UpdateStatic(x + pos.X, y + pos.Y, Resources.Id2Object[WallB].Type);
                    }

                    else if (t[x, y] == 6)
                    {
                        world.UpdateTile(x + pos.X, y + pos.Y, Resources.Id2Tile[Bridge].Type);
                    }
                    else if (t[x, y] == 7)
                    {
                        var c = new Container(0x0501, -1, null);
                        var loot = chest.GetLoots(3, 8).ToArray();
                        for (var k = 0; k < loot.Length; k++)
                        {
                            var roll = Resources.Type2Item[loot[k]].Roll();
                            c.Inventory[k] = loot[k];
                            c.ItemDatas[k] = roll.Item1 ? (int) roll.Item2 : -1;
                            c.UpdateInventorySlot(k);
                        }

                        world.AddEntity(c, new Vector2(pos.X + x + 0.5f, pos.Y + y + 0.5f));
                    }
                    else if (t[x, y] == 8)
                    {
                        var entity = Entity.Resolve(Resources.Id2Object["Cyclops God"].Type);
                        world.AddEntity(entity, new Vector2(pos.X + x, pos.Y + y));
                    }
                }
        }
    }
}
