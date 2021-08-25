using System.Collections.Generic;
using RotMG.Common;

namespace RotMG.Game.Entities
{
    public class OneWayContainer : Container
    {
        public OneWayContainer(List<int> items, ushort type, int ownerId, int? lifetime) 
            : base(type, ownerId, lifetime)
        {
            for (var i = 0; i < items.Count; i++)
            {
                Inventory[i] = items[i];
            }
            UpdateInventory();
        }
    }
}