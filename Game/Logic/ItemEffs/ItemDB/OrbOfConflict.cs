using RotMG.Common;
using RotMG.Game.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.ItemEffs.ItemDB
{
    class OrbOfConflict : IItemHandler
    {

        public void OnAbilityUse(Vector2 position, ItemDesc desc, ItemDataJson itemdata, Player player)
        {
            Enemy e = new Enemy(Resources.Id2Object["Black Orb Ally"].Type);
            Enemy e1 = new Enemy(Resources.Id2Object["White Orb Ally"].Type);
            player.Parent.AddEntity(e, position);
            e.PlayerOwner = player;
            player.Parent.AddEntity(e1, position);
            e1.PlayerOwner = player;
        }

    }
}
