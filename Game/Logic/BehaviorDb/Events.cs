using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;
using RotMG.Utils;
using RoTMG.Game.Logic.Transitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static RotMG.Game.Logic.LootDef;
using static RotMG.Game.Logic.Loots.TierLoot;

namespace RotMG.Game.Logic.Database
{
    class Events : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {

            db.EveryInit = new MobDrop[]
            {
                new MobDrop(new LootDef.Builder().Item("Bag of Potion Holding").Chance(0.05f).Threshold(0.001f).OverrideJson(
                        new ItemDataJson()
                        {
                            StoredItems = new List<int>(),
                            AllowedItems = new List<int> {
                                Resources.Id2Item["Potion of Attack"].Type,
                                Resources.Id2Item["Potion of Dexterity"].Type,
                                Resources.Id2Item["Potion of Speed"].Type,
                                Resources.Id2Item["Potion of Life"].Type,
                                Resources.Id2Item["Potion of Mana"].Type,
                                Resources.Id2Item["Potion of Vitality"].Type,
                                Resources.Id2Item["Potion of Wisdom"].Type,
                                Resources.Id2Item["Potion of Defense"].Type
                            }
                        }).Build())
            };

            db.Init("LH Spectral Sentry",
                HPScale.BOSS_HP_SCALE_DEFAULT(),
                new State("0", new PlayerWithinTransition(10f, "1", true)),
                new State("1",
                    new Follow(0.3f, 10f, 3),
                    new Charge(1f, 12f, distance: 1f, coolDown: 1000),
                    new Shoot(10f, 8, 10, 0, cooldown: 3000),
                    new Shoot(10f, 8, 10, 1, cooldown: 3000, cooldownOffset: 1000),
                    new Shoot(10f, 8, 10, 2, cooldown: 3000, cooldownOffset: 2000),

                    new Shoot(10f, 8, 360 / 8, 0, cooldown: 3000, cooldownOffset: 500),
                    new Shoot(10f, 8, 360 / 8, 1, cooldown: 3000, cooldownOffset: 1500),
                    new Shoot(10f, 8, 360 / 8, 2, cooldown: 3000, cooldownOffset: 2500),

                    new TimedTransition("2", 5000)
                ),
                new State("2",
                    new Flash(0xFF0000FF, 0.5, 3),
                    new TimedTransition("3", 1500)
                ),
                new State("3",
                    new Charge(1f, 12f, distance: 1f, coolDown: 100),

                    new Grenade(16f, 5, 3, cooldown: 1000, effect: ConditionEffectIndex.Slowed, effectDuration: 1000, color: 0xFF00FF00, speed: 500),
                    new Grenade(0, 200, 3, 0f, cooldown: 200, effect: ConditionEffectIndex.Sick, effectDuration: 1000, color: 0xFF0000FF, speed: 100),

                    new Shoot(10f, 4, 360 / 4, 0, cooldown: 500, fixedAngle: 0, rotateAngle: 45),
                    new Shoot(10f, 4, 360 / 4, 1, cooldown: 500, fixedAngle: 15, rotateAngle: 45),
                    new Shoot(10f, 4, 360 / 4, 2, cooldown: 500, fixedAngle: 30, rotateAngle: 45),

                    new NoPlayerWithinTransition(8f, "3a") { SubIndex = 0 },
                    new State("3a",
                        new Charge(0.8f, 12f, coolDown: 15000),
                        new PlayerWithinTransition(8f, "3", true) { SubIndex = 2 }
                    ),
                    new TimedTransition("1", 3000)
                ),
                new Threshold(0.001f, LootTemplates.CrystalsHardRegular())
            );

