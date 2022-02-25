using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Conditionals;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;
using RotMG.Networking;
using RotMG.Utils;
using System.Linq;
using static RotMG.Game.Logic.LootDef;
using static RotMG.Game.Logic.Loots.TierLoot;

namespace RotMG.Game.Logic.Database
{
    public class Mountains : IBehaviorDatabase
    {
        //TimedTransition\((\d+),\s(.+)\)
        //TimedTransition($2, $1)

        public void Init(BehaviorDb db)
        {
            db.EveryInit = new IBehavior[]
            {
                new TierLoot(7, LootType.Weapon, 0.5f),
                new TierLoot(8, LootType.Weapon, 0.4f),
                new TierLoot(9, LootType.Weapon, 0.3f),
                new TierLoot(7, LootType.Armor, 0.5f),
                new TierLoot(8, LootType.Armor, 0.4f),
                new TierLoot(9, LootType.Armor, 0.3f),
                new TierLoot(4, LootType.Ring, 0.4f),
                new TierLoot(5, LootType.Ring, 0.1f),
                new TierLoot(6, LootType.Ring, 0.05f),
                new TierLoot(3, LootType.Ability, 0.1f),
                new TierLoot(4, LootType.Ability, 0.05f),
                new TierLoot(5, LootType.Ability, 0.01f),
                new ItemLoot("Cracked Dangerous Prism", 0.035f, 0.01f),
                new ItemLoot("Realm Equipment Crystal", 0.01f),
                new ItemLoot("(Green) UT Egg", 0.01f, 0.01f),
                new ItemLoot("(Blue) RT Egg", 0.003f, 0.01f),
            };

            db.Init("Arena Horseman Anchor",
                new State("base",
                    new ConditionalEffect(ConditionEffectIndex.Invincible)
                    )
            );

            db.Init("Arena Headless Horseman",
                new State("base",
                    new Spawn("Arena Horseman Anchor", 1, 1),
                    new Prioritize(
                        new StayAbove(1, 200),
                        new Follow(1, range: 7),
                        new Wander(0.4f)
                        ),
                    new State("EverythingIsCool",
                        new HealthTransition(0.1f, "End"),
                        new State("Circle",
                            new Shoot(15, 3, shootAngle: 25, index: 0, cooldown: 1000),
                            new Shoot(15, index: 1, cooldown: 1000),
                            new Orbit(1, 5, 10, "Arena Horseman Anchor"),
                            new TimedTransition("Shoot", 8000)
                            ),
                        new State("Shoot",
                            new ReturnToSpawn(1.5f),
                            new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new Flash(0xF0E68C, 1, 6),
                            new Shoot(15, 8, index: 2, cooldown: 1500),
                            new Shoot(15, index: 1, cooldown: 2500),
                            new TimedTransition("Circle", 6000)
                            )
                        ),
                    new State("End",
                        new Prioritize(
                            new Follow(1.5f, 20, 1),
                            new Wander(1.5f)
                            ),
                        new Flash(0xF0E68C, 1, 1000),
                        new Shoot(15, 3, shootAngle: 25, index: 0, cooldown: 1000),
                        new Shoot(15, index: 1, cooldown: 1000)
                        ),
                    new DropPortalOnDeath("Haunted Cemetery Portal", .7f)
                    ),
                new Threshold(0.01f,
                    LootTemplates.MountainDrops()
                    )
            );
           db.Init("Lucky Ent God",
                new State("base",
                    new DropPortalOnDeath("Woodland Labyrinth", 100),
                    new Prioritize(
                        new StayAbove(1, 200),
                        new Follow(1, range: 7),
                        new Wander(0.4f)
                        ),
                    new Shoot(12, 5, 10, predictive: 1, cooldown: 1250)

                    ),
                new Threshold(0.18f,
                    new ItemLoot("Potion of Defense", 1f),
                    new ItemLoot("Potion of Attack", 1f),
                    new TierLoot(10, LootType.Weapon, 0.2f),
                    new TierLoot(10, LootType.Armor, 0.2f)
                    )
            );
          db.Init("Lucky Djinn",
                new State("base",
                    new DropPortalOnDeath("The Crawling Depths", 100),
                    new TransitionFrom("base", "Idle"),
                    new State("Idle",
                        new Prioritize(
                            new StayAbove(1, 200),
                            new Wander(0.8f)
                            ),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new PlayerWithinTransition(8, "Attacking")
                        ),
                    new State("Attacking",
                        new TransitionFrom("Attacking", "Bullet"),
                        new State("Bullet",
                            new Shoot(1, count: 4, cooldown: 200, fixedAngle: 90, rotateAngle: 10, shootAngle: 90),
                            new Shoot(1, count: 4, cooldown: 200, fixedAngle: 180, rotateAngle: -10, shootAngle: 90),
                            new TimedTransition("Wait", 2000)
                            ),
                        new State("Wait",
                            new Follow(0.7f, range: 0.5f),
                            new Flash(0xff00ff00, 0.1f, 20),
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new TimedTransition("Bullet", 2000)
                            ),
                        new NoPlayerWithinTransition(13, "Idle"),
                        new HealthTransition(0.5f, "FlashBeforeExplode")
                        ),
                    new State("FlashBeforeExplode",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0xff0000, 0.3f, 3),
                        new TimedTransition("Explode", 1000)
                        ),
                    new State("Explode",
                        new Shoot(0, 10, 36, fixedAngle: 0),
                        new Suicide()
                        )
                    ),
                new Threshold(0.01f,
                    LootTemplates.MountainDrops()
                    ),
                new Threshold(0.18f,
                    new ItemLoot("Potion of Defense", 0.25f),
                    new TierLoot(10, LootType.Ability, 0.2f),
                    new TierLoot(10, LootType.Ring, 0.2f)
                    )
            );
            db.Init("White Demon",
                new State("base",
                    new DropPortalOnDeath("Abyss of Demons Portal", .3f),
                    new Prioritize(
                        new StayAbove(1, 200),
                        new Follow(1, range: 7),
                        new Wander(0.4f)
                        ),
                    new Shoot(10, count: 3, shootAngle: 20, predictive: 1, cooldown: 500)
                    ),
                new Threshold(0.01f,
                    new ItemLoot("Potion of Vitality", 0.25f),
                    new ItemLoot("Antique White Clothing Dye", 0.01f),
                    new ItemLoot("Antique White Accessory Dye", 0.01f)
                    ),
                new Threshold(.01f,
                    LootTemplates.MountainDrops()
                    )
            );
            db.Init("Ghost God",
                new State("base",
                    new DropPortalOnDeath("Undead Lair Portal", .3f),
                    new DropPortalOnDeath("Lost Catacombs Portal", .08f),
                    new Prioritize(
                        new StayAbove(1, 200),
                        new Follow(1, range: 2),
                        new Wander(0.4f)
                        ),
                    new Shoot(10, count: 8, shootAngle: (360/8), predictive: 1, cooldown: 500)
                    ),
                new Threshold(0.01f,
                    new ItemLoot("Potion of Wisdom", 0.25f),
                    new ItemLoot("Spectral Gown", 0.02f, r: new RarityModifiedData(1.2f))
                    ),
                new Threshold(.01f,
                    LootTemplates.MountainDrops()
                )
            );

