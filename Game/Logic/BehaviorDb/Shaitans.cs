using RotMG.Common;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;
using RotMG.Game.SetPieces;
using RoTMG.Game.Logic.Transitions;
using System;
using System.Collections.Generic;
using System.Text;
using static RotMG.Game.Logic.Loots.TierLoot;

namespace RotMG.Game.Logic.Database
{
    class Shaitans : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {

            //db.Init("md1 Head of Shaitan",
            //    new State("1",
            //            new ConditionalEffect(ConditionEffectIndex.Invincible),
            //            new State("2",
            //                new PlayerWithinTransition(4, "3", true)
            //            ),
            //            new State("3",
            //                new OrderOnEntry(999, "md1 Right Hand of Shaitan", "2"),
            //                new OrderOnEntry(999, "md1 Left Hand of Shaitan", "2"),
            //                new TimedTransition("4", 0) { SubIndex = 2 } //Move an extra layer out while looking for next transition
            //            )
            //    ),
            //    new State("4",
            //        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
            //        new ChangeSize(25, 250),
            //        new State("nothin"),
            //        new State("5",
            //            new Shoot(9, 1, index: 0, cooldown: 1000),
            //            new EntitiesNotExistsTransition(999, "6", "md1 Right Hand of Shaitan", "md1 Left Hand of Shaitan")
            //        ),
            //        new State("6",
            //            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
            //            new SetAltTexture(1),
            //            new OrderOnEntry(999, "md1 Governor", "1"),
            //            new MoveTo(0.8f, 13.5f, 5),
            //            new TimedTransition("7", 2000) { SubIndex = 2 }
            //        )
            //    )
            //);

