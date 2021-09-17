using RotMG.Common;
using RotMG.Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RotMG.Game.Logic.ItemEffs.ItemDB
{
    class BrainOrb : IItemHandler
    {
        public void OnAbilityUse(Vector2 position, ItemDesc desc, ItemDataJson itemdata, Player p)
        {

            foreach (var j in p.Parent.EntityChunks.HitTest(position, 3).OfType<Enemy>().Where(a => a.Type != 0x5004))
            {
                Enemy e = new Enemy(0x5004);

                e.ApplyConditionEffect(ConditionEffectIndex.StasisImmune, 3000);

                p.Parent.AddEntity(e, j.Position);
                e.PlayerOwner = p;
            }

        }

        public void OnEnemyHit(Entity hit, Projectile by)
        {
        }

        public void OnHitByEnemy(Player hit, Entity hitBy, Projectile by)
        {
        }

        public void OnTick(Player p)
        {
        }
    }
}