            db.Init("Medusa",
                new State("base",
                    new DropPortalOnDeath("Snake Pit Portal", .3f),
                    new DropPortalOnDeath("Toxic Sewer Portal", .1f),
                    new Prioritize(
                        new StayAbove(1, 200),
                        new Follow(1, range: 7),
                        new Wander(0.4f)
                        ),
                    new Shoot(10, count: 5, shootAngle: 10, predictive: 1, cooldown: 2000),
                    new TossObject("Snake", throwEffect: true),
                    new Grenade(damage: 30, effect: ConditionEffectIndex.Bleeding, effectDuration: 200, cooldown: 600)
                    ),
                new Threshold(0.01f,
                    new ItemLoot("Potion of Speed", 0.05f),
                    new ItemLoot("Garment of Medusa", 0.02f, r: new RarityModifiedData(1.2f))
                    ),
                new Threshold(.01f,
                    LootTemplates.MountainDrops()
                )
            );


            db.Init("Sprite God",
                new State("base",
                    new Prioritize(
                        new StayAbove(1, 200),
                        new Wander(0.4f)
                        ),
                    new Shoot(12, index: 0, count: 4, shootAngle: 10, cooldown: 200, callback: (e) => {
                        e.ApplyConditionEffect((ConditionEffectIndex)MathUtils.Next(26), Settings.MillisecondsPerTick * 5);
                    }),
                    new Shoot(10, index: 1, predictive: 1, cooldown: 1000),
                    new Reproduce("Sprite Child", 5, 5, 5000)
                    ),
                new Threshold(0.01f,
                    LootTemplates.MountainDrops()
                    ),
                new Threshold(0.01f,
                    new ItemLoot("Potion of Attack", 0.05f),
                    new ItemLoot("Potion of Dexterity", 0.15f)
                    )
            );
            db.Init("Ent God",
                new State("base",
                    new Prioritize(
                        new StayAbove(1, 200),
                        new Follow(1, range: 7),
                        new Wander(0.4f)
                        ),
                    new Shoot(12, count: 5, shootAngle: 10, predictive: 1, cooldown: 1250),
                    new Shoot(12, count: 2, shootAngle: 10, predictive: 1, cooldown: 2500, index: 1)
                    ),
                new Threshold(0.01f,
                    LootTemplates.MountainDrops()
                    ),
                new Threshold(0.01f,
                    new ItemLoot("Potion of Defense", 0.25f)
                    )
            );
            db.Init("Beholder",
                new State("base",
                    new Prioritize(
                        new StayAbove(1, 200),
                        new Follow(1, range: 7),
                        new Wander(0.4f)
                        ),
                    new Shoot(12, index: 0, count: 5, shootAngle: 72, predictive: 0.5f, cooldown: 750),
                    new Shoot(10, index: 1, cooldown: 300)
                    ),
                new Threshold(0.01f,
                    LootTemplates.MountainDrops()
                    ),
                new Threshold(0.01f,
                    new ItemLoot("Potion of Defense", 0.25f),
                    new ItemLoot("Curious Eyeball", 0.02f, r: new RarityModifiedData(1.2f))
                    )
            );
            db.Init("Flying Brain",
                new State("base",
                    new Prioritize(
                        new StayAbove(1, 200),
                        new Follow(1, range: 1),
                        new Wander(0.4f)
                        ),
                    new Shoot(12, count: 5, shootAngle: 72, cooldown: 500),
                    new Shoot(12, count: 8, shootAngle: 45, cooldown: 1000),
                    new DropPortalOnDeath("Mad Lab Portal", .5f)
                    ),
                new Threshold(0.01f,
                    LootTemplates.MountainDrops()
                    ),
                new Threshold(0.01f,
                    new ItemLoot("Potion of Attack", 0.25f),
                    new ItemLoot("Mini Brain Orb", 0.01f)
                    )
            );
            db.Init("Slime God",
                new DropPortalOnDeath("Toxic Sewer Portal", 0.3f),
                new State("base",
                    new Prioritize(
                        new StayAbove(1, 200),
                        new Follow(1, range: 7),
                        new Wander(0.4f)
                        ),
                    new Shoot(12, index: 0, count: 5, shootAngle: 10, predictive: 1, cooldown: 1000),
                    new Shoot(10, index: 1, predictive: 1, cooldown: 650)
                    ),
                new Threshold(0.01f,
                    LootTemplates.MountainDrops()
                    ),
                new Threshold(0.01f,
                    new ItemLoot("Potion of Defense", 0.50f)
                    )
            );
            db.Init("Fire Slime",
                new DropPortalOnDeath("Shaitans Lair Portal", 0.2f),
                new State("base",
                    new Prioritize(
                        new StayAbove(1, 200),
                        new Follow(1, range: 7),
                        new Wander(0.4f)
                    ),
                    new Shoot(12, index: 0, count: 2, shootAngle: 10, predictive: 1, cooldown: 1000, callback: e =>
                    {
                        Entity spawning = Entity.Resolve(
                            Resources.Id2Object["Brute of the Abyss"].Type
                        );
                        e.Parent.AddEntity(spawning, e.Position);
                    }),
                    new Shoot(10, index: 1, predictive: 1, cooldown: 650)
                ),
                new Threshold(0.01f,
                    LootTemplates.MountainDrops()
                ),
                new Threshold(0.01f,
                    new ItemLoot("Potion of Attack", 0.75f)
                )
            );
            db.Init("Rock Bot",
                new State("base",
                    new Spawn("Paper Bot", maxChildren: 1, initialSpawn: 1, cooldown: 10000, givesNoXp: false),
                    new Spawn("Steel Bot", maxChildren: 1, initialSpawn: 1, cooldown: 10000, givesNoXp: false),
                    new Swirl(speed: 0.6f, radius: 3, targeted: false),
                    new State("Waiting",
                        new PlayerWithinTransition(15, "Attacking")
                        ),
                    new State("Attacking",
                        new Shoot(8, cooldown: 2000),
                        new HealEntity(8, "Papers", cooldown: 1000),
                        new Taunt(0.01f, "We are impervious to non-mystic attacks!"),
                        new TimedTransition("Waiting", 10000)
                        )
                    ),
                new Threshold(0.01f,
                    LootTemplates.MountainDrops()
                    ),
                new Threshold(0.04f,
                    new ItemLoot("Potion of Attack", 0.50f),
                    new ItemLoot("Crumbling Construct", 0.02f, r: new RarityModifiedData(1.2f))
                    )
            );
            db.Init("Paper Bot",
                new State("base",
                    new DropPortalOnDeath("Puppet Theatre Portal", 0.45f),
                    new Prioritize(
                        new Orbit(0.4f, 3, target: "Rock Bot"),
                        new Wander(0.8f)
                        ),
                    new State("Idle",
                        new PlayerWithinTransition(15, "Attack")
                        ),
                    new State("Attack",
                        new Shoot(8, count: 3, shootAngle: 20, cooldown: 800),
                        new HealEntity(8, "Steels", cooldown: 1000),
                        new NoPlayerWithinTransition(30, "Idle"),
                        new HealthTransition(0.2f, "Explode")
                        ),
                    new State("Explode",
                        new Shoot(0, count: 10, shootAngle: 36, fixedAngle: 0),
                        new Decay(0)
                        )
                    ),
                new Threshold(0.01f,
                    LootTemplates.MountainDrops()
                    ),
                new Threshold(0.04f,
                    new ItemLoot("Potion of Attack", 0.50f),
                    new ItemLoot("Crumbling Construct", 0.02f, r: new RarityModifiedData(1.2f))
                    )
            );
            db.Init("Steel Bot",
                new State("base",
                    new Prioritize(
                        new Orbit(0.4f, 3, target: "Rock Bot"),
                        new Wander(0.8f)
                        ),
                    new State("Idle",
                        new PlayerWithinTransition(15, "Attack")
                        ),
                    new State("Attack",
                        new Shoot(8, count: 3, shootAngle: 20, cooldown: 800),
                        new HealEntity(8, "Rocks", cooldown: 1000),
                        new Taunt(0.01f, "Silly squishy. We heal our brothers in a circle."),
                        new NoPlayerWithinTransition(30, "Idle"),
                        new HealthTransition(0.2f, "Explode")
                        ),
                    new State("Explode",
                        new Shoot(0, count: 10, shootAngle: 36, fixedAngle: 0),
                        new Decay(0)
                        )
                    ),
                new Threshold(0.01f,
                    LootTemplates.MountainDrops()
                    ),
                new Threshold(0.04f,
                    new ItemLoot("Potion of Attack", 0.50f),
                    new ItemLoot("Crumbling Construct", 0.02f, r: new RarityModifiedData(1.2f))
                    )
            );
            db.Init("Djinn",
                new State("base",
                    new TransitionFrom("base", "Idle"),
                    new State("Idle",
                        new Prioritize(
                            new StayAbove(1, 200),
                            new Wander(0.8f)
                            ),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new PlayerWithinTransition(8, "Attacking")
                    ),
                    new State("Attacking",
                        new TransitionFrom("Attacking", "Bullet"),
                        new State("Bullet",
                            new Shoot(1, count: 4, cooldown: 200, fixedAngle: 90, rotateAngle: 10, shootAngle: 90),
                            new Shoot(1, count: 4, cooldown: 200, fixedAngle: 180, rotateAngle: -10, shootAngle: 90),
                            new TimedTransition("Wait", 2000)
                        ),
                        new State("Wait",
                            new Follow(0.7f, range: 0.5f),
                            new Flash(0xff00ff00, 0.1f, 20),
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new TimedTransition("Bullet", 2000)
                        ),
                        new NoPlayerWithinTransition(13, "Idle"),
                        new DropPortalOnDeath("Treasure Cave Portal", 0.5f),
                        new HealthTransition(0.5f, "FlashBeforeExplode")
                    ),
                    new State("FlashBeforeExplode",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0xff0000, 0.3f, 3),
                        new TimedTransition("Explode", 1000)
                    ),
                    new State("Explode",
                        new Shoot(0, count: 10, shootAngle: 36, fixedAngle: 0),
                        new DropPortalOnDeath("Treasure Cave Portal", 0.5f),
                        new Suicide()
                    )
                    ),
                new Threshold(0.01f,
                    LootTemplates.MountainDrops()
                    ),
                new Threshold(0.01f,
                    new ItemLoot("Potion of Wisdom", 0.50f)
                    )
            );
            db.Init("Leviathan",
                new State("base",
                    new TransitionFrom("base", "Wander"),
                    new State("Wander",
                        new Swirl(),
                        new Shoot(10, 2, 10, 1, cooldown: 500),
                        new TimedTransition("Triangle", 5000)
                    ),
                    new State("Triangle",
                        new TransitionFrom("Triangle", "1"),
                        new State("1",
                            new MoveLine(.7f, 40),
                            new Shoot(1, 3, 120, fixedAngle: 34, cooldown: 300),
                            new Shoot(1, 3, 120, fixedAngle: 38, cooldown: 300),
                            new Shoot(1, 3, 120, fixedAngle: 42, cooldown: 300),
                            new Shoot(1, 3, 120, fixedAngle: 46, cooldown: 300),
                            new TimedTransition("2", 1500)
                            ),
                        new State("2",
                            new MoveLine(.7f, 160),
                            new Shoot(1, 3, 120, fixedAngle: 94, cooldown: 300),
                            new Shoot(1, 3, 120, fixedAngle: 98, cooldown: 300),
                            new Shoot(1, 3, 120, fixedAngle: 102, cooldown: 300),
                            new Shoot(1, 3, 120, fixedAngle: 106, cooldown: 300),
                            new TimedTransition("3", 1500)
                            ),
                        new State("3",
                            new MoveLine(.7f, 280),
                            new Shoot(1, 3, 120, fixedAngle: 274, cooldown: 300),
                            new Shoot(1, 3, 120, fixedAngle: 278, cooldown: 300),
                            new Shoot(1, 3, 120, fixedAngle: 282, cooldown: 300),
                            new Shoot(1, 3, 120, fixedAngle: 286, cooldown: 300),
                            new TimedTransition("Wander", 1500) { SubIndex = 2 }
                        )
                    ),
                new DropPortalOnDeath("Dragon Cave Portal", 0.45f),
                new Threshold(0.01f,
                    LootTemplates.MountainDrops()
                    ),
                new Threshold(0.01f,
                    new ItemLoot("Potion of Wisdom", 1f),
                    new ItemLoot("Potion of Dexterity", 0.3f)
                    )
            ));

