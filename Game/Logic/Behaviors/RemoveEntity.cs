using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotMG.Game.Logic.Behaviors
{
    public class RemoveEntity : Behavior
    {

        private float Range { get; set; }
        private string Entity { get; set; }

        public RemoveEntity(float range, string entity)
        {
            Range = range;
            Entity = entity;
        }

        public override void Enter(Entity host)
        {
            foreach(var entity in host.GetNearbyEntities(Range).Where(
                    a => a.Desc.Id.ToLower() == Entity.ToLower()
                )) {
                host.Parent.RemoveEntity(entity);
            }
        }

    }
}
