using RotMG.Common;
using RotMG.Game.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.Behaviors
{
    class Transform : Behavior
    {

        //State storage: none

        ushort target;
        public Transform(string target)
        {
            this.target = Resources.Id2Object[target].Type;
        }

        public override bool Tick(Entity host)
        {
            Entity entity = Entity.Resolve(target);
            if (entity is Portal
              && host.Parent.Name.Contains("Arena"))
            {
                return false;
            }
            Manager.StartOfTickAction(() =>
            {
                host?.Parent?.AddEntity(entity, host.Position);
                host?.Parent?.RemoveEntity(host);
            });
            return true;
        }

    }
}