            db.Init("Hermit God",
                HPScale.BOSS_HP_SCALE_DEFAULT(),
                new FunctorOnRegion(Region.Enemy1, onDeath: (tuple) =>
                {
                    var host = tuple.Item1;
                    var point = tuple.Item2;
                    host.Parent.UpdateTile(point.X, point.Y, Resources.Id2Tile["Shallow Water"].Type);
                }),
                new SpawnOnDeath("Oyster King", 0.3333f),
                new State("base",
                    new TransferDamageOnDeath("Hermit God Drop"),
                    new OrderOnDeath(20, "Hermit God Tentacle Spawner", "Die"),
                    new OrderOnDeath(20, "Hermit God Drop", "Die"),
                    new TransitionFrom("base", "spawn tentacle"),
                    new State("Spawn Tentacle",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new SetAltTexture(2),
                        new Order(20, "Hermit God Tentacle Spawner", "Tentacle"),
                        new EntitiesWithinTransition( 20, "Hermit God Tentacle", "Sleep")
                    ),
                    new State("Sleep",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Order(20, "Hermit God Tentacle Spawner", "Minions"),
                        new TimedTransition("Waiting", 1000)
                        ),
                    new State("Waiting",
                        new SetAltTexture(3),
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new EntitiesNotExistsTransition(20, "Wake", "Hermit God Tentacle")
                        ),
                    new State("Wake",
                        new SetAltTexture(2),
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new TossObject("Hermit Minion", 10, angle: 0, throwEffect: true),
                        new TossObject("Hermit Minion", 10, angle: 45, throwEffect: true),
                        new TossObject("Hermit Minion", 10, angle: 90, throwEffect: true),
                        new TossObject("Hermit Minion", 10, angle: 135, throwEffect: true),
                        new TossObject("Hermit Minion", 10, angle: 180, throwEffect: true),
                        new TossObject("Hermit Minion", 10, angle: 225, throwEffect: true),
                        new TossObject("Hermit Minion", 10, angle: 270, throwEffect: true),
                        new TossObject("Hermit Minion", 10, angle: 315, throwEffect: true),
                        new TimedTransition("Spawn Whirlpool", 100)
                        ),
                    new State("Spawn Whirlpool",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Order(20, "Hermit God Tentacle Spawner", "Whirlpool"),
                        new EntitiesWithinTransition(20, "Whirlpool", "Attack1")
                        ),
                    new State("Attack1",
                        new SetAltTexture(0),
                        new Prioritize(
                            new Wander(0.3f),
                            new StayCloseToSpawn(0.5f, 5)
                            ),
                        new Shoot(20, count: 3, shootAngle: 5, cooldown: 300),
                        new TimedTransition("Attack2", 6000)
                        ),
                    new State("Attack2",
                        new Prioritize(
                            new Wander(0.3f),
                            new StayCloseToSpawn(0.5f, 5)
                            ),
                        new Order(20, "Whirlpool", "Die"),
                        new Shoot(20, count: 1, defaultAngle: 0, fixedAngle: 0, rotateAngle: 45, index: 1,
                            cooldown: 1000),
                        new Shoot(20, count: 1, defaultAngle: 0, fixedAngle: 180, rotateAngle: 45, index: 1,
                            cooldown: 1000),
                        new TimedTransition("Spawn Tentacle", 6000)
                        )
                    )
            );
            db.Init("Hermit God Drop",
                new State("base",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new DropPortalOnDeath("Ocean Trench Portal", 1),

                    new State("Waiting"),
                    new State("Die",
                        new Suicide()
                    )
                ),
                new Threshold(0.03f,
                    new TierLoot(11, LootType.Weapon, 0.2f),
                    new TierLoot(11, LootType.Armor, 0.2f),
                    new TierLoot(10, LootType.Weapon, 0.35f),
                    new TierLoot(10, LootType.Armor, 0.35f),
                    new ItemLoot("Potion of Wisdom", 0.25f),
                    new ItemLoot("Potion of Dexterity", 0.15f),
                    new ItemLoot("Potion of Defense", 0.25f),
                    new ItemLoot("Potion of Wisdom", 0.15f),
                    new ItemLoot("Realm Equipment Crystal", 0.3f),
                    new ItemLoot("Helm of the Juggernaut", 0.0005f)
                )
            );

            db.Init("Skull Shrine",
                HPScale.BOSS_HP_SCALE_DEFAULT(),
                new State("base",
                    new Shoot(30, 4, 9, cooldown: 1500, predictive: 1f), // add prediction after fixing it...
                    new Reproduce("Blue Flaming Skull", 20, 50, cooldown: 100),
                    new Reproduce("Red Flaming Skull", 20, 5, cooldown: 1000)
                    ),
                new Threshold(0.03f,
                    new ItemLoot("Orb of Conflict", 0.005f),
                    new ItemLoot("Thousand Suns Spell", 0.005f)
                ),
                new Threshold(0.005f,
                    new TierLoot(5, LootType.Ability, 0.2f, r: new RarityModifiedData(1.5f, 2)),
                    new TierLoot(4, LootType.Ring, 0.2f, r: new RarityModifiedData(1.5f, 2)),
                    new TierLoot(4, LootType.Ability, 0.5f, r: new RarityModifiedData(1.5f, 2)),
                    new TierLoot(3, LootType.Ring, 0.5f, r: new RarityModifiedData(1.5f, 2)),
                    new ItemLoot("Potion of Dexterity", 1f),
                    new ItemLoot("Potion of Attack", 1f)
                ),
                new Threshold(0.001f, LootTemplates.CrystalsRealmBoss())
            );

