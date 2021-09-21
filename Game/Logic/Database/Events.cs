using RotMG.Common;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;
using RoTMG.Game.Logic.Transitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.Database
{
    class Events : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {

            db.Init("Hermit God",
                new State("base",
                    new TransferDamageOnDeath("Hermit God Drop"),
                    new OrderOnDeath(20, "Hermit God Tentacle Spawner", "Die", 1),
                    new OrderOnDeath(20, "Hermit God Drop", "Die", 1),
                    new State("Spawn Tentacle",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new SetAltTexture(2),
                        new Order(20, "Hermit God Tentacle Spawner", "Tentacle"),
                        new EntitiesWithinTransition( 20, "Hermit God Tentacle", "Sleep")
                        ),
                    new State("Sleep",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Order(20, "Hermit God Tentacle Spawner", "Minions"),
                        new TimedTransition("Waiting", 1000)
                        ),
                    new State("Waiting",
                        new SetAltTexture(3),
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new EntitiesNotExistsTransition(20, "Wake", "Hermit God Tentacle")
                        ),
                    new State("Wake",
                        new SetAltTexture(2),
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new TossObject("Hermit Minion", 10, angle: 0, throwEffect: true),
                        new TossObject("Hermit Minion", 10, angle: 45, throwEffect: true),
                        new TossObject("Hermit Minion", 10, angle: 90, throwEffect: true),
                        new TossObject("Hermit Minion", 10, angle: 135, throwEffect: true),
                        new TossObject("Hermit Minion", 10, angle: 180, throwEffect: true),
                        new TossObject("Hermit Minion", 10, angle: 225, throwEffect: true),
                        new TossObject("Hermit Minion", 10, angle: 270, throwEffect: true),
                        new TossObject("Hermit Minion", 10, angle: 315, throwEffect: true),
                        new TimedTransition("Spawn Whirlpool", 100)
                        ),
                    new State("Spawn Whirlpool",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Order(20, "Hermit God Tentacle Spawner", "Whirlpool"),
                        new EntitiesWithinTransition(20, "Whirlpool", "Attack1")
                        ),
                    new State("Attack1",
                        new SetAltTexture(0),
                        new Prioritize(
                            new Wander(0.3f),
                            new StayCloseToSpawn(0.5f, 5)
                            ),
                        new Shoot(20, count: 3, shootAngle: 5, cooldown: 300),
                        new TimedTransition("Attack2", 6000)
                        ),
                    new State("Attack2",
                        new Prioritize(
                            new Wander(0.3f),
                            new StayCloseToSpawn(0.5f, 5)
                            ),
                        new Order(20, "Whirlpool", "Die"),
                        new Shoot(20, count: 1, defaultAngle: 0, fixedAngle: 0, rotateAngle: 45, index: 1,
                            cooldown: 1000),
                        new Shoot(20, count: 1, defaultAngle: 0, fixedAngle: 180, rotateAngle: 45, index: 1,
                            cooldown: 1000),
                        new TimedTransition("Spawn Tentacle", 6000)
                        )
                    )
            );
            db.Init("Hermit Minion",
                new State("base",
                    new Prioritize(
                        new Follow(0.6f, 4, 1),
                        new Orbit(0.6f, 10, 15, "Hermit God", speedVariance: .2f, radiusVariance: 1.5f),
                        new Wander(0.6f)
                        ),
                    new Shoot(6, count: 3, shootAngle: 10, cooldown: 1000),
                    new Shoot(6, count: 2, shootAngle: 20, index: 1, cooldown: 2600, predictive: 0.8f)
                    ),
                new ItemLoot("Health Potion", 0.1f),
                new ItemLoot("Magic Potion", 0.1f)
            );
            db.Init("Whirlpool",
                new State("base",
                    new State("Attack",
                        new EntitiesNotExistsTransition( 100, "Die", "Hermit God"),
                        new Prioritize(
                            new Orbit(0.3f, 6, 10, "Hermit God")
                            ),
                        new Shoot(0, 1, fixedAngle: 0, rotateAngle: 30, cooldown: 400)
                        ),
                    new State("Die",
                        new Shoot(0, 8, fixedAngle: 360 / 8),
                        new Suicide()
                        )
                    )
            );
            db.Init("Hermit God Tentacle",
                new State("base",
                    new Prioritize(
                        new Follow(0.6f, 4, 1),
                        new Orbit(0.6f, 6, 15, "Hermit God", speedVariance: .2f, radiusVariance: .5f)
                        ),
                    new Shoot(3, count: 8, shootAngle: 360 / 8, cooldown: 500)
                    )
            );
            db.Init("Hermit God Tentacle Spawner",
                new State("base",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new State("Waiting Order"),
                    new State("Tentacle",
                        new Reproduce("Hermit God Tentacle", 3, 1, cooldown: 2000),
                        new EntitiesWithinTransition(1, "Hermit God Tentacle", "Waiting Order")
                        ),
                    new State("Whirlpool",
                        new Reproduce("Whirlpool", 3, 1, cooldown: 2000),
                        new EntitiesWithinTransition(1, "Whirlpool", "Waiting Order")
                        ),
                    new State("Minions",
                        new Reproduce("Hermit Minion", 40, 20, cooldown: 1000),
                        new TimedTransition("Waiting Order", 2000)
                        ),
                    new State("Die",
                        new Suicide()
                        )
                    )
            );
            db.Init("Hermit God Drop",
                new State("base",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new State("Waiting"),
                    new DropPortalOnDeath("Ocean Trench Portal", 1),
                    new State("Die",
                        new Suicide()
                        )
                    ),
                new Threshold(0.03f,
                    new ItemLoot("Helm of the Juggernaut", 0.002f)
                )
            );

            db.Init("Skull Shrine",
                new State("base",
                    new Shoot(30, 4, 9, cooldown: 1500, predictive: 1f), // add prediction after fixing it...
                    new Reproduce("Blue Flaming Skull", 20, 5, cooldown: 1000)
                    ),
                new Threshold(0.03f,
                    new ItemLoot("Orb of Conflict", 0.005f)
                    ),
            new Threshold(0.005f,
                    LootTemplates.BasicPots(0.5f)
                )
            );
            db.Init("Red Flaming Skull",
                new State("base",
                    new State("Orbit Skull Shrine",
                        new Prioritize(
                            new Protect(.2f, "Skull Shrine", 30, 15, 15),
                            new Wander(.2f)
                            ),
                            new EntitiesNotExistsTransition( 40, "Wander", "Skull Shrine")
                        ),
                    new State("Wander",
                        new Wander(.2f)
                        ),
                    new Shoot(12, 2, 10, cooldown: 750)
                    )
            );
            db.Init("Blue Flaming Skull",
                new State("base",
                    new State("Orbit Skull Shrine",
                        new Orbit(1.0f, 15, 40, "Skull Shrine", .6f, 10),
                        new EntitiesNotExistsTransition(40, "Wander", "Skull Shrine")
                        ),
                    new State("Wander",
                        new Wander(1.0f)
                        ),
                    new Shoot(12, 2, 10, cooldown: 750)
                    )
            );
        }
    }
}
