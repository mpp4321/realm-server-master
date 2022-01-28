using RotMG.Common;
using RotMG.Game.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.ItemEffs.ItemDB
{
    class BloodSuckingAmulet : IItemHandler
    {
        public void OnAbilityUse(Vector2 position, ItemDesc desc, ItemDataJson itemdata, Player player)
        {
        }

        public void OnEnemyHit(Entity hit, Projectile by, ref int damageDone)
        {
        }

        public void OnHitByEnemy(Player hit, Entity hitBy, Projectile by)
        {
        }

        public void OnTick(Player p)
        {
            float hpRegenPerTick = p.GetHPRegen() * Settings.SecondsPerTick;
            var maxHp = p.SVs.GetValueOrDefault(StatType.MaxHp, 0);
            if(p.Hp > ((int)(maxHp) / 2))
            {
                p.Hp -= (int)(hpRegenPerTick + 2);
            }
        }
    }
}
