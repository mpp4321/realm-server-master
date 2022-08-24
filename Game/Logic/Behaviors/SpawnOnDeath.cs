using RotMG.Common;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotMG.Game.Logic.Behaviors
{
    public class SpawnOnDeath : Behavior
    {

        private ushort Type { get; set; }
        private float Probability { get; set; }
        private int Amount { get; set; }
        
        public SpawnOnDeath(string id, float prob, int amount=1)
        {
            Type = Resources.Id2Object[id].Type;
            Probability = prob;
            Amount = amount;
        }

        public override void Death(Entity host)
        {
            for(int i = 0; i < Amount; i++)
            {
                if(MathUtils.Chance(Probability))
                {
                    Entity ent = Entity.Resolve(Type);
                    host.Parent.AddEntity(ent, host.Position);
                }
            }
        }
    }
}