            db.Init("Red Demon",
                new State("base",
                    // Drop shaitans eventually
                    new DropPortalOnDeath("Abyss of Demons Portal", 1f),
                    new Prioritize(
                        new Follow(1, range: 7),
                        new StayCloseToSpawn(1, 20),
                        new Wander(0.4f)
                        ),
                        new Shoot(10, count: 3, shootAngle: 20, predictive: 1, cooldown: 100),
                        new Shoot(10, count: 3, shootAngle: 20, predictive: 1, index: 1, cooldown: 5000)
                    ),
                new Threshold(0.01f,
                    new ItemLoot("Potion of Vitality", 1.0f),
                    new ItemLoot("Potion of Defense", 1.0f),
                    new ItemLoot("Sanguine Femur", 0.02f),
                    new ItemLoot("Fire Dragon Battle Armor", 0.001f)
                ),
            new Threshold(.01f,
                    LootTemplates.MountainDrops().Concat(
                        LootTemplates.BasicPots(0.01f)
                    ).ToArray()
                )
                );


            db.EveryInit = new IBehavior[] { };

            //So they don't get loot
            db.Init("Sprite Child",
                new State("base",
                    new Prioritize(
                        new StayAbove(1, 200),
                        new Protect(0.4f, "Sprite God", protectionRange: 1),
                        new Wander(0.4f)
                        ),
                    new DropPortalOnDeath("Glowing Portal", .4f)
                    )
            );

