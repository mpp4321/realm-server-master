using RotMG.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.Behaviors
{
    class DropPortalOnDeath : Behavior
    {
        private static Random Random = new Random();

        private readonly ushort _target;
        private readonly float _probability;
        private readonly int? _timeout;

        public DropPortalOnDeath(string target, float probability = 1, int timeout = 30)
        {
            _target = Resources.Id2Object[target].Type;
            _probability = probability;
            _timeout = timeout; // a value of 0 means never timeout, 
            // null means use xml timeout, 
            // a value means override xml timeout with that value (in seconds)
        }

        public override void Death(Entity host)
        {
            var owner = host.Parent;

            if (Random.NextDouble() < _probability)
            {
                var timeoutTime = _timeout.Value;
                var entity = Entity.Resolve(_target);
                host.Parent.AddEntity(entity, host.Position);

                if (timeoutTime != 0)
                    Manager.AddTimedAction(timeoutTime * 1000, () =>
                    {
                        try
                        {
                            host.Parent.RemoveEntity(entity);
                        }
                        catch
                        //couldn't remove portal, Owner became null. Should be fixed with RealmManager implementation
                        {
                            Console.WriteLine("Couldn't despawn portal.");
                        }
                    });
            }
        }

    }
}
