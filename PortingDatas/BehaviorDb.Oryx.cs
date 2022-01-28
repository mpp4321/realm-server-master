#region

using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

#endregion

namespace RotMG.Game.Logic.Database
{
    class Oryx
    {
        private _ Oryx = () => Behav()
            db.Init("Oryx the Mad God 2",
                new State("base",
                    new ScaleHP(50000),
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
                        new Spawn("Henchman of Oryx", 5, cooldown: 5000),
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
                new MostDamagers(3,
                    LootTemplates.RaidTokens()
                ),
                 new MostDamagers(3,
                    LootTemplates.Sor5Perc()
                    ),
                new Threshold(0.29f,
                    new ItemLoot("Potion of Vitality", 1)
                ),
                new Threshold(0.05f,
                    new ItemLoot("Potion of Attack", 0.3f),
                    new ItemLoot("Potion of Defense", 0.3f),
                    new ItemLoot("Potion of Wisdom", 0.3f)
                ),
                new Threshold(0.20f,
                    new ItemLoot("Oryx's Arena Key", 0.01f)
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
            )

                    db.Init("Oryx the Mad God 2OA",
                new State("base",
                    new ScaleHP(50000),
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
                        new Spawn("Henchman of Oryx", 5, cooldown: 5000),
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
                 new MostDamagers(3,
                    LootTemplates.Sor5Perc()
                    ),
                new Threshold(0.29f,
                    new ItemLoot("Potion of Vitality", 1)
                ),
                new Threshold(0.05f,
                    new ItemLoot("Potion of Attack", 0.3f),
                    new ItemLoot("Potion of Defense", 0.3f),
                    new ItemLoot("Potion of Wisdom", 0.3f)
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
            )

            db.Init("Oryx the Mad God 1",
                new State("base",
                    new DropPortalOnDeath("Wine Cellar Portal", 100, timeout: 120),
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
                        new Shoot(count: 2, cooldown: 1000, index: 1, radius: 7, shootAngle: 10,
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
                        new Shoot(count: 2, cooldown: 1500, index: 1, radius: 7, shootAngle: 10,
                            cooldownOffset: 2000),
                        new Shoot(count: 5, cooldown: 1500, index: 16, radius: 7, shootAngle: 10,
                            cooldownOffset: 2000),
                        new Follow(0.85f, range: 1, cooldown: 0),
                        new Flash(0xfFF0000, 0.5f, 9000001)
                        )
                    ),
                new MostDamagers(3,
                    LootTemplates.Sor5Perc()
                    ),
                new Threshold(0.20f,
                    new ItemLoot("Oryx's Arena Key", 0.005f)
                ),
                new Threshold(0.05f,
                    new ItemLoot("Potion of Attack", 0.3f),
                    new ItemLoot("Potion of Defense", 0.3f),
                    new ItemLoot("The Zol Awakening (Token)", 0.001f),
                    new ItemLoot("Calling of the Titan (Token)", 0.001f)
                ),
                new Threshold(0.1f,
                    new TierLoot(10, LootType.Weapon, 0.07f),
                    new TierLoot(11, LootType.Weapon, 0.06f),
                    new TierLoot(5, LootType.Ability, 0.07f),
                    new TierLoot(11, LootType.Armor, 0.07f),
                    new TierLoot(5, LootType.Ring, 0.06f)
                )
            )
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
            )
            db.Init("Minion of Oryx",
                new State("base",
                    new Wander(0.4f),
                    new Shoot(3, 3, 10, 0, cooldown: 1000),
                    new Shoot(3, 3, index: 1, shootAngle: 10, cooldown: 1000)
                    ),
                new TierLoot(7, LootType.Weapon, 0.2f),
                new ItemLoot("Magic Potion", 0.03f)
            )
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
            )
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
            )
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
                            )
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
                     new TossObject("Monstrosity Scarab", cooldown: 10000, range: 1, angle: 0, cooldownOffset: 1000)
                     )
                     ))
            db.Init("Monstrosity Scarab",
                new State("base",
                    new State("Attack",
                    new State("Charge",
                        new Prioritize(
                            new Charge(range: 25, cooldown: 1000),
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
                       )
            db.Init("Vintner of Oryx",
                new State("base",
                    new State("Attack",
                        new Prioritize(
                            new Protect(1, "Oryx the Mad God 2", protectionRange: 4, reprotectRange: 3),
                            new Charge(speed: 1, range: 15, cooldown: 2000),
                            new Protect(1, "Henchman of Oryx"),
                            new StayBack(1, 15),
                            new Wander(1)
                        ),
                        new Shoot(10, cooldown: 250)
                        )
                        ))
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
                    new TimedTransition("Toss1", randomized: true, 100),
                    new TimedTransition("Toss2", randomized: true, 100),
                    new TimedTransition("Toss3", randomized: true, 100),
                    new TimedTransition("Toss4", randomized: true, 100),
                    new TimedTransition("Toss5", randomized: true, 100),
                    new TimedTransition("Toss6", randomized: true, 100),
                    new TimedTransition("Toss7", randomized: true, 100),
                    new TimedTransition("Toss8", randomized: true, 100)
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
                    ))
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
                    )
        db.Init("Bile of Oryx",
            new State("base",
                new Prioritize(
                    new Protect(.4f, "Oryx the Mad God 2", protectionRange: 5, reprotectRange: 4),
                    new Wander(.5f)
                    )//,
                     //new Spawn("Purple Goo", maxChildren: 20, initialSpawn: 0, cooldown: 1000)
                )
                )
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
                    );
    }
}
