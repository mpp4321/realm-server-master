using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Worlds
{
    class TheRing : World
    {

        public TheRing(Map map, WorldDesc desc) : base(map, desc)
        {
        }

        private Region GetRelativeTeleportRegion(Region region)
        {
            switch (region)
            {
                case Region.Decoration1:
                    return Region.Decoration2;
                case Region.Decoration3:
                    return Region.Decoration4;
                default:
                    return Region.None;
            }
        }


        public override void MoveEntity(Entity en, Vector2 to)
        {

            if (!(en is Player pl))
            {
                base.MoveEntity(en, to);
                return;
            }

            var player_tile = GetTile((int) pl.Position.X, (int) pl.Position.Y).Region;
            var to_region = GetRelativeTeleportRegion(player_tile);
            if(to_region != Region.None) {
                var vp = GetRegion(to_region).ToVector2();
                vp += new Vector2(0.5f, 0.5f);
                pl.ForceMove(vp, Manager.TotalTime);
                return;
            }

            base.MoveEntity(en, to);
        }

    }
}