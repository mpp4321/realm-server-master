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
                    new HealEntity(12, "Players", healAmount: 500, mpHealAmount: 500, coolDown: 100)
                ));

            db.EveryInit = new MobDrop[] {
                new TierLoot(7, TierLoot.LootType.Weapon, 0.01f),
                new TierLoot(7, TierLoot.LootType.Armor, 0.01f),
                new TierLoot(7, TierLoot.LootType.Ring, 0.01f),
                new TierLoot(5, TierLoot.LootType.Ability, 0.005f),
                new ItemLoot("Potion of Life", 0.001f)
            };

            db.Init("Oasis Giant", new State("base",
                    new State("init", 
                        new PlayerWithinTransition(10.5f, "summon_minions")
                    ),
                    new State("summon_minions",
                        new TossObject("Oasis Ruler", 3, 0, cooldown: 100000, throwEffect: true),
                        new TossObject("Oasis Ruler", 3, 180, cooldown: 100000, throwEffect: true),
                        new TimedTransition("idle", 1000)
                    ),
                    new State("idle",
                        new Shoot(10, 10, 36, 0, cooldown: 300),
                        new Prioritize(
                            new StayCloseToSpawn(0.8f, 10),
                            new Follow(0.8f, range: 1),
                            new Wander(0.8f)
                        )
                    )
                ), new ItemLoot("Forest Giant Slayer", 0.01f), new Threshold(0.01f, LootTemplates.BasicPots(0.01f)));
            db.Init("Oasis Ruler", new State("base",
                    new State("init", 
                        new PlayerWithinTransition(10.5f, "summon_minions")
                    ),
                    new State("summon_minions",
                        new TossObject("Oasis Soldier", 3, 90, cooldown: 100000, throwEffect: true),
                        new TossObject("Oasis Soldier", 3, 270, cooldown: 100000, throwEffect: true),
                        new TimedTransition("idle", 1000)
                    ),
                    new State("idle",
                        new Shoot(3, 4, 90, 0, cooldown: 100),
                        new Prioritize(
                            new Follow(0.8f, range: 1)
                        )
                    )
                ));
            db.Init("Oasis Soldier", new State("base",
                    new Shoot(3, 4, 90, 0, cooldown: 100),
                    new Prioritize(
                        new Follow(0.8f, range: 1)
                    )
            ));

            db.EveryInit = new MobDrop[] { };

        }
    }
}
