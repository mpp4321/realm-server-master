using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Networking;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using wServer.logic;

namespace RotMG.Game.Logic.Behaviors
{
    class TossObject : Behavior
    {
        //State storage: cooldown timer

        private static Random Random = new Random();

        private readonly float _range;
        private readonly float? _angle;
        private Cooldown _coolDown;
        private readonly int _coolDownOffset;
        private readonly bool _tossInvis;
        private readonly double _probability;
        private readonly ushort[] _children;
        private readonly float? _minRange;
        private readonly float? _maxRange;
        private readonly float? _minAngle;
        private readonly float? _maxAngle;
        private readonly float? _densityRange;
        private readonly int? _maxDensity;
        private readonly string _group;
        private readonly Region _region;
        private readonly double _regionRange;
        private List<IntPoint> _reproduceRegions;
        private readonly bool _throwEffect;

        public TossObject(string child, float range = 5, float? angle = null,
            Cooldown coolDown = new Cooldown(), int coolDownOffset = 0, 
            bool tossInvis = false, float probability = 1, string group = null,
            float? minAngle = null, float? maxAngle = null,
            float? minRange = null, float? maxRange = null,
            float? densityRange = null, int? maxDensity = null,
            Region region = Region.None, float regionRange = 10,
            bool throwEffect = false
            )
        {
            if (group == null)
                _children = new ushort[] { Resources.Id2Object[child].Type };
            else
                _children = Resources.Id2Object.Values
                .Where(x => x.Group == group)
                .Select(x => x.Type).ToArray();
            
            _range = range;
            _angle = angle * MathF.PI / 180;
            _coolDown = coolDown.Normalize();
            _coolDownOffset = coolDownOffset;
            _tossInvis = tossInvis;
            _probability = probability;
            _minRange = minRange;
            _maxRange = maxRange;
            _minAngle = minAngle * MathF.PI / 180;
            _maxAngle = maxAngle * MathF.PI / 180;
            _densityRange = densityRange;
            _maxDensity = maxDensity;
            _group = group;
            _region = region;
            _regionRange = regionRange;
            _throwEffect = throwEffect;
        }

        public override void Enter(Entity host)
        {
            host.StateObject[Id] = _coolDownOffset;

            if (_region == Region.None)
                return;

            var map = host.Parent.Map;

            var w = map.Width;
            var h = map.Height;

            _reproduceRegions = new List<IntPoint>(
                    map.Regions[_region].Where(a => a.X < w && a.Y < h).ToList()
                );

        }

        public override bool Tick(Entity host)
        {
            int cool = (int) host.StateObject[Id];

            if (cool <= 0)
            {
                if (host.HasConditionEffect(ConditionEffectIndex.Stunned))
                    return false;

                if (Random.NextDouble() > _probability)
                {
                    host.StateObject[Id] = _coolDown.Next(Random);
                    return false;
                }

                Entity player = GameUtils.GetNearestPlayer(host, _range);
                if (player != null || _angle != null)
                {
                    if (_densityRange != null && _maxDensity != null)
                    {
                        var cnt = 0;
                        if (_children.Length > 1)
                            cnt = host.CountEntity(_densityRange.GetValueOrDefault(1.0f), _group);
                        else
                        {
                            cnt = host.CountEntity(_densityRange.GetValueOrDefault(1.0f), _children[0]);
                        }

                        if (cnt >= _maxDensity)
                        {
                            host.StateObject[Id] = _coolDown.Next(Random);
                            return false;
                        }
                            
                    }

                    var r = _range;
                    if (_minRange != null && _maxRange != null)
                        r = _minRange.GetValueOrDefault(0.0f) + (float)Random.NextDouble() * (_maxRange.GetValueOrDefault(0.0f) - _minRange.GetValueOrDefault(0.0f));

                    var a = _angle;
                    if (_angle == null && _minAngle != null && _maxAngle != null)
                        a = _minAngle.GetValueOrDefault(0.0f) + (float)Random.NextDouble() * (_maxAngle.GetValueOrDefault(0.0f) - _minAngle.GetValueOrDefault(0.0f));

                    Vector2 target;
                    if (a != null)
                        target = new Vector2()
                        {
                            X = host.Position.X + (float) (r*Math.Cos(a.Value)),
                            Y = host.Position.Y + (float) (r*Math.Sin(a.Value)),
                        };
                    else
                        target = new Vector2()
                        {
                            X = player.Position.X,
                            Y = player.Position.Y,
                        };

                    if (_reproduceRegions != null && _reproduceRegions.Count > 0)
                    {
                        var sx = (int)host.Position.X;
                        var sy = (int)host.Position.Y;
                        var regions = _reproduceRegions
                            .Where(p => Math.Abs(sx - p.X) <= _regionRange &&
                                        Math.Abs(sy - p.Y) <= _regionRange).ToList();
                        var tile = regions[Random.Next(regions.Count)];
                        target = new Vector2()
                        {
                            X = tile.X,
                            Y = tile.Y
                        };
                    }
                    var randChildren = _children[Random.Next(_children.Length)];
                    if (!_tossInvis && !_throwEffect)
                        host.Parent.BroadcastPacketNearby(GameServer.ShowEffect
                        (
                            ShowEffectIndex.Throw,
                            host.Id,
                            0xffffbf00,
                            target
                        ), target);
                    if (_throwEffect && !_tossInvis)
                        host.Parent.BroadcastPacketNearby(GameServer.ShowEffect
                        (
                            ShowEffectIndex.ThrowProjectile,
                            host.Id,
                            randChildren,
                            target,
                            host.Position
                        ), target);
                    Manager.AddTimedAction(1500, () =>
                    {
                        if (host == null || host.Parent == null) return;
                        if (!host.Parent.IsUnblocked(target, true))
                            return;

                        Entity entity = Entity.Resolve(randChildren);
                        entity.Parent = host.Parent;
                        host.Parent.AddEntity(entity, target);

                        var enemyHost = host as Enemy;
                        var enemyEntity = entity as Enemy;
                        if (enemyHost != null && enemyEntity != null)
                        {
                            enemyEntity.Terrain = enemyHost.Terrain;
                        }

                    });
                    cool = _coolDown.Next(Random);
                }
            }
            else
                cool -= Settings.MillisecondsPerTick;

            host.StateObject[Id] = cool;

            return true;
        }
    }
}
