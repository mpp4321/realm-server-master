using RotMG.Common;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Conditionals;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;
using ItemType = RotMG.Game.Logic.Loots.TierLoot.LootType;

namespace RotMG.Game.Logic.Database
{
    public class Shore : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {

            db.EveryInit = new IBehavior[]
            {
                new TierLoot(1, ItemType.Weapon, 0.2f),
                new TierLoot(2, ItemType.Weapon, 0.1f),
                new TierLoot(3, ItemType.Weapon, 0.05f),
                new TierLoot(1, ItemType.Armor, 0.2f),
                new TierLoot(2, ItemType.Armor, 0.1f),
                new TierLoot(3, ItemType.Armor, 0.05f),
                new TierLoot(1, ItemType.Ring, 0.2f),
                new TierLoot(2, ItemType.Ring, 0.1f),
                new TierLoot(3, ItemType.Ring, 0.05f),
                new TierLoot(1, ItemType.Ability, 0.1f),
                new TierLoot(2, ItemType.Ability, 0.05f),
            };

            db.Init("Pirate",
                new State(
                    "base",
                    new Prioritize(
                        //new Follow(0.85f, range: 1, duration: 5000, coolDown: 0),
                        new BuzzBehavior(speed: 1f),
                        new Wander(0.4f)
                        ),
                    new Shoot(3, cooldown: 500)
                    ),
                new ItemLoot("Health Potion", 0.03f)
            );
            db.Init("Piratess",
                new State(
                    "base",
                    new Prioritize(
                        new Follow(1.1f, range: 1, duration: 3000, cooldown: 1500),
                        new Wander(0.6f)
                        ),
                    new Shoot(3, cooldown: 500)
                    ),
                new ItemLoot("Health Potion", 0.03f)
            );

