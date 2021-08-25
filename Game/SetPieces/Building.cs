using RotMG.Common;
using RotMG.Utils;

namespace RotMG.Game.SetPieces
{
    class Building : ISetPiece
    {
        public int Size { get { return 21; } }

        static readonly string Floor = "Brown Lines";
        static readonly string Wall = "Wooden Wall";

        
        public void RenderSetPiece(World world, IntPoint pos)
        {
            int w = MathUtils.NextInt(19, 22), h = MathUtils.NextInt(19, 22);
            var t = new int[w, h];
            for (var x = 0; x < w; x++)                     //Perimeter
            {
                t[x, 0] = 1;
                t[x, h - 1] = 1;
            }
            for (var y = 0; y < h; y++)
            {
                t[0, y] = 1;
                t[w - 1, y] = 1;
            }

            var midPtH = h / 2 + MathUtils.NextInt(-2, 3);          //Mid hori wall
            var sepH = MathUtils.NextInt(2, 4);
            if (MathUtils.Chance(.5f))
            {
                for (var x = sepH; x < w; x++)
                    t[x, midPtH] = 1;
            }
            else
            {
                for (var x = 0; x < w - sepH; x++)
                    t[x, midPtH] = 1;
            }

            int begin, end;
            if (MathUtils.Chance(.5f))
            {
                begin = 0; end = midPtH;
            }
            else
            {
                begin = midPtH; end = h;
            }

            var midPtV = w / 2 + MathUtils.NextInt(-2, 3);          //Mid vert wall
            var sepW = MathUtils.NextInt(2, 4);
            if (MathUtils.Chance(.5f))
            {
                for (var y = begin + sepW; y < end; y++)
                    t[midPtV, y] = 1;
            }
            else
            {
                for (var y = begin; y < end - sepW; y++)
                    t[midPtV, y] = 1;
            }
            for (var x = 0; x < w; x++)                     //Flooring
                for (var y = 0; y < h; y++)
                    if (t[x, y] == 0)
                        t[x, y] = 2;

            for (var x = 0; x < w; x++)                     //Corruption
                for (var y = 0; y < h; y++)
                    if (MathUtils.Chance(.5f))
                        t[x, y] = 0;

            var rotation = MathUtils.NextInt(0, 4);                 //Rotation
            for (var i = 0; i < rotation; i++)
                t = SetPieces.RotateCW(t);
            w = t.GetLength(0); h = t.GetLength(1);
            
            for (var x = 0; x < w; x++)                     //Rendering
                for (var y = 0; y < h; y++)
                {
                    if (t[x, y] == 1)
                    {
                        world.UpdateStatic(x + pos.X, y + pos.Y, Resources.Id2Object[Wall].Type);
                    }
                    else if (t[x, y] == 2)
                    {
                        world.UpdateTile(x + pos.X, y + pos.Y, Resources.Id2Tile[Floor].Type);
                        world.RemoveStatic(x + pos.X, y + pos.Y);
                    }
                }
        }
    }
}
