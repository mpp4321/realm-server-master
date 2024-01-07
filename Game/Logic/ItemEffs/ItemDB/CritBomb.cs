using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Networking;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RotMG.Game.Logic.ItemEffs.RuneEffects
{
    class CritBomb : IItemHandler
    {
        public void OnEnemyHit(Entity hit, Projectile by, ref int damageDone)
        {
            if(by.DidCrit && by.Owner is Player pl)
            {
                var entities = hit.GetNearbyEntities(3.0f).OfType<Enemy>().Where(a => !a.HasConditionEffect(ConditionEffectIndex.Invincible) && !a.HasConditionEffect(ConditionEffectIndex.Invulnerable)).Take(3);
                foreach(var entity in entities)
                {
                    if (entity.Id == hit.Id) continue;
                    entity?.Damage(pl, damageDone, by.Desc.Effects, by.Desc.ArmorPiercing, true);
                }

                var nova = GameServer.ShowEffect(Common.ShowEffectIndex.Nova, hit.Id, 0xFFFF0000, new Common.Vector2(3.0f, 0));
                pl.Client.Send(nova);
            }
        }
    }
}