            db.Init("Grand Sphinx",
                HPScale.BOSS_HP_SCALE_DEFAULT(),
                new State("base",
                    new DropPortalOnDeath("Tomb of the Ancients Portal", 1.0f),
                    new State("Spawned",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Reproduce("Horrid Reaper", 30, 4, cooldown: 100),
                        new TimedTransition("Attack1", 500)
                        ),
                    new State("Attack1",
                        new Prioritize(
                            new Wander(0.5f)
                            ),
                        new Shoot(12, count: 1, cooldown: 800),
                        new Shoot(12, count: 3, shootAngle: 10, cooldown: 1000),
                        new Shoot(12, count: 1, shootAngle: 130, cooldown: 1000),
                        new Shoot(12, count: 1, shootAngle: 230, cooldown: 1000),
                        new TimedTransition("TransAttack2", 6000)
                        ),
                    new State("TransAttack2",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Wander(0.5f),
                        new Flash(0x00FF0C, .25f, 8),
                        new Taunt(0.99f, "You hide behind rocks like cowards but you cannot hide from this!"),
                        new TimedTransition("Attack2", 2000)
                        ),
                    new State("Attack2",
                        new Prioritize(
                            new Wander(0.5f)
                            ),
                        new Shoot(0, count: 8, shootAngle: 10, fixedAngle: 0, rotateAngle: 70, cooldown: 2000,
                            index: 1),
                        new Shoot(0, count: 8, shootAngle: 10, fixedAngle: 180, rotateAngle: 70, cooldown: 2000,
                            index: 1),
                        new TimedTransition("TransAttack3", 6200)
                        ),
                    new State("TransAttack3",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Wander(0.5f),
                        new Flash(0x00FF0C, .25f, 8),
                        new TimedTransition("Attack3", 2000)
                        ),
                    new State("Attack3",
                        new Prioritize(
                            new Wander(0.5f)
                            ),
                        new Shoot(20, count: 9, fixedAngle: 360 / 9, index: 2, cooldown: 2300),
                        new TimedTransition("TransAttack1", 6000),
                        new State("Shoot1",
                            new Shoot(20, count: 2, shootAngle: 4, index: 2, cooldown: 700),
                            new TimedRandomTransition(1000,
                                "Shoot1",
                                "Shoot2"
                                )
                            ),
                        new State("Shoot2",
                            new Shoot(20, count: 8, shootAngle: 5, index: 2, cooldown: 1100),
                            new TimedRandomTransition(1000,
                                "Shoot1",
                                "Shoot2"
                                )
                            )
                        ),
                    new State("TransAttack1",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Wander(0.5f),
                        new Flash(0x00FF0C, .25f, 8),
                        new TimedTransition("Attack1", 2000),
                        new HealthTransition(0.15f, "Order")
                        ),
                    new State("Order",
                        new Wander(0.5f),
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Order(30, "Horrid Reaper", "Die"),
                        new TimedTransition("Attack1", 1900)
                        )
                    ),
                new Threshold(0.005f,
                        LootTemplates.BasicPots()
                    ),
                //new Threshold(0.01f,
                //    new ItemLoot("Sovereign Garments", 0.007f),
                //    new ItemLoot("Ancient Pendant", 0.007f)
                //    ),
                new Threshold(0.03f,
                    new TierLoot(5, LootType.Ability, 0.2f),
                    new TierLoot(4, LootType.Ring, 0.2f),
                    new TierLoot(4, LootType.Ability, 0.35f),
                    new TierLoot(3, LootType.Ring, 0.35f),
                    new ItemLoot("Realm Equipment Crystal", 0.25f),
                    new ItemLoot("Helm of the Juggernaut", 0.01f),
                    new ItemLoot("Rune of the Juggernaut", 0.02f),
                    new ItemLoot("Spear of the Storm", 0.008f)
                ),
                new Threshold(0.001f, LootTemplates.CrystalsRealmBoss())
            );


            db.Init("Cube God",
                HPScale.BOSS_HP_SCALE_DEFAULT(),
                new State("base",
                    new Wander(.3f),
                    new Shoot(30, 9, 10, 0, predictive: .5f, cooldown: 750),
                    new Shoot(30, 4, 10, 1, predictive: .5f, cooldown: 1500),
                    new Reproduce("Cube Overseer", 30, 10, cooldown: 1500)
                    ),
                  new Threshold(.005f,
                     LootTemplates.BasicPots()
                     ),
                  new Threshold(0.01f,
                    new TierLoot(4, LootType.Ability, 0.2f),
                    new TierLoot(3, LootType.Ring, 0.2f),
                    new TierLoot(10, LootType.Armor, 0.2f),
                    new TierLoot(10, LootType.Weapon, 0.2f),
                    new ItemLoot("Realm Equipment Crystal", 0.03f),
                    new ItemLoot("Dirk of Cronus", 0.01f)
                  ),
                  new Threshold(0.001f, LootTemplates.CrystalsRealmBoss())
                );

