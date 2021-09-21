using RotMG.Common;
using RotMG.Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.logic;

namespace RotMG.Game.Logic.Behaviors
{
    class Reproduce : Behavior
    {
        //State storage: cooldown timer

        private readonly static Random _Random = new Random();

        private readonly float _densityRadius;
        private readonly int _densityMax;
        private readonly ushort? _children;
        private Cooldown _coolDown;
        private readonly Region _region;
        private readonly double _regionRange;
        private List<IntPoint> _reproduceRegions;

        public Reproduce(string children = null,
            float densityRadius = 10,
            int densityMax = 5,
            Cooldown cooldown = new Cooldown(),
            Region region = Region.None,
            double regionRange = 10)
        {
            _children = children == null ? null : (ushort?) Resources.Id2Object[children].Type;
            _densityRadius = densityRadius;
            _densityMax = densityMax;
            _coolDown = cooldown.Normalize(60000);
            _region = region;
            _regionRange = regionRange;
        }

        public override void Enter(Entity host) {
            host.StateCooldown[Id] = _coolDown.Next(_Random);

            if (_region == Region.None)
                return;

            var map = host.Parent.Map;

            var w = map.Width;
            var h = map.Height;

            _reproduceRegions = new List<IntPoint>();

            for (var y = 0; y < h; y++)
                for (var x = 0; x < w; x++)
                {
                    if (!map.Regions[_region].Contains(new IntPoint(x, y)))
                        continue;

                    _reproduceRegions.Add(new IntPoint(x, y));
                }
        }
        /// <returns>true if behavior complete</returns>

        public override bool Tick(Entity host)
        {
            //setpiece seem to have entering issues
            if(!(host.StateCooldown.ContainsKey(Id)))
            {
                host.StateCooldown[Id] = 0;
            }
            host.StateCooldown[Id] -= Settings.MillisecondsPerTick;
            if (host.StateCooldown[Id] <= 0)
            {
                var count = host.CountEntity(_densityRadius, _children ?? host.GetObjectDefinition().ObjectType);

                if (count < _densityMax)
                {
                    float targetX = host.Position.X;
                    float targetY = host.Position.Y;

                    if (_reproduceRegions != null && _reproduceRegions.Count > 0)
                    {
                        var sx = (int)host.Position.X;
                        var sy = (int)host.Position.Y;
                        var regions = _reproduceRegions
                            .Where(p => Math.Abs(sx - p.X) <= _regionRange &&
                                        Math.Abs(sy - p.Y) <= _regionRange).ToList();
                        var tile = regions[_Random.Next(regions.Count)];
                        targetX = tile.X;
                        targetY = tile.Y;
                    }

                    var entity = Entity.Resolve(_children ?? host.GetObjectDefinition().ObjectType);

                    var enemyHost = host as Enemy;
                    var enemyEntity = entity as Enemy;

                    if (enemyHost != null && enemyEntity != null)
                    {
                        enemyEntity.Terrain = enemyHost.Terrain;
                        //if (enemyHost.Spawned)
                        //{
                        //    enemyEntity.Spawned = true;
                        //    enemyEntity.ApplyConditionEffect(new ConditionEffect()
                        //    {
                        //        Effect = ConditionEffectIndex.Invisible,
                        //        DurationMS = -1
                        //    });
                        //}
                    }

                    host.Parent.AddEntity(entity, new Vector2(targetX, targetY));
                    host.StateCooldown[Id] = _coolDown.Next(_Random);
                }
            }
            return true;
        }
    }
}
