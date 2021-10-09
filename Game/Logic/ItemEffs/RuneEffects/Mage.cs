using RotMG.Common;
using RotMG.Game.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.ItemEffs.RuneEffects
{
    class Mage : IItemHandler
    {

        public void OnAbilityUse(Vector2 position, ItemDesc desc, ItemDataJson itemdata, Player player)
        {
            var mpCost = desc.MpCost;
            if(mpCost > 0)
            {
                player.Heal((int)(mpCost * 0.5), true, false);
            }
        }

    }
}
