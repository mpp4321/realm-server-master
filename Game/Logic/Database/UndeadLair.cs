
//by ???, GhostMaree and ppmaks
using RotMG.Common;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;
using static RotMG.Game.Logic.Loots.TierLoot;

namespace RotMG.Game.Logic.Database
{
	class UndeadLair : IBehaviorDatabase
	{
        public void Init(BehaviorDb db)
        {
            

            db.Init("Septavius the Ghost God",
                new DropPortalOnDeath(target: "Glowing Realm Portal"),
                new TransformOnDeath("Ghost Mage of Septavius", 1, 3),
                new TransformOnDeath("Ghost Rogue of Septavius", 1, 3),
                new TransformOnDeath("Ghost Warrior of Septavius", 1, 3),
                new State("base",
                    new Spawn("Lair Ghost Archer", 1, 1, cooldown: 2000),
                    new Spawn("Lair Ghost Knight", 2, 2, cooldown: 3000),
                    new Spawn("Lair Ghost Mage", 1, 1, cooldown: 2000),
                    new Spawn("Lair Ghost Rogue", 2, 2, cooldown: 3000),
                    new Spawn("Lair Ghost Paladin", 1, 1, cooldown: 2000),
                    new Spawn("Lair Ghost Warrior", 2, 2, cooldown: 3000),
                    new PlayerWithinTransition(8, "transition", seeInvis: true) { SubIndex = 1 },
                    new HealthTransition(0.3f, "warning"),
                    new State("transition",
                        new Wander(speed: 0.1f),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0x00FF00, 0.25f, 12),
                        new TimedTransition("spiral", 100) { SubIndex = 1 }
                        ),
                    new State("spiral",
                        new TimedTransition("transition0", 16500) { SubIndex = 2 },
                        new TransitionFrom("spiral", "spiral0"),
                        new State("spiral0",
                            new Wander(speed: 0.1f),
                            new Shoot(24, 3, 360 / 3, 1, 0, 16, cooldown: 300),
                            new Shoot(14, 3, 360 / 3, 0, 0, 16, cooldown: 150), 
                            new TimedTransition("spiral1", 4500)
                        ),
                        new State("spiral1",
                            new Wander(speed: 0.1f),
                            new Shoot(24, 3, 360 / 3, 1, 0, -16, cooldown: 300),
                            new Shoot(14, 3, 360 / 3, 0, 0, -16, cooldown: 150),
                            new TimedTransition("spiral0", 4500)
                        )
                    )
                ),
                new State("transition0",
                    new Wander(speed: 0.1f),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Flash(0x00FF00, 0.25f, 12),
                    new TimedTransition("ring", 1500)
                    ),
                new State("ring",
                    new TimedTransition("transition", 1800) { SubIndex = 0 },
                    new HealthTransition(0.3f, "warning"),
                    new Shoot(8, 3, 18, 2, predictive: 0.4f, cooldownVariance: 750, cooldown: 2250),
                    new TransitionFrom("ring", "ring0"),
                    new State("ring0",
                        new Shoot(8, 6, 360 / 6, 3, 0f, 24, cooldown: 1000),
                        new Wander(0.25f),
                        new TimedTransition("ring1", 400)
                    ),
                    new State("ring1",
                        new Shoot(8, 6, 360 / 6, 3, 0f, -24, cooldown: 800),
                        new Follow(0.6f, 16, 4),
                        new TimedTransition("ring2", 350)
                    ),
                    new State("ring2",
                        new Shoot(8, 6, 360 / 6, 3, 0f, 24, cooldown: 700),
                        new Follow(0.9f, 16, 3),
                        new TimedTransition("ring3", 300)
                    ),
                    new State("ring3",
                        new Shoot(8, 6, 360 / 6, 3, 0f, -24, cooldown: 600),
                        new Follow(1.2f, 16, 2),
                        new TimedRandomTransition(250, "ring4", "ring5")
                    ),
                    new State("ring4",
                        new Shoot(8, 6, 360 / 6, 3, 0f, 24, cooldown: 500),
                        new Orbit(1.5f, 2.75f, 18, speedVariance: 0.2f, radiusVariance: .75f, targetPlayers: true, pass: true)
                    ),
                    new State("ring5",
                        new Shoot(8, 6, 360 / 6, 3, 0f, -24, cooldown: 500),
                        new Orbit(1.5f, 2.75f, 18, speedVariance: 0.2f, radiusVariance: .75f, targetPlayers: true, pass: true)
                    )
                ),
                new State("warning",
                        new Flash(0xff0033ff, .3f, 3),
                        new TimedTransition("fire", 1000)
                    ),
                new State("fire",
                    new Shoot(10, 4, 10, 0, cooldown: 200),
                    new Shoot(10, 2, 40, 1, cooldown: 400),
                    new Prioritize(false,
                        new Prioritize(true,
                            new Charge(2.0, pred: e => e.HasConditionEffect(Common.ConditionEffectIndex.Quiet)),
                            new Shoot(10, 8, 45, 2, 0f)

                        ),
                        new Follow(0.8f, range: 1),
                        new Wander(0.4f)
                    ),
                    new TimedTransition("transition0", 6000)
                    ),
            new Threshold(0.01f,
                new ItemLoot("Undead Lair Key", 0.03f),
                new ItemLoot("Doom Bow", 0.004f),
                new ItemLoot("Wine Cellar Incantation", 0.05f),
                new ItemLoot("Potion of Wisdom", 1f),
                new ItemLoot("(Green) UT Egg", 0.1f, 0.01f),
                new ItemLoot("(Blue) RT Egg", 0.01f, 0.01f),
                new TierLoot(4, LootType.Ring, 0.25f),
                new TierLoot(5, LootType.Ring, 0.125f),
                new TierLoot(9, LootType.Weapon, 0.25f),
                new TierLoot(10, LootType.Weapon, 0.25f),
                new TierLoot(4, LootType.Ability, 0.25f),
                new TierLoot(5, LootType.Ability, 0.125f)
                )
            );
            db.Init("Ghost Mage of Septavius",
                new State("base",
                    new Prioritize(
                        new Protect(0.625f, "Septavius the Ghost God", protectionRange: 6),
                        new Follow(0.75f, range: 7)
                        ),
                    new Wander(0.25f),
                    new Shoot(8, 1, cooldown: 1000)
                    )
            );
            db.Init("Ghost Rogue of Septavius",
                new State("base",
                    new Follow(0.75f, range: 1),
                    new Wander(0.25f),
                    new Shoot(8, 1, cooldown: 1000)
                    )
            );
            db.Init("Ghost Warrior of Septavius",
                new State("base",
                    new Follow(0.75f, range: 1),
                    new Wander(0.25f),
                    new Shoot(8, 1, cooldown: 1000)
                    ),
                new ItemLoot("Health Potion", 0.25f),
                new ItemLoot("Magic Potion", 0.25f)
            );
            db.Init("Lair Ghost Archer",
                new State("base",
                    new Prioritize(
                        new Protect(0.625f, "Septavius the Ghost God", protectionRange: 6),
                        new Follow(0.75f, range: 7)
                        ),
                    new Wander(0.25f),
                    new Shoot(8, 1, cooldown: 1000)
                    ),
                new ItemLoot("Health Potion", 0.25f),
                new ItemLoot("Magic Potion", 0.25f)
            );
            db.Init("Lair Ghost Knight",
                new State("base",
                    new Follow(0.75f, range: 1),
                    new Wander(0.25f),
                    new Shoot(8, 1, cooldown: 1000)
                    ),
                new ItemLoot("Health Potion", 0.25f),
                new ItemLoot("Magic Potion", 0.25f)
            );
            db.Init("Lair Ghost Mage",
                new State("base",
                    new Prioritize(
                        new Protect(0.625f, "Septavius the Ghost God", protectionRange: 6),
                        new Follow(0.75f, range: 7)
                        ),
                    new Wander(0.25f),
                    new Shoot(8, 1, cooldown: 1000)
                    ),
                new ItemLoot("Health Potion", 0.25f),
                new ItemLoot("Magic Potion", 0.25f)
            );
            db.Init("Lair Ghost Paladin",
                new State("base",
                    new Follow(0.75f, range: 1),
                    new Wander(0.25f),
                    new Shoot(8, 1, cooldown: 1000),
					new HealEntity(5, "Lair Ghost", cooldown: 5000)
					),
                new ItemLoot("Health Potion", 0.25f),
                new ItemLoot("Magic Potion", 0.25f)
            );
            db.Init("Lair Ghost Rogue",
                new State("base",
                    new Follow(0.75f, range: 1),
                    new Wander(0.25f),
                    new Shoot(8, 1, cooldown: 1000)
                    ),
                new ItemLoot("Health Potion", 0.25f),
                new ItemLoot("Magic Potion", 0.25f)
            );
            db.Init("Lair Ghost Warrior",
                new State("base",
                    new Follow(0.75f, range: 1),
                    new Wander(0.25f),
                    new Shoot(8, 1, cooldown: 1000)
                    ),
                new ItemLoot("Health Potion", 0.25f),
                new ItemLoot("Magic Potion", 0.25f)
            );

