using RotMG.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.Behaviors
{
    class TransformOnDeath : Behavior
    {
        public static Random _Random = new Random();
        ushort target;
        int min;
        int max;
        float probability;

        public TransformOnDeath(string target, int min = 1, int max = 1, double probability = 1)
        {
            this.target = Resources.Id2Object[target].Type;
            this.min = min;
            this.max = max;
            this.probability = (float)probability;
        }

        public override void Death(Entity host)
        {
            if (_Random.NextDouble() < probability)
            {
                int count = _Random.Next(min, max + 1);
                for (int i = 0; i < count; i++)
                {
                    Entity entity = Entity.Resolve(target);
                    entity.Parent = host.Parent;
                    //entity.Parent.MoveEntity(entity, host.Position);
                    host.Parent.AddEntity(entity, host.Position);
                }
            }
        }
    }
}
