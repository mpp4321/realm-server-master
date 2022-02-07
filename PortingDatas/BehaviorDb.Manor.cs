#region

using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

#endregion

namespace RotMG.Game.Logic.Database
{
    class Manor
    {
        private _ Manor = () => Behav()
        //lord ruthven is waaay unfinished
                    db.Init("Lord Ruthven",
                new State("base",
                    new RealmPortalDrop(),
                    new State("default",
                        new PlayerWithinTransition(8, "spooksters")
                        ),
                    new State("spooksters",
                        new Wander(0.2f),
                        new Shoot(10, count: 5, shootAngle: 2, index: 0, cooldown: 900),
                        new TimedTransition("spooksters2", 6000)
                        ),
                    new State("spooksters2",
                        new Wander(0.15f),
                        new Shoot(8.4f, count: 40, index: 1, cooldown: 2750),
                        new Shoot(10, count: 5, shootAngle: 2, index: 0, cooldown: 900),
                        new TimedTransition("spooksters3", 4000)
                        ),
                    new State("spooksters3",
                        new HealSelf(cooldown: 1250),
                        new Shoot(8.4f, count: 40, index: 1, cooldown: 2750),
                        new TimedTransition("spooksters", 4000)
                        )
                    ),
                new MostDamagers(3,
                        LootTemplates.Sor2Perc()
                    ),
                new Threshold(0.025f,
                    new TierLoot(9, LootType.Weapon, 0.1f),
                    new TierLoot(4, LootType.Ability, 0.1f),
                    new TierLoot(9, LootType.Armor, 0.1f),
                    new TierLoot(3, LootType.Ring, 0.05f),
                    new TierLoot(10, LootType.Armor, 0.05f),
                    new TierLoot(10, LootType.Weapon, 0.05f),
                    new TierLoot(4, LootType.Ring, 0.025f),
                    new ItemLoot("Potion of Attack", 1),
                    new ItemLoot("Holy Water", 0.5f),
                    new ItemLoot("Death Tarot Card", 0.05f),
                    new ItemLoot("Chasuble of Holy Light", 0.01f),
                    new ItemLoot("St. Abraham's Wand", 0.01f),
                    new ItemLoot("Unholy Quiver", 0.001f),
                    new ItemLoot("Unholy Wand", 0.001f),
                    new ItemLoot("Tome of Purification", 0.001f),
                    new ItemLoot("Ring of Divine Faith", 0.01f),
                    new ItemLoot("Bone Dagger", 0.08f)
                    )
            )
            db.Init("Hellhound",
                new State("base",
                    new Follow(1.25f, 8, 1, cooldown: 275),
                    new Shoot(10, count: 5, shootAngle: 7, cooldown: 2000)
                    ),
                new ItemLoot("Magic Potion", 0.05f),
                new Threshold(0.5f,
                    new ItemLoot("Timelock Orb", 0.01f)
                    )
            )
                    db.Init("Vampire Bat Swarmer",
                new State("base",
                    new Follow(1.5f, 8, 1),
                    new Shoot(10, count: 1, cooldown: 6)
                    )
            )
                    db.Init("Lil Feratu",
                new State("base",
                    new Follow(0.35f, 8, 1),
                    new Shoot(10, count: 6, shootAngle: 2, cooldown: 900)
                    ),
                new ItemLoot("Health Potion", 0.05f),
                new Threshold(0.5f,
                    new ItemLoot("Steel Helm", 0.01f)
                    )
            )
                            db.Init("Lesser Bald Vampire",
                new State("base",
                    new Follow(0.35f, 8, 1),
                    new Shoot(10, count: 5, shootAngle: 6, cooldown: 1000)
                    ),
                new ItemLoot("Health Potion", 0.05f),
                new Threshold(0.5f,
                    new ItemLoot("Steel Helm", 0.01f)
                    )
            )
                  db.Init("Nosferatu",
                new State("base",
                    new Wander(0.25f),
                    new Shoot(10, count: 5, shootAngle: 2, index: 1, cooldown: 1000),
                    new Shoot(10, count: 6, shootAngle: 90, index: 0, cooldown: 1500)
                    ),
                new ItemLoot("Health Potion", 0.05f),
                new Threshold(0.5f,
                    new ItemLoot("Bone Dagger", 0.01f),
                    new ItemLoot("Wand of Death", 0.05f),
                    new ItemLoot("Golden Bow", 0.04f),
                    new ItemLoot("Steel Helm", 0.05f),
                    new ItemLoot("Ring of Paramount Defense", 0.09f)
                    )
            )
                          db.Init("Armor Guard",
                new State("base",
                    new Wander(0.2f),
                    new TossObject("RockBomb", 7, cooldown: 3000),
                    new Shoot(10, count: 1, index: 0, predictive: 7, cooldown: 1000),
                    new Shoot(10, count: 1, index: 1, cooldown: 750)
                    ),
                new ItemLoot("Magic Potion", 0.05f),
                new Threshold(0.5f,
                    new ItemLoot("Glass Sword", 0.01f),
                    new ItemLoot("Staff of Destruction", 0.01f),
                    new ItemLoot("Golden Shield", 0.01f),
                    new ItemLoot("Ring of Paramount Speed", 0.01f)
                    )
            )
                                  db.Init("Coffin Creature",
                new State("base",
                    new Spawn("Lil Feratu", initialSpawn: 1, maxChildren: 2, cooldown: 2250),
                    new Shoot(10, count: 1, index: 0, cooldown: 700)
                    ),
                new ItemLoot("Magic Potion", 0.05f)
            )
                              db.Init("RockBomb",
                        new State("base",
                    new State("BOUTTOEXPLODE",
                    new TimedTransition("boom", 1111)
                        ),
                    new State("boom",
                        new Shoot(8.4f, count: 1, fixedAngle: 0, index: 0, cooldown: 1000),
                        new Shoot(8.4f, count: 1, fixedAngle: 90, index: 0, cooldown: 1000),
                        new Shoot(8.4f, count: 1, fixedAngle: 180, index: 0, cooldown: 1000),
                        new Shoot(8.4f, count: 1, fixedAngle: 270, index: 0, cooldown: 1000),
                        new Shoot(8.4f, count: 1, fixedAngle: 45, index: 0, cooldown: 1000),
                        new Shoot(8.4f, count: 1, fixedAngle: 135, index: 0, cooldown: 1000),
                        new Shoot(8.4f, count: 1, fixedAngle: 235, index: 0, cooldown: 1000),
                        new Shoot(8.4f, count: 1, fixedAngle: 315, index: 0, cooldown: 1000),
                       new Suicide()
                    )
            )
    )
           db.Init("Coffin",
                        new State("base",
                    new State("Coffin1",
                        new HealthTransition(0.75f, "Coffin2")
                        ),
                    new State("Coffin2",
                        new Spawn("Vampire Bat Swarmer", initialSpawn: 1, maxChildren: 15, cooldown: 99999),
                         new HealthTransition(0.40f, "Coffin3")
                        ),
                       new State("Coffin3",
                           new Spawn("Vampire Bat Swarmer", initialSpawn: 1, maxChildren: 8, cooldown: 99999),
                            new Spawn("Nosferatu", initialSpawn: 1, maxChildren: 2, cooldown: 99999)
                        )
                ),
                new Threshold(0.5f,
                    new ItemLoot("Holy Water", 1.00f),
                     new ItemLoot("Potion of Attack", 0.5f),
                     new ItemLoot("Chasuble of Holy Light", 0.01f),
                     new ItemLoot("St. Abraham's Wand", 0.01f),
                     new ItemLoot("Tome of Purification", 0.001f),
                     new ItemLoot("Ring of Divine Faith", 0.01f),
                     new ItemLoot("Bone Dagger", 0.08f),
                     new TierLoot(7, LootType.Weapon, 0.05f),
                     new TierLoot(6, LootType.Armor, 0.2f),
                     new TierLoot(4, LootType.Ability, 0.15f)
                    )
            );
    }
}
