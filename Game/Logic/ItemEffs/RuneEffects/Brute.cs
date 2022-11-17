using RotMG.Game.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.ItemEffs.RuneEffects
{
    class Brute : IItemHandler
    {
        public virtual void OnHitByEnemy(Player hit, Entity hitBy, Projectile by) 
        {
            var pureDamageTaken = by.Damage;
            hit.AddIdentifiedEffectBoost(new Player.BoostTimer()
            {
                timer = 10.0f,
                index = 2 * (pureDamageTaken / 100),
                amount = 6,
                id = "Brute".GetHashCode()
            }, true, (a, b) =>
            {
                return Math.Min(a + b, 30);
            });
            hit.UpdateStats();
        }

    }
}
