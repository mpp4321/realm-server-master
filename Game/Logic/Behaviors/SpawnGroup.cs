using RotMG.Common;
using RotMG.Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.logic;

namespace RotMG.Game.Logic.Behaviors
{
    class SpawnGroup : Behavior
    {

        private static Random Random = new Random();

        //State storage: Spawn host.StateObject[Id]
        class SpawnState
        {
            public int CurrentNumber;
            public int RemainingTime;
        }

        int maxChildren;
        int initialSpawn;
        Cooldown coolDown;
        ushort[] children;
        double radius;

        public SpawnGroup(string group, int maxChildren = 5, double initialSpawn = 0.5, Cooldown cooldown = new Cooldown(), double radius = 0)
        {
            this.children = Resources.Id2Object.Values
                .Where(x => x.Group == group)
                .Select(x => x.Type).ToArray();
            this.maxChildren = maxChildren;
            this.initialSpawn = (int)(maxChildren * initialSpawn);
            this.coolDown = cooldown.Normalize(0);
            this.radius = radius;
        }

        public override void Enter(Entity host)
        {
            host.StateObject[Id] = new SpawnState()
            {
                CurrentNumber = initialSpawn,
                RemainingTime = coolDown.Next(Random)
            };
            for (int i = 0; i < initialSpawn; i++)
            {
                var x = host.Position.X + (float)(Random.NextDouble() * radius);
                var y = host.Position.Y + (float)(Random.NextDouble() * radius);

                if (!host.Parent.IsUnblocked(new Vector2(x, y), true))
                    continue;

                Entity entity = Entity.Resolve(children[Random.Next(children.Length)]);
                host.Parent.AddEntity(entity, new Vector2(x, y));

                var enemyHost = host as Enemy;
                var enemyEntity = entity as Enemy;
                if (enemyHost != null && enemyEntity != null)
                {
                    enemyEntity.Terrain = enemyHost.Terrain;
                }
            }
        }

        public override bool Tick(Entity host)
        {
            var spawn = (SpawnState)host.StateObject[Id];

            if (spawn.RemainingTime <= 0 && spawn.CurrentNumber < maxChildren)
            {
                var x = host.Position.X + (float)(Random.NextDouble() * radius);
                var y = host.Position.Y + (float)(Random.NextDouble() * radius);

                if (!host.Parent.IsUnblocked(new Vector2(x, y), true))
                {
                    spawn.RemainingTime = coolDown.Next(Random);
                    spawn.CurrentNumber++;
                    return false;
                }

                Entity entity = Entity.Resolve(children[Random.Next(children.Length)]);
                host.Parent.AddEntity(entity, new Vector2(x, y));

                var enemyHost = host as Enemy;
                var enemyEntity = entity as Enemy;
                if (enemyHost != null && enemyEntity != null)
                {
                    enemyEntity.Terrain = enemyHost.Terrain;
                }

                spawn.RemainingTime = coolDown.Next(Random);
                spawn.CurrentNumber++;
            }
            else
                spawn.RemainingTime -= Settings.MillisecondsPerTick;
            return true;
        }

    }
}
