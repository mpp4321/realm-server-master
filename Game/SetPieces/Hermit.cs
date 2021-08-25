using RotMG.Common;

namespace RotMG.Game.SetPieces
{
    class Hermit : ISetPiece
    {
        public int Size { get { return 32; } }

        public void RenderSetPiece(World world, IntPoint pos)
        {
            SetPieces.RenderFromMap(world, pos, Resources.SetPieces["Hermit God"]);
        }
    }
}
