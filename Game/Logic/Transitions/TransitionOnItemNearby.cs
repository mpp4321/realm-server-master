using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotMG.Game.Logic.Transitions
{
    public class TransitionOnItemNearby : Transition
    {

        float Distance { get; set; }
        ushort Item { get; set; }
        public bool Consume { get; set; } = true;

        public TransitionOnItemNearby(float distance, string item, string state) : base(state)
        {
            Distance = distance;
            Item = Resources.Id2Item[item].Type;
        }

        public override bool Tick(Entity host)
        {
            var containers = GameUtils.
                GetNearbyEntities(host, Distance).
                OfType<IContainer>().
                Where(a => !(a is Player) && a.Inventory.Contains(Item));
            if(containers.Any())
            {
                if (Consume)
                {
                    var containerToChange = containers.First();
                    for(int i = 0; i < containerToChange.Inventory.Length; i++)
                    {
                        if(containerToChange.Inventory[i] == Item)
                        {
                            containerToChange.Inventory[i] = -1;
                            containerToChange.ItemDatas[i] = new();
                            containerToChange.UpdateInventorySlot(i);
                            return true;
                        }
                    }
                }
                return true;
            }
            return false;
        }

    }
}