            db.Init("Greater Fire Skeleton", 
                new DropPortalOnDeath("Shaitans Lair Portal", 1f),
                new Prioritize(
                    new Orbit(2f, 2f, 10f, "Red Demon"),
                    new Wander(1.8f)
                ),
                new State("starting",
                    new PlayerWithinTransition(10f, "throwreddemon", true)
                ),
                new State("throwreddemon", 
                    new TossObject("Red Demon", 10f),
                    new Taunt("Eat red demons!"),
                    new TimedTransition("throwwhitedemons", 0)
                ),
                new State("throwwhitedemons", 
                    new TossObject("White Demon", 10f, cooldown: 15000),
                    new Shoot(20f, 3, index: 1, cooldown: 1000, rotateAngle: 90),
                    new Shoot(20f, 1, index: 0, cooldown: 1000)
                ),
                new ItemLoot("Potion of Life", 1f, 0.01f),
                new TierLoot(11, LootType.Weapon, 1f, 0.01f),
                new TierLoot(11, LootType.Armor, 1f, 0.01f),
                new TierLoot(6, LootType.Ring, 1f, 0.01f),
                new ItemLoot("Piece of Havoc", 0.003f, 0.01f),
                new ItemLoot("Unholy Robe", 0.003f, 0.01f),
                new ItemLoot("Amulet of Backwards Luck", 0.003f, 0.01f)
            );

        }
    }
}
