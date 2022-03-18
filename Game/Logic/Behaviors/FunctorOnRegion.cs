using RotMG.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotMG.Game.Logic.Behaviors
{
    public class FunctorOnRegion : Behavior
    {

        private readonly Region _region;
        private readonly Action<(Entity, IntPoint)> onEnter = null;
        private readonly Action<(Entity, IntPoint)> onDeath = null;
        private readonly Action<(Entity, IntPoint)> onTick = null;
        public FunctorOnRegion(Region region,
            Action<(Entity, IntPoint)> onEnter = null,
            Action<(Entity, IntPoint)> onDeath = null,
            Action<(Entity, IntPoint)> onTick = null)
        {
            _region = region;
            this.onEnter = onEnter;
            this.onDeath = onDeath;
            this.onTick = onTick;
        }

        public override void Death(Entity host)
        {
            if (onDeath == null) return;
            foreach(var point in host.Parent.GetAllRegion(_region))
            {
                onDeath((host, point));
            }
        }

        public override void Enter(Entity host)
        {
            if (onEnter == null) return;
            foreach(var point in host.Parent.GetAllRegion(_region))
            {
                onEnter((host, point));
            }
        }
        public override bool Tick(Entity host)
        {
            if (onTick == null) return true;
            foreach(var point in host.Parent.GetAllRegion(_region))
            {
                onTick((host, point));
            }
            return true;
        }
    }
}
