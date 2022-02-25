#region

using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

#endregion

//FULLY SHATTERS
//BEHAVIORS
//MADE BY
//MIKE (Qkm)
//MOISTED ON BY PATPOT

namespace RotMG.Game.Logic.Database
{
    class Shatters
    {
        private _ Shatters = () => Behav()
        #region restofmobs
            db.Init("shtrs Stone Paladin",
                new State("base",
                    new State("Idle",
                        new Prioritize(
                            new Wander(0.8f)
                            ),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Reproduce(densityMax: 4),
                        new PlayerWithinTransition(8, "Attacking")
                        ),
                    new State("Attacking",
                        new State("Bullet",
                            new Shoot(1, 4, cooldown: 10000, fixedAngle: 90, cooldownOffset: 0, shootAngle: 90),
                            new Shoot(1, 4, cooldown: 10000, fixedAngle: 100, cooldownOffset: 200, shootAngle: 90),
                            new Shoot(1, 4, cooldown: 10000, fixedAngle: 110, cooldownOffset: 400, shootAngle: 90),
                            new Shoot(1, 4, cooldown: 10000, fixedAngle: 120, cooldownOffset: 600, shootAngle: 90),
                            new Shoot(1, 4, cooldown: 10000, fixedAngle: 130, cooldownOffset: 800, shootAngle: 90),
                            new Shoot(1, 4, cooldown: 10000, fixedAngle: 140, cooldownOffset: 1000, shootAngle: 90),
                            new Shoot(1, 4, cooldown: 10000, fixedAngle: 150, cooldownOffset: 1200, shootAngle: 90),
                            new Shoot(1, 4, cooldown: 10000, fixedAngle: 160, cooldownOffset: 1400, shootAngle: 90),
                            new Shoot(1, 4, cooldown: 10000, fixedAngle: 170, cooldownOffset: 1600, shootAngle: 90),
                            new Shoot(1, 4, cooldown: 10000, fixedAngle: 180, cooldownOffset: 1800, shootAngle: 90),
                            new Shoot(1, 4, cooldown: 10000, fixedAngle: 180, cooldownOffset: 2000, shootAngle: 45),
                            new Shoot(1, 4, cooldown: 10000, fixedAngle: 180, cooldownOffset: 0, shootAngle: 90),
                            new Shoot(1, 4, cooldown: 10000, fixedAngle: 170, cooldownOffset: 200, shootAngle: 90),
                            new Shoot(1, 4, cooldown: 10000, fixedAngle: 160, cooldownOffset: 400, shootAngle: 90),
                            new Shoot(1, 4, cooldown: 10000, fixedAngle: 150, cooldownOffset: 600, shootAngle: 90),
                            new Shoot(1, 4, cooldown: 10000, fixedAngle: 140, cooldownOffset: 800, shootAngle: 90),
                            new Shoot(1, 4, cooldown: 10000, fixedAngle: 130, cooldownOffset: 1000, shootAngle: 90),
                            new Shoot(1, 4, cooldown: 10000, fixedAngle: 120, cooldownOffset: 1200, shootAngle: 90),
                            new Shoot(1, 4, cooldown: 10000, fixedAngle: 110, cooldownOffset: 1400, shootAngle: 90),
                            new Shoot(1, 4, cooldown: 10000, fixedAngle: 100, cooldownOffset: 1600, shootAngle: 90),
                            new Shoot(1, 4, cooldown: 10000, fixedAngle: 90, cooldownOffset: 1800, shootAngle: 90),
                            new Shoot(1, 4, cooldown: 10000, fixedAngle: 90, cooldownOffset: 2000, shootAngle: 22.5f),
                            new TimedTransition("Wait", 2000)
                            ),
                        new State("Wait",
                            new Follow(0.4f, range: 2),
                            new Flash(0xff00ff00, 0.1f, 20),
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new TimedTransition("Bullet", 2000)
                            ),
                        new NoPlayerWithinTransition(13, "Idle")
                        )
                    )
            )
            db.Init("shtrs Stone Knight",
            new State("base",
                new State("Follow",
                        new Follow(0.6f, 10, 5),
                        new PlayerWithinTransition(5, "Charge")
                    ),
                    new State("Charge",
                        new TimedTransition("Follow", 2000),
                        new Charge(2.5f, 6, cooldown: 2000),
                        new Shoot(5, 16, index: 0, cooldown: 2400, cooldownOffset: 400)
                        )
                    )
            )
        db.Init("shtrs Lava Souls",
                new State("base",
                    new State("active",
                        new Follow(.7f, range: 0),
                        new PlayerWithinTransition(2, "blink")
                    ),
                    new State("blink",
                        new Flash(0xfFF0000, flashRepeats: 10000, flashPeriod: 0.1f),
                        new TimedTransition("explode", 2000)
                    ),
                    new State("explode",
                        new Flash(0xfFF0000, flashRepeats: 5, flashPeriod: 0.1f),
                        new Shoot(5, 9),
                        new Suicide()
                    )
                )
            )
            db.Init("shtrs Glassier Archmage",
            new State("base",
                    new StayBack(0.5f, 5),
                new State("1st",
                    new Follow(0.8f, range: 7),
                    new Shoot(20, index: 2, count: 1, cooldown: 50),
                    new TimedTransition("next", 5000)
                    ),
                new State("next",
                    new Shoot(35, index: 0, count: 25, cooldown: 5000),
                    new TimedTransition("1st", 25)
                    )
               )
        )
            db.Init("shtrs Ice Adept",
            new State("base",
                new State("Main",
                    new TimedTransition("Throw", 5000),
                    new Follow(0.8f, range: 1),
                    new Shoot(10, 1, index: 0, cooldown: 100, predictive: 1),
                    new Shoot(10, 3, index: 1, shootAngle: 10, cooldown: 4000, predictive: 1)
                ),
                new State("Throw",
                    new TossObject("shtrs Ice Portal", 5, cooldown: 8000, cooldownOffset: 7000),
                    new TimedTransition("Main", 1000)
                )
                  ))

