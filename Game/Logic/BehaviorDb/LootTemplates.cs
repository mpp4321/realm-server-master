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

        public static MobDrop[] BasicPots(float chance=0.5f)
        {
            return new MobDrop[] { 
                new ItemLoot("Potion of Speed", chance),
                new ItemLoot("Potion of Vitality", chance),
                new ItemLoot("Potion of Wisdom", chance),
                new ItemLoot("Potion of Dexterity", chance),
                new ItemLoot("Potion of Attack", chance),
                new ItemLoot("Potion of Defense", chance)
            };
        }
        public static MobDrop[] CrystalsRealmBoss()
        {
            return new MobDrop[] { 
                new ItemLoot("Realm Equipment Crystal", 1.0f),
                new ItemLoot("Realm Equipment Crystal", 0.75f),
                new ItemLoot("Realm Equipment Crystal", 0.5f),
                new ItemLoot("Realm Equipment Crystal", 0.25f),
                new ItemLoot("Realm Equipment Crystal", 0.1f),
            };
        }
        public static MobDrop[] CrystalsDungeonBoss()
        {
            return new MobDrop[] { 
                new ItemLoot("Realm Equipment Crystal", 1.0f),
                new ItemLoot("Realm Equipment Crystal", 1.0f),
                new ItemLoot("Realm Equipment Crystal", 0.75f),
                new ItemLoot("Realm Equipment Crystal", 0.5f),
                new ItemLoot("Realm Equipment Crystal", 0.25f),
            };
        }
        public static MobDrop[] CrystalsHardRegular()
        {
            return new MobDrop[] { 
                new ItemLoot("Realm Equipment Crystal", 1.0f),
                new ItemLoot("Realm Equipment Crystal", 1.0f),
                new ItemLoot("Realm Equipment Crystal", 1.0f),
                new ItemLoot("Realm Equipment Crystal", 0.75f),
                new ItemLoot("Realm Equipment Crystal", 0.5f),
            };
        }
    }
}
