using System;
using System.Collections.Generic;
using RotMG.Common;
using RotMG.Utils;

namespace RotMG.Game.SetPieces
{
    class Grove : ISetPiece
    {
        public int Size
        {
            get { return 25; }
        }

        static readonly string Floor = "Light Grass";
        static readonly string Tree = "Cherry Tree";
        
        public void RenderSetPiece(World world, IntPoint pos)
        {
            var radius = MathUtils.NextInt(Size - 5, Size + 1) / 2;
            var border = new List<IntPoint>();

            var t = new int[Size, Size];
            for (var y = 0; y < Size; y++)
                for (var x = 0; x < Size; x++)
                {
                    var dx = x - (Size / 2.0);
                    var dy = y - (Size / 2.0);
                    var r = Math.Sqrt(dx * dx + dy * dy);
                    if (r <= radius)
                    {
                        t[x, y] = 1;
                        if (radius - r < 1.5)
                            border.Add(new IntPoint(x, y));
                    }
                }

            var trees = new HashSet<IntPoint>();
            while (trees.Count < border.Count * 0.5)
                trees.Add(border[MathUtils.Next(border.Count)]);

            foreach (var i in trees)
                t[i.X, i.Y] = 2;
            
            for (var x = 0; x < Size; x++)
                for (var y = 0; y < Size; y++)
                {
                    if (t[x, y] == 1)
                    {
                        world.UpdateTile(x + pos.X, y + pos.Y, Resources.Id2Tile[Floor].Type);
                        world.RemoveStatic(x + pos.X, y + pos.Y);
                    }
                    else if (t[x, y] == 2)
                    {
                        world.UpdateTile(x + pos.X, y + pos.Y, Resources.Id2Tile[Floor].Type);
                        world.UpdateStatic(x + pos.X, y + pos.Y, Resources.Id2Object[Tree].Type);
                        world.GetTile(x + pos.X, y + pos.Y).StaticObject.Size = MathUtils.Chance(.5f) ? 120 : 140;
                    }
                }

            var entity = Entity.Resolve(Resources.Id2Object["Ent Ancient"].Type);
            entity.Size = 140;
            world.AddEntity(entity, new Vector2(pos.X + Size / 2 + 1, pos.Y + Size / 2 + 1));
        }
    }
}
