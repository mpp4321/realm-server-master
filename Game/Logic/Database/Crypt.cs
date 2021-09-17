using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RotMG.Game.Logic.Database
{
    class Crypt : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {

            db.Init("Arena Spider",
                    new Shoot(10, 8, 45, 0, 0f, cooldown: 300)
                );

            db.Init("Arena Pumpkin King",
                    new State("base",
                        new Shoot(10, 8, 45, 1, 0f, cooldown: 1000),
                        new Prioritize(
                            new Follow(0.4f, range: 1),
                            new Wander(0.4f)
                            ),
                        new Prioritize(
                            new Charge(pred: e => e.HasConditionEffect(Common.ConditionEffectIndex.Paralyzed)),
                            new Shoot(5, count: 3, shootAngle: 10)
                        )
                    ),
                    new Threshold(0.01f,
                        LootTemplates.BasicPots(0.3f).Concat(
                            new MobDrop[] {
                                new TierLoot(8, TierLoot.LootType.Weapon, 1f, rs: 2),
                                new ItemLoot("Pumpkin Staff", 0.02f, fm: 1.2f),
                                new ItemLoot("Pumpkin Bow", 0.02f, fm: 1.2f)
                            }
                        ).ToArray()
                    )
                ) ;

        }
    }
}
