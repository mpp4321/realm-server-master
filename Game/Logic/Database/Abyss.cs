using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.Database
{
    class Abyss : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {

            db.Init("Brute of the Abyss", new State("base",
                    new Shoot(5, 2, shootAngle: 30, cooldown: 500),
                    new Prioritize(
                            new Follow(1.5f, 5, 2),
                            new Wander(0.2f)
                        )
                ));

            db.Init("Brute Warrior of the Abyss", new State("base",
                    new Shoot(5, 4, cooldown: 500, fixedAngle: 0f, rotateAngle: 45f/2),
                    new Prioritize(
                            new Follow(1.5f, 5, 2),
                            new Wander(0.2f)
                        )
                ));

            db.Init("Demon Mage of the Abyss", new State("base",
                    new Shoot(5, 4, cooldown: 1000, fixedAngle: 0f, rotateAngle: 45f/2),
                    new Prioritize(
                            new Wander(0.2f)
                        )
                ));

            db.Init("Demon Warrior of the Abyss", new State("base",
                    new Shoot(5, 2, shootAngle: 30, cooldown: 1000),
                    new Grenade(10, 20, 3, cooldown: 500),
                    new Prioritize(
                            new Follow(1.5f, 5, 4),
                            new Wander(0.2f)
                        )
                ));

            db.Init("Demon of the Abyss", new State("base",
                    new Shoot(5, 2, shootAngle: 30, cooldown: 1000),
                    new Grenade(10, 5, 5, cooldown: 250),
                    new Prioritize(
                            new Follow(1.5f, 5, 4),
                            new Wander(0.2f)
                        )
                ));

            db.Init("Archdemon Malphas", 
                new State("base",
                    new Shoot(5, 4, index: 1, shootAngle: 15, angleOffset: 15f, cooldown: 2500),
                    new Shoot(5, 4, index: 1, shootAngle: 15, angleOffset: 0f, cooldown: 3000),
                    new Grenade(10, 100, 2, cooldown: 400),
                    new Prioritize(
                            new Follow(1.5f, 5, 4),
                            new Wander(0.2f)
                        ),
                    new HealthTransition(0.25f, "rage"),
                    new TimedTransition("prepburst", 8000)
                ),
                new State("prepburst", 
                    new ConditionalEffect(Common.ConditionEffectIndex.Invulnerable),
                    new Flash(0xffff0000, 0.3f, 3),
                    new TimedTransition("burst", 1000)
                ),
                new State("burst", 
                        new Shoot(10, 8, shootAngle: 30, index: 0, cooldown: 200),
                        new TossObject("Brute Warrior of the Abyss", range: 5, cooldown: 1000),
                        new TossObject("Demon Mage of the Abyss", range: 10, cooldown: 2000),
                        new HealthTransition(0.25f, "rage"),
                        new TimedTransition("base", 3000)
                ),
                new State("rage",
                    new ChangeSize(50, 150),
                    new Shoot(10, 4, index: 1, shootAngle: 90f, fixedAngle: 0, rotateAngle: 195f, cooldown: 200),
                    new Shoot(10, 2, shootAngle: 30, index: 0, predictive: 1f, cooldown: 500),
                    new ConditionalEffect(Common.ConditionEffectIndex.Damaging),
                    new ConditionalEffect(Common.ConditionEffectIndex.Armored),
                    new Charge(1.8f, 15, coolDown: 100),
                    new Flash(0xffff0505, 0.5, 2)
                ),
                    new ItemLoot("Demon Frog Generator", 0.01f, 0.01f),
                    new ItemLoot("Potion of Life", 0.01f, 0.01f),
                    new Threshold(0.01f,
                        LootTemplates.BasicPots(0.01f)
                    )
                );

        }
    }
}
