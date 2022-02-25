using RotMG.Common;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotMG.Game.Worlds
{
    public class Maze : World
    {
        private ushort Wall = 0x6316;
        public Maze(Map map, WorldDesc desc) : base(map, desc)
        {
            GenerateMaze(map.Width, map.Height);
        }

        public List<IntPoint> GetNeighbors(IntPoint p)
        {
            return new List<IntPoint>()
            {
                p + new IntPoint(-1, 0),
                p + new IntPoint(1, 0),
                p + new IntPoint(0, 1),
                p + new IntPoint(0, -1),
            };
        }

        public List<IntPoint> GetCorners(IntPoint p)
        {
            return new List<IntPoint>()
             {
                 p + new IntPoint(-1, 1),
                 p + new IntPoint(1, 1),
                 p + new IntPoint(-1, -1),
                 p + new IntPoint(1, -1),
             };
        }

        public IntPoint RandomFrom(IEnumerable<IntPoint> points)
        {
            var list = points.ToList();
            var count = list.Count();
            return list[MathUtils.Next(count)];
        }

        public void GenerateMaze(int width, int height)
        {
            IntPoint current_point = new(0, 0);
            IntPoint end_point = new(width, height);
            HashSet<IntPoint> closed_set = new();
            // Defaults to 0
            Dictionary<IntPoint, byte> open_set = new();
            open_set.Add(current_point, 0);
            //points with still valid neighbors
            HashSet<IntPoint> paths = new();
            paths.Add(new(0, 0));

            var random = MathUtils.GetStaticRandom();
            var last_direction = new IntPoint(1, 0);
            while (open_set.Count() > 0)
            {
                var neighbors = GetNeighbors(current_point).Where(
                    a => !closed_set.Contains(a) && (a.X >= 0 && a.Y >= 0 && a.X < width && a.Y < height)
                ).ToList();

                foreach(var n in neighbors)
                    open_set.TryAdd(n, 0);

                // Choices valid after wall checking
                var valid_choices = new List<IntPoint>();
                foreach(var ipoint in neighbors)
                {
                    // This generates the walls
                    if (ipoint == end_point)
                    {
                        valid_choices.Add(ipoint);
                        continue;
                    }

                    open_set[ipoint] += 1;
                    if(open_set[ipoint] == 2)
                    {
                        open_set.Remove(ipoint);
                        closed_set.Add(ipoint);
                        UpdateStatic(ipoint.X, ipoint.Y, Wall);
                    } else
                    {
                        valid_choices.Add(ipoint);
                    }
                }

                open_set.Remove(current_point);
                closed_set.Add(current_point);

                if(valid_choices.Count() == 0)
                {
                    if (open_set.Count() == 0) continue;
                    current_point = RandomFrom(open_set.Keys);
                    continue;
                }

                IntPoint random_point = RandomFrom(valid_choices);
                current_point = random_point;
            }
        }

    }
}
