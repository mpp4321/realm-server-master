using RotMG.Common;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static RotMG.Game.Logic.LootDef;
using static RotMG.Game.Logic.Loots.TierLoot;

namespace RotMG.Game.Logic.Database
{
    class Crypt : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {
            db.Init("Arena Spider",
                new State("base",
                    new Shoot(10, 8, 45, 0, 0f, cooldown: 300),
                    new Prioritize(
                        new StayCloseToSpawn(3f, 2),
                        new Wander(0.2f)
                    ),
                    new PlayerWithinTransition(6, "chase")
                ),
                new State("chase",
                    new Shoot(10, 8, 45, 0, 0f, cooldown: 300),
                    new Charge(1.5f, 20, 500),
                    new TimedTransition("chase0", 1500)
                ),
                new State("chase0",
                    new Shoot(10, 8, 45, 0, 0f, cooldown: 300),
                    new Charge(2f, 20, 250),
                    new TimedTransition("chase1", 1000)
                ),
                new State("chase1",
                    new Shoot(10, 8, 45, 0, 0f, cooldown: 300),
                    new Charge(2.5f, 20, 100),
                    new TimedTransition("return", 400)
                ),
                new State("return",
                    new StayBack(1.5f, 8),
                    new Wander(4),
                    new TimedTransition("base", 1500)
                ));

            db.Init("Arena Pumpkin King",
                    new State("base",
                        new Shoot(10, 8, 45, 1, 0f, 10f, cooldown: 200),
                        new Prioritize(
                            new Follow(0.8f, range: 1),
                            new Wander(0.4f)
                            ),
                        new Prioritize(
                            new Charge(1.4f, coolDown: 800, pred: e => e.HasConditionEffect(Common.ConditionEffectIndex.Paralyzed)),
                            new Charge(1.4f, coolDown: 800, pred: e => e.HasConditionEffect(Common.ConditionEffectIndex.Slowed)),
                            new Shoot(5, count: 3, shootAngle: 10)
                        ),
                        new TimedTransition("somewhere", 7000
                    )),
                    new State("somewhere",
                        new StayBack(2f),
                        new Wander(4),
                        new TossObject("Arena Spider", 6, 360 / 6, 100000),
                        new TossObject("Arena Spider", 6, 360 / 6 * 2, 100000),
                        new TossObject("Arena Spider", 6, 360 / 6 * 3, 100000),
                        new TossObject("Arena Spider", 6, 360 / 6 * 4, 100000),
                        new TossObject("Arena Spider", 6, 360 / 6 * 5, 100000),
                        new TossObject("Arena Spider", 6, 360 / 6 * 6, 100000),
                        new TimedTransition("base", 2500)
                    ),
                    new Threshold(0.01f,
                        LootTemplates.BasicPots(0.01f).Concat(
                            new MobDrop[] {
                                new TierLoot(8, TierLoot.LootType.Weapon, 1f, r: new RarityModifiedData(1f, 2)),
                                new Threshold(0.01f,
                                    new ItemLoot("Reaper's Wit", 0.01f, r: new RarityModifiedData(1.2f)),
                                    new ItemLoot("Crypt Keeper's Crossbow", 0.01f, r: new RarityModifiedData(1.2f)),
                                    new ItemLoot("Amulet of Ancient Power", 0.005f),
                                    new ItemLoot("Potion of Dexterity", 0.9f)
                                )
                            }
                        ).ToArray()
                    )
                );

            db.Init("Phantom Mage",
                new State("hidden",
                    // new Wander(1f),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new ChangeSize(5, 0)
                ),
                new State("reveal",
                    new ConditionalEffect(ConditionEffectIndex.Armored),
                    new Shoot(16, 8, 360 / 8, 1, 0, 8, cooldown: 400),
                    new Shoot(6, 2, 24, cooldownVariance: 500, cooldown: 1500),
                    new ChangeSize(5, 120),
                    new EntitiesNotExistsTransition(12, "hidden", "Realm Reaper")
                )
            );
            db.Init("Scythe Phantom",
                new State("hidden",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new ChangeSize(5, 0)
                ),
                new State("reveal",
                    new Wander(0.4f),
                    new Prioritize(
                        new Follow(0.8f, 5, 2, 700, 300)
                    ),
                    new ChangeSize(5, 120),
                    new TimedTransition("revealed", 1500)
                ),
                new State("revealed",
                    new Wander(0.4f),
                    new Shoot(6, 1, predictive: 1f, cooldown: 800),
                    new Shoot(6, 2, 16, index: 1, predictive: 1f, cooldown: 800),
                    new Prioritize(
                        new Follow(0.8f, 5, 2, 700, 300)
                    ))

                );

