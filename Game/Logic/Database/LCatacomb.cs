using RotMG.Common;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;
using System;
using System.Collections.Generic;
using System.Text;
using static RotMG.Game.Logic.Loots.TierLoot;

namespace RotMG.Game.Logic.Database
{
    class LCatacomb : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {
            db.Init("Undead Giant Hand", 
                new State("base", 
                    new State("playerwait", new PlayerWithinTransition(10f, "jumping", false)),
                    new State("jumping",
                        new ChangeSize(-10, 0),
                        new TimedTransition("jumped", 1000)
                    ),
                    new State("jumped", 
                        new Charge(8f, 10, coolDown: 6000),
                        new ChangeSize(10, 50),
                        new Follow(0.1f, 99, 0.1f),
                        new Shoot(5, 1, 0, predictive: 1, fixedAngle: 0, cooldownOffset: 500,  cooldown: 5000, index: 1),
                        new TimedTransition("post jump", 1000)
                    ),
                    new State("post jump",
                        new Shoot(5, 8, 360 / 8.0f, predictive: 1, fixedAngle: 0, cooldownOffset: 500,  cooldown: 5000, index: 1),
                        new Shoot(5, 3, 30, 0, cooldown: 1000),
                        new Shoot(5, 2, 15, 0, cooldown: 2000),
                        new HealSelf(cooldown: 500, 1000),
                        new Grenade(0, 0, 5, 0f, 1000, effect: ConditionEffectIndex.Cursed, effectDuration: 1000, color: 0xff11ff11),
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new TimedTransition("jumping", 5000)
                    )
                ),
                new Threshold(0.01f,
                    new ItemLoot("Potion of Life", min: 2),
                    new ItemLoot("Potion of Vitality", min: 2),
                    new ItemLoot("Potion of Attack", min: 2),
                    new ItemLoot("Oryx Equipment Crystal", 0.02f),
                    new ItemLoot("Realm Equipment Crystal", 0.2f),
                    new TierLoot(12, LootType.Armor, 0.1f, r: new LootDef.RarityModifiedData(1.0f, 1, alwaysRare: true)),
                    new TierLoot(12, LootType.Weapon, 0.1f, r: new LootDef.RarityModifiedData(1.0f, 1, alwaysRare: true)),
                    new TierLoot(5, LootType.Ability, 0.1f, r: new LootDef.RarityModifiedData(1.0f, 1, alwaysRare: true)),
                    new ItemLoot("Bow of Life Energy", 0.01f),
                    new ItemLoot("Reinforced Ribcage", 0.01f)
                )
            );

            db.Init("LCatacomb Hand Spawner",
                    new State("waiting",
                        new ConditionalEffect(Common.ConditionEffectIndex.Invulnerable),
                        new ConditionalEffect(Common.ConditionEffectIndex.Invincible),
                        new EntitiesNotExistsTransition(99, "spawnndie", "Zombie Hulk")
                    ),
                    new State("spawnndie",
                        new Spawn("Undead Giant Hand", maxChildren: 1, initialSpawn: 1.0, givesNoXp: false),
                        new Suicide()
                    )
                );

            db.Init("Zombie Knight", 
                    new State("base",
                        new Shoot(
                            10,
                            2,
                            15,
                            cooldown: 600
                        ),
                        new Shoot(
                            3,
                            3,
                            10,
                            index: 1,
                            cooldown: 3000),
                        new Prioritize(
                            new Charge(1.0f, 10, 3000),
                            new Wander(0.3f)
                        )
                    )
                );

            db.Init("Zombie Wizard", 
                    new State("base",
                        new Shoot(
                            10,
                            2,
                            10,
                            cooldown: 1500
                        ),
                        new Prioritize(
                            new StayBack(1.0f, 5),
                            new Wander(0.5f)
                        )
                    )
                );

            db.Init("Zombie Mystic", 
                    new State("base",
                        new Shoot(
                            10,
                            2,
                            10,
                            cooldown: 3000
                        ),
                        new Shoot(
                            99,
                            8,
                            45,
                            index: 1,
                            cooldown: 1000),
                        new Prioritize(
                            new StayBack(1.0f, 5),
                            new Wander(0.5f)
                        )
                    )
                );

            db.Init("Zombie Assassin", 
                    new State("base",
                        new Shoot(
                            10,
                            2,
                            10,
                            cooldown: 1500
                        ),
                        new Grenade(6, damage: 100, radius: 3, color: 0xff00ff00, cooldown: 3000),
                        new Prioritize(
                            new StayBack(1.0f, 5),
                            new Wander(0.5f)
                        )
                    )
                );

            db.Init("Zombie Huntress", 
                    new State("base",
                        new Shoot(
                            10,
                            2,
                            10,
                            cooldown: 1500
                        ),
                        new TossObject("Arena Spider", 6, cooldown: 3000),
                        new Prioritize(
                            new StayBack(1.0f, 5),
                            new Wander(0.5f)
                        )
                    )
                );

            db.Init("Zombie Paladin", 
                    new State("base",
                        new Shoot(
                            10,
                            1,
                            cooldown: 1500
                        ),
                        new HealEntity(10, "Zombies", 50, cooldown: 300),
                        new Prioritize(
                            new StayBack(1.0f, 5),
                            new Wander(0.5f)
                        )
                    )
                );

        }
    }
}
