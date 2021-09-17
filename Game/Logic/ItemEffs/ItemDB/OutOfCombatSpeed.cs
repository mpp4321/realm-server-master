using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Game.Logic.ItemEffs;
using RotMG.Networking;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RotMG.Game.Logic.ItemEffs.ItemDB
{
    class OutOfCombatSpeed : IItemHandler
    {
        public HashSet<int> InCombat = new HashSet<int>();

        public void OnAbilityUse(Vector2 position, ItemDesc desc, ItemDataJson itemdata, Player player)
        {
        }

        public void OnEnemyHit(Entity hit, Projectile by)
        {
            if (!(by.Owner is Player)) return;
            if(!InCombat.Contains(by.Owner.Id))
            {
                var id = by.Owner.Id;
                InCombat.Add(id);
                Manager.AddTimedAction(2000, () =>
                {
                    InCombat.Remove(id);
                });
            }
        }

        public void OnHitByEnemy(Player hitBy, Entity hit, Projectile by)
        {
        }

        public void OnTick(Player p)
        {
            if(!InCombat.Contains(p.Id))
            {
                var boost = p.EffectBoosts.Find(a => a.id == 1);
                if (boost == null)
                    p.EffectBoosts.Add(new Player.BoostTimer()
                    {
                        timer = Settings.SecondsPerTick + .1f,
                        amount = 20,
                        index = 4,
                        id = 1
                    });
                else boost.timer = Settings.SecondsPerTick + .1f;
                p.UpdateStats();
            }
        }
    }
}