            db.Init("shtrs Fire Adept",
            new State("base",
                new State("Main",
                    new TimedTransition("Throw", 5000),
                    new Follow(0.8f, range: 1),
                    new Shoot(10, 1, index: 0, cooldown: 100, predictive: 1),
                    new Shoot(10, 3, index: 1, shootAngle: 10, cooldown: 4000, predictive: 1)
                ),
                new State("Throw",
                    new TossObject("shtrs Fire Portal", 5, cooldown: 8000, cooldownOffset: 7000),
                    new TimedTransition("Main", 1000)
                )
                  ))
        #endregion restofmobs
        #region generators
            db.Init("shtrs MagiGenerators",
            new State("base",
                new State("Main",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Shoot(15, 10, cooldown: 1000),
                    new Shoot(15, 1, index: 1, cooldown: 2500),
                    new EntitiesNotExistsTransition(30, "Hide", "Shtrs Twilight Archmage", "shtrs Inferno", "shtrs Blizzard")
                ),
                new State("Hide",
                    new SetAltTexture(1),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable)
                    ),
                new State("vulnerable",
                    new ConditionalEffect(ConditionEffectIndex.Armored)
                    ),
                new State("Despawn",
                    new Decay()
                    )
                  ))
        #endregion generators
        #region portals
            db.Init("shtrs Ice Portal",
                new State("base",
                    new State("Idle",
                        new TimedTransition("Spin", 1000)
                    ),
                    new State("Spin",
                            new Shoot(0, index: 0, count: 6, shootAngle: 60, fixedAngle: 0, cooldown: 1200),
                            new Shoot(0, index: 0, count: 6, shootAngle: 60, fixedAngle: 15, cooldown: 1200, cooldownOffset: 200),
                            new Shoot(0, index: 0, count: 6, shootAngle: 60, fixedAngle: 30, cooldown: 1200, cooldownOffset: 400),
                            new Shoot(0, index: 0, count: 6, shootAngle: 60, fixedAngle: 45, cooldown: 1200, cooldownOffset: 600),
                            new Shoot(0, index: 0, count: 6, shootAngle: 60, fixedAngle: 60, cooldown: 1200, cooldownOffset: 800),
                            new Shoot(0, index: 0, count: 6, shootAngle: 60, fixedAngle: 75, cooldown: 1200, cooldownOffset: 1000),
                            new TimedTransition("Pause", 1200)
                    ),
                    new State("Pause",
                       new TimedTransition("Idle", 5000)
                    )
                )
            )
            db.Init("shtrs Fire Portal",
                new State("base",
                    new State("Idle",
                        new TimedTransition("Spin", 1000)
                    ),
                    new State("Spin",
                            new Shoot(0, index: 0, count: 6, shootAngle: 60, fixedAngle: 0, cooldown: 1200),
                            new Shoot(0, index: 0, count: 6, shootAngle: 60, fixedAngle: 15, cooldown: 1200, cooldownOffset: 200),
                            new Shoot(0, index: 0, count: 6, shootAngle: 60, fixedAngle: 30, cooldown: 1200, cooldownOffset: 400),
                            new Shoot(0, index: 0, count: 6, shootAngle: 60, fixedAngle: 45, cooldown: 1200, cooldownOffset: 600),
                            new Shoot(0, index: 0, count: 6, shootAngle: 60, fixedAngle: 60, cooldown: 1200, cooldownOffset: 800),
                            new Shoot(0, index: 0, count: 6, shootAngle: 60, fixedAngle: 75, cooldown: 1200, cooldownOffset: 1000),
                            new TimedTransition("Pause", 1200)
                    ),
                    new State("Pause",
                       new TimedTransition("Idle", 5000)
                    )
                )
            )
            db.Init("shtrs Ice Shield",
                new State("base",
                    new HealthTransition(.2f, "Death"),
                    new State("base",
                        new Charge(0.6f, 7, cooldown: 5000),
                        new Shoot(3, 6, 60, index: 0, fixedAngle: 0, cooldown: 1200),
                        new Shoot(3, 6, 60, index: 0, fixedAngle: 10, cooldown: 1200, cooldownOffset: 200),
                        new Shoot(3, 6, 60, index: 0, fixedAngle: 20, cooldown: 1200, cooldownOffset: 400),
                        new Shoot(3, 6, 60, index: 0, fixedAngle: 30, cooldown: 1200, cooldownOffset: 600),
                        new Shoot(3, 6, 60, index: 0, fixedAngle: 40, cooldown: 1200, cooldownOffset: 800),
                        new Shoot(3, 6, 60, index: 0, fixedAngle: 50, cooldown: 1200, cooldownOffset: 1000)
                    ),
                    new State("Death",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Shoot(13, 45, 8, index: 1, fixedAngle: 1, cooldown: 10000),
                        new Timed(1000, new Suicide())
                    )
                )
            )
            db.Init("shtrs Ice Shield 2",
            new State("base",
                new HealthTransition(0.3f, "Death"),
                new State("base",
                    new Orbit(0.5f, 5, 1, "shtrs Twilight Archmage"),
                    new Charge(0.1f, 6, cooldown: 10000),
                new Shoot(13, 10, 8, index: 0, cooldown: 1000, fixedAngle: 1)
                ),
            new State("Death",
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new Shoot(13, 45, index: 1, cooldown: 10000),
                new Timed(1000, new Suicide())
                )
                )
            )
        #endregion portals
        #region 1stboss
            db.Init("shtrs Bridge Sentinel",
                new State("base",
                    new Shoot(2, index: 5, count: 3, fixedAngle: 0, cooldown: 10),
                    new Shoot(2, index: 5, count: 3, fixedAngle: 45, cooldown: 10),
                    new Shoot(2, index: 5, count: 3, fixedAngle: 90, cooldown: 10),
                    new Shoot(2, index: 5, count: 3, fixedAngle: 135, cooldown: 10),
                    new Shoot(2, index: 5, count: 3, fixedAngle: 180, cooldown: 10),
                    new HealthTransition(0.1f, "Death"),
                    new State("Idle",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new PlayerWithinTransition(15, "Close Bridge")
                        ),
                    new State("Close Bridge",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Order(46, "shtrs Bridge Closer", "Closer"),
                        new TimedTransition("Close Bridge2", 5000)
                        ),
                    new State("Close Bridge2",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Order(46, "shtrs Bridge Closer2", "Closer"),
                        new TimedTransition("Close Bridge3", 5000)
                        ),
                    new State("Close Bridge3",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Order(46, "shtrs Bridge Closer3", "Closer"),
                        new TimedTransition("Close Bridge4", 5000)
                        ),
                    new State("Close Bridge4",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Order(46, "shtrs Bridge Closer4", "Closer"),
                        new TimedTransition("BEGIN", 6000)
                        ),
                    new State("BEGIN",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntitiesNotExistsTransition(30, "Wake", "shtrs Bridge Obelisk A", "shtrs Bridge Obelisk B", "shtrs Bridge Obelisk D", "shtrs Bridge Obelisk E")
                    ),
                        new State("Wake",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt("Who has woken me...? Leave this place."),
                        new Timed(2100, new Shoot(15, 15, 12, index: 0, fixedAngle: 180, cooldown: 700, cooldownOffset: 3000)),
                        new TimedTransition("Swirl Shot", 8000)
                        ),
                        new State("Swirl Shot",
                            new Taunt("Go."),
                            new TimedTransition("Blobomb", 10000),
                            new State("Swirl1",
                            new Shoot(50, index: 0, count: 1, shootAngle: 102, fixedAngle: 102, cooldown: 6000),
                            new Shoot(50, index: 0, count: 1, shootAngle: 114, fixedAngle: 114, cooldown: 6000, cooldownOffset: 200),
                            new Shoot(50, index: 0, count: 1, shootAngle: 126, fixedAngle: 126, cooldown: 6000, cooldownOffset: 400),
                            new Shoot(50, index: 0, count: 1, shootAngle: 138, fixedAngle: 138, cooldown: 6000, cooldownOffset: 600),
                            new Shoot(50, index: 0, count: 1, shootAngle: 150, fixedAngle: 150, cooldown: 6000, cooldownOffset: 800),
                            new Shoot(50, index: 0, count: 1, shootAngle: 162, fixedAngle: 162, cooldown: 6000, cooldownOffset: 1000),
                            new Shoot(50, index: 0, count: 1, shootAngle: 174, fixedAngle: 174, cooldown: 6000, cooldownOffset: 1200),
                            new Shoot(50, index: 0, count: 1, shootAngle: 186, fixedAngle: 186, cooldown: 6000, cooldownOffset: 1400),
                            new Shoot(50, index: 0, count: 1, shootAngle: 198, fixedAngle: 198, cooldown: 6000, cooldownOffset: 1600),
                            new Shoot(50, index: 0, count: 1, shootAngle: 210, fixedAngle: 210, cooldown: 6000, cooldownOffset: 1800),
                            new Shoot(50, index: 0, count: 1, shootAngle: 222, fixedAngle: 222, cooldown: 6000, cooldownOffset: 2000),
                            new Shoot(50, index: 0, count: 1, shootAngle: 234, fixedAngle: 234, cooldown: 6000, cooldownOffset: 2200),
                            new Shoot(50, index: 0, count: 1, shootAngle: 246, fixedAngle: 246, cooldown: 6000, cooldownOffset: 2400),
                            new Shoot(50, index: 0, count: 1, shootAngle: 258, fixedAngle: 258, cooldown: 6000, cooldownOffset: 2600),
                            new Shoot(50, index: 0, count: 1, shootAngle: 270, fixedAngle: 270, cooldown: 6000, cooldownOffset: 2800),
                            new Shoot(50, index: 0, count: 1, shootAngle: 282, fixedAngle: 282, cooldown: 6000, cooldownOffset: 3000),
                            new Shoot(50, index: 0, count: 1, shootAngle: 270, fixedAngle: 270, cooldown: 6000, cooldownOffset: 3200),
                            new Shoot(50, index: 0, count: 1, shootAngle: 258, fixedAngle: 258, cooldown: 6000, cooldownOffset: 3400),
                            new Shoot(50, index: 0, count: 1, shootAngle: 246, fixedAngle: 246, cooldown: 6000, cooldownOffset: 3600),
                            new Shoot(50, index: 0, count: 1, shootAngle: 234, fixedAngle: 234, cooldown: 6000, cooldownOffset: 3800),
                            new Shoot(50, index: 0, count: 1, shootAngle: 222, fixedAngle: 222, cooldown: 6000, cooldownOffset: 4000),
                            new Shoot(50, index: 0, count: 1, shootAngle: 210, fixedAngle: 210, cooldown: 6000, cooldownOffset: 4200),
                            new Shoot(50, index: 0, count: 1, shootAngle: 198, fixedAngle: 198, cooldown: 6000, cooldownOffset: 4400),
                            new Shoot(50, index: 0, count: 1, shootAngle: 186, fixedAngle: 186, cooldown: 6000, cooldownOffset: 4600),
                            new Shoot(50, index: 0, count: 1, shootAngle: 174, fixedAngle: 174, cooldown: 6000, cooldownOffset: 4800),
                            new Shoot(50, index: 0, count: 1, shootAngle: 162, fixedAngle: 162, cooldown: 6000, cooldownOffset: 5000),
                            new Shoot(50, index: 0, count: 1, shootAngle: 150, fixedAngle: 150, cooldown: 6000, cooldownOffset: 5200),
                            new Shoot(50, index: 0, count: 1, shootAngle: 138, fixedAngle: 138, cooldown: 6000, cooldownOffset: 5400),
                            new Shoot(50, index: 0, count: 1, shootAngle: 126, fixedAngle: 126, cooldown: 6000, cooldownOffset: 5600),
                            new Shoot(50, index: 0, count: 1, shootAngle: 114, fixedAngle: 114, cooldown: 6000, cooldownOffset: 5800),
                            new Shoot(50, index: 0, count: 1, shootAngle: 102, fixedAngle: 102, cooldown: 6000, cooldownOffset: 6000),
                            new TimedTransition("Swirl1", 6000)
                            )
                            ),
                            new State("Blobomb",
                            new Taunt("You live still? DO NOT TEMPT FATE!"),
                            new Taunt("CONSUME!"),
                            new Order(20, "shtrs blobomb maker", "Spawn"),
                            new EntitiesNotExistsTransition(30, "SwirlAndShoot", "shtrs Blobomb")
                                ),
                                new State("SwirlAndShoot",
                                    new TimedTransition("Blobomb", 10000),
                                    new Taunt("FOOLS! YOU DO NOT UNDERSTAND!"),
                                    new ChangeSize(20, 130),
                            new Shoot(15, 15, 11, index: 0, fixedAngle: 180, cooldown: 700, cooldownOffset: 700),
                                    new State("Swirl1_2",
                            new Shoot(50, index: 0, count: 1, shootAngle: 102, fixedAngle: 102, cooldown: 6000),
                            new Shoot(50, index: 0, count: 1, shootAngle: 114, fixedAngle: 114, cooldown: 6000, cooldownOffset: 200),
                            new Shoot(50, index: 0, count: 1, shootAngle: 126, fixedAngle: 126, cooldown: 6000, cooldownOffset: 400),
                            new Shoot(50, index: 0, count: 1, shootAngle: 138, fixedAngle: 138, cooldown: 6000, cooldownOffset: 600),
                            new Shoot(50, index: 0, count: 1, shootAngle: 150, fixedAngle: 150, cooldown: 6000, cooldownOffset: 800),
                            new Shoot(50, index: 0, count: 1, shootAngle: 162, fixedAngle: 162, cooldown: 6000, cooldownOffset: 1000),
                            new Shoot(50, index: 0, count: 1, shootAngle: 174, fixedAngle: 174, cooldown: 6000, cooldownOffset: 1200),
                            new Shoot(50, index: 0, count: 1, shootAngle: 186, fixedAngle: 186, cooldown: 6000, cooldownOffset: 1400),
                            new Shoot(50, index: 0, count: 1, shootAngle: 198, fixedAngle: 198, cooldown: 6000, cooldownOffset: 1600),
                            new Shoot(50, index: 0, count: 1, shootAngle: 210, fixedAngle: 210, cooldown: 6000, cooldownOffset: 1800),
                            new Shoot(50, index: 0, count: 1, shootAngle: 222, fixedAngle: 222, cooldown: 6000, cooldownOffset: 2000),
                            new Shoot(50, index: 0, count: 1, shootAngle: 234, fixedAngle: 234, cooldown: 6000, cooldownOffset: 2200),
                            new Shoot(50, index: 0, count: 1, shootAngle: 246, fixedAngle: 246, cooldown: 6000, cooldownOffset: 2400),
                            new Shoot(50, index: 0, count: 1, shootAngle: 258, fixedAngle: 258, cooldown: 6000, cooldownOffset: 2600),
                            new Shoot(50, index: 0, count: 1, shootAngle: 270, fixedAngle: 270, cooldown: 6000, cooldownOffset: 2800),
                            new Shoot(50, index: 0, count: 1, shootAngle: 282, fixedAngle: 282, cooldown: 6000, cooldownOffset: 3000),
                            new Shoot(50, index: 0, count: 1, shootAngle: 270, fixedAngle: 270, cooldown: 6000, cooldownOffset: 3200),
                            new Shoot(50, index: 0, count: 1, shootAngle: 258, fixedAngle: 258, cooldown: 6000, cooldownOffset: 3400),
                            new Shoot(50, index: 0, count: 1, shootAngle: 246, fixedAngle: 246, cooldown: 6000, cooldownOffset: 3600),
                            new Shoot(50, index: 0, count: 1, shootAngle: 234, fixedAngle: 234, cooldown: 6000, cooldownOffset: 3800),
                            new Shoot(50, index: 0, count: 1, shootAngle: 222, fixedAngle: 222, cooldown: 6000, cooldownOffset: 4000),
                            new Shoot(50, index: 0, count: 1, shootAngle: 210, fixedAngle: 210, cooldown: 6000, cooldownOffset: 4200),
                            new Shoot(50, index: 0, count: 1, shootAngle: 198, fixedAngle: 198, cooldown: 6000, cooldownOffset: 4400),
                            new Shoot(50, index: 0, count: 1, shootAngle: 186, fixedAngle: 186, cooldown: 6000, cooldownOffset: 4600),
                            new Shoot(50, index: 0, count: 1, shootAngle: 174, fixedAngle: 174, cooldown: 6000, cooldownOffset: 4800),
                            new Shoot(50, index: 0, count: 1, shootAngle: 162, fixedAngle: 162, cooldown: 6000, cooldownOffset: 5000),
                            new Shoot(50, index: 0, count: 1, shootAngle: 150, fixedAngle: 150, cooldown: 6000, cooldownOffset: 5200),
                            new Shoot(50, index: 0, count: 1, shootAngle: 138, fixedAngle: 138, cooldown: 6000, cooldownOffset: 5400),
                            new Shoot(50, index: 0, count: 1, shootAngle: 126, fixedAngle: 126, cooldown: 6000, cooldownOffset: 5600),
                            new Shoot(50, index: 0, count: 1, shootAngle: 114, fixedAngle: 114, cooldown: 6000, cooldownOffset: 5800),
                            new Shoot(50, index: 0, count: 1, shootAngle: 102, fixedAngle: 102, cooldown: 6000, cooldownOffset: 6000),
                            new TimedTransition("Swirl1_2", 6000)
                            )
                                ),
                        new State("Death",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new CopyDamageOnDeath("shtrs Loot Balloon Bridge"),
                            new Taunt("I tried to protect you... I have failed. You release a great evil upon this realm...."),
                            new TimedTransition("Suicide", 2000)
                            ),
                        new State("Suicide",
                            new Shoot(35, index: 0, count: 30),
                            new Order(1, "shtrs Chest Spawner 1", "Open"),
                            new Suicide()
                    )
                )
            )
        #endregion 1stboss
        #region blobomb
            db.Init("shtrs Blobomb",
                new State("base",
                    new State("active",
                        new Follow(.7f, acquireRange: 40, range: 0),
                        new PlayerWithinTransition(2, "blink")
                    ),
                    new State("blink",
                        new Flash(0xfFF0000, flashRepeats: 10000, flashPeriod: 0.1f),
                        new TimedTransition("explode", 2000)
                    ),
                    new State("explode",
                        new Flash(0xfFF0000, flashRepeats: 5, flashPeriod: 0.1f),
                        new Shoot(30, 36, fixedAngle: 0),
                        new Suicide()
                    )
                )
            )
        #endregion blobomb
        #region 2ndboss
            db.Init("shtrs Twilight Archmage",
                new State("base",
                    new HealthTransition(.1f, "Death"),
                    new State("Idle",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntitiesNotExistsTransition(6, "Wake", "shtrs Archmage of Flame")
                    ),
                    new State("Wake",
                        new State("Comment1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new SetAltTexture(1),
                            new Taunt("Ha...ha........hahahahahaha! You will make a fine sacrifice!"),
                            new TimedTransition("Comment2", 3000)
                        ),
                        new SetAltTexture(1),
                        new State("Comment2",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Taunt("You will find that it was...unwise...to wake me."),
                            new TimedTransition("Comment3", 1000)
                        ),
                        new State("Comment3",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new SetAltTexture(1),
                            new Taunt("Let us see what can conjure up!"),
                            new TimedTransition("Comment4", 1000)
                        ),
                        new State("Comment4",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new SetAltTexture(1),
                            new Taunt("I will freeze the life from you!"),
                            new TimedTransition("Shoot", 1000)
                        )
                    ),
                    new State("TossShit",
                        new TossObject("shtrs Ice Portal", 10, cooldown: 25000),
                        new TossObject("shtrs FireBomb", 15, cooldown: 25000),
                        new TossObject("shtrs FireBomb", 15, cooldown: 25000),
                        new TossObject("shtrs FireBomb", 7, cooldown: 25000),
                        new TossObject("shtrs FireBomb", 1, cooldown: 25000),
                        new TossObject("shtrs FireBomb", 4, cooldown: 25000),
                        new TossObject("shtrs FireBomb", 8, cooldown: 25000),
                        new TossObject("shtrs FireBomb", 9, cooldown: 25000),        //NOT IN USE!
                        new TossObject("shtrs FireBomb", 5, cooldown: 25000),
                        new TossObject("shtrs FireBomb", 7, cooldown: 25000),
                        new TossObject("shtrs FireBomb", 11, cooldown: 25000),
                        new TossObject("shtrs FireBomb", 13, cooldown: 25000),
                        new TossObject("shtrs FireBomb", 12, cooldown: 25000),
                        new TossObject("shtrs FireBomb", 10, cooldown: 25000),
                        new Spawn("shtrs Ice Shield 2", maxChildren: 1, initialSpawn: 1, cooldown: 25000),
                        new TimedTransition("Shoot", 1)
                        ),
                  new State("Shoot",
                    new Shoot(15, 5, 5, index: 1, cooldown: 800),
                    new Shoot(15, 5, 5, index: 1, cooldown: 800, cooldownOffset: 200),
                    new Shoot(15, 5, 5, index: 1, cooldown: 800, cooldownOffset: 400),
                    new Shoot(15, 5, 5, index: 1, cooldown: 800, cooldownOffset: 600),
                    new Shoot(15, 5, 5, index: 1, cooldown: 800, cooldownOffset: 800),
                    new TimedTransition("Shoot", 800),
                    new HealthTransition(0.50f, "Pre Birds")
                        ),
                    new State("Pre Birds",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt("You leave me no choice...Inferno! Blizzard!"),
                        new TimedTransition("Birds", 2000)
                        ),
                    new State("Birds",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Spawn("shtrs Inferno", maxChildren: 1, initialSpawn: 1, cooldown: 1000000000),
                        new Spawn("shtrs Blizzard", maxChildren: 1, initialSpawn: 1, cooldown: 1000000000),
                        new EntitiesNotExistsTransition(500, "PreNewShit2", "shtrs Inferno", "shtrs Blizzard")
                        ),
                    new State("PreNewShit2",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new ReturnToSpawn(1),
                        new TimedTransition("NewShit2", 3000)
                        ),
                    new State("NewShit2",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new MoveTo2(0, -6, 1),
                        new TimedTransition("Active2", 3000)
                        ),
                    new State("Active2",
                        new Taunt("THE POWER...IT CONSUMES ME!"),
                        new Order(2, "shtrs MagiGenerators", "vulnerable"),
                        new Shoot(15, 20, index: 2, cooldown: 100000000, cooldownOffset: 100),
                        new Shoot(15, 20, index: 3, cooldown: 100000000, cooldownOffset: 200),
                        new Shoot(15, 20, index: 4, cooldown: 100000000, cooldownOffset: 300),
                        new Shoot(15, 20, index: 2, cooldown: 100000000, cooldownOffset: 400),
                        new Shoot(15, 20, index: 5, cooldown: 100000000, cooldownOffset: 500),
                        new Shoot(15, 20, index: 6, cooldown: 100000000, cooldownOffset: 600),
                        new TimedTransition("NewShit3", 2000)
                        ),
                    new State("NewShit3",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new MoveTo2(4, 0, 1),
                        new TimedTransition("Active3", 3000)
                        ),
                    new State("Active3",
                        new Taunt("THE POWER...IT CONSUMES ME!"),
                        new Order(2, "shtrs MagiGenerators", "vulnerable"),
                        new Shoot(15, 20, index: 2, cooldown: 100000000, cooldownOffset: 100),
                        new Shoot(15, 20, index: 3, cooldown: 100000000, cooldownOffset: 200),
                        new Shoot(15, 20, index: 4, cooldown: 100000000, cooldownOffset: 300),
                        new Shoot(15, 20, index: 2, cooldown: 100000000, cooldownOffset: 400),
                        new Shoot(15, 20, index: 5, cooldown: 100000000, cooldownOffset: 500),
                        new Shoot(15, 20, index: 6, cooldown: 100000000, cooldownOffset: 600),
                        new TimedTransition("NewShit4", 2000)
                        ),
                    new State("NewShit4",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new MoveTo2(0, 13, 1),
                        new TimedTransition("Active4", 3000)
                            ),
                    new State("Active4",
                        new Taunt("THE POWER...IT CONSUMES ME!"),
                        new Order(2, "shtrs MagiGenerators", "vulnerable"),
                        new Shoot(15, 20, index: 2, cooldown: 100000000, cooldownOffset: 100),
                        new Shoot(15, 20, index: 3, cooldown: 100000000, cooldownOffset: 200),
                        new Shoot(15, 20, index: 4, cooldown: 100000000, cooldownOffset: 300),
                        new Shoot(15, 20, index: 2, cooldown: 100000000, cooldownOffset: 400),
                        new Shoot(15, 20, index: 5, cooldown: 100000000, cooldownOffset: 500),
                        new Shoot(15, 20, index: 6, cooldown: 100000000, cooldownOffset: 600),
                        new TimedTransition("NewShit5", 2000)
                        ),
                    new State("NewShit5",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new MoveTo2(-4, 0, 1),
                        new TimedTransition("Active5", 3000)
                            ),
                    new State("Active5",
                        new Taunt("THE POWER...IT CONSUMES ME!"),
                        new Order(2, "shtrs MagiGenerators", "vulnerable"),
                        new Shoot(15, 20, index: 2, cooldown: 100000000, cooldownOffset: 100),
                        new Shoot(15, 20, index: 3, cooldown: 100000000, cooldownOffset: 200),
                        new Shoot(15, 20, index: 4, cooldown: 100000000, cooldownOffset: 300),
                        new Shoot(15, 20, index: 2, cooldown: 100000000, cooldownOffset: 400),
                        new Shoot(15, 20, index: 5, cooldown: 100000000, cooldownOffset: 500),
                        new Shoot(15, 20, index: 6, cooldown: 100000000, cooldownOffset: 600),
                        new TimedTransition("NewShit6", 2000)
                        ),
                    new State("NewShit6",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new MoveTo2(-4, 0, 1),
                        new TimedTransition("Active6", 3000)
                            ),
                    new State("Active6",
                        new Taunt("THE POWER...IT CONSUMES ME!"),
                        new Order(2, "shtrs MagiGenerators", "vulnerable"),
                        new Shoot(15, 20, index: 2, cooldown: 100000000, cooldownOffset: 100),
                        new Shoot(15, 20, index: 3, cooldown: 100000000, cooldownOffset: 200),
                        new Shoot(15, 20, index: 4, cooldown: 100000000, cooldownOffset: 300),
                        new Shoot(15, 20, index: 2, cooldown: 100000000, cooldownOffset: 400),
                        new Shoot(15, 20, index: 5, cooldown: 100000000, cooldownOffset: 500),
                        new Shoot(15, 20, index: 6, cooldown: 100000000, cooldownOffset: 600),
                        new TimedTransition("NewShit7", 2000)
                        ),
                    new State("NewShit7",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new MoveTo2(0, -13, 1),
                        new TimedTransition("Active7", 3000)
                            ),
                    new State("Active7",
                        new Taunt("THE POWER...IT CONSUMES ME!"),
                        new Order(2, "shtrs MagiGenerators", "vulnerable"),
                        new Shoot(15, 20, index: 2, cooldown: 100000000, cooldownOffset: 100),
                        new Shoot(15, 20, index: 3, cooldown: 100000000, cooldownOffset: 200),
                        new Shoot(15, 20, index: 4, cooldown: 100000000, cooldownOffset: 300),
                        new Shoot(15, 20, index: 2, cooldown: 100000000, cooldownOffset: 400),
                        new Shoot(15, 20, index: 5, cooldown: 100000000, cooldownOffset: 500),
                        new Shoot(15, 20, index: 6, cooldown: 100000000, cooldownOffset: 600)
                        ),
                        new State("Death",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Taunt("IM..POSSI...BLE!"),
                            new CopyDamageOnDeath("shtrs Loot Balloon Mage"),
                            new Order(1, "shtrs Chest Spawner 2", "Open"),
                            new TimedTransition("Suicide", 2000)
                            ),
                        new State("Suicide",
                            new Shoot(35, index: 0, count: 30),
                            new Suicide()
                    )
                )
            )
        #endregion 2ndboss
        #region birds
            db.Init("shtrs Inferno",
                new State("base",
                    new Orbit(0.5f, 4, 15, "shtrs Blizzard"),
                            new Shoot(0, index: 0, count: 6, shootAngle: 60, fixedAngle: 15, cooldown: 4333),
                            new Shoot(0, index: 0, count: 6, shootAngle: 60, fixedAngle: 30, cooldown: 3500),
                            new Shoot(0, index: 0, count: 6, shootAngle: 60, fixedAngle: 60, cooldown: 7250),
                            new Shoot(0, index: 0, count: 6, shootAngle: 60, fixedAngle: 90, cooldown: 10000)
                )
            )

