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
        int t = 0;
        public virtual void OnEnemyHit(Entity hit, Projectile by, ref int damageDone) 
        {
            if (t++ % 8 != 0) return;
            if(by.DidCrit && by.Owner is Player pl)
            {
                pl.EffectBoosts.Add(new Player.BoostTimer()
                {
                    timer = 5f,
                    amount = 3,
                    index = 7,
                    id = -1
                });

                pl.EffectBoosts.Add(new Player.BoostTimer()
                {
                    timer = 5f,
                    amount = 3,
                    index = 4,
                    id = -1
                });

                pl.UpdateStats();
            }
        }
    }
}
