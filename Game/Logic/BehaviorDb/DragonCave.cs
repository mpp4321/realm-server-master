using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;

namespace RotMG.Game.Logic.Database
{
    public class DragonCave : IBehaviorDatabase
    {

        public void Init(BehaviorDb db)
        {
            db.Init("Juvenile Red Dragon",
                    new HPScale(0.2f),
                    new Wander(0.4f),
                    new Shoot(5, 2, 10, 0, cooldown: 500),
                    new Shoot(5, 3, 10, predictive: 1, index: 2, cooldown: 1000, cooldownVariance: 500),
                    new Shoot(20, 4, 45, fixedAngle: 0, rotateAngle: 25f, index: 1),
                    new Prioritize(
                        new Follow(0.6f, 10, 3),
                        new StayCloseToSpawn(0.4f, 10)
                    ),
                    new ItemLoot("Potion of Attack"),
                    new ItemLoot("Potion of Defense"),
                    new ItemLoot("Rune of the Brute", 0.001f, 0.01f)
                );

            db.Init("Adult Red Dragon",
                    new HPScale(0.2f),
                    new Wander(0.4f),
                    new Grenade(8f, 30, 3, speed: 800, cooldown: 1000),
                    new Shoot(5, 2, 10, 0, cooldown: 500),
                    new Shoot(5, 3, 10, predictive: 1, index: 2, cooldown: 500),
                    new Shoot(20, 4, 45, fixedAngle: 0, rotateAngle: 25f, index: 1),
                    new Prioritize(
                        new Follow(0.6f, 10, 3),
                        new StayCloseToSpawn(0.4f, 10)
                    ),
                    new Threshold(0.05f,
                        new ItemLoot("Quiver of Flowing Magma", 0.001f),
                        new ItemLoot("Parthanax's Horn", 0.001f),
                        new ItemLoot("Dominion of the Dragon", 0.0002f),
                        new ItemLoot("Rune of the Brute", 0.005f),
                        new ItemLoot("Lesser Fiery Equipment Crystal", 0.02f)
                    )
                );

            db.Init("loot balloon dcave",
                    new Threshold(0.10f,
                        new ItemLoot("Potion of Dexterity", 0.5f),
                        new ItemLoot("Potion of Defense", 0.5f),
                        new ItemLoot("Potion of Wisdom", 0.5f),
                        new ItemLoot("Potion of Mana", 0.1f),
                        new ItemLoot("Lesser Fiery Equipment Crystal", 0.005f),
                        new ItemLoot("Rune of the Brute", 0.001f)
                    )
                );

            db.Init("dcavemanager1",
                    new State("base",
                        new ConditionalEffect(Common.ConditionEffectIndex.Invincible, true),
                        new EntitiesNotExistsTransition(99f, "walls", "loot balloon dcave") { SubIndex = 0 },
                        new State("walls",
                            new ClearRegionOnDeath(Region.Decoration1),
                            new Suicide(100)
                        )
                    )
                );

            db.Init("Parthanax",
                    new HPScale(0.3f),
                    new State("p_t", new PlayerWithinTransition(20f, "chat_intro")),
                    new State("chat_intro",
                        new Taunt(cooldown: 1000, "Welcome to my den humans...", "Prepare to die!")
                        {
                            Ordered = 0
                        },
                        new TimedTransition("play_1_stone", 2000)
                    ),
                    new State("play_1_stone",
                        new Wander(0.2f),
                        new Shoot(15f, 1, index: 3, cooldown: 3000, cooldownVariance: 1000),
                        new Shoot(15f, 1, index: 0, fixedAngle: 0f, rotateAngle: 45),
                        new Shoot(10f, 2, index: 1, cooldown: 500, cooldownOffset: 0, shootAngle: 20),
                        new Shoot(10f, 1, index: 1, cooldown: 500, cooldownOffset: 250),

                        new Shoot(10f, 2, 20, index: 2, fixedAngle: 90, cooldown: 500),
                        new Shoot(10f, 2, 20, index: 2, fixedAngle: 180, cooldown: 500),

                        new TimedTransition("t_2_chase", 10000)
                    ),
                    new State("t_2_chase",
                        new TimedTransition("play_2_chase", 3000),
                        new ConditionalEffect(Common.ConditionEffectIndex.Invulnerable, duration: 3000),
                        new Flash(0xFF332211, 1.0, 3),
                        new Taunt(cooldown: 4000, "Rawr!!")
                    ),
                    new State("play_2_chase",
                        new Prioritize(
                            new Charge(1.0f, 10f, coolDown: 1000),
                            new Wander(0.4f)
                        ),
                        new Shoot(10f, 2, index: 1, cooldown: 250, cooldownOffset: 0, shootAngle: 20),
                        new Shoot(10f, 1, index: 1, cooldown: 250, cooldownOffset: 100),
                        new Shoot(10f, 2, 20, index: 2, fixedAngle: 90, cooldown: 500),
                        new Shoot(10f, 2, 20, index: 2, fixedAngle: 180, cooldown: 500),
                        new Shoot(10f, 2, predictive: 1, cooldown: 500),
                        new TimedTransition("play_1_stone", 8000)
                    ),
                    new Threshold(0.01f,
                        new ItemLoot("(Green) UT Egg", 0.12f, 0.01f),
                        new ItemLoot("(Blue) RT Egg", 0.05f, 0.05f),
                        new ItemLoot("Potion of Dexterity", 1.0f),
                        new ItemLoot("Potion of Mana", 1.0f),
                        new ItemLoot("Potion of Attack", 1.0f),
                        new ItemLoot("Fishing Rod", 0.15f),
                        new ItemLoot("Lesser Fiery Equipment Crystal", 0.5f),
                        new TierLoot(10, TierLoot.LootType.Armor, 1.0f, r: new LootDef.RarityModifiedData(1.0f, 1, false)),
                        new TierLoot(11, TierLoot.LootType.Armor, 0.5f),
                        new TierLoot(10, TierLoot.LootType.Weapon, 1.0f, r: new LootDef.RarityModifiedData(1.0f, 1, false)),
                        new TierLoot(11, TierLoot.LootType.Weapon, 0.5f),
                        new TierLoot(5, TierLoot.LootType.Ring, 0.3f, r: new LootDef.RarityModifiedData(1.0f, 1, false)),
                        new TierLoot(5, TierLoot.LootType.Ability, 0.3f, r: new LootDef.RarityModifiedData(1.0f, 1, false))
                    ),
                    new Threshold(0.05f,
                        new ItemLoot("Quiver of Flowing Magma", 0.01f),
                        new ItemLoot("Parthanax's Horn", 0.01f),
                        new ItemLoot("Dominion of the Dragon", 0.005f),
                        new ItemLoot("Rune of the Brute", 0.01f)
                    ),
                    new Threshold(0.01f, LootTemplates.CrystalsHardRegular())
                );
        }

    }
}
