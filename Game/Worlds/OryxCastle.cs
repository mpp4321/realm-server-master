using System.Collections.Generic;
using System.Linq;
using RotMG.Common;
using RotMG.Utils;

namespace RotMG.Game.Worlds
{
    public class OryxCastle : World
    {
        public int IncomingPlayers;
        
        public OryxCastle(Map map, WorldDesc desc) : base(map, desc)
        {
        }

        public override IntPoint GetSpawnRegion()
        {
            IntPoint[] spawns;
            if (IncomingPlayers < 20)
            {
                spawns = GetAllRegion(Region.Spawn).Take(1).ToArray();
            }
            else if (IncomingPlayers < 40)
            {
                spawns = GetAllRegion(Region.Spawn).Take(2).ToArray();
            }
            else if (IncomingPlayers < 60)
            {
                spawns = GetAllRegion(Region.Spawn).Take(3).ToArray();
            }
            else 
            {
                spawns = GetAllRegion(Region.Spawn).ToArray();
            }

            return spawns[MathUtils.Next(spawns.Length)];
        }
    }
}