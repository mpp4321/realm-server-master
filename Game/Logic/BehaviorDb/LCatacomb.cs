using RotMG.Common;
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
    class LCatacomb : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {
            db.Init("Undead Giant Hand", 
                new State("base", 
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: Settings.MillisecondsPerTick, reapply: (e) =>
                    {
                        return e?.GetNearbyPlayers(10f).Count() == 0;
                    }),
                    new State("playerwait", new PlayerWithinTransition(10f, "jumping", false)),
                    new State("jumping",
                        new ChangeSize(-10, 0),
                        new TimedTransition("jumped", 1000)
                    ),
                    new State("jumped", 
                        new Charge(8f, 10, coolDown: 6000),
                        new ChangeSize(10, 50),
                        new Follow(0.1f, 99, 0.1f),
                        new Shoot(5, 1, 0, predictive: 1, fixedAngle: 0, cooldownOffset: 500,  cooldown: 5000, index: 1),
                        new TimedTransition("post jump", 1000)
                    ),
                    new State("post jump",
                        new Shoot(5, 8, 360 / 8.0f, predictive: 1, fixedAngle: 0, cooldownOffset: 500,  cooldown: 5000, index: 1),
                        new Shoot(5, 3, 30, 0, cooldown: 1000),
                        new Shoot(5, 2, 15, 0, cooldown: 2000),
                        new HealSelf(cooldown: 500, 1000),
                        new Grenade(0, 0, 5, 0f, 1000, effect: ConditionEffectIndex.CursedLower, effectDuration: 1000, color: 0xff11ff11),
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new TimedTransition("jumping", 5000)
                    )
                ),
                new Threshold(0.01f,
                    new ItemLoot("Potion of Life", min: 2),
                    new ItemLoot("Potion of Vitality", min: 2),
                    new ItemLoot("Potion of Attack", min: 2),
                    new ItemLoot("Oryx Equipment Crystal", 0.02f),
                    new ItemLoot("Realm Equipment Crystal", 0.2f),
                    new TierLoot(12, LootType.Armor, 0.1f, r: new LootDef.RarityModifiedData(1.0f, 1, alwaysRare: true)),
                    new TierLoot(12, LootType.Weapon, 0.1f, r: new LootDef.RarityModifiedData(1.0f, 1, alwaysRare: true)),
                    new TierLoot(5, LootType.Ability, 0.1f, r: new LootDef.RarityModifiedData(1.0f, 1, alwaysRare: true)),
                    new ItemLoot("Bow of Life Energy", 0.01f),
                    new ItemLoot("Reinforced Ribcage", 0.01f)
                )
            );

            db.Init("LCatacomb Hand Spawner",
                    new State("waiting",
                        new ConditionalEffect(Common.ConditionEffectIndex.Invulnerable),
                        new ConditionalEffect(Common.ConditionEffectIndex.Invincible),
                        new EntitiesNotExistsTransition(99, "spawnndie", "Zombie Hulk")
                    ),
                    new State("spawnndie",
                        new Spawn("Undead Giant Hand", maxChildren: 1, initialSpawn: 1.0, givesNoXp: false),
                        new Suicide()
                    )
                );

            db.Init("Zombie Knight", 
                    new State("base",
                        new Shoot(
                            10,
                            2,
                            15,
                            cooldown: 600
                        ),
                        new Shoot(
                            3,
                            3,
                            10,
                            index: 1,
                            cooldown: 3000),
                        new Prioritize(
                            new Charge(1.0f, 10, 3000),
                            new Wander(0.3f)
                        )
                    )
                );

            db.Init("Zombie Wizard", 
                    new State("base",
                        new Shoot(
                            10,
                            2,
                            10,
                            cooldown: 1500
                        ),
                        new Prioritize(
                            new StayBack(1.0f, 5),
                            new Wander(0.5f)
                        )
                    )
                );

            db.Init("Zombie Mystic", 
                    new State("base",
                        new Shoot(
                            10,
                            2,
                            10,
                            cooldown: 3000
                        ),
                        new Shoot(
                            99,
                            8,
                            45,
                            index: 1,
                            cooldown: 1000),
                        new Prioritize(
                            new StayBack(1.0f, 5),
                            new Wander(0.5f)
                        )
                    )
                );

            db.Init("Zombie Assassin", 
                    new State("base",
                        new Shoot(
                            10,
                            2,
                            10,
                            cooldown: 1500
                        ),
                        new Grenade(6, damage: 100, radius: 3, color: 0xff00ff00, cooldown: 3000),
                        new Prioritize(
                            new StayBack(1.0f, 5),
                            new Wander(0.5f)
                        )
                    )
                );

            db.Init("Zombie Huntress", 
                    new State("base",
                        new Shoot(
                            10,
                            2,
                            10,
                            cooldown: 1500
                        ),
                        new TossObject("Arena Spider", 6, cooldown: 3000),
                        new Prioritize(
                            new StayBack(1.0f, 5),
                            new Wander(0.5f)
                        )
                    )
                );

            db.Init("Zombie Paladin", 
                    new State("base",
                        new Shoot(
                            10,
                            1,
                            cooldown: 1500
                        ),
                        new HealEntity(10, "Zombies", 50, cooldown: 300),
                        new Prioritize(
                            new StayBack(1.0f, 5),
                            new Wander(0.5f)
                        )
                    )
                );

            db.Init("Sentinel Cube", 
                new State("wakeup",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new RandomTransition(
                        new TransitionFrom("wakeup", "slayer") { SubIndex=2 },
                        new TransitionFrom("wakeup", "watcher") { SubIndex=2 },
                        new TransitionFrom("wakeup", "charger") { SubIndex=2 },
                        new TransitionFrom("wakeup", "blob") { SubIndex=2 }
                    )
                ), 
                new State("wait",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable)
                ),
                new State("statue death",
                    new Taunt(cooldown: 0, "Grrr!"),
                    new TimedTransition("wakeup", 8000)
                ),
                new State("slayer", 
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Taunt("Here comes the slayer!"),
                    new TossObject("Sentinel Slayer", 7, 45f, 3000, throwEffect: true),
                    new TimedTransition("wait", 1000)
                ),
                new State("watcher", 
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Taunt("Here comes the watcher!"),
                    new TossObject("Sentinel Watcher", 7, 135f, 3000, throwEffect: true),
                    new TimedTransition("wait", 1000)
                ),
                new State("charger", 
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Taunt("Here comes the charger!"),
                    new TossObject("Sentinel Charger", 7, -45f, 3000, throwEffect: true),
                    new TimedTransition("wait", 1000)
                ),
                new State("blob", 
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Taunt("Here comes the BLOB!"),
                    new TossObject("Sentinel Blob", 7, -135f, 3000, throwEffect: true),
                    new TimedTransition("wait", 1000)
                ),
                new SetPieceOnDeath(new IntPoint(-3, -3), null, "SentinelDeath", true)
            );

            db.Init("Sentinel Slayer",
                new HPScale(0.1f),
                new ChangeGroundOnDeath(Region.Decoration1, "Green Cube Tile"),
                new ChangeGroundOnDeath(Region.Decoration2, "Red Cube Tile"),
                new ChangeGroundOnDeath(Region.Decoration3, "Pink Cube Tile"),
                new State("wakeup",
                    new ConditionalEffect(Common.ConditionEffectIndex.Invulnerable),
                    new Taunt(cooldown: 0, "Try to escape this!"),
                    new TimedTransition("go", 5000)
                ),
                new State("go",
                    new ChangeGroundOnEnter(Region.Decoration1, "Magma Lava"),
                    new ChangeGroundOnEnter(Region.Decoration2, "Magma Lava"),
                    new ChangeGroundOnEnter(Region.Decoration3, "Magma Lava"),
                    new OrderOnDeath(99f, "Sentinel Cube", "statue death"),
                    new Shoot(10f, 5, 360 / 5, 0, fixedAngle: 0, rotateAngle: 35, cooldown: 1000),
                    new QueuedBehav(true,
                        new CooldownBehav(500, null),
                        new RandomBehavior(
                            new Grenade(0f, 200, 10, 0, 0, color: 0xFFFF),
                            new Grenade(0f, 50, 10, 0, 0, color: 0xFF00FF, effect: ConditionEffectIndex.Bleeding, effectDuration: 500),
                            new Grenade(0f, 50, 10, 0, 0, color: 0x00FF00, effect: ConditionEffectIndex.Paralyzed, effectDuration: 500)
                        )
                    )
                )
            );

            db.Init("Sentinel Charger",
                new HPScale(0.1f),
                new ChangeGroundOnDeath(Region.Decoration1, "Green Cube Tile"),
                new ChangeGroundOnDeath(Region.Decoration4, "Yellow Cube Tile"),
                new ChangeGroundOnDeath(Region.Decoration2, "Red Cube Tile"),
                new State("wake", 
                    new TimedTransition("go", 3000),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Taunt(cooldown: 0, "Crush...")
                ),
                new State("go",
                    new ChangeGroundOnEnter(Region.Decoration1, "Magma Lava"),
                    new ChangeGroundOnEnter(Region.Decoration4, "Magma Lava"),
                    new ChangeGroundOnEnter(Region.Decoration2, "Magma Lava"),
                    new OrderOnDeath(10f, "Sentinel Cube", "statue death"),
                    new Shoot(99f, 12, 360/12, rotateAngle: 360/24, fixedAngle: 0, index: 0),
                    new Charge(1f, 10f, distance: 2f)
                )
            );

            db.Init("Sentinel Watcher",
                new HPScale(0.1f),
                new ChangeGroundOnDeath(Region.Decoration1, "Green Cube Tile"),
                new ChangeGroundOnDeath(Region.Decoration4, "Yellow Cube Tile"),
                new ChangeGroundOnDeath(Region.Decoration3, "Pink Cube Tile"),
                new State("wake", 
                    new TimedTransition("go", 3000),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Taunt(cooldown: 0, "...")
                ),
                new State("go",
                    new ChangeGroundOnEnter(Region.Decoration1, "Magma Lava"),
                    new ChangeGroundOnEnter(Region.Decoration4, "Magma Lava"),
                    new ChangeGroundOnEnter(Region.Decoration3, "Magma Lava"),
                    new OrderOnDeath(99f, "Sentinel Cube", "statue death"),
                    new Shoot(99f, 3, 15, 0, predictive: 1, cooldown: 500, cooldownVariance: 500),
                    new OrbitSpawn(2f, 6f, 99f)
                )
            );

            db.Init("Sentinel Blob",
                new HPScale(0.1f),
                new ChangeGroundOnDeath(Region.Decoration2, "Red Cube Tile"),
                new ChangeGroundOnDeath(Region.Decoration4, "Yellow Cube Tile"),
                new ChangeGroundOnDeath(Region.Decoration3, "Pink Cube Tile"),
                new State("wake", 
                    new TimedTransition("go", 3000),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Taunt(cooldown: 0, "...")
                ),
                new State("go",
                    new ChangeGroundOnEnter(Region.Decoration2, "Magma Lava"),
                    new ChangeGroundOnEnter(Region.Decoration4, "Magma Lava"),
                    new ChangeGroundOnEnter(Region.Decoration3, "Magma Lava"),
                    new OrderOnDeath(99f, "Sentinel Cube", "statue death"),
                    new Shoot(99f, 3, 360/3, 0, 0, 10),
                    new Shoot(99f, 8, 360/8, 1, 0, 360/18, cooldown: 300)
                )
            );
        }
    }
}
