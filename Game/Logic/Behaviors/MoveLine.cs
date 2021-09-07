using RotMG.Common;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.Behaviors
{
    class MoveLine : Behavior
    {

        private readonly float _speed;
        private readonly float _direction;
        private readonly float _distance;

        public MoveLine(double speed, double direction = 0, double distance = 0)
        {
            _speed = (float)speed;
            _direction = (float)direction * (float)Math.PI / 180;
            _distance = (float)distance;
        }

        public override bool Tick(Entity host)
        {
            float dist;
            if (host.StateObject[Id] == null)
                dist = _distance;
            else
                dist = (float)host.StateObject[Id];

            if (host.HasConditionEffect(ConditionEffectIndex.Paralyzed))
                return false;

            if (_distance == 0)
            {
                var vect = new Vector2((float)Math.Cos(_direction), (float)Math.Sin(_direction));
                var moveDist = host.GetSpeed(_speed) * Settings.SecondsPerTick;
                host.ValidateAndMove(vect * moveDist + host.Position);
            }
            if (dist > 0)
            {
                var moveDist = host.GetSpeed(_speed) * Settings.SecondsPerTick;
                var vect = new Vector2((float)Math.Cos(_direction), (float)Math.Sin(_direction));
                host.ValidateAndMove(vect * moveDist + host.Position);
                dist -= moveDist;
            }
            host.StateObject[Id] = dist;
            return true;
        }

        public override void Enter(Entity host)
        {
            host.StateObject[Id] = null;
        }

    }
}
