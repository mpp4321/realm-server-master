
using RotMG.Common;
using RotMG.Game.Entities;
using System;
using wServer.logic;

namespace RotMG.Game.Logic.Behaviors
{
    class Spawn : Behavior
    {
        private static Random _Random = new Random();
        //State storage: Spawn state
        class SpawnState
        {
            public int CurrentNumber;
            public int RemainingTime;
        }

        private readonly int _maxChildren;
        private readonly int _initialSpawn;
        private Cooldown _coolDown;
        private readonly ushort _children;

        public Spawn(string children, int maxChildren = 5, double initialSpawn = 0.5, Cooldown coolDown = new Cooldown(), bool givesNoXp = true)
        {
            _children = Resources.Id2Object[children].Type;
            _maxChildren = maxChildren;
            _initialSpawn = (int)(maxChildren * initialSpawn);
            _coolDown = coolDown.Normalize(0);
        }

        public override void Enter(Entity host)
        {
            host.StateObject[Id] = new SpawnState()
            {
                CurrentNumber = _initialSpawn,
                RemainingTime = _coolDown.Next(_Random)
            };
            for (int i = 0; i < _initialSpawn; i++)
            {
                Entity entity = Entity.Resolve(_children);
                entity.Parent = host.Parent;
                //entity.Parent.MoveEntity(entity, host.Position);

                var enemyHost = host as Enemy;
                var enemyEntity = entity as Enemy;

                if (enemyHost != null && enemyEntity != null)
                {
                    //enemyEntity.ParentEntity = host as Enemy;
                    enemyEntity.Terrain = enemyHost.Terrain;
                }

                host.Parent.AddEntity(entity, host.Position);
                (host.StateObject[Id] as SpawnState).CurrentNumber++;
            }
        }

        public override bool Tick(Entity host)
        {
            var spawn = host.StateObject[Id] as SpawnState;

            if (spawn == null)
                return false;

            if (spawn.RemainingTime <= 0 && spawn.CurrentNumber < _maxChildren)
            {
                Entity entity = Entity.Resolve(_children);
                entity.Parent = host.Parent;
                host.Parent.AddEntity(entity, host.Position);

                var enemyHost = host as Enemy;
                var enemyEntity = entity as Enemy;
                if (enemyHost != null && enemyEntity != null)
                {
                    enemyEntity.Terrain = enemyHost.Terrain;
                }

                spawn.RemainingTime = _coolDown.Next(_Random);
                spawn.CurrentNumber++;
            }
            else
                spawn.RemainingTime -= Settings.MillisecondsPerTick;

            return true;
        }
    }
}
