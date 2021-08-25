using RotMG.Common;

namespace RotMG.Game.SetPieces
{
    class KageKami : ISetPiece
    {
        public int Size { get { return 65; } }

        public void RenderSetPiece(World world, IntPoint pos)
        {
            SetPieces.RenderFromMap(world, pos, Resources.SetPieces["Kage Kami"]);
        }
    }
}
