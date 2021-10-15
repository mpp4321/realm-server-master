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
                    new Prioritize(
                        new StayBack(2, 12, entity: "Phantom Mage"),
                        new Wander(1f)
                        ),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new ChangeSize(5, 0)
                ),
                new State("reveal",
                    new ConditionalEffect(ConditionEffectIndex.Armored),
                    new Shoot(16, 8, 360 / 8, 1, 0, 8, cooldownOffset: 1000, cooldown: 400),
                    new Shoot(6, 2, 24, cooldownVariance: 500, cooldown: 1500),
                    new ChangeSize(5, 120),
                    new EntitiesNotExistsTransition(12, "hidden", "Realm Reaper")
                )
            );
            db.Init("Scythe Phantom",
                new State("hidden",
                    new Prioritize(
                        new StayBack(2, entity: "Scythe Phantom"),
                        new Wander(0.7f)
                        ),
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
                )),
                new State("orbit",
                    new Shoot(6, 1, predictive: 1f, cooldown: 800),
                    new Shoot(9, 2, 16, index: 1, predictive: 1f, cooldown: 800),
                    new StayBack(1f, 3, "Scythe Phantom"),
                    new Orbit(1.5f, 10, target: "Realm Reaper")
                ),
                new State("orbit1",
                    new Shoot(9, 2, 16, index: 1, predictive: 1f, cooldown: 800),
                    new StayBack(1f, 3, "Scythe Phantom"),
                    new Orbit(1.5f, 7, speedVariance: 0.6f, radiusVariance: 3, target: "Realm Reaper")
                )
                );

            db.Init("Scythe Switch",
                new ConditionalEffect(ConditionEffectIndex.Invisible)
                );

            db.Init("Mage Switch",
                new ConditionalEffect(ConditionEffectIndex.Invisible)
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
                new State("invis",
                    new ChangeSize(100, 0),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new EntitiesNotExistsTransition(99, "tauntWait", "Mage Switch", "Scythe Switch")
                ),
                new State("tauntWait",
                   // new ClearRegionOnDeath(Region.),
                    new PlayerWithinTransition(10, "taunt", true),
                    new ChangeSize(100, 100)
                ),
                new State("taunt",
                    new Taunt("You shouldn't have gone looking for something you didn't want to find!"),
                    new PlayerWithinTransition(16, "base")
                ),
                new State("base",
                    new HealthTransition(0.66f, "charging"),
                    new ConditionalEffect(ConditionEffectIndex.Armored),
                    new Shoot(99, 5, 360 / 5, 0, 0f, 5f, cooldown: 100),
                    new TimedTransition("hide", 4000),
                    new HealthTransition(0.85f, "dash")
                ),
                new State("dash",
                    new ChangeSize(40, 120),
                    new PlayerWithinTransition(3.5f, "sleep"),
                    new HealthTransition(0.66f, "charging"),
                    new Order(12, "Phantom Mage", "reveal"),
                    new TimedTransition("f1", 1800),
                    new Prioritize(
                        new ChargeShoot(
                                new Shoot(99, 4, 360 / 4, 2, cooldown: 50),
                                new Charge(1.5f, 99, 100)
                        ),
                        new Wander(0.4f)
                    )
                ),
                new State("hide",
                    new ConditionalEffect(ConditionEffectIndex.Invisible),
                    new ChangeSize(-20, 0),
                    new Orbit(4, 14, 99, speedVariance: 1.5f, targetPlayers: true),
                    new TimedRandomTransition(1000, "hide", "dash")
                ),
                new State("sleep",
                    new Order(12, "Phantom Mage", "reveal"),
                    new Orbit(0.7f, 4.5f, 99, speedVariance: 0.3f, radiusVariance: 2f, targetPlayers: true),
                    new Shoot(99, 5, 360 / 5, 0, 0f, 27f, cooldown: 500, cooldownVariance: 300),
                    new TimedTransition("hide", 4000)
                ),
                new State("charging",
                    new ConditionalEffect(ConditionEffectIndex.Armored),
                    new Order(99, "Phantom Mage", "hidden"),
                    new Order(99, "Scythe Phantom", "Reveal"),
                    new ReturnToSpawn(1.5f, 0.5f),
                    new TimedTransition("scythe", 5000)
                ),
                new State("scythe",
                    new Orbit(0.4f, 1, target: "Realm Reaper Anchor"),
                    new Order(14, "Scythe Phantom", "orbit"),
                    new Shoot(99, 2, 32, 1, cooldown: 2700),
                    new Shoot(99, 2, 28, 1, cooldownOffset: 100, cooldown: 1700),
                    new Shoot(99, 2, 24, 1, cooldownOffset: 200, cooldown: 1700),
                    new Shoot(99, 2, 20, 1, cooldownOffset: 300, cooldown: 1700),
                    new Shoot(99, 2, 16, 1, cooldownOffset: 400, cooldown: 1700),
                    new Shoot(99, 2, 12, 1, cooldownOffset: 500, cooldown: 1700),
                    new TimedTransition("scythe2", 5990),
                    new HealthTransition(0.33f, "rage")
                ),
                new State("scythe2",
                    new Orbit(0.4f, 1, target: "Realm Reaper Anchor"),
                    new Order(14, "Scythe Phantom", "orbit1"),
                    new TimedTransition("scythe", 8000),
                    new HealthTransition(0.33f, "rage")
                ),
                new State("stand",
                    new Order(8, "Scythe Phantom", "hidden"),
                    new Shoot(16, 8, 360 / 8, index: 2, fixedAngle: 0, rotateAngle: 5),
                    new TimedTransition("scythe", 5000),
                    new HealthTransition(0.33f, "rage")
                ),
                
                new State("rage",
                    new OrderFrom(99, "Phantom Mage", "hidden", "reveal"),
                    new Order(12, "Phantom Mage", "hidden"),
                    new OrderFrom(99, "Scythe Phantom", "hidden", "reveal"),
                    new TimedTransition("roto", 50) { SubIndex = 0 },
                    new State("roto",
                        new Shoot(99, 5, 360 / 5, 0, 0f, -5f, cooldown: 70),
                        new Shoot(99, 5, 360 / 5, 0, 0f, 5f, cooldownOffset: 2700, cooldown: 70),
                        new TimedTransition("range", 3200) { SubIndex = 1 }
                    ),
                    new State("range",
                        new Orbit(1f, 7, 99, speedVariance: 0.6f, radiusVariance: 2, targetPlayers: true),
                        new Shoot(99, 2, 32, 1, cooldown: 2700),
                        new Shoot(99, 2, 32, 1, cooldownOffset: 200, cooldown: 2700),
                        new Shoot(99, 2, 32, 1, cooldownOffset: 400, cooldown: 2700),
                        new Shoot(99, 2, 32, 1, cooldownOffset: 600, cooldown: 2700),
                        new Shoot(99, 2, 32, 1, cooldownOffset: 800, cooldown: 2700),
                        new Shoot(99, 2, 32, 1, cooldownOffset: 1000, cooldown: 2700),
                        new PlayerWithinTransition(3.5f, "crash"),
                        new TimedTransition("crash", 5000) { SubIndex = 1 }
                    ),
                    new State("crash",
                        new ConditionalEffect(ConditionEffectIndex.Cursed),
                        new Charge(1.4f, 99, 4000),
                        new Shoot(99, 2, 180, 3, 0f, 12f, cooldownVariance: 100, cooldown: 300),
                        new TimedRandomTransition(2800, "roto", "range") { SubIndex = 1 }
                )),
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

            db.Init("Realm Reaper Anchor",
                new ConditionalEffect(ConditionEffectIndex.Invincible)
            );

            //need to add flash, clear blackspace, close bones, how tf does subindex work bruh rage phases need to working
        }
    }
}
