using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.Behaviors
{
    class Bonegrind : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {
            db.Init("Butcher", new State("chase",
                    new Charge(1.2f, 20),
                    new Taunt(0.3f, 15000, "Come here you!"),
                    new Shoot(10, 2, 30, cooldown: 600, cooldownVariance: 100)
                ));

            db.Init("Bonegrind the Butcher",
                HPScale.BOSS_HP_SCALE_DEFAULT(),
                new State("wait", 
                    new PlayerWithinTransition(10, "start")
                ),
                new State("start",
                    new Taunt(1.0f, 0, "What am I doing here?"),
                    new Taunt(1.0f, 0, "This isn't the tutorial..."),
                    new Taunt(1.0f, 0, "Welp, time to slay some noobs!"),
                    new ConditionalEffect(Common.ConditionEffectIndex.Invincible, false, 5000),
                    new TimedTransition("fight", 5000)
                ),
                new State("fight", 
                    new TransitionFrom("fight", "chargemanager"),
                    new State("chargemanager",
                        new TransitionFrom("chargemanager", "1"),
                        new State("1",
                            new Charge(1.0f, 30),
                            new TimedTransition("2", 5000)
                        ),
                        new State("2",
                            new Taunt(cooldown: 0, "Oo, a bit tired..."),
                            new Charge(.3f, 30),
                            new TimedTransition("1", 5000)
                        )
                    ),
                    new Shoot(99, 8, 360 / 8, fixedAngle: 0, rotateAngle: 15, cooldown: 400),
                    new Shoot(10, 1, 0, 1, predictive: 0.2f, cooldown: 300, cooldownVariance: 150),
                    new HealthTransition(0.5f, "summon")
                ),
                new State("summon",
                    new Taunt(1.0f, 0, "Now you've done it!"),
                    new Taunt(1.0f, 0, "Butchers! Unite!"),
                    new ConditionalEffect(Common.ConditionEffectIndex.Invincible),
                    new TossObject("Butcher", 3, 0, coolDownOffset: 0, throwEffect: true),
                    new TossObject("Butcher", 3, 90, coolDownOffset: 200, throwEffect: true),
                    new TossObject("Butcher", 3, 180, coolDownOffset: 400, throwEffect: true),
                    new TossObject("Butcher", 3, 270, coolDownOffset: 600, throwEffect: true),
                    new TimedTransition("fight2", 800)
                ),
                new State("fight2", 
                    new Charge(1.2f, 30),
                    new Shoot(99, 8, 45, fixedAngle: 0, rotateAngle: 15, cooldown: 200),
                    new Shoot(10, 1, 0, 1, predictive: 0.2f, cooldown: 200, cooldownVariance: 150)
                ),
                new ItemLoot("Haunted Silo", 0.01f, 0.03f),
                new Threshold(0.01f, 
                    new ItemLoot("Potion of Attack", 1.0f, min: 3),
                    new ItemLoot("Backpack", 0.1f),
                    new ItemLoot("Magic Mushroom", 0.75f),
                    new ItemLoot("Bone Dagger", 0.1f),
                    new ItemLoot("Spirit Dagger", 0.05f),
                    new ItemLoot("Spectral Cloth Armor", 0.05f),
                    new ItemLoot("Realm Equipment Crystal", 0.75f),
                    new ItemLoot("(Green) UT Egg", 0.1f, 0.01f),
                    new ItemLoot("(Blue) RT Egg", 0.03f, 0.01f)
                )
           );
        }
    }
}
