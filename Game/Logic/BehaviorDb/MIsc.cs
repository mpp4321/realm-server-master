using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RotMG.Game.Logic.Database
{
    class MIsc : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {
            db.Init("White Fountain", new State(
                    "base",
                    new HealEntity(12, "Players", healAmount: 500, mpHealAmount: 500, cooldown: 100)
                ));

            db.EveryInit = new MobDrop[] {
                new TierLoot(7, TierLoot.LootType.Weapon, 0.01f),
                new TierLoot(7, TierLoot.LootType.Armor, 0.01f),
                new TierLoot(7, TierLoot.LootType.Ring, 0.01f),
                new TierLoot(5, TierLoot.LootType.Ability, 0.005f),
                new ItemLoot("Potion of Life", 0.001f)
            };

            db.Init("Oasis Giant",
                    new StayCloseToSpawn(1, 14),
                    new State("init", 
                        new PlayerWithinTransition(14f, "summon_minions")
                    ),
                    new State("summon_minions",
                        new TossObject("Oasis Ruler", 5, 0, cooldown: 100000, throwEffect: true),
                        new TossObject("Oasis Ruler", 5, 120, cooldown: 100000, throwEffect: true),
                        new TossObject("Oasis Ruler", 5, 240, cooldown: 100000, throwEffect: true),
                        new TimedTransition("whirlpool", 500)
                    ),
                    
                    new State("whirlpool",
                        new Order(20, "Oasis Soldier", "swim"),
                        new ReturnToSpawn(2, 0.5f),
                        new Shoot(16, 2, 180, 1, 0f, 8, cooldownOffset: 300),
                        new TimedTransition("base", 2800)
                    ),
                    new State("base",
                        new Shoot(10, 10, 36, 0, cooldown: 700),
                        new Prioritize(
                            new Wander(0.4f),
                            new Follow(0.6f, range: 1)
                            ),
                        new TimedTransition("bumrush", 3200)
                    ),
                    new State("bumrush",
                        new PlayerWithinTransition(2.5f, "tobase"),
                        new Order(20, "Oasis Soldier", "base"),
                        new Follow(0.3f, 6),
                        new ChargeShoot(
                            new Shoot(14, 2, 18, 0, predictive: 0.4f, cooldown: 700, cooldownVariance: 200),
                            new Charge(1.1f, 14, 1000)
                            ),
                        new Shoot(18, 2, 300, 3),
                        new Shoot(18, 2, 340, 4),
                        new TimedTransition("slink", 1300)
                    ),
                    new State("slink",
                        new Shoot(10, 10, 36, 0, cooldown: 700),
                        new ReturnToSpawn(0.3f),
                        new TimedTransition("whirlpool", 1700)
                    ),
                new Threshold(0.01f,
                    new ItemLoot("Forest Giant Slayer", 0.08f),
                    new ItemLoot("Wand of Deep Nature", 0.08f),
                    new TierLoot(9, TierLoot.LootType.Weapon, 0.35f),
                    new TierLoot(8, TierLoot.LootType.Armor, 0.35f),
                    new TierLoot(3, TierLoot.LootType.Ability, 0.35f)
                ));
            db.Init("Oasis Ruler", 
                new StayBack(0.5f, 0.75f, "Oasis Ruler"),
                new State("base",
                    new State("init", 
                        new PlayerWithinTransition(10.5f, "summon_minions")
                    ),
                    new State("summon_minions",
                        new TossObject("Oasis Soldier", 3, 90, cooldown: 100000, throwEffect: true),
                        new TossObject("Oasis Soldier", 3, 270, cooldown: 100000, throwEffect: true),
                        new TimedTransition("idle", 300)
                    ),
                    new State("idle",
                        new Shoot(5, 4, 90, 0, cooldown: 100),
                        new Prioritize(
                            new StayBack(0.5f, 0.75f, "Oasis Ruler"),
                            new Follow(0.8f, range: 1)
                        )
                    ),
                    new State("swim",
                        new StayBack(0.5f, 4f, "Oasis Ruler"),
                        new Shoot(7, 4, 90, 0, cooldown: 100, cooldownOffset: 200),
                        new Orbit(0.8f, 8, 16, "Oasis Giant", radiusVariance: 1.5f, pass: true),
                        new TimedTransition("base", 3400)
                        )
                ));
            db.Init("Oasis Soldier", new State("base",
                    new Shoot(5, 4, 90, 0, cooldown: 100),
                    new Prioritize(
                        new StayBack(0.5f, 0.75f, "Oasis Soldier"),
                        new Follow(0.8f, range: 1))),
                    new State("swim",
                        new StayBack(0.5f, 4f, "Oasis Soldier"),
                        new Shoot(7, 4, 90, 0, cooldown: 100, cooldownOffset: 200),
                        new Orbit(1f, 10, 16, "Oasis Giant", radiusVariance: 1.5f, pass: true),
                        new TimedTransition("base", 3400)
                        )

            );

            db.EveryInit = new MobDrop[] { };

        }
    }
}