            db.Init("Lair Skeleton",
                new State("base",
                    new Shoot(6, cooldown: 250),
                    new Prioritize(
                        new Follow(1, range: 1),
                        new Wander(0.4f)
                        )
                    ),
                new ItemLoot("Health Potion", 0.05f),
                new ItemLoot("Magic Potion", 0.05f)
            );
            db.Init("Lair Skeleton King",
                new State("base",
                    new Shoot(10, 3, shootAngle: 10, cooldown: 250),
                    new Prioritize(
                        new Follow(1, range: 7),
                        new Wander(0.4f)
                        )
                    ),
                new TierLoot(5, LootType.Armor, 0.2f),
                new Threshold(0.5f,
                    new TierLoot(6, LootType.Weapon, 0.2f),
                    new TierLoot(7, LootType.Weapon, 0.1f),
                    new TierLoot(8, LootType.Weapon, 0.05f),
                    new TierLoot(6, LootType.Armor, 0.1f),
                    new TierLoot(7, LootType.Armor, 0.05f),
                    new TierLoot(3, LootType.Ring, 0.1f),
                    new TierLoot(3, LootType.Ability, 0.1f)
                    )
            );
            db.Init("Lair Skeleton Mage",
                new State("base",
                    new Shoot(10, cooldown: 350),
                    new Prioritize(
                        new Follow(1, range: 7),
                        new Wander(0.4f)
                        )
                    ),
                new ItemLoot("Health Potion", 0.05f),
                new ItemLoot("Magic Potion", 0.05f)
            );
            db.Init("Lair Skeleton Swordsman",
                new State("base",
                    new Shoot(5, cooldown: 250),
                    new Prioritize(
                        new Follow(1, range: 1),
                        new Wander(0.4f)
                        )
                    ),
                new ItemLoot("Health Potion", 0.05f),
                new ItemLoot("Magic Potion", 0.05f)
            );
            db.Init("Lair Skeleton Veteran",
                new State("base",
                    new Shoot(5, cooldown: 200),
                    new Prioritize(
                        new Follow(1, range: 1),
                        new Wander(0.4f)
                        )
                    ),
                new ItemLoot("Health Potion", 0.05f),
                new ItemLoot("Magic Potion", 0.05f)
            );
            db.Init("Lair Mummy",
                new State("base",
                    new Shoot(10, cooldown: 300),
                    new Prioritize(
                        new Follow(0.9f, range: 7),
                        new Wander(0.4f)
                        )
                    ),
                new ItemLoot("Health Potion", 0.05f),
                new ItemLoot("Magic Potion", 0.05f)
            );
            db.Init("Lair Mummy King",
                new State("base",
                    new Shoot(10, cooldown: 270),
                    new Prioritize(
                        new Follow(0.9f, range: 7),
                        new Wander(0.4f)
                        )
                    ),
                new ItemLoot("Health Potion", 0.05f),
                new ItemLoot("Magic Potion", 0.05f)
            );
            db.Init("Lair Mummy Pharaoh",
                new State("base",
                    new Shoot(10, cooldown: 250),
                    new Prioritize(
                        new Follow(0.9f, range: 7),
                        new Wander(0.4f)
                        )
                    ),
                new TierLoot(5, LootType.Armor, 0.2f),
                new Threshold(0.5f,
                    new TierLoot(6, LootType.Weapon, 0.2f),
                    new TierLoot(7, LootType.Weapon, 0.1f),
                    new TierLoot(8, LootType.Weapon, 0.05f),
                    new TierLoot(6, LootType.Armor, 0.1f),
                    new TierLoot(7, LootType.Armor, 0.05f),
                    new TierLoot(3, LootType.Ring, 0.1f),
                    new TierLoot(3, LootType.Ability, 0.1f)
                    )
            );

