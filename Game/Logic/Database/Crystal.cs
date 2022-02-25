using RotMG.Common;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.Database
{
    class Crystal : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {
            db.Init("Mysterious Crystal",
                new State("base",
                    new State("Waiting",
                        new PlayerWithinTransition(10, "Idle", true)
                        ),
                    new State("Idle",
                        new Taunt(0.1f, "Break the crystal for great rewards..."),
                        new Taunt(0.1f, "Help me..."),
                        new HealthTransition(0.9999f, "Instructions"),
                        new TimedTransition("Idle", 10000)
                        ),
                    new State("Instructions",
                        new Flash(0xffffffff, 2, 100),
                        new Taunt(0.8f, "Fire upon this crystal with all your might for 5 seconds"),
                        new Taunt(0.8f, "If your attacks are weak, the crystal magically heals"),
                        new Taunt(0.8f, "Gather a large group to smash it open"),
                        new HealthTransition(0.998f, "Evaluation")
                        ),
                    new State("Evaluation",
                        new State("Comment1",
                            new Taunt(true, "Sweet treasure awaits for powerful adventurers!"),
                            new Taunt(0.4f, "Yes!  Smash my prison for great rewards!"),
                            new TimedTransition("Comment2", 5000)
                            ),
                        new State("Comment2",
                            new Taunt(0.3f, "If you are not very strong, this could kill you",
                                "If you are not yet powerful, stay away from the Crystal",
                                "New adventurers should stay away",
                                "That's the spirit. Lay your fire upon me.",
                                "So close..."
                                ),
                            new TimedTransition("Comment3", 5000)
                            ),
                        new State("Comment3",
                            new Taunt(0.4f, "I think you need more people...",
                                "Call all your friends to help you break the crystal!"
                                ),
                            new TimedTransition("Comment2", 10000)
                            ),
                        new HealEntity(1, "Crystals", cooldown: 6000),
                        new HealthTransition(0.95f, "StartBreak"),
                        new TimedTransition("Fail", 60000)
                        ),
                    new State("Fail",
                        new Taunt("Perhaps you need a bigger group. Ask others to join you!"),
                        new Flash(0xff000000, 5, 1),
                        new Shoot(10, count: 16, shootAngle: 22.5f, fixedAngle: 0, cooldown: 100000),
                        new HealEntity(1, "Crystals", cooldown: 2000),
                        new TimedTransition("Idle", 5000)
                        ),
                    new State("StartBreak",
                        new Taunt("You cracked the crystal! Soon we shall emerge!"),
                        new ChangeSize(-2, 80),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0xff000000, 2, 10),
                        new TimedTransition("BreakCrystal", 4000)
                        ),
                    new State("BreakCrystal",
                        new Taunt("This your reward! Imagine what evil even Oryx needs to keep locked up!"),
                        new Shoot(0, count: 16, shootAngle: 22.5f, fixedAngle: 0, cooldown: 100000),
                        new Spawn("Crystal Prisoner", maxChildren: 1, initialSpawn: 1, cooldown: 100000, givesNoXp: false),
                        new Decay(0)
                        )
                    )
            );
            db.Init("Crystal Prisoner",
                HPScale.BOSS_HP_SCALE_DEFAULT(),
                new State("base",
                    new Spawn("Crystal Prisoner Steed", maxChildren: 3, initialSpawn: 0, cooldown: 200, givesNoXp: false),
                    new State("pause",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition("start_the_fun", 2000)
                        ),
                    new State("start_the_fun",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt("I'm finally free! Yesss!!!"),
                        new TimedTransition("Daisy_attack", 1500)
                        ),
                    new State("Daisy_attack",
                        new Prioritize(
                            new StayCloseToSpawn(0.3f, range: 7),
                            new Wander(0.3f)
                            ),
                        new State("Quadforce1",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 0, cooldown: 300),
                            new TimedTransition("Quadforce2", 200)
                            ),
                        new State("Quadforce2",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 15, cooldown: 300),
                            new TimedTransition("Quadforce3", 200)
                            ),
                        new State("Quadforce3",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 30, cooldown: 300),
                            new TimedTransition("Quadforce4", 200)
                            ),
                        new State("Quadforce4",
                            new Shoot(10, index: 3, cooldown: 1000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 300),
                            new TimedTransition("Quadforce5", 200)
                            ),
                        new State("Quadforce5",
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 60, cooldown: 300),
                            new TimedTransition("Quadforce6", 200)
                            ),
                        new State("Quadforce6",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 75, cooldown: 300),
                            new TimedTransition("Quadforce7", 200)
                            ),
                        new State("Quadforce7",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 90, cooldown: 300),
                            new TimedTransition("Quadforce8", 200)
                            ),
                        new State("Quadforce8",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Shoot(10, index: 3, cooldown: 1000),
                            new Shoot(0, index: 0, count: 4, shootAngle: 90, fixedAngle: 105, cooldown: 300),
                            new TimedTransition("Quadforce1", 200)
                            ),
                        new HealthTransition(0.3f, "Whoa_nelly"),
                        new TimedTransition("Warning", 18000)
                        ),
                    new State("Warning",
                        new Prioritize(
                            new StayCloseToSpawn(0.5f, range: 7),
                            new Wander(0.5f)
                            ),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0xff00ff00, 0.2f, 15),
                        new Follow(0.4f, acquireRange: 9, range: 2),
                        new TimedTransition("Summon_the_clones", 3000)
                        ),
                    new State("Summon_the_clones",
                        new Prioritize(
                            new StayCloseToSpawn(0.85f, range: 7),
                            new Wander(0.85f)
                            ),
                        new Shoot(10, index: 0, cooldown: 1000),
                        new Spawn("Crystal Prisoner Clone", maxChildren: 4, initialSpawn: 0, cooldown: 200),
                        new TossObject("Crystal Prisoner Clone", range: 5, angle: 0, cooldown: 100000, throwEffect: true),
                        new TossObject("Crystal Prisoner Clone", range: 5, angle: 240, cooldown: 100000, throwEffect: true),
                        new TossObject("Crystal Prisoner Clone", range: 7, angle: 60, cooldown: 100000, throwEffect: true),
                        new TossObject("Crystal Prisoner Clone", range: 7, angle: 300, cooldown: 100000, throwEffect: true),
                        new State("invulnerable_clone",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new TimedTransition("vulnerable_clone", 3000)
                            ),
                        new State("vulnerable_clone",
                            new TimedTransition("invulnerable_clone", 1200)
                            ),
                        new TimedTransition("Warning2", 16000)
                        ),
                    new State("Warning2",
                        new Prioritize(
                            new StayCloseToSpawn(0.85f, range: 7),
                            new Wander(0.85f)
                            ),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0xff00ff00, 0.2f, 25),
                        new TimedTransition("Whoa_nelly", 5000)
                        ),
                    new State("Whoa_nelly",
                        new Prioritize(
                            new StayCloseToSpawn(0.6f, range: 7),
                            new Wander(0.6f)
                            ),
                        new Shoot(10, index: 3, count: 3, shootAngle: 120, cooldown: 900),
                        new Shoot(10, index: 2, count: 3, shootAngle: 15, fixedAngle: 40, cooldown: 1600,
                            cooldownOffset: 0),
                        new Shoot(10, index: 2, count: 3, shootAngle: 15, fixedAngle: 220, cooldown: 1600,
                            cooldownOffset: 0),
                        new Shoot(10, index: 2, count: 3, shootAngle: 15, fixedAngle: 130, cooldown: 1600,
                            cooldownOffset: 800),
                        new Shoot(10, index: 2, count: 3, shootAngle: 15, fixedAngle: 310, cooldown: 1600,
                            cooldownOffset: 800),
                        new State("invulnerable_whoa",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new TimedTransition("vulnerable_whoa", 2600)
                            ),
                        new State("vulnerable_whoa",
                            new TimedTransition("invulnerable_whoa", 1200)
                            ),
                        new TimedTransition("Absolutely_Massive", 10000)
                        ),
                    new State("Absolutely_Massive",
                        new ChangeSize(13, 260),
                        new Prioritize(
                            new StayCloseToSpawn(0.2f, range: 7),
                            new Wander(0.2f)
                            ),
                        new Shoot(10, index: 1, count: 9, shootAngle: 40, fixedAngle: 40, cooldown: 2000,
                            cooldownOffset: 400),
                        new Shoot(10, index: 1, count: 9, shootAngle: 40, fixedAngle: 60, cooldown: 2000,
                            cooldownOffset: 800),
                        new Shoot(10, index: 1, count: 9, shootAngle: 40, fixedAngle: 50, cooldown: 2000,
                            cooldownOffset: 1200),
                        new Shoot(10, index: 1, count: 9, shootAngle: 40, fixedAngle: 70, cooldown: 2000,
                            cooldownOffset: 1600),
                        new State("invulnerable_mass",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new TimedTransition("vulnerable_mass", 2600)
                            ),
                        new State("vulnerable_mass",
                            new TimedTransition("invulnerable_mass", 1000)
                            ),
                        new TimedTransition("Start_over_again", 14000)
                        ),
                    new State("Start_over_again",
                        new ChangeSize(-20, 100),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0xff00ff00, 0.2f, 15),
                        new TimedTransition("Daisy_attack", 3000)
                        )
                    ),
                new Threshold(0.01f,
                    new ItemLoot("Crystal Sword", 0.05f),
                    new ItemLoot("Crystal Wand", 0.05f),
                    new ItemLoot("Crystal Dagger", 0.05f),
                    new ItemLoot("Arcane Shiv", 0.05f),
                    new ItemLoot("Potion of Attack", 0.8f),
                    new ItemLoot("Potion of Defense", 0.8f),
                    new ItemLoot("Potion of Dexterity", 0.8f),
                    new ItemLoot("Realm Equipment Crystal", 0.2f),
                    new ItemLoot("Potion of Vitality", 0.8f),
                    new ItemLoot("Potion of Wisdom", 0.8f),
                    new ItemLoot("Potion of Life", 0.2f),
                    new ItemLoot("Potion of Mana", 0.2f),
                    new TierLoot(10, TierLoot.LootType.Weapon, 1.0f),
                    new TierLoot(10, TierLoot.LootType.Armor, 1.0f),
                    new TierLoot(4, TierLoot.LootType.Ring, 1.0f)
               )
                            );
            db.Init("Crystal Prisoner Clone",
                new State("base",
                    new Prioritize(
                        new StayCloseToSpawn(0.85f, range: 5),
                        new Wander(0.85f)
                        ),
                    new Shoot(10, cooldown: 1400),
                    new State("taunt",
                        new Taunt(0.09f, "I am everywhere and nowhere!"),
                        new TimedTransition("no_taunt", 1000)
                        ),
                    new State("no_taunt",
                        new TimedTransition("taunt", 1000)
                        ),
                    new Decay(17000)
                    )
            );
            db.Init("Crystal Prisoner Steed",
                new State("base",
                    new State("change_position_fast",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Prioritize(
                            new StayCloseToSpawn(3.6f, range: 12),
                            new Wander(3.6f)
                            ),
                        new TimedTransition("attack", 800)
                        ),
                    new State("attack",
                        new Shoot(10, predictive: 0.3f, cooldown: 500),
                        new State("keep_distance",
                            new Prioritize(
                                new StayCloseToSpawn(1, range: 12),
                                new Orbit(1, 9, target: "Crystal Prisoner", radiusVariance: 0)
                                ),
                            new TimedTransition("go_anywhere", 2000)
                            ),
                        new State("go_anywhere",
                            new Prioritize(
                                new StayCloseToSpawn(1, range: 12),
                                new Wander(1)
                                ),
                            new TimedTransition("keep_distance", 2000)
                            )
                        )
                    )
            );
        }
    }
}
