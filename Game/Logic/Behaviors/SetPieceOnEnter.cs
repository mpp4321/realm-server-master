using RotMG.Common;
using RotMG.Game.SetPieces;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.Behaviors
{
    class SetPieceOnEnter : Behavior
    {

        private readonly ISetPiece SetPiece;
        private readonly Map SetPieceAsMap;
        private readonly IntPoint Point;
        private readonly bool Relative;
        private readonly bool Revert;

        public SetPieceOnEnter(IntPoint p, ISetPiece setPiece=null, string setpieceid=null, bool relative=false, bool revert=false)
        {
            this.Point = p;
            this.SetPiece = setPiece;
            this.SetPieceAsMap = setpieceid == null ? null : Resources.SetPieces[setpieceid];
            this.Relative = relative;
            this.Revert = revert;
        }

        public override void Enter(Entity host)
        {
            if(SetPiece != null) {
                SetPiece.RenderSetPiece(host.Parent, Relative ? (Point.ToVector2() + host.Position).ToIntPoint() : Point);
            } else {
                SetPieceAsMap.ProjectOntoWorld(host.Parent, Relative ? (Point.ToVector2() + host.Position).ToIntPoint() : Point);
            }
        }

    }
}
