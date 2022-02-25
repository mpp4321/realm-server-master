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
                    new Shoot(5, 2, shootAngle: 30, cooldown: 300),
                    new Prioritize(
                            new Follow(1.5f, 7, 2),
                            new Wander(0.2f)
                        )
                ));

            db.Init("Brute Warrior of the Abyss", new State("base",
                    new Shoot(5, 4, cooldown: 200, fixedAngle: 0f, rotateAngle: 45f/2),
                    new Prioritize(
                            new Charge(0.7, 8, 500),
                            new Wander(0.3f)
                        )
                ));

            db.Init("Demon Mage of the Abyss", new State("base",
                    new Shoot(5, 1, cooldown: 150, fixedAngle: 0f, rotateAngle: 45f/2),
                    new Prioritize(
                            new Wander(0.2f)
                        )
                ));

            db.Init("Demon Warrior of the Abyss", new State("base",
                    new Shoot(5, 2, shootAngle: 30, cooldown: 800),
                    new Grenade(10, 50, 3, cooldown: 250),
                    new Prioritize(
                            new Orbit(1.2f, 4, radiusVariance: 3),
                            new Wander(0.2f)
                        )
                ));

            db.Init("Demon of the Abyss", new State("base",
                    new Shoot(5, 2, shootAngle: 30, cooldown: 1000),
                    new Grenade(10, 75, 5, cooldown: 1200),
                    new Prioritize(
                            new Follow(1.5f, 5, 4),
                            new Wander(0.2f)
                        )
                ));

            db.Init("Imp of the Abyss", new State("base",
                    new Shoot(6, 5, shootAngle: 22, cooldown: 750),
                    new Prioritize(
                            new Follow(1.5f, 5, 4),
                            new Wander(0.2f)
                        )
                ));

            db.Init("Malphas Protector",
                new State("base",
                    new Grenade(8, 65, 3, cooldown: 750),
                    new Prioritize(
                            new StayBack(1, 7)
                        ),
                    new TimedTransition("Slam", 3500)
                ),
                new State("Slam",
                    new Shoot(8, 4, 245, cooldown: 100),
                    new Charge(1.5f, 15, coolDown: 100),
                    new TimedTransition("base", 700)
                    )
                );

            db.Init("Archdemon Malphas",
                HPScale.BOSS_HP_SCALE_DEFAULT(),
                new State("Waiting",
                    new PlayerWithinTransition(14, "base", seeInvis: true)
                ),
                new State("base",
                    new Shoot(10, 1, 5, 3, fixedAngle: 0f, rotateAngle: 12f / 2, cooldown: 150),
                    new Shoot(10, 1, 5, 3, fixedAngle: 0f, rotateAngle: -26f / 2, cooldown: 150),
                    new Shoot(10, 1, 5, 3, fixedAngle: 0f, rotateAngle: -12f / 2, cooldown: 150),
                    new Shoot(10, 1, 5, 3, fixedAngle: 0f, rotateAngle: 26f / 2, cooldown: 150),
                    new Grenade(10, 100, 2, cooldown: 400),
                    new Prioritize(
                            new Follow(1.5f, 5, 4),
                            new Wander(0.2f)
                        ),
                    new HealthTransition(0.25f, "rage"),
                    new HealthTransition(0.80f, "burst"),
                    new TimedTransition("prepburst", 8000)
                ),
                new State("prepburst", 
                    new ConditionalEffect(Common.ConditionEffectIndex.Invulnerable),
                    new Flash(0xffff0000, 0.3f, 3),
                    new TimedTransition("burst", 1000)
                ),
                new State("burst", 
                        new Shoot(10, 5, 5, fixedAngle: 0f, rotateAngle: 20f / 2, index: 0),
                        new TossObject("Brute Warrior of the Abyss", range: 5, cooldown: 5000),
                        new Spawn("Malphas Protector", 2, cooldown: 2000),
                        new HealthTransition(0.25f, "rage"),
                        new TimedTransition("burst1", 4000)
                ),

                new State("burst1",
                        new Shoot(10, 5, 5, fixedAngle: 0f, rotateAngle: 20f / 2, index: 0),
                        new Spawn("Malphas Protector", 2, cooldown: 1000),
                        new HealthTransition(0.25f, "rage"),
                        new TimedTransition("burst2", 7000)

                ),

                new State("burst2",
                        new Shoot(10, 5, 5, fixedAngle: 0f, rotateAngle: -20f / 2, index: 0),
                        new Spawn("Malphas Protector", 2, cooldown: 1000),
                        new HealthTransition(0.25f, "rage"),
                        new TimedTransition("burst1", 7000)

                ),

                new State("rage",
                    new ChangeSize(50, 150),
                    new Shoot(10, 4, index: 1, shootAngle: 90f, fixedAngle: 0, rotateAngle: 195f, cooldown: 200),
                    new Shoot(10, 2, shootAngle: 30, index: 0, predictive: 1f, cooldown: 500),
                    new ConditionalEffect(Common.ConditionEffectIndex.Damaging),
                    new ConditionalEffect(Common.ConditionEffectIndex.Armored),
                    new Charge(1.8f, 15, coolDown: new wServer.logic.Cooldown(1500, 1000)),
                    new Flash(0xffff0505, 0.5, 2)
                ),
                    new ItemLoot("Demon Frog Generator", 0.02f, 0.01f),
                    new ItemLoot("Demon Blade", 0.05f, 0.01f),
                    new ItemLoot("Potion of Life", 0.05f, 0.01f),
                    new Threshold(0.01f, 
                        new ItemLoot("Realm Equipment Crystal", 0.2f),
                        new ItemLoot("(Green) UT Egg", 0.03f, 0.01f),
                        new ItemLoot("(Blue) RT Egg", 0.005f, 0.01f),
                        new ItemLoot("The War Path", 0.005f),
                        new ItemLoot("Exuberant Heavy Plate", 0.005f),
                        new ItemLoot("The Horned Circlet", 0.005f),
                        new ItemLoot("Abyssal Emblem", 0.005f),
                        new ItemLoot("Realm Equipment Crystal", 0.05f)
                    ),
                    new Threshold(0.5f, 
                        new ItemLoot("Blazed Bow", 0.005f)
                    ),
                    new Threshold(0.01f,
                        new ItemLoot("Potion of Defense", 0.75f),
                        new ItemLoot("Potion of Vitality", 0.75f),
                        new ItemLoot("Potion of Defense", 0.1f),
                        new ItemLoot("Potion of Vitality", 0.1f)
                    ),
                    new TopDamagersOnly(1, new ItemLoot("Potion of Life", 0.5f, 0.001f))
                );

        }
    }
}