            db.Init("Lair Big Brown Slime",
                new State("base",
                    new Shoot(10, 3, shootAngle: 10, cooldown: 500),
                    new Wander(0.1f),
                    new TransformOnDeath("Lair Little Brown Slime", 1, 6, 1)
                    // new SpawnOnDeath("Lair Little Brown Slime", 1.0f, 6)
                    )
            );
            db.Init("Lair Little Brown Slime",
                new State("base",
                    new Shoot(10, 3, shootAngle: 10, cooldown: 500),
                    new Protect(0.1f, "Lair Big Brown Slime", acquireRange: 5, protectionRange: 2),
                    new Wander(0.1f)
                    ),
                new ItemLoot("Health Potion", 0.05f),
                new ItemLoot("Magic Potion", 0.05f)
            );
            db.Init("Lair Big Black Slime",
                new State("base",
                    new Shoot(10, cooldown: 1000),
                    new Wander(0.1f),
                    new TransformOnDeath("Lair Little Black Slime", 1, 4, 1)
                    //new SpawnOnDeath("Lair Medium Black Slime", 1.0f, 4)
                    )
            );
            db.Init("Lair Medium Black Slime",
                new State("base",
                    new Shoot(10, cooldown: 1000),
                    new Wander(0.1f),
                    new TransformOnDeath("Lair Little Black Slime", 1, 4, 1)
                    // new SpawnOnDeath("Lair Little Black Slime", 1.0f, 4)
                    )
            );
            db.Init("Lair Little Black Slime",
                new State("base",
                    new Shoot(10, cooldown: 1000),
                    new Wander(0.1f)
                    ),
                new ItemLoot("Health Potion", 0.05f),
                new ItemLoot("Magic Potion", 0.05f)
            );

