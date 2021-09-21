using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Utils;

namespace RotMG.Game.Logic.Behaviors
{
    class TransferDamageOnDeath : Behavior
    {
        private readonly ushort _target;
        private readonly float _radius;

        public TransferDamageOnDeath(string target, float radius = 50)
        {
            _target = Resources.Id2Object[target].Type;
            _radius = radius;
        }


        public override void Death(Entity host)
        {
            var enemy = host as Enemy;
            
            if (enemy == null)
                return;

            var targetObj = GameUtils.GetNearestEntity(host, _radius, _target) as Enemy;

            if (targetObj == null)
                return;

            targetObj.DamageStorage = enemy.DamageStorage;
        }
    }
}
