using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;
using System;
using System.Collections.Generic;
using System.Text;
using static RotMG.Game.Logic.Loots.TierLoot;

namespace RotMG.Game.Logic.Database
{
    class SnakePit : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {

            db.Init("Greater Pit Snake",
                    new State("based",
                        new Follow(2f, 10, 6),
                        new Shoot(8, 3, shootAngle: 12, cooldown: 300),
                        new Wander(0.7f)
                        )
                );
            db.Init("Greater Pit Viper",
                    new State("based",
                        new Follow(1f, 10, 6),
                        new Shoot(8, 2, shootAngle: 8, cooldown: 300, predictive: 0.4f),
                        new Wander(0.7f)
                    )
                );

            db.Init("Fire Python",
                    new State("based",
                        new Follow(3f, 10, 6),
                        new Shoot(8, 4, shootAngle: 16, cooldown: 300),
                        new Wander(0.5f)
                    ),
                    new State("protect",
                    new Shoot(8, 4, shootAngle: 16, cooldown: 300),
                        new Prioritize(
                            new Orbit(0.6f, 1.5f, 6, "Snakepit Guard")
                            )
                        )
                );
            db.Init("Brown Python",
                    new State("based",
                        new Follow(3f, 10, 6),
                        new Shoot(8, 2, shootAngle: 12, cooldown: 200),
                        new Wander(0.5f)
                    ),
                    new State("protect",
                    new Shoot(8, 2, shootAngle: 12, cooldown: 200),
                        new Prioritize(
                            new Orbit(0.6f, 1.5f, 6, "Snakepit Guard")
                            )
                    )
                );
            db.Init("Yellow Python",
                    new State("based",
                        new Follow(3f, 10, 6),
                        new Shoot(8, 1, cooldown: 100, predictive: 0.6f),
                        new Wander(0.5f)
                    ),
                    new State("protect",
                    new Shoot(8, 1, cooldown: 100, predictive: 0.6f),
                        new Prioritize(
                            new Orbit(0.6f, 1.5f, 6, "Snakepit Guard")
                            )
                        )
                );
            db.Init("Pit Viper",
                    new State("based",
                        new Shoot(8, 1, cooldown: 250, predictive: 0.6f),
                        new Wander(0.3f)
                    ),
                    new State("protect",
                    new Shoot(8, 1, cooldown: 250, predictive: 0.6f),
                        new Prioritize(
                            new Orbit(0.4f, 2.5f, 6, "Snakepit Guard")
                            )
                        )
                );
            db.Init("Pit Snake",
                    new State("based",
                        new Shoot(8, 1, cooldown: 250, predictive: 0.6f),
                        new Wander(0.3f)
                    ),
                    new State("protect",
                    new Shoot(8, 1, cooldown: 250, predictive: 0.6f),
                        new Prioritize(
                            new Orbit(0.4f, 2.5f, 6, "Snakepit Guard")
                            )
                        )
                );

            db.Init("Snakepit Guard Spawner",
                    new State("based",
                        new ConditionalEffect(Common.ConditionEffectIndex.Invincible),
                        new State("waiting"),
                        new State("spawn",
                            new Spawn("Snakepit Guard", 1, cooldown: 0),
                            new TimedTransition("waiting", 0)
                        )

                    )
                );

            db.Init("Snakepit Guard",
                new Wander(0.3f),
                    new State("based",
                        new StayBack(0.7f, 2.5f),
                        new Shoot(6, 5, 360 / 5, fixedAngle: 0f, rotateAngle: 12, angleOffset: 4, cooldown: 400, cooldownVariance: 150),
                        new TimedTransition("order", 3000)
                    ),
                    new State("order",
                        new Order(6, "Pit Snake", "protect"),
                        new Order(6, "Pit Viper", "protect"),
                        new Order(3, "Yellow Python", "protect"),
                        new Order(3, "Brown Python", "protect"),
                        new Order(3, "Fire Python", "protect"),
                        new TimedTransition("Avoid", 100)
                    ),
                    new State("Avoid",
                        new StayBack(0.7f, 2.5f),
                        new Shoot(7, 2, 32, 1, cooldown: 650),
                        new Shoot(6, 1, cooldown: 650),
                        new TimedTransition("push", 4000)
                    ),
                    new State("push",
                        new Follow(1.2f, range: 2f, duration: 1500),
                        new Shoot(6, 3, 360 / 3, 2, fixedAngle: 0f, rotateAngle: 24f, cooldown: 150),
                        new TimedTransition("Avoid", 2500)
                    ),
                    new Threshold(0.01f,
                        new ItemLoot("Potion of Vitality", 0.5f),
                        new ItemLoot("Potion of Dexterity", 0.25f),
                        new ItemLoot("Potion of Attack", 0.1f)
                    )
                );

            db.Init("Snakepit Button",
                    new ConditionalEffect(Common.ConditionEffectIndex.Invincible),
                    new State("based",
                        new PlayerWithinTransition(1, "spawn", seeInvis: true)
                    ),
                    new State("spawn",
                        new OrderOnEntry(12, "Snakepit Guard Spawner", "spawn")
                    )
                );

