using System;
using System.Linq;
using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Game.Logic;
using RotMG.Game.Logic.Loots;
using RotMG.Utils;

namespace RotMG.Game.SetPieces
{
    class LavaFissure : ISetPiece
    {
        public int Size
        {
            get { return 40; }
        }

        static readonly string Lava = "Lava Blend";
        static readonly string Floor = "Partial Red Floor";

        static readonly Loot chest = new Loot(
                new TierLoot(7, TierLoot.LootType.Weapon, 0.3f),
                new TierLoot(8, TierLoot.LootType.Weapon, 0.2f),
                new TierLoot(9, TierLoot.LootType.Weapon, 0.1f),

                new TierLoot(6, TierLoot.LootType.Armor, 0.3f),
                new TierLoot(7, TierLoot.LootType.Armor, 0.2f),
                new TierLoot(8, TierLoot.LootType.Armor, 0.1f),

                new TierLoot(2, TierLoot.LootType.Ability, 0.3f),
                new TierLoot(3, TierLoot.LootType.Ability, 0.2f),
                new TierLoot(4, TierLoot.LootType.Ability, 0.1f),

                new TierLoot(2, TierLoot.LootType.Ring, 0.25f),
                new TierLoot(3, TierLoot.LootType.Ring, 0.15f)
            );
        
        public void RenderSetPiece(World world, IntPoint pos)
        {
            var p = new int[Size, Size];
            const double SCALE = 5.5;
            for (var x = 0; x < Size; x++)      //Lava
            {
                var t = (double)x / Size * Math.PI;
                var x_ = t / Math.Sqrt(2) - Math.Sin(t) / (SCALE * Math.Sqrt(2));
                var y1 = t / Math.Sqrt(2) - 2 * Math.Sin(t) / (SCALE * Math.Sqrt(2));
                var y2 = t / Math.Sqrt(2) + Math.Sin(t) / (SCALE * Math.Sqrt(2));
                y1 /= Math.PI / Math.Sqrt(2);
                y2 /= Math.PI / Math.Sqrt(2);

                var y1_ = (int)Math.Ceiling(y1 * Size);
                var y2_ = (int)Math.Floor(y2 * Size);
                for (var i = y1_; i < y2_; i++)
                    p[x, i] = 1;
            }

            for (var x = 0; x < Size; x++)      //Floor
                for (var y = 0; y < Size; y++)
                {
                    if (p[x, y] == 1 && MathUtils.Chance(.2f))
                        p[x, y] = 2;
                }

            var r = MathUtils.Next(4);            //Rotation
            for (var i = 0; i < r; i++)
                p = SetPieces.RotateCW(p);
            p[20, 20] = 2;
            
            for (var x = 0; x < Size; x++)      //Rendering
                for (var y = 0; y < Size; y++)
                {
                    if (p[x, y] == 1)
                    {
                        world.UpdateTile(x + pos.X, y + pos.Y, Resources.Id2Tile[Lava].Type);
                        world.RemoveStatic(x + pos.X, y + pos.Y);
                    }
                    else if (p[x, y] == 2)
                    {
                        world.UpdateTile(x + pos.X, y + pos.Y, Resources.Id2Tile[Lava].Type);
                        world.UpdateStatic(x + pos.X, y + pos.Y, Resources.Id2Object[Floor].Type);
                    }
                }



            var entity = Entity.Resolve(Resources.Id2Object["Red Demon"].Type);
            world.AddEntity(entity, new Vector2(pos.X + 20.5f, pos.Y + 20.5f));

            var c = new Container(0x0501, -1, null);
            var loot = chest.GetLoots(3, 8).ToArray();
            for (var k = 0; k < loot.Length; k++)
            {
                var roll = Resources.Type2Item[loot[k]].Roll();
                c.Inventory[k] = loot[k];
                c.ItemDatas[k] = roll.Item1 ? (int) roll.Item2 : -1;
                c.UpdateInventorySlot(k);
            }

            world.AddEntity(c, new Vector2(pos.X + 20.5f, pos.Y + 20.5f));
        }
    }
}
