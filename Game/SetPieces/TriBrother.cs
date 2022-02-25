using RotMG.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotMG.Game.SetPieces
{
    public class TriBrother : ISetPiece
    {
        public int Size => 26;

        public void RenderSetPiece(World world, IntPoint pos)
        {
            SetPieces.RenderFromMap(world, pos, Resources.SetPieces["TriBrother"]);
        }
    }
}
