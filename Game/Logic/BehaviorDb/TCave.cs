using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static RotMG.Game.Logic.Loots.TierLoot;

namespace RotMG.Game.Logic.Database
{
    class TCave : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {

            db.Init("Treasure Flame Trap 1.7 Sec",
            new State("base",
                new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                //new RingAttack(1, 1, 0, 0, 0),
                new State("Wait",
                    new SetAltTexture(0, 0),
                    new TimedTransition("Start", 1000)
                    ),
                new State("Start",
                    new Shoot(100, 1, index: 0, cooldown: 200),
                    new SetAltTexture(1, 1),
                    new TimedTransition("Start 2", 140)
                    ),
                new State("Start 2",
                    new Shoot(100, 1, cooldown: 200, index: 0),
                    new SetAltTexture(2, 2),
                    new TimedTransition("Start 3", 140)
                    ),
                new State("Start 3",
                    new Shoot(100, 1, index: 0, cooldown: 200),
                    new SetAltTexture(3, 3),
                    new TimedTransition("Start 4", 140)
                    ),
                new State("Start 4",
                    new Shoot(100, 1, index: 0, cooldown: 200),
                    new SetAltTexture(4, 4),
                    new TimedTransition("Start 5", 140)
                    ),
                new State("Start 5",
                    new Shoot(100, 1, index: 0, cooldown: 200),
                    new SetAltTexture(5, 5),
                    new TimedTransition("Wait", 140)
                    )
                )
            );
        db.Init("Log Trap Clockwise",
            new State("base",
                new Shoot(20, 1, index: 0, cooldown: 200),
                new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                new SetAltTexture(0, 3, 100, true),
                new State("Check Ground",
                    new GroundTransition("Track N End", "Move Down"),
                    new GroundTransition("Track S End", "Move Up"),
                    new GroundTransition("Track W End", "Move Left"),
                    new GroundTransition("Track E End", "Move Right")
                    ),
                new State("Move Down",
                    new GroundTransition("Track N End", "Move Down"),
                    new GroundTransition("Track S End", "Move Up"),
                    new GroundTransition("Track W End", "Move Left"),
                    new GroundTransition("Track E End", "Move Right"),
                    new GroundTransition("Track SW Corner", "Move Right"),
                    new GroundTransition("Track SE Corner", "Move Left"),
                    new MoveLine(0.5f, 90)
                    ),
                new State("Move Up",
                    new GroundTransition("Track N End", "Move Down"),
                    new GroundTransition("Track S End", "Move Up"),
                    new GroundTransition("Track W End", "Move Left"),
                    new GroundTransition("Track E End", "Move Right"),
                    new GroundTransition("Track NW Corner", "Move Right"),
                    new GroundTransition("Track NE Corner", "Move Left"),
                    new MoveLine(0.5f, -90)
                    ),
                new State("Move Left",
                    new GroundTransition("Track N End", "Move Down"),
                    new GroundTransition("Track S End", "Move Up"),
                    new GroundTransition("Track W End", "Move Left"),
                    new GroundTransition("Track E End", "Move Right"),
                    new GroundTransition("Track SW Corner", "Move Up"),
                    new GroundTransition("Track NW Corner", "Move Down"),
                    new MoveLine(0.5f, 0)
                    ),
                new State("Move Right",
                    new GroundTransition("Track N End", "Move Down"),
                    new GroundTransition("Track S End", "Move Up"),
                    new GroundTransition("Track W End", "Move Left"),
                    new GroundTransition("Track E End", "Move Right"),
                    new GroundTransition("Track SE Corner", "Move Up"),
                    new GroundTransition("Track NE Corner", "Move Down"),
                    new MoveLine(0.5f, 180)
                    )
                )
            );
        db.Init("Boulder",
            new State("base",
                new SetAltTexture(0, 3, 100, true),
                new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                new Shoot(20, 1, index: 0, cooldown: 200),
                    new GroundTransition("Tunnel Ground Up", "MoveUp"),
                    new GroundTransition("Tunnel Ground Down", "MoveDown"),
                    new GroundTransition("Tunnel Ground Left", "MoveLeft"),
                    new GroundTransition("Tunnel Ground Right", "MoveRight"),
                new State("MoveUp",
                    new MoveLine(0.8f, -90),
                    new GroundTransition("Tunnel Hole Up", "Suicide")
                    ),
                new State("MoveDown",
                    new MoveLine(0.8f, 90),
                    new GroundTransition("Tunnel Hole Down", "Suicide")
                    ),
                new State("MoveLeft",
                    new MoveLine(0.8f, 0),
                    new GroundTransition("Tunnel Hole Left", "Suicide")
                    ),
                new State("MoveRight",
                    new MoveLine(0.8f, 180),
                    new GroundTransition("Tunnel Hole Right", "Suicide")
                    ),
                new State("Suicide",
                    new Suicide()
                    )
                )
            );
        db.Init("Boulder Spawner",
            new State("base",
                new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                new State("CHeck Player",
                    new PlayerWithinTransition(100, "Start")
                    ),
                new State("Start",
                    new Reproduce("Boulder", 20, 1, cooldown: 200)
                    )
                )
            );
        db.Init("Treasure Pot",
            new Threshold(0.01f,
                new TierLoot(1, LootType.Ability, 0.12f),
                new TierLoot(2, LootType.Ability, 0.08f),
                new TierLoot(1, LootType.Ring, 0.1f),
                new TierLoot(6, LootType.Weapon, 0.12f),
                new ItemLoot("Magic Potion", 0.8f),
                new ItemLoot("Health Potion", 0.8f)
                )
            );
        db.Init("Treasure Plunderer",
            new State("base",
                new State("Player",
                    new PlayerWithinTransition(15, "Start")
                    ),
                new State("Start",
                    new Shoot(7, 1, index: 0, cooldown: 1),
                    new Grenade(3, 75, 7, cooldown: 1000),
                    new Wander(0.4f)
                    )
                ),
            new ItemLoot("Magic Potion", 0.3f),
            new ItemLoot("Health Potion", 0.3f),
            new Threshold(0.01f,
                new ItemLoot("Potion of Defense", 0.03f)
                )
            );
        db.Init("Treasure Robber",
            new State("base",
                new State("Player",
                    new PlayerWithinTransition(10, "Start")
                    ),
                new State("Start",
                    new SetAltTexture(0, 0),
                    new Shoot(15, 3, index: 0, shootAngle: 20, cooldown: 1000),
                    new TimedTransition("Invisible", 2500),
                    new Wander(0.4f)
                    ),
                new State("Invisible",
                    new SetAltTexture(0, 7, 140),
                    new Shoot(15, 3, index: 0, shootAngle: 20, cooldown: 1000),
                    new Wander(0.4f),
                    new TimedTransition("Start", 6000)
                    )
                ),
            new ItemLoot("Magic Potion", 0.3f),
            new ItemLoot("Health Potion", 0.3f)
            );
        db.Init("Treasure Thief",
            new State("base",
                new State("Player",
                    new PlayerWithinTransition(10, "Start")
                    ),
                new State("Start",
                    new Wander(0.6f),
                    new StayBack(0.6f, 6)
                    )
                ),
            new ItemLoot("Magic Potion", 0.3f),
            new ItemLoot("Health Potion", 0.3f),
            new Threshold(0.01f,
                new ItemLoot("Potion of Attack", 0.01f),
                new ItemLoot("Potion of Dexterity", 0.01f)
                )
            );
        db.Init("Treasure Enemy",
            new State("base",
                new State("Player",
                    new PlayerWithinTransition(4, "Start")
                    ),
                new State("Start",
                    new Shoot(20, 2, index: 0, shootAngle: 20, cooldown: 1000),
                    new Shoot(20, 1, index: 1, cooldown: 1000),
                    new NoPlayerWithinTransition(4, "Player"),
                    new Follow(0.4f, 6, 1),
                    new Wander(0.4f)
                    )
                )
            );
        db.Init("Treasure Rat",
            new State("base",
                new State("Player",
                    new PlayerWithinTransition(7, "Start")
                    ),
                new State("Start",
                    new SetAltTexture(0, 1, cooldown: 140),
                    new ChangeSize(20, 200),
                    new Shoot(10, 1, index: 0, cooldown: 1500),
                    new Follow(0.3f, 10, 1),
                    new Wander(0.3f)
                    )
                ),
            new ItemLoot("Health Potion", 0.3f),
            new ItemLoot("Magic Potion", 0.3f)
            );
            db.Init("Golden Oryx Effigy",
                HPScale.BOSS_HP_SCALE_DEFAULT(),
                new State("base",
                    //new HPScale(30),
                    new DropPortalOnDeath(target: "Realm Portal", probability: 1),
                    new OrderOnDeath(20, "Gold Planet", "Die"),
                    new State("Ini",
                        new HealthTransition(threshold: 0.99f, targetState: "Q1 Spawn Minion")
                        ),
                    new State("Q1 Spawn Minion",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TossObject(child: "Gold Planet", range: 7, angle: 0, cooldown: 10000000, throwEffect: true),
                        new TossObject(child: "Gold Planet", range: 7, angle: 45, cooldown: 10000000, throwEffect: true),
                        new TossObject(child: "Gold Planet", range: 7, angle: 90, cooldown: 10000000, throwEffect: true),
                        new TossObject(child: "Gold Planet", range: 7, angle: 135, cooldown: 10000000, throwEffect: true),
                        new TossObject(child: "Gold Planet", range: 7, angle: 180, cooldown: 10000000, throwEffect: true),
                        new TossObject(child: "Gold Planet", range: 7, angle: 225, cooldown: 10000000, throwEffect: true),
                        new TossObject(child: "Gold Planet", range: 7, angle: 270, cooldown: 10000000, throwEffect: true),
                        new TossObject(child: "Gold Planet", range: 7, angle: 315, cooldown: 10000000, throwEffect: true),
                        new TossObject(child: "Treasure Oryx Defender", range: 3, angle: 0, cooldown: 10000000, throwEffect: true),
                        new TossObject(child: "Treasure Oryx Defender", range: 3, angle: 90, cooldown: 10000000, throwEffect: true),
                        new TossObject(child: "Treasure Oryx Defender", range: 3, angle: 180, cooldown: 10000000, throwEffect: true),
                        new TossObject(child: "Treasure Oryx Defender", range: 3, angle: 270, cooldown: 10000000, throwEffect: true),
                        new ChangeSize(rate: -1, target: 60),
                        new TimedTransition(time: 4000, targetState: "Q1 Invulnerable")
                        ),
                    new State("Q1 Invulnerable",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        //order Expand
                        new EntitiesNotExistsTransition(10, "Q1 Vulnerable Transition", "Treasure Oryx Defender"),
                        new TimedTransition("Q1 Vulnerable Transition", 30000)
                        ),
                    new State("Q1 Vulnerable Transition",
                        new State("T1",
                            new SetAltTexture(2),
                            new TimedTransition(time: 50, targetState: "T2")
                            ),
                        new State("T2",
                            new SetAltTexture(minValue: 0, maxValue: 1, cooldown: 100, loop: true)
                            ),
                        new TimedTransition(time: 800, targetState: "Q1 Vulnerable")
                        ),
                    new State("Q1 Vulnerable",
                        new SetAltTexture(1),
                        new Taunt(0.75f, "My protectors!", "My guardians are gone!", "What have you done?", "You destroy my guardians in my house? Blasphemy!"),
                        //order Shrink
                        new HealthTransition(threshold: 0.75f, targetState: "Q2 Invulnerable Transition")
                        ),
                    new State("Q2 Invulnerable Transition",
                        new State("T1_2",
                            new SetAltTexture(2),
                            new TimedTransition(time: 50, targetState: "T2_2")
                            ),
                        new State("T2_2",
                            new SetAltTexture(minValue: 0, maxValue: 1, cooldown: 100, loop: true)
                            ),
                        new TimedTransition(time: 800, targetState: "Q2 Spawn Minion")
                        ),
                    new State("Q2 Spawn Minion",
                        new SetAltTexture(0),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TossObject(child: "Treasure Oryx Defender", range: 3, angle: 0, cooldown: 10000000, throwEffect: true),
                        new TossObject(child: "Treasure Oryx Defender", range: 3, angle: 90, cooldown: 10000000, throwEffect: true),
                        new TossObject(child: "Treasure Oryx Defender", range: 3, angle: 180, cooldown: 10000000, throwEffect: true),
                        new TossObject(child: "Treasure Oryx Defender", range: 3, angle: 270, cooldown: 10000000, throwEffect: true),
                        new ChangeSize(rate: -1, target: 60),
                        new TimedTransition(time: 4000, targetState: "Q2 Invulnerable")
                        ),
                    new State("Q2 Invulnerable",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        //order expand
                        new EntitiesNotExistsTransition(10, "Q2 Vulnerable Transition", "Treasure Oryx Defender"),
                        new TimedTransition("Q2 Vulnerable Transition", 30000)
                        ),
                    new State("Q2 Vulnerable Transition",
                        new State("T1_3",
                            new SetAltTexture(2),
                            new TimedTransition(time: 50, targetState: "T2_3")
                            ),
                        new State("T2_3",
                            new SetAltTexture(minValue: 0, maxValue: 1, cooldown: 100, loop: true)
                            ),
                        new TimedTransition(time: 800, targetState: "Q2 Vulnerable")
                        ),
                    new State("Q2 Vulnerable",
                        new SetAltTexture(1),
                        new Taunt(0.75f, "My protectors are no more!", "You Mongrels are ruining my beautiful treasure!", "You won't leave with your pilfered loot!", "I'm weakened"),
                        //Shrink
                        new HealthTransition(threshold: 0.6f, targetState: "Q3 Vulnerable Transition")
                        ),
                    new State("Q3 Vulnerable Transition",
                        new State("T1_4",
                            new SetAltTexture(2),
                            new TimedTransition(time: 50, targetState: "T2_4")
                            ),
                        new State("T2_4",
                            new SetAltTexture(minValue: 0, maxValue: 1, cooldown: 100, loop: true)
                            ),
                        new TimedTransition(time: 800, targetState: "Q3") { SubIndex = 2 }
                        ),
                    new State("Q3",
                        new SetAltTexture(1),
                        new State("Attack1",
                            new State("CardinalBarrage",
                                new Grenade(radius: 0.5f, damage: 70, range: 0, fixedAngle: 0, cooldown: 1000),
                                new Grenade(radius: 1, damage: 70, range: 3, fixedAngle: 0, cooldown: 1000),
                                new Grenade(radius: 1, damage: 70, range: 3, fixedAngle: 90, cooldown: 1000),
                                new Grenade(radius: 1, damage: 70, range: 3, fixedAngle: 180, cooldown: 1000),
                                new Grenade(radius: 1, damage: 70, range: 3, fixedAngle: 270, cooldown: 1000),
                                new TimedTransition(time: 1500, targetState: "OrdinalBarrage")
                                ),
                            new State("OrdinalBarrage",
                                new Grenade(radius: 0.5f, damage: 70, range: 0, fixedAngle: 0, cooldown: 1000),
                                new Grenade(radius: 1, damage: 70, range: 3, fixedAngle: 45, cooldown: 1000),
                                new Grenade(radius: 1, damage: 70, range: 3, fixedAngle: 135, cooldown: 1000),
                                new Grenade(radius: 1, damage: 70, range: 3, fixedAngle: 225, cooldown: 1000),
                                new Grenade(radius: 1, damage: 70, range: 3, fixedAngle: 315, cooldown: 1000),
                                new TimedTransition(time: 1500, targetState: "CardinalBarrage2")
                                ),
                            new State("CardinalBarrage2",
                                new Grenade(radius: 0.5f, damage: 70, range: 0, fixedAngle: 0, cooldown: 1000),
                                new Grenade(radius: 1, damage: 70, range: 3, fixedAngle: 0, cooldown: 1000),
                                new Grenade(radius: 1, damage: 70, range: 3, fixedAngle: 90, cooldown: 1000),
                                new Grenade(radius: 1, damage: 70, range: 3, fixedAngle: 180, cooldown: 1000),
                                new Grenade(radius: 1, damage: 70, range: 3, fixedAngle: 270, cooldown: 1000),
                                new TimedTransition(time: 1500, targetState: "OrdinalBarrage2")
                                ),
                            new State("OrdinalBarrage2",
                                new Grenade(radius: 0.5f, damage: 70, range: 0, fixedAngle: 0, cooldown: 1000),
                                new Grenade(radius: 1, damage: 70, range: 3, fixedAngle: 45, cooldown: 1000),
                                new Grenade(radius: 1, damage: 70, range: 3, fixedAngle: 135, cooldown: 1000),
                                new Grenade(radius: 1, damage: 70, range: 3, fixedAngle: 225, cooldown: 1000),
                                new Grenade(radius: 1, damage: 70, range: 3, fixedAngle: 315, cooldown: 1000),
                                new TimedTransition(time: 1500, targetState: "CardinalBarrage")
                                ),
                            new TimedTransition(time: 8500, targetState: "Attack2") { SubIndex = 2}
                            ),
                        new State("Attack2",
                            new Flash(color: 0x0000FF, flashPeriod: 0.1f, flashRepeats: 10),
                            new Shoot(range: 0, count: 4, shootAngle: 90, index: 1, defaultAngle: 90, cooldown: 10000000, cooldownOffset: 0),
                            new Shoot(range: 0, count: 4, shootAngle: 90, index: 1, defaultAngle: 90, cooldown: 10000000, cooldownOffset: 200),
                            new Shoot(range: 0, count: 4, shootAngle: 90, index: 1, defaultAngle: 80, cooldown: 10000000, cooldownOffset: 400),
                            new Shoot(range: 0, count: 4, shootAngle: 90, index: 1, defaultAngle: 70, cooldown: 10000000, cooldownOffset: 600),
                            new Shoot(range: 0, count: 4, shootAngle: 90, index: 1, defaultAngle: 60, cooldown: 10000000, cooldownOffset: 800),
                            new Shoot(range: 0, count: 4, shootAngle: 90, index: 1, defaultAngle: 50, cooldown: 10000000, cooldownOffset: 1000),
                            new Shoot(range: 0, count: 4, shootAngle: 90, index: 1, defaultAngle: 40, cooldown: 10000000, cooldownOffset: 1200),
                            new Shoot(range: 0, count: 4, shootAngle: 90, index: 1, defaultAngle: 30, cooldown: 10000000, cooldownOffset: 1400),
                            new Shoot(range: 0, count: 4, shootAngle: 90, index: 1, defaultAngle: 20, cooldown: 10000000, cooldownOffset: 1600),
                            new Shoot(range: 0, count: 4, shootAngle: 90, index: 1, defaultAngle: 10, cooldown: 10000000, cooldownOffset: 1800),
                            new Shoot(range: 0, count: 4, shootAngle: 45, index: 1, defaultAngle: 0, cooldown: 10000000, cooldownOffset: 2200),
                            new Shoot(range: 0, count: 4, shootAngle: 45, index: 1, defaultAngle: 0, cooldown: 10000000, cooldownOffset: 2400),
                            new Shoot(range: 0, count: 4, shootAngle: 90, index: 1, defaultAngle: 0, cooldown: 10000000, cooldownOffset: 2600),
                            new Shoot(range: 0, count: 4, shootAngle: 90, index: 1, defaultAngle: 10, cooldown: 10000000, cooldownOffset: 2800),
                            new Shoot(range: 0, count: 4, shootAngle: 90, index: 1, defaultAngle: 20, cooldown: 10000000, cooldownOffset: 3000),
                            new Shoot(range: 0, count: 4, shootAngle: 90, index: 1, defaultAngle: 30, cooldown: 10000000, cooldownOffset: 3200),
                            new Shoot(range: 0, count: 4, shootAngle: 90, index: 1, defaultAngle: 40, cooldown: 10000000, cooldownOffset: 3400),
                            new Shoot(range: 0, count: 4, shootAngle: 90, index: 1, defaultAngle: 50, cooldown: 10000000, cooldownOffset: 3600),
                            new Shoot(range: 0, count: 4, shootAngle: 90, index: 1, defaultAngle: 60, cooldown: 10000000, cooldownOffset: 3800),
                            new Shoot(range: 0, count: 4, shootAngle: 90, index: 1, defaultAngle: 70, cooldown: 10000000, cooldownOffset: 4000),
                            new Shoot(range: 0, count: 4, shootAngle: 90, index: 1, defaultAngle: 80, cooldown: 10000000, cooldownOffset: 4200),
                            new Shoot(range: 0, count: 4, shootAngle: 90, index: 1, defaultAngle: 90, cooldown: 10000000, cooldownOffset: 4400),
                            new Shoot(range: 0, count: 4, shootAngle: 45, index: 1, defaultAngle: 90, cooldown: 10000000, cooldownOffset: 4600),
                            new Shoot(range: 0, count: 4, shootAngle: 90, index: 1, defaultAngle: 90, cooldown: 10000000, cooldownOffset: 4800),
                            new Shoot(range: 0, count: 4, shootAngle: 90, index: 1, defaultAngle: 90, cooldown: 10000000, cooldownOffset: 5000),
                            new Shoot(range: 0, count: 4, shootAngle: 90, index: 1, defaultAngle: 90, cooldown: 10000000, cooldownOffset: 5200),
                            new Shoot(range: 0, count: 4, shootAngle: 90, index: 1, defaultAngle: 80, cooldown: 10000000, cooldownOffset: 5400),
                            new Shoot(range: 0, count: 4, shootAngle: 90, index: 1, defaultAngle: 70, cooldown: 10000000, cooldownOffset: 5600),
                            new Shoot(range: 0, count: 4, shootAngle: 90, index: 1, defaultAngle: 60, cooldown: 10000000, cooldownOffset: 5800),
                            new Shoot(range: 0, count: 4, shootAngle: 90, index: 1, defaultAngle: 50, cooldown: 10000000, cooldownOffset: 6000),
                            new Shoot(range: 0, count: 4, shootAngle: 90, index: 1, defaultAngle: 40, cooldown: 10000000, cooldownOffset: 6200),
                            new Shoot(range: 0, count: 4, shootAngle: 90, index: 1, defaultAngle: 30, cooldown: 10000000, cooldownOffset: 6400),
                            new Shoot(range: 0, count: 4, shootAngle: 90, index: 1, defaultAngle: 20, cooldown: 10000000, cooldownOffset: 6600),
                            new Shoot(range: 0, count: 4, shootAngle: 90, index: 1, defaultAngle: 10, cooldown: 10000000, cooldownOffset: 6800),
                            new Shoot(range: 0, count: 4, shootAngle: 45, index: 1, defaultAngle: 0, cooldown: 10000000, cooldownOffset: 7000),
                            new TimedTransition(time: 7000, targetState: "Recuperate") { SubIndex = 2 }
                            ),
                        new State("Recuperate",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new HealSelf(cooldown: 1000, amount: 200),
                            new TimedTransition(time: 3000, targetState: "Attack1")
                            )
                        )
                ),
                new Threshold(0.005f,
                   LootTemplates.BasicPots(0.6f).Concat(new []
                   {
                       new TierLoot(9, LootType.Weapon, 0.5f),
                       new TierLoot(9, LootType.Armor, 0.5f),
                       new TierLoot(5, LootType.Ability, 0.2f),
                       new TierLoot(6, LootType.Ring, 0.5f),
                   }).ToArray()
                ),
                new Threshold(0.001f,
                new ItemLoot("Cracked Waraxe", 0.01f),
                new ItemLoot("Hero's Garb", 0.005f),
                new ItemLoot("Gilded Sword", 0.02f, r: new LootDef.RarityModifiedData(1.2f, 2, true)),
                new ItemLoot("Realm Equipment Crystal", 0.1f),
                new ItemLoot("Golden Tome", 0.01f),
                new MobDrop(new LootDef.Builder()
                            .Item("Skin Unlocker")
                            .OverrideJson( new ItemDataJson() { SkinId = "Golden_Wizard" })
                            .Chance(0.001f)
                            .Build()
                ),
                new MobDrop(new LootDef.Builder()
                            .Item("Skin Unlocker")
                            .OverrideJson( new ItemDataJson() { SkinId = "Golden_Archer" })
                            .Chance(0.001f)
                            .Build()
                ),
                new MobDrop(new LootDef.Builder()
                            .Item("Skin Unlocker")
                            .OverrideJson( new ItemDataJson() { SkinId = "Golden_Priest" })
                            .Chance(0.001f)
                            .Build()
                ),
                new MobDrop(new LootDef.Builder()
                            .Item("Skin Unlocker")
                            .OverrideJson( new ItemDataJson() { SkinId = "Golden_Paladin" })
                            .Chance(0.001f)
                            .Build()
                ),
                new MobDrop(new LootDef.Builder()
                            .Item("Skin Unlocker")
                            .OverrideJson( new ItemDataJson() { SkinId = "Golden_Huntress" })
                            .Chance(0.001f)
                            .Build()
                ),
                new MobDrop(new LootDef.Builder()
                            .Item("Skin Unlocker")
                            .OverrideJson( new ItemDataJson() { SkinId = "Golden_Sorcerer" })
                            .Chance(0.001f)
                            .Build()
                ),
                new MobDrop(new LootDef.Builder()
                            .Item("Skin Unlocker")
                            .OverrideJson( new ItemDataJson() { SkinId = "Golden_Ninja" })
                            .Chance(0.001f)
                            .Build()
                ),
                new MobDrop(new LootDef.Builder()
                            .Item("Skin Unlocker")
                            .OverrideJson( new ItemDataJson() { SkinId = "Golden_Necromancer" })
                            .Chance(0.001f)
                            .Build()
                ),
                new MobDrop(new LootDef.Builder()
                            .Item("Skin Unlocker")
                            .OverrideJson( new ItemDataJson() { SkinId = "Golden_Warrior" })
                            .Chance(0.001f)
                            .Build()
                ))
            );
            db.Init("Treasure Oryx Defender",
                new State("base",
                    new Prioritize(
                        new Orbit(speed: 0.5f, radius: 3, acquireRange: 6, target: "Golden Oryx Effigy", speedVariance: 0, radiusVariance: 0)
                        ),
                    new Shoot(range: 0, count: 8, shootAngle: 45, defaultAngle: 0, cooldown: 3000)
                )
            );
            db.Init("Gold Planet",
                new State("base",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Prioritize(
                        new Orbit(speed: 0.5f, radius: 7, acquireRange: 20, target: "Golden Oryx Effigy", speedVariance: 0, radiusVariance: 0)
                        ),
                    new State("GreySpiral",
                        new Shoot(range: 0, count: 2, shootAngle: 180, index: 1, defaultAngle: 90, cooldown: 10000, cooldownOffset: 0),
                        new Shoot(range: 0, count: 2, shootAngle: 180, index: 1, defaultAngle: 90, cooldown: 10000, cooldownOffset: 400),
                        new Shoot(range: 0, count: 2, shootAngle: 180, index: 1, defaultAngle: 80, cooldown: 10000, cooldownOffset: 800),
                        new Shoot(range: 0, count: 2, shootAngle: 180, index: 1, defaultAngle: 70, cooldown: 10000, cooldownOffset: 1200),
                        new Shoot(range: 0, count: 2, shootAngle: 180, index: 0, defaultAngle: 60, cooldown: 10000, cooldownOffset: 1600),
                        new Shoot(range: 0, count: 2, shootAngle: 180, index: 1, defaultAngle: 50, cooldown: 10000, cooldownOffset: 2000),
                        new Shoot(range: 0, count: 2, shootAngle: 180, index: 1, defaultAngle: 40, cooldown: 10000, cooldownOffset: 2400),
                        new Shoot(range: 0, count: 2, shootAngle: 180, index: 1, defaultAngle: 30, cooldown: 10000, cooldownOffset: 2800),
                        new Shoot(range: 0, count: 2, shootAngle: 180, index: 1, defaultAngle: 20, cooldown: 10000, cooldownOffset: 3200),
                        new Shoot(range: 0, count: 2, shootAngle: 180, index: 0, defaultAngle: 10, cooldown: 10000, cooldownOffset: 3600),
                        new Shoot(range: 0, count: 2, shootAngle: 180, index: 1, defaultAngle: 0, cooldown: 10000, cooldownOffset: 4000),
                        new Shoot(range: 0, count: 2, shootAngle: 180, index: 1, defaultAngle: -10, cooldown: 10000, cooldownOffset: 4400),
                        new Shoot(range: 0, count: 2, shootAngle: 180, index: 1, defaultAngle: -20, cooldown: 10000, cooldownOffset: 4800),
                        new Shoot(range: 0, count: 2, shootAngle: 180, index: 1, defaultAngle: -30, cooldown: 10000, cooldownOffset: 5200),
                        new Shoot(range: 0, count: 2, shootAngle: 180, index: 0, defaultAngle: -40, cooldown: 10000, cooldownOffset: 5600),
                        new TimedTransition(time: 5600, targetState: "Reset")
                        ),
                    new State("Reset",
                        new TimedTransition(time: 0, targetState: "GreySpiral")
                        ),
                    new State("Die",
                        new Suicide()
                        )
                )
        );;

        }
    }
}
