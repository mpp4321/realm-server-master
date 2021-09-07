using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.Behaviors
{
    class ReturnToSpawn : Behavior
    {

        private readonly float _speed;
        private readonly float _returnWithinRadius;

        public ReturnToSpawn(double speed, double returnWithinRadius = 1)
        {
            _speed = (float)speed;
            _returnWithinRadius = (float)returnWithinRadius;
        }

        public override bool Tick(Entity host) 
        {
            if (!(host is Enemy)) return false;

            if (host.HasConditionEffect(ConditionEffectIndex.Paralyzed))
                return false;

            var spawn = (host as Enemy).SpawnPoint;
            var vect = spawn - host.Position;
            if (vect.Length() > _returnWithinRadius)
            {
                vect.Normalize();
                vect *= host.GetSpeed(_speed) * Settings.SecondsPerTick;
                host.ValidateAndMove(new Vector2(host.Position.X + vect.X, host.Position.Y + vect.Y));
            }
            return true;
        }

    }
}
