
using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Utils;
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
        private readonly float dispersion;
        private readonly float probability;

        public Spawn(string children, int maxChildren = 5, double initialSpawn = 0.5, float probability = 1.0f, Cooldown cooldown = new Cooldown(), bool givesNoXp = true, float dispersion=0.0f)
        {
            _children = Resources.Id2Object[children].Type;
            _maxChildren = maxChildren;
            _initialSpawn = (int)(maxChildren * initialSpawn);
            _coolDown = cooldown.Normalize(0);
            this.probability = probability;
            this.dispersion = dispersion;
        }

        public void SpawnChildAt(Entity host)
        {
            if (!MathUtils.Chance(probability))
                return;
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

            entity.PlayerOwner = host.PlayerOwner;

            Func<int> gen = () => (_Random.Next(1) == 1 ? -1 : 1);
            var vectDispersion = new Vector2(gen() * dispersion, gen() * dispersion);
            host.Parent.AddEntity(entity, host.Position + vectDispersion);
            (host.StateObject[Id] as SpawnState).CurrentNumber++;
        }

        public void InitializeState(Entity host)
        {
            host.StateObject[Id] = new SpawnState()
            {
                CurrentNumber = _initialSpawn,
                RemainingTime = _coolDown.Next(_Random)
            };
            for (int i = 0; i < _initialSpawn; i++)
            {
                SpawnChildAt(host);
            }
        }

        public override void Enter(Entity host)
        {
            if (host.StateObject[Id] != null) return;
            InitializeState(host);
        }

        public override bool Tick(Entity host)
        {
            var spawn = host.StateObject[Id] as SpawnState;

            if (spawn == null)
            {
                InitializeState(host);
                return false;
            }

            if (spawn.RemainingTime <= 0 && spawn.CurrentNumber < _maxChildren)
            {
                SpawnChildAt(host);

                spawn.RemainingTime = _coolDown.Next(_Random);
                spawn.CurrentNumber++;
            }
            else
                spawn.RemainingTime -= Settings.MillisecondsPerTick;

            host.StateObject[Id] = spawn;
            return true;
        }
    }
}
