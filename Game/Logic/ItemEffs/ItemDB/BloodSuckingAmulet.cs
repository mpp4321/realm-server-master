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

        public void OnEnemyHit(Entity hit, Projectile by)
        {
        }

        public void OnHitByEnemy(Player hit, Entity hitBy, Projectile by)
        {
        }

        public void OnTick(Player p)
        {
            float hpRegenPerTick = p.GetHPRegen() * Settings.SecondsPerTick;
            if(p.Hp > ((int)(p.SVs[StatType.MaxHp]) / 2))
            {
                p.Hp -= (int)(hpRegenPerTick + 2);
            }
        }
    }
}
