using RotMG.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotMG.Game.Logic.Behaviors
{
    public class ReplaceTileNearby : Behavior
    {

        private String From;
        private String To;
        private int Range;
        private Region? Region;

        public ReplaceTileNearby(string from, string to, int range, Region? region = null)
        {
            From = from;
            To = to;
            Range = range;
            Region = region;
        }

        public override void Enter(Entity host)
        {
            int hostPosX = (int) host.Position.X;
            int hostPosY = (int) host.Position.Y;
            ushort FromType = Resources.Id2Tile[From].Type;
            ushort ToType = Resources.Id2Tile[To].Type;
            for (int i = -Range; i < Range; i++)
            {
                for(int j = -Range; j < Range; j++)
                {
                    int relPositionX = hostPosX + i;
                    int relPositionY = hostPosY + j;

                    var tile = host.Parent.GetTile(relPositionX, relPositionY);
                    if(tile.Type == FromType)
                    {
                        if(Region != null && tile.Region == Region.Value)
                        {
                            host.Parent.UpdateTile(relPositionX, relPositionY, ToType);
                        }
                    }
                }
            }
        }

    }
}
