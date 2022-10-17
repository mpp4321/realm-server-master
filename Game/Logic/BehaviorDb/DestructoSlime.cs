using RotMG.Common;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RotMG.Game.Logic.Loots.TierLoot;

namespace RotMG.Game.Logic.Database
{
    class DestructoSlime : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {
            db.Init("Destructo Slime",
                new State("init",
                    new Wander(0.4f),
                    new Grenade(8f, 40, 03, null, 300, 0, 200, Common.ConditionEffectIndex.Berserk, 1000, 0xff00ff, 700),
                    new Shoot(12f, 3, 10, 0, null, null, 0, null, 0f, 0, 300, 1000, callback: (entity) =>
                    {
                        Entity en = Entity.Resolve(
                                Resources.Id2Object["Big Green Slime"].Type
                            );
                        entity.Parent?.AddEntity(en, entity.Position);
                    }),
                    new Shoot(12f, 8, 45, 1, 0, null, 0, null, 0f, 0, 0, 2000),
                    new Spawn("Big Green Slime", 12, 0.5, 1.0f, 15000)
                ),
                new ItemLoot("Potion of Dexterity", 1.0f, 0.01f),
                new TierLoot(5, LootType.Ability, 0.1f, 0.01f),
                new TierLoot(10, LootType.Armor, 0.1f, 0.01f),
                new TierLoot(10, LootType.Weapon, 0.1f, 0.01f),
                new TierLoot(4, LootType.Ring, 0.1f, 0.01f),
                new ItemLoot("Piss Jar", 0.01f, 0.01f),
                new Threshold(0.01f,
                    new ItemLoot("Potion of Defense", 0.75f),
                    new ItemLoot("Potion of Dexterity", 0.75f),
                    new ItemLoot("Potion of Vitality", 0.75f),
                    new ItemLoot("Realm Equipment Crystal", 0.03f),
                    new ItemLoot("Goopy Gloopy Armor", 0.03f),
                    new ItemLoot("Goo Trap", 0.03f),
                    new ItemLoot("Dark Green Clothing Dye", 0.3f),
                    new ItemLoot("Dark Green Accessory Dye", 0.3f),
                    new TierLoot(7, LootType.Weapon, 1.0f, r: new LootDef.RarityModifiedData(1.0f, 2, true))
                )
            );
        }
    }
}
