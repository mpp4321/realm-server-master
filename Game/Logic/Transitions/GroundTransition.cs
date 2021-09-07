using RotMG.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.Transitions
{
    class GroundTransition : Transition
    {
        //State storage: none

        private readonly string _ground;
        private ushort? _groundType;

        public GroundTransition(string ground, string targetState)
            : base(targetState)
        {
            _ground = ground;
        }

        public override bool Tick(Entity host)
        {
            if (_groundType == null)
                _groundType = Resources.Id2Tile[_ground].Type;

            var tile = host.Parent.Map.Tiles[(int)host.Position.X, (int)host.Position.Y];

            return tile.GroundType == _groundType;
        }
    }
}
