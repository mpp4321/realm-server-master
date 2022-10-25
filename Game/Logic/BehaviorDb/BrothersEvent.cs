using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotMG.Game.Logic.Database
{
    public class BrothersEvent : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {

            MobDrop[] BrotherLoots = new MobDrop[] 
            {
                new ItemLoot("Potion of Defense", 0.8f),
                new ItemLoot("Potion of Vitality", 0.8f),
                new ItemLoot("Potion of Wisdom", 0.8f),
                new ItemLoot("Potion of Life", 0.1f),
                new TierLoot(9, TierLoot.LootType.Armor, 0.5f),
                new TierLoot(9, TierLoot.LootType.Weapon, 0.5f),
                new TierLoot(10, TierLoot.LootType.Weapon, 0.2f),
                new TierLoot(11, TierLoot.LootType.Weapon, 0.1f),
                new TierLoot(10, TierLoot.LootType.Armor, 0.2f),
                new TierLoot(11, TierLoot.LootType.Armor, 0.1f),
                new ItemLoot("Tincture of Fear", 0.3f),
                new ItemLoot("Tincture of Courage", 0.3f),
                new ItemLoot("Tincture of Life", 0.3f)
            };



            db.Init("shtrs Firebomb",
                    new ConditionalEffect(Common.ConditionEffectIndex.Invincible),
                    new QueuedBehav(
                        new CooldownBehav(1500, new Flash(0xFFFF0000, 1.5, 2)),
                        new Shoot(10, 8, 360.0f / 8),
                        new Suicide(0)
                    )
                );

            const int ROT_TIME = 6000;
            db.Init("Green Fire Brother",
                    new State("base",
                        new StayCloseToSpawn(1.0f, 25),
                        new Shoot(99f, 10, 360 / 10.0f, 0, 0, 10.0f, cooldown: 200),
                        new TossObject("shtrs Firebomb", 20f, cooldown: new wServer.logic.Cooldown(2000, 1000), minRange: 3f, maxRange: 10f),
                        new TransitionFrom("base", "trans_state"),
                        new State("trans_state",
                            new OrderOnEntry(99f, "Yellow Fire Brother", "trans_state"),
                            new OrderOnEntry(99f, "Blue Fire Brother", "trans_state"),
                            new TimedRandomTransition(3100, "rotate_1", "rotate_2", "rotate_3")
                        ),
                        new State("mytime",
                            new Grenade(10f, 100, 3, cooldown: 500, color: 0xFF00FF00)
                        ),
                        new State("rotate_1",
                            new OrderOnEntry(99f, "Yellow Fire Brother", "rotate_1"),
                            new OrderOnEntry(99f, "Blue Fire Brother", "mytime"),
                            new Prioritize(
                                new Orbit(0.5f, 6, 99f, "Blue Fire Brother"),
                                new Wander(0.4f)
                            ),
                            new TimedTransition("trans_state", ROT_TIME)
                        ),
                        new State("rotate_2",
                            new OrderOnEntry(99f, "Blue Fire Brother", "rotate_2"),
                            new OrderOnEntry(99f, "Yellow Fire Brother", "mytime"),
                            new Prioritize(
                                new Orbit(0.5f, 6, 99f, "Yellow Fire Brother"),
                                new Wander(0.4f)
                            ),
                            new TimedTransition("trans_state", ROT_TIME)
                        ),
                        new State("rotate_3",
                            new OrderOnEntry(99f, "Blue Fire Brother", "rotate_3"),
                            new OrderOnEntry(99f, "Yellow Fire Brother", "rotate_3"),
                            new Prioritize(
                                new Orbit(0.5f, 6, 99f, "Yellow Fire Brother"),
                                new Wander(0.4f)
                            ),
                            new TimedTransition("trans_state", ROT_TIME)
                        )
                    ),
                    new Threshold(0.01f, BrotherLoots),
                    new ItemLoot("Sword of Heroic Flame", 0.01f, 0.1f),
                    new Threshold(0.05f, LootTemplates.CrystalsRealmBoss())
                );

            db.Init("Yellow Fire Brother",
                    new State("base",
                        new StayCloseToSpawn(1.0f, 25),
                        new Shoot(99f, 10, 360 / 10.0f, 0, 0, 10.0f, cooldown: 200),
                        new TossObject("shtrs Firebomb", 20f, cooldown: new wServer.logic.Cooldown(2000, 1000), minRange: 3f, maxRange: 10f),
                        new TransitionFrom("base", "trans_state"),
                        new State("trans_state",
                            new OrderOnEntry(99f, "Green Fire Brother", "trans_state"),
                            new OrderOnEntry(99f, "Blue Fire Brother", "trans_state"),
                            new TimedRandomTransition(3200, "rotate_1", "rotate_2", "rotate_3")
                        ),
                        new State("mytime",
                            new Shoot(10f, 3, 10, 1, cooldown: 300),
                            new Shoot(10f, 2, 10, 1, cooldown: 300),
                            new Shoot(10f, 5, 20, 1, cooldown: 300)
                        ),
                        new State("rotate_1",
                            new OrderOnEntry(99f, "Blue Fire Brother", "mytime"),
                            new OrderOnEntry(99f, "Green Fire Brother", "rotate_1"),
                            new Prioritize(
                                new Orbit(0.5f, 6, 99f, "Blue Fire Brother"),
                                new Wander(0.4f)
                            ),
                            new TimedTransition("trans_state", ROT_TIME)
                        ),
                        new State("rotate_2",
                            new OrderOnEntry(99f, "Green Fire Brother", "rotate_1"),
                            new OrderOnEntry(99f, "Blue Fire Brother", "rotate_1"),
                            new Prioritize(
                                new Orbit(0.5f, 6, 99f, "Green Fire Brother"),
                                new Wander(0.4f)
                            ),
                            new TimedTransition("trans_state", ROT_TIME)
                        ),
                        new State("rotate_3",
                            new OrderOnEntry(99f, "Green Fire Brother", "mytime"),
                            new OrderOnEntry(99f, "Blue Fire Brother", "rotate_3"),
                            new Prioritize(
                                new Orbit(0.5f, 6, 99f, "Green Fire Brother"),
                                new Wander(0.4f)
                            ),
                            new TimedTransition("trans_state", ROT_TIME)
                        )
                    ),
                    new Threshold(0.01f, BrotherLoots),
                    new Threshold(0.05f, LootTemplates.CrystalsRealmBoss()),
                    new ItemLoot("Pendant of Thunder", 0.01f, 0.1f)
                );

            db.Init("Blue Fire Brother",
                    new State("base",
                        new StayCloseToSpawn(1.0f, 25),
                        new Shoot(99f, 10, 360 / 10.0f, 0, 0, 10.0f, cooldown: 200),
                        new TossObject("shtrs Firebomb", 20f, cooldown: new wServer.logic.Cooldown(2000, 1000), minRange: 3f, maxRange: 10f),
                        new TransitionFrom("base", "trans_state"),
                        new State("trans_state",
                            new OrderOnEntry(99f, "Yellow Fire Brother", "trans_state"),
                            new OrderOnEntry(99f, "Green Fire Brother", "trans_state"),
                            new TimedRandomTransition(3300, "rotate_1", "rotate_2", "rotate_3")
                        ),
                        new State("mytime",
                            new Shoot(10f, 3, 10, 1, cooldown: 300),
                            new Shoot(10f, 2, 10, 1, cooldown: 300),
                            new Shoot(10f, 5, 20, 1, cooldown: 300)
                        ),
                        new State("rotate_1",
                            new OrderOnEntry(99f, "Yellow Fire Brother", "rotate_1"),
                            new OrderOnEntry(99f, "Green Fire Brother", "rotate_1"),
                            new Prioritize(
                                new Orbit(0.5f, 6, 99f, "Yellow Fire Brother"),
                                new Wander(0.4f)
                            ),
                            new TimedTransition("trans_state", ROT_TIME)
                        ),
                        new State("rotate_2",
                            new OrderOnEntry(99f, "Green Fire Brother", "rotate_1"),
                            new OrderOnEntry(99f, "Yellow Fire Brother", "mytime"),
                            new Prioritize(
                                new Orbit(0.5f, 6, 99f, "Yellow Fire Brother"),
                                new Wander(0.4f)
                            ),
                            new TimedTransition("trans_state", ROT_TIME)
                        ),
                        new State("rotate_3",
                            new OrderOnEntry(99f, "Green Fire Brother", "mytime"),
                            new OrderOnEntry(99f, "Yellow Fire Brother", "rotate_3"),
                            new Prioritize(
                                new Orbit(0.5f, 6, 99f, "Green Fire Brother"),
                                new Wander(0.4f)
                            ),
                            new TimedTransition("trans_state", ROT_TIME)
                        )
                    ),
                    new Threshold(0.01f, BrotherLoots),
                    new ItemLoot("Star of Flowing Water", 0.03f, 0.01f),
                    new Threshold(0.05f, LootTemplates.CrystalsRealmBoss())
                );
        }
    }
}
