﻿using RotMG.Common;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.Behaviors
{
    class MoveTo : Behavior
    {

        private readonly float _speed;
        private readonly float _x;
        private readonly float _y;

        public MoveTo(float speed, float x, float y)
        {
            _speed = speed;
            _x = x;
            _y = y;
        }

        public override bool Tick(Entity host)
        {
            if (host.HasConditionEffect(ConditionEffectIndex.Paralyzed))
                return false;
            var path = new Vector2(_x - host.Position.X, _y - host.Position.Y);
            var dist = host.GetSpeed(_speed) * Settings.SecondsPerTick;
            if (path.Length() <= dist)
            {
                host.ValidateAndMove(new Vector2(_x, _y));
                return false;
            }
            else
            {
                path.Normalize();
                host.ValidateAndMove(new Vector2(host.Position.X + path.X * dist, host.Position.Y + path.Y * dist));
                return true;
            }
        }

    }
}
