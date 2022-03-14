using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Utils;
using System.Collections.Generic;
using System.Linq;

namespace RotMG.Game.Worlds
{
    public class OceanTrench : World
    {

        private static List<string> possibleTreasures = new List<string>()
        {
            "Coral Bow",
            "Coral Ring",
            "Coral Silk Armor",
            "Oryx Equipment Crystal"
        };

        private static List<string> commonTreasures = new List<string>()
        {
            "Potion of Attack",
            "Potion of Attack",
            "Coral Juice",
            "Coral Juice",
            "Potion of Defense",
            "Potion of Dexterity",
            "Potion of Mana",
            "Potion of Mana",
            "Realm Equipment Crystal"
        };

        public OceanTrench(Map map, WorldDesc desc) : base(map, desc)
        {
            foreach(var tile in map.Regions[Region.Note])
            {
                if(MathUtils.Chance(0.1f))
                {
                    var items = new List<string>();
                    // Generate treasures
                    for(int i = 0; i < 8; i++)
                    {
                        if(MathUtils.Chance(0.01f))
                        {
                            items.Add(
                                possibleTreasures[MathUtils.NextInt(0, possibleTreasures.Count - 1)]
                            );
                        } else
                        {
                            items.Add(
                                commonTreasures[MathUtils.NextInt(0, commonTreasures.Count - 1)]
                            );
                        }
                    }
                    var c = new Container(Container.FromBagType(6), -1, 40000 * 20);
                    c.Inventory = items.Select(item =>
                    {
                        return (int)Resources.Id2Item[item].Type;
                    }).ToArray();
                    AddEntity(c, tile.ToVector2() + new Vector2(0.5f, 0.5f));
                    c.UpdateInventory();
                }
            }
        }

    }
}