            db.Init("md1 Head of Shaitan",
                HPScale.BOSS_HP_SCALE_DEFAULT(),
                new State("base",
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new State("2",
                            new PlayerWithinTransition(4, "3", true)
                        ),
                        new State("3",
                            new OrderOnEntry(999, "md1 Right Hand of Shaitan", "2"),
                            new OrderOnEntry(999, "md1 Left Hand of Shaitan", "2"),
                            new TimedTransition("4", 0) { SubIndex = 2 } //Move an extra layer out while looking for next transition
                        )
                    ),
                    new State("4",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new ChangeSize(25, 250),
                        //State entry isnt default in this soorce
                        new TransitionFrom("4", "5"),
                        new State("5",
                            new Shoot(9, 1, index: 0, cooldown: 1000),
                            new EntitiesNotExistsTransition(999, "6", "md1 Right Hand of Shaitan", "md1 Left Hand of Shaitan")
                        ),
                        new State("6",
                            new SetAltTexture(1),
                            new OrderOnEntry(999, "md1 Governor", "1"),
                            new MoveTo(0.8f, 13.5f, 5f),
                            new TimedTransition("7", 2000) { SubIndex = 2 }
                        )
                    ),
                    new State("7",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition("8", 0)
                    ),
                    new State("8",
                        new HealthTransition(0.8f, "18"),
                        new SetAltTexture(0),
                        new Shoot(8, 7, 15, 0, predictive: 1, cooldown: 1000),
                        new TimedTransition("9", 5000)
                    ),
                    new State("9",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(1),
                        new MoveTo(0.8f, 8.5f, 5),
                        new TimedTransition("10", 2000)
                        ),
                    new State("10",
                        new HealthTransition(0.8f, "18"),
                        new SetAltTexture(0),
                        new Shoot(8, 7, 15, 0, predictive: 1, cooldown: 1000),
                        new TimedTransition("11", 5000)
                        ),
                    new State("11",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(1),
                        new MoveTo(0.8f, 3.5f, 5),
                        new TimedTransition("12", 2000)
                        ),
                    new State("12",
                        new HealthTransition(0.8f, "18"),
                        new SetAltTexture(0),
                        new Shoot(8, 7, 15, 0, predictive: 1, cooldown: 1000),
                        new TimedTransition("13", 5000)
                        ),
                    new State("13",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new ReturnToSpawn(0.8f),
                        new TimedTransition("14", 2000)
                        ),
                    new State("14",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new OrderOnEntry(999, "md1 Right Hand spawner", "1"),
                        new OrderOnEntry(999, "md1 Left Hand spawner", "1"),
                        new EntitiesWithinTransition(999, "md1 Right Hand of Shaitan", "15")
                        ),
                    new State("15",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntitiesWithinTransition(999, "md1 Left Hand of Shaitan", "16")
                        ),
                    new State("16",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new OrderOnEntry(999, "md1 Right Hand of Shaitan", "14"),
                        new OrderOnEntry(999, "md1 Left Hand of Shaitan", "14"),
                        new TimedTransition("17", 0)
                        ),
                    new State("17",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Shoot(9, 1, index: 0, cooldown: 1000),
                        new EntitiesNotExistsTransition(999, "5", "md1 Right Hand of Shaitan", "md1 Left Hand of Shaitan")
                        ),
                    //Phase 2
                    new State("18",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(0),
                        new ReturnToSpawn(0.8f),
                        new Taunt(true, 0, "Let loose the fists of war!"),
                        new Shoot(9, 1, index: 0, cooldown: 1000),
                        new TransitionFrom("18", "19"),
                        new State("19",
                            new OrderOnEntry(999, "md1 Governor", "1"),
                            new OrderOnEntry(999, "md1 Right Hand spawner", "2"),
                            new OrderOnEntry(999, "md1 Left Hand spawner", "2"),
                            new TossObject("md1 CreepyHead", 5, 90, throwEffect: true),
                            new TimedTransition("20", 0)
                            ),
                        new State("20",
                            new TossObject("md1 Lava Makers", 14, cooldown: 500),
                            new TimedTransition("21", 5500) { SubIndex = 2 }
                            )
                        ),
                    new State("21",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(1),
                        new OrderOnEntry(999, "md1 Governor", "1"),
                        new MoveTo(0.8f, 13.5f, 5),
                        new TimedTransition("22", 2000)
                        ),
                    new State("22",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition("23", 0)
                        ),
                    new State("23",
                        new HealthTransition(0.6f, "43"),
                        new SetAltTexture(0),
                        new Shoot(8, 7, 15, 0, predictive: 1, cooldown: 1000),
                        new TimedTransition("24", 5000)
                        ),
                    new State("24",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(1),
                        new MoveTo(0.8f, 8.5f, 5),
                        new TimedTransition("25", 2000)
                        ),
                    new State("25",
                        new HealthTransition(0.6f, "43"),
                        new SetAltTexture(0),
                        new Shoot(8, 7, 15, 0, predictive: 1, cooldown: 1000),
                        new TimedTransition("26", 5000)
                        ),
                    new State("26",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(1),
                        new MoveTo(0.8f, 3.5f, 5),
                        new TimedTransition("27", 2000)
                        ),
                    new State("27",
                        new HealthTransition(0.6f, "43"),
                        new SetAltTexture(0),
                        new Shoot(8, 7, 15, 0, predictive: 1, cooldown: 1000),
                        new TimedTransition("28", 5000)
                        ),
                    new State("28",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new ReturnToSpawn(0.8f),
                        new TimedTransition("29", 2000)
                        ),
                    new State("29",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new OrderOnEntry(999, "md1 Right Hand spawner", "1"),
                        new OrderOnEntry(999, "md1 Left Hand spawner", "1"),
                        new EntitiesWithinTransition(999, "md1 Right Hand of Shaitan", "30")
                        ),
                    new State("30",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntitiesWithinTransition(999, "md1 Left Hand of Shaitan", "31")
                        ),
                    new State("31",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new OrderOnEntry(999, "md1 Right Hand of Shaitan", "14"),
                        new OrderOnEntry(999, "md1 Left Hand of Shaitan", "14"),
                        new TimedTransition("32", 0)
                        ),
                    new State("32",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Shoot(9, 1, index: 0, cooldown: 1000),
                        new EntitiesNotExistsTransition(999, "33", "md1 Right Hand of Shaitan", "md1 Left Hand of Shaitan")
                        ),
                    new State("33",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(0),
                        new ReturnToSpawn(0.8f),
                        new Shoot(9, 1, index: 0, cooldown: 1000),
                        new State("34",
                            new OrderOnEntry(999, "md1 Governor", "1"),
                            new OrderOnEntry(999, "md1 Right Hand spawner", "2"),
                            new OrderOnEntry(999, "md1 Left Hand spawner", "2"),
                            new TossObject("md1 CreepyHead", 5, 90, throwEffect: true),
                            new TimedTransition("35", 0)
                            ),
                        new State("35",
                            new TossObject("md1 Lava Makers", 14, cooldown: 500),
                            new TimedTransition("36", 5500)
                            )
                        ),
                    new State("36",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(1),
                        new OrderOnEntry(999, "md1 Governor", "1"),
                        new MoveTo(0.8f, 13.5f, 5),
                        new TimedTransition("37", 2000)
                        ),
                    new State("37",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition("38", 0)
                        ),
                    new State("38",
                        new HealthTransition(0.6f, "43"),
                        new SetAltTexture(0),
                        new Shoot(8, 7, 15, 0, predictive: 1, cooldown: 1000),
                        new TimedTransition("39", 5000)
                        ),
                    new State("39",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(1),
                        new MoveTo(0.8f, 8.5f, 5),
                        new TimedTransition("40", 2000)
                        ),
                    new State("40",
                        new HealthTransition(0.6f, "43"),
                        new SetAltTexture(0),
                        new Shoot(8, 7, 15, 0, predictive: 1, cooldown: 1000),
                        new TimedTransition("41", 5000)
                        ),
                    new State("41",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(1),
                        new MoveTo(0.8f, 3.5f, 5),
                        new TimedTransition("42", 2000)
                        ),
                    new State("42",
                        new HealthTransition(0.6f, "43"),
                        new SetAltTexture(0),
                        new Shoot(8, 7, 15, 0, predictive: 1, cooldown: 1000),
                        new TimedTransition("28", 5000)
                        ),
                    //Phase 3
                    new State("43",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(0),
                        new ReturnToSpawn(0.8f),
                        new Taunt(true, 0, "YOUR ARE BEGINNING TO UPSET ME. ENJOY A FAST DEATH!"),
                        new Shoot(9, 1, index: 0, cooldown: 1000),
                        new TransitionFrom("43", "44"),
                        new State("44",
                            new OrderOnEntry(999, "md1 Governor", "1"),
                            new OrderOnEntry(999, "md1 Right Hand spawner", "2"),
                            new OrderOnEntry(999, "md1 Left Hand spawner", "2"),
                            new TossObject("md1 CreepyHead", 5, 65, throwEffect: true),
                            new TossObject("md1 CreepyHead", 5, 105, throwEffect: true),
                            new TimedTransition("45", 0)
                            ),
                        new State("45",
                            new TossObject("md1 Lava Makers", 14, cooldown: 500),
                            new TimedTransition("46", 5500) { SubIndex = 2 }
                            )
                        ),
                    new State("46",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(1),
                        new OrderOnEntry(999, "md1 Governor", "1"),
                        new MoveTo(0.8f, 13.5f, 5),
                        new TimedTransition("47", 2000)
                        ),
                    new State("47",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition("48", 0)
                        ),
                    new State("48",
                        new HealthTransition(0.4f, "68"),
                        new SetAltTexture(0),
                         new ConditionalEffect(ConditionEffectIndex.ArmorBroken),
                        new Shoot(8, 7, 15, 0, predictive: 1, cooldown: 1000),
                        new TimedTransition("49", 5000)
                        ),
                    new State("49",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(1),
                        new MoveTo(0.8f, 8.5f, 5),
                        new TimedTransition("50", 2000)
                        ),
                    new State("50",
                        new HealthTransition(0.4f, "68"),
                        new SetAltTexture(0),
                         new ConditionalEffect(ConditionEffectIndex.ArmorBroken),
                        new Shoot(8, 7, 15, 0, predictive: 1, cooldown: 1000),
                        new TimedTransition("51", 5000)
                        ),
                    new State("51",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(1),
                        new MoveTo(0.8f, 3.5f, 5),
                        new TimedTransition("52", 2000)
                        ),
                    new State("52",
                        new HealthTransition(0.4f, "68"),
                        new SetAltTexture(0),
                         new ConditionalEffect(ConditionEffectIndex.ArmorBroken),
                        new Shoot(8, 7, 15, 0, predictive: 1, cooldown: 1000),
                        new TimedTransition("53", 5000)
                        ),
                    new State("53",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new ReturnToSpawn(0.8f),
                        new TimedTransition("54", 2000)
                        ),
                    new State("54",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new OrderOnEntry(999, "md1 Right Hand spawner", "1"),
                        new OrderOnEntry(999, "md1 Left Hand spawner", "1"),
                        new EntitiesWithinTransition(999, "md1 Right Hand of Shaitan", "55")
                        ),
                    new State("55",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntitiesWithinTransition(999, "md1 Left Hand of Shaitan", "56")
                        ),
                    new State("56",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new OrderOnEntry(999, "md1 Right Hand of Shaitan", "14"),
                        new OrderOnEntry(999, "md1 Left Hand of Shaitan", "14"),
                        new TimedTransition("57", 0)
                        ),
                    new State("57",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Shoot(9, 1, index: 0, cooldown: 1000),
                        new EntitiesNotExistsTransition(999, "58", "md1 Right Hand of Shaitan", "md1 Left Hand of Shaitan")
                        ),
                    new State("58",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(0),
                        new ReturnToSpawn(0.8f),
                        new Shoot(9, 1, index: 0, cooldown: 1000),
                        new TransitionFrom("58", "59"),
                        new State("59",
                            new OrderOnEntry(999, "md1 Governor", "1"),
                            new OrderOnEntry(999, "md1 Right Hand spawner", "2"),
                            new OrderOnEntry(999, "md1 Left Hand spawner", "2"),
                            new TossObject("md1 CreepyHead", 5, 65, throwEffect: true),
                            new TossObject("md1 CreepyHead", 5, 105, throwEffect: true),
                            new TimedTransition("60", 0)
                            ),
                        new State("60",
                            new TossObject("md1 Lava Makers", 14, cooldown: 500),
                            new TimedTransition("61", 5500) { SubIndex = 2 }
                            )
                        ),
                    new State("61",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(1),
                        new OrderOnEntry(999, "md1 Governor", "1"),
                        new MoveTo(0.8f, 13.5f, 5),
                        new TimedTransition("62", 0)
                        ),
                    new State("62",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition("63", 0)
                        ),
                    new State("63",
                        new HealthTransition(0.4f, "68"),
                        new SetAltTexture(0),
                        new ConditionalEffect(ConditionEffectIndex.ArmorBroken),
                        new Shoot(8, 7, 15, 0, predictive: 1, cooldown: 1000),
                        new TimedTransition("64", 5000)
                    ),
                    new State("64",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(1),
                        new MoveTo(0.8f, 8.5f, 5),
                        new TimedTransition("65", 5000)
                    ),
                    new State("65",
                        new HealthTransition(0.4f, "68"),
                        new SetAltTexture(0),
                         new ConditionalEffect(ConditionEffectIndex.ArmorBroken),
                        new Shoot(8, 7, 15, 0, predictive: 1, cooldown: 1000),
                        new TimedTransition("66", 5000)
                        ),
                    new State("66",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(1),
                        new MoveTo(0.8f, 3.5f, 5),
                        new TimedTransition("67", 2000)
                        ),
                    new State("67",
                        new HealthTransition(0.4f, "68"),
                        new SetAltTexture(0),
                         new ConditionalEffect(ConditionEffectIndex.ArmorBroken),
                        new Shoot(8, 7, 15, 0, predictive: 1, cooldown: 1000),
                        new TimedTransition("53", 5000)
                        ),
                    //Phase 4
                    new State("68",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(0),
                        new ReturnToSpawn(0.8f),
                        new Taunt(true, 0, "BE CONSUMED BY FLAMES!"),
                        new Shoot(9, 1, index: 0, cooldown: 1000),
                        new TransitionFrom("68", "69"),
                        new State("69",
                            new OrderOnEntry(999, "md1 Governor", "1"),
                            new OrderOnEntry(999, "md1 Right Hand spawner", "2"),
                            new OrderOnEntry(999, "md1 Left Hand spawner", "2"),
                            new TossObject("md1 CreepyHead", 5, 65, throwEffect: true),
                            new TossObject("md1 CreepyHead", 5, 90, throwEffect: true),
                            new TossObject("md1 CreepyHead", 5, 105, throwEffect: true),
                            new TimedTransition("70", 0)
                        ),
                        new State("70",
                            new TossObject("md1 Lava Makers", 14, cooldown: 500),
                            new TimedTransition("71", 5500) { SubIndex = 2 }
                            )
                        ),
                    new State("71",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(1),
                        new OrderOnEntry(999, "md1 Governor", "1"),
                        new MoveTo(0.8f, 13.5f, 5),
                        new TimedTransition("72", 2000)
                        ),
                    new State("72",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition("73", 0)
                        ),
                    new State("73",
                        new HealthTransition(0.2f, "94"),
                        new SetAltTexture(0),
                         new ConditionalEffect(ConditionEffectIndex.ArmorBroken),
                        new Shoot(8, 7, 15, 0, predictive: 1, cooldown: 1000),
                        new TimedTransition("74", 5000)
                        ),
                    new State("74",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(1),
                        new MoveTo(0.8f, 8.5f, 5),
                        new TimedTransition("75", 2000)
                        ),
                    new State("75",
                        new HealthTransition(0.2f, "94"),
                        new SetAltTexture(0),
                         new ConditionalEffect(ConditionEffectIndex.ArmorBroken),
                        new Shoot(8, 7, 15, 0, predictive: 1, cooldown: 1000),
                        new TimedTransition("76", 5000)
                        ),
                    new State("76",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(1),
                        new MoveTo(0.8f, 3.5f, 5),
                        new TimedTransition("77", 2000)
                        ),
                    new State("77",
                        new HealthTransition(0.2f, "94"),
                        new SetAltTexture(0),
                        new ConditionalEffect(ConditionEffectIndex.ArmorBroken),
                        new Shoot(8, 7, 15, 0, predictive: 1, cooldown: 1000),
                        new TimedTransition("78", 5000)
                    ),
                    new State("78",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new ReturnToSpawn(0.8f),
                        new TimedTransition("79", 2000)
                    ),
                    new State("79",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new OrderOnEntry(999, "md1 Right Hand spawner", "1"),
                        new OrderOnEntry(999, "md1 Left Hand spawner", "1"),
                        new EntitiesWithinTransition(999, "md1 Right Hand of Shaitan", "80")
                        ),
                    new State("80",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntitiesWithinTransition(999, "md1 Left Hand of Shaitan", "81")
                        ),
                    new State("81",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new OrderOnEntry(999, "md1 Right Hand of Shaitan", "14"),
                        new OrderOnEntry(999, "md1 Left Hand of Shaitan", "14"),
                        new TimedTransition("82", 0)
                        ),
                    new State("82",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Shoot(9, 1, index: 0, cooldown: 1000),
                        new EntitiesNotExistsTransition(999, "83", "md1 Right Hand of Shaitan", "md1 Left Hand of Shaitan")
                        ),
                    new State("83",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(0),
                        new ReturnToSpawn(0.8f),
                        new Shoot(9, 1, index: 0, cooldown: 1000),
                        new TransitionFrom("83", "84"),
                        new State("84",
                            new OrderOnEntry(999, "md1 Governor", "1"),
                            new OrderOnEntry(999, "md1 Right Hand spawner", "2"),
                            new OrderOnEntry(999, "md1 Left Hand spawner", "2"),
                            new TossObject("md1 CreepyHead", 5, 65, throwEffect: true),
                            new TossObject("md1 CreepyHead", 5, 90, throwEffect: true),
                            new TossObject("md1 CreepyHead", 5, 105, throwEffect: true),
                            new TimedTransition("85", 0)
                            ),
                        new State("85",
                            new TossObject("md1 Lava Makers", 14, cooldown: 500),
                            new TimedTransition("86", 5500) { SubIndex = 2 }
                            )
                        ),
                    new State("86",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(1),
                        new OrderOnEntry(999, "md1 Governor", "1"),
                        new MoveTo(0.8f, 13.5f, 5),
                        new TimedTransition("88", 2000)
                        ),
                    new State("88",
                        new HealthTransition(0.2f, "94"),
                        new SetAltTexture(0),
                         new ConditionalEffect(ConditionEffectIndex.ArmorBroken),
                        new Shoot(8, 7, 15, 0, predictive: 1, cooldown: 1000),
                        new TimedTransition("89", 5000)
                        ),
                    new State("89",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(1),
                        new MoveTo(0.8f, 8.5f, 5),
                        new TimedTransition("90", 5000)
                        ),
                    new State("90",
                        new HealthTransition(0.2f, "94"),
                        new SetAltTexture(0),
                         new ConditionalEffect(ConditionEffectIndex.ArmorBroken),
                        new Shoot(8, 7, 15, 0, predictive: 1, cooldown: 1000),
                        new TimedTransition("91", 5000)
                        ),
                    new State("91",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(1),
                        new MoveTo(0.8f, 3.5f, 5),
                        new TimedTransition("92", 2000)
                        ),
                    new State("92",
                        new HealthTransition(0.2f, "94"),
                        new SetAltTexture(0),
                         new ConditionalEffect(ConditionEffectIndex.ArmorBroken),
                        new Shoot(8, 7, 15, 0, predictive: 1, cooldown: 1000),
                        new TimedTransition("79", 5000)
                        ),
                    new State("94",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt(true, 0, "THE MAD GOD SHALL KNOW OF THESE TRANSGRESSIONS!"),
                        new TimedTransition("95", 2000)
                        ),
                    new State("95",
                        new Shoot(999, 5, 72, 0, 0, 0),
                        new Suicide()
                        )
                    )
            );
            db.Init("md1 LeftHandSmash",
                new State("base",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("1",
                        new EntitiesNotExistsTransition(999, "2", "md1 Head of Shaitan")
                    ),
                    new State("2",
                        new TimedTransition("3", 2000)
                    ),
                    new State("3",
                        new Spawn("md1 Loot Balloon Shaitan", 1, 1, cooldown: 0),
                        new Suicide(100)
                    )
                )
            );

