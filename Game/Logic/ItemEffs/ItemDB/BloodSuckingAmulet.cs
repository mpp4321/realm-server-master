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
            } else
            {
                var c = Manager.TotalTimeUnsynced % 20000 > 10000;
                p.AddIdentifiedEffectBoost(new Player.BoostTimer()
                {
                    amount = 15,
                    id = "BloodSuck".GetHashCode(),
                    index = c ? 2 : 5,
                    timer = 0.2f,
                }, false);
                p.UpdateStats();
            }
        }
    }
}
