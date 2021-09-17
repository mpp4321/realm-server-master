using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.Behaviors
{
    class OnDeath : Behavior
    {
        private readonly Action<Entity> _f;

        public OnDeath(Action<Entity> f) : base()
        {
            _f = f;
        }

        public override void Death(Entity host)
        {
            _f(host);
        }
    }
}
