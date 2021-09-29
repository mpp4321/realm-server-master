using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
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
                        new Follow(3f, 10, 6),
                        new Shoot(8, 3, shootAngle: 12, cooldown: 300),
                        new Wander(2.0f)
                    ),
                    new Threshold(0.01f, 
                        new ItemLoot("Tincture of Fear", 0.2f),
                        new ItemLoot("Snake Pit Key", 0.01f),
                        new ItemLoot("Ring of Speed", 1)
                    )
                );

        }
    }
}
