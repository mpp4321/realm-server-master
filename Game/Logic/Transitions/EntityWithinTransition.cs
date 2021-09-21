using RotMG.Common;
using RotMG.Game;
using RotMG.Game.Logic;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoTMG.Game.Logic.Transitions
{
    class EntitiesWithinTransition : Transition
    {
        private readonly float _dist;
        private readonly ushort _entity;

        public EntitiesWithinTransition(float dist, string entity, string targetState)
            : base(targetState)
        {
            _dist = dist;
            _entity = Resources.Id2Object[entity].Type;
        }

        public override bool Tick(Entity host)
        {
            return GameUtils.GetNearestEntity(host, _dist, _entity) != null;
        }
    }
}