            db.Init("Lord of the Lost Lands",
                HPScale.BOSS_HP_SCALE_DEFAULT(),
                new State("base",
                    new PlayerWithinTransition(6, "start", true)
                ),
                    new State("Start",
                        new SetAltTexture(0),
                        new Prioritize(
                            new Wander(0.3f)
                            ),
                        new Shoot(0, count: 7, shootAngle: 10, fixedAngle: 180, cooldown: 2000),
                        new Shoot(0, count: 7, shootAngle: 10, fixedAngle: 0, cooldown: 2000),
                        new TimedTransition("Spawning Guardian", 6000),
                        new HealthTransition(0.025f, "dead"),
                        new NoPlayerWithinTransition(16, "waiting")
                        ),
                    new State("Spawning Guardian",
                        new TossObject("Guardian of the Lost Lands", 7, 45, cooldown: 9999),
                        new TossObject("Guardian of the Lost Lands", 7, 45 + 90, cooldown: 9999),
                        new TossObject("Guardian of the Lost Lands", 7, 45 + (90 * 2), cooldown: 9999),
                        new TossObject("Guardian of the Lost Lands", 7, 45 + (90 * 3), cooldown: 9999),
                        new TimedTransition("Attack", 3100),
                        new HealthTransition(0.025f, "dead"),
                        new NoPlayerWithinTransition(16, "waiting")
                        ),
                    new State("Attack",
                        new SetAltTexture(0),
                        new Wander(0.8f),
                        new TimedRandomTransition(10, "Attack1.1f", "Attack1.2f")
                    ),
                    new State("Attack1.1f",
                        new TimedTransition("Gathering", 10000),
                        new Wander(0.6f),
                        new HealthTransition(0.025f, "dead"),
                        new Shoot(12, count: 7, shootAngle: 10, cooldown: 2000),
                        new Shoot(12, count: 7, shootAngle: 190, cooldown: 2000),
                        new TimedTransition("Attack1.2f", 2000),
                            new State("Attack1.2f",
                                new Shoot(0, count: 7, shootAngle: 10, fixedAngle: 180, cooldown: 3000),
                                new Shoot(0, count: 7, shootAngle: 10, fixedAngle: 0, cooldown: 3000),
                                new TimedTransition("Attack1.0f", 2000),
                        new NoPlayerWithinTransition(16, "waiting")
                    )),
                    new State("Gathering",
                        new Taunt(0.99f, "Gathering power!"),
                        new SetAltTexture(3),
                        new TimedTransition("Gathering1.0f", 2000),
                        new HealthTransition(0.025f, "dead"),
                        new NoPlayerWithinTransition(16, "waiting")
                    ),
                    new State("Gathering1.0f",
                        new TimedTransition("Protection", 5000),
                        new TimedRandomTransition(10, "Gathering1.1f", "Gathering1.2f"),
                        new HealthTransition(0.025f, "dead"),
                        new NoPlayerWithinTransition(16, "waiting")
                    ),
                    new State("Gathering1.1f",
                        new Shoot(30, 4, 360 / 4, 1, 0, 16, cooldown: 350),
                        new TimedTransition("Gathering1.2f", 2000),
                        new TimedTransition("Protection", 8000),
                            new State("Gathering1.2f",
                                new Shoot(30, 4, 360 / 4, 1, 0, -16, cooldown: 350),
                                new TimedTransition("Gathering1.1f", 2000),
                        new HealthTransition(0.025f, "dead"),
                        new NoPlayerWithinTransition(16, "waiting")
                    )),
                    new State("Protection",
                        new SetAltTexture(0),
                        new TossObject("Protection Crystal", 4, angle: 0, cooldown: 4000, throwEffect: true),
                        new TossObject("Protection Crystal", 4, angle: 45, cooldown: 5000, throwEffect: true),
                        new TossObject("Protection Crystal", 4, angle: 90, cooldown: 4000, throwEffect: true),
                        new TossObject("Protection Crystal", 4, angle: 135, cooldown: 5000, throwEffect: true),
                        new TossObject("Protection Crystal", 4, angle: 180, cooldown: 4000, throwEffect: true),
                        new TossObject("Protection Crystal", 4, angle: 225, cooldown: 5000, throwEffect: true),
                        new TossObject("Protection Crystal", 4, angle: 270, cooldown: 4000, throwEffect: true),
                        new TossObject("Protection Crystal", 4, angle: 315, cooldown: 5000, throwEffect: true),
                        new TimedTransition("Start", 8001),
                        new HealthTransition(0.025f, "dead"),
                        new NoPlayerWithinTransition(16, "waiting")
                    ),
                    new State("Waiting",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(1),
                        new Taunt("I knew you'd come back!"),
                        new PlayerWithinTransition(6, "start", true)
                        ),
                    new State("Dead",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(3),
                        new Taunt(0.99f, "NOOOO!!!!!!"),
                        new Flash(0xFF0000, .1f, 1000),
                        new TimedTransition("Suicide", 2000)
                        ),
                    new State("Suicide",
                        new ConditionalEffect(ConditionEffectIndex.StunImmune, true),
                        new Shoot(0, 8, fixedAngle: 360 / 8, index: 1),
                        new Suicide()
                        ),
                new Threshold(0.03f,
                        new TierLoot(11, LootType.Armor, 0.85f),
                        new TierLoot(5, LootType.Ring, 0.2f),
                        new TierLoot(4, LootType.Ring, 0.8f),
                        new TierLoot(10, LootType.Armor, 0.85f),
                        new ItemLoot("Shield of Ogmur", 0.03f),
                        new ItemLoot("Soul of the Lost Land", 0.02f),
                        new ItemLoot("Skysplitter Sword", 0.1f, r: new RarityModifiedData(1f, 2, true)),
                        new ItemLoot("Realm Equipment Crystal", 1f),
                        new ItemLoot("Realm Equipment Crystal", 0.25f),
                        new ItemLoot("Mithril Shield", 0.1f, r: new RarityModifiedData(1f, 2, true)),
                        new ItemLoot("Rune of Vampirism", 0.025f)
                    ),
                new Threshold(0.005f,
                    LootTemplates.BasicPots()
                ),
                new Threshold(0.001f, LootTemplates.CrystalsRealmBoss())
            );

            db.Init("Pentaract Tower Corpse",
                new State("base",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("Waiting",
                        new TimedTransition("Spawn", 15000),
                        new EntitiesNotExistsTransition(50, "Die", "Pentaract Tower")
                        ),
                    new State("Spawn",
                        new Transform("Pentaract Tower")
                        ),
                    new State("Die",
                        new Suicide()
                    )
                ),
                new Threshold(0.0001f,
                    new ItemLoot("Seal of Blasphemous Prayer", 0.001f)
                    ),
                new Threshold(0.0001f,
                   new TierLoot(8, LootType.Weapon, .3f),
                    new TierLoot(9, LootType.Weapon, .3f),
                    new TierLoot(10, LootType.Weapon, .2f),
                    new TierLoot(11, LootType.Weapon, .2f),
                    new TierLoot(4, LootType.Ability, .2f),
                    new TierLoot(5, LootType.Ability, .2f),
                    new TierLoot(8, LootType.Armor, .2f),
                    new TierLoot(9, LootType.Armor, .15f),
                    new TierLoot(10, LootType.Armor, .10f),
                    new TierLoot(11, LootType.Armor, .2f),
                    new TierLoot(12, LootType.Armor, .24f),
                    new TierLoot(3, LootType.Ring, .25f),
                    new TierLoot(4, LootType.Ring, .27f),
                    new TierLoot(5, LootType.Ring, .23f),
                    new ItemLoot("Potion of Defense", .1f),
                    new ItemLoot("Potion of Attack", .1f),
                    new ItemLoot("Potion of Vitality", .1f),
                    new ItemLoot("Potion of Wisdom", .1f),
                    new ItemLoot("Potion of Speed", .1f),
                    new ItemLoot("Realm Equipment Crystal", .05f),
                    new ItemLoot("Jackpot", 0.001f)
                ),
                new Threshold(0.001f, LootTemplates.CrystalsRealmBoss())
            );

