using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotMG.Game.Logic.Behaviors
{
    public class QueuedBehav : Behavior
    {
        private Behavior[] OrderedBehaviors { get; set; }
        private bool Repeat;

        public QueuedBehav( params Behavior[] behavs)
        {
            OrderedBehaviors = behavs;
            Repeat = false;
        }
        public QueuedBehav(bool repeat, params Behavior[] behavs)
        {
            OrderedBehaviors = behavs;
            Repeat = repeat;
        }

        public override bool Tick(Entity host)
        {
            int c = (int)host.StateObject[Id];
            if (c == -1) return false;

            if(OrderedBehaviors[c].Tick(host))
            {
                IncrementBehav(host);
            }

            return true;
        }

        public override void Enter(Entity host)
        {
            host.StateObject[Id] = 0;
            OrderedBehaviors[0].Enter(host);
        }

        public override void Exit(Entity host)
        {
            int c = (int) host.StateObject[Id];
            if (c == -1) return;
            OrderedBehaviors[c].Exit(host);
        }

        public void IncrementBehav(Entity host)
        {
            int c = (int) host.StateObject.GetValueOrDefault(Id, 0);
            if(c == OrderedBehaviors.Length - 1 && !Repeat)
            {
                OrderedBehaviors[c].Exit(host);
                host.StateObject[Id] = -1;
                return;
            } 
            if(c == OrderedBehaviors.Length - 1 && Repeat)
            {
                OrderedBehaviors[c].Exit(host);
                OrderedBehaviors[0].Enter(host);
                host.StateObject[Id] = 0;
            } else
            {
                OrderedBehaviors[c].Exit(host);
                OrderedBehaviors[c + 1].Enter(host);
                host.StateObject[Id] = c + 1;
            }
        }
    }
}
