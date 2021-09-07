using RotMG.Game.Logic.Loots;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.Database
{
    class LootTemplates
    {

        public static MobDrop[] MountainDrops()
        {
            return new MobDrop[] {
                new TierLoot(4, TierLoot.LootType.Ability, 0.1f),
                new TierLoot(6, TierLoot.LootType.Weapon, 0.1f),
                new TierLoot(7, TierLoot.LootType.Armor, 0.1f)
            };
        }

        internal static MobDrop[] BasicPots(float chance=0.5f)
        {
            return new MobDrop[] { 
                new ItemLoot("Potion of Speed", chance),
                new ItemLoot("Potion of Vitality", chance),
                new ItemLoot("Potion of Wisdom", .5f),
                new ItemLoot("Potion of Dexterity", .5f),
                new ItemLoot("Potion of Attack", .5f),
                new ItemLoot("Potion of Defense", .5f)
            };
        }
    }
}