            db.Init("Stheno the Snake Queen",
                new State("wait",
                    new PlayerWithinTransition(8, "wait1", seeInvis: true),
                    new HealthTransition(0.99f, "wait1")
                ),
                new State("wait1",
                    new TimedTransition("start", 1000),
                    new TossObject("Stheno Pet", 7, 0, 100000, throwEffect: true),
                    new TossObject("Stheno Pet", 7, 60, 100000, throwEffect: true),
                    new TossObject("Stheno Pet", 7, 120, 100000, throwEffect: true),
                    new TossObject("Stheno Pet", 7, 180, 100000, throwEffect: true),
                    new TossObject("Stheno Pet", 7, 240, 100000, throwEffect: true),
                    new TossObject("Stheno Pet", 7, 300, 100000, throwEffect: true)
                ),
                new State("start",
                    new Shoot(12, 5, 70, 1, fixedAngle: 0f, rotateAngle: 6f, cooldown: 100, cooldownVariance: 50),
                    new Grenade(8, 60, 4, cooldown: 500),
                    new HealthTransition(0.6f, "rotateCW")
                ),
                new State("rotateCW",
                    new Prioritize(
                        new Wander(0.15f)
                        ),
                    new Shoot(15, 6, 60, fixedAngle: 0f, rotateAngle: 6f, cooldown: 100),
                    new TimedTransition("rotateCCW", 5000),
                    new HealthTransition(0.3f, "blind")
                    ),
                new State("rotateCCW",
                    new Prioritize(
                        new Wander(0.15f)
                        ),
                    new Shoot(15, 6, 60, fixedAngle: 0f, rotateAngle: -6f, cooldown: 100),
                    new TimedTransition("rotateCW", 5000),
                    new HealthTransition(0.3f, "blind")
                    ),
                new State("blind",
                    new Order(15, "Stheno Pet", "protect"),
                    new Wander(0.3f),
                    new Shoot(15, 3, 16, 1, predictive: 0.6f, cooldown: 200),
                    new Grenade(8, 60, 4, cooldown: 500),
                    new Order(20, "Stheno Pet", "protect"),
                    new Prioritize(
                        new StayBack(0.7f, 6)
                        ),
                    new TimedTransition("blindCharge", 4000)
                    ),
                    new State("blindCharge",
                        new Order(15, "Stheno Pet", "based"),
                        new Shoot(6, 3, 120, fixedAngle: 0f, rotateAngle: 16f),
                        new Prioritize(
                            new Charge(1.6f, 15),
                            new Follow(1.2f, range: 2.5f)
                            ),
                        new TimedTransition("blind", 1500)
                    ),
                    new Threshold(0.03f,
                            new ItemLoot("Potion of Mana", 1f),
                            new ItemLoot("Potion of Vitality", 1.0f),
                            new ItemLoot("Potion of Vitality", 1.0f),
                            new ItemLoot("Potion of Attack", 1.0f),
                            new ItemLoot("Serpentine Guise", 0.01f),
                            new ItemLoot("Stheno's Scourge", 0.01f),
                            new ItemLoot("Ophidian Gem", 0.01f),
                            new ItemLoot("Snakeskin Guard", 0.01f),
                            new ItemLoot("(Green) UT Egg", 0.1f, 0.01f),
                            new ItemLoot("(Blue) RT Egg", 0.02f, 0.01f),
                            new ItemLoot("Realm Equipment Crystal", 0.02f),
                            new ItemLoot("Wand of the Bulwark", 0.01f),
                            new ItemLoot("Wine Cellar Incantation", 0.03f),
                            new TierLoot(11, LootType.Armor, 0.3f),
                            new TierLoot(10, LootType.Armor, 0.4f),
                            new TierLoot(11, LootType.Weapon, 0.25f),
                            new TierLoot(10, LootType.Weapon, 0.4f),
                            new TierLoot(4, LootType.Ring, 0.25f),
                            new TierLoot(3, LootType.Ring, 0.4f),
                            new TierLoot(5, LootType.Ability, 0.2f),
                            new TierLoot(4, LootType.Ability, 1f)
                        )
                );
            db.Init("Stheno Pet",
                    new State("based",

                        new Shoot(8, 2, shootAngle: 12, cooldown: 300),
                            new Prioritize(
                                new Orbit(1.5f, 8, 20, target: "Stheno the Snake Queen")
                                ),
                        new TimedTransition("based1", 10000)
                    ),
                    new State("based1",
                        new Orbit(0.8f, 2, 20, target: "Stheno the Snake Queen"),
                        new Shoot(8, 2, shootAngle: 12, cooldown: 300),
                        new TimedTransition("based", 5000)
                    ),
                    new State("protect",
                        new Shoot(8, predictive: 0.4f, cooldown: 300),
                        new Prioritize(
                            new Orbit(1f, 1, 20, target: "Stheno the Snake Queen")
                        )
                    )
                );

        }
    }
}