            db.Init("Poison Scorpion",
                new State(
                    "base",
                    new Prioritize(
                        new Protect(0.4f, "Scorpion Queen"),
                        new Wander(0.4f)
                        ),
                    new Shoot(8, cooldown: 500),
                    new Grenade(3, 15, radius: 3.5f, cooldown: 2500, color: 0xffccff00)
                )
            );
            db.Init("Scorpion Queen",
                new State(
                    "base",
                    new StayBack(0.1f, 6, "Scorpion Queen"),
                    new Wander(0.2f),
                    new Spawn("Poison Scorpion", givesNoXp: false),
                    new Reproduce("Poison Scorpion", cooldown: 10000, densityMax: 10),
                    new Reproduce(densityMax: 2, densityRadius: 40),
                    new Grenade(0, 35, 3, 0, cooldown: 2500, color: 0xffb3ff00, speed: 1000),
                    new Grenade(4, 25, 2, 0, cooldown: 2500, color: 0xff88c200, speed: 2000),
                    new Grenade(4, 25, 2, 90, cooldown: 2500, color: 0xff88c200, speed: 2000),
                    new Grenade(4, 25, 2, 180, cooldown: 2500, color: 0xff88c200, speed: 2000),
                    new Grenade(4, 25, 2, 270, cooldown: 2500, color: 0xff88c200, speed: 2000),
                    new Grenade(6, 15, 1.5f, 45, cooldown: 2500, color: 0xff4d6e00, speed: 3000),
                    new Grenade(6, 25, 1.5f, 135, cooldown: 2500, color: 0xff4d6e00, speed: 3000),
                    new Grenade(6, 25, 1.5f, 225, cooldown: 2500, color: 0xff4d6e00, speed: 3000),
                    new Grenade(6, 25, 1.5f, 315, cooldown: 2500, color: 0xff4d6e00, speed: 3000)
                    ),
                new ItemLoot("Health Potion", 0.03f),
                new ItemLoot("Magic Potion", 0.02f)
            );
            db.Init("Bandit Enemy",
                new State(
                    "base",
                    new State("fast_follow",
                        new Shoot(3, cooldownVariance: 100, cooldown: 300),
                        new Prioritize(
                            new Protect(0.6f, "Bandit Leader", acquireRange: 9, protectionRange: 7, reprotectRange: 3),
                            new Follow(1, range: 1),
                            new Wander(0.6f)
                            ),
                        new TimedTransition("scatter1", 3000)
                        ),
                    new State("scatter1",
                        new Prioritize(
                            new Protect(0.6f, "Bandit Leader", acquireRange: 9, protectionRange: 7, reprotectRange: 3),
                            new Wander(1),
                            new Wander(0.6f)
                            ),
                        new TimedTransition("slow_follow", 2000)
                        ),
                    new State("slow_follow",
                        new Shoot(3, cooldownVariance: 100, cooldown: 300),
                        new Prioritize(
                            new Protect(0.6f, "Bandit Leader", acquireRange: 9, protectionRange: 7, reprotectRange: 3),
                            new Follow(0.5f, acquireRange: 9, range: 3.5f, duration: 4000),
                            new Wander(0.5f)
                            ),
                        new TimedTransition("scatter2", 3000)
                        ),
                    new State("scatter2",
                        new Prioritize(
                            new Protect(0.6f, "Bandit Leader", acquireRange: 9, protectionRange: 7, reprotectRange: 3),
                            new Wander(1),
                            new Wander(0.6f)
                            ),
                        new TimedTransition("fast_follow", 2000)
                        ),
                    new State("escape",
                        new StayBack(0.5f, 8),
                        new TimedTransition("fast_follow", 15000)
                        )
                    )
            );
            db.Init("Bandit Leader",
                new State(
                    "base",
                    new Spawn("Bandit Enemy", cooldown: 8000, maxChildren: 4, givesNoXp: false),
                    new State("bold",
                        new State("warn_about_grenades",
                            new Taunt(0.15, "Catch!"),
                            new TimedTransition("wimpy_grenade1", 400)
                            ),
                        new State("wimpy_grenade1",
                            new Grenade(1.4f, 12, radius: 2, cooldown: 10000, speed: 500),
                            new Prioritize(
                                new StayAbove(0.3, 7),
                                new Wander(0.3f)
                                ),
                            new TimedTransition("wimpy_grenade2", 2000)
                            ),
                        new State("wimpy_grenade2",
                            new Grenade(1.4f, 12, radius: 2, cooldown: 10000, speed: 500),
                            new Prioritize(
                                new StayAbove(0.5, 7),
                                new Wander(0.5f)
                                ),
                            new TimedTransition("slow_follow", 3000)
                            ),
                        new State("slow_follow",
                            new Shoot(13, cooldown: 1000),
                            new Prioritize(
                                new StayAbove(0.4, 7),
                                new Follow(0.4f, acquireRange: 9, range: 3.5f, duration: 4000),
                                new Wander(0.4f)
                                ),
                            new TimedTransition("warn_about_grenades", 4000)
                            ),
                        new HealthTransition(0.45f, "meek")
                        ),
                    new State("meek",
                        new Taunt(0.5, "Forget this... run for it!"),
                        new StayBack(0.5f, 6),
                        new Order(10, "Bandit Enemy", "escape"),
                        new TimedTransition("bold", 12000)
                        )
                    ),
                new ItemLoot("Health Potion", 0.12f),
                new ItemLoot("Magic Potion", 0.14f)
            );
            db.Init("Red Gelatinous Cube",
                new State(
                    "base_state",
                    new Shoot(8, count: 3, index: 0, shootAngle: 7, predictive: 0.2f, cooldown: 2000),
                    new Shoot(8, count: 2, index: 0, shootAngle: 7, predictive: 0.2f, cooldownOffset: 200, cooldown: 2000),
                    new Shoot(8, count: 1, index: 0, predictive: 0.2f, cooldownOffset: 300, cooldown: 2000),
                    new Wander(0.4f)
                    ),
                new ItemLoot("Health Potion", 0.04f),
                new ItemLoot("Magic Potion", 0.04f)
            );
            db.Init("Purple Gelatinous Cube",
                new State(
                    "base",
                    //new Shoot(8, count: 8, index: 0, shootAngle: 14, predictive: 0.2f, cooldown: 2000),
                    new Shoot(8, count: 1, index: 0, predictive: 0.2f, cooldown: 2000),
                    new Shoot(8, count: 2, index: 0, shootAngle: 16, predictive: 0.2f, cooldownOffset: 250, cooldown: 2000),
                    new Shoot(8, count: 2, index: 0, shootAngle: 32, predictive: 0.2f, cooldownOffset: 500, cooldown: 2000),
                    new Shoot(8, count: 2, index: 0, shootAngle: 64, predictive: 0.2f, cooldownOffset: 600, cooldown: 2000),
                    new Shoot(8, count: 2, index: 0, shootAngle: 32, predictive: 0.2f, cooldownOffset: 700, cooldown: 2000),
                    new Shoot(8, count: 2, index: 0, shootAngle: 16, predictive: 0.2f, cooldownOffset: 800, cooldown: 2000),
                    new Shoot(8, count: 1, index: 0, predictive: 0.2f, cooldownOffset: 1500, cooldown: 900),
                    new Wander(0.4f)
                    ),
                new ItemLoot("Health Potion", 0.04f),
                new ItemLoot("Magic Potion", 0.04f)
            );

            db.Init("Green Gelatinous Cube",
                new State(
                    "base_state",
                    new Grenade(12, 20, color: 0x00ff28, cooldown: 3000),
                    new Grenade(12, 15, radius: 3, color: 0x00ff28, cooldown: 3000, speed: 2000),
                    new Grenade(12, 10, radius: 1.5f, color: 0x00ff28, cooldown: 3000, speed: 2500),
                    new Wander(0.4f)
                    ),
                new ItemLoot("Health Potion", 0.04f),
                new ItemLoot("Magic Potion", 0.04f)
            );

            db.EveryInit = new IBehavior[] { };

            db.Init("Snake",
                new State(
                    "base",
                    new Wander(0.8f),
                    new Shoot(10, cooldown: 500, effect: new ConditionEffectIndex[] { ConditionEffectIndex.Slowed }, effect_duration: 3000)
                    ),
                new ItemLoot("Health Potion", 0.03f),
                new ItemLoot("Magic Potion", 0.02f),
                new ItemLoot("Kendo 'Stick'", 0.02f)
            );

        }
    }
}
