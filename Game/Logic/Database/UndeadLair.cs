
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
                new State("base",
                    new DropPortalOnDeath(target: "Glowing Realm Portal"),
                    new TransitionFrom("base", "Ini"),
					new State("Ini",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new PlayerWithinTransition(8, "transition1", seeInvis: true)
                    ),
                    new State("transition1",
                        new Wander(speed: 0.1f),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0x00FF00, 0.25f, 12),
                        new TimedTransition("spiral", 3000)
                        ),
                    new State("transition2",
                        new Wander(speed: 0.1f),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0x00FF00, 0.25f, 12),
                        new TimedTransition("ring", 3000)
                        ),
                    new State("transition3",
                        new Wander(speed: 0.1f),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0x00FF00, 0.25f, 12),
                        new TimedTransition("quiet", 3000)
                        ),
                    new State("transition4",
                        new Wander(speed: 0.1f),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0x00FF00, 0.25f, 12),
                        new TimedTransition("spawn", 3000)
                        ),
                    new State("spiral",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable,duration:3000),
                        new Wander(speed: 0.1f),
                        //new Shoot(24, count: 3, shootAngle: 15, index: 4, cooldown: 500),
                        new Spawn("Lair Ghost Archer", 1, 1),
                        new Spawn("Lair Ghost Knight", 2, 2),
                        new Spawn("Lair Ghost Mage", 1, 1),
                        new Spawn("Lair Ghost Rogue", 2, 2),
                        new Spawn("Lair Ghost Paladin", 1, 1),
                        new Spawn("Lair Ghost Warrior", 2, 2),
                        new Shoot(10, 3, fixedAngle: 0,rotateAngle:15,index:0, cooldownOffset: 0, cooldown: 200),
                          new Shoot(10,1,index:4,predictive:0.30f,cooldown:1000),
                        new TimedTransition("transition2", 5000)
                        ),
                    new State("ring",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Wander(0.4f),
                        new Shoot(10, 12, index: 4, cooldown: 2000),
                        new TimedTransition("ring2", 8000)
                        ),
                      new State("ring2",
                        new Wander(0.3f),
                        new Shoot(10, 12, index: 4, cooldown: 2000),
                        new TimedTransition("ring3", 4000)
                        ),
                       new State("ring3",
                           new ConditionalEffect(ConditionEffectIndex.Invulnerable,duration:2000),
                        new Wander(0.3f),
                         new Spawn("Lair Ghost Archer", 1, 1),
                        new Spawn("Lair Ghost Knight", 2, 2),
                        new Spawn("Lair Ghost Mage", 1, 1),
                        new Spawn("Lair Ghost Rogue", 2, 2),
                        new Spawn("Lair Ghost Paladin", 1, 1),
                        new Spawn("Lair Ghost Warrior", 2, 2),
                        new Shoot(24, count: 3, shootAngle: 15,predictive:0.40f, index: 4, cooldown: 600),
                        new TimedTransition("ring4", 4000)
                        ),
                        new State("ring4",
                           new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Taunt("r4"),
                        new Wander(0.3f),
                         new Spawn("Lair Ghost Archer", 1, 1),
                        new Spawn("Lair Ghost Knight", 2, 2),
                        new Spawn("Lair Ghost Mage", 1, 1),
                        new Spawn("Lair Ghost Rogue", 2, 2),
                        new Spawn("Lair Ghost Paladin", 1, 1),
                        new Spawn("Lair Ghost Warrior", 2, 2),
                        new Shoot(24, count: 3, shootAngle: 15, predictive: 0.50f, index: 4, cooldown: 600),
                        new TimedTransition("transition3", 6000)
                        ),
                    new State("quiet",
                       new ConditionalEffect(ConditionEffectIndex.Armored,duration:5000),
                       new Taunt("quiet"),
                       new Flash(0x00FF00, 0.25f, 12),
                        new Wander(0.1f),
                        new Shoot(24, count: 3, shootAngle: 15, index: 2, cooldown: 600),
                        new Shoot(10, 8, index: 1, cooldown: 1000),
                        new Shoot(10, 8, index: 1, cooldownOffset: 500, angleOffset: 22.5f, cooldown: 1000),
                        new TimedTransition("transition4", 5000)
                        ),
                    new State("spawn",
						new Wander(0.1f),
                        new Spawn("Ghost Mage of Septavius", 2, 2),
                        new Spawn("Ghost Rogue of Septavius", 2, 2),
                        new Spawn("Ghost Warrior of Septavius", 2, 2),
                        new Reproduce("Ghost Mage of Septavius", densityMax: 2, cooldown: 1000),
                        new Reproduce("Ghost Rogue of Septavius", densityMax: 2, cooldown: 1000),
                        new Reproduce("Ghost Warrior of Septavius", densityMax: 2, cooldown: 1000),
                        new Shoot(8, 3, shootAngle: 10, index: 4, cooldown: 1000),
                        new TimedTransition("transition1", 6000)
                        )
                ),
                new Threshold(0.0001f,
                    new ItemLoot("Undead Lair Key", 0.03f),
                    new ItemLoot("Doom Bow", 0.004f),
                    new ItemLoot("Wine Cellar Incantation", 0.05f),
                    new ItemLoot("Potion of Wisdom", 0.3f, 3),
                    new TierLoot(3, LootType.Ring, 0.25f),
                    new TierLoot(4, LootType.Ring, 0.125f),
                    new TierLoot(7, LootType.Weapon, 0.25f),
                    new TierLoot(8, LootType.Weapon, 0.25f),
                    new TierLoot(3, LootType.Ability, 0.25f),
                    new TierLoot(4, LootType.Ability, 0.125f),
                    new TierLoot(5, LootType.Ability, 0.0625f)
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
                    new Shoot(6),
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
                    new Shoot(10, 3, shootAngle: 10),
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
                    new Shoot(10),
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
                    new Shoot(5),
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
                    new Shoot(5),
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
                    new Shoot(10),
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
                    new Shoot(10),
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
                    new Shoot(10),
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
                    new Shoot(3),
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
                       new Shoot(8.4f, count: 12, index: 0),
                       new Suicide()
                    )));
           db.Init("Lair Blast Trap",
                new State("base",
                    new TransitionFrom("base", "FinnaBustANut"),
                    new State("FinnaBustANut",
                        new PlayerWithinTransition(3, "Aaa")
                    ),
                    new State("Aaa",
                        new Shoot(25, index: 0, count: 12, cooldown: 3000),
                        new Suicide()
                    ))

            );
        }
    }
}
