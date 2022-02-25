using RotMG.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotMG.Game.Logic.Behaviors
{
    public class ChangeGroundOnEnter : Behavior
    {

        private Region From;
        private string To;

        public ChangeGroundOnEnter(Region from, string to)
        {
            From = from;
            To = to;
        }

        public override void Enter(Entity host)
        {
            ushort tileType = Resources.Id2Tile[To].Type;
            foreach(var point in host.Parent.GetAllRegion(From))
            {
                host.Parent.UpdateTile(point.X, point.Y, tileType);
            }
        }

    }
}
