using System;
using System.Collections.Generic;
using RotMG.Common;

namespace RotMG.Game.Entities
{
    public class GiftChest : OneWayContainer
    {
        public GiftChest(List<int> items, AccountModel owner) 
            : base(items, 0x0744, -1, null)
        {
        }

        public override void UpdateInventorySlot(int slot)
        {
            base.UpdateInventorySlot(slot);
            foreach (var item in Inventory)
            {
                if (item != -1)
                    return;
            }
            var closedChest = new Entity(0x0743);
            Parent.AddEntity(closedChest, Position);
            Parent.RemoveEntity(this);
        }
    }
}