            db.Init("Lair Construct Giant",
                new State("base",
                    new Prioritize(
                        new Follow(0.8f, range: 7),
                        new Wander(0.4f)
                        ),
                    new Shoot(10, 3, shootAngle: 20, cooldown: 1000),
                    new Shoot(10, index: 1, cooldown: 1000)
                    ),
                new TierLoot(5, LootType.Armor, 0.2f),
                new Threshold(0.5f,
                    new TierLoot(6, LootType.Weapon, 0.2f),
                    new TierLoot(7, LootType.Weapon, 0.1f),
                    new TierLoot(8, LootType.Weapon, 0.05f),
                    new TierLoot(6, LootType.Armor, 0.1f),
                    new TierLoot(7, LootType.Armor, 0.05f),
                    new TierLoot(3, LootType.Ring, 0.1f),
                    new TierLoot(3, LootType.Ability, 0.1f)
                    )
            );
            db.Init("Lair Construct Titan",
                new State("base",
                    new Prioritize(
                        new Follow(0.8f, range: 7),
                        new Wander(0.4f)
                        ),
                    new Shoot(10, 3, shootAngle: 20, cooldown: 1000),
                    new Shoot(10, 3, shootAngle: 20, index: 1, cooldownOffset: 100, cooldown: 2000)
                    ),
                new TierLoot(5, LootType.Armor, 0.2f),
                new Threshold(0.5f,
                    new TierLoot(6, LootType.Weapon, 0.2f),
                    new TierLoot(7, LootType.Weapon, 0.1f),
                    new TierLoot(8, LootType.Weapon, 0.05f),
                    new TierLoot(6, LootType.Armor, 0.1f),
                    new TierLoot(7, LootType.Armor, 0.05f),
                    new TierLoot(3, LootType.Ring, 0.1f),
                    new TierLoot(3, LootType.Ability, 0.1f)
                    )
            );

