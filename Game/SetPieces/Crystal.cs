using RotMG.Common;

namespace RotMG.Game.SetPieces
{
    class Crystal: ISetPiece
    {
        public int Size { get { return 5; } }
        
        static readonly string Floor = "Rock";
        static readonly byte[,] Circle =
        {
            {0,0,1,0,0},
            {0,1,1,1,0},
            {1,1,2,1,1},
            {0,1,1,1,0},
            {0,0,1,0,0}
        };

        public void RenderSetPiece(World world, IntPoint pos)
        {
            for (var x = 0; x < 5; x++)
                for (var y = 0; y < 5; y++)
                {
                    if (Circle[x, y] == 1)
                    {
                        world.UpdateTile(x + pos.X, y + pos.Y, Resources.Id2Tile[Floor].Type);
                        world.RemoveStatic(x + pos.X, y + pos.Y);
                    }
                    else if (Circle[x, y] == 2)
                    {
                        world.UpdateTile(x + pos.X, y + pos.Y, Resources.Id2Tile[Floor].Type);
                        world.RemoveStatic(x + pos.X, y + pos.Y);
                        var entity = Entity.Resolve(Resources.Id2Object["Mysterious Crystal"].Type);
                        world.AddEntity(entity, new Vector2(pos.X + x + .5f, pos.Y + y + .5f));
                    }
                }
        }
    }
}