            db.Init("Ghost Ship",
                HPScale.BOSS_HP_SCALE_DEFAULT(),
                new State("waiting",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new PlayerWithinTransition(8, "rotate", true)
                ),
                new State("rotate",
                    new Prioritize(
                        new StayCloseToSpawn(0.3f),
                        new Wander(0.4f)
                        ),
                    new Shoot(8, 4, 18, 0, 0, 22, cooldown: 350),
                    new Shoot(8, 3, 18, 0, 180, 22, cooldown: 350),
                    new TimedTransition("rotateWait", 2500, true),
                    new HealthTransition(0.8f, "angy")
                ),
                new State("rotateBack",
                    new Prioritize(
                        new StayCloseToSpawn(0.3f),
                        new Wander(0.4f)
                    ),
                    new Shoot(8, 4, 18, 0, 0, -22, cooldown: 350),
                    new Shoot(8, 3, 18, 0, 180, -22, cooldown: 350),
                    new TimedTransition("rotateWait1", 2500, true),
                    new HealthTransition(0.8f, "angy")
                ),
                new State("rotateWait",
                    new Prioritize(
                        new StayCloseToSpawn(0.3f),
                        new Wander(0.4f)
                    ),
                    new TimedTransition("rotateBack", 1500),
                    new HealthTransition(0.8f, "angy")
                ),
                new State("rotateWait1",
                    new Prioritize(
                        new StayCloseToSpawn(0.3f),
                        new Wander(0.4f)
                    ),
                    new TimedTransition("rotate", 1500),
                    new HealthTransition(0.8f, "angy")
                ),
                new State("angy",
                    new Prioritize(
                        new StayCloseToSpawn(0.4f),
                        new Wander(0.6f)
                    ),
                    new Shoot(8, 1, index: 1, predictive: 0.5f, cooldownVariance: 500, cooldown: 1200),
                    new Shoot(8, 2, 28, 0, predictive: 0.3f, cooldownVariance: 200, cooldown: 800),
                    // I cannot for my life make this transition to anything but its 1st string what da dogi doin
                    new RandomTransition(
                        new TimedTransition("slam", 3000),
                        new TimedTransition("run", 3000),
                        new TimedTransition("roto", 3000),
                        new TimedTransition("roto1", 3000)
                    )
                ),
                new State("slam",
                    new Prioritize(
                        new Charge(2.8f, 8, 1750),
                        new StayCloseToSpawn(0.5f, 5)
                    ),
                    new Shoot(4, 5, 360 / 6, 0, cooldown: 200),
                    new TimedTransition("run", 2500)
                ),
                new State("run",
                    new Prioritize( 
                        new StayBack(0.9f, 4),
                        new StayCloseToSpawn(0.7f),
                        new Wander(0.3f)
                    ),
                    new Shoot(12, 1, index: 1, predictive: 0.7f, cooldownVariance: 600, cooldown: 1400),
                    new Shoot(8, 4, 28, 0, predictive: 0.3f, cooldownVariance: 350, cooldown: 900),
                    new TimedTransition("angy", 6000)
                ),
                new State("roto",
                    new Prioritize(
                        new Orbit(1, 4.5f, target: "Ghost Ship Anchor", speedVariance: 0.3f),
                        new Wander(0.2f)
                        ),
                    new ShootAt("Ghost Ship Anchor", 12, 6, 0, cooldownVariance: 150, cooldown: 900),
                    new Shoot(12, 1, index: 1, predictive: 0.7f, cooldownVariance: 600, cooldown: 1800),
                    new TimedTransition("angy", 8000)
                ),
                new State("roto1",
                    new Prioritize(
                        new Orbit(0.8f, 4.5f, target: "Ghost Ship Anchor", speedVariance: 0.3f, orbitClockwise: true),
                        new Wander(0.2f)
                        ),
                    new ShootAt("Ghost Ship Anchor", 12, 6, 0, cooldownVariance: 150, cooldown: 900),
                    new Shoot(12, 1, index: 1, predictive: 0.7f, cooldownVariance: 600, cooldown: 1800),
                    new TimedTransition("angy", 8000)
                ),
                new Threshold(0.001f,
                    new TierLoot(8, LootType.Weapon, .3f),
                    new TierLoot(9, LootType.Weapon, .3f),
                    new TierLoot(10, LootType.Weapon, .2f),
                    new TierLoot(11, LootType.Weapon, .2f),
                    new TierLoot(4, LootType.Ability, .2f),
                    new TierLoot(5, LootType.Ability, .2f),
                    new TierLoot(8, LootType.Armor, .2f),
                    new TierLoot(9, LootType.Armor, .15f),
                    new TierLoot(10, LootType.Armor, .10f),
                    new TierLoot(11, LootType.Armor, .2f),
                    new TierLoot(12, LootType.Armor, .24f),
                    new TierLoot(3, LootType.Ring, .25f),
                    new TierLoot(4, LootType.Ring, .27f),
                    new TierLoot(5, LootType.Ring, .23f),
                    new ItemLoot("Potion of Defense", .4f),
                    new ItemLoot("Mithril Studded Bow", 0.5f, r: new RarityModifiedData(1.5f, 2)),
                    new ItemLoot("Potion of Attack", .4f),
                    new ItemLoot("Potion of Vitality", .4f),
                    new ItemLoot("Potion of Wisdom", .4f),
                    new ItemLoot("Potion of Speed", .4f),
                    new ItemLoot("Realm Equipment Crystal", .2f),
                    new ItemLoot("Trap of the Vile Spirit", 0.01f, r: new RarityModifiedData(1.1f, 1, true))
                ),
                new Threshold(0.001f, LootTemplates.CrystalsRealmBoss())
            );

