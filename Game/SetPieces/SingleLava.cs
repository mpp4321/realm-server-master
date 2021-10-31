using RotMG.Common;

namespace RotMG.Game.SetPieces
{
    class SingleLava : ISetPiece
    {
        public int Size { get { return 1; } }

        public void RenderSetPiece(World world, IntPoint pos)
        {
            world.UpdateTile(pos.X, pos.Y, Resources.Id2Tile["Hot Lava"].Type);
        }
    }
}
