using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Transitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotMG.Game.Logic.Database
{
    public class MazeEnemies : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {
            db.Init("Armoured Minotaur", 
                    new State("init",
                        new TransitionFrom("init", "1"),
                        new Shoot(10f, 2, 10, index: 3, cooldown: 1000),
                        new Shoot(10f, 8, 45, index: 2, cooldown: 500),
                        new Shoot(10f, 4, 90, index: 0, cooldown: 500, cooldownVariance: 250),
                        new Shoot(10f, 2, 180, angleOffset: 90, index: 1, cooldown: 2000, cooldownVariance: 1000),
                        new State("1",
                            new TimedTransition("2") { SubIndex = 2 },
                            new Prioritize(
                                new Follow(0.4f, 10f, 1f)
                            )
                        ),
                        new State("2",
                            new Prioritize(
                                new Follow(1.0f, 2f)
                            ),
                            new TimedTransition("1") { SubIndex = 2 }
                        )
                    )
                );
        }
    }
}
