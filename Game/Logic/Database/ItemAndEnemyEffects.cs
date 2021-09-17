using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Transitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.Database
{
    class ItemAndEnemyEffects : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {

            db.Init("Mini Flying Brain",
                new State("Base", 
                        new Wander(0.4f),
                        new Shoot(12, count: 5, shootAngle: 72, cooldown: 500, playerOwner: e => e.PlayerOwner),
                        new TimedTransition("Die", 5000)
                    ),
                    new State("Die", new Suicide())
                );

        }
    }
}