            db.EveryInit = new ItemLoot[0];

            //below needs their sprites indexed
            db.Init("Vengeful Spirit",
                new State("base",
                    new ChangeSize(10, 100),
                    new Prioritize(
                        new StayCloseToSpawn(0.8f, 2),
                        new Wander(0.25f)
                        ),
                    new Shoot(4, 3, 14, cooldown: 800),
                    new NoPlayerWithinTransition(6, "hide")
                ),
                new State("hide",
                    new Prioritize(
                        new StayCloseToSpawn(0.8f, 2),
                        new Wander(0.25f)
                        ),
                    new ChangeSize(10, 0),
                    new PlayerWithinTransition(6, "base")
                )
            );

            db.Init("Tempest Cloud",
                new State("wait",
                    new PlayerWithinTransition(6, "texture")
                ),
                new State("Texture",
                    new ChangeSize(20, 140),
                    new SetAltTexture(1, 9, cooldown: 100),
                    new TimedTransition("attack", 1100)
                ),
                new State("attack",
                    new Shoot(5, 5, 360 / 5, cooldownVariance: 250, cooldown: 1000),
                    new NoPlayerWithinTransition(6, "untexture")
                ),
                new State("untexture",
                    new ChangeSize(10, 0),
                    new TimedTransition("wait", 500)
                ),
                new ConditionalEffect(ConditionEffectIndex.Invincible, true)
            );

            db.Init("Hermit Minion",
                new State("base",
                    new Prioritize(
                        new Follow(0.6f, 4, 1),
                        new Orbit(0.6f, 10, 15, "Hermit God", speedVariance: .2f, radiusVariance: 1.5f),
                        new Wander(0.6f)
                        ),
                    new Shoot(6, count: 3, shootAngle: 10, cooldown: 1000),
                    new Shoot(6, count: 2, shootAngle: 20, index: 1, cooldown: 2600, predictive: 0.8f)
                    ),
                new ItemLoot("Health Potion", 0.1f),
                new ItemLoot("Magic Potion", 0.1f)
            );

            db.Init("Whirlpool",
                new State("base",
                    new State("Attack",
                        new EntitiesNotExistsTransition( 100, "Die", "Hermit God"),
                        new Prioritize(
                            new Orbit(0.3f, 6, 10, "Hermit God")
                            ),
                        new Shoot(0, 1, fixedAngle: 0, rotateAngle: 30, cooldown: 400)
                        ),
                    new State("Die",
                        new Shoot(0, 8, fixedAngle: 360 / 8),
                        new Suicide()
                        )
                    )
            );

            db.Init("Hermit God Tentacle",
                new State("base",
                    new Prioritize(
                        new Follow(0.6f, 4, 1),
                        new Orbit(0.6f, 6, 15, "Hermit God", speedVariance: .2f, radiusVariance: .5f),
                        new Orbit(0.6f, 6, 15, "Ancient Hermit God", speedVariance: .2f, radiusVariance: .5f)
                        ),
                    new Shoot(3, count: 8, shootAngle: 360 / 8, cooldown: 500)
                    )
            );

            db.Init("Hermit God Tentacle Spawner",
                new State("base",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new State("Waiting Order"),
                    new State("Tentacle",
                        new Reproduce("Hermit God Tentacle", 3, 1, cooldown: 2000),
                        new EntitiesWithinTransition(1, "Hermit God Tentacle", "Waiting Order")
                        ),
                    new State("Whirlpool",
                        new Reproduce("Whirlpool", 3, 1, cooldown: 2000),
                        new EntitiesWithinTransition(1, "Whirlpool", "Waiting Order")
                        ),
                    new State("Minions",
                        new Reproduce("Hermit Minion", 40, 20, cooldown: 1000),
                        new TimedTransition("Waiting Order", 2000)
                        ),
                    new State("Die",
                        new Suicide()
                        )
                    )
            );

