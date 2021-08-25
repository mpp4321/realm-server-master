using RotMG.Common;
using RotMG.Game.Logic;
using RotMG.Game.Logic.Loots;

namespace RotMG.Game.SetPieces
{
    abstract class Temple : ISetPiece
    {
        public abstract int Size { get; }
        public abstract void RenderSetPiece(World world, IntPoint pos);

        protected static readonly string DarkGrass = "Dark Grass";
        protected static readonly string Floor = "Jungle Temple Floor";
        protected static readonly string WallA = "Jungle Temple Bricks";
        protected static readonly string WallB = "Jungle Temple Walls";
        protected static readonly string WallC = "Jungle Temple Column";
        protected static readonly string Flower = "Jungle Ground Flowers";
        protected static readonly string Grass = "Jungle Grass";
        protected static readonly string Tree = "Jungle Tree Big";

        protected static readonly Loot chest = new Loot(
                new TierLoot(4, TierLoot.LootType.Weapon, 0.3f),
                new TierLoot(5, TierLoot.LootType.Weapon, 0.2f),

                new TierLoot(4, TierLoot.LootType.Armor, 0.3f),
                new TierLoot(5, TierLoot.LootType.Armor, 0.2f),

                new TierLoot(1, TierLoot.LootType.Ability, 0.25f),
                new TierLoot(2, TierLoot.LootType.Ability, 0.15f),

                new TierLoot(2, TierLoot.LootType.Ring, 0.3f),
                new TierLoot(3, TierLoot.LootType.Ring, 0.2f)
            );

        protected static void Render(Temple temple, World world, IntPoint pos, int[,] ground, int[,] objs)
        {
            for (var x = 0; x < temple.Size; x++)                  //Rendering
                for (var y = 0; y < temple.Size; y++)
                {
                    if (ground[x, y] == 1)
                    {
                        world.UpdateTile(x + pos.X, y + pos.Y, Resources.Id2Tile[DarkGrass].Type);
                        world.RemoveStatic(x + pos.X, y + pos.Y);
                    }
                    else if (ground[x, y] == 2)
                    {
                        world.UpdateTile(x + pos.X, y + pos.Y, Resources.Id2Tile[Floor].Type);
                        world.RemoveStatic(x + pos.X, y + pos.Y);
                    }
                    if (objs[x, y] == 1)
                    {
                        world.UpdateStatic(x + pos.X, y + pos.Y, Resources.Id2Object[WallA].Type);
                    }
                    else if (objs[x, y] == 2)
                    {
                        world.UpdateStatic(x + pos.X, y + pos.Y, Resources.Id2Object[WallB].Type);
                    }
                    else if (objs[x, y] == 3)
                    {
                        world.UpdateStatic(x + pos.X, y + pos.Y, Resources.Id2Object[WallC].Type);
                    }
                    else if (objs[x, y] == 4)
                    {
                        world.UpdateStatic(x + pos.X, y + pos.Y, Resources.Id2Object[Flower].Type);
                    }
                    else if (objs[x, y] == 5)
                    {
                        world.UpdateStatic(x + pos.X, y + pos.Y, Resources.Id2Object[Grass].Type);
                    }
                    else if (objs[x, y] == 6)
                    {
                        world.UpdateStatic(x + pos.X, y + pos.Y, Resources.Id2Object[Tree].Type);
                    }
                }
        }
    }
}
