using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static RotMG.Game.Logic.LootDef;

namespace RotMG.Game.Logic.Database
{
    class Crypt : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {
            db.Init("Arena Spider",
                    new Shoot(10, 8, 45, 0, 0f, cooldown: 300),
                    new Prioritize(
                        new StayCloseToSpawn(3f, 3),
                        new Wander(0.2f)
                    )
                );

            db.Init("Arena Pumpkin King",
                    new State("base",
                        new Shoot(10, 8, 45, 1, 0f, cooldown: 1000),
                        new Prioritize(
                            new Follow(0.8f, range: 1),
                            new Wander(0.4f)
                            ),
                        new Prioritize(
                            new Charge(2, pred: e => e.HasConditionEffect(Common.ConditionEffectIndex.Paralyzed)),
                            new Shoot(5, count: 3, shootAngle: 10)
                        )
                    ),
                    new Threshold(0.01f,
                        LootTemplates.BasicPots(0.01f).Concat(
                            new MobDrop[] {
                                new TierLoot(8, TierLoot.LootType.Weapon, 1f, r: new RarityModifiedData(1f, 2)),
                                new Threshold(0.01f, 
                                    new ItemLoot("Pumpkin Staff", 0.01f, r: new RarityModifiedData(1.2f)),
                                    new ItemLoot("Pumpkin Bow", 0.01f, r: new RarityModifiedData(1.2f)),
                                    new ItemLoot("Amulet of Ancient Power", 0.005f),
                                    new ItemLoot("Potion of Dexterity", 0.9f)
                                )
                            }
                        ).ToArray()
                    )
                ) ;

            db.Init("Septavius the Ghost God",
                    new State("warning",
                        new Flash(0xff0033ff, .3f, 3),
                        new TimedTransition("fire", 1000)
                    ),
                    new State("fire",
                        new Shoot(10, 4, 10, 0, cooldown: 200),
                        new Shoot(10, 2, 40, 1, cooldown: 400),
                        new Prioritize(false,
                            new Prioritize(true,
                                new Charge(2.0, pred: e => e.HasConditionEffect(Common.ConditionEffectIndex.Hexed)),
                                new Shoot(10, 8, 45, 2, 0f)
                            ),
                            new Follow(0.8f, range: 1),
                            new Wander(0.4f)
                        ),
                        new TimedTransition("warning", 1200)
                    ),
                    new Threshold(0.01f,
                        LootTemplates.BasicPots(0.3f).Concat(
                            new MobDrop[] {
                                new TierLoot(8, TierLoot.LootType.Weapon, 1f, r: new RarityModifiedData(1f, 2)),
                                new ItemLoot("Pumpkin Staff", 0.02f, r: new RarityModifiedData(1.2f)),
                                new ItemLoot("Pumpkin Bow", 0.02f, r: new RarityModifiedData(1.2f))
                            }
                        ).ToArray()
                    )
                );

        }
    }
}
