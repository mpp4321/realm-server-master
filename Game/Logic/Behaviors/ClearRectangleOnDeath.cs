using RotMG.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.Behaviors
{
    class ClearRectangleOnDeath : Behavior
    {

        private readonly IntPoint p1, p2;

        public ClearRectangleOnDeath(IntPoint p1, IntPoint p2)
        {
            this.p1 = p1;
            this.p2 = p2;
        }

        public override void Death(Entity host)
        {
            for(int x = p1.X; x <= p2.X; x++)
            {
                for(int y = p1.Y; y <= p2.Y; y++)
                {
                    host.Parent.RemoveStatic(x, y);
                }
            }
        }

    }
}
