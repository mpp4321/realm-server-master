using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RotMG.Game.Logic.Transitions
{
    class EntitiesNotExistsTransition : Transition
    {
        private readonly float _dist;
        private readonly ushort[] _targets;
        private readonly bool allEntities = false;

        public EntitiesNotExistsTransition(float dist, string targetState, params string[] targets)
            : base(targetState)
        {
            _dist = dist;
            allEntities = _dist > 100;

            if (targets.Length <= 0)
                return;

            _targets = targets
                .Select(a => Resources.Id2Object[a].Type)
                .ToArray();
        }

        private int rateLimit = 0;

        public override bool Tick(Entity host)
        {
            if (_targets == null)
                return false;

            if ((rateLimit++ % 4) != 0) return false;

            if(allEntities)
            {
                return host.Parent.Entities.Select(a => a.Value).OfType<Enemy>().All(a => !_targets.Contains(a.Type));
            }
            return _targets.All(t => GameUtils.GetNearestEntity(host, _dist, t) == null);
        }

    }
}
