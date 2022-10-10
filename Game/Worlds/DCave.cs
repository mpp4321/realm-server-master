using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Utils;

namespace RotMG.Game.Worlds
{
    public class DCave : World
    {

        int killed_juves = 0;

        public DCave(Map map, WorldDesc desc) : base(map, desc)
        {
        }
        public override void EnemyKilled(Enemy enemy, Player killer)
        {

            if(enemy.Desc.Id == "Juvenile Red Dragon")
            {
                killed_juves++;
            }
        
        }


        public override void MoveEntity(Entity en, Vector2 to)
        {
            if (!(en is Player pl))
            {
                base.MoveEntity(en, to);
                return;
            }

            if (killed_juves < 3)
            {
                base.MoveEntity(en, to);
                return;
            }

            if (Map.Regions[Region.Note].Contains(to.ToIntPoint()))
            {
                var vp = GetRegion(Region.Decoration2).ToVector2();
                vp += new Vector2(0.5f, 0.5f);
                pl.ForceMove(vp, Manager.TotalTimeUnsynced);
            } else
            {
                base.MoveEntity(en, to);
            }
        }

    }
}
