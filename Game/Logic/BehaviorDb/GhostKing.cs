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
    class GhostKing : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {
            db.Init("Ghost King",
                new State("base",
                    new State("Idle",
                        new BackAndForth(0.3f, 3),
                        new HealthTransition(0.99999f, "EvaluationStart1")
                        ),
                    new State("EvaluationStart1",
                        new Taunt("No corporeal creature can kill my sorrow"),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Prioritize(
                            new StayCloseToSpawn(0.4f, range: 3),
                            new Wander(0.4f)
                            ),
                        new TimedTransition("EvaluationStart2", 2500)
                        ),
                    new State("EvaluationStart2",
                        new Flash(0x0000ff, 0.1f, 60),
                        new ChangeSize(20, 140),
                        new Shoot(10, count: 4, shootAngle: 30, defaultAngle: 0, cooldown: 1000),
                        new Prioritize(
                            new StayCloseToSpawn(0.4f, range: 3),
                            new Wander(0.4f)
                            ),
                        new HealthTransition(0.87f, "EvaluationEnd"),
                        new TimedTransition("EvaluationEnd", 6000)
                        ),
                    new State("EvaluationEnd",
                        new Taunt(0.5f, "Aye, let's be miserable together"),
                        new HealthTransition(0.875f, "HugeMob"),
                        new HealthTransition(0.952f, "Mob"),
                        new HealthTransition(0.985f, "SmallGroup"),
                        new HealthTransition(0.99999f, "Solo")
                        ),
                    new State("HugeMob",
                        new Taunt("What a HUGE MOB!"),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0x00ff00, 0.2f, 300),
                        new TossObject("Small Ghost", range: 4, angle: 0, cooldown: 100000, throwEffect: true),
                        new TossObject("Small Ghost", range: 4, angle: 60, cooldown: 100000, throwEffect: true),
                        new TossObject("Small Ghost", range: 4, angle: 120, cooldown: 100000, throwEffect: true),
                        new TossObject("Large Ghost", range: 4, angle: 180, cooldown: 100000, throwEffect: true),
                        new TossObject("Large Ghost", range: 4, angle: 240, cooldown: 100000, throwEffect: true),
                        new TossObject("Large Ghost", range: 4, angle: 300, cooldown: 100000, throwEffect: true),
                        new TimedTransition("HugeMob2", 30000)
                        ),
                    new State("HugeMob2",
                        new Taunt("I feel almost manic!"),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0x00ff00, 0.2f, 300),
                        new TossObject("Small Ghost", range: 4, angle: 0, cooldown: 100000, throwEffect: true),
                        new TossObject("Small Ghost", range: 4, angle: 60, cooldown: 100000, throwEffect: true),
                        new TossObject("Small Ghost", range: 4, angle: 120, cooldown: 100000, throwEffect: true),
                        new TossObject("Large Ghost", range: 4, angle: 180, cooldown: 100000, throwEffect: true),
                        new TossObject("Large Ghost", range: 4, angle: 240, cooldown: 100000, throwEffect: true),
                        new TossObject("Large Ghost", range: 4, angle: 300, cooldown: 100000, throwEffect: true),
                        new TimedTransition("Company", 30000)
                        ),
                    new State("Mob",
                        new Taunt("There's a MOB of you."),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0x00ff00, 0.2f, 300),
                        new TossObject("Small Ghost", range: 4, angle: 0, cooldown: 100000, throwEffect: true),
                        new TossObject("Small Ghost", range: 4, angle: 60, cooldown: 100000, throwEffect: true),
                        new TossObject("Small Ghost", range: 4, angle: 120, cooldown: 100000, throwEffect: true),
                        new TossObject("Large Ghost", range: 4, angle: 180, cooldown: 100000, throwEffect: true),
                        new TossObject("Large Ghost", range: 4, angle: 240, cooldown: 100000, throwEffect: true),
                        new TossObject("Large Ghost", range: 4, angle: 300, cooldown: 100000, throwEffect: true),
                        new TimedTransition("Company", 30000)
                        ),
                    new State("Company",
                        new Taunt("Misery loves company!"),
                        new TossObject("Ghost Master", range: 4, angle: 0, cooldown: 100000, throwEffect: true),
                        new TossObject("Medium Ghost", range: 4, angle: 60, cooldown: 100000, throwEffect: true),
                        new TossObject("Medium Ghost", range: 4, angle: 120, cooldown: 100000, throwEffect: true),
                        new TossObject("Large Ghost", range: 4, angle: 180, cooldown: 100000, throwEffect: true),
                        new TossObject("Large Ghost", range: 4, angle: 240, cooldown: 100000, throwEffect: true),
                        new TossObject("Large Ghost", range: 4, angle: 300, cooldown: 100000, throwEffect: true),
                        new TimedTransition("Wait", 2000)
                        ),
                    new State("SmallGroup",
                        new Taunt("Such a small party."),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0x00ff00, 0.2f, 300),
                        new TossObject("Small Ghost", range: 4, angle: 0, cooldown: 100000, throwEffect: true),
                        new TossObject("Small Ghost", range: 4, angle: 60, cooldown: 100000, throwEffect: true),
                        new TossObject("Small Ghost", range: 4, angle: 120, cooldown: 100000, throwEffect: true),
                        new TossObject("Medium Ghost", range: 4, angle: 180, cooldown: 100000, throwEffect: true),
                        new TossObject("Medium Ghost", range: 4, angle: 240, cooldown: 100000, throwEffect: true),
                        new TossObject("Medium Ghost", range: 4, angle: 300, cooldown: 100000, throwEffect: true),
                        new TimedTransition("SmallGroup2", 30000)
                        ),
                    new State("SmallGroup2",
                        new Taunt("Misery loves company!"),
                        new TossObject("Ghost Master", range: 4, angle: 0, cooldown: 100000, throwEffect: true),
                        new TossObject("Small Ghost", range: 4, angle: 60, cooldown: 100000, throwEffect: true),
                        new TossObject("Small Ghost", range: 4, angle: 120, cooldown: 100000, throwEffect: true),
                        new TossObject("Medium Ghost", range: 4, angle: 180, cooldown: 100000, throwEffect: true),
                        new TossObject("Medium Ghost", range: 4, angle: 240, cooldown: 100000, throwEffect: true),
                        new TossObject("Medium Ghost", range: 4, angle: 300, cooldown: 100000, throwEffect: true),
                        new TimedTransition("Wait", 2000)
                        ),
                    new State("Solo",
                        new Taunt("Just you?  I guess you don't have any friends to play with."),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0x00ff00, 0.2f, 10),
                        new TossObject("Ghost Master", range: 4, angle: 0, cooldown: 100000, throwEffect: true),
                        new TossObject("Small Ghost", range: 4, angle: 70, cooldown: 100000, throwEffect: true),
                        new TossObject("Small Ghost", range: 4, angle: 140, cooldown: 100000, throwEffect: true),
                        new TossObject("Small Ghost", range: 4, angle: 210, cooldown: 100000, throwEffect: true),
                        new TossObject("Small Ghost", range: 4, angle: 280, cooldown: 100000, throwEffect: true),
                        new TimedTransition("Wait", 1000)
                        ),
                    new State("Wait",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0x00ff00, 0.2f, 10000),
                        new Prioritize(
                            new StayCloseToSpawn(1, range: 8),
                            new Follow(0.6f, range: 2, duration: 2000, cooldown: 2000)
                            ),
                        new Shoot(10, cooldown: 1000),
                        new State("Speak",
                            new Taunt("I cannot be defeated while my loyal subjects sustain me!"),
                            new TimedTransition("Quiet", 1000)
                            ),
                        new State("Quiet",
                            new TimedTransition("Speak", 22000)
                            ),
                        new TimedTransition("Overly_long_combat", 140000)
                        ),
                    new State("Overly_long_combat",
                        new Taunt("You have sapped my energy. A curse on you!"),
                        new Prioritize(
                            new StayCloseToSpawn(1, range: 8),
                            new Follow(0.6f, range: 2, duration: 2000, cooldown: 2000)
                            ),
                        new Shoot(10, cooldown: 1000),
                        new Order(30, "Ghost Master", "Decay"),
                        new Order(30, "Small Ghost", "Decay"),
                        new Order(30, "Medium Ghost", "Decay"),
                        new Order(30, "Large Ghost", "Decay"),
                        new Transform("Actual Ghost King")
                        ),
                    new State("Killed",
                        new Taunt("I feel my flesh again! For the first time in a 1000 years I LIVE!"),
                        new Taunt(0.5f, "Will you release me?"),
                        new Transform("Actual Ghost King")
                        )
                    )
            );
            db.Init("Ghost Master",
                new State("base",
                    new State("Attack1",
                        new State("NewLocation1",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Flash(0xff00ff00, 0.2f, 10),
                            new Prioritize(
                                new StayCloseToSpawn(2, range: 7),
                                new Wander(2)
                                ),
                            new TimedTransition("Att1", 1000)
                            ),
                        new State("Att1",
                            new Shoot(10, count: 4, shootAngle: 90, fixedAngle: 0, cooldown: 400),
                            new TimedTransition("NewLocation1", 9000)
                            ),
                        new HealthTransition(0.99f, "Attack2")
                        ),
                    new State("Attack2",
                        new State("Intro",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Flash(0xff00ff00, 0.2f, 10),
                            new ChangeSize(20, 140),
                            new TimedTransition("NewLocation2", 1000)
                            ),
                        new State("NewLocation2",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Flash(0xff00ff00, 0.2f, 10),
                            new Prioritize(
                                new StayCloseToSpawn(2, range: 7),
                                new Wander(2)
                                ),
                            new TimedTransition("Att2", 1000)
                            ),
                        new State("Att2",
                            new Shoot(10, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 400),
                            new TimedTransition("NewLocation2", 6000)
                            ),
                        new HealthTransition(0.98f, "Attack3")
                        ),
                    new State("Attack3",
                        new State("Intro",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Flash(0xff00ff00, 0.2f, 10),
                            new ChangeSize(20, 180),
                            new TimedTransition("NewLocation3", 1000)
                            ),
                        new State("NewLocation3",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Flash(0xff00ff00, 0.2f, 10),
                            new Prioritize(
                                new StayCloseToSpawn(2, range: 7),
                                new Wander(2)
                                ),
                            new TimedTransition("Att3", 1000)
                            ),
                        new State("Att3",
                            new Shoot(10, count: 4, shootAngle: 90, fixedAngle: 22.5f, cooldown: 400),
                            new TimedTransition("NewLocation3", 3000)
                            ),
                        new HealthTransition(0.94f, "KillKing")
                        ),
                    new State("KillKing",
                        new Taunt("Your secret soul master is dying, Your Majesty"),
                        new Order(30, "Ghost King", "Killed"),
                        new TimedTransition("Suicide", 3000)
                        ),
                    new State("Suicide",
                        new Taunt("I cannot live with my betrayal..."),
                        new Shoot(0, count: 8, shootAngle: 45, fixedAngle: 22.5f),
                        new Decay(0)
                        ),
                    new State("Decay",
                        new Decay(0)
                        )
                    )
            );
            db.Init("Actual Ghost King",
                new State("base",
                    new Taunt(0.9f, "I am still so very alone"),
                    new ChangeSize(-20, 95),
                    new Flash(0xff000000, 0.4f, 100),
                    new BackAndForth(0.5f, distance: 3)
                    ),
                new TierLoot(2, LootType.Ring, 0.25f),
                new TierLoot(3, LootType.Ring, 0.08f),
                new TierLoot(7, LootType.Weapon, 0.3f),
                new TierLoot(8, LootType.Weapon, 0.1f),
                new TierLoot(7, LootType.Armor, 0.3f),
                new TierLoot(8, LootType.Armor, 0.1f),
                new TierLoot(2, LootType.Ability, 0.7f),
                new TierLoot(3, LootType.Ability, 0.16f),
                new TierLoot(4, LootType.Ability, 0.02f),
                new TierLoot(6, LootType.Ring, 0.01f, 0.01f),
                new ItemLoot("Health Potion", 0.7f),
                new ItemLoot("Magic Potion", 0.7f),
                new Threshold(0.001f,
                    new ItemLoot("Mithril Sword", 0.04f, r: new LootDef.RarityModifiedData(1.3f, 2)),
                    new ItemLoot("Realm Equipment Crystal", 0.05f),
                    new ItemLoot("Potion of Mana", 0.1f),
                    new ItemLoot("Potion of Dexterity", 0.6f, min: 2)
                )
            );
            db.Init("Small Ghost",
                new State("base",
                    new TransformOnDeath("Medium Ghost"),
                    new State("NewLocation",
                        new Taunt(0.1f, "Switch!"),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0xff00ff00, 0.2f, 10),
                        new Prioritize(
                            new StayCloseToSpawn(2, range: 7),
                            new Wander(2)
                            ),
                        new TimedTransition("Attack", 1000)
                        ),
                    new State("Attack",
                        new Taunt(0.1f, "Save the King's Soul!"),
                        new Shoot(10, count: 4, shootAngle: 90, fixedAngle: 0, cooldown: 400),
                        new TimedTransition("NewLocation", 9000)
                        ),
                    new State("Decay",
                        new Decay(0)
                        ),
                    new Decay(160000)
                    ),
                new ItemLoot("Magic Potion", 0.02f),
                new ItemLoot("Ring of Magic", 0.02f),
                new ItemLoot("Ring of Attack", 0.02f)
            );
            db.Init("Medium Ghost",
                new State("base",
                    new TransformOnDeath("Large Ghost"),
                    new State("Intro",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0xff00ff00, 0.2f, 10),
                        new ChangeSize(20, 140),
                        new TimedTransition("NewLocation", 1000)
                        ),
                    new State("NewLocation",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0xff00ff00, 0.2f, 10),
                        new Prioritize(
                            new StayCloseToSpawn(2, range: 7),
                            new Wander(2)
                            ),
                        new TimedTransition("Attack", 1000)
                        ),
                    new State("Attack",
                        new Taunt(0.02f, "I come back more powerful than you could ever imagine"),
                        new Shoot(10, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 800),
                        new TimedTransition("NewLocation", 6000)
                        ),
                    new State("Decay",
                        new Decay(0)
                        ),
                    new Decay(160000)
                    ),
                new ItemLoot("Magic Potion", 0.02f),
                new ItemLoot("Ring of Speed", 0.02f),
                new ItemLoot("Ring of Attack", 0.02f),
                new ItemLoot("Iron Quiver", 0.02f)
            );
            db.Init("Large Ghost",
                new State("base",
                    new State("Intro",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0xff00ff00, 0.2f, 10),
                        new ChangeSize(20, 180),
                        new TimedTransition("NewLocation", 1000)
                        ),
                    new State("NewLocation",
                        new Taunt(0.01f,
                            "The Ghost King protects this sacred ground",
                            "The Ghost King gave his heart to the Ghost Master.  He cannot die.",
                            "Only the Secret Ghost Master can kill the King."),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0xff00ff00, 0.2f, 10),
                        new Prioritize(
                            new StayCloseToSpawn(2, range: 7),
                            new Wander(2)
                            ),
                        new TimedTransition("Attack", 1000)
                        ),
                    new State("Attack",
                        new Taunt(0.01f, "The King's wife died here.  For her memory."),
                        new Shoot(10, count: 8, shootAngle: 45, fixedAngle: 22.5f, cooldown: 800),
                        new TimedTransition("NewLocation", 3000),
                        new EntitiesNotExistsTransition(30, "AttackKingGone", "Ghost King")
                        ),
                    new State("AttackKingGone",
                        new Taunt(0.01f, "The King's wife died here.  For her memory."),
                        new Shoot(10, count: 8, shootAngle: 45, fixedAngle: 22.5f, cooldown: 800, cooldownOffset: 800),
                        new TransformOnDeath("Imp", min: 2, max: 3),
                        new TimedTransition("NewLocation", 3000)
                        ),
                    new State("Decay",
                        new Decay(0)
                        ),
                    new Decay(160000)
                    ),
                    new ItemLoot("Magic Potion", 0.02f)
            );
        }
    }
}
