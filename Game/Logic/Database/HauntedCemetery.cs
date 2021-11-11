using RotMG.Common;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Conditionals;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;

namespace RotMG.Game.Logic.Database
{
    public class HauntedCemetery : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {
            db.Init("Arena Skeleton",
                new Prioritize(
                    new Follow(0.45f, 8, 1),
                    new Wander(0.35f)
                ),
                new Shoot(10, 1, index: 0, predictive: 0.5f, cooldown: 1200),
                new Shoot(10, 1, index: 0, cooldown: 900),
                new ItemLoot("Spirit Salve Tome", 0.02f),
                new Threshold(0.5f,
                    new ItemLoot("Ring of Greater Dexterity", 0.01f),
                    new ItemLoot("Magesteel Quiver", 0.01f)
                )
            );
            db.Init("Troll 1",
                new Follow(0.4f, 8, 1),
                new Shoot(10, 1, index: 0, cooldown: 850),
                new ItemLoot("Tome of Renewing", 0.02f),
                new Threshold(0.5f,
                    new ItemLoot("Ring of Greater Defense", 0.01f),
                    new ItemLoot("Magesteel Quiver", 0.01f)
                )
            );
            db.Init("Troll 2",
                new Follow(0.3f, 8, 1),
                new Shoot(10, 1, index: 0, cooldown: 850),
                new Grenade(3.5f, 80, 8, cooldown: 2750),
                new ItemLoot("Ravenheart Sword", 0.02f),
                new Threshold(0.5f,
                    new ItemLoot("Ring of Greater Health", 0.01f),
                    new ItemLoot("Steel Shield", 0.01f)
                )
            );
            db.Init("Arena Mushroom",
                new State("Mush1",
                    new TimedTransition( "Mush2", 1000)
                ),
                new State("Mush2",
                    new SetAltTexture(1),
                    new TimedTransition( "Mush3", 1000)
                ),
                new State("Mush3",
                    new SetAltTexture(2),
                    new TimedTransition( "die", 1000)
                ),
                new State("die",
                    new Shoot(8.4f, 1, fixedAngle: 0, index: 0, cooldown: 1000),
                    new Shoot(8.4f, 1, fixedAngle: 90, index: 0, cooldown: 1000),
                    new Shoot(8.4f, 1, fixedAngle: 180, index: 0, cooldown: 1000),
                    new Shoot(8.4f, 1, fixedAngle: 270, index: 0, cooldown: 1000),
                    new Shoot(8.4f, 1, fixedAngle: 45, index: 0, cooldown: 1000),
                    new Shoot(8.4f, 1, fixedAngle: 135, index: 0, cooldown: 1000),
                    new Shoot(8.4f, 1, fixedAngle: 235, index: 0, cooldown: 1000),
                    new Shoot(8.4f, 1, fixedAngle: 315, index: 0, cooldown: 1000),
                    new Suicide()
                )
            );
            db.Init("Troll 3",
                new State("trololo",
                    new PlayerWithinTransition(8, "move"),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable)
                ),
                new State("move",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new MoveTo(1, 22, 18),
                    new TimedTransition( "trolled", 850)
                ),
                new State("trolled",
                    new TossObject("Arena Mushroom", 3, 0, 5000),
                    new TossObject("Arena Mushroom", 3, 90, 5000),
                    new TossObject("Arena Mushroom", 3, 180, 5000),
                    new TossObject("Arena Mushroom", 3, 270, 5000),
                    new Shoot(10, 6, index: 0, cooldown: 1250),
                    new Shoot(10, 1, index: 1, cooldown: 1000),
                    new TimedTransition( "skilitins", 5000)
                ),
                new State("skilitins",
                    new TossObject("Arena Mushroom", 3, 45, 5000),
                    new TossObject("Arena Mushroom", 3, 315, 5000),
                    new TossObject("Arena Mushroom", 3, 225, 5000),
                    new TossObject("Arena Mushroom", 3, 135, 5000),
                    new Shoot(10, predictive: 1, cooldown: 1750),
                    new TossObject("Arena Skeleton", 3, cooldown: 1000),
                    new TimedTransition( "skelcheck", 4750)
                ),
                new State("skelcheck",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Shoot(10, predictive: 1, cooldown: 1750),
                    new TossObject("Arena Skeleton", 3, cooldown: 3000),
                    new EntitiesNotExistsTransition(9999, "trolled", "Arena Skeleton")
                ),
                new Threshold(0.01f,
                    new TierLoot(8, TierLoot.LootType.Weapon, 0.1f),
                    new TierLoot(5, TierLoot.LootType.Ability, 0.1f),
                    new TierLoot(9, TierLoot.LootType.Armor, 0.1f),
                    new TierLoot(3, TierLoot.LootType.Ring, 0.05f),
                    new TierLoot(10, TierLoot.LootType.Armor, 0.05f),
                    new TierLoot(10, TierLoot.LootType.Weapon, 0.05f),
                    new TierLoot(4, TierLoot.LootType.Ring, 0.025f),
                    new ItemLoot("Potion of Speed", 1),
                    new ItemLoot("Potion of Wisdom", 0.5f)
                )
                // new Threshold(0.05f,
                    // new ItemLoot("Tome of Unholy Prayers", Loot.GLChanceDungeon)
                // )
            );
            db.Init("Arena Possessed Girl",
                new Prioritize(
                    new Follow(0.55f, 8, 1),
                    new Wander(0.35f)
                ),
                new Shoot(10, 8, index: 0, cooldown: 1250),
                new Threshold(0.5f,
                    new ItemLoot("Ring of Superior Magic", 0.01f),
                    new ItemLoot("Magesteel Quiver", 0.01f)
                )
            );
            db.Init("Arena Ghost 1",
                new Follow(0.4f, 8, 1),
                new Shoot(10, 1, index: 0, predictive: 2.5f, cooldown: 1200),
                new Shoot(10, 1, index: 0, cooldown: 900),
                new Threshold(0.5f,
                    new ItemLoot("Magesteel Quiver", 0.01f)
                )
            );
            db.Init("Arena Ghost 2",
                new State("GetRekt",
                    new Follow(0.3f, 8, 1),
                    new SetAltTexture(0),
                    new Shoot(8.4f, 3, 20, 0, cooldown: 1250),
                    new TimedTransition( "Scrub", 3700)
                ),
                new State("Scrub",
                    new Wander(0.6f),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new SetAltTexture(1),
                    new TimedTransition( "GetRekt", 500)
                )
            );
            db.Init("Arena Statue Left",
                new State("GetRekt",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new SetAltTexture(1)
                ),
                new State("getem",
                    new SetAltTexture(0),
                    new Follow(0.6f, 8, 1),
                    new Shoot(8.4f, 1, index: 0, cooldown: 275)
                )
            );
            db.Init("Arena Statue Right",
                new State("GetRekt",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new SetAltTexture(1)
                ),
                new State("getem",
                    new SetAltTexture(0),
                    new Follow(0.6f, 8, 1),
                    new Shoot(8.4f, 1, index: 1, cooldown: 575),
                    new Shoot(8.4f, 3, 20)
                )
            );
            db.Init("Arena Ghost Bride",
                new State("trololo",
                    new PlayerWithinTransition(8, "moveGlory"),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable)
                ),
                new State("moveFate",
                    new SetAltTexture(0),
                    new MoveLine(1, 90),
                    new TimedTransition( "fightTillFate", 500)
                ),
                new State("moveGlory",
                    new MoveLine(1, 90),
                    new TimedTransition( "fightTillFate", 1500)
                ),
                new State("fightTillFate",
                    new BackAndForth(1, 4),
                    new SetAltTexture(0),
                    new Shoot(10, 1, index: 0, cooldown: 1100),
                    new Shoot(10, 8, index: 1, angleOffset: 43, cooldown: 1000),
                    new TimedTransition( "fightTillGlory", 5000)
                ),
                new State("fightTillGlory",
                    new BackAndForth(1, 4),
                    new SetAltTexture(0),
                    new Shoot(10, 1, index: 0, cooldown: 1100),
                    new Shoot(10, 8, index: 1, angleOffset: 43, cooldown: 1000),
                    new TimedTransition( "fightTillFate", 5000)
                ),
                new State("goByeFate",
                    //  new Order(9999, "Arena Statue Left", "getem"),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new SetAltTexture(1),
                    new EntitiesNotExistsTransition(9999, "fightTillGlory", "Arena Statue Left")
                ),
                new State("goByeGlory",
                    new SetAltTexture(1),
                    //  new Order(9999, "Arena Statue Right", "getem"),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new EntitiesNotExistsTransition(9999, "justfight", "Arena Statue Right")
                ),
                new State("justfight",
                    new Wander(.6f),
                    new Shoot(10, 6, 5, 0, cooldown: 1100),
                    new Shoot(10, 2, 5, 1, cooldown: 1000)
                ),
                new Threshold(0.001f,
                    new TierLoot(8, TierLoot.LootType.Weapon, 0.1f),
                    new TierLoot(5, TierLoot.LootType.Ability, 0.1f),
                    new TierLoot(9, TierLoot.LootType.Armor, 0.1f),
                    new TierLoot(3, TierLoot.LootType.Ring, 0.05f),
                    new TierLoot(10, TierLoot.LootType.Armor, 0.05f),
                    new TierLoot(10, TierLoot.LootType.Weapon, 0.05f),
                    new TierLoot(4, TierLoot.LootType.Ring, 0.025f),
                    new ItemLoot("Potion of Speed", 1),
                    new ItemLoot("Potion of Wisdom", 0.5f)
                    // new ItemLoot("Quiver of a Dead Hero", Loot.UTChanceDungeon)
                )
                // new Threshold(0.03f,
                    // new ItemLoot("Ancient Stone Katana", Loot.GLChanceDungeon),
                    // new ItemLoot("Amulet of Resurrection", Loot.GLChanceDungeon)
                    // )
            );
            db.Init("Arena Risen Archer",
                new StayBack(0.65f, 4),
                new Shoot(10, 1, index: 0, predictive: 0.5f, cooldown: 1200),
                new Shoot(10, 8, index: 1, cooldown: 1450)
                ,
                new ItemLoot("Spirit Salve Tome", 0.02f),
                new Threshold(0.5f,
                    new ItemLoot("Ring of Greater Dexterity", 0.01f),
                    new ItemLoot("Magesteel Quiver", 0.01f)
                )
            );
            db.Init("Arena Risen Brawler",
                new Follow(0.55f, 8, 1),
                new Shoot(10, 5, index: 0, shootAngle: 5, predictive: 0.5f, cooldown: 1200)
                ,
                new ItemLoot("Spirit Salve Tome", 0.02f),
                new Threshold(0.5f,
                    new ItemLoot("Ring of Greater Dexterity", 0.01f),
                    new ItemLoot("Magesteel Quiver", 0.01f)
                )
            );
            db.Init("Arena Risen Mummy",
                new Follow(0.55f, 8, 1),
                new Shoot(10, 8, index: 0, shootAngle: 14, predictive: 5, cooldown: 3750)
                ,
                new ItemLoot("Spirit Salve Tome", 0.02f),
                new Threshold(0.5f,
                    new ItemLoot("Ring of Superior Dexterity", 0.01f),
                    new ItemLoot("Magesteel Quiver", 0.01f)
                )
            );
            db.Init("Arena Risen Mage",
                new Follow(0.3f, 8, 1),
                new Shoot(10, 1, index: 0, cooldown: 1200),
                new HealEntity(10, "Hallowrena", cooldown: 5000)
                ,
                new ItemLoot("Magesteel Quiver", 0.02f),
                new Threshold(0.5f,
                    new ItemLoot("Ring of Greater Dexterity", 0.01f)
                )
            );
            db.Init("Arena Risen Warrior",
                new Follow(0.6f, 8, 1),
                new Shoot(10, 1, index: 0, cooldown: 10)
            );
            db.Init("Arena Blue Flame",
                new State("imgonnagetcha",
                    new Follow(0.65f, 8, 1),
                    new PlayerWithinTransition(1, "blam")
                ),
                new State("blam",
                    new Shoot(8.4f, 1, fixedAngle: 0, index: 0, cooldown: 1000),
                    new Shoot(8.4f, 1, fixedAngle: 90, index: 0, cooldown: 1000),
                    new Shoot(8.4f, 1, fixedAngle: 180, index: 0, cooldown: 1000),
                    new Shoot(8.4f, 1, fixedAngle: 270, index: 0, cooldown: 1000),
                    new Shoot(8.4f, 1, fixedAngle: 45, index: 0, cooldown: 1000),
                    new Shoot(8.4f, 1, fixedAngle: 135, index: 0, cooldown: 1000),
                    new Shoot(8.4f, 1, fixedAngle: 235, index: 0, cooldown: 1000),
                    new Shoot(8.4f, 1, fixedAngle: 315, index: 0, cooldown: 1000),
                    new Suicide()
                )
            );
            db.Init("Arena Grave Caretaker",
                new State("gravesandk",
                    new MoveLine(1, 90),
                    new TimedTransition( "attackgravesandk", 1500)
                ),
                new State("attackgravesandk",
                    new Wander(.6f),
                    new Shoot(10, 1, index: 0, cooldown: 1500),
                    new Shoot(10, 1, index: 0, predictive: 7, cooldown: 300),
                    new Shoot(10, 6, index: 1, cooldown: 1000),
                    new Spawn("Arena Blue Flame", initialSpawn: 1, maxChildren: 1, cooldown: 3750)
                ),
                new Threshold(0.001f,
                    new TierLoot(10, TierLoot.LootType.Weapon, 0.1f),
                    new TierLoot(4, TierLoot.LootType.Ability, 0.1f),
                    new TierLoot(9, TierLoot.LootType.Armor, 0.1f),
                    new TierLoot(4, TierLoot.LootType.Ring, 0.05f),
                    new TierLoot(11, TierLoot.LootType.Armor, 0.05f),
                    new TierLoot(11, TierLoot.LootType.Weapon, 0.05f),
                    new TierLoot(4, TierLoot.LootType.Ring, 0.025f),
                    new ItemLoot("Potion of Speed", 1),
                    new ItemLoot("Potion of Wisdom", 0.5f)
                    // new ItemLoot("Grave Caretaker's Jacket", Loot.UTChanceDungeon)
                )
                // new Threshold(0.02f,
                        // new ItemLoot("Lantern of the Damned", Loot.LGChanceDungeon)
                    // )
            );
            db.Init("Classic Ghost",
                new Wander(0.5f),
                new Shoot(10, 3, index: 0, shootAngle: 3, predictive: 1, cooldown: 1200),
                new ItemLoot("Timelock Orb", 0.02f),
                new Threshold(0.5f,
                    new ItemLoot("Ring of Greater Attack", 0.01f),
                    new ItemLoot("Steel Shield", 0.01f)
                )
            );

            db.Init("Werewolf Cub",
                new Follow(0.5f, 8, 1),
                new Shoot(10, 3, index: 0, cooldown: 1200),
                new ItemLoot("Magic Potion", 0.02f),
                new Threshold(0.5f,
                    new ItemLoot("Ring of Greater Attack", 0.01f),
                    new ItemLoot("Steel Shield", 0.01f)
                )
            );

            db.Init("Zombie Hulk",
                new Prioritize(
                    new Follow(0.6f, 8, 1),
                    new Wander(0.2f)
                ),
                new Shoot(10, 4, index: 0, shootAngle: 5, cooldown: 1500),
                new Shoot(10, 1, index: 1, cooldown: 2000)
                ,
                new ItemLoot("Health Potion", 0.02f),
                new Threshold(0.5f,
                    new ItemLoot("Ring of Greater Dexterity", 0.01f),
                    new ItemLoot("Magesteel Quiver", 0.01f)
                )
            );
            db.Init("Werewolf",
                new Wander(0.4f),
                new Shoot(10, 6, index: 0, shootAngle: 5, cooldown: 1500),
                new Spawn("Werewolf Cub", initialSpawn: 1, maxChildren: 3, cooldown: 7000)
                ,
                new ItemLoot("Health Potion", 0.02f),
                new Threshold(0.5f,
                    new ItemLoot("Ring of Greater Dexterity", 0.01f),
                    new ItemLoot("Magesteel Quiver", 0.01f)
                )
            );
            db.Init("Blue Zombie",
                new Follow(0.1f, 8, 1),
                new Shoot(10)
            );
            db.Init("Flying Flame Skull",
                new Swirl(0.65f, 7),
                new Shoot(10, 10, index: 0, cooldown: 500)
            );

            db.Init("Halloween Zombie Spawner",
                new State("idle",
                    new ConditionalEffect(ConditionEffectIndex.Invincible)
                ),
                new State("lmao",
                    new EntitiesNotExistsTransition(100, "die", "Ghost of Skuld")
                ),
                new State("spawn",
                    new Spawn("Blue Zombie", initialSpawn: 1, maxChildren: 1, cooldown: 6850),
                    new EntitiesNotExistsTransition(100, "die", "Ghost of Skuld")
                ),
                new State("die",
                    new Suicide()
                )
            );

            db.Init("Ghost of Skuld",
                new DropPortalOnDeath("Realm Portal"),
                new HealthTransition(0.1f, "suicidetime"),
                new State("skuld1",
                    new Order(9999, "Halloween Zombie Spawner", "spawn"),
                    new Taunt(1f, "Your reward is....A SWIFT DEATH!"),
                    new TossObject("Flying Flame Skull", 6, 270, 9999),
                    new TossObject("Flying Flame Skull", 6, 90, 9999),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new TimedTransition( "gotem", 2500)
                ),
                new State("suicidetime", new Suicide()),
                new State("gotem",
                    new Shoot(10, 5, 5, 0, cooldown: 700),
                    new Shoot(10, 1, index: 0, cooldown: 1200),
                    new Shoot(12, 1, fixedAngle: 0, index: 2, cooldown: 4000),
                    new Shoot(12, 1, fixedAngle: 90, index: 2, cooldown: 4000),
                    new Shoot(12, 1, fixedAngle: 180, index: 2, cooldown: 4000),
                    new Shoot(12, 1, fixedAngle: 270, index: 2, cooldown: 4000),
                    new TimedTransition( "telestart", 10000)
                ),
                new State("blam1",
                    new SetAltTexture(1),
                    new Shoot(10, 7, 12),
                    new Shoot(8.4f, 1, fixedAngle: 0, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 90, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 180, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 270, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 45, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 135, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 235, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 315, index: 1, cooldown: 1400),
                    new TimedTransition( "tele7", 5000)
                ),
                new State("blam2",
                    new SetAltTexture(1),
                    new Shoot(10, 7, 12),
                    new Shoot(8.4f, 1, fixedAngle: 0, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 90, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 180, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 270, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 45, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 135, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 235, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 315, index: 1, cooldown: 1400),
                    new TimedTransition( "tele5", 5000)
                ),
                new State("blam3",
                    new SetAltTexture(1),
                    new Shoot(10, 7, 12),
                    new Shoot(8.4f, 1, fixedAngle: 0, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 90, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 180, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 270, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 45, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 135, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 235, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 315, index: 1, cooldown: 1400),
                    new TimedTransition( "tele4", 5000)
                ),
                new State("blam4",
                    new SetAltTexture(1),
                    new Shoot(10, 7, 12),
                    new Shoot(8.4f, 1, fixedAngle: 0, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 90, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 180, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 270, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 45, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 135, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 235, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 315, index: 1, cooldown: 1400),
                    new TimedTransition( "tele6", 5000)
                ),
                new State("blam5",
                    new SetAltTexture(1),
                    new Shoot(10, 7, 12),
                    new Shoot(8.4f, 1, fixedAngle: 0, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 90, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 180, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 270, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 45, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 135, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 235, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 315, index: 1, cooldown: 1400),
                    new TimedTransition( "tele2", 5000)
                ),
                new State("blam6",
                    new SetAltTexture(1),
                    new Shoot(10, 7, 12),
                    new Shoot(8.4f, 1, fixedAngle: 0, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 90, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 180, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 270, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 45, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 135, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 235, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 315, index: 1, cooldown: 1400),
                    new TimedTransition( "tele3", 5000)
                ),
                new State("blam7",
                    new SetAltTexture(1),
                    new Shoot(10, 7, 12),
                    new Shoot(8.4f, 1, fixedAngle: 0, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 90, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 180, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 270, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 45, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 135, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 235, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 315, index: 1, cooldown: 1400),
                    new TimedTransition( "tele8", 5000)
                ),
                new State("blam8",
                    new SetAltTexture(1),
                    new Shoot(10, 7, 12),
                    new Shoot(8.4f, 1, fixedAngle: 0, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 90, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 180, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 270, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 45, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 135, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 235, index: 1, cooldown: 1400),
                    new Shoot(8.4f, 1, fixedAngle: 315, index: 1, cooldown: 1400),
                    new TimedTransition( "tele8", 5000)
                ),
                new State("telestart",
                    new SetAltTexture(1),
                    new TimedTransition( "tele1", 3000)
                ),
                new State("tele1",
                    new SetAltTexture(11),
                    new MoveTo(2, 24, 18),
                    new TimedTransition( "blam2", 1000)
                ),
                new State("tele2",
                    new SetAltTexture(11),
                    new MoveTo(2, 17, 26),
                    new TimedTransition( "blam3", 1000)
                ),
                new State("tele3",
                    new SetAltTexture(11),
                    new MoveTo(2, 29, 19),
                    new TimedTransition( "blam4", 1000)
                ),
                new State("tele4",
                    new SetAltTexture(11),
                    new MoveTo(2, 29, 29),
                    new TimedTransition( "blam5", 1000)
                ),
                new State("tele5",
                    new SetAltTexture(11),
                    new MoveTo(2, 24, 18),
                    new TimedTransition( "blam6", 1000)
                ),
                new State("tele6",
                    new SetAltTexture(11),
                    new MoveTo(2, 25, 35),
                    new TimedTransition( "blam7", 1000)
                ),
                new State("tele7",
                    new SetAltTexture(11),
                    new MoveTo(2, 20, 29),
                    new TimedTransition( "blam8", 1000)
                ),
                new State("tele8",
                    new SetAltTexture(11),
                    new MoveTo(2, 24, 24),
                    new TimedTransition( "gotem", 1000)
                ),
              
                // new Threshold(0.02f,
                //     new ItemLoot("Resurrected Warrior's Armor", Loot.LGChanceDungeon),
                //     new ItemLoot("Rusted Executioner", Loot.LGChanceDungeon),
                //     new ItemLoot("Plague Concoction", Loot.LGChanceDungeon),
                //     new ItemLoot("Skuld's Garments", Loot.LGChanceDungeon)
                //     ),
                // new Threshold(0.03f,
                //     new ItemLoot("The Plague Reaper", Loot.GLChanceDungeon),
                //     new ItemLoot("Plague Reaper's Attire", Loot.GLChanceDungeon)
                // ),
                // new Threshold(0.05f,
                //     new ItemLoot("Mysterious Liquid", Loot.ATChanceDungeon),
                //     new ItemLoot("Tome of Unholy Prayers", Loot.ATChanceDungeon)
                //     ),
                new Threshold(0.001f,
                    new TierLoot(11, TierLoot.LootType.Weapon, 0.1f),
                    new TierLoot(4, TierLoot.LootType.Ability, 0.1f),
                    new TierLoot(11, TierLoot.LootType.Armor, 0.1f),
                    new TierLoot(5, TierLoot.LootType.Ring, 0.05f),
                    new TierLoot(11, TierLoot.LootType.Armor, 0.05f),
                    new TierLoot(11, TierLoot.LootType.Weapon, 0.05f),
                    new TierLoot(4, TierLoot.LootType.Ring, 0.025f),
                    new ItemLoot("Potion of Vitality", 1),
                    new ItemLoot("Potion of Wisdom", 0.1f),
                    new ItemLoot("Etherite Dagger", 0.005f),
                    new ItemLoot("Ghastly Drape", 0.005f),
                    new ItemLoot("Mantle of Skuld", 0.005f)
                )
            );
            db.Init("Arena South Gate Spawner",
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("Greeting"),
                new State("arena1wave1",
                    new Spawn("Arena Skeleton", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                ),
                new State("arena1wave2",
                    new Spawn("Arena Skeleton", initialSpawn: 1, maxChildren: 3, cooldown: 90000)
                ),
                new State("arena1wave3",
                    new Spawn("Arena Skeleton", initialSpawn: 1, maxChildren: 3, cooldown: 90000),
                    new Spawn("Troll 1", initialSpawn: 1, maxChildren: 1, cooldown: 90000),
                    new Spawn("Troll 2", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                ),
                new State("arena1wave4",
                    new Spawn("Arena Skeleton", initialSpawn: 1, maxChildren: 2, cooldown: 90000),
                    new Spawn("Troll 1", initialSpawn: 1, maxChildren: 1, cooldown: 90000),
                    new Spawn("Troll 2", initialSpawn: 1, maxChildren: 2, cooldown: 90000)
                ),
                new State("arena2wave1",
                    new Spawn("Arena Ghost 1", initialSpawn: 1, maxChildren: 1, cooldown: 90000),
                    new Spawn("Arena Ghost 2", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                ),
                new State("arena2wave2",
                    new Spawn("Arena Ghost 1", initialSpawn: 1, maxChildren: 1, cooldown: 90000),
                    new Spawn("Arena Ghost 2", initialSpawn: 1, maxChildren: 1, cooldown: 90000),
                    new Spawn("Arena Possessed Girl", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                ),
                new State("arena2wave3",
                    new Spawn("Arena Ghost 1", initialSpawn: 1, maxChildren: 1, cooldown: 90000),
                    new Spawn("Arena Ghost 2", initialSpawn: 1, maxChildren: 2, cooldown: 90000),
                    new Spawn("Arena Possessed Girl", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                ),
                new State("arena2wave4",
                    new Spawn("Arena Ghost 1", initialSpawn: 1, maxChildren: 2, cooldown: 90000),
                    new Spawn("Arena Ghost 2", initialSpawn: 1, maxChildren: 2, cooldown: 90000),
                    new Spawn("Arena Possessed Girl", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                ),
                new State("arena3wave1",
                    new Spawn("Arena Risen Archer", initialSpawn: 1, maxChildren: 2, cooldown: 90000),
                    new Spawn("Arena Risen Mage", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                ),
                new State("arena3wave2",
                    new Spawn("Arena Risen Archer", initialSpawn: 1, maxChildren: 1, cooldown: 90000),
                    new Spawn("Arena Risen Warrior", initialSpawn: 1, maxChildren: 2, cooldown: 90000)
                ),
                new State("arena3wave3",
                    new Spawn("Arena Risen Mage", initialSpawn: 1, maxChildren: 1, cooldown: 90000),
                    new Spawn("Arena Risen Brawler", initialSpawn: 1, maxChildren: 2, cooldown: 90000)
                ),
                new State("arena3wave4",
                    new Spawn("Arena Risen Mage", initialSpawn: 1, maxChildren: 1, cooldown: 90000),
                    new Spawn("Arena Risen Brawler", initialSpawn: 1, maxChildren: 1, cooldown: 90000),
                    new Spawn("Arena Risen Archer", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                ),
                new State("arena4wave1",
                    new Spawn("Classic Ghost", initialSpawn: 1, maxChildren: 2, cooldown: 90000),
                    new Spawn("Werewolf", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                ),
                new State("arena4wave2",
                    new Spawn("Classic Ghost", initialSpawn: 1, maxChildren: 2, cooldown: 90000),
                    new Spawn("Zombie Hulk", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                ),
                new State("arena4wave3",
                    new Spawn("Classic Ghost", initialSpawn: 1, maxChildren: 2, cooldown: 90000),
                    new Spawn("Zombie Hulk", initialSpawn: 1, maxChildren: 1, cooldown: 90000),
                    new Spawn("Werewolf", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                ),
                new State("arena4wave4",
                    new Spawn("Zombie Hulk", initialSpawn: 2, maxChildren: 1, cooldown: 90000),
                    new Spawn("Werewolf", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                )
            );
            db.Init("Arena East Gate Spawner",
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("Greeting"),
                new State("arena1wave1",
                    new Spawn("Arena Skeleton", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                ),
                new State("arena1wave2",
                    new Spawn("Arena Skeleton", initialSpawn: 1, maxChildren: 3, cooldown: 90000)
                ),
                new State("arena1wave3",
                    new Spawn("Arena Skeleton", initialSpawn: 1, maxChildren: 3, cooldown: 90000)
                ),
                new State("arena1wave4",
                    new Spawn("Arena Skeleton", initialSpawn: 1, maxChildren: 2, cooldown: 90000),
                    new Spawn("Troll 1", initialSpawn: 1, maxChildren: 1, cooldown: 90000),
                    new Spawn("Troll 2", initialSpawn: 1, maxChildren: 2, cooldown: 90000)
                ),
                new State("arena2wave1",
                    new Spawn("Arena Ghost 1", initialSpawn: 1, maxChildren: 1, cooldown: 90000),
                    new Spawn("Arena Ghost 2", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                ),
                new State("arena2wave2",
                    new Spawn("Arena Ghost 1", initialSpawn: 1, maxChildren: 1, cooldown: 90000),
                    new Spawn("Arena Ghost 2", initialSpawn: 1, maxChildren: 1, cooldown: 90000),
                    new Spawn("Arena Possessed Girl", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                ),
                new State("arena2wave3",
                    new Spawn("Arena Ghost 1", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                ),
                new State("arena2wave4",
                    new Spawn("Arena Ghost 1", initialSpawn: 1, maxChildren: 2, cooldown: 90000)
                ),
                new State("arena3wave1",
                    new Spawn("Arena Risen Archer", initialSpawn: 1, maxChildren: 2, cooldown: 90000),
                    new Spawn("Arena Risen Mage", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                ),
                new State("arena3wave2",
                    new Spawn("Arena Risen Archer", initialSpawn: 1, maxChildren: 1, cooldown: 90000),
                    new Spawn("Arena Risen Warrior", initialSpawn: 1, maxChildren: 2, cooldown: 90000)
                ),
                new State("arena3wave3",
                    new Spawn("Arena Risen Mage", initialSpawn: 1, maxChildren: 1, cooldown: 90000),
                    new Spawn("Arena Risen Brawler", initialSpawn: 1, maxChildren: 2, cooldown: 90000)
                ),
                new State("arena3wave4",
                    new Spawn("Arena Risen Mage", initialSpawn: 1, maxChildren: 1, cooldown: 90000),
                    new Spawn("Arena Risen Brawler", initialSpawn: 1, maxChildren: 1, cooldown: 90000),
                    new Spawn("Arena Risen Archer", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                ),
                new State("arena4wave1",
                    new Spawn("Classic Ghost", initialSpawn: 1, maxChildren: 2, cooldown: 90000),
                    new Spawn("Werewolf", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                ),
                new State("arena4wave2",
                    new Spawn("Classic Ghost", initialSpawn: 1, maxChildren: 2, cooldown: 90000),
                    new Spawn("Zombie Hulk", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                ),
                new State("arena4wave3",
                    new Spawn("Classic Ghost", initialSpawn: 1, maxChildren: 2, cooldown: 90000),
                    new Spawn("Zombie Hulk", initialSpawn: 1, maxChildren: 1, cooldown: 90000),
                    new Spawn("Werewolf", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                ),
                new State("arena4wave4",
                    new Spawn("Zombie Hulk", initialSpawn: 2, maxChildren: 1, cooldown: 90000),
                    new Spawn("Werewolf", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                )
            );
            db.Init("Arena West Gate Spawner",
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("Greeting"),
                new State("arena1wave1",
                    new Spawn("Arena Skeleton", initialSpawn: 1, maxChildren: 2, cooldown: 90000)
                ),
                new State("arena1wave2",
                    new Spawn("Arena Skeleton", initialSpawn: 1, maxChildren: 3, cooldown: 90000),
                    new Spawn("Troll 1", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                ),
                new State("arena1wave3",
                    new Spawn("Arena Skeleton", initialSpawn: 1, maxChildren: 3, cooldown: 90000),
                    new Spawn("Troll 1", initialSpawn: 1, maxChildren: 1, cooldown: 90000),
                    new Spawn("Troll 2", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                ),
                new State("arena1wave4",
                    new Spawn("Arena Skeleton", initialSpawn: 1, maxChildren: 2, cooldown: 90000),
                    new Spawn("Troll 1", initialSpawn: 1, maxChildren: 1, cooldown: 90000),
                    new Spawn("Troll 2", initialSpawn: 1, maxChildren: 2, cooldown: 90000)
                ),
                new State("arena2wave1",
                    new Spawn("Arena Ghost 1", initialSpawn: 1, maxChildren: 1, cooldown: 90000),
                    new Spawn("Arena Ghost 2", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                ),
                new State("arena2wave2",
                    new Spawn("Arena Ghost 1", initialSpawn: 1, maxChildren: 1, cooldown: 90000),
                    new Spawn("Arena Ghost 2", initialSpawn: 1, maxChildren: 1, cooldown: 90000),
                    new Spawn("Arena Possessed Girl", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                ),
                new State("arena2wave3",
                    new Spawn("Arena Ghost 1", initialSpawn: 1, maxChildren: 1, cooldown: 90000),
                    new Spawn("Arena Ghost 2", initialSpawn: 1, maxChildren: 2, cooldown: 90000),
                    new Spawn("Arena Possessed Girl", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                ),
                new State("arena2wave4",
                    new Spawn("Arena Ghost 1", initialSpawn: 1, maxChildren: 2, cooldown: 90000),
                    new Spawn("Arena Ghost 2", initialSpawn: 1, maxChildren: 2, cooldown: 90000),
                    new Spawn("Arena Possessed Girl", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                ),
                new State("arena3wave1",
                    new Spawn("Arena Risen Archer", initialSpawn: 1, maxChildren: 2, cooldown: 90000),
                    new Spawn("Arena Risen Mage", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                ),
                new State("arena3wave2",
                    new Spawn("Arena Risen Archer", initialSpawn: 1, maxChildren: 1, cooldown: 90000),
                    new Spawn("Arena Risen Warrior", initialSpawn: 1, maxChildren: 2, cooldown: 90000)
                ),
                new State("arena3wave3",
                    new Spawn("Arena Risen Mage", initialSpawn: 1, maxChildren: 1, cooldown: 90000),
                    new Spawn("Arena Risen Brawler", initialSpawn: 1, maxChildren: 2, cooldown: 90000)
                ),
                new State("arena3wave4",
                    new Spawn("Arena Risen Mage", initialSpawn: 1, maxChildren: 1, cooldown: 90000),
                    new Spawn("Arena Risen Brawler", initialSpawn: 1, maxChildren: 1, cooldown: 90000),
                    new Spawn("Arena Risen Archer", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                ),
                new State("arena4wave1",
                    new Spawn("Classic Ghost", initialSpawn: 1, maxChildren: 2, cooldown: 90000),
                    new Spawn("Werewolf", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                ),
                new State("arena4wave2",
                    new Spawn("Classic Ghost", initialSpawn: 1, maxChildren: 2, cooldown: 90000),
                    new Spawn("Zombie Hulk", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                ),
                new State("arena4wave3",
                    new Spawn("Classic Ghost", initialSpawn: 1, maxChildren: 2, cooldown: 90000),
                    new Spawn("Zombie Hulk", initialSpawn: 1, maxChildren: 1, cooldown: 90000),
                    new Spawn("Werewolf", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                ),
                new State("arena4wave4",
                    new Spawn("Zombie Hulk", initialSpawn: 2, maxChildren: 1, cooldown: 90000),
                    new Spawn("Werewolf", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                )
            );
            db.Init("Arena North Gate Spawner",
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("Greeting"),
                new State("arena1wave1",
                    new Spawn("Arena Skeleton", initialSpawn: 1, maxChildren: 2, cooldown: 90000)
                ),
                new State("arena1wave2",
                    new Spawn("Troll 1", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                ),
                new State("arena1wave3",
                    new Spawn("Troll 2", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                ),
                new State("arena1wave4",
                    new Spawn("Arena Skeleton", initialSpawn: 1, maxChildren: 1, cooldown: 90000),
                    new Spawn("Troll 1", initialSpawn: 1, maxChildren: 1, cooldown: 90000),
                    new Spawn("Troll 2", initialSpawn: 1, maxChildren: 2, cooldown: 90000)
                ),
                new State("arena1boss",
                    new Spawn("Troll 3", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                ),
                new State("arena2wave1",
                    new Spawn("Arena Ghost 1", initialSpawn: 1, maxChildren: 1, cooldown: 90000),
                    new Spawn("Arena Ghost 2", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                ),
                new State("arena2wave2",
                    new Spawn("Arena Ghost 1", initialSpawn: 1, maxChildren: 1, cooldown: 90000),
                    new Spawn("Arena Ghost 2", initialSpawn: 1, maxChildren: 1, cooldown: 90000),
                    new Spawn("Arena Possessed Girl", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                ),
                new State("arena2wave3",
                    new Spawn("Arena Ghost 1", initialSpawn: 1, maxChildren: 1, cooldown: 90000),
                    new Spawn("Arena Ghost 2", initialSpawn: 1, maxChildren: 1, cooldown: 90000),
                    new Spawn("Arena Possessed Girl", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                ),
                new State("arena2wave4",
                    new Spawn("Arena Ghost 1", initialSpawn: 1, maxChildren: 2, cooldown: 90000),
                    new Spawn("Arena Ghost 2", initialSpawn: 1, maxChildren: 1, cooldown: 90000),
                    new Spawn("Arena Possessed Girl", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                ),
                new State("arena2boss",
                    new Spawn("Arena Ghost Bride", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                ),
                new State("arena3wave1",
                    new Spawn("Arena Risen Archer", initialSpawn: 1, maxChildren: 1, cooldown: 90000),
                    new Spawn("Arena Risen Mage", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                ),
                new State("arena3wave2",
                    new Spawn("Arena Risen Archer", initialSpawn: 1, maxChildren: 1, cooldown: 90000),
                    new Spawn("Arena Risen Warrior", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                ),
                new State("arena3wave3",
                    new Spawn("Arena Risen Mage", initialSpawn: 1, maxChildren: 1, cooldown: 90000),
                    new Spawn("Arena Risen Brawler", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                ),
                new State("arena3wave4",
                    new Spawn("Arena Risen Mage", initialSpawn: 1, maxChildren: 1, cooldown: 90000),
                    new Spawn("Arena Risen Brawler", initialSpawn: 1, maxChildren: 1, cooldown: 90000),
                    new Spawn("Arena Risen Archer", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                ),
                new State("arena3boss",
                    new Spawn("Arena Grave Caretaker", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                ),
                new State("arena4wave1",
                    new Spawn("Classic Ghost", initialSpawn: 1, maxChildren: 2, cooldown: 90000),
                    new Spawn("Werewolf", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                ),
                new State("arena4wave2",
                    new Spawn("Classic Ghost", initialSpawn: 1, maxChildren: 1, cooldown: 90000),
                    new Spawn("Zombie Hulk", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                ),
                new State("arena4wave3",
                    new Spawn("Classic Ghost", initialSpawn: 1, maxChildren: 1, cooldown: 90000),
                    new Spawn("Zombie Hulk", initialSpawn: 1, maxChildren: 1, cooldown: 90000),
                    new Spawn("Werewolf", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                ),
                new State("arena4wave4",
                    new Spawn("Zombie Hulk", initialSpawn: 2, maxChildren: 1, cooldown: 90000),
                    new Spawn("Werewolf", initialSpawn: 1, maxChildren: 1, cooldown: 90000)
                )
            );
            db.Init("Area 1 Controller",
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("Greeting",
                    new PlayerWithinTransition(5, "talk1")
                ),
                new State("talk1",
                    new SetAltTexture(2),
                    new Taunt(1f,
                        "Welcome to my domain. I challenge you, warrior, to defeat my undead hordes and claim your prize..."),
                    new TimedTransition( "talk3", 3000)
                ),
                new State("talk3",
                    new SetAltTexture(1),
                    new Taunt(1f, "Prepare yourself, The horde is coming!"),
                    // new PlayerTextTransition("1", "Ready", 99, false, true)
                    new TimedTransition( "1", 3000)
                ),
                new State("1",
                    new SetAltTexture(0),
                    new Order(9999, "Arena East Gate Spawner", "arena1wave1"),
                    new Order(9999, "Arena West Gate Spawner", "arena1wave1"),
                    new Order(9999, "Arena South Gate Spawner", "arena1wave1"),
                    new Order(9999, "Arena North Gate Spawner", "arena1wave1"),
                    new TimedTransition( "8", 2750)
                ),
                new State("2",
                    new SetAltTexture(0),
                    new EntitiesNotExistsTransition(9999, "3",
                        "Arena Skeleton", "Troll 1", "Troll 2"
                    )
                ),
                new State("3",
                    new SetAltTexture(1),
                    new Taunt(1f, "Only seconds until the next wave."),
                    new TimedTransition( "4", 2000)
                ),
                new State("4",
                    new SetAltTexture(0),
                    new Order(9999, "Arena East Gate Spawner", "arena1wave2"),
                    new Order(9999, "Arena West Gate Spawner", "arena1wave2"),
                    new Order(9999, "Arena South Gate Spawner", "arena1wave2"),
                    new Order(9999, "Arena North Gate Spawner", "arena1wave2"),
                    new TimedTransition( "5", 2750)
                ),
                new State("5",
                    new SetAltTexture(0),
                    new EntitiesNotExistsTransition(9999, "6", "Arena Skeleton", "Troll 1", "Troll 2")
                ),
                new State("6",
                    new SetAltTexture(1),
                    new Taunt(1f, "Only seconds until the next wave."),
                    new TimedTransition( "7", 2000)
                ),
                new State("7",
                    new SetAltTexture(0),
                    new Order(9999, "Arena East Gate Spawner", "arena1wave3"),
                    new Order(9999, "Arena West Gate Spawner", "arena1wave3"),
                    new Order(9999, "Arena South Gate Spawner", "arena1wave3"),
                    new Order(9999, "Arena North Gate Spawner", "arena1wave3"),
                    new TimedTransition( "8", 2750)
                ),
                new State("8",
                    new SetAltTexture(0),
                    new EntitiesNotExistsTransition(9999, "9", "Arena Skeleton", "Troll 1", "Troll 2")
                ),
                new State("9",
                    new SetAltTexture(1),
                    new Taunt(1f, "Only seconds until the next wave."),
                    new TimedTransition( "10", 2000)
                ),
                new State("10",
                    new SetAltTexture(0),
                    new Order(9999, "Arena East Gate Spawner", "arena1wave4"),
                    new Order(9999, "Arena West Gate Spawner", "arena1wave4"),
                    new Order(9999, "Arena South Gate Spawner", "arena1wave4"),
                    new Order(9999, "Arena North Gate Spawner", "arena1wave4"),
                    new TimedTransition( "11", 2750)
                ),
                new State("11",
                    new SetAltTexture(0),
                    new EntitiesNotExistsTransition(9999, "12", "Arena Skeleton", "Troll 1", "Troll 2")
                ),
                new State("12",
                    new SetAltTexture(1),
                    new Taunt(1f, "Only seconds until the next wave."),
                    new TimedTransition( "13", 2000)
                ),
                new State("13",
                    new SetAltTexture(0),
                    new Order(9999, "Arena North Gate Spawner", "arena1boss"),
                    new TimedTransition( "14", 2750)
                ),
                new State("14",
                    new SetAltTexture(0),
                    new EntitiesNotExistsTransition(9999, "portal", "Troll 3")
                ),
                new State("portal",
                    new SetAltTexture(0),
                    new DropPortalOnDeath("Haunted Cemetery Gates Portal", 1),
                    new Suicide()
                )
            );
            db.Init("Area 2 Controller",
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("Greeting",
                    new PlayerWithinTransition(5, "talk3")
                ),
                new State("talk3",
                    new SetAltTexture(1),
                    new Taunt(1f, "Prepare yourself, the hoard is coming!"),
                    // new PlayerTextTransition("1", "Ready", 99, false, true)
                    new TimedTransition( "1", 2000)
                ),
                new State("1",
                    new SetAltTexture(0),
                    new Order(9999, "Arena East Gate Spawner", "arena2wave1"),
                    new Order(9999, "Arena West Gate Spawner", "arena2wave1"),
                    new Order(9999, "Arena South Gate Spawner", "arena2wave1"),
                    new Order(9999, "Arena North Gate Spawner", "arena2wave1"),
                    new TimedTransition( "8", 2750)
                ),
                new State("2",
                    new SetAltTexture(0),
                    new EntitiesNotExistsTransition(9999, "3", "Arena Possessed Girl", "Arena Ghost 1",
                        "Arena Ghost 2")
                ),
                new State("3",
                    new SetAltTexture(2),
                    new Taunt(1f, "Only seconds until the next wave."),
                    new TimedTransition( "4", 2000)
                ),
                new State("4",
                    new SetAltTexture(0),
                    new Order(9999, "Arena East Gate Spawner", "arena2wave2"),
                    new Order(9999, "Arena West Gate Spawner", "arena2wave2"),
                    new Order(9999, "Arena South Gate Spawner", "arena2wave2"),
                    new Order(9999, "Arena North Gate Spawner", "arena2wave2"),
                    new TimedTransition( "5", 2750)
                ),
                new State("5",
                    new SetAltTexture(0),
                    new EntitiesNotExistsTransition(9999, "6", "Arena Possessed Girl", "Arena Ghost 1",
                        "Arena Ghost 2")
                ),
                new State("6",
                    new SetAltTexture(2),
                    new Taunt(1f, "Only seconds until the next wave."),
                    new TimedTransition( "7", 2000)
                ),
                new State("7",
                    new SetAltTexture(0),
                    new Order(9999, "Arena East Gate Spawner", "arena2wave3"),
                    new Order(9999, "Arena West Gate Spawner", "arena2wave3"),
                    new Order(9999, "Arena South Gate Spawner", "arena2wave3"),
                    new Order(9999, "Arena North Gate Spawner", "arena2wave3"),
                    new TimedTransition( "8", 2750)
                ),
                new State("8",
                    new SetAltTexture(0),
                    new EntitiesNotExistsTransition(9999, "9", "Arena Possessed Girl", "Arena Ghost 1",
                        "Arena Ghost 2")
                ),
                new State("9",
                    new SetAltTexture(2),
                    new Taunt(1f, "Only seconds until the next wave."),
                    new TimedTransition( "10", 2000)
                ),
                new State("10",
                    new SetAltTexture(0),
                    new Order(9999, "Arena East Gate Spawner", "arena2wave4"),
                    new Order(9999, "Arena West Gate Spawner", "arena2wave4"),
                    new Order(9999, "Arena South Gate Spawner", "arena2wave4"),
                    new Order(9999, "Arena North Gate Spawner", "arena2wave4"),
                    new TimedTransition( "11", 2750)
                ),
                new State("11",
                    new SetAltTexture(0),
                    new EntitiesNotExistsTransition(9999, "12", "Arena Possessed Girl", "Arena Ghost 1",
                        "Arena Ghost 2")
                ),
                new State("12",
                    new SetAltTexture(2),
                    new Taunt(1f, "Only seconds until the next wave."),
                    new TimedTransition( "13", 2000)
                ),
                new State("13",
                    new SetAltTexture(0),
                    new Order(9999, "Arena North Gate Spawner", "arena2boss"),
                    new TimedTransition( "14", 2750)
                ),
                new State("14",
                    new SetAltTexture(0),
                    new EntitiesNotExistsTransition(9999, "portal", "Arena Ghost Bride")
                ),
                new State("portal",
                    new SetAltTexture(0),
                    new DropPortalOnDeath("Haunted Cemetery Graves Portal", 1),
                    new Suicide()
                )
            );
            db.Init("Area 3 Controller",
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("Greeting",
                    new PlayerWithinTransition(5, "talk1")
                ),
                new State("talk1",
                    new SetAltTexture(2),
                    new Taunt(1f, "You've made it this far.."),
                    new TimedTransition( "talk3", 2000)
                ),
                new State("talk3",
                    new SetAltTexture(1),
                    new Taunt(1f, "Prepare yourself, the hoard is coming!"),
                    //new PlayerTextTransition("1", "Ready", 99, false, true)
                    new TimedTransition( "1", 3000)
                ),
                new State("1",
                    new SetAltTexture(0),
                    new Order(9999, "Arena East Gate Spawner", "arena3wave1"),
                    new Order(9999, "Arena West Gate Spawner", "arena3wave1"),
                    new Order(9999, "Arena South Gate Spawner", "arena3wave1"),
                    new Order(9999, "Arena North Gate Spawner", "arena3wave1"),
                    new TimedTransition( "8", 2750)
                ),
                new State("2",
                    new SetAltTexture(0),
                    new EntitiesNotExistsTransition(9999, "3", "Arena Risen Warrior", "Arena Risen Archer",
                        "Arena Risen Mage", "Arena Risen Brawler")
                ),
                new State("3",
                    new SetAltTexture(2),
                    new Taunt(1f, "Only seconds until the next wave."),
                    new TimedTransition( "4", 2000)
                ),
                new State("4",
                    new SetAltTexture(0),
                    new Order(9999, "Arena East Gate Spawner", "arena3wave2"),
                    new Order(9999, "Arena West Gate Spawner", "arena3wave2"),
                    new Order(9999, "Arena South Gate Spawner", "aren3wave2"),
                    new Order(9999, "Arena North Gate Spawner", "arena3wave2"),
                    new TimedTransition( "5", 2750)
                ),
                new State("5",
                    new SetAltTexture(0),
                    new EntitiesNotExistsTransition(9999, "6", "Arena Risen Warrior", "Arena Risen Archer",
                        "Arena Risen Mage", "Arena Risen Brawler")
                ),
                new State("6",
                    new SetAltTexture(2),
                    new Taunt(1f, "Only seconds until the next wave."),
                    new TimedTransition( "7", 2000)
                ),
                new State("7",
                    new SetAltTexture(0),
                    new Order(9999, "Arena East Gate Spawner", "arena3wave3"),
                    new Order(9999, "Arena West Gate Spawner", "arena3wave3"),
                    new Order(9999, "Arena South Gate Spawner", "arena3wave3"),
                    new Order(9999, "Arena North Gate Spawner", "arena3wave3"),
                    new TimedTransition( "8", 2750)
                ),
                new State("8",
                    new SetAltTexture(0),
                    new EntitiesNotExistsTransition(9999, "9", "Arena Risen Warrior", "Arena Risen Archer",
                        "Arena Risen Mage", "Arena Risen Brawler")
                ),
                new State("9",
                    new SetAltTexture(2),
                    new Taunt(1f, "Only seconds until the next wave."),
                    new TimedTransition( "10", 2000)
                ),
                new State("10",
                    new SetAltTexture(0),
                    new Order(9999, "Arena East Gate Spawner", "arena3wave4"),
                    new Order(9999, "Arena West Gate Spawner", "arena3wave4"),
                    new Order(9999, "Arena South Gate Spawner", "arena3wave4"),
                    new Order(9999, "Arena North Gate Spawner", "arena3wave4"),
                    new TimedTransition( "11", 2750)
                ),
                new State("11",
                    new SetAltTexture(0),
                    new EntitiesNotExistsTransition(9999, "12", "Arena Risen Warrior", "Arena Risen Archer",
                        "Arena Risen Mage", "Arena Risen Brawler")
                ),
                new State("12",
                    new SetAltTexture(2),
                    new Taunt(1f, "Only seconds until the next wave."),
                    new TimedTransition( "13", 2000)
                ),
                new State("13",
                    new SetAltTexture(0),
                    new Order(9999, "Arena North Gate Spawner", "arena3boss"),
                    new TimedTransition( "14", 2750)
                ),
                new State("14",
                    new SetAltTexture(0),
                    new EntitiesNotExistsTransition(9999, "portal", "Arena Grave Caretaker")
                ),
                new State("portal",
                    new SetAltTexture(0),
                    new DropPortalOnDeath("Haunted Cemetery Final Rest Portal", 1),
                    new Suicide()
                )
            );
            db.Init("Area 4 Controller",
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("Greeting",
                    new PlayerWithinTransition(5, "talk1")
                ),
                new State("talk1",
                    new SetAltTexture(2),
                    new Taunt(1f, "The final battle is imminent."),
                    new TimedTransition( "talk3", 2000)
                ),
                new State("talk3",
                    new SetAltTexture(1),
                    new Taunt(1f, "Prepare yourself, the hoard is coming!"),
                    //new PlayerTextTransition("1", "Ready", 99, false, true)
                    new TimedTransition( "1", 3000)
                ),
                new State("1",
                    new SetAltTexture(0),
                    new Order(9999, "Arena East Gate Spawner", "arena4wave1"),
                    new Order(9999, "Arena West Gate Spawner", "arena4wave1"),
                    new Order(9999, "Arena South Gate Spawner", "arena4wave1"),
                    new Order(9999, "Arena North Gate Spawner", "arena4wave1"),
                    new TimedTransition( "8", 2750)
                ),
                new State("2",
                    new SetAltTexture(0),
                    new EntitiesNotExistsTransition(9999, "3", "Classic Ghost", "Werewolf", "Zombie Hulk",
                        "Werewolf Cub")
                ),
                new State("3",
                    new SetAltTexture(2),
                    new Taunt(1f, "Only seconds until the next wave."),
                    new TimedTransition( "4", 2000)
                ),
                new State("4",
                    new SetAltTexture(0),
                    new Order(9999, "Arena East Gate Spawner", "arena4wave2"),
                    new Order(9999, "Arena West Gate Spawner", "arena4wave2"),
                    new Order(9999, "Arena South Gate Spawner", "arena4wave2"),
                    new Order(9999, "Arena North Gate Spawner", "arena4wave2"),
                    new TimedTransition( "5", 2750)
                ),
                new State("5",
                    new SetAltTexture(0),
                    new EntitiesNotExistsTransition(9999, "6", "Classic Ghost", "Werewolf", "Zombie Hulk",
                        "Werewolf Cub")
                ),
                new State("6",
                    new SetAltTexture(2),
                    new Taunt(1f, "Only seconds until the next wave."),
                    new TimedTransition( "7", 2000)
                ),
                new State("7",
                    new SetAltTexture(0),
                    new Order(9999, "Arena East Gate Spawner", "arena4wave3"),
                    new Order(9999, "Arena West Gate Spawner", "arena4wave3"),
                    new Order(9999, "Arena South Gate Spawner", "arena4wave3"),
                    new Order(9999, "Arena North Gate Spawner", "arena4wave3"),
                    new TimedTransition( "8", 2750)
                ),
                new State("8",
                    new SetAltTexture(0),
                    new EntitiesNotExistsTransition(9999, "9", "Classic Ghost", "Werewolf", "Zombie Hulk",
                        "Werewolf Cub")
                ),
                new State("9",
                    new SetAltTexture(2),
                    new Taunt(1f, "Only seconds until the next wave."),
                    new TimedTransition( "10", 2000)
                ),
                new State("10",
                    new SetAltTexture(0),
                    new Order(9999, "Arena East Gate Spawner", "arena4wave4"),
                    new Order(9999, "Arena West Gate Spawner", "arena4wave4"),
                    new Order(9999, "Arena South Gate Spawner", "arena4wave4"),
                    new Order(9999, "Arena North Gate Spawner", "arena4wave4"),
                    new TimedTransition( "11", 2750)
                ),
                new State("11",
                    new SetAltTexture(0),
                    new EntitiesNotExistsTransition(9999, "12", "Classic Ghost", "Werewolf", "Zombie Hulk",
                        "Werewolf Cub")
                ),
                new State("12",
                    new SetAltTexture(2),
                    new Taunt(1f, "Only seconds until the next wave."),
                    new TimedTransition( "13", 2000)
                ),
                new State("13",
                    new SetAltTexture(0),
                    new TimedTransition( "14", 2750)
                ),
                new State("14",
                    new SetAltTexture(0),
                    new EntitiesNotExistsTransition(9999, "portal", "Classic Ghost", "Werewolf", "Zombie Hulk",
                        "Werewolf Cub")
                ),
                new State("portal",
                    new SetAltTexture(0),
                    new TransformOnDeath("Ghost of Skuld"),
                    new Suicide()
                )
            );
        }
    }
}