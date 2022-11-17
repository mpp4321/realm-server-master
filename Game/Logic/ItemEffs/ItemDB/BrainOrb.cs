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
            var iter = p.Parent.EntityChunks.HitTest(position, 3).OfType<Enemy>().Where(a => a.Type != 0x5004);
            if (iter.Count() > 5) iter = iter.Take(5);
            foreach (var j in iter)
            {
                Enemy e = new Enemy(0x5004);

                e.ApplyConditionEffect(ConditionEffectIndex.StasisImmune, 10000);

                p.Parent.AddEntity(e, j.Position);
                e.PlayerOwner = p;
            }
        }
    }
}
