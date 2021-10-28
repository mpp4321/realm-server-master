using RotMG.Game.Logic.Behaviors;
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

        }
    }
}