            db.Init("md1 Governor",
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("x"),
                new State("1",
                    new Spawn("md1 CreepyHands", 2, 1, cooldown: 0),
                    new TimedTransition("x", 1000)
                )
            );

            db.Init("md1 CreepyHands",
                new State("base",
                    new Follow(0.5f, 10, 2),
                    new Wander(0.2f),
                    new Shoot(4, 6, 60, 0, cooldown: 1500)
                    )
            );
            db.Init("md1 Right Smashing Hand",
                new State("base",
                    new Follow(2, 15, 0),
                    new PlayerWithinTransition(2f, "2"),
                    new State("2",
                        new Shoot(999, 10, 36, 0, 0, cooldown: 1000),
                        new Suicide(1000)
                    )
                )
            );
            db.Init("md1 Left Smashing Hand",
                new State("base",
                    new Follow(2, 15, 0),
                    new PlayerWithinTransition(2f, "2"),
                    new State("2",
                        new Shoot(999, 10, 36, 0, 0, cooldown: 1000),
                        new Suicide(1000)
                    )
                )
            );
            db.Init("md1 Lava Makers",
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("1",
                    new GroundTransform("Hot Lava"),
                    new TimedTransition("2", 5000)
                    ),
                new State("2",
                    new GroundTransform("Earth Light"),
                    new Suicide()
                )
            );
            db.Init("md1 CreepyHead",
                new State("base",
                    new Follow(0.2f, 15, 4),
                    new Shoot(6, 2, 10, 0, cooldown: 400)
                    )
            );
            db.Init("md1 Right Hand spawner",
                new State("base",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("x"),
                    new State("1",
                        new Spawn("md1 Right Hand of Shaitan", 1, 1, cooldown: 0),
                        new TimedTransition("x", 1000)
                        ),
                    new State("2",
                        new Spawn("md1 Right Smashing Hand", 1, 1, cooldown: 0),
                        new TimedTransition("3", 2000)
                        ),
                    new State("3",
                        new Spawn("md1 Right Smashing Hand", 1, 1, cooldown: 0),
                        new TimedTransition("x", 2000)
                        )
                    )
            );
            db.Init("md1 Left Hand spawner",
                new State("base",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("x"),
                    new State("1",
                        new Spawn("md1 Left Hand of Shaitan", 1, 1, cooldown: 0),
                        new TimedTransition("x", 1000)
                        ),
                    new State("2",
                        new Spawn("md1 Left Smashing Hand", 1, 1, cooldown: 0),
                        new TimedTransition("3", 2000)
                        ),
                    new State("3",
                        new Spawn("md1 Left Smashing Hand", 1, 1, cooldown: 0),
                        new TimedTransition("x", 2000)
                        )
                 )
            );
            db.Init("md1 Right Hand of Shaitan",
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invincible)
                    ),
                    new State("2",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new ChangeSize(20, 200),
                        new TimedTransition("3", 1000)
                    ),
                    new State("3",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new Taunt(true, cooldown: 0, "Hah. Weakings. This will be slightly enjoyable.", "You are in the presence of demi-god, motral", "What hath the keepers brought Shaitan?", "You disturb an ancient evil...", "You think it wise to use such cheap tricks?", "You make a foolish mistake, mortal."),
                            //Transition forcefully if we are in state three but never after, this way we can continue to tick conditoneffects but not
                            // move into 4, basically this transition only runs if state 3 is the deepest current state
                            new TransitionFrom("3", "4") { SubIndex = 0 },
                            new State("4",
                                new SetAltTexture(1),
                                new Spawn("md1 Right Hand spawner", 1, 1, cooldown: 0),
                                new TimedTransition("5", 400)
                                ),
                            new State("5",
                                new SetAltTexture(2),
                                new TimedTransition("6", 400)
                                ),
                            new State("6",
                                new SetAltTexture(1),
                                new TimedTransition("7", 400)
                                ),
                            new State("7",
                                new SetAltTexture(2),
                                new TimedTransition("8", 400)
                                ),
                            new State("8",
                                new SetAltTexture(1),
                                new TimedTransition("9", 400)
                                ),
                            new State("9",
                                new SetAltTexture(2),
                                new TimedTransition("10", 400)
                                ),
                            new State("10",
                                new SetAltTexture(1),
                                new TimedTransition("11", 400)
                                ),
                            new State("11",
                                new SetAltTexture(2),
                                new TimedTransition("12", 400)
                                ),
                            new State("12",
                                new OrderOnEntry(999, "md1 Left Hand of Shaitan", "3"),
                                new TimedTransition("13", 0) 
                            ),
                            new State("13",
                                new SetAltTexture(0),
                                new EntitiesNotExistsTransition(20f, "14", "md1 Left Hand of Shaitan") { SubIndex = 2 }
                            )
                        ),
                        new State("14",
                            new TimedTransition("15", 0)
                        ),
                        new State("15",
                            new Shoot(999, 6, 25, 0, 45, cooldown: 500),
                            new TimedTransition("16", 4000)
                        ),
                        new State("16",
                            new Follow(0.8f, 99, 1),
                            new Shoot(999, 6, 60, 2, 0, 25, cooldown: 750),
                            new TimedTransition("17", 2000)
                            ),
                        new State("17",
                            new Taunt(true, cooldown: 0, "Hah. Weakings. This will be slightly enjoyable.", "You are in the presence of demi-god, motral", "What hath the keepers brought Shaitan?", "You disturb an ancient evil...", "You think it wise to use such cheap tricks?", "You make a foolish mistake, mortal."),
                            new ReturnToSpawn(0.8f),
                            new Flash(0xFF0000, .2f, 12),
                            new TransitionFrom("17", "18"),
                            new State("18",
                                new SetAltTexture(1),
                                new TimedTransition("19", 400)
                                ),
                            new State("19",
                                new SetAltTexture(2),
                                new TimedTransition("20", 400)
                                ),
                            new State("20",
                                new SetAltTexture(1),
                                new TimedTransition("21", 400)
                                ),
                            new State("21",
                                new SetAltTexture(2),
                                new TimedTransition("22", 400)
                                ),
                            new State("22",
                                new SetAltTexture(0),
                                new TimedTransition("15", 0) { SubIndex = 2 }
                                )
                            )
                    );
            db.Init("md1 Left Hand of Shaitan",
                new State("base",
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invincible)
                        ),
                    new State("2",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new ChangeSize(20, 200)
                    ),
                    new State("3",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new Taunt(true, cooldown: 0, "Yes, little mortals. Meet your doom at the hands of SHAITAN!", "My firey fingers of frustrating flame force foes to fumble, fall, and fail!", "You think it wise to use such cheap tricks?", "You make a foolish mistake, mortal."),
                            new TransitionFrom("3", "4") { SubIndex = 0 },
                            new State("4",
                                new SetAltTexture(1),
                                new Spawn("md1 Left Hand spawner", 1, 1, cooldown: 0),
                                new TimedTransition("5", 400)
                            ),
                            new State("5",
                                new SetAltTexture(2),
                                new TimedTransition("6", 400)
                            ),
                            new State("6",
                                new SetAltTexture(1),
                                new TimedTransition("7", 400)
                            ),
                            new State("7",
                                new SetAltTexture(2),
                                new TimedTransition("8", 400)
                            ),
                            new State("8",
                                new SetAltTexture(1),
                                new TimedTransition("9", 400)
                            ),
                            new State("9",
                                new SetAltTexture(2),
                                new TimedTransition("10", 400)
                            ),
                            new State("10",
                                new SetAltTexture(1),
                                new TimedTransition("11", 400)
                            ),
                            new State("11",
                                new SetAltTexture(2),
                                new TimedTransition("12", 400)
                            ),
                            new State("12",
                                new OrderOnEntry(999, "md1 Right Hand of Shaitan", "14"),
                                new TimedTransition("13", 0) { SubIndex = 2 }
                            )
                        ),
                        new State("13",
                            new SetAltTexture(0),
                            new Shoot(999, 6, 25, 0, 135, cooldown: 500),
                            new TimedTransition("14", 4000)
                            ),
                        new State("14",
                            new Follow(0.8f, 99, 1),
                            new Shoot(999, 6, 60, 2, 0, 25, cooldown: 750),
                            new TimedTransition("15", 2000)
                            ),
                        new State("15",
                            new Taunt(true, 0, "Yes, little mortals. Meet your doom at the hands of SHAITAN!", "My firey fingers of frustrating flame force foes to fumble, fall, and fail!", "You think it wise to use such cheap tricks?", "You make a foolish mistake, mortal."),
                            new ReturnToSpawn(0.8f),
                            new Flash(0xFF0000, .2f, 12),
                            new TransitionFrom("15", "16"),
                            new State("16",
                                new SetAltTexture(1),
                                new TimedTransition("17", 400)
                                ),
                            new State("17",
                                new SetAltTexture(2),
                                new TimedTransition("18", 400)
                                ),
                            new State("18",
                                new SetAltTexture(1),
                                new TimedTransition("19", 400)
                                ),
                            new State("19",
                                new SetAltTexture(2),
                                new TimedTransition("13", 400) { SubIndex = 2 }
                            )
                            )
                        )
                    );
            db.Init("md1 Loot Balloon Shaitan",
                new State("base",
                    new State("Idle",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition("UnsetEffect", 5000)
                    ),
                    new State("UnsetEffect")
                ),
                new Threshold(0.00001f,
                    new ItemLoot("Potion of Attack", 0.9f),
                    new ItemLoot("Potion of Defense", 0.9f),
                    new TierLoot(10, LootType.Weapon, 0.2625f),
                    new TierLoot(11, LootType.Weapon, 0.1625f),
                    new TierLoot(10, LootType.Armor, 0.225f),
                    new TierLoot(11, LootType.Armor, 0.125f),
                    new ItemLoot("Large Flame Cloth", 0.1f),
                    new ItemLoot("Small Flame Cloth", 0.1f),
                    new ItemLoot("(Green) UT Egg", 0.2f, 0.01f),
                    new ItemLoot("(Blue) RT Egg", 0.05f, 0.01f),
                    new ItemLoot("Large Crossbox Cloth", 0.1f),
                    new ItemLoot("Small Crossbox Cloth", 0.1f),
                    new ItemLoot("Large Heavy Chainmail Cloth", 0.1f),
                    new ItemLoot("Book of Shaitan", 0.01f),
                    new ItemLoot("Piercing Fire", 0.01f),
                    new ItemLoot("Master Sword", 0.01f),
                    new ItemLoot("Hylian Shield", 0.01f),
                    new ItemLoot("Blue Tunic", 0.01f),
                    new ItemLoot("Blue Ring", 0.01f),
                    new ItemLoot("Spell of Kinetic Projection", 0.01f),
                    new ItemLoot("Fiery Equipment Crystal", 0.03f, 0.01f),
                    new ItemLoot("Lesser Fiery Equipment Crystal", 1.0f),
                    new ItemLoot("Realm Equipment Crystal", 0.5f, 0.01f),
                    new ItemLoot("Realm Equipment Crystal", 0.1f, 0.1f),
                    new ItemLoot("Realm Equipment Crystal", 0.3f, 0.2f),
                    new ItemLoot("Realm Equipment Crystal", 0.2f, 0.5f)
                    //new ItemLoot("50 Credits", 0.01f),
                    //new ItemLoot("Potion of Critical Chance", 0.02f),
                    //new ItemLoot("Potion of Critical Damage", 0.02f)
                    //new ItemLoot("Small Heavy Chainmail Cloth", 0.1f),
                    //new ItemLoot("Skull of Endless Torment", 0.004f),
                    //new ItemLoot("Staff of Eruption", 0.008f),
                    //new ItemLoot("Ring of the Inferno", 0.008f)//Trap of Everlasting Fire
                )
            );

        }
    }
}
