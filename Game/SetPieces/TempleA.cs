using System;
using System.Linq;
using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Utils;

namespace RotMG.Game.SetPieces
{
    class TempleA : Temple
    {
        public override int Size { get { return 60; } }

        
        public override void RenderSetPiece(World world, IntPoint pos)
        {
            var t = new int[Size, Size];
            var o = new int[Size, Size];

            for (var x = 0; x < 60; x++)                    //Flooring
                for (var y = 0; y < 60; y++)
                {
                    if (Math.Abs(x - Size / 2) / (Size / 2.0) + MathUtils.NextFloat() * 0.3 < 0.9 &&
                        Math.Abs(y - Size / 2) / (Size / 2.0) + MathUtils.NextFloat() * 0.3 < 0.9)
                    {
                        var dist = Math.Sqrt(((x - Size / 2) * (x - Size / 2) + (y - Size / 2) * (y - Size / 2)) / ((Size / 2.0) * (Size / 2.0)));
                        t[x, y] = MathUtils.NextFloat() < (1 - dist) * (1 - dist) ? 2 : 1;
                    }
                }

            for (var x = 0; x < Size; x++)                  //Corruption
                for (var y = 0; y < Size; y++)
                    if (MathUtils.Chance(.02f))
                        t[x, y] = 0;

            const int bas = 17;                             //Walls
            for (var x = 0; x < 20; x++)
            {
                o[bas + x, bas] = x == 19 ? 2 : 1;
                o[bas + x, bas + 1] = 2;
            }
            for (var y = 0; y < 20; y++)
            {
                o[bas, bas + y] = y == 19 ? 2 : 1;
                if (y != 0)
                    o[bas + 1, bas + y] = 2;
            }
            for (var x = 0; x < 19; x++)
            {
                o[bas + 8 + x, bas + 25] = 2;
                o[bas + 8 + x, bas + 26] = x == 0 ? 2 : 1;
            }
            for (var y = 0; y < 19; y++)
            {
                if (y != 18)
                    o[bas + 25, bas + 8 + y] = 2;
                o[bas + 26, bas + 8 + y] = y == 0 ? 2 : 1;
            }
            o[bas + 5, bas + 5] = 3;                        //Columns
            o[bas + 21, bas + 5] = 3;
            o[bas + 5, bas + 21] = 3;
            o[bas + 21, bas + 21] = 3;
            o[bas + 9, bas + 9] = 3;
            o[bas + 17, bas + 9] = 3;
            o[bas + 9, bas + 17] = 3;
            o[bas + 17, bas + 17] = 3;

            for (var x = 0; x < Size; x++)                  //Plants
                for (var y = 0; y < Size; y++)
                {
                    if (((x > 5 && x < bas) || (x < Size - 5 && x > Size - bas) ||
                         (y > 5 && y < bas) || (y < Size - 5 && y > Size - bas)) &&
                        o[x, y] == 0 && t[x, y] == 1)
                    {
                        double r = MathUtils.NextFloat();
                        if (r > 0.6)        //0.4
                            o[x, y] = 4;
                        else if (r > 0.35)  //0.25
                            o[x, y] = 5;
                        else if (r > 0.33)  //0.02
                            o[x, y] = 6;
                    }
                }

            int rotation = MathUtils.Next(4);               //Rotation
            for (var i = 0; i < rotation; i++)
            {
                t = SetPieces.RotateCW(t);
                o = SetPieces.RotateCW(o);
            }

            Render(this, world, pos, t, o);

            //Boss & Chest

            var c = new Container(0x0501, -1, null);
            var loot = chest.GetLoots(3, 8).ToArray();
            for (var k = 0; k < loot.Length; k++)
            {
                var roll = Resources.Type2Item[loot[k]].Roll();
                c.Inventory[k] = loot[k];
                c.ItemDatas[k] = roll.Item1 ? (int) roll.Item2 : -1;
                c.UpdateInventorySlot(k);
            }

            world.AddEntity(c, new Vector2(pos.X + Size / 2, pos.Y + Size / 2));

            var snake = Entity.Resolve(0x0dc2);
            world.AddEntity(snake, new Vector2(pos.X + Size / 2, pos.Y + Size / 2));
        }
    }
}
