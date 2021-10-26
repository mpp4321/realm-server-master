using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Worlds
{
    class CyclopsLair : World
    {

        int arenaEnemyCount = 0;

        public readonly List<ushort> entitiesInArena;

        public Region TeleportRegion = Region.Enemy3;
        public Region CurrentArenaRegion = Region.Enemy3;

        public Region[] ArenaRegions = new Region[] { Region.Enemy1, Region.Enemy2, Region.Enemy3, Region.Decoration1 };

        public CyclopsLair(Map map, WorldDesc desc) : base(map, desc)
        {

            entitiesInArena = new List<ushort>();
            entitiesInArena.Add(Resources.Id2Object["Cyclops King"].Type);
            entitiesInArena.Add(Resources.Id2Object["Cyclops Warrior"].Type);
            entitiesInArena.Add(Resources.Id2Object["Cyclops Noble"].Type);
            entitiesInArena.Add(Resources.Id2Object["Cyclops Prince"].Type);

            SpawnArenaEnemies();
        }

        public void SpawnArenaEnemies()
        {
            for (int i = 0; i < 16; i++)
            {
                //This returns random point
                IntPoint SpawnRegion = GetRegion(CurrentArenaRegion);
                Enemy en = new Enemy(entitiesInArena[MathUtils.NextInt(entitiesInArena.Count)]);
                var spawnLoc = SpawnRegion.ToVector2();
                spawnLoc += new Vector2(0.5f, 0.5f);
                AddEntity(en, spawnLoc);
                arenaEnemyCount++;
            }
        }

        public override void MoveEntity(Entity en, Vector2 to)
        {
            if (!(en is Player pl))
            {
                base.MoveEntity(en, to);
                return;
            }

            if (Map.Regions[TeleportRegion].Contains(to.ToIntPoint()))
            {
                var vp = GetRegion(GetRelativeTeleportRegion()).ToVector2();
                vp += new Vector2(0.5f, 0.5f);
                pl.ForceMove(vp, Manager.TotalTime);
            } else
            {
                base.MoveEntity(en, to);
            }
        }

        public Region GetRelativeTeleportRegion()
        {
            var teleportRegion = Region.Callback3;
            switch(CurrentArenaRegion)
            {
                case Region.Enemy1:
                    teleportRegion = Region.Callback1;
                    break;
                case Region.Enemy2:
                    teleportRegion = Region.Callback2;
                    break;
                case Region.Enemy3:
                    teleportRegion = Region.Callback3;
                    break;
                case Region.Decoration1:
                    teleportRegion = Region.Decoration1;
                    break;
            }
            return teleportRegion;
        }

        public override void EnemyKilled(Enemy enemy, Player killer)
        {
            arenaEnemyCount--;
            if(arenaEnemyCount == 0)
            {
                TeleportRegion = CurrentArenaRegion;
                CurrentArenaRegion = ArenaRegions[MathUtils.NextInt(ArenaRegions.Length)];
                if(CurrentArenaRegion != Region.Decoration1) 
                    SpawnArenaEnemies();
            }
        }

    }
}
