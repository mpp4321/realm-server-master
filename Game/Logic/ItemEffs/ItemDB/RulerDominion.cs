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
    public class RulerDominion : IItemHandler
    {

        public void OnTick(Player p) 
        {
            var nearbyEntities = p.GetNearbyEntities(2.0f).Where(
                e =>
                {
                    return e.Desc.Enemy && e.MaxHp < p.GetStatTotal(0);
                }
            );

            foreach(var ne in nearbyEntities)
            {
                var nova = GameServer.ShowEffect(Common.ShowEffectIndex.Nova, p.Id, 0xFFFFFF00, new Common.Vector2(3.0f, 0));
                (ne as Enemy).Damage(p, ne.MaxHp, new Common.ConditionEffectDesc[] { }, true, false);
                GameUtils.ShowEffectRange(p, p.Parent, p.Position, 10f, nova);
            }
        }

    }
}
