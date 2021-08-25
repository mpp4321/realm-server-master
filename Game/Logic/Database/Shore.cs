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
            db.Init("Pirate",
                new State(
                    "base",
                    new Prioritize(
                        new Follow(0.85f, range: 1, duration: 5000, coolDown: 0),
                        new Wander(0.4f)
                        ),
                    new Shoot(3, cooldown: 2500)
                    ),
                new TierLoot(1, ItemType.Weapon, 0.2f),
                new ItemLoot("Health Potion", 0.03f)
            );
            db.Init("Piratess",
                new State(
                    "base",
                    new Prioritize(
                        new Follow(1.1f, range: 1, duration: 3000, coolDown: 1500),
                        new Wander(0.6f)
                        ),
                    new Shoot(3, cooldown: 2500),
                    new Reproduce("Pirate", densityMax: 5),
                    new Reproduce("Piratess", densityMax: 5)
                    ),
                new TierLoot(1, ItemType.Armor, 0.2f),
                new ItemLoot("Health Potion", 0.03f)
            );
            db.Init("Snake",
                new State(
                    "base",
                    new Wander(0.8f),
                    new Shoot(10, cooldown: 2000),
                    new Reproduce(densityMax: 5)
                    ),
                new ItemLoot("Health Potion", 0.03f),
                new ItemLoot("Magic Potion", 0.02f)
            );
            db.Init("Poison Scorpion",
                new State(
                    "base",
                    new Prioritize(
                        new Protect(0.4f, "Scorpion Queen"),
                        new Wander(0.4f)
                        ),
                    new Shoot(8, cooldown: 2000)
                    )
            );
            db.Init("Scorpion Queen",
                new State(
                    "base",
                    new Wander(0.2f),
                    new Spawn("Poison Scorpion", givesNoXp: false),
                    new Reproduce("Poison Scorpion", coolDown: 10000, densityMax: 10),
                    new Reproduce(densityMax: 2, densityRadius: 40)
                    ),
                new ItemLoot("Health Potion", 0.03f),
                new ItemLoot("Magic Potion", 0.02f)
            );
            db.Init("Bandit Enemy",
                new State(
                    "base",
                    new State("fast_follow",
                        new Shoot(3),
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
                        new Shoot(4.5f),
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
                    new Spawn("Bandit Enemy", coolDown: 8000, maxChildren: 4, givesNoXp: false),
                    new State("bold",
                        new State("warn_about_grenades",
                            new Taunt(0.15, "Catch!"),
                            new TimedTransition("wimpy_grenade1", 400)
                            ),
                        new State("wimpy_grenade1",
                            new Grenade(1.4f, 12, cooldown: 10000),
                            new Prioritize(
                                new StayAbove(0.3, 7),
                                new Wander(0.3f)
                                ),
                            new TimedTransition("wimpy_grenade2", 2000)
                            ),
                        new State("wimpy_grenade2",
                            new Grenade(1.4f, 12, cooldown: 10000),
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
                new TierLoot(1, ItemType.Weapon, 0.2f),
                new TierLoot(1, ItemType.Armor, 0.2f),
                new TierLoot(2, ItemType.Weapon, 0.12f),
                new TierLoot(2, ItemType.Armor, 0.12f),
                new ItemLoot("Health Potion", 0.12f),
                new ItemLoot("Magic Potion", 0.14f)
            );
            db.Init("Red Gelatinous Cube",
                new State(
                    "base_state",
                    new Shoot(8, count: 3, index: 0, shootAngle: 7, predictive: 0.2f, cooldown: 2000),
                    new Wander(0.4f),
                    new Reproduce(densityMax: 3)
                    ),
                new ItemLoot("Health Potion", 0.04f),
                new ItemLoot("Magic Potion", 0.04f)
            );
            db.Init("Purple Gelatinous Cube",
                new State(
                    "base",
                    new Shoot(8, count: 8, index: 0, shootAngle: 14, predictive: 0.2f, cooldown: 2000),
                    new Wander(0.4f),
                    new Reproduce(densityMax: 3)
                    ),
                new ItemLoot("Health Potion", 0.04f),
                new ItemLoot("Magic Potion", 0.04f)
            );
            db.Init("Green Gelatinous Cube",
                new State(
                    "base_state",
                    new Grenade(12, 20, color: 0x00ff28, cooldown: 3000),
                    new Wander(0.4f),
                    new Reproduce(densityMax: 3)
                    ),
                new ItemLoot("Health Potion", 0.04f),
                new ItemLoot("Magic Potion", 0.04f)
            );
        }
    }
}
