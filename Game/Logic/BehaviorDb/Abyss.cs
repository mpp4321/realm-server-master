﻿using RotMG.Common;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RotMG.Game.Logic.Database
{
    class Abyss : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {

            db.Init("Brute of the Abyss", new State("base",
                    new Shoot(5, 2, shootAngle: 30, cooldown: 300),
                    new Prioritize(
                            new Follow(1.5f, 7, 2),
                            new Wander(0.2f)
                        )
                ));

            db.Init("Brute Warrior of the Abyss", new State("base",
                    new Shoot(5, 4, cooldown: 200, fixedAngle: 0f, rotateAngle: 45f/2),
                    new Prioritize(
                            new Charge(0.7, 8, 500),
                            new Wander(0.3f)
                        )
                ));

            db.Init("Demon Mage of the Abyss", new State("base",
                    new Shoot(5, 1, cooldown: 150, fixedAngle: 0f, rotateAngle: 45f/2),
                    new Prioritize(
                            new Wander(0.2f)
                        )
                ));

            db.Init("Demon Warrior of the Abyss", new State("base",
                    new Shoot(5, 2, shootAngle: 30, cooldown: 800),
                    new Grenade(10, 50, 3, cooldown: 250),
                    new Prioritize(
                            new Orbit(1.2f, 4, radiusVariance: 3),
                            new Wander(0.2f)
                        )
                ));

            db.Init("Demon of the Abyss", new State("base",
                    new Shoot(5, 2, shootAngle: 30, cooldown: 1000),
                    new Grenade(10, 75, 5, cooldown: 1200),
                    new Prioritize(
                            new Follow(1.5f, 5, 4),
                            new Wander(0.2f)
                        )
                ));

            db.Init("Imp of the Abyss", new State("base",
                    new Shoot(6, 5, shootAngle: 22, cooldown: 750),
                    new Prioritize(
                            new Follow(1.5f, 5, 4),
                            new Wander(0.2f)
                        )
                ));

            db.Init("Malphas Protector",
                new State("base",
                    new Grenade(8, 65, 3, cooldown: 750),
                    new Prioritize(
                            new StayBack(1, 7)
                        ),
                    new TimedTransition("Slam", 3500)
                ),
                new State("Slam",
                    new Shoot(8, 4, 245, cooldown: 100),
                    new Charge(1.5f, 15, coolDown: 100),
                    new TimedTransition("base", 700)
                    )
                );

            // post boss troom thang
            db.Init("Abyss Idol",

                new State("0",
                    new ConditionalEffect(Common.ConditionEffectIndex.Invincible),
                    new TransitionOnItemNearby(3f, "Potion of Life", "pre a")
                ),

                new State("pre a",
                    new ConditionalEffect(Common.ConditionEffectIndex.Invulnerable),
                    new Flash(0xFFFF0000, 3.0, 9),
                    new TimedTransition("a", 3000)
                ),

                //Shoot AOE ring then toss array of bombs
                new State("a",
                    new HealthTransition(0.5f, "b"),
                    new TransitionFrom("a", "1"),
                    new State("1",
                        new Shoot(0, 5, 14, 0, 45, cooldown: 9999),
                        new Shoot(0, 5, 14, 0, 135, cooldown: 9999),
                        new Shoot(0, 5, 14, 0, 225, cooldown: 9999),
                        new Shoot(0, 5, 14, 0, 315, cooldown: 9999),

                        // Spawn some noobs here to fight players
                        new Spawn("Imp of the Abyss", 2, 1.0, 0.2f, cooldown: 10000),
                        new Spawn("Brute of the Abyss", 1, 1.0, 0.2f, cooldown: 10000),
                        new Spawn("Demon of the Abyss", 1, 1.0, 0.2f, cooldown: 10000),

                #region grenade_array
                        //Grenade array
                        new TimedBehav(500,
                            new QueuedBehav( 
                                new Grenade(1, 100, 2, 0, 1000),
                                new TimedBehav(100, new Grenade(2, 100, 2, 0, 1000)),
                                new TimedBehav(100, new Grenade(3, 100, 2, 0, 1000)),
                                new TimedBehav(100, new Grenade(4, 100, 2, 0, 1000)),
                                new TimedBehav(100, new Grenade(5, 100, 2, 0, 1000))
                            )
                        ),
                        new TimedBehav(500,
                            new QueuedBehav(
                                new Grenade(1, 100, 2, 90, 1000),
                                new TimedBehav(100, new Grenade(2, 100, 2, 90, 1000)),
                                new TimedBehav(100, new Grenade(3, 100, 2, 90, 1000)),
                                new TimedBehav(100, new Grenade(4, 100, 2, 90, 1000)),
                                new TimedBehav(100, new Grenade(5, 100, 2, 90, 1000))
                            )
                        ),
                        new TimedBehav(500,
                            new QueuedBehav(
                                new Grenade(1, 100, 2, 90, 1000),
                                new TimedBehav(100, new Grenade(2, 100, 2, 180, 1000)),
                                new TimedBehav(100, new Grenade(3, 100, 2, 180, 1000)),
                                new TimedBehav(100, new Grenade(4, 100, 2, 180, 1000)),
                                new TimedBehav(100, new Grenade(5, 100, 2, 180, 1000))
                            )
                        ),
                        new TimedBehav(500,
                            new QueuedBehav(
                                new Grenade(1, 100, 2, 90, 1000),
                                new TimedBehav(100, new Grenade(2, 100, 2, 270, 1000)),
                                new TimedBehav(100, new Grenade(3, 100, 2, 270, 1000)),
                                new TimedBehav(100, new Grenade(4, 100, 2, 270, 1000)),
                                new TimedBehav(100, new Grenade(5, 100, 2, 270, 1000))
                            )
                        ),
    #endregion
                        new TimedTransition("2", 2000)
                    ),
                    // Bullet spiral
                    new State("2", 
                       new Shoot(0, 4, 90, 1, null, rotateAngle: 10, cooldown: 200),
                       new TimedBehav(4000,
                           new Flash(0xFFFF0000, 1.0, 3)
                       ),

                       // Outer ring of bombs to deny rangers
                       new Grenade(9, 100, 3, 0, 600),
                       new Grenade(9, 100, 3, 45, 600),
                       new Grenade(9, 100, 3, 90, 600),
                       new Grenade(9, 100, 3, 135, 600),
                       new Grenade(9, 100, 3, 180, 600),
                       new Grenade(9, 100, 3, 225, 600),
                       new Grenade(9, 100, 3, 270, 600),
                       new Grenade(9, 100, 3, 315, 600),

                       new TimedTransition("1", 5000)
                    )
                ),
                new State("b",
                    new TransitionFrom("b", "b1"),
                    new State("b1",
                       new Shoot(0, 4, 90, 1, null, rotateAngle: 10, cooldown: 200),
                       new Flash(0xFFFF0000, 1.0, 3),
                       // Outer ring of bombs to deny rangers
                       new Grenade(9, 100, 3, 0, 600),
                       new Grenade(9, 100, 3, 45, 600),
                       new Grenade(9, 100, 3, 90, 600),
                       new Grenade(9, 100, 3, 135, 600),
                       new Grenade(9, 100, 3, 180, 600),
                       new Grenade(9, 100, 3, 225, 600),
                       new Grenade(9, 100, 3, 270, 600),
                       new Grenade(9, 100, 3, 315, 600),

                        //Grenade array
                        new TimedBehav(500,
                            new QueuedBehav(true,
                                new Grenade(1, 100, 2, 0, 1000),
                                new TimedBehav(40, new Grenade(2, 100, 2, 0, 1000)),
                                new TimedBehav(40, new Grenade(3, 100, 2, 0, 1000)),
                                new TimedBehav(40, new Grenade(4, 100, 2, 0, 1000)),
                                new TimedBehav(40, new Grenade(5, 100, 2, 0, 1000))
                            )
                        ),
                        new TimedBehav(500,
                            new QueuedBehav(true,
                                new Grenade(1, 100, 2, 90, 1000),
                                new TimedBehav(40, new Grenade(2, 100, 2, 90, 1000)),
                                new TimedBehav(40, new Grenade(3, 100, 2, 90, 1000)),
                                new TimedBehav(40, new Grenade(4, 100, 2, 90, 1000)),
                                new TimedBehav(40, new Grenade(5, 100, 2, 90, 1000))
                            )
                        ),
                        new TimedBehav(500,
                            new QueuedBehav(true,
                                new Grenade(1, 100, 2, 90, 1000),
                                new TimedBehav(40, new Grenade(2, 100, 2, 180, 1000)),
                                new TimedBehav(40, new Grenade(3, 100, 2, 180, 1000)),
                                new TimedBehav(40, new Grenade(4, 100, 2, 180, 1000)),
                                new TimedBehav(40, new Grenade(5, 100, 2, 180, 1000))
                            )
                        ),
                        new TimedBehav(500,
                            new QueuedBehav(true,
                                new Grenade(1, 100, 2, 90, 1000),
                                new TimedBehav(40, new Grenade(2, 100, 2, 270, 1000)),
                                new TimedBehav(40, new Grenade(3, 100, 2, 270, 1000)),
                                new TimedBehav(40, new Grenade(4, 100, 2, 270, 1000)),
                                new TimedBehav(40, new Grenade(5, 100, 2, 270, 1000))
                            )
                        )
                    )
                ),
                new ItemLoot("Golden Demonic Metal", 0.06f, 0.01f),
                new ItemLoot("Fiery Equipment Crystal", 0.45f, 0.01f),
                new Threshold(0.01f, LootTemplates.CrystalsDungeonBoss()),
                new ItemLoot("Potion of Vitality", 1.0f, 0.01f),
                new ItemLoot("Potion of Vitality", 1.0f, 0.01f),
                new ItemLoot("Potion of Vitality", 1.0f, 0.01f),
                new ItemLoot("Demon Frog Generator", 0.01f, 0.01f)
            );

            db.Init("Archdemon Malphas",
                HPScale.BOSS_HP_SCALE_DEFAULT(),
                new State("Waiting",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: Settings.MillisecondsPerTick, reapply: (e) =>
                    {
                        return e?.GetNearbyPlayers(14f).Count() == 0;
                    }),
                    new PlayerWithinTransition(14, "base", seeInvis: true)
                ),
                new State("base",
                    new Shoot(10, 1, 5, 3, fixedAngle: 0f, rotateAngle: 12f / 2, cooldown: 150),
                    new Shoot(10, 1, 5, 3, fixedAngle: 0f, rotateAngle: -26f / 2, cooldown: 150),
                    new Shoot(10, 1, 5, 3, fixedAngle: 0f, rotateAngle: -12f / 2, cooldown: 150),
                    new Shoot(10, 1, 5, 3, fixedAngle: 0f, rotateAngle: 26f / 2, cooldown: 150),
                    new Grenade(14f, 100, 2, cooldown: 400),
                    new Prioritize(
                            new Follow(1.5f, 5, 4),
                            new Wander(0.2f)
                        ),
                    new HealthTransition(0.25f, "rage"),
                    new HealthTransition(0.80f, "burst"),
                    new TimedTransition("prepburst", 8000)
                ),
                new State("prepburst", 
                    new ConditionalEffect(Common.ConditionEffectIndex.Invulnerable),
                    new Flash(0xffff0000, 0.3f, 3),
                    new TimedTransition("burst", 1000)
                ),
                new State("burst", 
                        new Shoot(10, 5, 5, fixedAngle: 0f, rotateAngle: 20f / 2, index: 0),
                        new TossObject("Brute Warrior of the Abyss", range: 5, cooldown: 5000),
                        new Spawn("Malphas Protector", 2, cooldown: 2000),
                        new HealthTransition(0.25f, "rage"),
                        new TimedTransition("burst1", 4000)
                ),

                new State("burst1",
                        new Shoot(10, 5, 5, fixedAngle: 0f, rotateAngle: 20f / 2, index: 0),
                        new Spawn("Malphas Protector", 2, cooldown: 1000),
                        new HealthTransition(0.25f, "rage"),
                        new TimedTransition("burst2", 7000)

                ),

                new State("burst2",
                        new Shoot(10, 5, 5, fixedAngle: 0f, rotateAngle: -20f / 2, index: 0),
                        new Spawn("Malphas Protector", 2, cooldown: 1000),
                        new HealthTransition(0.25f, "rage"),
                        new TimedTransition("burst1", 7000)
                ),
                new State("rage",
                    new ChangeSize(50, 150),
                    new Shoot(10, 4, index: 1, shootAngle: 90f, fixedAngle: 0, rotateAngle: 195f, cooldown: 200),
                    new Shoot(10, 2, shootAngle: 30, index: 0, predictive: 1f, cooldown: 500),
                    new ConditionalEffect(Common.ConditionEffectIndex.Damaging),
                    new ConditionalEffect(Common.ConditionEffectIndex.Armored),
                    new Charge(1.8f, 15, coolDown: new wServer.logic.Cooldown(1500, 1000)),
                    new Flash(0xffff0505, 0.5, 2)
                ),
                    new ItemLoot("Demon Frog Generator", 0.02f, 0.01f),
                    new ItemLoot("Demon Blade", 0.05f, 0.01f),
                    new ItemLoot("Potion of Life", 0.05f, 0.01f),
                    new Threshold(0.01f, LootTemplates.CrystalsDungeonBoss()),
                    new Threshold(0.01f, 
                        new ItemLoot("(Green) UT Egg", 0.03f, 0.01f),
                        new ItemLoot("(Blue) RT Egg", 0.005f, 0.01f),
                        new ItemLoot("The War Path", 0.005f),
                        new ItemLoot("Exuberant Heavy Plate", 0.005f),
                        new ItemLoot("The Horned Circlet", 0.005f),
                        new ItemLoot("Abyssal Emblem", 0.005f)
                    ),
                    new Threshold(0.5f, 
                        new ItemLoot("Blazed Bow", 0.005f)
                    ),
                    new Threshold(0.01f,
                        new ItemLoot("Potion of Defense", 0.75f),
                        new ItemLoot("Potion of Vitality", 0.75f),
                        new ItemLoot("Potion of Defense", 0.1f),
                        new ItemLoot("Potion of Vitality", 0.1f)
                    ),
                    new TopDamagersOnly(1, new ItemLoot("Potion of Life", 0.5f, 0.001f))
                );

        }
    }
}