            db.Init("Pentaract Eye",
                new State("base",
                    new Prioritize(
                        new Swirl(2, 8, 20, true),
                        new Protect(2, "Pentaract Tower", 20, 6, 4)
                        ),
                    new Shoot(9, 1, cooldown: 1000)
                    )
            );
            db.Init("Pentaract Tower",
                new State("base",
                    new Spawn("Pentaract Eye", 5, cooldown: 5000, givesNoXp: false),
                    new Grenade(4, 100, 8, cooldown: 5000),
                    new TransformOnDeath("Pentaract Tower Corpse"),
                    new TransferDamageOnDeath("Pentaract"),
                    new TransferDamageOnDeath("Pentaract Tower Corpse")
                    )
            );
            db.Init("Pentaract",
                new State("base",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("Waiting",
                        new EntitiesNotExistsTransition(50, "Die", "Pentaract Tower")
                        ),
                    new State("Die",
                        new Suicide()
                        )
                    )
            );
            db.Init("Knight of the Lost Lands",
                new State("base",
                    new Prioritize(
                        new Orbit(0.8f, 5, 14, "Lord of the Lost Lands", 0.4f, 3, true)
                        ),
                    new Shoot(13, 1, cooldown: 700)
                    ),
                new ItemLoot("Health Potion", 0.1f),
                new ItemLoot("Magic Potion", 0.1f)
            );
            db.Init("Protection Crystal",
                new State("base",
                    new Prioritize(
                        new Orbit(0.5f, 3, 10, "Lord of the Lost Lands", radiusVariance: 1.5f)
                        ),
                    new Shoot(8, count: 3, shootAngle: 7, cooldown: 500),
                    new Decay(10000)
                    )
            );
            db.Init("Guardian of the Lost Lands",
                new Decay(12000),
                new State("base",
                    new State("Full",
                        new Spawn("Knight of the Lost Lands", 2, 1, cooldown: 4000),
                        new Prioritize(
                            new Orbit(0.6f, 3, 10, "Lord of the Lost Lands", 0.2f, 2f)
                            ),
                        new Shoot(10, count: 8, fixedAngle: 360 / 8, cooldown: 3000, index: 1),
                        new Shoot(10, count: 5, shootAngle: 10, cooldown: 1500),
                        new HealthTransition(0.25f, "Low")
                        ),
                    new State("Low",
                        new Prioritize(
                            new StayBack(0.6f, 5),
                            new Wander(0.2f)
                            ),
                        new Shoot(10, count: 8, fixedAngle: 360 / 8, cooldown: 3000, index: 1),
                        new Shoot(10, count: 5, shootAngle: 10, cooldown: 1500)
                        )
                    ),
                new ItemLoot("Health Potion", 0.1f),
                new ItemLoot("Magic Potion", 0.1f)
            );
            db.Init("Cube Overseer",
                new State("base",
                    new Prioritize(
                        new Orbit(.375f, 10, 30, "Cube God", .075f, 5),
                        new Wander(.375f)
                        ),
                    new Reproduce("Cube Defender", 12, 10, cooldown: 1000),
                    new Reproduce("Cube Blaster", 30, 10, cooldown: 1000),
                    new Shoot(10, 4, 10, 0, cooldown: 750),
                    new Shoot(10, index: 1, cooldown: 1500)
                    ),
                new Threshold(.01f,
                    new ItemLoot("Fire Sword", .05f)
                    )
            );
            db.Init("Cube Defender",
                new State("base",
                    new Prioritize(
                        new Orbit(1.05f, 5, 15, "Cube Overseer", .15f, 3),
                        new Wander(1.05f)
                        ),
                    new Shoot(10, cooldown: 500)
                    )
            );
            db.Init("Cube Blaster",
                new State("base",
                    new State("Orbit",
                        new Prioritize(
                            new Orbit(1.05f, 7.5f, 40, "Cube Overseer", .15f, 3),
                            new Wander(1.05f)
                            ),
                        new EntitiesNotExistsTransition(10, "Follow", "Cube Overseer")
                        ),
                    new State("Follow",
                        new Prioritize(
                            new Follow(.75f, 10, 1, 5000),
                            new Wander(1.05f)
                            ),
                        new EntitiesNotExistsTransition(10, "Orbit","Cube Defender"),
                        new TimedTransition("Orbit", 5000)
                        ),
                    new Shoot(10, 2, 10, 1, predictive: 1, cooldown: 500),
                    new Shoot(10, predictive: 1, cooldown: 1500)
                    )
            );
            db.Init("Horrid Reaper",
                new State("base",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("Move",
                        new Prioritize(
                            new StayCloseToSpawn(3, 10),
                            new Wander(3)
                            ),
                        new EntitiesNotExistsTransition( 50, "Die", "Grand Sphinx"), //Just to be sure
                        new TimedRandomTransition(2000, "Attack")
                        ),
                    new State("Attack",
                        new Shoot(0, count: 6, fixedAngle: 360 / 6, cooldown: 700),
                        new PlayerWithinTransition(2, "Follow"),
                        new TimedRandomTransition(5000, "Move")
                        ),
                    new State("Follow",
                        new Prioritize(
                            new Follow(0.7f, 10, 3)
                            ),
                        new Shoot(7, count: 1, cooldown: 700),
                        new TimedRandomTransition(5000, "Move")
                        ),
                    new State("Die",
                        new Taunt(0.99f, "OOaoaoAaAoaAAOOAoaaoooaa!!!"),
                        new Decay(1000)
                        )
                    )
            );

            db.Init("Red Flaming Skull",
                new State("base",
                    new State("Orbit Skull Shrine",
                        new Prioritize(
                            new Protect(.2f, "Skull Shrine", 30, 15, 15),
                            new Wander(.2f)
                            ),
                            new EntitiesNotExistsTransition( 40, "Wander", "Skull Shrine")
                        ),
                    new State("Wander",
                        new Wander(.2f)
                        ),
                    new Shoot(12, 2, 10, cooldown: 750)
                    )
            );
            db.Init("Blue Flaming Skull",
                new State("base",
                    new State("Orbit Skull Shrine",
                        new Orbit(1.0f, 15, 40, "Skull Shrine", .6f, 10),
                        new EntitiesNotExistsTransition(40, "Wander", "Skull Shrine")
                        ),
                    new State("Wander",
                        new Wander(1.0f)
                        ),
                    new Shoot(12, 2, 10, cooldown: 750)
                    )
            );

            db.Init("Boshy",
                new State("base", 
                    new Shoot(16, 5, shootAngle: 10, cooldownOffset: 100),
                    new Shoot(16, 5, shootAngle: 10, index: 1),
                    new Wander(0.6f)
                ),
                new Threshold(0.001f,
                    LootTemplates.BasicPots(0.5f).Concat(
                        new MobDrop[] {
                            new TierLoot(11, LootType.Weapon, 1.0f, r: new RarityModifiedData(1.3f, 2)),
                            new TierLoot(11, LootType.Armor, 1.0f, r: new RarityModifiedData(1.3f, 2)),
                            new TierLoot(5, LootType.Ring, 1.0f, r: new RarityModifiedData(1.3f, 2)),
                        }
                    ).ToArray()
                ),
                new Threshold(0.001f, LootTemplates.CrystalsHardRegular())
            );

