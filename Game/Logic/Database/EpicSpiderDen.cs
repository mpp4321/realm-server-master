using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RotMG.Game.Logic.Database
{
    class EpicSpiderDen : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {
            db.Init("Crawling Depths Egg Sac",
                new State("base",
                    new PlayerWithinTransition(2.5f, "pop"),
                    new HealthTransition(0.35f, "pop")
                ),
                new State("pop",
                    new Spawn("Crawling Spider Hatchling", 12, 0.4f, 1000)
            ));
            db.Init("Crawling Spider Hatchling",
                new ConditionalEffect(Common.ConditionEffectIndex.Armored, false, 2500),
                new ConditionalEffect(Common.ConditionEffectIndex.Slowed, false, 2500),
                new State("base",
                new Wander(0.4f),
                    new Prioritize(
                        new Follow(0.8f, 7, 3, 5000, 2000)
                        ),
                    new Shoot(8, 2, 12, 1, predictive: 0.6f, cooldownVariance: 250, cooldown: 750),
                    new Shoot(10, predictive: 0.85f, cooldownVariance: 350, cooldown: 800)
            ));
            db.Init("Crawling Green Spider",
                new Wander(0.6f),
                new State("base",
                    new Follow(1.2f, 12, 3.5f, 6000, 2500),
                    new Shoot(6, 3, 14, cooldownVariance: 500, cooldown: 1500),
                    new Shoot(12, predictive: 1, cooldownVariance: 500, cooldown: 800)
            ));
            db.Init("Crawling Grey Spider",
                new Wander(0.8f),
                new State("base",
                    new Shoot(9, 1, predictive: 0.8f, cooldownVariance: 350, cooldown: 1000),
                    new TimedTransition("charge", 3000)
                ),
                new State("charge",
                    new Charge(1.4f, 8, 1500),
                    new Follow(1.2f, 5, 0, 1500),
                    new Shoot(6, 6, 360 / 6, 1, cooldown: 100),
                    new TimedTransition("base", 1500)
            ));
            db.Init("Crawling Red Spotted Spider",
                new Wander(0.4f),
                new StayBack(1.4f, 4),
                new Follow(2, range: 6, duration: 500, cooldown: 1500),
                new State("base",
                    new Shoot(9, 2, 18, predictive: 0.5f, cooldown: 700)
            ));
            db.Init("Crawling Grey Spotted Spider",
                new Wander(0.4f),
                new Follow(1.2f, range: 3, duration: 2500, cooldown: 4000),
                new State("base",
                    new Shoot(6, 3, 24, predictive: 0.3f, cooldown: 800)
            ));
            db.Init("Silver Son of Arachna Giant Egg Sac",
                new State("base",
                    new PlayerWithinTransition(8, "spawn", true)
                ),
                new State("spawn",
                    new Spawn("Crawling Grey Spider", 3, 1, 5000)
            ));
            db.Init("Yellow Son of Arachna Giant Egg Sac",
                new State("base",
                    new PlayerWithinTransition(8, "spawn", true)
                ),
                new State("spawn",
                    new Spawn("Crawling Grey Spotted Spider", 3, 1, 5000)
            ));
            db.Init("Blue Son of Arachna Giant Egg Sac",
                new State("base",
                    new PlayerWithinTransition(8, "spawn", true)
                ),
                new State("spawn",
                    new Spawn("Crawling Spider Hatchling", 8, 1, 1500)
            ));
            db.Init("Red Son of Arachna Giant Egg Sac",
                new State("base",
                    new PlayerWithinTransition(8, "spawn", true)
                ),
                new State("spawn",
                    new Spawn("Crawling Red Spotted Spider", 3, 1, 5000)
            ));
            db.Init("Son of Arachna",
                new State("sleep",
                    new ConditionalEffect(Common.ConditionEffectIndex.Armored),
                    new PlayerWithinTransition(8, "awake", true)
                ),
                new State("awake",
                    new Shoot(16, 6, 360 / 6, 6, 0f, 6f),
                    new TimedRandomTransition(3000, "chase", "wander", "stayback")
                ),
                new State("stayback",
                    new Wander(0.5f),
                    new StayBack(1.3f, 5),
                    new Shoot(16, 8, 360 / 8, 0, 0f, 30f, cooldown: 800),
                    new Shoot(16, 1, index: 1, predictive: 1, cooldownVariance: 250, cooldown: 1000),
                    new TimedRandomTransition(6000, "chase", "wander", "awake")
                ),
                new State("wander",
                    new Wander(1.2f),
                    new StayBack(0.6f, 2),
                    new Shoot(16, 6, 360 / 6, 6, 0f, 6f),
                    new Shoot(16, 1, index: 1, predictive: 1, cooldownVariance: 350, cooldown: 1200),
                    new TimedRandomTransition(4000, "chase", "awake", "stayback")
                ),
                new State("chase",
                    new Wander(0.3f),
                    new Follow(0.8f, 16, 4, 2000, 700),
                    new Charge(1.2f, 16, 4000),
                    new Shoot(16, 1, index: 1, predictive: 1, cooldownVariance: 350, cooldown: 1200),
                    new TimedRandomTransition(8000, "awake", "wander", "stayback")
                ),
                new Threshold(0.01f,
                    new ItemLoot("Muramasa", 0.01f)
                    ));
            db.Init("Epic Arachna Web Spoke 1",
                new State("base",
                    new PlayerWithinTransition(16, "web", true)
                ),
                new State("web",
                    new Shoot(16, 1)
                )
            );

        }
    }
}
