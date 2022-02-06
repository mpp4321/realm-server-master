
//from LOE-V6, bit chages by ghostmaree
using RotMG.Common;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;
using RoTMG.Game.Logic.Transitions;
using static RotMG.Game.Logic.Loots.TierLoot;

namespace RotMG.Game.Logic.Database
{
    class Sewers : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {
            db.Init("DS Gulpord the Slime God",
                HPScale.BOSS_HP_SCALE_DEFAULT(),
                new State("base",
                    //new ScaleHP2(40,3,15),
                    new DropPortalOnDeath(target: "Glowing Realm Portal"),
                    new State("idle",
                        new PlayerWithinTransition(dist: 12, targetState: "begin", seeInvis: true)
                        ),
                    new State("begin",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(time: 3500, targetState: "shoot")
                        ),
                    new State("shoot",
                        new HealthTransition(0.90f, "randomshooting"),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 1500),
                        new Shoot(10, 8, 45, 1, cooldown: 2000),
                        new Shoot(10, 5, 72, 0, 0, cooldown: 600, cooldownOffset: 300),
                        new Shoot(10, 5, 72, 0, 3, cooldown: 600, cooldownOffset: 300),
                        new Shoot(10, 5, 72, 0, 36, cooldown: 600, cooldownOffset: 600),
                        new Shoot(10, 5, 72, 0, 39, cooldown: 600, cooldownOffset: 600)
                        ),
                    new State("randomshooting",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 1500),
                        new Shoot(10, 8, 45, 0, fixedAngle: 0, cooldown: 600, cooldownOffset: 300),
                        new Shoot(10, 8, 45, 0, fixedAngle: 22, cooldown: 600, cooldownOffset: 600),
                        new ReturnToSpawn(speed: 1),
                        new HealthTransition(0.70f, "tossnoobs"),
                        new TimedTransition("tossnoobs", 6000)
                        ),
                    new State("tossnoobs",
                        new TossObject("DS Boss Minion", 3, 0, cooldown: 99999999),
                        new TossObject("DS Boss Minion", 3, 45, cooldown: 99999999),
                        new TossObject("DS Boss Minion", 3, 90, cooldown: 99999999),
                        new TossObject("DS Boss Minion", 3, 135, cooldown: 99999999),
                        new TossObject("DS Boss Minion", 3, 180, cooldown: 99999999),
                        new TossObject("DS Boss Minion", 3, 225, cooldown: 99999999),
                        new TossObject("DS Boss Minion", 3, 270, cooldown: 99999999),
                        new TossObject("DS Boss Minion", 3, 315, cooldown: 99999999),
                        new TimedTransition("derp", 100)
                        ),
                    new State("derp",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 1500),
                        new HealthTransition(0.50f, "baibaiscrubs"),
                        new Shoot(10, 6, 12, 0, cooldown: 1000),
                        new Wander(0.5f),
                        new StayCloseToSpawn(0.5f, 7)
                        ),
                    new State("baibaiscrubs",
                        new ReturnToSpawn(speed: 2),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new TimedTransition("seclol", 1500)
                        ),
                    new State("seclol",
                        new ChangeSize(20, 0),
                        new TimedTransition("nubs", 1000)
                        ),
                    new State("nubs",
                        new TossObject("DS Gulpord the Slime God M", 3, 32, cooldown: 9999999, tossInvis: true),
                        new TossObject("DS Gulpord the Slime God M", 3, 15, cooldown: 9999999, tossInvis: true),
                        new EntitiesWithinTransition(dist: 999, entity: "DS Gulpord the Slime God M", targetState: "idleeeee")
                        ),
                    new State("idleeeee",
                        new EntitiesNotExistsTransition(999, "nubs2", "DS Gulpord the Slime God M")
                        ),
                    new State("nubs2",
                        new TossObject("DS Gulpord the Slime God S", 3, 32, cooldown: 9999999, tossInvis: true),
                        new TossObject("DS Gulpord the Slime God S", 3, 15, cooldown: 9999999, tossInvis: true),
                        new TossObject("DS Gulpord the Slime God S", 3, 26, cooldown: 9999999, tossInvis: true),
                        new TossObject("DS Gulpord the Slime God S", 3, 21, cooldown: 9999999, tossInvis: true),
                        new EntitiesWithinTransition(dist: 999, entity: "DS Gulpord the Slime God S", targetState: "idleeeeee")
                        ),
                    new State("idleeeeee",
                        new EntitiesNotExistsTransition(999, "seclolagain", "DS Gulpord the Slime God S")
                        ),
                    new State("seclolagain",
                        new ChangeSize(20, 120),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition("GO ANGRY!!!!111!!11", 1000)
                        ),
                    new State("GO ANGRY!!!!111!!11",
                        new Flash(0xFF0000, 1, 1),
                        new TimedTransition("FOLLOW", 1000)
                        ),
                    new State("FOLLOW",
                        new ConditionalEffect(ConditionEffectIndex.StunImmune),
                        new Shoot(10, 8, 45, 2, cooldown: 2000),
                        new Shoot(3, 1, 0, 1, cooldown: 1000),
                        new Shoot(10, 2, 10, 0, cooldown: 150, angleOffset: 0.1f),
                        new Follow(speed: 0.6f, acquireRange: 10, range: 0)
                    )
                ),
                new Threshold(0.01f,
                    new TierLoot(10, LootType.Weapon, 0.25f),
                    new TierLoot(11, LootType.Weapon, 0.25f),
                    new TierLoot(10, LootType.Armor, 0.25f),
                    new TierLoot(11, LootType.Armor, 0.25f),
                    new TierLoot(5, LootType.Ability, 0.125f),
                    new TierLoot(4, LootType.Ring, 0.25f),
                    new TierLoot(5, LootType.Ring, 0.125f),
                    new ItemLoot("Potion of Defense", 1.0f, 2),
                    new ItemLoot("Wine Cellar Incantation", 0.05f),
                    new ItemLoot("(Green) UT Egg", 0.1f, 0.01f),
                    new ItemLoot("(Blue) RT Egg", 0.01f, 0.01f),
                    new ItemLoot("Realm Equipment Crystal", 0.02f),
                    new ItemLoot("Void Blade", 0.001f, 0.01f)
                    //new ItemLoot("Murky Toxin", 0.004f),
                    //new ItemLoot("Virulent Venom", 0.006f),
                    //new ItemLoot("Dagger of Toxin", 0.006f),
                    //new ItemLoot("Sewer Cocktail", 0.006f)
                )
            );
            db.Init("DS Boss Minion",
                new State("base",
                    new Wander(0.6f),
                    new Grenade(3, 50, 10, cooldown: 5000)
                )
            );
            db.Init("DS Gulpord the Slime God M",
                new State("base",
                    new Orbit(0.6f, 3, target: "DS Gulpord the Slime God"),
                    new Shoot(10, 8, 45, 1, cooldown: 1500),
                    new Shoot(10, 4, 60, 0, cooldown: 4500)
                )
            );
            db.Init("DS Gulpord the Slime God S",
                new State("base",
                    new Orbit(0.6f, 3, target: "DS Gulpord the Slime God"),
                    new Shoot(10, 4, 20, 1, cooldown: 2000)
                )
            );
            db.Init("DS Natural Slime God",
                new State("base",
                    new Prioritize(
                        new StayAbove(speed: 1, altitude: 200),
                        new Follow(speed: 1, range: 7),
                        new Wander(speed: 0.4f)
                    ),
                    new Shoot(12, count: 5, shootAngle: 10, index: 0, predictive: 1, cooldown: 1000),
                    new Shoot(10, count: 1, index: 1, predictive: 1, cooldown: 650)
                )
            );
            ;
        }
    }
}
