using RotMG.Common;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Conditionals;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;
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
                new TierLoot(7, LootType.Weapon, 0.2f),
                new TierLoot(8, LootType.Weapon, 0.1f),
                new TierLoot(9, LootType.Weapon, 0.05f),
                new TierLoot(7, LootType.Armor, 0.2f),
                new TierLoot(8, LootType.Armor, 0.1f),
                new TierLoot(9, LootType.Armor, 0.05f),
                new TierLoot(4, LootType.Ring, 0.2f),
                new TierLoot(5, LootType.Ring, 0.1f),
                new TierLoot(6, LootType.Ring, 0.05f),
                new TierLoot(3, LootType.Ability, 0.1f),
                new TierLoot(4, LootType.Ability, 0.05f),
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
                    new State("Idle",
                        new Prioritize(
                            new StayAbove(1, 200),
                            new Wander(0.8f)
                            ),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new PlayerWithinTransition(8, "Attacking")
                        ),
                    new State("Attacking",
                        new State("Bullet",
                            new Shoot(1, 4, cooldown: 10000, fixedAngle: 90, cooldownOffset: 0, shootAngle: 90),
                            new Shoot(1, 4, cooldown: 10000, fixedAngle: 100, cooldownOffset: 200, shootAngle: 90),
                            new Shoot(1, 4, cooldown: 10000, fixedAngle: 110, cooldownOffset: 400, shootAngle: 90),
                            new Shoot(1, 4, cooldown: 10000, fixedAngle: 120, cooldownOffset: 600, shootAngle: 90),
                            new Shoot(1, 4, cooldown: 10000, fixedAngle: 130, cooldownOffset: 800, shootAngle: 90),
                            new Shoot(1, 4, cooldown: 10000, fixedAngle: 140, cooldownOffset: 1000, shootAngle: 90),
                            new Shoot(1, 4, cooldown: 10000, fixedAngle: 150, cooldownOffset: 1200, shootAngle: 90),
                            new Shoot(1, 4, cooldown: 10000, fixedAngle: 160, cooldownOffset: 1400, shootAngle: 90),
                            new Shoot(1, 4, cooldown: 10000, fixedAngle: 170, cooldownOffset: 1600, shootAngle: 90),
                            new Shoot(1, 4, cooldown: 10000, fixedAngle: 180, cooldownOffset: 1800, shootAngle: 90),
                            new Shoot(1, 8, cooldown: 10000, fixedAngle: 180, cooldownOffset: 2000, shootAngle: 45),
                            new Shoot(1, 4, cooldown: 10000, fixedAngle: 180, cooldownOffset: 0, shootAngle: 90),
                            new Shoot(1, 4, cooldown: 10000, fixedAngle: 170, cooldownOffset: 200, shootAngle: 90),
                            new Shoot(1, 4, cooldown: 10000, fixedAngle: 160, cooldownOffset: 400, shootAngle: 90),
                            new Shoot(1, 4, cooldown: 10000, fixedAngle: 150, cooldownOffset: 600, shootAngle: 90),
                            new Shoot(1, 4, cooldown: 10000, fixedAngle: 140, cooldownOffset: 800, shootAngle: 90),
                            new Shoot(1, 4, cooldown: 10000, fixedAngle: 130, cooldownOffset: 1000, shootAngle: 90),
                            new Shoot(1, 4, cooldown: 10000, fixedAngle: 120, cooldownOffset: 1200, shootAngle: 90),
                            new Shoot(1, 4, cooldown: 10000, fixedAngle: 110, cooldownOffset: 1400, shootAngle: 90),
                            new Shoot(1, 4, cooldown: 10000, fixedAngle: 100, cooldownOffset: 1600, shootAngle: 90),
                            new Shoot(1, 4, cooldown: 10000, fixedAngle: 90, cooldownOffset: 1800, shootAngle: 90),
                            new Shoot(1, 4, cooldown: 10000, fixedAngle: 90, cooldownOffset: 2000, shootAngle: 22.5f),
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
                    new ItemLoot("Potion of Defense", 0.05f),
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
                    new Shoot(10, count: 3, shootAngle: 20, predictive: 1, cooldown: 500),
                    new Reproduce(densityMax: 3)
                    ),
                new Threshold(0.01f,
                    new ItemLoot("Potion of Vitality", 0.05f)
                    ),
            new Threshold(.01f,
                    LootTemplates.MountainDrops()
                    )
            );
            db.Init("Ghost God",
                new State("base",
                    new DropPortalOnDeath("Undead Lair", .3f),
                    new Prioritize(
                        new StayAbove(1, 200),
                        new Follow(1, range: 2),
                        new Wander(0.4f)
                        ),
                    new Shoot(10, count: 8, shootAngle: (360/8), predictive: 1, cooldown: 500),
                    new Reproduce(densityMax: 3)
                    ),
                new Threshold(0.01f,
                    new ItemLoot("Potion of Speed", 0.05f)
                    ),
                new Threshold(.01f,
                    LootTemplates.MountainDrops()
                )
            );

            db.Init("Medusa",
                new State("base",
                    new DropPortalOnDeath("Snake Pit", .3f),
                    new Prioritize(
                        new StayAbove(1, 200),
                        new Follow(1, range: 7),
                        new Wander(0.4f)
                        ),
                    new Shoot(10, count: 5, shootAngle: 10, predictive: 1, cooldown: 2000),
                    new TossObject("Snake", throwEffect: true),
                    new Grenade(damage: 30, effect: ConditionEffectIndex.Bleeding, effectDuration: 200, cooldown: 600),
                    new Reproduce(densityMax: 3)
                    ),
                new Threshold(0.01f,
                    new ItemLoot("Potion of Speed", 0.05f)
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
                    new Shoot(12, index: 0, count: 4, shootAngle: 10, cooldown: 100),
                    new Shoot(10, index: 1, predictive: 1, cooldown: 1000),
                    new Reproduce(densityMax: 3),
                    new Reproduce("Sprite Child", 5, 5, 5000)
                    ),
                new Threshold(0.01f,
                    LootTemplates.MountainDrops()
                    ),
                new Threshold(0.01f,
                    new ItemLoot("Potion of Attack", 0.05f)
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
                    new Reproduce(densityMax: 3)
                    ),
                new Threshold(0.01f,
                    LootTemplates.MountainDrops()
                    ),
                new Threshold(0.01f,
                    new ItemLoot("Potion of Defense", 0.05f)
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
                    new Shoot(10, index: 1, predictive: 1),
                    new Reproduce(densityMax: 3)
                    ),
                new Threshold(0.01f,
                    LootTemplates.MountainDrops()
                    ),
                new Threshold(0.01f,
                    new ItemLoot("Potion of Defense", 0.05f)
                    )
            );
            db.Init("Flying Brain",
                new State("base",
                    new Prioritize(
                        new StayAbove(1, 200),
                        new Follow(1, range: 7),
                        new Wander(0.4f)
                        ),
                    new Shoot(12, count: 5, shootAngle: 72, cooldown: 500),
                    new Reproduce(densityMax: 3),
                    new DropPortalOnDeath("Mad Lab Portal", .5f)
                    ),
                new Threshold(0.01f,
                    LootTemplates.MountainDrops()
                    ),
                new Threshold(0.01f,
                    new ItemLoot("Potion of Attack", 0.05f)
                    )
            );
            db.Init("Slime God",
                new State("base",
                    new Prioritize(
                        new StayAbove(1, 200),
                        new Follow(1, range: 7),
                        new Wander(0.4f)
                        ),
                    new Shoot(12, index: 0, count: 5, shootAngle: 10, predictive: 1, cooldown: 1000),
                    new Shoot(10, index: 1, predictive: 1, cooldown: 650),
                    new Reproduce(densityMax: 2)
                    ),
                new Threshold(0.01f,
                    LootTemplates.MountainDrops()
                    ),
                new Threshold(0.01f,
                    new ItemLoot("Potion of Defense", 0.05f)
                    )
            );
            db.Init("Rock Bot",
                new State("base",
                    new Spawn("Paper Bot", maxChildren: 1, initialSpawn: 1, coolDown: 10000, givesNoXp: false),
                    new Spawn("Steel Bot", maxChildren: 1, initialSpawn: 1, coolDown: 10000, givesNoXp: false),
                    new Swirl(speed: 0.6f, radius: 3, targeted: false),
                    new State("Waiting",
                        new PlayerWithinTransition(15, "Attacking")
                        ),
                    new State("Attacking",
                        new Shoot(8, cooldown: 2000),
                        new HealEntity(8, "Papers", coolDown: 1000),
                        new Taunt(0.5f, "We are impervious to non-mystic attacks!"),
                        new TimedTransition("Waiting", 10000)
                        )
                    ),
                new Threshold(0.01f,
                    LootTemplates.MountainDrops()
                    ),
                new Threshold(0.04f,
                    new ItemLoot("Potion of Attack", 0.05f)
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
                        new HealEntity(8, "Steels", coolDown: 1000),
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
                    new ItemLoot("Potion of Attack", 0.05f)
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
                        new HealEntity(8, "Rocks", coolDown: 1000),
                        new Taunt(0.5f, "Silly squishy. We heal our brothers in a circle."),
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
                    new ItemLoot("Potion of Attack", 0.05f)
                    )
            );
            db.Init("Djinn",
                new State("base",
                    new State("Idle",
                        new Prioritize(
                            new StayAbove(1, 200),
                            new Wander(0.8f)
                            ),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Reproduce(densityMax: 3, densityRadius: 20),
                        new PlayerWithinTransition(8, "Attacking")
                        ),
                    new State("Attacking",
                        new State("Bullet",
                            new Shoot(1, count: 4, cooldown: 10000, fixedAngle: 90, cooldownOffset: 0, shootAngle: 90),
                            new Shoot(1, count: 4, cooldown: 10000, fixedAngle: 100, cooldownOffset: 200, shootAngle: 90),
                            new Shoot(1, count: 4, cooldown: 10000, fixedAngle: 110, cooldownOffset: 400, shootAngle: 90),
                            new Shoot(1, count: 4, cooldown: 10000, fixedAngle: 120, cooldownOffset: 600, shootAngle: 90),
                            new Shoot(1, count: 4, cooldown: 10000, fixedAngle: 130, cooldownOffset: 800, shootAngle: 90),
                            new Shoot(1, count: 4, cooldown: 10000, fixedAngle: 140, cooldownOffset: 1000,
                                shootAngle: 90),
                            new Shoot(1, count: 4, cooldown: 10000, fixedAngle: 150, cooldownOffset: 1200,
                                shootAngle: 90),
                            new Shoot(1, count: 4, cooldown: 10000, fixedAngle: 160, cooldownOffset: 1400,
                                shootAngle: 90),
                            new Shoot(1, count: 4, cooldown: 10000, fixedAngle: 170, cooldownOffset: 1600,
                                shootAngle: 90),
                            new Shoot(1, count: 4, cooldown: 10000, fixedAngle: 180, cooldownOffset: 1800,
                                shootAngle: 90),
                            new Shoot(1, count: 8, cooldown: 10000, fixedAngle: 180, cooldownOffset: 2000,
                                shootAngle: 45),
                            new Shoot(1, count: 4, cooldown: 10000, fixedAngle: 180, cooldownOffset: 0, shootAngle: 90),
                            new Shoot(1, count: 4, cooldown: 10000, fixedAngle: 170, cooldownOffset: 200, shootAngle: 90),
                            new Shoot(1, count: 4, cooldown: 10000, fixedAngle: 160, cooldownOffset: 400, shootAngle: 90),
                            new Shoot(1, count: 4, cooldown: 10000, fixedAngle: 150, cooldownOffset: 600, shootAngle: 90),
                            new Shoot(1, count: 4, cooldown: 10000, fixedAngle: 140, cooldownOffset: 800, shootAngle: 90),
                            new Shoot(1, count: 4, cooldown: 10000, fixedAngle: 130, cooldownOffset: 1000,
                                shootAngle: 90),
                            new Shoot(1, count: 4, cooldown: 10000, fixedAngle: 120, cooldownOffset: 1200,
                                shootAngle: 90),
                            new Shoot(1, count: 4, cooldown: 10000, fixedAngle: 110, cooldownOffset: 1400,
                                shootAngle: 90),
                            new Shoot(1, count: 4, cooldown: 10000, fixedAngle: 100, cooldownOffset: 1600,
                                shootAngle: 90),
                            new Shoot(1, count: 4, cooldown: 10000, fixedAngle: 90, cooldownOffset: 1800, shootAngle: 90),
                            new Shoot(1, count: 4, cooldown: 10000, fixedAngle: 90, cooldownOffset: 2000,
                                shootAngle: 22.5f),
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
                    new ItemLoot("Potion of Wisdom", 0.05f)
                    )
            );
            db.Init("Leviathan",
                new State("base",
                    new State("Wander",
                        new Swirl(),
                        new Shoot(10, 2, 10, 1, cooldown: 500),
                        new TimedTransition("Triangle", 5000)
                        ),
                    new State("Triangle",
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
                            new TimedTransition("Wander", 1500)
                        )
                    ),
                new Threshold(0.01f,
                    LootTemplates.MountainDrops()
                    ),
                new Threshold(0.01f,
                    new ItemLoot("Potion of Wisdom", 0.05f)
                    )
            ));

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
        }
    }
}
