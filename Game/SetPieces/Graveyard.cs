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
    public class Graveyard : ISetPiece
    {
        public int Size { get { return 34; } }

        static readonly string Floor = "Grass";
        static readonly string WallA = "Grey Wall";
        static readonly string WallB = "Destructible Grey Wall";
        static readonly string Cross = "Cross";

        static readonly Loot chest = new Loot(
                new TierLoot(4, TierLoot.LootType.Weapon, 0.3f),
                new TierLoot(5, TierLoot.LootType.Weapon, 0.2f),
                new TierLoot(6, TierLoot.LootType.Weapon, 0.1f),

                new TierLoot(3, TierLoot.LootType.Armor, 0.3f),
                new TierLoot(4, TierLoot.LootType.Armor, 0.2f),
                new TierLoot(5, TierLoot.LootType.Armor, 0.1f),

                new TierLoot(1, TierLoot.LootType.Ability, 0.3f),
                new TierLoot(2, TierLoot.LootType.Ability, 0.2f),
                new TierLoot(3, TierLoot.LootType.Ability, 0.2f),

                new TierLoot(1, TierLoot.LootType.Ring, 0.25f),
                new TierLoot(2, TierLoot.LootType.Ring, 0.15f)
            );
        
        public void RenderSetPiece(World world, IntPoint pos)
        {
            var t = new int[23, 35];

            for (var x = 0; x < 23; x++)    //Floor
                for (var y = 0; y < 35; y++)
                    t[x, y] = MathUtils.Chance(.33f) ? 0 : 1;

            for (var y = 0; y < 35; y++)    //Perimeters
                t[0, y] = t[22, y] = 2;
            for (var x = 0; x < 23; x++)
                t[x, 0] = t[x, 34] = 2;

            var pts = new List<IntPoint>();
            for (var y = 0; y < 11; y++)    //Crosses
                for (var x = 0; x < 7; x++)
                {
                    if (MathUtils.Chance(.66f))
                        t[2 + 3 * x, 2 + 3 * y] = 4;
                    else
                        pts.Add(new IntPoint(2 + 3 * x, 2 + 3 * y));
                }

            for (var x = 0; x < 23; x++)    //Corruption
                for (var y = 0; y < 35; y++)
                {
                    if (t[x, y] == 1 || t[x, y] == 0 || t[x, y] == 4) continue;
                    var p = MathUtils.NextFloat();
                    if (p < 0.1)
                        t[x, y] = 1;
                    else if (p < 0.4)
                        t[x, y]++;
                }


            //Boss & Chest
            var pt = pts[MathUtils.Next(pts.Count)];
            t[pt.X, pt.Y] = 5;
            t[pt.X+1, pt.Y] = 6;

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
                        world.UpdateTile(x + pos.X, y + pos.Y, Resources.Id2Tile[Floor].Type);
                        world.UpdateStatic(x + pos.X, y + pos.Y, Resources.Id2Object[WallA].Type);
                    }
                    else if (t[x, y] == 3)
                    {
                        world.UpdateTile(x + pos.X, y + pos.Y, Resources.Id2Tile[Floor].Type);
                        var wall = Entity.Resolve(Resources.Id2Object[WallB].Type);
                        world.AddEntity(wall, new Vector2(x + pos.X + 0.5f, y + pos.Y + 0.5f));
                    }
                    else if (t[x, y] == 4)
                    {
                        world.UpdateTile(x + pos.X, y + pos.Y, Resources.Id2Tile[Floor].Type);
                        world.UpdateStatic(x + pos.X, y + pos.Y, Resources.Id2Object[Cross].Type);
                    }
                    else if (t[x, y] == 5)
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
                    else if (t[x, y] == 6)
                    {
                        var entity = Entity.Resolve(Resources.Id2Object["Deathmage"].Type);
                        world.AddEntity(entity, new Vector2(pos.X + x, pos.Y + y));
                    }
                }
        }
    }
}