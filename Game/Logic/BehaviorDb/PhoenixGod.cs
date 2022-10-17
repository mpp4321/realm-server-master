using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Transitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotMG.Game.Logic.Database
{
    public class PhoenixGod : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {

            db.Init("PhoenixGodPhase1",
                new Orbit(1.0f, 3, 99f, "Phoenix God"),
                new Shoot(0f, 4, 360/4, 0, fixedAngle: 0, 45, cooldown: 500),
                new Shoot(10f, 2, 30, 0, cooldown: 1000, cooldownVariance: 300),
                new Shoot(10f, 1, 30, 0, cooldown: 500, cooldownVariance: 500),
                new TossObject("Birdman Chief", cooldown: 3000),
                new HealEntity(10f, "Phoenix God", 100)
            );

            db.Init("PhoenixGodPhase3",
                new ConditionalEffect(Common.ConditionEffectIndex.Invincible)
            );

            db.Init("Phoenix God",
                    new State("-1",
                        new ConditionalEffect(Common.ConditionEffectIndex.Invulnerable),
                        new Taunt(cooldown: 0, "I have been summoned. Come, adventurers, and meet your despise."),
                        new Shoot(99f, 24, 360f / 24f, index: 2, cooldown: 100000),
                        new Flash(0xffff0000, 100.0, 100),
                        new KeyTransition("phoenixGodTransition1", 1, "-2"),
                        new Prioritize(
                            new QueuedBehav(
                                new CooldownBehav(1200, null),
                                new Taunt(cooldown: 0, "I have been in slumber for eons to meet you, {PLAYER}. Awe at my tremendous power as I burn you down to the ashes you shall be."),
                                new CooldownBehav(1100, null),
                                new ChangeSize(100, 100),
                                new CooldownBehav(1100, null),
                                new ChangeSize(100, 110),
                                new CooldownBehav(1100, null),
                                new ChangeSize(100, 120),
                                new CooldownBehav(1100, null),
                                new ChangeSize(100, 130),
                                new CooldownBehav(1100, null),
                                new ChangeSize(100, 140),
                                new Shoot(99f, 16, 360/16f, index: 5)
                            ),
                            new SetKey("phoenixGodTransition1", 1)
                        )
                    ),
                    new State("-2",
                        new ConditionalEffect(Common.ConditionEffectIndex.Armored),
                        new Wander(0.6f),
                        new QueuedBehav(
                            new CooldownBehav(800, new Spawn("PhoenixGodPhase1", 1, 1f, cooldown: 99999)),
                            new CooldownBehav(800, new Spawn("PhoenixGodPhase1", 1, 1f, cooldown: 99999))
                        ),
                        new TimedTransition("-3", 3000)
                    ),
                    new State("-3",
                        new Wander(0.3f),
                        new QueuedBehav(true,
                            new CooldownBehav(500,
                                new Shoot(10f, 1, 0, index: 2, angleOffset: -90f, rotateAngle: 10f)
                            ),
                            new CooldownBehav(2000, null)
                        ),
                        new Shoot(3f, 3, 15),
                        new HealthTransition(0.6f, "-4"),
                        new EntitiesNotExistsTransition(99f, "-4", "PhoenixGodPhase1")
                    ),
                    new State("-4",
                        new ConditionalEffect(Common.ConditionEffectIndex.Invincible, false, 1000),
                        new QueuedBehav(
                            new CooldownBehav(500, new Taunt(cooldown: 0, "Hm, {PLAYER} it's been millenia since a mortal has been able to match my power.")),
                            new CooldownBehav(500, new Taunt(cooldown: 0, "Then I will show you my true power!"))
                        ),
                        new TimedTransition("-4a", 1100)
                    ),
                    new State("-4a", 
                        new Shoot(10f, 3, 20, index: 2, cooldownOffset: 500, cooldown: 1000),
                        new Shoot(10f, 2, 20, index: 2, cooldownOffset: 500, cooldown: 1000),
                        new Shoot(10f, 8, 360/8, index: 3, cooldown: 300),
                        new Shoot(10f, 8, 360/8, index: 4, angleOffset: 360/16, cooldown: 500),
                        new Follow(0.2f, 99f, 1),
                        new HealthTransition(0.4f, "-5")
                    ),
                    new State("-5")
                );
        }
    }
}
