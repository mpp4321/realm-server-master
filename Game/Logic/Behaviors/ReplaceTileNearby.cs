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

        public ReplaceTileNearby(string from, string to, int range)
        {
            From = from;
            To = to;
            Range = range;
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

                    if(host.Parent.GetTile(relPositionX, relPositionY).Type == FromType)
                    {
                        host.Parent.UpdateTile(relPositionX, relPositionY, ToType);
                    }
                }
            }
        }

    }
}
