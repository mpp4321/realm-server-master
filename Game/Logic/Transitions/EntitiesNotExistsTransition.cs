using RotMG.Common;
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

        public EntitiesNotExistsTransition(float dist, string targetState, params string[] targets)
            : base(targetState)
        {
            _dist = dist;

            if (targets.Length <= 0)
                return;

            _targets = targets
                .Select(a => Resources.Id2Object[a].Type)
                .ToArray();
        }

        public override bool Tick(Entity host)
        {
            if (_targets == null)
                return false;

            return _targets.All(t => GameUtils.GetNearestEntity(host, _dist, t) == null);
        }

    }
}
