using System;
using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Utils;

namespace RotMG.Game.SetPieces
{
    class Sphinx : ISetPiece
    {
        public int Size
        {
            get { return 81; }
        }

        static readonly byte[,] Center = new byte[,]
        {
            { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 1, 1, 1, 0, 1, 1, 1, 0, 1, 1, 1, 0, 0, 0 },
            { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 },
            { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 },
            { 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0 },
            { 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0 },
            { 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0 },
            { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 },
            { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 },
            { 0, 0, 0, 1, 1, 1, 0, 1, 1, 1, 0, 1, 1, 1, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
        };

        static readonly string Floor = "Gold Sand";
        static readonly string Central = "Sand Tile";
        static readonly string Pillar = "Tomb Wall";
        
        public void RenderSetPiece(World world, IntPoint pos)
        {
            var t = new int[81, 81];
            for (var x = 0; x < Size; x++)                      //Flooring
                for (var y = 0; y < Size; y++)
                {
                    var dx = x - (Size / 2.0);
                    var dy = y - (Size / 2.0);
                    var r = Math.Sqrt(dx * dx + dy * dy) + MathUtils.NextFloat() * 4 - 2;
                    if (r <= 35)
                        t[x, y] = 1;
                }

            for (var x = 0; x < 17; x++)                        //Center
                for (var y = 0; y < 17; y++)
                {
                    if (Center[x, y] != 0)
                        t[32 + x, 32 + y] = 2;
                }

            t[36, 36] = t[44, 36] = t[36, 44] = t[44, 44] = 3;  //Pillars
            t[30, 30] = t[50, 30] = t[30, 50] = t[50, 50] = 4;

            t[40, 26] = t[40, 27] = t[39, 27] = t[41, 27] = 4;
            t[40, 54] = t[40, 53] = t[39, 53] = t[41, 53] = 4;
            t[26, 40] = t[27, 40] = t[27, 39] = t[27, 41] = 4;
            t[54, 40] = t[53, 40] = t[53, 39] = t[53, 41] = 4;
            
            for (var x = 0; x < Size; x++)                      //Rendering
                for (var y = 0; y < Size; y++)
                    if (t[x, y] == 1)
                    {
                        world.UpdateTile(x + pos.X, y + pos.Y, Resources.Id2Tile[Floor].Type);
                        world.RemoveStatic(x + pos.X, y + pos.Y);
                    }
                    else if (t[x, y] == 2)
                    {
                        world.UpdateTile(x + pos.X, y + pos.Y, Resources.Id2Tile[Central].Type);
                        world.RemoveStatic(x + pos.X, y + pos.Y);
                    }
                    else if (t[x, y] == 3)
                    {
                        world.UpdateTile(x + pos.X, y + pos.Y, Resources.Id2Tile[Central].Type);
                        var wall = Entity.Resolve(Resources.Id2Object[Pillar].Type) as ConnectedObject;
                        world.UpdateStatic(x + pos.X, y + pos.Y, wall);
                    }
                    else if (t[x, y] == 4)
                    {
                        world.UpdateTile(x + pos.X, y + pos.Y, Resources.Id2Tile[Floor].Type);
                        var wall = Entity.Resolve(Resources.Id2Object[Pillar].Type) as ConnectedObject;
                        world.UpdateStatic(x + pos.X, y + pos.Y, wall);
                    }

            var entity = Entity.Resolve(Resources.Id2Object["Grand Sphinx"].Type);
            world.AddEntity(entity, new Vector2(pos.X + 40.5f, pos.Y + 40.5f));
        }
    }
}
