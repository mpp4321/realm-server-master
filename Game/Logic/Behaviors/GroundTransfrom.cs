using RotMG.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.Behaviors
{
    class GroundTransform : Behavior
    {

        // host.StateObject[Id] object: TileState
        class TileState
        {
            public ushort TileType;
            public int X;
            public int Y;
            public bool Spawned;
        }

        private readonly string _tileId;
        private readonly int _radius;
        private readonly bool _persist;
        private readonly int? _relativeX;
        private readonly int? _relativeY;

        public GroundTransform(
            string tileId,
            int radius = 0,
            int? relativeX = null,
            int? relativeY = null,
            bool persist = false)
        {
            _tileId = tileId;
            _radius = radius;
            _persist = persist;
            _relativeX = relativeX;
            _relativeY = relativeY;
        }

        public override void Enter(Entity host)
        {
            var map = host.Parent.Map;
            var hx = (int)host.Position.X;
            var hy = (int)host.Position.Y;

            var tileType = Resources.Id2Tile[_tileId].Type;

            var tiles = new List<TileState>();

            if (_relativeX != null && _relativeY != null)
            {
                var x = hx + (int)_relativeX;
                var y = hy + (int)_relativeY;

                if (!map.IsWithin(new IntPoint(x, y)))
                    return;

                var tile = map.Tiles[x, y];

                if (tileType == tile.GroundType)
                    return;

                tiles.Add(new TileState()
                {
                    TileType = tile.GroundType,
                    X = x,
                    Y = y
                });

                host.Parent.UpdateTile(x, y, tileType);
                return;
            }

            for (int i = hx - _radius; i <= hx + _radius; i++)
                for (int j = hy - _radius; j <= hy + _radius; j++)
                {
                    if (!map.IsWithin(new IntPoint(i, j)))
                        continue;

                    var tile = map.Tiles[i, j];

                    if (tileType == tile.GroundType)
                        continue;

                    tiles.Add(new TileState()
                    {
                        TileType = tile.GroundType,
                        X = i,
                        Y = j
                    });

                    host.Parent.UpdateTile(i, j, tileType);
                }

            host.StateObject[Id] = tiles;
        }

        public override void Exit(Entity host)
        {
            var tiles = host.StateObject[Id] as List<TileState>;

            if (tiles == null || _persist)
                return;

            foreach (var tile in tiles)
            {
                var x = tile.X;
                var y = tile.Y;
                var tileType = tile.TileType;
                host.Parent.UpdateTile(x, y, tileType);
            }
        }

    }
}
