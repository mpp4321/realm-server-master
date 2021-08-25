using System;
using RotMG.Common;
using RotMG.Utils;

namespace RotMG.Game.SetPieces
{
    class Tower : ISetPiece
    {
        static int[,] quarter;
        static Tower()
        {
            var s =
"............XX\n" +
"........XXXXXX\n" +
"......XXXXXXXX\n" +
".....XXXX=====\n" +
"....XXX=======\n" +
"...XXX========\n" +
"..XXX=========\n" +
"..XX==========\n" +
".XXX==========\n" +
".XX===========\n" +
".XX===========\n" +
".XX===========\n" +
"XXX===========\n" +
"XXX===========";
            var a = s.Split('\n');
            quarter = new int[14, 14];
            for (var y = 0; y < 14; y++)
                for (var x = 0; x < 14; x++)
                    quarter[x, y] =
                        a[y][x] == 'X' ? 1 :
                            (a[y][x] == '=' ? 2 : 0);
        }

        public int Size { get { return 27; } }

        static readonly string Floor = "Rock";
        static readonly string Wall = "Grey Wall";

        public void RenderSetPiece(World world, IntPoint pos)
        {
            var t = new int[27, 27];

            var q = (int[,])quarter.Clone();

            for (var y = 0; y < 14; y++)        //Top left
                for (var x = 0; x < 14; x++)
                    t[x, y] = q[x, y];

            q = SetPieces.ReflectHori(q);           //Top right
            for (var y = 0; y < 14; y++)
                for (var x = 0; x < 14; x++)
                    t[13 + x, y] = q[x, y];

            q = SetPieces.ReflectVert(q);           //Bottom right
            for (var y = 0; y < 14; y++)
                for (var x = 0; x < 14; x++)
                    t[13 + x, 13 + y] = q[x, y];

            q = SetPieces.ReflectHori(q);           //Bottom left
            for (var y = 0; y < 14; y++)
                for (var x = 0; x < 14; x++)
                    t[x, 13 + y] = q[x, y];

            for (var y = 1; y < 4; y++)             //Opening
                for (var x = 8; x < 19; x++)
                    t[x, y] = 2;
            t[12, 0] = t[13, 0] = t[14, 0] = 2;


            var r = MathUtils.Next(4);                //Rotation
            for (var i = 0; i < r; i++)
                t = SetPieces.RotateCW(t);

            t[13, 13] = 3;
            
            for (var x = 0; x < 27; x++)            //Rendering
                for (var y = 0; y < 27; y++)
                {
                    if (t[x, y] == 1)
                    {
                        world.UpdateTile(x + pos.X, y + pos.Y, Resources.Id2Tile[Floor].Type);
                        world.UpdateStatic(x + pos.X, y + pos.Y, Resources.Id2Object[Wall].Type);
                    }
                    else if (t[x, y] == 2)
                    {
                        world.UpdateTile(x + pos.X, y + pos.Y, Resources.Id2Tile[Floor].Type);
                        world.RemoveStatic(x + pos.X, y + pos.Y);
                    }

                    else if (t[x, y] == 3)
                    {
                        world.UpdateTile(x + pos.X, y + pos.Y, Resources.Id2Tile[Floor].Type);
                        world.RemoveStatic(x + pos.X, y + pos.Y);
                        
                        var ghostKing = Entity.Resolve(0x0928);
                        world.AddEntity(ghostKing, new Vector2(pos.X + x, pos.Y + y));
                    }
                }
        }
    }
}