            db.Init("Septavius the Ghost God",
                    new State("warning",
                        new Flash(0xff0033ff, .3f, 3),
                        new TimedTransition("fire", 1000)
                    ),
                    new State("fire",
                        new Shoot(10, 4, 10, 0, cooldown: 200),
                        new Shoot(10, 2, 40, 1, cooldown: 400),
                        new Prioritize(false,
                            new Prioritize(true,
                                new Charge(2.0, pred: e => e.HasConditionEffect(Common.ConditionEffectIndex.Hexed)),
                                new Shoot(10, 8, 45, 2, 0f)
                            ),
                            new Follow(0.8f, range: 1),
                            new Wander(0.4f)
                        ),
                        new TimedTransition("warning", 1200)
                    ),
                    new Threshold(0.01f,
                        LootTemplates.BasicPots(0.3f).Concat(
                            new MobDrop[] {
                                new TierLoot(8, TierLoot.LootType.Weapon, 1f, r: new RarityModifiedData(1f, 2))
                         
                            }
                        ).ToArray()
                    )
                );

            db.Init("Realm Reaper",
              //  new State("invis",
               //     new ChangeSize(100, 0),
               //     new ConditionalEffect(ConditionEffectIndex.Invincible)
             //   ),
                new State("taunt",
                    new Taunt("You shouldn't have gone looking for something you didn't want to find!"),
                    new PlayerWithinTransition(16, "base")
                ),
                new State("base",
                    new OrderFrom(99, "Phantom Mage", "reveal", "hidden"),
                    new Order(99, "Phantom Mage", "hidden"),
                    new ConditionalEffect(ConditionEffectIndex.Armored),
                    new Shoot(99, 5, 360 / 5, 0, 0f, 15f, cooldown: 100),
                    new TimedTransition("f", 4000),
                    new HealthTransition(0.85f, "charging1")
                ),
                new State("f",
                    new Prioritize(
                        new ChargeShoot(
                                new Shoot(99, 4, 360 / 4, 2, cooldown: 300),
                                new Charge(1.5f, 99, 100)
                        ),
                        new Wander(0.4f)
                    ),
                    new OrderFrom(8, "Phantom Mage", "hidden", "Reveal"),
                    new Order(8, "Phantom Mage","Reveal"),
                    new TimedTransition("base", 5500)
                    //new HealthTransition(0.85f, "charging1")
                ),
                new State("charging1",
                    new OrderFrom(16, "Phantom Mage", "reveal", "hidden"),
                    new Shoot(16, 8, 360 / 8, index: 2, fixedAngle: 0, rotateAngle: 5),
                    new Flash(500573, 1000, 4),
                    new TimedTransition("charge", 4500)
                ),
                new State("charge",
                    new Order(8, "Scythe Phantom", "reveal"),
                    new Charge(1.2f, 16, 2000),
                    new TimedTransition("stand", 5990)
                ),
                new State("stand",
                    new Shoot(16, 8, 360 / 8, index: 2, fixedAngle: 0, rotateAngle: 5),
                    new TimedTransition("charging", 5000)
                ),
                new Threshold(0.001f,
                    new TierLoot(10, LootType.Weapon, 0.1f),
                    new TierLoot(11, LootType.Weapon, 0.05f),
                    new TierLoot(10, LootType.Armor, 0.1f),
                    new TierLoot(11, LootType.Armor, 0.05f),
                    new TierLoot(5, LootType.Ability, 0.2f),
                    new ItemLoot("Potion of Attack", 1f),
                    new ItemLoot("Potion of Defense", 1f),
                    new ItemLoot("Potion of Mana", 1f)
                )
            );


        }
    }
}
