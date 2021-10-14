
using RotMG.Common;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;
using RoTMG.Game.Logic.Transitions;
using wServer.logic;
using static RotMG.Game.Logic.Loots.TierLoot;

namespace RotMG.Game.Logic.Database
{
    class OryxCastle : IBehaviorDatabase
    {

        public void Init(BehaviorDb db)
        {
            db.Init("Oryx Stone Guardian Right",
                new State("Idle",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                    new PlayerWithinTransition(2, "Order")
                ),
                new State("Order",
                    new Order(10, "Oryx Stone Guardian Left", "Start"),
                    new TimedTransition("Start", 0)
                ),
                new State("Start",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Flash(0xC0C0C0, 0.5f, 3),
                    new TimedTransition("Together is better", 1500)
                ),
                new State("Together is better",
                    new EntitiesNotExistsTransition(100, "Forever Alone", "Oryx Stone Guardian Left"),
                    new TransitionFrom("Together is better", "Lets go"),
                    new State("Lets go",
                        new TimedTransition("Circle", 10000),
                        new TransitionFrom("Lets go", "Imma Follow"),
                        new State("Imma Follow",
                            new Follow(1, 2, 0.3f),
                            new Shoot(5, 5, shootAngle: 5, cooldown: 1000),
                            new TimedTransition("Imma chill", 5000)
                        ),
                        new State("Imma chill",
                            new Prioritize(
                                new StayCloseToSpawn(0.5f, 3),
                                new Wander(0.5f)
                            ),
                            new Shoot(0, 10, index: 2, fixedAngle: 0, cooldown: 1000),
                            new TimedTransition("Imma Follow", 5000)
                        )
                    ),
                    new State("Circle",
                        new TransitionFrom("Circle", "Prepare"),
                        new State("Prepare",
                            new MoveTo(speed: 1, x: 127.5f, y: 39.5f),
                            new EntitiesWithinTransition(1, "Oryx Stone Guardian Left", "Prepare2")
                        ),
                        new State("Prepare2",
                            new MoveTo(speed: 1, x: 130.5f, y: 39.5f),
                            new TimedTransition("PrepareEnd", 1000)
                        ),
                        new State("PrepareEnd",
                            new TransitionFrom("PrepareEnd", "cpe_1"),
                            new Orbit(1, 5, target: "Oryx Guardian TaskMaster"),
                            new State("cpe_1",
                                new Shoot(0, 2, fixedAngle: 0, index: 1),
                                new TimedTransition("cpe_2", 200)
                            ),
                            new State("cpe_2",
                                new Shoot(0, 2, fixedAngle: 36, index: 1),
                                new TimedTransition("cpe_3", 200)
                            ),
                            new State("cpe_3",
                                new Shoot(0, 2, fixedAngle: 72, index: 1),
                                new TimedTransition("cpe_4", 200)
                            ),
                            new State("cpe_4",
                                new Shoot(0, 2, fixedAngle: 108, index: 1),
                                new TimedTransition("cpe_5", 200)
                            ),
                            new State("cpe_5",
                                new Shoot(0, 2, fixedAngle: 144, index: 1),
                                new TimedTransition("cpe_6", 200)
                            ),
                            new State("cpe_6",
                                new Shoot(0, 2, fixedAngle: 180, index: 1),
                                new TimedTransition("cpe_7", 200)
                            ),
                            new State("cpe_7",
                                new Shoot(0, 2, fixedAngle: 216, index: 1),
                                new TimedTransition("cpe_8", 200)
                            ),
                            new State("cpe_8",
                                new Shoot(0, 2, fixedAngle: 252, index: 1),
                                new TimedTransition("cpe_9", 200)
                            ),
                            new State("cpe_9",
                                new Shoot(0, 2, fixedAngle: 288, index: 1),
                                new TimedTransition("cpe_10", 200)
                            ),
                            new State("cpe_10",
                                new Shoot(0, 2, fixedAngle: 324, index: 1),
                                new TimedTransition("checkEntities", 200)
                            ),
                            new State("checkEntities",
                                new TimedTransition("cpe_x", 0)
                            ),
                            new State("cpe_x",
                                new TimedTransition("Move Sideways", 5000) { SubIndex = 3 },
                                new PlayerWithinTransition(3, "cpe_Imma Follow") { SubIndex = 0 },
                                new NoPlayerWithinTransition(3, "cpe_Imma chill") { SubIndex = 0 },
                                new State("cpe_Imma Follow",
                                    new Follow(1, 3, 0.3f),
                                    new Shoot(5, 5, cooldown: 1000),
                                    new TimedTransition("cpe_Imma chill", 2500)
                                ),
                                new State("cpe_Imma chill",
                                    new Prioritize(
                                        new StayCloseToSpawn(0.5f, 3),
                                        new Wander(0.5f)
                                    ),
                                    new Shoot(0, 10, index: 2, fixedAngle: 0, cooldown: 1000),
                                    new TimedTransition("cpe_Imma Follow", 2500)
                                )
                            )
                        )
                    ),
                    new State("Move Sideways",
                        new TransitionFrom("Move Sideways", "msw_prepare"),
                        new State("msw_prepare",
                            new MoveTo(speed: 1, x: 141.5f, y: 39.5f),
                            new TimedTransition("msw_shoot", 1500)
                        ),
                        new State("msw_shoot",
                            new Shoot(0, 2, fixedAngle: 90, cooldownOffset: 0),
                            new Shoot(0, 2, fixedAngle: 85.5f, cooldownOffset: 100),
                            new Shoot(0, 2, fixedAngle: 81, cooldownOffset: 200),
                            new Shoot(0, 2, fixedAngle: 76.5f, cooldownOffset: 300),
                            new Shoot(0, 2, fixedAngle: 72, cooldownOffset: 400),
                            new Shoot(0, 2, fixedAngle: 67.5f, cooldownOffset: 500),
                            new Shoot(0, 2, fixedAngle: 63, cooldownOffset: 600),
                            new Shoot(0, 2, fixedAngle: 58.5f, cooldownOffset: 700),
                            new Shoot(0, 2, fixedAngle: 54, cooldownOffset: 800),
                            new Shoot(0, 2, fixedAngle: 49.5f, cooldownOffset: 900),
                            new Shoot(0, 2, fixedAngle: 45, cooldownOffset: 1000),
                            new Shoot(0, 2, fixedAngle: 40.5f, cooldownOffset: 1100),
                            new Shoot(0, 2, fixedAngle: 36, cooldownOffset: 1200),
                            new Shoot(0, 2, fixedAngle: 31.5f, cooldownOffset: 1300),
                            new Shoot(0, 2, fixedAngle: 27, cooldownOffset: 1400),
                            new Shoot(0, 2, fixedAngle: 22.5f, cooldownOffset: 1500),
                            new Shoot(0, 2, fixedAngle: 18, cooldownOffset: 1600),
                            new Shoot(0, 2, fixedAngle: 13.5f, cooldownOffset: 1700),
                            new Shoot(0, 2, fixedAngle: 9, cooldownOffset: 1800),
                            new Shoot(0, 2, fixedAngle: 4.5f, cooldownOffset: 1900),
                            new TimedTransition("Circle", 2000) { SubIndex = 2 }
                        )
                    )
                ),
                new State("Forever Alone"),
                new Threshold(0.1f,
                    new ItemLoot("Ancient Stone Sword", 0.02f),
                    new ItemLoot("Potion of Defense", 1),
                    //new ItemLoot("Gauntlet Chaos", 0.001f),
                    new TierLoot(8, LootType.Weapon, 0.1f),
                    new TierLoot(7, LootType.Armor, 0.1f),
                    new TierLoot(3, LootType.Ring, 0.1f)
                )
            );
            db.Init("Oryx Stone Guardian Left",
                    new State("Idle",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new PlayerWithinTransition(2, "Order")
                    ),
                    new State("Order",
                        new Order(10, "Oryx Stone Guardian Right", "Start"),
                        new TimedTransition("Start", 0)
                    ),
                    new State("Start",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0xC0C0C0, 0.5f, 3),
                        new TimedTransition("Together is better", 1500)
                    ),
                    new State("Together is better",
                        new EntitiesNotExistsTransition(100, "Forever Alone", "Oryx Stone Guardian Right"),
                        new TransitionFrom("Together is better", "Lets go"),
                        new State("Lets go",
                            new TimedTransition("Circle", 10000),
                            new TransitionFrom("Lets go", "Imma Follow"),
                            new State("Imma Follow",
                                new Follow(1, 2, 0.3f),
                                new Shoot(5, 5, shootAngle: 5, cooldown: 1000),
                                new TimedTransition("Imma chill", 5000)
                            ),
                            new State("Imma chill",
                                new Prioritize(
                                    new StayCloseToSpawn(0.5f, 3),
                                    new Wander(0.5f)
                                ),
                                new Shoot(0, 10, index: 2, fixedAngle: 0, cooldown: 1000),
                                new TimedTransition("Imma Follow", 5000)
                            )
                        ),
                        new State("Circle",
                            new TransitionFrom("Circle", "Prepare"),
                            new State("Prepare",
                                new MoveTo(speed: 1, x: 127.5f, y: 39.5f),
                                new EntitiesWithinTransition(1, "Oryx Stone Guardian Right", "Prepare2")
                            ),
                            new State("Prepare2",
                                new MoveTo(speed: 1, x: 124.5f, y: 39.5f),
                                new TimedTransition("PrepareEnd", 1000)
                            ),
                            new State("PrepareEnd",
                                new Orbit(1, 5, target: "Oryx Guardian TaskMaster"),
                                new TransitionFrom("PrepareEnd", "cpe_1"),
                                new State("cpe_1",
                                    new Shoot(0, 2, fixedAngle: 0, index: 1),
                                    new TimedTransition("cpe_2", 200)
                                ),
                                new State("cpe_2",
                                    new Shoot(0, 2, fixedAngle: 36, index: 1),
                                    new TimedTransition("cpe_3", 200)
                                ),
                                new State("cpe_3",
                                    new Shoot(0, 2, fixedAngle: 72, index: 1),
                                    new TimedTransition("cpe_4", 200)
                                ),
                                new State("cpe_4",
                                    new Shoot(0, 2, fixedAngle: 108, index: 1),
                                    new TimedTransition("cpe_5", 200)
                                ),
                                new State("cpe_5",
                                    new Shoot(0, 2, fixedAngle: 144, index: 1),
                                    new TimedTransition("cpe_6", 200)
                                ),
                                new State("cpe_6",
                                    new Shoot(0, 2, fixedAngle: 180, index: 1),
                                    new TimedTransition("cpe_7", 200)
                                ),
                                new State("cpe_7",
                                    new Shoot(0, 2, fixedAngle: 216, index: 1),
                                    new TimedTransition("cpe_8", 200)
                                ),
                                new State("cpe_8",
                                    new Shoot(0, 2, fixedAngle: 252, index: 1),
                                    new TimedTransition("cpe_9", 200)
                                ),
                                new State("cpe_9",
                                    new Shoot(0, 2, fixedAngle: 288, index: 1),
                                    new TimedTransition("cpe_10", 200)
                                ),
                                new State("cpe_10",
                                    new Shoot(0, 2, fixedAngle: 324, index: 1),
                                    new TimedTransition("checkEntities", 200)
                                ),
                                new State("checkEntities",
                                    new TimedTransition("cpe_x", 0)
                                ),
                                new State("cpe_x",
                                    new TimedTransition("Move Sideways", 5000) { SubIndex = 3 },
                                    new PlayerWithinTransition(3, "cpe_Imma Follow") { SubIndex = 0 },
                                    new NoPlayerWithinTransition(3, "cpe_Imma chill") { SubIndex = 0 },
                                    new State("cpe_Imma Follow",
                                        new Follow(1, 3, 0.3f),
                                        new Shoot(5, 5, cooldown: 1000),
                                        new TimedTransition("cpe_Imma chill", 2500)
                                    ),
                                    new State("cpe_Imma chill",
                                        new Prioritize(
                                            new StayCloseToSpawn(0.5f, 3),
                                            new Wander(0.5f)
                                        ),
                                        new Shoot(0, 10, index: 2, fixedAngle: 0, cooldown: 1000),
                                        new TimedTransition("cpe_Imma Follow", 2500)
                                    )
                                )
                            )
                        ),
                        new State("Move Sideways",
                            new TransitionFrom("Move Sideways", "msw_prepare"),
                            new State("msw_prepare",
                                new MoveTo(speed: 1, x: 113.5f, y: 39.5f),
                                new TimedTransition("msw_shoot", 1500)
                            ),
                            new State("msw_shoot",
                                new Shoot(0, 2, fixedAngle: 90, cooldownOffset: 0),
                                new Shoot(0, 2, fixedAngle: 85.5f, cooldownOffset: 100),
                                new Shoot(0, 2, fixedAngle: 81, cooldownOffset: 200),
                                new Shoot(0, 2, fixedAngle: 76.5f, cooldownOffset: 300),
                                new Shoot(0, 2, fixedAngle: 72, cooldownOffset: 400),
                                new Shoot(0, 2, fixedAngle: 67.5f, cooldownOffset: 500),
                                new Shoot(0, 2, fixedAngle: 63, cooldownOffset: 600),
                                new Shoot(0, 2, fixedAngle: 58.5f, cooldownOffset: 700),
                                new Shoot(0, 2, fixedAngle: 54, cooldownOffset: 800),
                                new Shoot(0, 2, fixedAngle: 49.5f, cooldownOffset: 900),
                                new Shoot(0, 2, fixedAngle: 45, cooldownOffset: 1000),
                                new Shoot(0, 2, fixedAngle: 40.5f, cooldownOffset: 1100),
                                new Shoot(0, 2, fixedAngle: 36, cooldownOffset: 1200),
                                new Shoot(0, 2, fixedAngle: 31.5f, cooldownOffset: 1300),
                                new Shoot(0, 2, fixedAngle: 27, cooldownOffset: 1400),
                                new Shoot(0, 2, fixedAngle: 22.5f, cooldownOffset: 1500),
                                new Shoot(0, 2, fixedAngle: 18, cooldownOffset: 1600),
                                new Shoot(0, 2, fixedAngle: 13.5f, cooldownOffset: 1700),
                                new Shoot(0, 2, fixedAngle: 9, cooldownOffset: 1800),
                                new Shoot(0, 2, fixedAngle: 4.5f, cooldownOffset: 1900),
                                new TimedTransition("Circle", 2000) { SubIndex = 2 }
                            )
                        )
                    ),
                    new State("Forever Alone"),
                 new Threshold(0.1f,
                    new ItemLoot("Ancient Stone Sword", 0.02f),
                    new ItemLoot("Potion of Defense", 1),
                    new TierLoot(8, LootType.Weapon, 0.1f),
                    new TierLoot(7, LootType.Armor, 0.1f),
                    new TierLoot(3, LootType.Ring, 0.1f)
                )
            );
            db.Init("Oryx Guardian TaskMaster",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new State("Idle",
                        new EntitiesNotExistsTransition(100, "Death", "Oryx Stone Guardian Right", "Oryx Stone Guardian Left")
                    ),
                    new State("Death",
                        new Spawn("Oryx's Chamber Portal", 1, 1),
                        new Suicide()
                    )
            );
            db.Init("Oryx's Living Floor Fire Down",
                    new State("Idle",
                        new PlayerWithinTransition(20, "Toss")
                    ),
                    new State("Toss",
                        new TossObject("Quiet Bomb", 10, cooldown: 1000),
                        new NoPlayerWithinTransition(21, "Idle"),
                        new PlayerWithinTransition(5, "Shoot and Toss")
                    ),
                    new State("Shoot and Toss",
                        new NoPlayerWithinTransition(21, "Idle"),
                        new NoPlayerWithinTransition(6, "Toss"),
                        new Shoot(0, 18, fixedAngle: 0, cooldown: 750, cooldownVariance: 250),
                        new TossObject("Quiet Bomb", 10, cooldown: 1000)
                    )
            );
            db.Init("Oryx Knight",
                      new State("waiting for u bae <3",
                          new PlayerWithinTransition(10, "tim 4 rekkings")
                          ),
                      new State("tim 4 rekkings",
                          new Prioritize(
                              new Wander(0.2f),
                              new Follow(0.6f, 10, 3, -1, 0)
                             ),
                          new Shoot(10, 3, 20, 0, cooldown: 350),
                          new TimedTransition("tim 4 singular rekt", 5000)
                          ),
                      new State("tim 4 singular rekt",
                          new Prioritize(
                                 new Wander(0.2f),
                              new Follow(0.7f, 10, 3, -1, 0)
                              ),
                          new Shoot(10, 1, index: 0, cooldown: 50),
                          new Shoot(10, 1, index: 1, cooldown: 1000),
                          new Shoot(10, 1, index: 2, cooldown: 450),
                          new TimedTransition("tim 4 rekkings", 2500)
                         )
            );
            db.Init("Oryx Pet",
                      new State("swagoo baboon",
                          new PlayerWithinTransition(10, "anuspiddle")
                          ),
                      new State("anuspiddle",
                          new Prioritize(
                              new Wander(0.2f),
                              new Follow(0.6f, 10, 0, -1, 0)
                              ),
                          new Shoot(10, 2, shootAngle: 20, index: 0, cooldown: 1),
                        new Shoot(10, 1, index: 0, cooldown: 1)
                         )
            );
            db.Init("Oryx Insect Commander",
                      new State("lol jordan is a nub",
                          new Prioritize(
                              new Wander(0.2f)
                              ),
                          new Reproduce("Oryx Insect Minion", 10, 20, 1),
                          new Shoot(10, 1, index: 0, cooldown: 900)
                         )
            );
            db.Init("Oryx Insect Minion",
                      new State("its SWARMING time",
                          new Prioritize(
                              new Wander(0.2f),
                              new StayCloseToSpawn(0.4f, 8),
                                 new Follow(0.8f, 10, 1, -1, 0)
                              ),
                          new Shoot(10, 5, index: 0, cooldown: 1500),
                          new Shoot(10, 1, index: 0, cooldown: 230)
                          )
            );
            db.Init("Oryx Suit of Armor",
                      new State("idle",
                          new PlayerWithinTransition(8, "attack me pl0x")
                          ),
                      new State("attack me pl0x",
                          new HealthTransition(0.99f, "jordan is stanking")
                          ),
                      new State("jordan is stanking",
                          new Prioritize(
                               new Wander(0.2f),
                               new Follow(0.4f, 10, 2, -1, 0)
                              ),
                          new SetAltTexture(1),
                          new Shoot(10, 2, 15, 0, cooldown: 600),
                          new HealthTransition(0.2f, "heal")
                          ),
                      new State("heal",
                          new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                          new SetAltTexture(0),
                          new Shoot(10, 6, index: 0, cooldown: 200),
                          new HealSelf(cooldown: 2000, amount: 200),
                          new TimedTransition("jordan is stanking", 1500)
                         )
            );
            db.Init("Oryx Eye Warrior",
                    new State("swaggin",
                        new PlayerWithinTransition(10, "penispiddle")
                        ),
                    new State("penispiddle",
                          new Prioritize(
                              new Follow(0.6f, 10, 0, -1, 0)
                              ),
                          new Shoot(10, 5, index: 0, cooldown: 1000),
                          new Shoot(10, 1, index: 1, cooldown: 500)
                         )
            );
            db.Init("Oryx Brute",
                      new State("swaggin",
                          new PlayerWithinTransition(10, "piddle")
                        ),
                      new State("piddle",
                          new Prioritize(
                              new Wander(0.2f),
                              new Follow(0.4f, 10, 1, -1, 0)
                              ),
                          new Shoot(10, 5, index: 1, cooldown: 1000),
                          new Reproduce("Oryx Eye Warrior", 10, 4, 2),
                          new TimedTransition("charge", 5000)
                          ),
                      new State("charge",
                          new Prioritize(
                              new Wander(0.3f),
                              new Follow(1.2f, 10, 1, -1, 0)
                              ),
                          new Shoot(10, 5, index: 1, cooldown: 1000),
                          new Shoot(10, 5, index: 2, cooldown: 750),
                          new Reproduce("Oryx Eye Warrior", 10, 4, 2),
                          new Shoot(10, 3, 10, index: 0, cooldown: 300),
                          new TimedTransition("piddle", 4000)
                         )
            );
            db.Init("Quiet Bomb",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new State("Idle",
                        new State("Tex1",
                            new TimedTransition("Tex2", 250)
                        ),
                        new State("Tex2",
                            new SetAltTexture(1),
                            new TimedTransition("Tex3", 250)
                        ),
                        new State("Tex3",
                            new SetAltTexture(0),
                            new TimedTransition("Tex4", 250)
                        ),
                        new State("Tex4",
                            new SetAltTexture(1),
                            new TimedTransition("Explode", 250)
                        )
                    ),
                    new State("Explode",
                        new SetAltTexture(0),
                        new Shoot(0, 18, fixedAngle: 0),
                        new Suicide()
                    )
            ); ;
        }
    }
}
