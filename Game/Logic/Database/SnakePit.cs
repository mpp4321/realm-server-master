using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.Database
{
    class SnakePit : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {

            db.Init("Greater Pit Snake",
                    new State("based",
                        new Follow(2f, 10, 6),
                        new Shoot(8, 3, shootAngle: 12, cooldown: 300),
                        new Wander(0.7f)
                        )
                );
            db.Init("Greater Pit Viper",
                    new State("based",
                        new Follow(1f, 10, 6),
                        new Shoot(8, 2, shootAngle: 8, cooldown: 300, predictive: 0.4f),
                        new Wander(0.7f)
                    )
                );

            db.Init("Fire Python",
                    new State("based",
                        new Follow(3f, 10, 6),
                        new Shoot(8, 4, shootAngle: 16, cooldown: 300),
                        new Wander(0.5f)
                    )
                );
            db.Init("Brown Python",
                    new State("based",
                        new Follow(3f, 10, 6),
                        new Shoot(8, 2, shootAngle: 12, cooldown: 200),
                        new Wander(0.5f)
                    )
                );
            db.Init("Yellow Python",
                    new State("based",
                        new Follow(3f, 10, 6),
                        new Shoot(8, 1, cooldown: 100, predictive: 0.6f),
                        new Wander(0.5f)
                    )
                );
            db.Init("Pit Viper",
                    new State("based",
                        new Shoot(8, 1, cooldown: 250, predictive: 0.6f),
                        new Wander(0.3f)
                    )
                );
            db.Init("Pit Snake",
                    new State("based",
                        new Shoot(8, 1, cooldown: 250, predictive: 0.6f),
                        new Wander(0.3f)
                    )
                );
            db.Init("Stheno the Snake Queen",
                new State("wait",
                    new TossObject("Stheno Pet", 7, 0, 100000, throwEffect: true),
                    new TossObject("Stheno Pet", 7, 60, 100000, throwEffect: true),
                    new TossObject("Stheno Pet", 7, 120, 100000, throwEffect: true),
                    new TossObject("Stheno Pet", 7, 180, 100000, throwEffect: true),
                    new TossObject("Stheno Pet", 7, 240, 100000, throwEffect: true),
                    new TossObject("Stheno Pet", 7, 300, 100000, throwEffect: true),
                    new PlayerWithinTransition(8, "wait1", seeInvis: true)
                    ),
                new State("wait1",
                    new TimedTransition("start", 1000)

                ),
                new State("start",
                    new Shoot(12, 5, 70, 1, fixedAngle: 0f, rotateAngle: 6f, cooldown: 100, cooldownVariance: 50),
                    new Grenade(8, 60, 4, cooldown: 500),
                    new HealthTransition(0.6f, "rotateCW")
                ),
                new State("rotateCW",
                    new Prioritize(
                        new Wander(0.15f)
                        ),
                    new Shoot(15, 6, 60, fixedAngle: 0f, rotateAngle: 8f, cooldown: 100),
                    new TimedTransition("rotateCCW", 5000),
                    new HealthTransition(0.3f, "blind")
                    ),
                new State("rotateCCW",
                    new Prioritize(
                        new Wander(0.15f)
                        ),
                    new Shoot(15, 6, 60, fixedAngle: 0f, rotateAngle: -8f, cooldown: 100),
                    new TimedTransition("rotateCW", 5000),
                    new HealthTransition(0.3f, "blind")
                    ),
                new State("blind",
                    new Shoot(15, 3, 16, 1, predictive: 0.6f, cooldown: 200),
                    new Grenade(8, 60, 4, cooldown: 500),
                    new Order(20, "Stheno Pet", "protect"),
                    new Prioritize(
                        new StayBack(0.7f, 6)
                        ),
                    new TimedTransition("blindCharge", 4000)
                    ),
                    new State("blindCharge",
                        new Shoot(6, 3, 120, fixedAngle: 0f, rotateAngle: 16f),
                        new Prioritize(
                            new Charge(1.3f, pred: e => e.HasConditionEffect(Common.ConditionEffectIndex.Blind))
                            ),
                        new TimedTransition("blind", 1500)
                    
                    )
                );
            db.Init("Stheno Pet",
                    new State("based",

                        new Shoot(8, 2, shootAngle: 12, cooldown: 300),
                            new Prioritize(
                                new Charge(1.3f, 3, pred: e => e.HasConditionEffect(Common.ConditionEffectIndex.Blind)),
                                new Orbit(1.5f, 14, 20, target: "Stheno the Snake Queen")
                                ),
                        new TimedTransition("based1", 10000)
                    ),
                    new State("based1",
                        new Orbit(0.8f, 2, 20, target: "Stheno the Snake Queen"),
                        new Shoot(8, 2, shootAngle: 12, cooldown: 300),
                        new TimedTransition("based", 5000)
                    ),
                    new State("protect",
                        new Shoot(8, predictive: 0.4f, cooldown: 300),
                        new Prioritize(
                            new Protect(0.6f, "Stheno the Snake Queen", 15, 1),
                            new Orbit(0.8f, 2, 20, target: "Stheno the Snake Queen")
                        )
                    )
                );

        }
    }
}
