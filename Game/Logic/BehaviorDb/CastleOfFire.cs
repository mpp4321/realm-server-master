using System;
using System.Linq;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;

namespace RotMG.Game.Logic.Database
{
    public class CastleOfFire : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {
            db.Init("Red Sentinel", new ClearRegionOnDeath(Region.Decoration1, 5f));
            db.Init("Lava Slug", 
                new Wander(0.5f),
                new Shoot(10, 3, 30, 0, cooldown: 1000, cooldownVariance: 300),
                new Shoot(10, 1, 0, 0, cooldown: 300, cooldownVariance: 100),
                new TossObject("md1 Lava Makers", cooldown: 2000),
                new ItemLoot("Greater Health Potion", 0.3f),
                new ItemLoot("Greater Magic Potion", 0.3f)
            );

            db.Init("Lava Protector",
                new HealEntity(10f, "Lava Slug", 100, null, cooldown: 500),
                new State("frozon", new ConditionalEffect(Common.ConditionEffectIndex.Invincible), new PlayerWithinTransition(5f, "fight")),
                new State("fight", new Wander(0.3f), new Shoot(99f, 8, 45, 0, cooldown: 1000), new NoPlayerWithinTransition(5f, "frozon")),
                new ItemLoot("Greater Health Potion", 0.3f),
                new ItemLoot("Greater Magic Potion", 0.3f)
            );

            db.Init("Flaming Giant Skull", 
                HPScale.BOSS_HP_SCALE_DEFAULT(),
                new TossObject("md1 Lava Makers", range: 10,  cooldown: 100),
                new Shoot(30, 8, 45, 0, 0, 45/2f, cooldown: 500),
                new Shoot(30, 8, 45, 1, 0, 45/2f, cooldown: 1000),
                new Shoot(30, 2, 180, 2, 0, 45/2f, cooldown: 100),
                new Threshold(0.01f, 
                    new ItemLoot("Obsidian Platemail", 0.025f),
                    new ItemLoot("Tempered Staff", 0.025f),
                    new ItemLoot("Potion of Dexterity", 1f),
                    new ItemLoot("Potion of Dexterity", 1f),
                    new ItemLoot("Potion of Mana", 0.05f),
                    new TierLoot(10, TierLoot.LootType.Weapon, 0.8f, r: new LootDef.RarityModifiedData(1.0f, 3, true)),
                    new TierLoot(3, TierLoot.LootType.Ring, 0.8f, r: new LootDef.RarityModifiedData(1.0f, 3, true))
                ),
                new Threshold(0.05f, LootTemplates.CrystalsDungeonBoss())
            );
        }
    }
}
