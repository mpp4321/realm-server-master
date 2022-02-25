using RotMG.Game.Entities;
using RotMG.Networking;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotMG.Game.Logic.ItemEffs.ItemDB
{
    public class Thunderbolts : IItemHandler
    {

        public virtual void OnEnemyHit(Entity hit, Projectile by, ref int damageDone)
        {

            if (MathUtils.Chance(0.10f))
            {
                var @nova = GameServer.ShowEffect(Common.ShowEffectIndex.Nova, hit.Id, 0xFFFFFF00, new Common.Vector2(1.5f, 0f));
                GameUtils.ShowEffectRange(by.Owner as Player, hit.Parent, hit.Position, 10f, @nova);
                var pl = by.Owner as Player;
                var speedTotal = pl.GetStatTotal(4);
                foreach(var enemy in GameUtils.GetNearbyEntities(hit, 1.5f).OfType<Enemy>())
                {
                    if (enemy == null) continue;
                    enemy.Damage(pl, speedTotal * 10, new Common.ConditionEffectDesc[] { }, false, true);
                }
            }

        }

    }
}
