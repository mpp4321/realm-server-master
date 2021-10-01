using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Utils;
using System.Collections.Generic;
using System.Linq;

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

            foreach(var targetObj in GameUtils.GetNearbyEntities(host, _radius).OfType<Enemy>().Where(a => a.Type == _target))
            {
                if (targetObj == null)
                    return;

                targetObj.DamageStorage = new Dictionary<Player, int>(enemy.DamageStorage);
            } 

        }
    }
}