            db.Init("Lair Brown Bat",
                new State("base",
                    new Wander(0.1f),
                    new Charge(3, 8, 2000),
                    new Shoot(3, cooldown: 1000)
                    ),
                new ItemLoot("Health Potion", 0.05f),
                new ItemLoot("Magic Potion", 0.05f)
            );
            db.Init("Lair Ghost Bat",
                new State("base",
                    new Wander(0.1f),
                    new Charge(3, 8, 2000),
                    new Shoot(3, cooldown: 1000)
                    ),
                new ItemLoot("Health Potion", 0.05f),
                new ItemLoot("Magic Potion", 0.05f)
            );

            db.Init("Lair Reaper",
                new State("base",
                    new Shoot(3, cooldown: 100),
                    new Follow(1.3f, range: 1),
                    new Wander(0.1f)
                    ),
                new TierLoot(5, LootType.Armor, 0.2f),
                new Threshold(0.5f,
                    new TierLoot(6, LootType.Weapon, 0.2f),
                    new TierLoot(7, LootType.Weapon, 0.1f),
                    new TierLoot(8, LootType.Weapon, 0.05f),
                    new TierLoot(6, LootType.Armor, 0.1f),
                    new TierLoot(7, LootType.Armor, 0.05f),
                    new TierLoot(3, LootType.Ring, 0.1f),
                    new TierLoot(3, LootType.Ability, 0.1f)
                    )
            );
            db.Init("Lair Vampire",
                new State("base",
                    new Shoot(10, cooldown: 500),
                    new Shoot(3, cooldown: 1000),
                    new Follow(1.3f, range: 1),
                    new Wander(0.1f)
                    ),
                new ItemLoot("Health Potion", 0.05f),
                new ItemLoot("Magic Potion", 0.05f)
            );
            db.Init("Lair Vampire King",
                new State("base",
                    new Shoot(10, cooldown: 500),
                    new Shoot(3, cooldown: 1000),
                    new Follow(1.3f, range: 1),
                    new Wander(0.1f)
                    ),
                new TierLoot(5, LootType.Armor, 0.2f),
                new Threshold(0.5f,
                    new TierLoot(6, LootType.Weapon, 0.2f),
                    new TierLoot(7, LootType.Weapon, 0.1f),
                    new TierLoot(8, LootType.Weapon, 0.05f),
                    new TierLoot(6, LootType.Armor, 0.1f),
                    new TierLoot(7, LootType.Armor, 0.05f),
                    new TierLoot(3, LootType.Ring, 0.1f),
                    new TierLoot(3, LootType.Ability, 0.1f)
                    )
            );

            db.Init("Lair Grey Spectre",
                new State("base",
                    new Wander(0.1f),
                    new Shoot(10, cooldown: 1000),
                    new Grenade(2.5f, 50, 8, cooldown: 1000)
                    )
            );
            db.Init("Lair Blue Spectre",
                new State("base",
                    new Wander(0.1f),
                    new Shoot(10, cooldown: 1000),
                    new Grenade(2.5f, 70, 8, cooldown: 1000)
                    )
            );
            db.Init("Lair White Spectre",
                new State("base",
                    new Wander(0.1f),
                    new Shoot(10, cooldown: 1000),
                    new Grenade(2.5f, 90, 8, cooldown: 1000)
                    ),
                new Threshold(0.5f,
                    new TierLoot(4, LootType.Ability, 0.15f)
                    )
            );
           db.Init("Lair Burst Trap",
                new State("base",
                    new State("FinnaBustANut",
                    new PlayerWithinTransition(3, "Aaa")
                        ),
                    new State("Aaa",
                       new Shoot(8.4f, count: 12, index: 0, cooldownOffset: 20),
                       new Suicide(25)
                    )));
           db.Init("Lair Blast Trap",
                new State("base",
                    new TransitionFrom("base", "FinnaBustANut"),
                    new State("FinnaBustANut",
                        new PlayerWithinTransition(3, "Aaa")
                    ),
                    new State("Aaa",
                        new Shoot(25, index: 0, count: 12, cooldownOffset: 20, cooldown: 3000),
                        new Suicide(25)
                    ))

            );
        }
    }
}