            db.Init("shtrs Blizzard",
                new State("base",
                    new State("Follow",
                    new Follow(0.3f, range: 1, cooldown: 1000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 25),
                            new TimedTransition("Spin", 7000)
                    ),
                    new State("Spin",
                        new State("Quadforce1",
                            new Shoot(0, index: 0, count: 6, shootAngle: 60, fixedAngle: 0, cooldown: 300),
                            new TimedTransition("Quadforce2", 10)
                        ),
                        new State("Quadforce2",
                            new Shoot(0, index: 0, count: 6, shootAngle: 60, fixedAngle: 15, cooldown: 300),
                            new TimedTransition("Quadforce3", 10)
                        ),
                        new State("Quadforce3",
                            new Shoot(0, index: 0, count: 6, shootAngle: 60, fixedAngle: 30, cooldown: 300),
                            new TimedTransition("Quadforce4", 10)
                        ),
                        new State("Quadforce4",
                            new Shoot(0, index: 0, count: 6, shootAngle: 60, fixedAngle: 45, cooldown: 300),
                            new TimedTransition("Quadforce5", 10)
                        ),
                        new State("Quadforce5",
                            new Shoot(0, index: 0, count: 6, shootAngle: 60, fixedAngle: 60, cooldown: 300),
                            new TimedTransition("Quadforce6", 10)
                        ),
                        new State("Quadforce6",
                            new Shoot(0, index: 0, count: 6, shootAngle: 60, fixedAngle: 75, cooldown: 300),
                            new TimedTransition("Quadforce7", 10)
                        ),
                        new State("Quadforce7",
                            new Shoot(0, index: 0, count: 6, shootAngle: 60, fixedAngle: 90, cooldown: 300),
                            new TimedTransition("Quadforce8", 10)
                        ),
                        new State("Quadforce8",
                            new Shoot(0, index: 0, count: 6, shootAngle: 60, fixedAngle: 105, cooldown: 300),
                            new TimedTransition("Quadforce9", 10)
                        ),
                        new State("Quadforce9",
                            new Shoot(0, index: 0, count: 6, shootAngle: 60, fixedAngle: 120, cooldown: 300),
                            new TimedTransition("Quadforce10", 10)
                        ),
                        new State("Quadforce10",
                            new Shoot(0, index: 0, count: 6, shootAngle: 60, fixedAngle: 135, cooldown: 300),
                            new TimedTransition("Quadforce11", 10)
                        ),
                        new State("Quadforce11",
                            new Shoot(0, index: 0, count: 6, shootAngle: 60, fixedAngle: 150, cooldown: 300),
                            new TimedTransition("Quadforce12", 10)
                        ),
                        new State("Quadforce12",
                            new Shoot(0, index: 0, count: 6, shootAngle: 60, fixedAngle: 165, cooldown: 300),
                            new TimedTransition("Quadforce13", 10)
                        ),
                        new State("Quadforce13",
                            new Shoot(0, index: 0, count: 6, shootAngle: 60, fixedAngle: 180, cooldown: 300),
                            new TimedTransition("Quadforce14", 10)
                        ),
                        new State("Quadforce14",
                            new Shoot(0, index: 0, count: 6, shootAngle: 60, fixedAngle: 195, cooldown: 300),
                            new TimedTransition("Quadforce15", 10)
                        ),
                        new State("Quadforce15",
                            new Shoot(0, index: 0, count: 6, shootAngle: 60, fixedAngle: 210, cooldown: 300),
                            new TimedTransition("Quadforce16", 10)
                        ),
                        new State("Quadforce16",
                            new Shoot(0, index: 0, count: 6, shootAngle: 60, fixedAngle: 225, cooldown: 300),
                            new TimedTransition("Follow", 10)

                            ))
                )
            )
        #endregion birds
        #region 1stbosschest
            db.Init("shtrs Loot Balloon Bridge",
                new State("base",
                    new State("Idle",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Order(46, "shtrs Spawn Bridge", "Open"),
                        new TimedTransition("Bridge", 5000)
                    ),
                    new State("Bridge")

                ),
                new Threshold(0.1f,
                    new TierLoot(11, LootType.Weapon, 1),
                    new TierLoot(12, LootType.Weapon, 1),
                    new TierLoot(6, LootType.Ability, 1),
                    new TierLoot(12, LootType.Armor, 1),
                    new TierLoot(13, LootType.Armor, 1),
                    new TierLoot(6, LootType.Ring, 1),
                    new ItemLoot("Potion of Attack", 1),
                    new ItemLoot("Potion of Defense", 1),
                    new ItemLoot("Bracer of the Guardian", 0.01f)
                )
            )
        #endregion 1stbosschest
        #region 2ndbosschest
            db.Init("shtrs Loot Balloon Mage",
                new State("base",
                    new State("Idle",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition("Mage", 5000)
                    ),
                    new State("Mage")
                ),
                new Threshold(0.1f,
                    new TierLoot(11, LootType.Weapon, 1),
                    new TierLoot(12, LootType.Weapon, 1),
                    new TierLoot(6, LootType.Ability, 1),
                    new TierLoot(12, LootType.Armor, 1),
                    new TierLoot(13, LootType.Armor, 1),
                    new TierLoot(6, LootType.Ring, 1),
                    new ItemLoot("Potion of Mana", 1),
                    new ItemLoot("The Twilight Gemstone", 0.01f)
                )
            )
        #endregion 2ndbosschest
        #region BridgeStatues
            db.Init("shtrs Bridge Obelisk A",
                new State("base",
                    new State("Idle",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntitiesNotExistsTransition(100, "TALK", "Shtrs Bridge Closer4")
                        ),
                    new State("TALK",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt("DO NOT WAKE THE BRIDGE GUARDIAN!"),
                        new TimedTransition("AFK", 2000)
                        ),
                    new State("AFK",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Flash(0x0000FF0C, 0.5f, 4),
                            new TimedTransition("activatetimer", 2500)
                        ),
                    new State("activatetimer",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Order(60, "shtrs obelisk timer", "timer1"),
                            new TimedTransition("stopsettingoffmytimer", 1)
                        ),
                    new State("stopsettingoffmytimer",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable)
                        ),
                    new State("Shoot",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 1000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 1200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 1400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 1600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 1800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 2000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 2200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 2400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 2600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 2800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 3000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 3200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 3400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 3600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 3800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 4000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 4200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 4400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 4600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 4800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 5000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 5200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 5400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 5600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 5800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 6000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 6200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 6400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 6600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 6800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 7000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 7200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 7400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 7600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 7800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 8000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 8200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 8400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 8600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 8800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 9000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 9200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 9400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 9600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 9800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 10000)
                        ),
                    new State("Pause",
                        new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Spawn("shtrs Stone Knight", maxChildren: 1, initialSpawn: 1, cooldown: 7500),
                        new Spawn("shtrs Stone Mage", maxChildren: 1, initialSpawn: 1, cooldown: 7500)
                        )
                    )
            )
            db.Init("shtrs Bridge Obelisk B",
                new State("base",
                    new State("Idle",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntitiesNotExistsTransition(100, "TALK", "Shtrs Bridge Closer4")
                        ),
                    new State("TALK",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt("DO NOT WAKE THE BRIDGE GUARDIAN!"),
                        new TimedTransition("AFK", 2000)
                        ),
                    new State("AFK",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Flash(0x0000FF0C, 0.5f, 4)
                        ),
                    new State("Shoot",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 1000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 1200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 1400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 1600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 1800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 2000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 2200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 2400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 2600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 2800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 3000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 3200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 3400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 3600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 3800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 4000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 4200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 4400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 4600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 4800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 5000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 5200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 5400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 5600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 5800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 6000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 6200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 6400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 6600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 6800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 7000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 7200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 7400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 7600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 7800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 8000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 8200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 8400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 8600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 8800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 9000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 9200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 9400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 9600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 9800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 10000)
                        ),
                    new State("guardiancheck",
                        new EntitiesNotExistsTransition(30, "Pause", "shtrs Bridge Obelisk A")
                        ),
                    new State("Pause",
                        new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Spawn("shtrs Stone Knight", maxChildren: 1, initialSpawn: 1),
                        new Spawn("shtrs Stone Mage", maxChildren: 1, initialSpawn: 1)
                        )
                    )
            )
            db.Init("shtrs Bridge Obelisk D",
                new State("base",
                    new State("Idle",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntitiesNotExistsTransition(100, "TALK", "Shtrs Bridge Closer4")
                        ),
                    new State("TALK",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt("DO NOT WAKE THE BRIDGE GUARDIAN!"),
                        new TimedTransition("AFK", 2000)
                        ),
                    new State("AFK",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Flash(0x0000FF0C, 0.5f, 4)
                        ),
                    new State("Shoot",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 1000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 1200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 1400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 1600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 1800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 2000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 2200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 2400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 2600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 2800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 3000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 3200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 3400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 3600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 3800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 4000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 4200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 4400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 4600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 4800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 5000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 5200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 5400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 5600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 5800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 6000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 6200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 6400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 6600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 6800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 7000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 7200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 7400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 7600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 7800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 8000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 8200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 8400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 8600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 8800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 9000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 9200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 9400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 9600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 9800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 10000)
                        ),
                    new State("guardiancheck",
                        new EntitiesNotExistsTransition(30, "Pause", "shtrs Bridge Obelisk B")
                        ),
                    new State("Pause",
                        new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Spawn("shtrs Stone Knight", maxChildren: 1, initialSpawn: 1),
                        new Spawn("shtrs Stone Mage", maxChildren: 1, initialSpawn: 1)
                    )
            )
            )
            db.Init("shtrs Bridge Obelisk E",
                new State("base",
                    new State("Idle",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntitiesNotExistsTransition(100, "TALK", "Shtrs Bridge Closer4")
                        ),
                    new State("TALK",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt("DO NOT WAKE THE BRIDGE GUARDIAN!"),
                        new TimedTransition("AFK", 2000)
                        ),
                    new State("AFK",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Flash(0x0000FF0C, 0.5f, 4)
                        ),
                    new State("Shoot",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 1000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 1200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 1400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 1600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 1800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 2000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 2200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 2400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 2600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 2800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 3000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 3200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 3400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 3600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 3800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 4000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 4200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 4400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 4600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 4800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 5000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 5200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 5400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 5600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 5800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 6000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 6200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 6400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 6600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 6800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 7000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 7200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 7400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 7600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 7800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 8000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 8200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 8400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 8600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 8800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 9000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 9200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 9400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 9600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 9800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 10000)
                        ),
                    new State("guardiancheck",
                        new EntitiesNotExistsTransition(30, "Pause", "shtrs Bridge Obelisk D")
                        ),
                    new State("Pause",
                        new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Spawn("shtrs Stone Knight", maxChildren: 1, initialSpawn: 1),
                        new Spawn("shtrs Stone Mage", maxChildren: 1, initialSpawn: 1)
                    )
            )
            )
            db.Init("shtrs Bridge Obelisk C",                                                     //YELLOW TOWERS!
                new State("base",
                    new State("Idle",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new EntitiesNotExistsTransition(100, "JustKillMe", "Shtrs Bridge Closer4")
                        ),
                    new State("JustKillMe",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition("AFK", 2000)
                        ),
                    new State("AFK",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Flash(0x0000FF0C, 0.5f, 4),
                            new TimedTransition("Shoot", 2500)
                        ),
                    new State("Shoot",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 1000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 1200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 1400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 1600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 1800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 2000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 2200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 2400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 2600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 2800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 3000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 3200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 3400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 3600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 3800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 4000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 4200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 4400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 4600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 4800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 5000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 5200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 5400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 5600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 5800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 6000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 6200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 6400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 6600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 6800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 7000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 7200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 7400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 7600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 7800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 8000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 8200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 8400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 8600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 8800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 9000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 9200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 9400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 9600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 9800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 10000),
                            new TimedTransition("Pause", 10000)
                        ),
                    new State("Pause",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Spawn("shtrs Stone Paladin", maxChildren: 1, initialSpawn: 1, cooldown: 7500),
                        new TimedTransition("Shoot", 7000)
                        )
                    )
            )
            db.Init("shtrs Bridge Obelisk F",                                                     //YELLOW TOWERS!
                new State("base",
                    new State("Idle",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new EntitiesNotExistsTransition(100, "JustKillMe", "Shtrs Bridge Closer4")
                        ),
                    new State("JustKillMe",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition("AFK", 2000)
                        ),
                    new State("AFK",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new ConditionalEffect(ConditionEffectIndex.Armored),
                            new Flash(0x0000FF0C, 0.5f, 4),
                            new TimedTransition("Shoot", 2500)
                        ),
                    new State("Shoot",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 1000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 1200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 1400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 1600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 1800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 2000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 2200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 2400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 2600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 2800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 3000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 3200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 3400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 3600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 3800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 4000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 4200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 4400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 4600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 4800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 5000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 5200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 5400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 5600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 5800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 6000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 6200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 6400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 6600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 6800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 7000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 7200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 7400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 7600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 7800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 8000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 8200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 8400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 8600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 8800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 9000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 9200),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 9400),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 9600),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 9800),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 10000, cooldownOffset: 10000),
                            new TimedTransition("Pause", 10000)
                        ),
                    new State("Pause",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Spawn("shtrs Stone Paladin", maxChildren: 1, initialSpawn: 1, cooldown: 7500),
                        new TimedTransition("Shoot", 7000)
                        )
                    )
            )
        #endregion BridgeStatues
        #region SomeMobs
            db.Init("shtrs obelisk controller",
            new State("base",
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("wait",
                    new EntitiesNotExistsTransition(30, "obeliskshoot", "shtrs Bridge Obelisk A", "shtrs Bridge Obelisk B", "shtrs Bridge Obelisk D", "shtrs Bridge Obelisk E")
                        ),
                    new State("obeliskshoot",
                        new Order(60, "shtrs Bridge Obelisk A", "Shoot"),
                        new Order(60, "shtrs Bridge Obelisk B", "Shoot"),
                        new Order(60, "shtrs Bridge Obelisk D", "Shoot"),
                        new Order(60, "shtrs Bridge Obelisk E", "Shoot")
                        ),
                    new State("guardiancheck",
                        new Order(60, "shtrs Bridge Obelisk A", "Pause"),
                        new Order(60, "shtrs Bridge Obelisk B", "guardiancheck"),
                        new Order(60, "shtrs Bridge Obelisk D", "guardiancheck"),
                        new Order(60, "shtrs Bridge Obelisk E", "guardiancheck"),
                        new TimedTransition("leavemychecksalone", 1)
                        ),
                    new State("leavemychecksalone",
                        new ConditionalEffect(ConditionEffectIndex.Invincible)
                        )
                )
            )
            db.Init("shtrs obelisk timer",
            new State("base",
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("wait",
                    new EntitiesNotExistsTransition(30, "timer1", "shtrs Bridge Obelisk A", "shtrs Bridge Obelisk B", "shtrs Bridge Obelisk D", "shtrs Bridge Obelisk E")
                        ),
                    new State("timer1",
                        new Order(60, "shtrs obelisk controller", "obeliskshoot"),
                        new TimedTransition("guardiancheck", 10000)
                        ),
                    new State("guardiancheck",
                        new Order(60, "shtrs obelisk controller", "guardiancheck"),
                        new TimedTransition("leavemychecksalone", 1)
                        ),
                    new State("leavemychecksalone",
                        new TimedTransition("timer1", 7000)
                        )
                )
            )
            db.Init("shtrs Titanum",
                new State("base",
                    new State("Wait",
                        new PlayerWithinTransition(7, "spawn")
                            ),
                    new State("spawn",
                        new Spawn("shtrs Stone Knight", maxChildren: 1, initialSpawn: 1, cooldown: 5000),
                        new Spawn("shtrs Stone Mage", maxChildren: 1, initialSpawn: 1, cooldown: 7500),
                        new TimedTransition("Wait", 5000)
                        )
                    )
            )
            db.Init("shtrs Paladin Obelisk",
                new State("base",
                    new State("Wait",
                        new PlayerWithinTransition(5, "spawn")
                            ),
                    new State("spawn",
                        new Spawn("shtrs Stone Paladin", maxChildren: 1, initialSpawn: 1, cooldown: 7500)
                        )
                    )
            )
            db.Init("shtrs Ice Mage",
                new State("base",
                    new State("Wait",
                        new PlayerWithinTransition(5, "fire")
                            ),
                    new State("fire",
                        new Follow(0.5f, range: 1),
                        new Shoot(10, 5, 10, index: 0, cooldown: 1500),
                        new TimedTransition("Spawn", 15000)
                        ),
                    new State("Spawn",
                        new Spawn("shtrs Ice Shield", maxChildren: 1, initialSpawn: 1, cooldown: 750000000),
                        new TimedTransition("fire", 25)
                        )
                    )
            )
            db.Init("shtrs Archmage of Flame",
            new State("base",
                new State("wait",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new PlayerWithinTransition(7, "Follow")
                    ),
                new State("Follow",
                    new Follow(1, range: 1),
                    new TimedTransition("Throw", 5000)
                    ),
                new State("Throw",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new TossObject("shtrs Firebomb", 1, angle: 90, cooldown: 5000),
                    new TossObject("shtrs Firebomb", 2, angle: 20, cooldown: 5000),
                    new TossObject("shtrs Firebomb", 3, angle: 72, cooldown: 5000),
                    new TossObject("shtrs Firebomb", 4, angle: 270, cooldown: 5000),
                    new TossObject("shtrs Firebomb", 5, angle: 140, cooldown: 5000),
                    new TossObject("shtrs Firebomb", 6, angle: 220, cooldown: 5000),
                    new TossObject("shtrs Firebomb", 6, angle: 48, cooldown: 5000),
                    new TossObject("shtrs Firebomb", 5, angle: 56, cooldown: 5000),
                    new TossObject("shtrs Firebomb", 4, angle: 180, cooldown: 5000),
                    new TossObject("shtrs Firebomb", 3, angle: 30, cooldown: 5000),
                    new TossObject("shtrs Firebomb", 2, angle: 0, cooldown: 5000),
                    new TossObject("shtrs Firebomb", 1, angle: 190, cooldown: 5000),
                    new TimedTransition("Fire", 4000)
                    ),
                new State("Fire",
                    new Shoot(0, index: 0, count: 1, shootAngle: 45, fixedAngle: 45, cooldown: 1, cooldownOffset: 0),
                    new Shoot(0, index: 0, count: 1, shootAngle: 90, fixedAngle: 90, cooldown: 1, cooldownOffset: 0),
                    new Shoot(0, index: 0, count: 1, shootAngle: 135, fixedAngle: 135, cooldown: 1, cooldownOffset: 0),
                    new Shoot(0, index: 0, count: 1, shootAngle: 180, fixedAngle: 180, cooldown: 1, cooldownOffset: 0),
                    new Shoot(0, index: 0, count: 1, shootAngle: 225, fixedAngle: 225, cooldown: 1, cooldownOffset: 0),
                    new Shoot(0, index: 0, count: 1, shootAngle: 270, fixedAngle: 270, cooldown: 1, cooldownOffset: 0),
                    new Shoot(0, index: 0, count: 1, shootAngle: 315, fixedAngle: 315, cooldown: 1, cooldownOffset: 0),
                    new Shoot(0, index: 0, count: 1, shootAngle: 360, fixedAngle: 360, cooldown: 1, cooldownOffset: 0),
                    new TimedTransition("wait", 5000)
                    )
                )
            )

            db.Init("shtrs Firebomb",
                new State("base",
                    new State("Idle",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new TimedTransition("Explode", 2000)
                        ),
                    new State("Explode",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Shoot(100, index: 0, count: 8),
                        new Suicide()
                        )
                    )
            )

            db.Init("shtrs Fire Mage",
                new State("base",
                    new State("Wait",
                        new PlayerWithinTransition(5, "fire")
                            ),
                    new State("fire",
                        new Follow(0.5f, range: 1),
                        new Shoot(10, 5, 10, index: 0, cooldown: 1500),
                        new TimedTransition("nothing", 10000)
                            ),
                    new State("nothing",
                        new TimedTransition("fire", 1000)
                        )
                    )
            )
            db.Init("shtrs Stone Mage",
                new State("base",
                    new State("Wait",
                        new PlayerWithinTransition(5, "fire")
                            ),
                    new State("fire",
                        new Follow(0.5f, range: 1),
                        new Shoot(10, 2, 10, index: 1, cooldown: 200),
                        new TimedTransition("invulnerable", 10000)
                            ),
                    new State("invulnerable",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Shoot(10, 2, 10, index: 0, cooldown: 200),
                        new TimedTransition("fire", 3000)
                        )
                    )
            )
        #endregion SomeMobs
        #region WOODENGATESSWITCHESBRIDGES
            db.Init("shtrs Wooden Gate 3",
                new State("base",
                    new State("Despawn",
                        new Decay(0)
                        )
                    )
            )
            //db.Init("OBJECTHERE",
            //new State("base",
            //      new EntityNotExistTransition("shtrs Abandoned Switch 1", 10, "OPENGATE")
            //        ),
            //      new State("OPENGATE",
            //            new OpenGate("shtrs Wooden Gate", 10)
            //              )
            //        )
            //      )
            db.Init("shtrs Wooden Gate",
                new State("base",
                    new State("Idle",
                        new EntitiesNotExistsTransition(10, "Despawn", "shtrs Abandoned Switch 1")
                        ),
                    new State("Despawn",
                        new Decay(0)
                        )
                    )
            )
            db.Init("shtrs Abandoned Switch 1",
                new State("base",
                    new RemoveObjectOnDeath("shtrs Wooden Gate", 8)
                    )
            )
            db.Init("Tooky Shatters Master",
                new State("base",
                    new SetNoXP(),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new RemoveObjectOnDeath("shtrs Wooden Gate 2", 14)
                    )
            )
            db.Init("shtrs Wooden Gate 2",
                new State("base",
                    new State("Idle",
                        new EntitiesNotExistsTransition(60, "Despawn", "shtrs Abandoned Switch 2")
                        ),
                    new State("Despawn",
                        new Decay(0)
                        )
                    )
            )
        db.Init("shtrs Bridge Closer",
            new State("base",
                new State("Idle",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                    ),
                new State("Closer",
                    new ChangeGroundOnDeath(new[] { "shtrs Bridge" }, new[] { "shtrs Pure Evil" },
                        1),
                    new Suicide()
                    ),
                new State("TwilightClose",
                    new ChangeGroundOnDeath(new[] { "shtrs Shattered Floor", "shtrs Disaster Floor" }, new[] { "shtrs Pure Evil" },
                        1),
                    new Suicide()

                    )
                )
            )
        db.Init("shtrs Bridge Closer2",
            new State("base",
                new State("Idle",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                    ),
                new State("Closer",
                    new ChangeGroundOnDeath(new[] { "shtrs Bridge" }, new[] { "shtrs Pure Evil" },
                        1),
                    new Suicide()
                    ),
                new State("TwilightClose",
                    new ChangeGroundOnDeath(new[] { "shtrs Shattered Floor", "shtrs Disaster Floor" }, new[] { "shtrs Pure Evil" },
                        1),
                    new Suicide()

                    )
                )
            )
        db.Init("shtrs Bridge Closer3",
            new State("base",
                new State("Idle",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                    ),
                new State("Closer",
                    new ChangeGroundOnDeath(new[] { "shtrs Bridge" }, new[] { "shtrs Pure Evil" },
                        1),
                    new Suicide()
                    ),
                new State("TwilightClose",
                    new ChangeGroundOnDeath(new[] { "shtrs Shattered Floor", "shtrs Disaster Floor" }, new[] { "shtrs Pure Evil" },
                        1),
                    new Suicide()

                    )
                )
            )
        db.Init("shtrs Bridge Closer4",
            new State("base",
                new State("Idle",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                    ),
                new State("Closer",
                    new ChangeGroundOnDeath(new[] { "shtrs Bridge" }, new[] { "shtrs Pure Evil" },
                        1),
                    new Suicide()
                    ),
                new State("TwilightClose",
                    new ChangeGroundOnDeath(new[] { "shtrs Shattered Floor", "shtrs Disaster Floor" }, new[] { "shtrs Pure Evil" },
                        1),
                    new Suicide()

                    )
                )
            )
        db.Init("shtrs Spawn Bridge",
            new State("base",
                new State("Idle",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                    ),
                new State("Open",
                    new ChangeGroundOnDeath(new[] { "shtrs Pure Evil" }, new[] { "shtrs Bridge" },
                        1),
                    new Suicide()
                    )
                )
            )
        db.Init("shtrs Spawn Bridge 2",
            new State("base",
                new State("Idle",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new EntitiesNotExistsTransition(500, "Open", "shtrs Abandoned Switch 3")
                    ),
                new State("Open",
                    new ChangeGroundOnDeath(new[] { "shtrs Pure Evil" }, new[] { "shtrs Shattered Floor" },
                        1),
                    new Suicide()
                    ),
                new State("CloseBridge2",
                    new ChangeGroundOnDeath(new[] { "shtrs Shattered Floor" }, new[] { "shtrs Pure Evil" },
                        1),
                    new Suicide()
                    )
                )
            )
        db.Init("shtrs Spawn Bridge 3",
            new State("base",
                new State("Idle",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new EntitiesNotExistsTransition(500, "Open", "shtrs Twilight Archmage")
                    ),
                new State("Open",
                    new ChangeGroundOnDeath(new[] { "shtrs Pure Evil" }, new[] { "shtrs Shattered Floor" },
                        1),
                    new Suicide()
                    )
                )
            )
        db.Init("shtrs Spawn Bridge 5",
            new State("base",
                new State("Idle",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new EntitiesNotExistsTransition(100, "Open", "shtrs Royal Guardian L")
                    ),
                new State("Open",
                    new ChangeGroundOnDeath(new[] { "Dark Cobblestone" }, new[] { "Hot Lava" },
                        1),
                    new Suicide()
                    )
                )
            )
        #endregion WOODENGATESSWITCHESBRIDGES
        #region 3rdboss
            db.Init("shtrs The Forgotten King",
                new State("base",
                    new HealthTransition(0.1f, "Death"),
                    new State("Idle",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new ConditionalEffect(ConditionEffectIndex.Invisible),
                        new ConditionalEffect(ConditionEffectIndex.Stasis),
                        new TimedTransition("1st", 2000)
                    ),
                    new State("1st",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new ConditionalEffect(ConditionEffectIndex.Invisible),
                        new ConditionalEffect(ConditionEffectIndex.Stasis),
                        new Taunt("You have made a grave mistake coming here I will destroy you, and reclaim my place in the Realm."),
                        new TimedTransition("crystals", 2500)
                        ),
                    new State("crystals",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new ConditionalEffect(ConditionEffectIndex.Invisible),
                        new ConditionalEffect(ConditionEffectIndex.Stasis),
                        new Spawn("shtrs Crystal Tracker", maxChildren: 1, initialSpawn: 1, cooldown: 999999),
                        new Spawn("shtrs Green Crystal", maxChildren: 1, initialSpawn: 1, cooldown: 999999),
                        new Spawn("shtrs Yellow Crystal", maxChildren: 1, initialSpawn: 1, cooldown: 999999),
                        new Spawn("shtrs Red Crystal", maxChildren: 1, initialSpawn: 1, cooldown: 999999),
                        new Spawn("shtrs Blue Crystal", maxChildren: 1, initialSpawn: 1, cooldown: 999999),
                        new EntitiesNotExistsTransition(40, "fireandice", "shtrs Green Crystal", "shtrs Yellow Crystal", "shtrs Red Crystal", "shtrs Blue Crystal")
                        ),
                    new State("fireandice",
                        new Shoot(40, 2, 45, index: 2, cooldown: 500, cooldownOffset: 200),
                        new Shoot(40, 2, 45, index: 3, cooldown: 500),
                        new Shoot(40, 1, fixedAngle: 180, index: 1, cooldown: 9999),
                        new Shoot(40, 1, fixedAngle: 0, index: 1, cooldown: 9999),
                        new Shoot(40, 1, fixedAngle: 169, index: 1, cooldown: 9999, cooldownOffset: 400),
                        new Shoot(40, 1, fixedAngle: 11, index: 1, cooldown: 9999, cooldownOffset: 400),
                        new Shoot(40, 1, fixedAngle: 158, index: 1, cooldown: 9999, cooldownOffset: 400),
                        new Shoot(40, 1, fixedAngle: 22, index: 1, cooldown: 9999, cooldownOffset: 400),
                        new Shoot(40, 1, fixedAngle: 147, index: 1, cooldown: 9999, cooldownOffset: 700),
                        new Shoot(40, 1, fixedAngle: 33, index: 1, cooldown: 9999, cooldownOffset: 700),
                        new Shoot(40, 1, fixedAngle: 135, index: 1, cooldown: 9999, cooldownOffset: 700),
                        new Shoot(40, 1, fixedAngle: 45, index: 1, cooldown: 9999, cooldownOffset: 700),
                        new Shoot(40, 1, fixedAngle: 124, index: 1, cooldown: 9999, cooldownOffset: 700),
                        new Shoot(40, 1, fixedAngle: 56, index: 1, cooldown: 9999, cooldownOffset: 700),
                        new Shoot(40, 1, fixedAngle: 117, index: 1, cooldown: 9999, cooldownOffset: 1000),
                        new Shoot(40, 1, fixedAngle: 63, index: 1, cooldown: 9999, cooldownOffset: 1000),
                        new Shoot(40, 1, fixedAngle: 103, index: 1, cooldown: 9999, cooldownOffset: 1150),
                        new Shoot(40, 1, fixedAngle: 77, index: 1, cooldown: 9999, cooldownOffset: 1150),
                        new Shoot(40, 1, fixedAngle: 90, index: 1, cooldown: 9999, cooldownOffset: 1250),
                        new Shoot(40, 1, fixedAngle: 90, index: 1, cooldown: 9999, cooldownOffset: 1250),
                        new Shoot(40, 1, fixedAngle: 100, index: 1, cooldown: 9999, cooldownOffset: 1310),
                        new Shoot(40, 1, fixedAngle: 80, index: 1, cooldown: 9999, cooldownOffset: 1310),
                        new Shoot(40, 1, fixedAngle: 124, index: 1, cooldown: 9999, cooldownOffset: 1400),
                        new Shoot(40, 1, fixedAngle: 56, index: 1, cooldown: 9999, cooldownOffset: 1400),
                        new Shoot(40, 1, fixedAngle: 135, index: 1, cooldown: 9999, cooldownOffset: 1550),
                        new Shoot(40, 1, fixedAngle: 45, index: 1, cooldown: 9999, cooldownOffset: 1550),
                        new Shoot(40, 1, fixedAngle: 147, index: 1, cooldown: 9999, cooldownOffset: 1650),
                        new Shoot(40, 1, fixedAngle: 33, index: 1, cooldown: 9999, cooldownOffset: 1650),
                        new Shoot(40, 1, fixedAngle: 180, index: 1, cooldown: 9999, cooldownOffset: 1750),
                        new Shoot(40, 1, fixedAngle: 0, index: 1, cooldown: 9999, cooldownOffset: 1750),
                        new Shoot(40, 1, fixedAngle: 169, index: 1, cooldown: 9999, cooldownOffset: 1850),
                        new Shoot(40, 1, fixedAngle: 11, index: 1, cooldown: 9999, cooldownOffset: 1850),
                        new Shoot(40, 1, fixedAngle: 169, index: 1, cooldown: 9999, cooldownOffset: 1950),
                        new Shoot(40, 1, fixedAngle: 11, index: 1, cooldown: 9999, cooldownOffset: 1950),
                        new Shoot(40, 1, fixedAngle: 158, index: 1, cooldown: 9999, cooldownOffset: 2050),
                        new Shoot(40, 1, fixedAngle: 22, index: 1, cooldown: 9999, cooldownOffset: 2050),
                        new Shoot(40, 1, fixedAngle: 147, index: 1, cooldown: 9999, cooldownOffset: 2150),
                        new Shoot(40, 1, fixedAngle: 33, index: 1, cooldown: 9999, cooldownOffset: 2150),
                        new Shoot(40, 1, fixedAngle: 135, index: 1, cooldown: 9999, cooldownOffset: 2250),
                        new Shoot(40, 1, fixedAngle: 45, index: 1, cooldown: 9999, cooldownOffset: 2250),
                        new Shoot(40, 1, fixedAngle: 124, index: 1, cooldown: 9999, cooldownOffset: 2350),
                        new Shoot(40, 1, fixedAngle: 56, index: 1, cooldown: 9999, cooldownOffset: 2350),
                        new Shoot(40, 1, fixedAngle: 117, index: 1, cooldown: 9999, cooldownOffset: 2450),
                        new Shoot(40, 1, fixedAngle: 63, index: 1, cooldown: 9999, cooldownOffset: 2450),
                        new Shoot(40, 1, fixedAngle: 90, index: 1, cooldown: 9999, cooldownOffset: 2550),
                        new Shoot(40, 1, fixedAngle: 90, index: 1, cooldown: 9999, cooldownOffset: 2550),
                        new Shoot(40, 1, fixedAngle: 100, index: 1, cooldown: 9999, cooldownOffset: 2610),
                        new Shoot(40, 1, fixedAngle: 80, index: 1, cooldown: 9999, cooldownOffset: 2610),
                        new Shoot(40, 1, fixedAngle: 124, index: 1, cooldown: 9999, cooldownOffset: 2680),
                        new Shoot(40, 1, fixedAngle: 56, index: 1, cooldown: 9999, cooldownOffset: 2680),
                        new Shoot(40, 1, fixedAngle: 135, index: 1, cooldown: 9999, cooldownOffset: 2830),
                        new Shoot(40, 1, fixedAngle: 45, index: 1, cooldown: 9999, cooldownOffset: 2830),
                        new Shoot(40, 1, fixedAngle: 147, index: 1, cooldown: 9999, cooldownOffset: 2980),
                        new Shoot(40, 1, fixedAngle: 33, index: 1, cooldown: 9999, cooldownOffset: 2980),
                        new Shoot(40, 1, fixedAngle: 180, index: 1, cooldown: 9999, cooldownOffset: 3030),
                        new Shoot(40, 1, fixedAngle: 0, index: 1, cooldown: 9999, cooldownOffset: 3030),
                        new Shoot(40, 1, fixedAngle: 169, index: 1, cooldown: 9999, cooldownOffset: 3180),
                        new Shoot(40, 1, fixedAngle: 11, index: 1, cooldown: 9999, cooldownOffset: 3180),
                        new Shoot(40, 1, fixedAngle: 169, index: 1, cooldown: 9999, cooldownOffset: 3230),
                        new Shoot(40, 1, fixedAngle: 11, index: 1, cooldown: 9999, cooldownOffset: 3230),
                        new Shoot(40, 1, fixedAngle: 158, index: 1, cooldown: 9999, cooldownOffset: 3380),
                        new Shoot(40, 1, fixedAngle: 22, index: 1, cooldown: 9999, cooldownOffset: 3380),
                        new Shoot(40, 1, fixedAngle: 147, index: 1, cooldown: 9999, cooldownOffset: 3530),
                        new Shoot(40, 1, fixedAngle: 33, index: 1, cooldown: 9999, cooldownOffset: 3530),
                        new Shoot(40, 1, fixedAngle: 135, index: 1, cooldown: 9999, cooldownOffset: 3680),
                        new Shoot(40, 1, fixedAngle: 45, index: 1, cooldown: 9999, cooldownOffset: 3680),
                        new Shoot(40, 1, fixedAngle: 124, index: 1, cooldown: 9999, cooldownOffset: 3830),
                        new Shoot(40, 1, fixedAngle: 56, index: 1, cooldown: 9999, cooldownOffset: 3830),
                        new Shoot(40, 1, fixedAngle: 117, index: 1, cooldown: 9999, cooldownOffset: 3980),
                        new Shoot(40, 1, fixedAngle: 63, index: 1, cooldown: 9999, cooldownOffset: 3980),
                        new TimedTransition("middle", 4000)
                        ),
                     new State("middle",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new ConditionalEffect(ConditionEffectIndex.Invisible),
                        new ConditionalEffect(ConditionEffectIndex.Stasis),
                            new MoveTo2(0, 8, 0.5f),
                            new TimedTransition("J Guardians", 3000)
                            ),
                        new State("J Guardians",
                            new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new ConditionalEffect(ConditionEffectIndex.Invisible),
                            new ConditionalEffect(ConditionEffectIndex.Stasis),
                            new Spawn("shtrs Royal Guardian J", 10),
                            new TimedTransition("waiting", 50)
                            ),
                        new State("waiting",
                            new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new ConditionalEffect(ConditionEffectIndex.Invisible),
                            new ConditionalEffect(ConditionEffectIndex.Stasis),
                            new EntitiesNotExistsTransition(10, "littlerage", "shtrs Royal Guardian J")
                            ),
                        new State("littlerage",
                            new Shoot(40, 2, 45, index: 2, cooldown: 500, cooldownOffset: 200),
                            new Shoot(40, 2, 45, index: 3, cooldown: 500),
                            new Shoot(40, 8, index: 1, cooldown: 1000),
                            new TimedTransition("tentacles", 4000)
                            ),
                        new State("tentacles",
                            new Shoot(40, 2, 45, index: 2, cooldown: 500, cooldownOffset: 200),
                            new Shoot(40, 2, 45, index: 3, cooldown: 500),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 3, cooldown: 10800),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 4, cooldown: 10800),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 6, cooldown: 10800, cooldownOffset: 200),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 7, cooldown: 10800, cooldownOffset: 200),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 9, cooldown: 10800, cooldownOffset: 400),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 10, cooldown: 10800, cooldownOffset: 400),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 12, cooldown: 10800, cooldownOffset: 600),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 13, cooldown: 10800, cooldownOffset: 600),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 15, cooldown: 10800, cooldownOffset: 800),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 16, cooldown: 10800, cooldownOffset: 800),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 18, cooldown: 10800, cooldownOffset: 1000),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 19, cooldown: 10800, cooldownOffset: 1000),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 21, cooldown: 10800, cooldownOffset: 1200),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 22, cooldown: 10800, cooldownOffset: 1200),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 24, cooldown: 10800, cooldownOffset: 1400),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 25, cooldown: 10800, cooldownOffset: 1400),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 27, cooldown: 10800, cooldownOffset: 1600),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 28, cooldown: 10800, cooldownOffset: 1600),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 30, cooldown: 10800, cooldownOffset: 1800),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 31, cooldown: 10800, cooldownOffset: 1800),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 33, cooldown: 10800, cooldownOffset: 2000),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 34, cooldown: 10800, cooldownOffset: 2000),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 36, cooldown: 10800, cooldownOffset: 2200),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 37, cooldown: 10800, cooldownOffset: 2200),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 39, cooldown: 10800, cooldownOffset: 2400),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 40, cooldown: 10800, cooldownOffset: 2400),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 42, cooldown: 10800, cooldownOffset: 2600),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 43, cooldown: 10800, cooldownOffset: 2600),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 45, cooldown: 10800, cooldownOffset: 2800),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 46, cooldown: 10800, cooldownOffset: 2800),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 48, cooldown: 10800, cooldownOffset: 3000),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 49, cooldown: 10800, cooldownOffset: 3000),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 51, cooldown: 10800, cooldownOffset: 3200),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 52, cooldown: 10800, cooldownOffset: 3200),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 54, cooldown: 10800, cooldownOffset: 3400),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 55, cooldown: 10800, cooldownOffset: 3400),
                            new HealthTransition(0.6f, "moveaftertentacles"),
                            new TimedTransition("tentacles2", 3400)
                            ),
                        new State("tentacles2",
                            new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new ConditionalEffect(ConditionEffectIndex.Invisible),
                            new ConditionalEffect(ConditionEffectIndex.Stasis),
                            new Order(60, "shtrs Lava Souls maker", "Spawn"),
                            new Order(60, "shtrs king lava1", "lava"),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 3, cooldown: 15000),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 4, cooldown: 15000),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 6, cooldown: 15000, cooldownOffset: 200),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 7, cooldown: 15000, cooldownOffset: 200),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 9, cooldown: 15000, cooldownOffset: 400),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 10, cooldown: 15000, cooldownOffset: 400),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 12, cooldown: 15000, cooldownOffset: 600),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 13, cooldown: 15000, cooldownOffset: 600),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 15, cooldown: 15000, cooldownOffset: 800),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 16, cooldown: 15000, cooldownOffset: 800),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 18, cooldown: 15000, cooldownOffset: 1000),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 19, cooldown: 15000, cooldownOffset: 1000),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 21, cooldown: 15000, cooldownOffset: 1200),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 22, cooldown: 15000, cooldownOffset: 1200),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 24, cooldown: 15000, cooldownOffset: 1400),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 25, cooldown: 15000, cooldownOffset: 1400),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 27, cooldown: 15000, cooldownOffset: 1600),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 28, cooldown: 15000, cooldownOffset: 1600),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 30, cooldown: 15000, cooldownOffset: 1800),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 31, cooldown: 15000, cooldownOffset: 1800),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 33, cooldown: 15000, cooldownOffset: 2000),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 34, cooldown: 15000, cooldownOffset: 2000),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 36, cooldown: 15000, cooldownOffset: 2200),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 37, cooldown: 15000, cooldownOffset: 2200),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 39, cooldown: 15000, cooldownOffset: 2400),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 40, cooldown: 15000, cooldownOffset: 2400),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 42, cooldown: 15000, cooldownOffset: 2600),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 43, cooldown: 15000, cooldownOffset: 2600),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 45, cooldown: 15000, cooldownOffset: 2800),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 46, cooldown: 15000, cooldownOffset: 2800),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 48, cooldown: 15000, cooldownOffset: 3000),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 49, cooldown: 15000, cooldownOffset: 3000),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 51, cooldown: 15000, cooldownOffset: 3200),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 52, cooldown: 15000, cooldownOffset: 3200),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 54, cooldown: 15000, cooldownOffset: 3400),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 55, cooldown: 15000, cooldownOffset: 3400),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 57, cooldown: 15000, cooldownOffset: 3600),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 58, cooldown: 15000, cooldownOffset: 3600),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 60, cooldown: 15000, cooldownOffset: 3800),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 61, cooldown: 15000, cooldownOffset: 3800),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 63, cooldown: 15000, cooldownOffset: 4000),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 64, cooldown: 15000, cooldownOffset: 4000),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 66, cooldown: 15000, cooldownOffset: 4200),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 67, cooldown: 15000, cooldownOffset: 4200),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 69, cooldown: 15000, cooldownOffset: 4400),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 70, cooldown: 15000, cooldownOffset: 4400),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 72, cooldown: 15000, cooldownOffset: 4600),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 73, cooldown: 15000, cooldownOffset: 4600),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 75, cooldown: 15000, cooldownOffset: 4800),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 76, cooldown: 15000, cooldownOffset: 4800),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 78, cooldown: 15000, cooldownOffset: 5000),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 79, cooldown: 15000, cooldownOffset: 5000),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 81, cooldown: 15000, cooldownOffset: 5200),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 82, cooldown: 15000, cooldownOffset: 5200),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 84, cooldown: 15000, cooldownOffset: 5400),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 85, cooldown: 15000, cooldownOffset: 5400),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 87, cooldown: 15000, cooldownOffset: 5600),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 88, cooldown: 15000, cooldownOffset: 5600),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 90, cooldown: 15000, cooldownOffset: 5800),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 91, cooldown: 15000, cooldownOffset: 5800),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 93, cooldown: 15000, cooldownOffset: 6000),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 94, cooldown: 15000, cooldownOffset: 6000),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 96, cooldown: 15000, cooldownOffset: 6200),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 97, cooldown: 15000, cooldownOffset: 6200),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 99, cooldown: 15000, cooldownOffset: 6400),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 100, cooldown: 15000, cooldownOffset: 6400),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 102, cooldown: 15000, cooldownOffset: 6600),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 103, cooldown: 15000, cooldownOffset: 6600),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 105, cooldown: 15000, cooldownOffset: 6800),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 106, cooldown: 15000, cooldownOffset: 6800),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 108, cooldown: 15000, cooldownOffset: 7000),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 109, cooldown: 15000, cooldownOffset: 7000),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 111, cooldown: 15000, cooldownOffset: 7200),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 112, cooldown: 15000, cooldownOffset: 7200),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 114, cooldown: 15000, cooldownOffset: 7400),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 115, cooldown: 15000, cooldownOffset: 7400),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 117, cooldown: 15000, cooldownOffset: 7400),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 118, cooldown: 15000, cooldownOffset: 7400),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 120, cooldown: 15000, cooldownOffset: 7600),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 121, cooldown: 15000, cooldownOffset: 7600),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 123, cooldown: 15000, cooldownOffset: 7800),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 124, cooldown: 15000, cooldownOffset: 7800),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 126, cooldown: 15000, cooldownOffset: 8000),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 127, cooldown: 15000, cooldownOffset: 8000),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 129, cooldown: 15000, cooldownOffset: 8200),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 130, cooldown: 15000, cooldownOffset: 8200),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 132, cooldown: 15000, cooldownOffset: 8400),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 133, cooldown: 15000, cooldownOffset: 8400),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 135, cooldown: 15000, cooldownOffset: 8600),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 136, cooldown: 15000, cooldownOffset: 8600),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 138, cooldown: 15000, cooldownOffset: 8800),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 139, cooldown: 15000, cooldownOffset: 8800),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 141, cooldown: 15000, cooldownOffset: 9000),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 142, cooldown: 15000, cooldownOffset: 9000),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 144, cooldown: 15000, cooldownOffset: 9200),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 145, cooldown: 15000, cooldownOffset: 9200),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 147, cooldown: 15000, cooldownOffset: 9400),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 148, cooldown: 15000, cooldownOffset: 9400),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 150, cooldown: 15000, cooldownOffset: 9600),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 151, cooldown: 15000, cooldownOffset: 9600),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 153, cooldown: 15000, cooldownOffset: 9800),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 154, cooldown: 15000, cooldownOffset: 10000),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 156, cooldown: 15000, cooldownOffset: 10000),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 157, cooldown: 15000, cooldownOffset: 10200),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 159, cooldown: 15000, cooldownOffset: 10200),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 160, cooldown: 15000, cooldownOffset: 10400),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 162, cooldown: 15000, cooldownOffset: 10400),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 163, cooldown: 15000, cooldownOffset: 10600),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 165, cooldown: 15000, cooldownOffset: 10600),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 166, cooldown: 15000, cooldownOffset: 10800),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 168, cooldown: 15000, cooldownOffset: 10800),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 169, cooldown: 15000, cooldownOffset: 11000),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 171, cooldown: 15000, cooldownOffset: 11200),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 172, cooldown: 15000, cooldownOffset: 11200),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 174, cooldown: 15000, cooldownOffset: 11400),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 175, cooldown: 15000, cooldownOffset: 11400),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 177, cooldown: 15000, cooldownOffset: 11600),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 178, cooldown: 15000, cooldownOffset: 11600),
                            new Shoot(50, index: 4, count: 6, shootAngle: 60, fixedAngle: 180, cooldown: 15000, cooldownOffset: 11800),

                            new TimedTransition("tentaclestimer", 11800)
                            ),
                        new State("tentaclestimer",
                            new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new ConditionalEffect(ConditionEffectIndex.Invisible),
                            new ConditionalEffect(ConditionEffectIndex.Stasis),
                            new TimedTransition("tentacles", 2500)
                            ),
                        new State("moveaftertentacles",
                            new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new ConditionalEffect(ConditionEffectIndex.Invisible),
                            new ConditionalEffect(ConditionEffectIndex.Stasis),
                            new MoveTo2(0, -8, 0.5f, once: true),
                            new Order(40, "shtrs Lava Souls maker", "Idle"),
                            new Order(60, "shtrs king lava1", "lava"),
                            new Order(60, "shtrs king lava2", "lava"),
                            new TimedTransition("aftertentacles", 3000)
                            ),
                        new State("aftertentacles",
                            new HealthTransition(0.4f, "godpatience"),
                            new TimedTransition("shootattop", 3000)
                            ),
                        new State("shootattop",
                            new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new ConditionalEffect(ConditionEffectIndex.Invisible),
                            new ConditionalEffect(ConditionEffectIndex.Stasis),
                            new Shoot(40, 2, 45, index: 2, cooldown: 500, cooldownOffset: 200),
                            new Shoot(40, 2, 45, index: 3, cooldown: 500),
                            new Shoot(40, 1, fixedAngle: 180, index: 1, cooldown: 9999),
                            new Shoot(40, 1, fixedAngle: 0, index: 1, cooldown: 9999),
                            new Shoot(40, 1, fixedAngle: 169, index: 1, cooldown: 9999, cooldownOffset: 400),
                            new Shoot(40, 1, fixedAngle: 11, index: 1, cooldown: 9999, cooldownOffset: 400),
                            new Shoot(40, 1, fixedAngle: 158, index: 1, cooldown: 9999, cooldownOffset: 400),
                            new Shoot(40, 1, fixedAngle: 22, index: 1, cooldown: 9999, cooldownOffset: 400),
                            new Shoot(40, 1, fixedAngle: 147, index: 1, cooldown: 9999, cooldownOffset: 700),
                            new Shoot(40, 1, fixedAngle: 33, index: 1, cooldown: 9999, cooldownOffset: 700),
                            new Shoot(40, 1, fixedAngle: 135, index: 1, cooldown: 9999, cooldownOffset: 700),
                            new Shoot(40, 1, fixedAngle: 45, index: 1, cooldown: 9999, cooldownOffset: 700),
                            new Shoot(40, 1, fixedAngle: 124, index: 1, cooldown: 9999, cooldownOffset: 700),
                            new Shoot(40, 1, fixedAngle: 56, index: 1, cooldown: 9999, cooldownOffset: 700),
                            new Shoot(40, 1, fixedAngle: 117, index: 1, cooldown: 9999, cooldownOffset: 1000),
                            new Shoot(40, 1, fixedAngle: 63, index: 1, cooldown: 9999, cooldownOffset: 1000),
                            new Shoot(40, 1, fixedAngle: 103, index: 1, cooldown: 9999, cooldownOffset: 1150),
                            new Shoot(40, 1, fixedAngle: 77, index: 1, cooldown: 9999, cooldownOffset: 1150),
                            new Shoot(40, 1, fixedAngle: 90, index: 1, cooldown: 9999, cooldownOffset: 1250),
                            new Shoot(40, 1, fixedAngle: 90, index: 1, cooldown: 9999, cooldownOffset: 1250),
                            new Shoot(40, 1, fixedAngle: 100, index: 1, cooldown: 9999, cooldownOffset: 1310),
                            new Shoot(40, 1, fixedAngle: 80, index: 1, cooldown: 9999, cooldownOffset: 1310),
                            new Shoot(40, 1, fixedAngle: 124, index: 1, cooldown: 9999, cooldownOffset: 1400),
                            new Shoot(40, 1, fixedAngle: 56, index: 1, cooldown: 9999, cooldownOffset: 1400),
                            new Shoot(40, 1, fixedAngle: 135, index: 1, cooldown: 9999, cooldownOffset: 1550),
                            new Shoot(40, 1, fixedAngle: 45, index: 1, cooldown: 9999, cooldownOffset: 1550),
                            new Shoot(40, 1, fixedAngle: 147, index: 1, cooldown: 9999, cooldownOffset: 1650),
                            new Shoot(40, 1, fixedAngle: 33, index: 1, cooldown: 9999, cooldownOffset: 1650),
                            new Shoot(40, 1, fixedAngle: 180, index: 1, cooldown: 9999, cooldownOffset: 1750),
                            new Shoot(40, 1, fixedAngle: 0, index: 1, cooldown: 9999, cooldownOffset: 1750),
                            new Shoot(40, 1, fixedAngle: 169, index: 1, cooldown: 9999, cooldownOffset: 1850),
                            new Shoot(40, 1, fixedAngle: 11, index: 1, cooldown: 9999, cooldownOffset: 1850),
                            new Shoot(40, 1, fixedAngle: 169, index: 1, cooldown: 9999, cooldownOffset: 1950),
                            new Shoot(40, 1, fixedAngle: 11, index: 1, cooldown: 9999, cooldownOffset: 1950),
                            new Shoot(40, 1, fixedAngle: 158, index: 1, cooldown: 9999, cooldownOffset: 2050),
                            new Shoot(40, 1, fixedAngle: 22, index: 1, cooldown: 9999, cooldownOffset: 2050),
                            new Shoot(40, 1, fixedAngle: 147, index: 1, cooldown: 9999, cooldownOffset: 2150),
                            new Shoot(40, 1, fixedAngle: 33, index: 1, cooldown: 9999, cooldownOffset: 2150),
                            new Shoot(40, 1, fixedAngle: 135, index: 1, cooldown: 9999, cooldownOffset: 2250),
                            new Shoot(40, 1, fixedAngle: 45, index: 1, cooldown: 9999, cooldownOffset: 2250),
                            new Shoot(40, 1, fixedAngle: 124, index: 1, cooldown: 9999, cooldownOffset: 2350),
                            new Shoot(40, 1, fixedAngle: 56, index: 1, cooldown: 9999, cooldownOffset: 2350),
                            new Shoot(40, 1, fixedAngle: 117, index: 1, cooldown: 9999, cooldownOffset: 2450),
                            new Shoot(40, 1, fixedAngle: 63, index: 1, cooldown: 9999, cooldownOffset: 2450),
                            new Shoot(40, 1, fixedAngle: 90, index: 1, cooldown: 9999, cooldownOffset: 2550),
                            new Shoot(40, 1, fixedAngle: 90, index: 1, cooldown: 9999, cooldownOffset: 2550),
                            new Shoot(40, 1, fixedAngle: 100, index: 1, cooldown: 9999, cooldownOffset: 2610),
                            new Shoot(40, 1, fixedAngle: 80, index: 1, cooldown: 9999, cooldownOffset: 2610),
                            new Shoot(40, 1, fixedAngle: 124, index: 1, cooldown: 9999, cooldownOffset: 2680),
                            new Shoot(40, 1, fixedAngle: 56, index: 1, cooldown: 9999, cooldownOffset: 2680),
                            new Shoot(40, 1, fixedAngle: 135, index: 1, cooldown: 9999, cooldownOffset: 2830),
                            new Shoot(40, 1, fixedAngle: 45, index: 1, cooldown: 9999, cooldownOffset: 2830),
                            new Shoot(40, 1, fixedAngle: 147, index: 1, cooldown: 9999, cooldownOffset: 2980),
                            new Shoot(40, 1, fixedAngle: 33, index: 1, cooldown: 9999, cooldownOffset: 2980),
                            new Shoot(40, 1, fixedAngle: 180, index: 1, cooldown: 9999, cooldownOffset: 3030),
                            new Shoot(40, 1, fixedAngle: 0, index: 1, cooldown: 9999, cooldownOffset: 3030),
                            new Shoot(40, 1, fixedAngle: 169, index: 1, cooldown: 9999, cooldownOffset: 3180),
                            new Shoot(40, 1, fixedAngle: 11, index: 1, cooldown: 9999, cooldownOffset: 3180),
                            new Shoot(40, 1, fixedAngle: 169, index: 1, cooldown: 9999, cooldownOffset: 3230),
                            new Shoot(40, 1, fixedAngle: 11, index: 1, cooldown: 9999, cooldownOffset: 3230),
                            new Shoot(40, 1, fixedAngle: 158, index: 1, cooldown: 9999, cooldownOffset: 3380),
                            new Shoot(40, 1, fixedAngle: 22, index: 1, cooldown: 9999, cooldownOffset: 3380),
                            new Shoot(40, 1, fixedAngle: 147, index: 1, cooldown: 9999, cooldownOffset: 3530),
                            new Shoot(40, 1, fixedAngle: 33, index: 1, cooldown: 9999, cooldownOffset: 3530),
                            new Shoot(40, 1, fixedAngle: 135, index: 1, cooldown: 9999, cooldownOffset: 3680),
                            new Shoot(40, 1, fixedAngle: 45, index: 1, cooldown: 9999, cooldownOffset: 3680),
                            new Shoot(40, 1, fixedAngle: 124, index: 1, cooldown: 9999, cooldownOffset: 3830),
                            new Shoot(40, 1, fixedAngle: 56, index: 1, cooldown: 9999, cooldownOffset: 3830),
                            new Shoot(40, 1, fixedAngle: 117, index: 1, cooldown: 9999, cooldownOffset: 3980),
                            new Shoot(40, 1, fixedAngle: 63, index: 1, cooldown: 9999, cooldownOffset: 3980),
                            new HealthTransition(0.4f, "godpatience"),
                            new TimedTransition("shootattop2", 4000)
                            ),
                        new State("shootattop2",
                            new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new ConditionalEffect(ConditionEffectIndex.Invisible),
                            new ConditionalEffect(ConditionEffectIndex.Stasis),
                            new Shoot(40, 2, 45, index: 2, cooldown: 500, cooldownOffset: 200),
                            new Shoot(40, 2, 45, index: 3, cooldown: 500),
                            new Shoot(40, 1, fixedAngle: 180, index: 1, cooldown: 9999),
                            new Shoot(40, 1, fixedAngle: 0, index: 1, cooldown: 9999),
                            new Shoot(40, 1, fixedAngle: 169, index: 1, cooldown: 9999, cooldownOffset: 400),
                            new Shoot(40, 1, fixedAngle: 11, index: 1, cooldown: 9999, cooldownOffset: 400),
                            new Shoot(40, 1, fixedAngle: 158, index: 1, cooldown: 9999, cooldownOffset: 400),
                            new Shoot(40, 1, fixedAngle: 22, index: 1, cooldown: 9999, cooldownOffset: 400),
                            new Shoot(40, 1, fixedAngle: 147, index: 1, cooldown: 9999, cooldownOffset: 700),
                            new Shoot(40, 1, fixedAngle: 33, index: 1, cooldown: 9999, cooldownOffset: 700),
                            new Shoot(40, 1, fixedAngle: 135, index: 1, cooldown: 9999, cooldownOffset: 700),
                            new Shoot(40, 1, fixedAngle: 45, index: 1, cooldown: 9999, cooldownOffset: 700),
                            new Shoot(40, 1, fixedAngle: 124, index: 1, cooldown: 9999, cooldownOffset: 700),
                            new Shoot(40, 1, fixedAngle: 56, index: 1, cooldown: 9999, cooldownOffset: 700),
                            new Shoot(40, 1, fixedAngle: 117, index: 1, cooldown: 9999, cooldownOffset: 1000),
                            new Shoot(40, 1, fixedAngle: 63, index: 1, cooldown: 9999, cooldownOffset: 1000),
                            new Shoot(40, 1, fixedAngle: 103, index: 1, cooldown: 9999, cooldownOffset: 1150),
                            new Shoot(40, 1, fixedAngle: 77, index: 1, cooldown: 9999, cooldownOffset: 1150),
                            new Shoot(40, 1, fixedAngle: 90, index: 1, cooldown: 9999, cooldownOffset: 1250),
                            new Shoot(40, 1, fixedAngle: 90, index: 1, cooldown: 9999, cooldownOffset: 1250),
                            new Shoot(40, 1, fixedAngle: 100, index: 1, cooldown: 9999, cooldownOffset: 1310),
                            new Shoot(40, 1, fixedAngle: 80, index: 1, cooldown: 9999, cooldownOffset: 1310),
                            new Shoot(40, 1, fixedAngle: 124, index: 1, cooldown: 9999, cooldownOffset: 1400),
                            new Shoot(40, 1, fixedAngle: 56, index: 1, cooldown: 9999, cooldownOffset: 1400),
                            new Shoot(40, 1, fixedAngle: 135, index: 1, cooldown: 9999, cooldownOffset: 1550),
                            new Shoot(40, 1, fixedAngle: 45, index: 1, cooldown: 9999, cooldownOffset: 1550),
                            new Shoot(40, 1, fixedAngle: 147, index: 1, cooldown: 9999, cooldownOffset: 1650),
                            new Shoot(40, 1, fixedAngle: 33, index: 1, cooldown: 9999, cooldownOffset: 1650),
                            new Shoot(40, 1, fixedAngle: 180, index: 1, cooldown: 9999, cooldownOffset: 1750),
                            new Shoot(40, 1, fixedAngle: 0, index: 1, cooldown: 9999, cooldownOffset: 1750),
                            new Shoot(40, 1, fixedAngle: 169, index: 1, cooldown: 9999, cooldownOffset: 1850),
                            new Shoot(40, 1, fixedAngle: 11, index: 1, cooldown: 9999, cooldownOffset: 1850),
                            new Shoot(40, 1, fixedAngle: 169, index: 1, cooldown: 9999, cooldownOffset: 1950),
                            new Shoot(40, 1, fixedAngle: 11, index: 1, cooldown: 9999, cooldownOffset: 1950),
                            new Shoot(40, 1, fixedAngle: 158, index: 1, cooldown: 9999, cooldownOffset: 2050),
                            new Shoot(40, 1, fixedAngle: 22, index: 1, cooldown: 9999, cooldownOffset: 2050),
                            new Shoot(40, 1, fixedAngle: 147, index: 1, cooldown: 9999, cooldownOffset: 2150),
                            new Shoot(40, 1, fixedAngle: 33, index: 1, cooldown: 9999, cooldownOffset: 2150),
                            new Shoot(40, 1, fixedAngle: 135, index: 1, cooldown: 9999, cooldownOffset: 2250),
                            new Shoot(40, 1, fixedAngle: 45, index: 1, cooldown: 9999, cooldownOffset: 2250),
                            new Shoot(40, 1, fixedAngle: 124, index: 1, cooldown: 9999, cooldownOffset: 2350),
                            new Shoot(40, 1, fixedAngle: 56, index: 1, cooldown: 9999, cooldownOffset: 2350),
                            new Shoot(40, 1, fixedAngle: 117, index: 1, cooldown: 9999, cooldownOffset: 2450),
                            new Shoot(40, 1, fixedAngle: 63, index: 1, cooldown: 9999, cooldownOffset: 2450),
                            new Shoot(40, 1, fixedAngle: 90, index: 1, cooldown: 9999, cooldownOffset: 2550),
                            new Shoot(40, 1, fixedAngle: 90, index: 1, cooldown: 9999, cooldownOffset: 2550),
                            new Shoot(40, 1, fixedAngle: 100, index: 1, cooldown: 9999, cooldownOffset: 2610),
                            new Shoot(40, 1, fixedAngle: 80, index: 1, cooldown: 9999, cooldownOffset: 2610),
                            new Shoot(40, 1, fixedAngle: 124, index: 1, cooldown: 9999, cooldownOffset: 2680),
                            new Shoot(40, 1, fixedAngle: 56, index: 1, cooldown: 9999, cooldownOffset: 2680),
                            new Shoot(40, 1, fixedAngle: 135, index: 1, cooldown: 9999, cooldownOffset: 2830),
                            new Shoot(40, 1, fixedAngle: 45, index: 1, cooldown: 9999, cooldownOffset: 2830),
                            new Shoot(40, 1, fixedAngle: 147, index: 1, cooldown: 9999, cooldownOffset: 2980),
                            new Shoot(40, 1, fixedAngle: 33, index: 1, cooldown: 9999, cooldownOffset: 2980),
                            new Shoot(40, 1, fixedAngle: 180, index: 1, cooldown: 9999, cooldownOffset: 3030),
                            new Shoot(40, 1, fixedAngle: 0, index: 1, cooldown: 9999, cooldownOffset: 3030),
                            new Shoot(40, 1, fixedAngle: 169, index: 1, cooldown: 9999, cooldownOffset: 3180),
                            new Shoot(40, 1, fixedAngle: 11, index: 1, cooldown: 9999, cooldownOffset: 3180),
                            new Shoot(40, 1, fixedAngle: 169, index: 1, cooldown: 9999, cooldownOffset: 3230),
                            new Shoot(40, 1, fixedAngle: 11, index: 1, cooldown: 9999, cooldownOffset: 3230),
                            new Shoot(40, 1, fixedAngle: 158, index: 1, cooldown: 9999, cooldownOffset: 3380),
                            new Shoot(40, 1, fixedAngle: 22, index: 1, cooldown: 9999, cooldownOffset: 3380),
                            new Shoot(40, 1, fixedAngle: 147, index: 1, cooldown: 9999, cooldownOffset: 3530),
                            new Shoot(40, 1, fixedAngle: 33, index: 1, cooldown: 9999, cooldownOffset: 3530),
                            new Shoot(40, 1, fixedAngle: 135, index: 1, cooldown: 9999, cooldownOffset: 3680),
                            new Shoot(40, 1, fixedAngle: 45, index: 1, cooldown: 9999, cooldownOffset: 3680),
                            new Shoot(40, 1, fixedAngle: 124, index: 1, cooldown: 9999, cooldownOffset: 3830),
                            new Shoot(40, 1, fixedAngle: 56, index: 1, cooldown: 9999, cooldownOffset: 3830),
                            new Shoot(40, 1, fixedAngle: 117, index: 1, cooldown: 9999, cooldownOffset: 3980),
                            new Shoot(40, 1, fixedAngle: 63, index: 1, cooldown: 9999, cooldownOffset: 3980),
                            new HealthTransition(0.4f, "godpatience"),
                            new TimedTransition("aftertentacles", 4000)
                            ),
                        new State("godpatience",
                            new Order(60, "shtrs king lava1", "lava"),
                            new Order(60, "shtrs king lava2", "lava"),
                            new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new ConditionalEffect(ConditionEffectIndex.Invisible),
                            new ConditionalEffect(ConditionEffectIndex.Stasis),
                            new Taunt("YOU TEST THE PATIENCE OF A GOD!"),
                            new Order(40, "shtrs Lava Souls Maker", "Spawn"),
                            new Spawn("shtrs king timer", maxChildren: 1, initialSpawn: 1, cooldown: 999999),
                            new TimedTransition("diedie", 2000)
                            ),
                        new State("diedie",
                            new Order(60, "shtrs king timer", "timer1"),
                            new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new ConditionalEffect(ConditionEffectIndex.Invisible),
                            new ConditionalEffect(ConditionEffectIndex.Stasis),
                            new NoPlayerWithinTransition(7, "diewait"),
                            new PlayerWithinTransition(7, "dieshoot")
                            ),
                        new State("dieshoot",
                            new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new ConditionalEffect(ConditionEffectIndex.Invisible),
                            new ConditionalEffect(ConditionEffectIndex.Stasis),
                            new NoPlayerWithinTransition(7, "diewait"),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 3, cooldown: 15000),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 4, cooldown: 15000),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 6, cooldown: 15000, cooldownOffset: 200),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 7, cooldown: 15000, cooldownOffset: 200),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 9, cooldown: 15000, cooldownOffset: 400),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 10, cooldown: 15000, cooldownOffset: 400),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 12, cooldown: 15000, cooldownOffset: 600),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 13, cooldown: 15000, cooldownOffset: 600),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 15, cooldown: 15000, cooldownOffset: 800),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 16, cooldown: 15000, cooldownOffset: 800),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 18, cooldown: 15000, cooldownOffset: 1000),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 19, cooldown: 15000, cooldownOffset: 1000),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 21, cooldown: 15000, cooldownOffset: 1200),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 22, cooldown: 15000, cooldownOffset: 1200),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 24, cooldown: 15000, cooldownOffset: 1400),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 25, cooldown: 15000, cooldownOffset: 1400),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 27, cooldown: 15000, cooldownOffset: 1600),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 28, cooldown: 15000, cooldownOffset: 1600),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 30, cooldown: 15000, cooldownOffset: 1800),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 31, cooldown: 15000, cooldownOffset: 1800),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 33, cooldown: 15000, cooldownOffset: 2000),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 34, cooldown: 15000, cooldownOffset: 2000),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 36, cooldown: 15000, cooldownOffset: 2200),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 37, cooldown: 15000, cooldownOffset: 2200),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 39, cooldown: 15000, cooldownOffset: 2400),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 40, cooldown: 15000, cooldownOffset: 2400),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 42, cooldown: 15000, cooldownOffset: 2600),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 43, cooldown: 15000, cooldownOffset: 2600),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 45, cooldown: 15000, cooldownOffset: 2800),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 46, cooldown: 15000, cooldownOffset: 2800),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 48, cooldown: 15000, cooldownOffset: 3000),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 49, cooldown: 15000, cooldownOffset: 3000),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 51, cooldown: 15000, cooldownOffset: 3200),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 52, cooldown: 15000, cooldownOffset: 3200),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 54, cooldown: 15000, cooldownOffset: 3400),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 55, cooldown: 15000, cooldownOffset: 3400),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 57, cooldown: 15000, cooldownOffset: 3600),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 58, cooldown: 15000, cooldownOffset: 3600),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 60, cooldown: 15000, cooldownOffset: 3800),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 61, cooldown: 15000, cooldownOffset: 3800),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 63, cooldown: 15000, cooldownOffset: 4000),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 64, cooldown: 15000, cooldownOffset: 4000),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 66, cooldown: 15000, cooldownOffset: 4200),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 67, cooldown: 15000, cooldownOffset: 4200),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 69, cooldown: 15000, cooldownOffset: 4400),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 70, cooldown: 15000, cooldownOffset: 4400),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 72, cooldown: 15000, cooldownOffset: 4600),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 73, cooldown: 15000, cooldownOffset: 4600),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 75, cooldown: 15000, cooldownOffset: 4800),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 76, cooldown: 15000, cooldownOffset: 4800),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 78, cooldown: 15000, cooldownOffset: 5000),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 79, cooldown: 15000, cooldownOffset: 5000),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 81, cooldown: 15000, cooldownOffset: 5200),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 82, cooldown: 15000, cooldownOffset: 5200),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 84, cooldown: 15000, cooldownOffset: 5400),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 85, cooldown: 15000, cooldownOffset: 5400),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 87, cooldown: 15000, cooldownOffset: 5600),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 88, cooldown: 15000, cooldownOffset: 5600),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 90, cooldown: 15000, cooldownOffset: 5800),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 91, cooldown: 15000, cooldownOffset: 5800),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 93, cooldown: 15000, cooldownOffset: 6000),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 94, cooldown: 15000, cooldownOffset: 6000),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 96, cooldown: 15000, cooldownOffset: 6200),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 97, cooldown: 15000, cooldownOffset: 6200),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 99, cooldown: 15000, cooldownOffset: 6400),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 100, cooldown: 15000, cooldownOffset: 6400),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 102, cooldown: 15000, cooldownOffset: 6600),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 103, cooldown: 15000, cooldownOffset: 6600),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 105, cooldown: 15000, cooldownOffset: 6800),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 106, cooldown: 15000, cooldownOffset: 6800),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 108, cooldown: 15000, cooldownOffset: 7000),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 109, cooldown: 15000, cooldownOffset: 7000),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 111, cooldown: 15000, cooldownOffset: 7200),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 112, cooldown: 15000, cooldownOffset: 7200),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 114, cooldown: 15000, cooldownOffset: 7400),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 115, cooldown: 15000, cooldownOffset: 7400),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 6, cooldown: 15000, cooldownOffset: 7600),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 7, cooldown: 15000, cooldownOffset: 7600),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 9, cooldown: 15000, cooldownOffset: 7800),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 10, cooldown: 15000, cooldownOffset: 7800),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 12, cooldown: 15000, cooldownOffset: 8000),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 13, cooldown: 15000, cooldownOffset: 8000),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 15, cooldown: 15000, cooldownOffset: 8200),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 16, cooldown: 15000, cooldownOffset: 8200),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 18, cooldown: 15000, cooldownOffset: 8400),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 19, cooldown: 15000, cooldownOffset: 8400),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 21, cooldown: 15000, cooldownOffset: 8600),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 22, cooldown: 15000, cooldownOffset: 8600),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 24, cooldown: 15000, cooldownOffset: 8800),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 25, cooldown: 15000, cooldownOffset: 8800),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 27, cooldown: 15000, cooldownOffset: 9000),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 28, cooldown: 15000, cooldownOffset: 9000),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 30, cooldown: 15000, cooldownOffset: 9200),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 31, cooldown: 15000, cooldownOffset: 9200),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 33, cooldown: 15000, cooldownOffset: 9400),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 34, cooldown: 15000, cooldownOffset: 9400),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 36, cooldown: 15000, cooldownOffset: 9600),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 37, cooldown: 15000, cooldownOffset: 9600),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 39, cooldown: 15000, cooldownOffset: 9800),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 40, cooldown: 15000, cooldownOffset: 10000),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 42, cooldown: 15000, cooldownOffset: 10000),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 43, cooldown: 15000, cooldownOffset: 10200),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 45, cooldown: 15000, cooldownOffset: 10200),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 46, cooldown: 15000, cooldownOffset: 10400),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 48, cooldown: 15000, cooldownOffset: 10400),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 49, cooldown: 15000, cooldownOffset: 10600),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 51, cooldown: 15000, cooldownOffset: 10600),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 52, cooldown: 15000, cooldownOffset: 10800),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 54, cooldown: 15000, cooldownOffset: 10800),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 55, cooldown: 15000, cooldownOffset: 11000),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 57, cooldown: 15000, cooldownOffset: 11200),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 58, cooldown: 15000, cooldownOffset: 11200),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 60, cooldown: 15000, cooldownOffset: 11400),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 61, cooldown: 15000, cooldownOffset: 11400),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 63, cooldown: 15000, cooldownOffset: 11600),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 64, cooldown: 15000, cooldownOffset: 11600),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 66, cooldown: 15000, cooldownOffset: 11800),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 67, cooldown: 15000, cooldownOffset: 11800),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 69, cooldown: 15000, cooldownOffset: 12000),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 70, cooldown: 15000, cooldownOffset: 12000),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 72, cooldown: 15000, cooldownOffset: 12200),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 73, cooldown: 15000, cooldownOffset: 12200),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 75, cooldown: 15000, cooldownOffset: 12400),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 76, cooldown: 15000, cooldownOffset: 12400),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 78, cooldown: 15000, cooldownOffset: 12600),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 79, cooldown: 15000, cooldownOffset: 12600),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 81, cooldown: 15000, cooldownOffset: 12800),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 82, cooldown: 15000, cooldownOffset: 12800),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 84, cooldown: 15000, cooldownOffset: 13000),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 85, cooldown: 15000, cooldownOffset: 13000),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 87, cooldown: 15000, cooldownOffset: 13200),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 88, cooldown: 15000, cooldownOffset: 13200),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 90, cooldown: 15000, cooldownOffset: 13400),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 91, cooldown: 15000, cooldownOffset: 13400),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 93, cooldown: 15000, cooldownOffset: 13600),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 94, cooldown: 15000, cooldownOffset: 13600),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 96, cooldown: 15000, cooldownOffset: 13800),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 97, cooldown: 15000, cooldownOffset: 13800),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 99, cooldown: 15000, cooldownOffset: 14000),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 100, cooldown: 15000, cooldownOffset: 14000),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 102, cooldown: 15000, cooldownOffset: 14200),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 103, cooldown: 15000, cooldownOffset: 14200),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 105, cooldown: 15000, cooldownOffset: 14400),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 106, cooldown: 15000, cooldownOffset: 14400),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 108, cooldown: 15000, cooldownOffset: 14600),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 109, cooldown: 15000, cooldownOffset: 14600),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 111, cooldown: 15000, cooldownOffset: 14800),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 112, cooldown: 15000, cooldownOffset: 14800),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 114, cooldown: 15000, cooldownOffset: 15000),
                            new Shoot(2, index: 4, count: 6, shootAngle: 60, fixedAngle: 115, cooldown: 15000, cooldownOffset: 15000),
                            new Shoot(2, 2, 45, index: 2, cooldown: 500, cooldownOffset: 200),
                            new Shoot(2, 2, 45, index: 3, cooldown: 500),
                            new Shoot(2, 1, fixedAngle: 180, index: 1, cooldown: 9999),
                            new Shoot(2, 1, fixedAngle: 0, index: 1, cooldown: 9999),
                            new Shoot(2, 1, fixedAngle: 169, index: 1, cooldown: 9999, cooldownOffset: 400),
                            new Shoot(2, 1, fixedAngle: 11, index: 1, cooldown: 9999, cooldownOffset: 400),
                            new Shoot(2, 1, fixedAngle: 158, index: 1, cooldown: 9999, cooldownOffset: 400),
                            new Shoot(2, 1, fixedAngle: 22, index: 1, cooldown: 9999, cooldownOffset: 400),
                            new Shoot(2, 1, fixedAngle: 147, index: 1, cooldown: 9999, cooldownOffset: 700),
                            new Shoot(2, 1, fixedAngle: 33, index: 1, cooldown: 9999, cooldownOffset: 700),
                            new Shoot(2, 1, fixedAngle: 135, index: 1, cooldown: 9999, cooldownOffset: 700),
                            new Shoot(2, 1, fixedAngle: 45, index: 1, cooldown: 9999, cooldownOffset: 700),
                            new Shoot(2, 1, fixedAngle: 124, index: 1, cooldown: 9999, cooldownOffset: 700),
                            new Shoot(2, 1, fixedAngle: 56, index: 1, cooldown: 9999, cooldownOffset: 700),
                            new Shoot(2, 1, fixedAngle: 117, index: 1, cooldown: 9999, cooldownOffset: 1000),
                            new Shoot(2, 1, fixedAngle: 63, index: 1, cooldown: 9999, cooldownOffset: 1000),
                            new Shoot(2, 1, fixedAngle: 103, index: 1, cooldown: 9999, cooldownOffset: 1150),
                            new Shoot(2, 1, fixedAngle: 77, index: 1, cooldown: 9999, cooldownOffset: 1150),
                            new Shoot(2, 1, fixedAngle: 90, index: 1, cooldown: 9999, cooldownOffset: 1250),
                            new Shoot(2, 1, fixedAngle: 90, index: 1, cooldown: 9999, cooldownOffset: 1250),
                            new Shoot(2, 1, fixedAngle: 100, index: 1, cooldown: 9999, cooldownOffset: 1310),
                            new Shoot(2, 1, fixedAngle: 80, index: 1, cooldown: 9999, cooldownOffset: 1310),
                            new Shoot(2, 1, fixedAngle: 124, index: 1, cooldown: 9999, cooldownOffset: 1400),
                            new Shoot(2, 1, fixedAngle: 56, index: 1, cooldown: 9999, cooldownOffset: 1400),
                            new Shoot(2, 1, fixedAngle: 135, index: 1, cooldown: 9999, cooldownOffset: 1550),
                            new Shoot(2, 1, fixedAngle: 45, index: 1, cooldown: 9999, cooldownOffset: 1550),
                            new Shoot(2, 1, fixedAngle: 147, index: 1, cooldown: 9999, cooldownOffset: 1650),
                            new Shoot(2, 1, fixedAngle: 33, index: 1, cooldown: 9999, cooldownOffset: 1650),
                            new Shoot(2, 1, fixedAngle: 180, index: 1, cooldown: 9999, cooldownOffset: 1750),
                            new Shoot(2, 1, fixedAngle: 0, index: 1, cooldown: 9999, cooldownOffset: 1750),
                            new Shoot(2, 1, fixedAngle: 169, index: 1, cooldown: 9999, cooldownOffset: 1850),
                            new Shoot(2, 1, fixedAngle: 11, index: 1, cooldown: 9999, cooldownOffset: 1850),
                            new Shoot(2, 1, fixedAngle: 169, index: 1, cooldown: 9999, cooldownOffset: 1950),
                            new Shoot(2, 1, fixedAngle: 11, index: 1, cooldown: 9999, cooldownOffset: 1950),
                            new Shoot(2, 1, fixedAngle: 158, index: 1, cooldown: 9999, cooldownOffset: 2050),
                            new Shoot(2, 1, fixedAngle: 22, index: 1, cooldown: 9999, cooldownOffset: 2050),
                            new Shoot(2, 1, fixedAngle: 147, index: 1, cooldown: 9999, cooldownOffset: 2150),
                            new Shoot(2, 1, fixedAngle: 33, index: 1, cooldown: 9999, cooldownOffset: 2150),
                            new Shoot(2, 1, fixedAngle: 135, index: 1, cooldown: 9999, cooldownOffset: 2250),
                            new Shoot(2, 1, fixedAngle: 45, index: 1, cooldown: 9999, cooldownOffset: 2250),
                            new Shoot(2, 1, fixedAngle: 124, index: 1, cooldown: 9999, cooldownOffset: 2350),
                            new Shoot(2, 1, fixedAngle: 56, index: 1, cooldown: 9999, cooldownOffset: 2350),
                            new Shoot(2, 1, fixedAngle: 117, index: 1, cooldown: 9999, cooldownOffset: 2450),
                            new Shoot(2, 1, fixedAngle: 63, index: 1, cooldown: 9999, cooldownOffset: 2450),
                            new Shoot(2, 1, fixedAngle: 90, index: 1, cooldown: 9999, cooldownOffset: 2550),
                            new Shoot(2, 1, fixedAngle: 90, index: 1, cooldown: 9999, cooldownOffset: 2550),
                            new Shoot(2, 1, fixedAngle: 100, index: 1, cooldown: 9999, cooldownOffset: 2610),
                            new Shoot(2, 1, fixedAngle: 80, index: 1, cooldown: 9999, cooldownOffset: 2610),
                            new Shoot(2, 1, fixedAngle: 124, index: 1, cooldown: 9999, cooldownOffset: 2680),
                            new Shoot(2, 1, fixedAngle: 56, index: 1, cooldown: 9999, cooldownOffset: 2680),
                            new Shoot(2, 1, fixedAngle: 135, index: 1, cooldown: 9999, cooldownOffset: 2830),
                            new Shoot(2, 1, fixedAngle: 45, index: 1, cooldown: 9999, cooldownOffset: 2830),
                            new Shoot(2, 1, fixedAngle: 147, index: 1, cooldown: 9999, cooldownOffset: 2980),
                            new Shoot(2, 1, fixedAngle: 33, index: 1, cooldown: 9999, cooldownOffset: 2980),
                            new Shoot(2, 1, fixedAngle: 180, index: 1, cooldown: 9999, cooldownOffset: 3030),
                            new Shoot(2, 1, fixedAngle: 0, index: 1, cooldown: 9999, cooldownOffset: 3030),
                            new Shoot(2, 1, fixedAngle: 169, index: 1, cooldown: 9999, cooldownOffset: 3180),
                            new Shoot(2, 1, fixedAngle: 11, index: 1, cooldown: 9999, cooldownOffset: 3180),
                            new Shoot(2, 1, fixedAngle: 169, index: 1, cooldown: 9999, cooldownOffset: 3230),
                            new Shoot(2, 1, fixedAngle: 11, index: 1, cooldown: 9999, cooldownOffset: 3230),
                            new Shoot(2, 1, fixedAngle: 158, index: 1, cooldown: 9999, cooldownOffset: 3380),
                            new Shoot(2, 1, fixedAngle: 22, index: 1, cooldown: 9999, cooldownOffset: 3380),
                            new Shoot(2, 1, fixedAngle: 147, index: 1, cooldown: 9999, cooldownOffset: 3530),
                            new Shoot(2, 1, fixedAngle: 33, index: 1, cooldown: 9999, cooldownOffset: 3530),
                            new Shoot(2, 1, fixedAngle: 135, index: 1, cooldown: 9999, cooldownOffset: 3680),
                            new Shoot(2, 1, fixedAngle: 45, index: 1, cooldown: 9999, cooldownOffset: 3680),
                            new Shoot(2, 1, fixedAngle: 124, index: 1, cooldown: 9999, cooldownOffset: 3830),
                            new Shoot(2, 1, fixedAngle: 56, index: 1, cooldown: 9999, cooldownOffset: 3830),
                            new Shoot(2, 1, fixedAngle: 117, index: 1, cooldown: 9999, cooldownOffset: 3980),
                            new Shoot(2, 1, fixedAngle: 63, index: 1, cooldown: 9999, cooldownOffset: 3980)
                            ),
                        new State("diewait",
                            new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new ConditionalEffect(ConditionEffectIndex.Invisible),
                            new ConditionalEffect(ConditionEffectIndex.Stasis),
                            new PlayerWithinTransition(7, "dieshoot")
                            ),
                        new State("heheh",
                            new RemoveConditionalEffect(ConditionEffectIndex.Invincible),
                            new RemoveConditionalEffect(ConditionEffectIndex.Invisible),
                            new RemoveConditionalEffect(ConditionEffectIndex.Stasis),
                            new Taunt("Ha... haa..."),
                            new Shoot(40, 6, index: 4, rotateAngle: 1, cooldown: 999999),
                            new Shoot(40, 6, index: 4, rotateAngle: 1, cooldown: 999999, cooldownOffset: 50),
                            new Shoot(40, 6, index: 4, rotateAngle: 1, cooldown: 999999, cooldownOffset: 100),
                            new TimedTransition("flash", 8000)
                            ),
                        new State("flash",
                            new Flash(0xfFF0000, flashRepeats: 10000, flashPeriod: 0.1f),
                            new TimedTransition("diedie", 2000)


                            ),

                        new State("Death",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new CopyDamageOnDeath("shtrs Loot Balloon King"),
                            new Order(1, "shtrs Chest Spawner 3", "Open"),
                            new Taunt("Impossible..........IMPOSSIBLE!"),
                            new TimedTransition("Suicide", 2000)
                            ),
                        new State("Suicide",
                            new Shoot(35, index: 0, count: 30),
                            new Suicide()
                    )
                )
            )
            db.Init("shtrs Royal Guardian J",
                new State("base",
                    new State("shoot",
                        new Orbit(0.35f, 2, 5, "shtrs The Forgotten King"),
                        new Shoot(15, 8, index: 0, cooldown: new Cooldown(3600, 3600))
                        )
                    )
            )
            db.Init("shtrs Royal Guardian L",
                new State("base",
                    new State("1st",
                        new Follow(1, 8, 5),
                        new Shoot(15, 20, index: 0),
                        new TimedTransition("2nd", 1000)
                        ),
                    new State("2nd",
                        new Follow(1, 8, 5),
                        new Shoot(10, index: 1),
                        new TimedTransition("3rd", 1000)
                        ),
                    new State("3rd",
                        new Follow(1, 8, 5),
                        new Shoot(10, index: 1),
                        new TimedTransition("1st", 1000)
                        )
                    )
            )
            db.Init("shtrs Green Crystal",
                new State("base",
                    new HealGroup(30, "Crystals", cooldown: 2000, healAmount: 1500),
                    new State("spawn",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Orbit(0.4f, 1, 5, "shtrs The Forgotten King"),
                        new TimedTransition("follow", 7000)
                        ),
                    new State("follow",
                        new Follow(0.4f, range: 6),
                        new Follow(0.6f, range: 2),
                        new TimedTransition("dafuq", 3000)
                        ),
                    new State("dafuq",
                        new Orbit(1.0f, 4, 10, "shtrs Crystal Tracker"),
                        new Follow(0.4f, range: 6),
                        new Follow(0.4f, range: 2, duration: 2000, cooldown: 1500),
                        new TimedTransition("follow", 2000)
                        )
                    )
            )
            db.Init("shtrs Yellow Crystal",
                new State("base",
                    new State("spawn",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Orbit(0.4f, 1, 5, "shtrs The Forgotten King"),
                        new TimedTransition("follow", 7000)
                        ),
                    new State("follow",
                        new Follow(0.4f, range: 6),
                        new Follow(0.4f, range: 6),
                        new TimedTransition("dafuq", 25)
                        ),
                    new State("dafuq",
                        new Orbit(1.0f, 4, 10, "shtrs Crystal Tracker"),
                        new Follow(0.4f, range: 6),
                        new Shoot(5, 4, 4, index: 0, cooldown: 1000)
                        )
                    )
            )
            db.Init("shtrs Red Crystal",
                new State("base",
                     new State("spawn",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Orbit(0.4f, 1, 5, "shtrs The Forgotten King"),
                        new TimedTransition("orbit", 7000)
                        ),
                    new State("orbit",
                        new TossObject("shtrs Fire Portal", 5, cooldown: 8000),
                        new Orbit(1.0f, 4, 10, "shtrs Crystal Tracker"),
                        new Follow(0.4f, range: 6),
                        new Follow(0.4f, range: 6),
                        new TimedTransition("ThrowPortal", 5000)
                        ),
                    new State("ThrowPortal",
                       new Orbit(1.0f, 4, 10, "shtrs Crystal Tracker"),
                        new Follow(0.4f, range: 6),
                        new Follow(0.4f, range: 6),
                        new TossObject("shtrs Fire Portal", 5, cooldown: 8000),
                        new TimedTransition("orbit", 8000)
                        )
                    )
            )
            db.Init("shtrs Blue Crystal",
                new State("base",
                     new State("spawn",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Orbit(0.4f, 1, 5, "shtrs The Forgotten King"),
                        new TimedTransition("orbit", 7000)
                        ),
                    new State("orbit",
                        new TossObject("shtrs Ice Portal", 5, cooldown: 8000),
                        new Orbit(1.0f, 4, 10, "shtrs Crystal Tracker"),
                        new Follow(0.4f, range: 6),
                        new TimedTransition("ThrowPortal", 5000)
                        ),
                    new State("ThrowPortal",
                        new Orbit(1.0f, 4, 10, "shtrs Crystal Tracker"),
                        new Follow(0.4f, range: 6),
                        new Follow(0.4f, range: 6),
                        new TossObject("shtrs Ice Portal", 5, cooldown: 8000),
                        new TimedTransition("orbit", 8000)
                        )
                    )
            )
        db.Init("shtrs Crystal Tracker",
            new State("base",
                new Follow(2, 10, 1)
                )
            )
        db.Init("shtrs king timer",
            new State("base",
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("wait",
                    new EntitiesNotExistsTransition(100, "death", "shtrs The Forgotten King")
                        ),
                    new State("timer1",
                        new TimedTransition("heheh", 28000)
                        ),
                    new State("heheh",
                        new Order(60, "shtrs The Forgotten King", "heheh"),
                        new TimedTransition("wait", 1)
                        ),
                    new State("death",
                        new Suicide()
                        )
                )
            )
        db.Init("shtrs king lava1",
            new State("base",
                 new ConditionalEffect(ConditionEffectIndex.Invincible),
                 new State("wait",
                    new ConditionalEffect(ConditionEffectIndex.Invisible)
                        ),
                 new State("lava",
                    new ReplaceTile("Dark Cobblestone", "Hot Lava", 0)
                     ),
                 new State("death",
                     new Suicide()
                 )
            )
            )
          db.Init("shtrs king lava2",
            new State("base",
                 new ConditionalEffect(ConditionEffectIndex.Invincible),
                 new State("wait",
                    new ConditionalEffect(ConditionEffectIndex.Invisible)
                        ),
                 new State("lava",
                    new ReplaceTile("Dark Cobblestone", "Hot Lava", 0)
                     ),
                 new State("death",
                     new Suicide()
                 )
            )
                )
        db.Init("shtrs The Cursed Crown",
            new State("base",
                new State("Idle",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new EntitiesNotExistsTransition(100, "Open", "shtrs Royal Guardian L")
                    ),
                new State("Open",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new MoveTo2(0, -15, 0.5f),
                    new TimedTransition("WADAFAK", 3000)
                    ),
                new State("WADAFAK",
                    new TransformOnDeath("shtrs The Forgotten King"),
                    new Suicide()
                    )
                )
            )
        #endregion 3rdboss
        #region 3rdbosschest
            db.Init("shtrs Loot Balloon King",
                new State("base",
                    new State("Idle",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition("Crown", 5000)
                    ),
                    new State("Crown")
                ),
                new Threshold(0.1f,
                    new TierLoot(11, LootType.Weapon, 1),
                    new TierLoot(12, LootType.Weapon, 1),
                    new TierLoot(6, LootType.Ability, 1),
                    new TierLoot(12, LootType.Armor, 1),
                    new TierLoot(13, LootType.Armor, 1),
                    new TierLoot(6, LootType.Ring, 1),
                    new ItemLoot("Potion of Life", 1),
                    new ItemLoot("The Forgotten Crown", 0.01f)
                )
            )
        #endregion 3rdbosschest
        // Use this for other stuff.
        #region NotInUse
        //      db.Init("shtrs Spawn Bridge 6",
        //          new State("base",
        //              new State("Idle",
        //                  new ConditionalEffect(ConditionEffectIndex.Invincible, true),
        //                  new EntitiesNotExistsTransition(100, "Open", "shtrs Royal Guardian L")
        //                  ),
        //              new State("Open",
        //                  new ChangeGroundOnDeath(new[] { "Green BigSmall Squared" }, new[] { "Hot Lava" },
        //                      1),
        //                  new Suicide()
        //                  )
        //              )
        //          )
        //      db.Init("shtrs Spawn Bridge 7",
        //          new State("base",
        //              new State("Idle",
        //                  new ConditionalEffect(ConditionEffectIndex.Invincible, true),
        //                  new EntitiesNotExistsTransition(100, "Open", "shtrs Royal Guardian L")
        //                  ),
        //              new State("Open",
        //                  new ChangeGroundOnDeath(new[] { "Gold Tile" }, new[] { "Hot Lava" },
        //                      1),
        //                  new Suicide()
        //                  )
        //              )
        //          )
        //      db.Init("shtrs Spawn Bridge 8",
        //          new State("base",
        //              new State("Idle",
        //                  new ConditionalEffect(ConditionEffectIndex.Invincible, true),
        //                  new EntitiesNotExistsTransition(100, "Open", "shtrs Royal Guardian L")
        //                  ),
        //              new State("Open",
        //                  new ChangeGroundOnDeath(new[] { "Shattered Floor" }, new[] { "Hot Lava" },
        //                      1),
        //                  new Suicide()
        //                  )
        //              )
        //          )
        #endregion NotInUse
        #region MISC
        db.Init("shtrs Chest Spawner 1",
            new State("base",
                new State("Idle",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new EntitiesNotExistsTransition(500, "Open", "shtrs Bridge Sentinel")
                    ),
                new State("Open",
                    new TransformOnDeath("shtrs Loot Balloon Bridge"),
                    new Suicide()
                    )
                )
            )
        db.Init("shtrs Chest Spawner 2",
            new State("base",
                new State("Idle",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new EntitiesNotExistsTransition(500, "Open", "shtrs Twilight Archmage")
                    ),
                new State("Open",
                    new TransformOnDeath("shtrs Loot Balloon Mage"),
                    new Suicide()
                    )
                )
            )
        db.Init("shtrs blobomb maker",
            new State("base",
                 new State("Idle",
                    new ConditionalEffect(ConditionEffectIndex.Invincible)
                    ),
                     new State("Spawn",
                        new Spawn("shtrs Blobomb", cooldown: 1500),
                        new TimedTransition("Spawn2", 3000)
                         ),
                     new State("Spawn2",
                         new Spawn("shtrs Blobomb", cooldown: 1000),
                         new TimedTransition("Idle", 3000)
                        )
                    )
                )
          db.Init("shtrs Lava Souls maker",
            new State("base",
                 new State("Idle",
                    new ConditionalEffect(ConditionEffectIndex.Invincible)
                    ),
                     new State("Spawn",
                        new Spawn("shtrs Lava Souls", maxChildren: 1, cooldown: 8000),
                        new TimedTransition("Spawn2", 8000)
                         ),
                     new State("Spawn2",
                         new Spawn("shtrs Lava Souls", maxChildren: 1, cooldown: 8000),
                         new TimedTransition("Spawn", 8000)
                        )
                    )
                )

        db.Init("shtrs Chest Spawner 3",
            new State("base",
                new State("Idle",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new EntitiesNotExistsTransition(30, "Open", "shtrs The Cursed Crown", "shtrs The Forgotten King")
                    ),
                new State("Open",
                    new TransformOnDeath("shtrs Loot Balloon King"),
                    new Suicide()
                    )
                )
            )
        #endregion MISC
            ;
    }
}
