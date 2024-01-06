using System.Collections.Generic;
using System.Linq;
using RotMG.Common;

namespace RotMG.Game.Entities
{
    public class OneWayContainer : Container
    {
        public OneWayContainer(List<int> items, List<ItemDataJson> itemDatas, ushort type, int ownerId, int? lifetime) 
            : base(type, ownerId, lifetime)
        {
            for (var i = 0; i < items.Count; i++)
            {
                Inventory[i] = items[i];
                ItemDatas[i] = itemDatas.ElementAtOrDefault(i) ?? new ItemDataJson();
            }
            UpdateInventory();
        }
    }
}