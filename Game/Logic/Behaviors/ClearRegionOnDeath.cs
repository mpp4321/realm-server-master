using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.Behaviors
{
    class ClearRegionOnDeath : Behavior
    {

        private readonly Region _region;
        private readonly float _radius;
        public ClearRegionOnDeath(Region region, float radius=0.0f)
        {
            _region = region;
            _radius = radius;
        }

        public override void Death(Entity host)
        {
            foreach(var point in host.Parent.GetAllRegion(_region))
            {
                if (_radius > 0.0f)
                {
                    var dist = MathUtils.Distance(host.Position, point.ToVector2());
                    if (dist > _radius)
                    {
                        continue;
                    }
                }
                host.Parent.RemoveStatic(point.X, point.Y);
            }
        }

    }
}
