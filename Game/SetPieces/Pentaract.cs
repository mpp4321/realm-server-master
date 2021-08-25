using System;
using RotMG.Common;

namespace RotMG.Game.SetPieces
{
    class Pentaract : ISetPiece
    {
        public int Size { get { return 41; } }

        static readonly string Floor = "Scorch Blend";
        static readonly byte[,] Circle = new byte[,]
        {
            { 0, 0, 1, 1, 1, 0, 0 },
            { 0, 1, 1, 1, 1, 1, 0 },
            { 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1 },
            { 0, 1, 1, 1, 1, 1, 0 },
            { 0, 0, 1, 1, 1, 0, 0 },
        };

        Random rand = new Random();
        public void RenderSetPiece(World world, IntPoint pos)
        {
            var t = new int[41, 41];

            for (var i = 0; i < 5; i++)
            {
                double angle = (360 / 5 * i) * (float)Math.PI / 180;
                var x_ = (int)(Math.Cos(angle) * 15 + 20 - 3);
                var y_ = (int)(Math.Sin(angle) * 15 + 20 - 3);

                for (var x = 0; x < 7; x++)
                    for (var y = 0; y < 7; y++)
                    {
                        t[x_ + x, y_ + y] = Circle[x, y];
                    }
                t[x_ + 3, y_ + 3] = 2;
            }
            t[20, 20] = 3;

            for (var x = 0; x < 40; x++)
                for (var y = 0; y < 40; y++)
                {
                    if (t[x, y] == 1)
                    {
                        world.UpdateTile(x + pos.X, y + pos.Y, Resources.Id2Tile[Floor].Type);
                        world.RemoveStatic(x + pos.X, y + pos.Y);
                    }
                    else if (t[x, y] == 2)
                    {
                        world.UpdateTile(x + pos.X, y + pos.Y, Resources.Id2Tile[Floor].Type);
                        world.RemoveStatic(x + pos.X, y + pos.Y);

                        var penta = Entity.Resolve(0x0d5e);
                        world.AddEntity(penta, new Vector2(pos.X + x + .5f, pos.Y + y + .5f));
                    }
                    else if (t[x, y] == 3)
                    {
                        var entity = Entity.Resolve(Resources.Id2Object["Pentaract"].Type);
                        world.AddEntity(entity, new Vector2(pos.X + x + .5f, pos.Y + y + .5f));
                    }
                }
        }
    }
}
