using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotMG.Game.Logic.ItemEffs.ItemDB
{
    public class Bloodlust : IItemHandler
    {
        public void OnEnemyHit(Entity hit, Projectile by, ref int damageDone) 
        {
            by.Damage += 100;
        }
    }
}
