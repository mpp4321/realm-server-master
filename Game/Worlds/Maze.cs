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
        private ushort Wall = 0x0d70;
        public Maze(Map map, WorldDesc desc) : base(map, desc)
        {
            GenerateMaze(0, 0, 16, 16, new int[] {});
        }

        public void GenerateMaze(int x, int y, int x1, int y1, int[] validEdges)
        {
            int inner_x = x;// + 2;
            int outer_x = x1;// - 1;
            int inner_y = y;// + 2;
            int outer_y = y1;// - 1;
            if(outer_x - inner_x < 6 || outer_y - inner_y < 6)
            {
                return;
            }
            IntPoint random_point = new IntPoint(
                MathUtils.NextInt(inner_x, outer_x - 1),
                MathUtils.NextInt(inner_y, outer_y - 1)
            );
            for(int i = 0; i < 4; i++)
            {
                // Generate the inner rects from random_point
                int tx = 0, ty = 0, tx1 = 0, ty1 = 0;
                int[] vEdges = new int[] { }; 
                switch(i)
                {
                    case 0:
                        tx = x;
                        ty = y;
                        tx1 = random_point.X;
                        ty1 = random_point.Y;
                        vEdges = new int[] { 2, 3 };
                        break;
                    case 1:
                        tx = random_point.X;
                        ty = y;
                        tx1 = x1;
                        ty1 = random_point.Y;
                        vEdges = new int[] { 0, 3 };
                        break;
                    case 2:
                        tx = random_point.X;
                        ty = random_point.Y;
                        tx1 = x1;
                        ty1 = y1;
                        vEdges = new int[] { 0, 1 };
                        break;
                    case 3:
                        tx = x;
                        ty = random_point.Y;
                        tx1 = random_point.X;
                        ty1 = y1;
                        vEdges = new int[] { 1, 2 };
                        break;
                }
                CreateBox(tx, ty, tx1, ty1);
                GenerateMaze(tx, ty, tx1, ty1, vEdges);
            }
        }

        public void CreateBox(int x, int y, int x1, int y1)
        {
            for(int i = x; i < x1; i++)
            {
                for(int j = y; j < y1; j++)
                {
                    if (i == x || j == y || j == y1 - 1 || i == x1 - 1)
                    {
                        UpdateStatic(i, j, Wall);
                    }
                }
            }
        }
    }
}
