﻿using RotMG.Common;
using RotMG.Utils;
using System.Collections.Generic;
using System.Linq;

namespace RotMG.Game.Logic.Behaviors
{
    class OrderOnEntry : Behavior
    {
        //State storage: none

        private readonly float _range;
        private readonly ushort _children;
        private readonly string _targetStateName;

        public OrderOnEntry(float range, string children, string targetState)
        {
            _range = range;
            _children = Resources.Id2Object[children].Type;
            _targetStateName = targetState;
        }

        public override void Enter(Entity host)
        {
            foreach (var i in host.Parent.EntityChunks.HitTest(host.Position, _range).Where(z => z.GetObjectDefinition().ObjectType == _children))
            {
                OrderOnDeath.ChangeStateTree(i, _targetStateName);
            }
        }
    }
}
