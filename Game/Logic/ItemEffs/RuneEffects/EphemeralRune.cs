using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotMG.Game.Logic.ItemEffs.RuneEffects
{
    class EphemeralRune : IItemHandler
    {
        public virtual void OnEnemyHit(Entity hit, Projectile by, ref int damageDone) 
        {
            if(MathUtils.Chance(damageDone/100) && by.Owner is Player pl) {
                pl.EffectBoosts.Add(new Player.BoostTimer()
                {
                    timer = 2f,
                    amount = -10,
                    index = 2,
                    id = -1
                });

                pl.EffectBoosts.Add(new Player.BoostTimer()
                {
                    timer = 2f,
                    amount = 15,
                    index = 5,
                    id = -1
                });

                pl.UpdateStats();
            }
        }
    }
}