            db.Init("Super Snail", 
                new Charge(1f, 99, coolDown: 300, distance: 0.01f),
                new Shoot(10, 1, cooldown: 1000),
                new Shoot(10, 1, angleOffset: 180.0f, cooldown: 1000),
                new Shoot(10, 1, angleOffset: 90.0f, cooldown: 1000),
                new Shoot(10, 1, angleOffset: 270.0f, cooldown: 1000),

                new Shoot(10, 4, 90f, 1, rotateAngle: 45f, cooldown: 500)
            );

            db.Init("NM Green Dragon God Hardmode",
                new ChangeSize(75, 300),
                new State("1",
                    new Wander(0.05f),
                    new Shoot(0, 5, 40, cooldown: 3000),
                    new Shoot(5, 3, 20, index: 3, cooldown: 1000),
                    new HealthTransition(0.9f, "2")
                ),
                new State("2",
                    new Wander(0.05f),
                    new Shoot(0, 5, 40, cooldown: 1500),
                    new Shoot(5, 3, 20, index: 3, cooldown: 500),
                    new Shoot(0, 8, 45 / 2, 1, null, null, 180),
                    new Shoot(0, 1, null, 2, null, 45 / 2, 180, cooldown: 200),
                    new TossObject("Dragon God Gas Cloud", 10, cooldown: 800, tossInvis: true, minAngle: 0f, maxAngle: 360f, minRange: -10f, maxRange: 10f),
                    new HealthTransition(0.5f, "3")
                ),
                new State("3",
                    new Wander(0.1f),
                    new TossObject("Dragon God Gas Cloud", 10, cooldown: 400, tossInvis: true, minAngle: 0f, maxAngle: 360f, minRange: 0f, maxRange: 10f),
                    new TimedTransition("inner1", 3000) { SubIndex = 0 },
                    new TimedTransition("inner2", 9000) { SubIndex = 0 },
                    new Shoot(0, 5, 40, cooldown: 1500),
                    new Shoot(5, 3, 20, index: 3, cooldown: 500),
                    new Shoot(0, 8, 45 / 2, 1, null, null, 180),
                    new Shoot(0, 1, null, 2, null, 45 / 2, 180, cooldown: 200),
                    new QueuedBehav(true,
                        new Taunt(cooldown: 0, "Fear me!"),
                        new CooldownBehav(3000, new Flash(0xff, 0.1, 3)),
                        new CooldownBehav(1000, new Charge(1f, 10))
                    ),
                    new HealthTransition(0.3f, "4")
                ),
                new State("4",
                    new Wander(0.15f),
                    new ConditionalEffect(ConditionEffectIndex.Armored),
                    new TossObject("Dragon God Gas Cloud", 10, cooldown: 200, tossInvis: true, minAngle: 0f, maxAngle: 360f, minRange: 0f, maxRange: 10f),
                    new TimedTransition("inner1", 3000) { SubIndex = 0 },
                    new TimedTransition("inner2", 9000) { SubIndex = 0 },
                    new Shoot(0, 5, 40, cooldown: 1500),
                    new Shoot(5, 3, 20, index: 3, cooldown: 500),
                    new Shoot(0, 8, 45 / 2, 1, null, null, 180),
                    new Shoot(0, 1, null, 2, null, 45 / 2, 180, cooldown: 200),
                    new QueuedBehav(true,
                        new Taunt(cooldown: 0, "Fear me!"),
                        new CooldownBehav(3000, new Flash(0xff, 0.1, 3)),
                        new CooldownBehav(1000, new Charge(1.5f, 10))
                    ),
                    new HealthTransition(0.1f, "die")
                ),
                new State("die",
                    new Flash(0xff0000, 1.0, 3),
                    new Suicide(1000),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Taunt(cooldown: 0, "Lucky.")
                ),
                new Threshold(0.01f, LootTemplates.CrystalsHardRegular()),
                new Threshold(0.01f,
                    new ItemLoot("Oryx Equipment Crystal", 0.4f),
                    new ItemLoot("Oryx Equipment Crystal", 0.1f),
                    new ItemLoot("Ghastly Equipment Crystal", 0.1f),
                    new ItemLoot("Ghastly Equipment Crystal", 0.1f),
                    new TierLoot(6, LootType.Ring, 1.0f, r: new(1.0f, 1, true)),
                    new TierLoot(6, LootType.Ring, 1.0f, r: new(1.0f, 1, true)),
                    new TierLoot(12, LootType.Weapon, 1.0f, r: new(1.0f, 1, true)),
                    new TierLoot(12, LootType.Armor, 1.0f, r: new(1.0f, 1, true))
                ),
                new Threshold(0.1f,
                    new ItemLoot("Plague Poison", 0.05f)
                )
            );

            db.Init("Dragon God Gas Cloud", 
            new ConditionalEffect(ConditionEffectIndex.Invincible), 
            new Wander(0.3f),
            new PulseFire((e) =>
            {
                var nearbyPlayers = e.GetNearbyPlayers(2);
                foreach(var p in nearbyPlayers)
                {
                    if(p is Player pl)
                    {
                        pl.Damage("Gas Cloud", 50, new ConditionEffectDesc[] { new ConditionEffectDesc(ConditionEffectIndex.Sick, 300) }, true, true);
                    }
                }
                return true;
            }, 100), /*new ChangeSize(2, 0),*/ new Decay(5000));

        }
    }
}