
using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Utils;
using System;
using System.Collections.Generic;
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
            public List<int> Entities = new List<int>();
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

        private void SpawnChildAt(Entity host, ref SpawnState state)
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
            state.Entities.Add(entity.Id);
            state.CurrentNumber++;
        }

        private void CheckEntities(Entity host, ref SpawnState state)
        {
            var NewEntities = new List<int>();
            foreach(int entId in state.Entities)
            {
                if(host.Parent != null)
                {
                    var ent = host.Parent.GetEntity(entId);
                    if (ent == null || ent.Dead) continue;
                    NewEntities.Add(ent.Id);
                }
            }
            state.Entities = NewEntities;
            state.CurrentNumber = NewEntities.Count;
        }

        public void InitializeState(Entity host)
        {
            if(host.StateObject[Id] == null)
                host.StateObject[Id] = new SpawnState()
                {
                    CurrentNumber = 0,
                    RemainingTime = _coolDown.Next(_Random)
                };

            var state = host.StateObject[Id] as SpawnState;
            CheckEntities(host, ref state);

            for (int i = 0; i < Math.Min(_initialSpawn, _maxChildren - state.CurrentNumber); i++)
            {
                SpawnChildAt(host, ref state);
            }
        }

        public override void Enter(Entity host)
        {
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
                CheckEntities(host, ref spawn);
                SpawnChildAt(host, ref spawn);
                spawn.RemainingTime = _coolDown.Next(_Random);
            }
            else
            {
                CheckEntities(host, ref spawn);
                spawn.RemainingTime -= Settings.MillisecondsPerTick;
            }

            host.StateObject[Id] = spawn;
            return true;
        }
    }
}
