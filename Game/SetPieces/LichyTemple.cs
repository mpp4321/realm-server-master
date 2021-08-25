using RotMG.Common;
using RotMG.Utils;

namespace RotMG.Game.SetPieces
{
    class LichyTemple : ISetPiece
    {
        public int Size { get { return 26; } }

        static readonly string Floor = "Blue Floor";
        static readonly string WallA = "Blue Wall";
        static readonly string WallB = "Destructible Blue Wall";
        static readonly string PillarA = "Blue Pillar";
        static readonly string PillarB = "Broken Blue Pillar";

        
        public void RenderSetPiece(World world, IntPoint pos)
        {
            var t = new int[25, 26];

            for (var x = 2; x < 23; x++)    //Floor
                for (var y = 1; y < 24; y++)
                    t[x, y] = MathUtils.Chance(.1f) ? 0 : 1;

            for (var y = 1; y < 24; y++)    //Perimeters
                t[2, y] = t[22, y] = 2;
            for (var x = 2; x < 23; x++)
                t[x, 23] = 2;
            for (var x = 0; x < 3; x++)
                for (var y = 0; y < 3; y++)
                    t[x + 1, y] = t[x + 21, y] = 2;
            for (var x = 0; x < 5; x++)
                for (var y = 0; y < 5; y++)
                {
                    if ((x == 0 && y == 0) ||
                        (x == 0 && y == 4) ||
                        (x == 4 && y == 0) ||
                        (x == 4 && y == 4)) continue;
                    t[x, y + 21] = t[x + 20, y + 21] = 2;
                }

            for (var y = 0; y < 6; y++)     //Pillars
                t[9, 4 + 3 * y] = t[15, 4 + 3 * y] = 4;

            for (var x = 0; x < 25; x++)    //Corruption
                for (var y = 0; y < 26; y++)
                {
                    if (t[x, y] == 1 || t[x, y] == 0) continue;
                    var p = MathUtils.NextFloat();
                    if (p < 0.1)
                        t[x, y] = 1;
                    else if (p < 0.4)
                        t[x, y]++;
                }

            var r = MathUtils.Next(4);
            for (var i = 0; i < r; i++)     //Rotation
                t = SetPieces.RotateCW(t);
            int w = t.GetLength(0), h = t.GetLength(1);
            
            for (var x = 0; x < w; x++)    //Rendering
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
                        world.UpdateStatic(x + pos.X, y + pos.Y, Resources.Id2Object[WallB].Type);
                    }
                    else if (t[x, y] == 4)
                    {
                        world.UpdateTile(x + pos.X, y + pos.Y, Resources.Id2Tile[Floor].Type);
                        world.UpdateStatic(x + pos.X, y + pos.Y, Resources.Id2Object[PillarA].Type);
                    }
                    else if (t[x, y] == 5)
                    {
                        world.UpdateTile(x + pos.X, y + pos.Y, Resources.Id2Tile[Floor].Type);
                        world.UpdateStatic(x + pos.X, y + pos.Y, Resources.Id2Object[PillarB].Type);
                    }
                }

            //Boss
            var entity = Entity.Resolve(Resources.Id2Object["Lich"].Type);
            world.AddEntity(entity, new Vector2(pos.X + Size / 2, pos.Y + Size / 2));
        }
    }
}
