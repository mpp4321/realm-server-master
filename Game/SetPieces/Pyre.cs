using System;
using System.Linq;
using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Game.Logic;
using RotMG.Game.Logic.Loots;
using RotMG.Utils;

namespace RotMG.Game.SetPieces
{
    class Pyre : ISetPiece
    {
        public int Size
        {
            get { return 30; }
        }

        static readonly string Floor = "Scorch Blend";

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
            for (var x = 0; x < Size; x++)
                for (var y = 0; y < Size; y++)
                {
                    var dx = x - (Size / 2.0);
                    var dy = y - (Size / 2.0);
                    var r = Math.Sqrt(dx * dx + dy * dy) + MathUtils.NextFloat() * 4 - 2;
                    if (r <= 10)
                    {
                        world.UpdateTile(x + pos.X, y + pos.Y, Resources.Id2Tile[Floor].Type);
                        world.RemoveStatic(x + pos.X, y + pos.Y);
                    }
                }

            var entity = Entity.Resolve(Resources.Id2Object["Phoenix Lord"].Type);
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
