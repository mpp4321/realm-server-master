using RotMG.Game.Entities;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using static RotMG.Game.Entities.Player;

namespace RotMG.Game.Logic.ItemEffs.ItemDB
{
    class DragonRage : IItemHandler
    {
        public void OnProjectileShoot(Player p, ref Projectile projectile)
        {
            foreach(Player pl in p.GetNearbyPlayers(3.0f))
            {
                pl.EffectBoosts.Add(new Player.BoostTimer
                {
                    amount = 3,
                    index = 2,
                    timer = 1.5f
                });
                pl.UpdateStats();
            }
        }
    }
}
