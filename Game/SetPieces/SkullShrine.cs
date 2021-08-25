using System;
using RotMG.Common;
using RotMG.Utils;

namespace RotMG.Game.SetPieces
{
    class SkullShrine : ISetPiece
    {
        public int Size { get { return 33; } }

        static readonly string Grass = "Blue Grass";
        static readonly string Tile = "Castle Stone Floor Tile";
        static readonly string TileDark = "Castle Stone Floor Tile Dark";
        static readonly string Stone = "Cracked Purple Stone";
        static readonly string PillarA = "Blue Pillar";
        static readonly string PillarB = "Broken Blue Pillar";

        
        public void RenderSetPiece(World world, IntPoint pos)
        {
            var t = new int[33, 33];

            for (var x = 0; x < 33; x++)                    //Grassing
                for (var y = 0; y < 33; y++)
                {
                    if (Math.Abs(x - Size / 2) / (Size / 2.0) + MathUtils.NextFloat() * 0.3 < 0.95 &&
                        Math.Abs(y - Size / 2) / (Size / 2.0) + MathUtils.NextFloat() * 0.3 < 0.95)
                        t[x, y] = 1;
                }

            for (var x = 12; x < 21; x++)                   //Outer
                for (var y = 4; y < 29; y++)
                    t[x, y] = 2;
            t = SetPieces.RotateCW(t);
            for (var x = 12; x < 21; x++)
                for (var y = 4; y < 29; y++)
                    t[x, y] = 2;

            for (var x = 13; x < 20; x++)                   //Inner
                for (var y = 5; y < 28; y++)
                    t[x, y] = 4;
            t = SetPieces.RotateCW(t);
            for (var x = 13; x < 20; x++)
                for (var y = 5; y < 28; y++)
                    t[x, y] = 4;

            for (var i = 0; i < 4; i++)                     //Ext
            {
                for (var x = 13; x < 20; x++)
                    for (var y = 5; y < 7; y++)
                        t[x, y] = 3;
                t = SetPieces.RotateCW(t);
            }

            for (var i = 0; i < 4; i++)                     //Pillars
            {
                t[13, 7] = MathUtils.Chance(.33f) ? 6 : 5;
                t[19, 7] = MathUtils.Chance(.33f) ? 6 : 5;
                t[13, 10] = MathUtils.Chance(.33f) ? 6 : 5;
                t[19, 10] = MathUtils.Chance(.33f) ? 6 : 5;
                t = SetPieces.RotateCW(t);
            }

            var noise = new Noise(Environment.TickCount); //Perlin noise
            for (var x = 0; x < 33; x++)
                for (var y = 0; y < 33; y++)
                    if (noise.GetNoise(x / 33f * 8, y / 33f * 8, .5f) < 0.2)
                        t[x, y] = 0;
            
            for (var x = 0; x < 33; x++)                    //Rendering
                for (var y = 0; y < 33; y++)
                {
                    if (t[x, y] == 1)
                    {
                        world.UpdateTile(x + pos.X, y + pos.Y, Resources.Id2Tile[Grass].Type);
                        world.RemoveStatic(x + pos.X, y + pos.Y);
                    }
                    else if (t[x, y] == 2)
                    {
                        world.UpdateTile(x + pos.X, y + pos.Y, Resources.Id2Tile[TileDark].Type);
                        world.RemoveStatic(x + pos.X, y + pos.Y);
                    }
                    else if (t[x, y] == 3)
                    {
                        world.UpdateTile(x + pos.X, y + pos.Y, Resources.Id2Tile[Tile].Type);
                        world.RemoveStatic(x + pos.X, y + pos.Y);
                    }
                    else if (t[x, y] == 4)
                    {
                        world.UpdateTile(x + pos.X, y + pos.Y, Resources.Id2Tile[Stone].Type);
                        world.RemoveStatic(x + pos.X, y + pos.Y);
                    }
                    else if (t[x, y] == 5)
                    {
                        world.UpdateTile(x + pos.X, y + pos.Y, Resources.Id2Tile[Stone].Type);
                        world.UpdateStatic(x + pos.X, y + pos.Y, Resources.Id2Object[PillarA].Type);
                    }
                    else if (t[x, y] == 6)
                    {
                        world.UpdateTile(x + pos.X, y + pos.Y, Resources.Id2Tile[Stone].Type);
                        world.UpdateStatic(x + pos.X, y + pos.Y, Resources.Id2Object[PillarB].Type);
                    }
                }

            var entity = Entity.Resolve(Resources.Id2Object["Skull Shrine"].Type);
            world.AddEntity(entity, new Vector2(pos.X + Size / 2f, pos.Y + Size / 2f));
        }
    }
}
