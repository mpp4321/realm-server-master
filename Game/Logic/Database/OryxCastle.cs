
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
                        new Decay(0)
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
            );

            // Oryx chamber oryx + Wine Celler + WC Minions

            db.Init("Oryx the Mad God 2",
                            new State("base",
                                //new ScaleHP(50000),
                                new State("Attack",
                                    new Wander(.05f),
                                    new Shoot(25, index: 0, count: 8, shootAngle: 45, cooldown: 1500, cooldownOffset: 1500),
                                    new Shoot(25, index: 1, count: 3, shootAngle: 10, cooldown: 1000, cooldownOffset: 1000),
                                    new Shoot(25, index: 2, count: 3, shootAngle: 10, predictive: 0.2f, cooldown: 1000,
                                        cooldownOffset: 1000),
                                    new Shoot(25, index: 3, count: 2, shootAngle: 10, predictive: 0.4f, cooldown: 1000,
                                        cooldownOffset: 1000),
                                    new Shoot(25, index: 4, count: 3, shootAngle: 10, predictive: 0.6f, cooldown: 1000,
                                        cooldownOffset: 1000),
                                    new Shoot(25, index: 5, count: 2, shootAngle: 10, predictive: 0.8f, cooldown: 1000,
                                        cooldownOffset: 1000),
                                    new Shoot(25, index: 6, count: 3, shootAngle: 10, predictive: 1, cooldown: 1000,
                                        cooldownOffset: 1000),
                                    new Taunt(1, 6000, "Puny mortals! My {HP} HP will annihilate you!"),
                                    new Spawn("Henchman of Oryx", 5, cooldown: 15000),
                                    new HealthTransition(.2f, "prepareRage")
                                ),
                                new State("prepareRage",
                                    new Follow(.1f, 15, 3),
                                    new Taunt("Can't... keep... henchmen... alive... anymore! ARGHHH!!!"),
                                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                                    new Shoot(25, 30, fixedAngle: 0, index: 7, cooldown: 4000, cooldownOffset: 4000),
                                    new Shoot(25, 30, fixedAngle: 30, index: 8, cooldown: 4000, cooldownOffset: 4000),
                                    new TimedTransition("rage", 10000)
                                ),
                                new State("rage",
                                    new Follow(.1f, 15, 3),
                                    new Shoot(25, 30, index: 7, cooldown: 90000001, cooldownOffset: 8000),
                                    new Shoot(25, 30, index: 8, cooldown: 90000001, cooldownOffset: 8500),
                                    new Shoot(25, index: 0, count: 8, shootAngle: 45, cooldown: 1500, cooldownOffset: 1500),
                                    new Shoot(25, index: 1, count: 3, shootAngle: 10, cooldown: 1000, cooldownOffset: 1000),
                                    new Shoot(25, index: 2, count: 3, shootAngle: 10, predictive: 0.2f, cooldown: 1000,
                                        cooldownOffset: 1000),
                                    new Shoot(25, index: 3, count: 2, shootAngle: 10, predictive: 0.4f, cooldown: 1000,
                                        cooldownOffset: 1000),
                                    new Shoot(25, index: 4, count: 3, shootAngle: 10, predictive: 0.6f, cooldown: 1000,
                                        cooldownOffset: 1000),
                                    new Shoot(25, index: 5, count: 2, shootAngle: 10, predictive: 0.8f, cooldown: 1000,
                                        cooldownOffset: 1000),
                                    new Shoot(25, index: 6, count: 3, shootAngle: 10, predictive: 1, cooldown: 1000,
                                        cooldownOffset: 1000),
                                    new TossObject("Monstrosity Scarab", 7, 0, cooldown: 1000),
                                    new Taunt(1, 6000, "Puny mortals! My {HP} HP will annihilate you!")
                                )
                            ),
                            new Threshold(0.01f,
                                new ItemLoot("Potion of Vitality", 1)
                            ),
                            new Threshold(0.05f,
                                new ItemLoot("Potion of Attack", 0.3f),
                                new ItemLoot("Potion of Defense", 0.3f),
                                new ItemLoot("Potion of Wisdom", 0.3f)
                            ),
                            new Threshold(0.1f, 
                                new ItemLoot("Scarlet's Power", 0.002f),
                                new ItemLoot("Horned Suguyari", 0.002f)
                            ),
                            new Threshold(0.1f,
                                new TierLoot(10, LootType.Weapon, 0.07f),
                                new TierLoot(11, LootType.Weapon, 0.06f),
                                new TierLoot(12, LootType.Weapon, 0.05f),
                                new TierLoot(5, LootType.Ability, 0.07f),
                                new TierLoot(6, LootType.Ability, 0.05f),
                                new TierLoot(11, LootType.Armor, 0.07f),
                                new TierLoot(12, LootType.Armor, 0.06f),
                                new TierLoot(13, LootType.Armor, 0.05f),
                                new TierLoot(5, LootType.Ring, 0.06f)
                            )
                        );

            db.Init("Oryx the Mad God 1",
                new State("base",
                    new DropPortalOnDeath("Locked Wine Cellar Portal", 100, timeout: 120),
                    new HealthTransition(.2f, "rage"),
                    new State("Slow",
                        new Taunt("Fools! I still have {HP} hitpoints!"),
                        new Spawn("Minion of Oryx", 5, 0, 350000),
                        new Reproduce("Minion of Oryx", 10, 5, 1500),
                        new Shoot(25, 4, 10, 4, cooldown: 1000),
                        new TimedTransition("Dance 1", 20000)
                        ),
                    new State("Dance 1",
                        new Flash(0xf389E13, 0.5f, 60),
                        new Taunt("BE SILENT!!!"),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Shoot(50, 8, index: 6, cooldown: 700, cooldownOffset: 200),
                        new TossObject("Ring Element", 9, 24, 320000),
                        new TossObject("Ring Element", 9, 48, 320000),
                        new TossObject("Ring Element", 9, 72, 320000),
                        new TossObject("Ring Element", 9, 96, 320000),
                        new TossObject("Ring Element", 9, 120, 320000),
                        new TossObject("Ring Element", 9, 144, 320000),
                        new TossObject("Ring Element", 9, 168, 320000),
                        new TossObject("Ring Element", 9, 192, 320000),
                        new TossObject("Ring Element", 9, 216, 320000),
                        new TossObject("Ring Element", 9, 240, 320000),
                        new TossObject("Ring Element", 9, 264, 320000),
                        new TossObject("Ring Element", 9, 288, 320000),
                        new TossObject("Ring Element", 9, 312, 320000),
                        new TossObject("Ring Element", 9, 336, 320000),
                        new TossObject("Ring Element", 9, 360, 320000),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        //new Grenade(radius: 4, damage: 150, fixedAngle: new Random().Next(0, 359), range: 5, cooldown: 2000),
                        //new Grenade(radius: 4, damage: 150, fixedAngle: new Random().Next(0, 359), range: 5, cooldown: 2000),
                        //new Grenade(radius: 4, damage: 150, fixedAngle: new Random().Next(0, 359), range: 5, cooldown: 2000),
                        new TimedTransition("artifacts", 25000)
                        ),
                    new State("artifacts",
                        new Taunt("My Artifacts will protect me!"),
                        new Flash(0xf389E13, 0.5f, 60),
                        new Shoot(50, 3, index: 9, cooldown: 1500, cooldownOffset: 200),
                        new Shoot(50, 10, index: 8, cooldown: 2000, cooldownOffset: 200),
                        new Shoot(50, 10, index: 7, cooldown: 500, cooldownOffset: 200),

                        //Inner Elements
                        new TossObject("Guardian Element 1", 1, 0, 90000001, 1000),
                        new TossObject("Guardian Element 1", 1, 90, 90000001, 1000),
                        new TossObject("Guardian Element 1", 1, 180, 90000001, 1000),
                        new TossObject("Guardian Element 1", 1, 270, 90000001, 1000),
                        new TossObject("Guardian Element 2", 9, 0, 90000001, 1000),
                        new TossObject("Guardian Element 2", 9, 90, 90000001, 1000),
                        new TossObject("Guardian Element 2", 9, 180, 90000001, 1000),
                        new TossObject("Guardian Element 2", 9, 270, 90000001, 1000),
                        new TimedTransition("gaze", 25000)
                        ),

            #region gaze
                    new State("gaze",
                        new Taunt("All who looks upon my face shall die."),
                        new Shoot(range: 7, count: 2, cooldown: 1000, index: 1, shootAngle: 10,
                            cooldownOffset: 800),
                        new TimedTransition("Dance 2", 10000)
            #endregion gaze

                        ),

            #region Dance 2
                    new State("Dance 2",
                        new Flash(0xf389E13, 0.5f, 60),
                        new Taunt("Time for more dancing!"),
                        new Shoot(50, 8, index: 6, cooldown: 700, cooldownOffset: 200),
                        new TossObject("Ring Element", 9, 24, 320000),
                        new TossObject("Ring Element", 9, 48, 320000),
                        new TossObject("Ring Element", 9, 72, 320000),
                        new TossObject("Ring Element", 9, 96, 320000),
                        new TossObject("Ring Element", 9, 120, 320000),
                        new TossObject("Ring Element", 9, 144, 320000),
                        new TossObject("Ring Element", 9, 168, 320000),
                        new TossObject("Ring Element", 9, 192, 320000),
                        new TossObject("Ring Element", 9, 216, 320000),
                        new TossObject("Ring Element", 9, 240, 320000),
                        new TossObject("Ring Element", 9, 264, 320000),
                        new TossObject("Ring Element", 9, 288, 320000),
                        new TossObject("Ring Element", 9, 312, 320000),
                        new TossObject("Ring Element", 9, 336, 320000),
                        new TossObject("Ring Element", 9, 360, 320000),
                        new TimedTransition("Dance2, 1", 1000)
                        ),
                    new State("Dance2, 1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Shoot(0, index: 8, count: 4, shootAngle: 90, fixedAngle: 0, cooldown: 170),
                        new TimedTransition("Dance2, 2", 200)
                        ),
                    new State("Dance2, 2",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Shoot(0, index: 8, count: 4, shootAngle: 90, fixedAngle: 30, cooldown: 170),
                        new TimedTransition("Dance2, 3", 200)
                        ),
                    new State("Dance2, 3",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Shoot(0, index: 8, count: 4, shootAngle: 90, fixedAngle: 15, cooldown: 170),
                        new TimedTransition("Dance2, 4", 200)
                        ),
                    new State("Dance2, 4",
                        new Shoot(0, index: 8, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 170),
                        new TimedTransition("Dance2, 1", 200)
                        ),

            #endregion Dance 2
                    new State("rage",
                        new ChangeSize(10, 200),
                        new Taunt(.3f, "I HAVE HAD ENOUGH OF YOU!!!",
                            "ENOUGH!!!",
                            "DIE!!!"),
                        new Spawn("Minion of Oryx", 10, 0, 350000),
                        new Reproduce("Minion of Oryx", 10, 5, 1500),
                        new Shoot(count: 2, cooldown: 1500, index: 1, range: 7, shootAngle: 10,
                            cooldownOffset: 2000),
                        new Shoot(count: 5, cooldown: 1500, index: 16, range: 7, shootAngle: 10,
                            cooldownOffset: 2000),
                        new Follow(0.85f, range: 1, cooldown: 0),
                        new Flash(0xfFF0000, 0.5f, 9000001)
                        )
                    ),
                new Threshold(0.05f,
                    new ItemLoot("Potion of Attack", 1f, min: 3),
                    new ItemLoot("Potion of Defense", 1f, min: 3),
                    new ItemLoot("Potion of Life", 1f)
                ),
                new Threshold(0.1f, 
                    new ItemLoot("The Molten Cape", 0.002f),
                    new ItemLoot("Seal of Havoc", 0.002f)
                ),
                new Threshold(0.1f,
                    new TierLoot(10, LootType.Weapon, 0.07f, r: new LootDef.RarityModifiedData(1.0f, 1, alwaysRare: true)),
                    new TierLoot(11, LootType.Weapon, 0.06f, r: new LootDef.RarityModifiedData(1.0f, 1, alwaysRare: true)),
                    new TierLoot(5, LootType.Ability, 0.07f, r: new LootDef.RarityModifiedData(1.0f, 1, alwaysRare: true)),
                    new TierLoot(11, LootType.Armor, 0.07f, r: new LootDef.RarityModifiedData(1.0f, 1, alwaysRare: true)),
                    new TierLoot(5, LootType.Ring, 0.06f, r: new LootDef.RarityModifiedData(1.0f, 1, alwaysRare: true))
                )
            );
            db.Init("Ring Element",
                new State("base",
                    new State("base",
                        new Shoot(50, 12, index: 0, cooldown: 700, cooldownOffset: 200),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition("Despawn", 20000)
                        ),
                    new State("Despawn", //new Decay(time:0)
                        new Suicide()
                        )
                    )
            );
            db.Init("Minion of Oryx",
                new State("base",
                    new Wander(0.4f),
                    new Shoot(3, 3, 10, 0, cooldown: 1000),
                    new Shoot(3, 3, index: 1, shootAngle: 10, cooldown: 1000)
                    ),
                new TierLoot(7, LootType.Weapon, 0.2f),
                new ItemLoot("Magic Potion", 0.03f)
            );
            db.Init("Guardian Element 1",
                new State("base",
                    new State("base",
                        new Orbit(1, 1, target: "Oryx the Mad God 1", radiusVariance: 0),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Shoot(25, 3, 10, 0, cooldown: 1000),
                        new TimedTransition("Grow", 10000)
                        ),
                    new State("Grow",
                        new ChangeSize(100, 200),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Orbit(1, 1, target: "Oryx the Mad God 1", radiusVariance: 0),
                        new Shoot(3, 1, 10, 0, cooldown: 700),
                        new TimedTransition("Despawn", 10000)
                        ),
                    new State("Despawn",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Orbit(1, 1, target: "Oryx the Mad God 1", radiusVariance: 0),
                        new ChangeSize(100, 100),
                        new Suicide()
                        )
                    )
            );
            db.Init("Guardian Element 2",
                new State("base",
                    new State("base",
                        new Orbit(1.3f, 9, target: "Oryx the Mad God 1", radiusVariance: 0),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Shoot(25, 3, 10, 0, cooldown: 1000),
                        new TimedTransition("Despawn", 20000)
                        ),
                    new State("Despawn", new Suicide()
                        )
                    )
            );
            db.Init("Henchman of Oryx",
                    new State("base",
                        new State("Attack",
                            new Prioritize(
                                new Orbit(.2f, 2, target: "Oryx the Mad God 2", radiusVariance: 1),
                                new Wander(.3f)
                                ),
                        new Shoot(15, predictive: 1, cooldown: 2500),
                        new Shoot(10, count: 3, shootAngle: 10, index: 1, cooldown: 2500),
                        new Spawn("Vintner of Oryx", maxChildren: 1, initialSpawn: 1, cooldown: 5000),
                        //  new Spawn("Bile of Oryx", maxChildren: 1, initialSpawn: 1, cooldown: 5000),
                        new Spawn("Aberrant of Oryx", maxChildren: 1, initialSpawn: 1, cooldown: 5000),
                        new Spawn("Monstrosity of Oryx", maxChildren: 1, initialSpawn: 1, cooldown: 5000),
                        new Spawn("Abomination of Oryx", maxChildren: 1, initialSpawn: 1, cooldown: 5000)
                                ),
                        new State("Suicide",
                            new Decay(0)
                                 )
                                )
                                );
            db.Init("Monstrosity of Oryx",
                new State("base",
                    new State("Wait", new PlayerWithinTransition(15, "Attack")),
                    new State("Attack",
                        new TimedTransition("Wait", 10000),
                    new Prioritize(
                        new Orbit(.1f, 6, target: "Oryx the Mad God 2", radiusVariance: 3),
                        new Follow(.1f, acquireRange: 15),
                        new Wander(.2f)
                        ),
                     new TossObject("Monstrosity Scarab", cooldown: 10000, range: 1, angle: 0, coolDownOffset: 1000)
                     )
                     ));
            db.Init("Monstrosity Scarab",
                new State("base",
                    new State("Attack",
                    new State("Charge",
                        new Prioritize(
                            new Charge(range: 25, coolDown: 1000),
                            new Wander(.3f)
                            ),
                        new PlayerWithinTransition(1, "Boom")
                        ),
                    new State("Boom",
                        new Shoot(1, count: 16, shootAngle: 360 / 16, fixedAngle: 0),
                        new Decay(0)
                       )
                       )
                       )
                       );
            db.Init("Vintner of Oryx",
                new State("base",
                    new State("Attack",
                        new Prioritize(
                            new Protect(1, "Oryx the Mad God 2", protectionRange: 4, reprotectRange: 3),
                            new Charge(speed: 1, range: 15, coolDown: 2000),
                            new Protect(1, "Henchman of Oryx"),
                            new StayBack(1, 15),
                            new Wander(1)
                        ),
                        new Shoot(10, cooldown: 250)
                        )
                        ));
            db.Init("Aberrant of Oryx",
               new State("base",
                   new Prioritize(
                       new Protect(.2f, "Oryx the Mad God 2"),
                       new Wander(.7f)
                       ),
                   new State("Wait", new PlayerWithinTransition(15, "Attack")),
                   new State("Attack",
                   new TimedTransition("Wait", 10000),
                   new State("Randomize",
                       new TimedTransition("Toss1", randomized: true, time: 100),
                       new TimedTransition("Toss2", randomized: true, time: 100),
                       new TimedTransition("Toss3", randomized: true, time: 100),
                       new TimedTransition("Toss4", randomized: true, time: 100),
                       new TimedTransition("Toss5", randomized: true, time: 100),
                       new TimedTransition("Toss6", randomized: true, time: 100),
                       new TimedTransition("Toss7", randomized: true, time: 100),
                       new TimedTransition("Toss8", randomized: true, time: 100)
                      ),
                   new State("Toss1",
                       new TossObject("Aberrant Blaster", cooldown: 40000, range: 5, angle: 0),
                       new TimedTransition("Randomize", 4900)
                       ),
                   new State("Toss2",
                       new TossObject("Aberrant Blaster", cooldown: 40000, range: 5, angle: 45),
                       new TimedTransition("Randomize", 4900)
                       ),
                   new State("Toss3",
                       new TossObject("Aberrant Blaster", cooldown: 40000, range: 5, angle: 90),
                       new TimedTransition("Randomize", 4900)
                       ),
                   new State("Toss4",
                       new TossObject("Aberrant Blaster", cooldown: 40000, range: 5, angle: 135),
                       new TimedTransition("Randomize", 4900)
                       ),
                   new State("Toss5",
                       new TossObject("Aberrant Blaster", cooldown: 40000, range: 5, angle: 180),
                       new TimedTransition("Randomize", 4900)
                       ),
                   new State("Toss6",
                       new TossObject("Aberrant Blaster", cooldown: 40000, range: 5, angle: 225),
                       new TimedTransition("Randomize", 4900)
                       ),
                   new State("Toss7",
                       new TossObject("Aberrant Blaster", cooldown: 40000, range: 5, angle: 270),
                       new TimedTransition("Randomize", 4900)
                       ),
                   new State("Toss8",
                       new TossObject("Aberrant Blaster", cooldown: 40000, range: 5, angle: 315),
                       new TimedTransition("Randomize", 4900)
                       ))
                       ));
            db.Init("Aberrant Blaster",
                new State("base",
                    new State("Wait",
                        new PlayerWithinTransition(3, "Boom")
                        ),
                    new State("Boom",
                        new Shoot(10, count: 5, shootAngle: 7),
                        new Decay(0)
                        )
                        )
                        );
            db.Init("Bile of Oryx",
                new State("base",
                    new Prioritize(
                        new Protect(.4f, "Oryx the Mad God 2", protectionRange: 5, reprotectRange: 4),
                        new Wander(.5f)
                        )//,
                         //new Spawn("Purple Goo", maxChildren: 20, initialSpawn: 0, cooldown: 1000)
                    )
                    );
            db.Init("Abomination of Oryx",
                new State("base",
                    new State("Shoot",
                        new Shoot(1, 3, shootAngle: 5, index: 0),
                        new Shoot(1, 5, shootAngle: 5, index: 1),
                        new Shoot(1, 7, shootAngle: 5, index: 2),
                        new Shoot(1, 5, shootAngle: 5, index: 3),
                        new Shoot(1, 3, shootAngle: 5, index: 4),
                        new TimedTransition("Wait", 1000)
                        ),
                    new State("Wait",
                        new PlayerWithinTransition(2, "Shoot")),
                    new Prioritize(
                        new Charge(3, 10, 3000),
                        new Wander(.5f))
                        )
                        ); ;




        }
    }
}
