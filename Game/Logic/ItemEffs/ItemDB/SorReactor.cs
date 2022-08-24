using RotMG.Game.Entities;
using RotMG.Utils;

namespace RotMG.Game.Logic.ItemEffs.ItemDB
{
    public class SorReactor : IItemHandler
    {
        public void OnHitByEnemy(Player hit, Entity hitBy, Projectile by) 
        { 
            if(MathUtils.Chance(0.1f))
            {
                Decoy d = new Decoy(hit, 0f, 1600) { Speed = 0.0f };
                hit.Parent.AddEntity(d, hit.Position);
            }
        }
    }
}
