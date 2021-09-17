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
    class CuriousEyeball : IItemHandler
    {
        public void OnAbilityUse(Vector2 position, ItemDesc desc, ItemDataJson itemdata)
        {
        }

        public void OnEnemyHit(Entity hit, Projectile by)
        {
            if(MathUtils.NextFloat() < 0.105f)
            {
                if (!(hit is Enemy en) || !(by.Owner is Player pl))
                    return;

                var entities = en.GetNearbyEntities(10.0f).OfType<Enemy>();
                int N = entities.Count();
                int dmgSpread = (int)((by.Damage + 1.0f) / (2.0f * N));

                var lines = new byte[N][];

                var c = 0;
                foreach(var nen in entities)
                {
                    lines[c] = GameServer.ShowEffect(ShowEffectIndex.Line, nen.Id, 0xffff0088, pl.Position, nen.Position);
                    if(nen is Enemy Enen)
                    {
                        Enen.Damage(pl, dmgSpread, new ConditionEffectDesc[] { }, false, true);
                    }
                    c++;
                }
               
                for(int i = 0; i < N; i++)
                {
                    pl.Client.Send(lines[i]);
                }
            }
        }

        public void OnHitByEnemy(Player hitBy, Entity hit, Projectile by)
        {
        }
    }
}
