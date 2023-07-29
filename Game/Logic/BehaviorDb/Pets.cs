using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Transitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.Database
{
    class Pets : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {
            db.Init("Crab", new PetFollow(
                    1f, range: 2
                ));

            db.Init("Panda", new PetFollow(
                    1f, range: 2
                ));

            db.Init("Raven", new PetFollow(
                    1f, range: 2
                ));

            db.Init("Bee", new PetFollow(
                    1f, range: 2
                ));

            db.Init("Baby Dragon", new PetFollow(
                    1f, range: 2
                ));

            db.Init("Demon Frog", new PetFollow(
                    1f, range: 2
                ));

            db.Init("Witch", new PetFollow(
                    1f, range: 2
                ));

            db.Init("Spider", new PetFollow(
                    1f, range: 2
                ));

            db.Init("Sumo Pet", new PetFollow(
                    1f, range: 2
                ));

            db.Init("Realm Reaper Pet", new PetFollow(
                    1f, range: 2
                ));

            db.Init("Parthanax Pet", 
                new State("0",
                    new TransitionOnItemNearby(10f, "Metal Kendo Stick", "say once"),
                    new PetFollow(
                        1f, range: 2
                    )
                ),
                new State("say once",
                    new Taunt(cooldown: 0, "Yum... Kendo Sticks"),
                    new TimedTransition("0", 100)
                )
            );

            db.Init("Blue Snail", new PetFollow(
                1f, range: 2
            ));

        }
    }
}
