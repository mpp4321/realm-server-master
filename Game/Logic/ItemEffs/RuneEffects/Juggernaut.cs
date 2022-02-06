using RotMG.Common;
using RotMG.Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotMG.Game.Logic.ItemEffs.RuneEffects
{
    public class Juggernaut : IItemHandler
    {
        public void OnAbilityUse(Vector2 position, ItemDesc desc, ItemDataJson itemdata, Player player) 
        {
            if(desc.CooldownMS > 3000)
            {
                player.ApplyConditionEffect(ConditionEffectIndex.Invulnerable, 1000);
            }
        }
    }
}
