using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RotMG.Common;
using RotMG.Game.Worlds;
using RotMG.Utils;

namespace RotMG.Game.SetPieces
{
    public interface ISetPiece
    {
        int Size { get; }
        void RenderSetPiece(World world, IntPoint pos);
    }

    public class SetPieces
    {
        private struct Rect
        {
            public int X;
            public int Y;
            public int W;
            public int H;

            public static bool Intersects(Rect r1, Rect r2)
            {
                return !(r2.X > r1.X + r1.W || r2.X + r2.W < r1.X || r2.Y > r1.Y + r1.H || r2.Y + r2.H < r1.Y);
            }
        }

        private static Tuple<string, ISetPiece, int, int, Terrain[]> SetPiece(string criticalEnemy, ISetPiece piece, int min, int max,
            params Terrain[] terrains)
        {
            return Tuple.Create(criticalEnemy, piece, min, max, terrains);
        }

        private static readonly List<Tuple<string, ISetPiece, int, int, Terrain[]>> setPieces =
            new List<Tuple<string, ISetPiece, int, int, Terrain[]>>()
            {
                SetPiece("", new Building(), 80, 100, Terrain.LowForest, Terrain.LowPlains, Terrain.MidForest),
                SetPiece("", new Graveyard(), 5, 10, Terrain.LowSand, Terrain.LowPlains),
                SetPiece("Ent Ancient", new Grove(), 17, 25, Terrain.MidForest, Terrain.MidPlains),
                SetPiece("Lich", new LichyTemple(), 4, 7, Terrain.MidForest, Terrain.MidPlains),
                SetPiece("Cyclops God", new Castle(), 4, 7, Terrain.HighForest, Terrain.HighPlains),
                SetPiece("Ghost King", new Tower(), 8, 15, Terrain.HighForest, Terrain.HighPlains),
                SetPiece("", new TempleA(), 10, 20, Terrain.MidForest, Terrain.MidPlains),
                SetPiece("", new TempleB(), 10, 20, Terrain.MidForest, Terrain.MidPlains),
                SetPiece("Oasis Giant", new Oasis(), 0, 5, Terrain.LowSand, Terrain.MidSand),
                SetPiece("Phoenix Lord", new Pyre(), 0, 5, Terrain.MidSand, Terrain.HighSand),
                SetPiece("Red Demon",new LavaFissure(), 3, 5, Terrain.Mountains),
                SetPiece("", new Crystal(), 1, 1, Terrain.Mountains),
                SetPiece("", new KageKami(), 2, 3, Terrain.HighForest, Terrain.HighPlains)
            };

        public static int[,] RotateCW(int[,] mat)
        {
            var m = mat.GetLength(0);
            var n = mat.GetLength(1);
            var ret = new int[n, m];
            for (var r = 0; r < m; r++)
            {
                for (var c = 0; c < n; c++)
                {
                    ret[c, m - 1 - r] = mat[r, c];
                }
            }

            return ret;
        }

        public static int[,] ReflectVert(int[,] mat)
        {
            var m = mat.GetLength(0);
            var n = mat.GetLength(1);
            var ret = new int[m, n];
            for (var x = 0; x < m; x++)
            for (var y = 0; y < n; y++)
                ret[x, n - y - 1] = mat[x, y];
            return ret;
        }

        public static int[,] ReflectHori(int[,] mat)
        {
            var m = mat.GetLength(0);
            var n = mat.GetLength(1);
            var ret = new int[m, n];
            for (var x = 0; x < m; x++)
            for (var y = 0; y < n; y++)
                ret[m - x - 1, y] = mat[x, y];
            return ret;
        }

        public static Dictionary<string, int> ApplySetPieces(Realm world)
        {
            var map = world.Map;
            
            var rects = new HashSet<Rect>();
            var spawns = new Dictionary<string, int>();
            foreach (var dat in setPieces)
            {
                var size = dat.Item2.Size;
                var count = MathUtils.NextInt(dat.Item3, dat.Item4);
                for (var i = 0; i < count; i++)
                {
                    var pt = new IntPoint();
                    Rect rect;

                    var max = 50;
                    do
                    {
                        pt.X = MathUtils.Next(world.Width);
                        pt.Y = MathUtils.Next(world.Height);
                        rect = new Rect() { X = pt.X, Y = pt.Y, W = size, H = size };
                        max--;
                    } while ((Array.IndexOf(dat.Item5, map.Tiles[pt.X, pt.Y].Terrain) == -1 ||
                              rects.Any(_ => Rect.Intersects(rect, _))) && max > 0);

                    if (max <= 0) continue;
                    dat.Item2.RenderSetPiece(world, pt);
                    rects.Add(rect);
                    if (!string.IsNullOrEmpty(dat.Item1))
                    {
                        if (!spawns.ContainsKey(dat.Item1))
                            spawns[dat.Item1] = 0;
                        spawns[dat.Item1]++;
                    }
                }
            }

            return spawns;
        }

        public static void RenderFromMap(World world, IntPoint pos, Map map)
        {
            map.ProjectOntoWorld(world, pos);
        }
    }
}