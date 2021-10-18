using RotMG.Common;
using RotMG.Game.SetPieces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.Behaviors
{
    class SetPieceOnEnter : Behavior
    {

        private readonly IntPoint Point;
        private readonly string Piece;

        public SetPieceOnEnter(IntPoint p, string setpieceid)
        {
            this.Point = p;
            this.Piece = setpieceid;
        }

        public override void Enter(Entity host)
        {
            Resources.SetPieces[Piece].ProjectOntoWorld(host.Parent, Point);
        }

    }
}
