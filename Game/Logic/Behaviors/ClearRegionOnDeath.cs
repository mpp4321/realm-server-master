using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.Behaviors
{
    class ClearRegionOnDeath : Behavior
    {

        private readonly Region _region;
        public ClearRegionOnDeath(Region region)
        {
            _region = region;
        }

        public override void Death(Entity host)
        {
            foreach(var point in host.Parent.GetAllRegion(_region))
            {
                host.Parent.RemoveStatic(point.X, point.Y);
            }
        }

    }
}
