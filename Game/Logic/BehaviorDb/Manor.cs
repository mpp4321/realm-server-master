using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RotMG.Game.Logic.Loots.TierLoot;

namespace RotMG.Game.Logic.Database
{
    public class Manor : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {
            db.Init("Lord Ruthven",
                new HPScale(0.3f),
                new State("base",
                    //new RealmPortalDrop(),
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
                    //new ItemLoot("Unholy Quiver", 0.001f),
                    //new ItemLoot("Unholy Wand", 0.001f),
                    new ItemLoot("Tome of Purification", 0.001f),
                    new ItemLoot("Ring of Divine Faith", 0.005f),
                    new ItemLoot("Bone Dagger", 0.08f)
                    )
            );
            db.Init("Hellhound",
                new State("base",
                    new Follow(1.25f, 8, 1, cooldown: 275),
                    new Shoot(10, count: 5, shootAngle: 7, cooldown: 2000)
                    ),
                new ItemLoot("Magic Potion", 0.05f),
                new Threshold(0.5f,
                    new ItemLoot("Timelock Orb", 0.01f, r: new LootDef.RarityModifiedData(1.2f, 1, true))
                )
            );
            db.Init("Vampire Bat Swarmer",
                    new State("base",
                    new Follow(1.5f, 8, 1),
                    new Shoot(10, count: 1, cooldown: 6)
                )
            );
            db.Init("Lil Feratu",
                new State("base",
                    new Follow(0.35f, 8, 1),
                    new Shoot(10, count: 6, shootAngle: 2, cooldown: 900)
                    ),
                new ItemLoot("Health Potion", 0.05f),
                new Threshold(0.5f,
                    new ItemLoot("Steel Helm", 0.01f)
                    )
            );
            db.Init("Lesser Bald Vampire",
                new State("base",
                    new Follow(0.35f, 8, 1),
                    new Shoot(10, count: 5, shootAngle: 6, cooldown: 1000)
                    ),
                new ItemLoot("Health Potion", 0.05f),
                new Threshold(0.5f,
                    new ItemLoot("Steel Helm", 0.01f)
                )
            );
            db.Init("Nosferatu",
              new State("base",
                  new Wander(0.25f),
                  new Shoot(10, count: 5, shootAngle: 2, index: 1, cooldown: 1000),
                  new Shoot(10, count: 6, shootAngle: 90, index: 0, cooldown: 1500)
              ),
              new ItemLoot("Health Potion", 0.05f),
              new Threshold(0.5f,
                  new ItemLoot("Bone Dagger", 0.01f),
                  new ItemLoot("Wand of Death", 0.05f, r: new LootDef.RarityModifiedData(1.2f, 1, true)),
                  new ItemLoot("Steel Helm", 0.05f, r: new LootDef.RarityModifiedData(1.2f, 1, true)),
                  new ItemLoot("Ring of Paramount Defense", 0.09f, r: new LootDef.RarityModifiedData(1.2f, 1, true))
              )
            );
            db.Init("Armor Guard",
                 new State("base",
                     new Wander(0.2f),
                     new TossObject("RockBomb", 7, cooldown: 3000),
                     new Shoot(10, count: 1, index: 0, predictive: 7, cooldown: 1000),
                     new Shoot(10, count: 1, index: 1, cooldown: 750)
                     ),
                 new ItemLoot("Magic Potion", 0.05f),
                 new Threshold(0.5f,
                     new ItemLoot("Glass Sword", 0.01f, r: new LootDef.RarityModifiedData(1.2f, 1, true)),
                     new ItemLoot("Golden Shield", 0.01f, r: new LootDef.RarityModifiedData(1.2f, 1, true)),
                     new ItemLoot("Ring of Paramount Speed", 0.01f, r: new LootDef.RarityModifiedData(1.2f, 1, true))
                     )
             );
            db.Init("Coffin Creature",
                new State("base",
                    new Spawn("Lil Feratu", initialSpawn: 1, maxChildren: 2, cooldown: 2250),
                    new Shoot(10, count: 1, index: 0, cooldown: 700)
                ),
                new ItemLoot("Magic Potion", 0.05f)
            );
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
            );
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
                      new ItemLoot("Realm Equipment Crystal", 0.4f),
                      new ItemLoot("Tome of Purification", 0.001f),
                      new ItemLoot("Ring of Divine Faith", 0.01f),
                      new ItemLoot("Bone Dagger", 0.08f),
                      new TierLoot(7, LootType.Weapon, 0.05f),
                      new TierLoot(6, LootType.Armor, 0.2f),
                      new TierLoot(4, LootType.Ability, 0.15f)
                     )
             );

            // Special manor enemy here

            db.Init("Amal Dragon",

                new State("find players",
                    new PlayerWithinTransition(10f, "play", true)
                ),
                new State("play",
                    new Shoot(5f, 3, 10f, 0, cooldown: 800),
                    new Shoot(20f, 8, 45f, fixedAngle: 0f, cooldown: 2000, index: 2),
                    new Follow(0.5f, 10f, 2f),
                    new Spawn("Adult White Dragon", 5, 0.2, 1f, cooldown: new wServer.logic.Cooldown(3000, 2000), false, 3.0f),
                    new TimedTransition("fly", 10000)
                ),
                new State("fly", 
                    new OrbitSpawn(1.0f, 8f, 99f),
                    new Shoot(5f, 3, 10f, 0, cooldown: 800),
                    new Shoot(10f, 6, 5, cooldown: 1000, cooldownVariance: 300, index: 1),
                    new Shoot(20f, 8, 45f, fixedAngle: 0f, cooldown: 5000, index: 2),
                    new Spawn("Adult White Dragon", 5, 0.2, 1f, cooldown: new wServer.logic.Cooldown(3000, 2000), false, 3.0f),
                    new TimedTransition("play", 10000)
                ),

                new DropPortalOnDeath("Butcher's House Portal", 0.5f, dist: 2.0f),
                new DropPortalOnDeath("Manor of the Immortals Portal", 1.0f, dist: 2.0f),

                new Threshold(0.01f, 
                 new ItemLoot("Potion of Defense", 1.0f, min: 3),
                 new ItemLoot("Realm Equipment Crystal", 0.4f),
                 new ItemLoot("Potion of Defense", 1.0f, min: 3),
                 new ItemLoot("Potion of Mana", 1.0f),
                 new ItemLoot("Prism of Redirection", 0.01f)
                )
               );

        }
    }
}
