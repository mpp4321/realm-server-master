
using RotMG.Common;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;
using RoTMG.Game.Logic.Transitions;
using wServer.logic;
using static RotMG.Game.Logic.Loots.TierLoot;

namespace RotMG.Game.Logic.Database
{
    class MadLab : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {
            db.Init("Dr Terrible",
                HPScale.BOSS_HP_SCALE_DEFAULT(),
                new State("base",
                    new HealthTransition(.2f, "rage"),
                    new State("idle",
                        new PlayerWithinTransition(12, "GP", true)
                        ),
                    new State("rage",
                        new OrderOnEntry(100, "Monster Cage", "no spawn"),
                        new OrderOnEntry(100, "Dr Terrible Bubble", "nothing change"),
                        new OrderOnEntry(100, "Red Gas Spawner UL", "OFF"),
                        new OrderOnEntry(100, "Red Gas Spawner UR", "OFF"),
                        new OrderOnEntry(100, "Red Gas Spawner LL", "OFF"),
                        new OrderOnEntry(100, "Red Gas Spawner LR", "OFF"),
                        new Wander(0.5f),
                        new SetAltTexture(0),
                        new TossObject("Green Potion", cooldown: 1500, coolDownOffset: 0, throwEffect: true),
                        new TimedTransition("rage TA", 12000)
                        ),
                    new State("rage TA",
                        new OrderOnEntry(100, "Monster Cage", "no spawn"),
                        new OrderOnEntry(100, "Dr Terrible Bubble", "nothing change"),
                        new OrderOnEntry(100, "Red Gas Spawner UL", "OFF"),
                        new OrderOnEntry(100, "Red Gas Spawner UR", "OFF"),
                        new OrderOnEntry(100, "Red Gas Spawner LL", "OFF"),
                        new OrderOnEntry(100, "Red Gas Spawner LR", "OFF"),
                        new Wander(0.5f),
                        new SetAltTexture(0),
                        new TossObject("Turret Attack", cooldown: 1500, coolDownOffset: 0, throwEffect: true),
                        new TimedTransition("rage", 10000)
                        ),
                    new State("GP",
                        new OrderOnEntry(100, "Monster Cage", "no spawn"),
                        new OrderOnEntry(100, "Dr Terrible Bubble", "nothing change"),
                        new OrderOnEntry(100, "Red Gas Spawner UL", "OFF"),
                        new OrderOnEntry(100, "Red Gas Spawner UR", "ON"),
                        new OrderOnEntry(100, "Red Gas Spawner LL", "ON"),
                        new OrderOnEntry(100, "Red Gas Spawner LR", "ON"),
                        new Wander(0.5f),
                        new SetAltTexture(0),
                        new Taunt(0.5f, cooldown: 0, "For Science"),
                        new TossObject("Green Potion", cooldown: 2000, coolDownOffset: 0, throwEffect: true),
                        new TimedTransition("TA", 12000)
                        ),
                    new State("TA",
                        new OrderOnEntry(100, "Monster Cage", "no spawn"),
                        new OrderOnEntry(100, "Dr Terrible Bubble", "nothing change"),
                        new OrderOnEntry(100, "Red Gas Spawner UL", "OFF"),
                        new OrderOnEntry(100, "Red Gas Spawner UR", "ON"),
                        new OrderOnEntry(100, "Red Gas Spawner LL", "ON"),
                        new OrderOnEntry(100, "Red Gas Spawner LR", "ON"),
                        new Wander(0.5f),
                        new SetAltTexture(0),
                        new TossObject("Turret Attack", cooldown: 2000, coolDownOffset: 0, throwEffect: true),
                        new TimedTransition("hide", 10000)
                        ),
                    new State("hide",
                        new OrderOnEntry(100, "Monster Cage", "spawn"),
                        new OrderOnEntry(100, "Dr Terrible Bubble", "Bubble time"),
                        new OrderOnEntry(100, "Red Gas Spawner UL", "OFF"),
                        new OrderOnEntry(100, "Red Gas Spawner UR", "OFF"),
                        new OrderOnEntry(100, "Red Gas Spawner LL", "OFF"),
                        new OrderOnEntry(100, "Red Gas Spawner LR", "OFF"),
                        new ReturnToSpawn(speed: 1),
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new SetAltTexture(1),
                        new TimedTransition("nohide", 15000)
                        ),
                    new State("nohide",
                        new OrderOnEntry(100, "Monster Cage", "no spawn"),
                        new OrderOnEntry(100, "Dr Terrible Bubble", "nothing change"),
                        new OrderOnEntry(100, "Red Gas Spawner UL", "ON"),
                        new OrderOnEntry(100, "Red Gas Spawner UR", "OFF"),
                        new OrderOnEntry(100, "Red Gas Spawner LL", "ON"),
                        new OrderOnEntry(100, "Red Gas Spawner LR", "ON"),
                        new Wander(0.5f),
                        new SetAltTexture(0),
                        new TossObject("Green Potion", cooldown: 2000, coolDownOffset: 0, throwEffect: true),
                        new TimedTransition("TA2", 12000)
                        ),
                    new State("TA2",
                        new OrderOnEntry(100, "Monster Cage", "no spawn"),
                        new OrderOnEntry(100, "Dr Terrible Bubble", "nothing change"),
                        new OrderOnEntry(100, "Red Gas Spawner UL", "ON"),
                        new OrderOnEntry(100, "Red Gas Spawner UR", "OFF"),
                        new OrderOnEntry(100, "Red Gas Spawner LL", "ON"),
                        new OrderOnEntry(100, "Red Gas Spawner LR", "ON"),
                        new Wander(0.5f),
                        new SetAltTexture(0),
                        new TossObject("Green Potion", cooldown: 2000, coolDownOffset: 0, throwEffect: true),
                        new TimedTransition("hide2", 10000)
                        ),
                    new State("hide2",
                        new OrderOnEntry(100, "Monster Cage", "spawn"),
                        new OrderOnEntry(100, "Dr Terrible Bubble", "Bubble time"),
                        new OrderOnEntry(100, "Red Gas Spawner UL", "OFF"),
                        new OrderOnEntry(100, "Red Gas Spawner UR", "OFF"),
                        new OrderOnEntry(100, "Red Gas Spawner LL", "OFF"),
                        new OrderOnEntry(100, "Red Gas Spawner LR", "OFF"),
                        new ReturnToSpawn(speed: 1),
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new SetAltTexture(1),
                        new TimedTransition("nohide2", 15000)
                        ),
                    new State("nohide2",
                        new OrderOnEntry(100, "Monster Cage", "no spawn"),
                        new OrderOnEntry(100, "Dr Terrible Bubble", "nothing change"),
                        new OrderOnEntry(100, "Red Gas Spawner UL", "ON"),
                        new OrderOnEntry(100, "Red Gas Spawner UR", "ON"),
                        new OrderOnEntry(100, "Red Gas Spawner LL", "OFF"),
                        new OrderOnEntry(100, "Red Gas Spawner LR", "ON"),
                        new Wander(0.5f),
                        new SetAltTexture(0),
                        new TossObject("Green Potion", cooldown: 2000, coolDownOffset: 0, throwEffect: true),
                        new TimedTransition("TA3", 12000)
                        ),
                    new State("TA3",
                        new OrderOnEntry(100, "Monster Cage", "no spawn"),
                        new OrderOnEntry(100, "Dr Terrible Bubble", "nothing change"),
                        new OrderOnEntry(100, "Red Gas Spawner UL", "ON"),
                        new OrderOnEntry(100, "Red Gas Spawner UR", "ON"),
                        new OrderOnEntry(100, "Red Gas Spawner LL", "OFF"),
                        new OrderOnEntry(100, "Red Gas Spawner LR", "ON"),
                        new Wander(0.5f),
                        new SetAltTexture(0),
                        new TossObject("Green Potion", cooldown: 2000, coolDownOffset: 0, throwEffect: true),
                        new TimedTransition("hide3", 10000)
                        ),
                    new State("hide3",
                        new OrderOnEntry(100, "Monster Cage", "spawn"),
                        new OrderOnEntry(100, "Dr Terrible Bubble", "Bubble time"),
                        new OrderOnEntry(100, "Red Gas Spawner UL", "OFF"),
                        new OrderOnEntry(100, "Red Gas Spawner UR", "OFF"),
                        new OrderOnEntry(100, "Red Gas Spawner LL", "OFF"),
                        new OrderOnEntry(100, "Red Gas Spawner LR", "OFF"),
                        new ReturnToSpawn(speed: 1),
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new SetAltTexture(1),
                        new TimedTransition("nohide3", 15000)
                        ),
                    new State("nohide3",
                        new OrderOnEntry(100, "Monster Cage", "no spawn"),
                        new OrderOnEntry(100, "Dr Terrible Bubble", "nothing change"),
                        new OrderOnEntry(100, "Red Gas Spawner UL", "ON"),
                        new OrderOnEntry(100, "Red Gas Spawner UR", "ON"),
                        new OrderOnEntry(100, "Red Gas Spawner LL", "ON"),
                        new OrderOnEntry(100, "Red Gas Spawner LR", "OFF"),
                        new Wander(0.5f),
                        new SetAltTexture(0),
                        new TossObject("Green Potion", cooldown: 2000, coolDownOffset: 0, throwEffect: true),
                        new TimedTransition("TA4", 12000)
                        ),
                    new State("TA4",
                        new OrderOnEntry(100, "Monster Cage", "no spawn"),
                        new OrderOnEntry(100, "Dr Terrible Bubble", "nothing change"),
                        new OrderOnEntry(100, "Red Gas Spawner UL", "ON"),
                        new OrderOnEntry(100, "Red Gas Spawner UR", "ON"),
                        new OrderOnEntry(100, "Red Gas Spawner LL", "ON"),
                        new OrderOnEntry(100, "Red Gas Spawner LR", "OFF"),
                        new Wander(0.5f),
                        new SetAltTexture(0),
                        new TossObject("Green Potion", cooldown: 2000, coolDownOffset: 0, throwEffect: true),
                        new TimedTransition("hide4", 10000)
                        ),
                    new State("hide4",
                        new OrderOnEntry(100, "Monster Cage", "spawn"),
                        new OrderOnEntry(100, "Dr Terrible Bubble", "Bubble time"),
                        new OrderOnEntry(100, "Red Gas Spawner UL", "OFF"),
                        new OrderOnEntry(100, "Red Gas Spawner UR", "OFF"),
                        new OrderOnEntry(100, "Red Gas Spawner LL", "OFF"),
                        new OrderOnEntry(100, "Red Gas Spawner LR", "OFF"),
                        new ReturnToSpawn(speed: 1),
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new SetAltTexture(1),
                        new TimedTransition("idle", 15000)
                        )
                    ),
                    new Threshold(0.0001f,
                        new ItemLoot("Conducting Wand", 0.01f, threshold: 0.01f),
                        new ItemLoot("Scepter of Fulmination", 0.02f, threshold: 0.01f),
                        new ItemLoot("Robe of the Mad Scientist", 0.02f, threshold: 0.01f),
                        new ItemLoot("Experimental Ring", 0.05f, threshold: 0.01f),//Power Battery
                        new ItemLoot("Wine Cellar Incantation", 0.05f),
                        //new ItemLoot("Power Battery", 0.0045f, threshold: 0.01f),
                        new ItemLoot("Potion of Wisdom", 1f),
                        new TierLoot(8, LootType.Weapon, 0.25f),
                        new TierLoot(9, LootType.Weapon, 0.125f),
                        new TierLoot(10, LootType.Weapon, 0.0625f),
                        new TierLoot(11, LootType.Weapon, 0.0625f),
                        new TierLoot(8, LootType.Armor, 0.25f),
                        new TierLoot(9, LootType.Armor, 0.25f),
                        new TierLoot(10, LootType.Armor, 0.125f),
                        new TierLoot(11, LootType.Armor, 0.125f),
                        new TierLoot(4, LootType.Ability, 0.125f),
                        new TierLoot(5, LootType.Ability, 0.0625f)
                        )
                );
            db.Init("Dr Terrible Mini Bot",
                new State("base",
                     new Wander(0.5f),
                     new Shoot(10, 2, 20, angleOffset: 0 / 2, index: 0, cooldown: 1000)
                     )
                );
            db.Init("Dr Terrible Rampage Cyborg",
                new State("base",
                    new State("idle",
                        new PlayerWithinTransition(10, "Hp_Check"),
                    new State("Hp_Check",
                        new HealthTransition(.2f, "blink"),
                        new State("normal",
                            new Wander(0.5f),
                            new Follow(0.6f, range: 1, duration: 5000, cooldown: 0),
                            new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, index: 0, predictive: 1,
                            cooldown: 800, cooldownOffset: 0),
                            new TimedTransition("rage blink", 10000)
                            ),
                        new State("rage blink",
                            new Wander(0.5f),
                            new Flash(0xf0e68c, flashRepeats: 5, flashPeriod: 0.1f),
                            new Follow(0.6f, range: 1, duration: 5000, cooldown: 0),
                            new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, index: 1, predictive: 1,
                            cooldown: 800, cooldownOffset: 0),
                            new TimedTransition("rage", 3000)
                            ),
                        new State("rage",
                             new Wander(0.5f),
                            new Flash(0xf0e68c, flashRepeats: 5, flashPeriod: 0.1f),
                            new Follow(0.6f, range: 1, duration: 5000, cooldown: 0),
                            new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, index: 1, predictive: 1,
                            cooldown: 800, cooldownOffset: 0)
                            )
                        ),
                    new State("blink",
                        new Flash(0xfFF0000, flashRepeats: 10000, flashPeriod: 0.1f),
                        new TimedTransition("explode", 2000)
                        ),
                    new State("explode",
                        new Flash(0xfFF0000, 1, 9000001),
                        new Shoot(10, count: 8, index: 2, fixedAngle: 0),
                        new Suicide()
                    )
                        )
                )
                ); ;
            db.Init("Dr Terrible Escaped Experiment",
                  new State("base",
                      new Wander(0.5f),
                      new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, index: 0, predictive: 1,
                      cooldown: 800, cooldownOffset: 0)
                      )
                   );
            db.Init("Mini Bot",
            new State("base",
                 new Wander(0.5f),
                 new Shoot(10, 2, 20, angleOffset: 0 / 2, index: 0, cooldown: 1000)
                 )
            );
            db.Init("Rampage Cyborg",
               new State("base",
                   new State("idle",
                       new PlayerWithinTransition(10, "normal"),
                   new State("normal",
                       new Wander(0.5f),
                       new Follow(0.6f, range: 1, duration: 5000, cooldown: 0),
                       new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, index: 0, predictive: 1,
                       cooldown: 800, cooldownOffset: 0),
                       new HealthTransition(.2f, "blink"),
                       new TimedTransition("rage blink", 10000)
                       ),
                   new State("rage blink",
                       new Wander(0.5f),
                       new Flash(0xf0e68c, flashRepeats: 5, flashPeriod: 0.1f),
                       new Follow(0.6f, range: 1, duration: 5000, cooldown: 0),
                       new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, index: 1, predictive: 1,
                       cooldown: 800, cooldownOffset: 0),
                       new HealthTransition(.2f, "blink"),
                       new TimedTransition("rage", 3000)
                       ),
                   new State("rage",
                       new Wander(0.5f),
                       new Flash(0xf0e68c, flashRepeats: 5, flashPeriod: 0.1f),
                       new Follow(0.6f, range: 1, duration: 5000, cooldown: 0),
                       new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, index: 1, predictive: 1,
                       cooldown: 800, cooldownOffset: 0),
                       new HealthTransition(.2f, "blink")
                       ),
                   new State("blink",
                       new Wander(0.5f),
                       new Follow(0.6f, range: 1, duration: 5000, cooldown: 0),
                       new Flash(0xfFF0000, flashRepeats: 10000, flashPeriod: 0.1f),
                       new TimedTransition("explode", 2000)
                       ),
                   new State("explode",
                       new Flash(0xfFF0000, 1, 9000001),
                       new Shoot(10, count: 8, index: 2, fixedAngle: 0),
                       new Suicide()
                   )
                       )
               )
                   );
            db.Init("Escaped Experiment",
                  new State("base",
                      new Wander(0.5f),
                      new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, index: 0, predictive: 1,
                      cooldown: 800, cooldownOffset: 0)
                      )
                );
            db.Init("West Automated Defense Turret",
                    new State("base",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Shoot(32, fixedAngle: 0, cooldown: 3000, cooldownVariance: 1000)
                        )
                );
            db.Init("East Automated Defense Turret",
                    new State("base",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Shoot(32, fixedAngle: 180, cooldown: 3000, cooldownVariance: 1000)
                        )
                );
            db.Init("Crusher Abomination",
                new State("base",
                    new State("1 step",
                        new Wander(0.5f),
                        new Shoot(10, 3, 20, angleOffset: 0 / 3, index: 0, cooldown: 1000),
                        new HealthTransition(.75f, "2 step")
                        ),
                    new State("2 step",
                        new Wander(0.5f),
                        new ChangeSize(11, 150),
                        new Shoot(10, 2, 20, angleOffset: 0 / 3, index: 1, cooldown: 1000),
                        new HealthTransition(.5f, "3 step")
                        ),
                    new State("3 step",
                         new Wander(0.5f),
                        new ChangeSize(11, 175),
                        new Shoot(10, 2, 20, angleOffset: 0 / 3, index: 2, cooldown: 1000),
                        new HealthTransition(.25f, "4 step")
                        ),
                    new State("4 step",
                        new Wander(0.5f),
                        new ChangeSize(11, 200),
                        new Shoot(10, 2, 20, angleOffset: 0 / 3, index: 3, cooldown: 1000)
                        )
                    )
                );
            db.Init("Enforcer Bot 3000",
                new State("base",
                    new Wander(0.5f),
                    new Shoot(10, 3, 20, angleOffset: 0 / 3, index: 0, cooldown: 1000),
                    new Shoot(10, 4, 20, angleOffset: 0 / 4, index: 1, cooldown: 1000),
                    new TransformOnDeath("Mini Bot", 0, 3)

                    )
                );
            db.Init("Green Potion",
                new State("base",
                    new State("Idle",
                        new Flash(0x001DFF, .1f, 15),
                        new TimedTransition("explode", 2000)
                        ),
                    new State("explode",
                          new Shoot(10, count: 6, index: 0, fixedAngle: 0),
                          new Suicide()
                        )
                    )
                );
            db.Init("Red Gas Spawner UL",
                new State("base",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new EntitiesNotExistsTransition(50, "OFF", "Dr Terrible"),
                    new State("OFF"),
                    new State("ON",
                        new Shoot(10, count: 18, index: 0, fixedAngle: 0, angleOffset: 0, cooldown: 1000)
                    )
                    )
            );
            db.Init("Red Gas Spawner UR",
               new State("base",
                   new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new EntitiesNotExistsTransition(50, "OFF", "Dr Terrible"),
                    new State("OFF"),
                    new State("ON",
                        new Shoot(10, count: 18, index: 0, fixedAngle: 0, angleOffset: 0, cooldown: 1000)
                    )
                    )
            );
            db.Init("Red Gas Spawner LL",
                new State("base",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new EntitiesNotExistsTransition(50, "OFF", "Dr Terrible"),
                    new State("OFF"),
                    new State("ON",
                        new Shoot(10, count: 18, index: 0, fixedAngle: 0, angleOffset: 0, cooldown: 1000)
                    )
                    )
            );
            db.Init("Red Gas Spawner LR",
                new State("base",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new EntitiesNotExistsTransition(50, "OFF", "Dr Terrible"),
                    new State("OFF"),
                    new State("ON",
                        new Shoot(10, count: 18, index: 0, fixedAngle: 0, angleOffset: 0, cooldown: 1000)
                    )
                )
            );
            db.Init("Turret Attack",
                new State("base",
                    new Shoot(10, 2, 35, angleOffset: 0 / 2, index: 0, cooldown: 2000)
                )
            );
            db.Init("Mad Scientist Summoner",
                new State("base",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new DropPortalOnDeath("Glowing Realm Portal"),
                    new State("idle",
                         new EntitiesNotExistsTransition(300, "Death", "Dr Terrible")
                        ),
                    new State("Death",
                        new Decay(0)
                        )
                    )
                );
            db.Init("Dr Terrible Bubble",
                new State("base",
                    new State("nothing change",
                        new ConditionalEffect(ConditionEffectIndex.Invincible)
                        //new SetAltTexture(0)
                        ),
                    new State("Bubble time",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        //new SetAltTexture(1),
                        new TimedTransition("Bubble time2", 1000)
                        ),
                    new State("Bubble time2",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        //new SetAltTexture(2),
                        new TimedTransition("Bubble time", 1000)
                    )
                )
                );
            db.Init("Mad Gas Controller", //don't need xD
                new State("base",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                   )
               );
            db.Init("Monster Cage",
                new State("base",
                    new State("no spawn"),
                    new State("spawn",
                        // new SetAltTexture(2),
                        new Spawn("Dr Terrible Rampage Cyborg", maxChildren: 1, initialSpawn: 1),
                        new Spawn("Dr Terrible Mini Bot", maxChildren: 1, initialSpawn: 1),
                        new Spawn("Dr Terrible Escaped Experiment", maxChildren: 1, initialSpawn: 1)
                        )
                    )
                );
            db.Init("Mad Lab Open Wall",
                new ClearRectangleOnDeath(new IntPoint(10, 21), new IntPoint(13, 21)),
                new State("base",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("1",
                        new EntitiesNotExistsTransition(99, "2", "Dr Terrible")
                    ),
                    new State("2",
                        new Suicide()
                    )
                )
            );
            db.Init("Horrific Creation Summoner",
                new State("base",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new State("1",
                        new EntitiesNotExistsTransition(99, "2", "Dr Terrible")
                        ),
                    new State("2",
                        new Transform("Horrific Creation")
                        )
                   )
               );
            db.Init("Horrific Creation",
                HPScale.BOSS_HP_SCALE_DEFAULT(),
                new State("base",
                    new StayCloseToSpawn(0.5f, 9),
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Taunt(true, cooldown: 0, "WHO KILL MASTER? RAHHHH!"),
                        new EntitiesNotExistsTransition(9999, "2", "Tesla Coil")
                        ),
                    new State("2",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Taunt(true, cooldown: 0, "My door is open. Come let me crush you!"),
                        new TimedTransition("3", 0)
                        ),
                    new State("3",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TransitionFrom("3", "4"),
                        new EntitiesWithinTransition(1, "Hexxer", "10"),
                        new State("4",
                            new SetAltTexture(0),
                            new Wander(0.1f),
                            new Follow(0.4f, 10, 6),
                            new Shoot(10, 3, 30, 0, cooldown: 1000),
                            new TimedTransition("5", 6000)
                            ),
                        new State("5",
                            new Flash(0xFFFFFF, .1f, 32),
                            new Taunt(true, cooldown: 0, "What, you scared???"),
                            new TimedTransition("6", 4000)
                            ),
                        new State("6",//16 shoots
                            new SetAltTexture(1),
                            new Shoot(12, 1, index: 5, cooldown: 250),
                            new TimedTransition("7", 4000)
                            ),
                        new State("7",
                            new SetAltTexture(0),
                            new Taunt(true, cooldown: 0, "Me can do this all day!"),
                            new Charge(2, 14),
                            new Shoot(7, 8, 45, 4, angleOffset: 45, cooldown: 750),
                            new TimedTransition("8", 3000)
                            ),
                        new State("8",
                            new Taunt(true, cooldown: 0, "Attacks no hurt me!!!!"),
                            new Flash(0xFFFFFF, .1f, 16),
                            new TimedTransition("9", 1000)
                            ),
                        new State("9",
                            new Shoot(999, 8, 45, 3, 0, 0, cooldown: 999999),
                            new TimedTransition("3", 2500) { SubIndex = 3 }
                            )
                        ),
                    new State("10",
                        new TimedRandomTransition(100, "a12", "a13", "a14", "a15", "a16", "a17") { SubIndex = 2 }
                        ),
                    new State("a12",
                        new SetAltTexture(2),
                        new BackAndForth(0.8f, 1),
                        new TimedTransition("BackToBlue", 5000)
                        ),
                    new State("a13",
                        new SetAltTexture(3),
                        new BackAndForth(0.8f, 2),
                        new TimedTransition("BackToBlue", 5000)
                        ),
                    new State("a14",
                        new SetAltTexture(4),
                        new BackAndForth(0.8f, 2),
                        new TimedTransition("BackToBlue", 5000)
                        ),
                    new State("a15",
                        new SetAltTexture(5),
                        new BackAndForth(0.8f, 2),
                        new TimedTransition("BackToBlue", 5000)
                        ),
                    new State("a16",
                        new SetAltTexture(6),
                        new BackAndForth(0.8f, 2),
                        new TimedTransition("BackToBlue", 5000)
                        ),
                    new State("a17",
                        new SetAltTexture(7),
                        new BackAndForth(0.8f, 2),
                        new TimedTransition("BackToBlue", 5000)
                        ),
                    new State("BackToBlue",
                        new MoveLine(2, 0, 14),
                        new EntitiesWithinTransition(2, "DeHexxer", "3"),
                        new TimedTransition("3", 3000)
                        )
                   ),
                    new Threshold(0.01f,
                        new ItemLoot("Conducting Wand", 0.008f, threshold: 0.01f),
                        new ItemLoot("Experimental Ring", 0.04f, threshold: 0.01f),
                        new ItemLoot("Realm Equipment Crystal", 0.01f),
                        new ItemLoot("Wine Cellar Incantation", 0.05f),
                        new ItemLoot("Potion of Wisdom", 1.0f),
                        new ItemLoot("Potion of Defense", 0.3f, 0.5f, min: 3),
                        new ItemLoot("Rune of the Mage", 0.005f),
                        //new ItemLoot("Grotesque Scepter", 0.006f,  threshold: 0.01f),
                        //new ItemLoot("Garment of the Beast", 0.006f, threshold: 0.01f),
                        //new ItemLoot("Power Battery", 0.008f, threshold: 0.01f),
                        new TierLoot(8, LootType.Weapon, 0.25f),
                        new TierLoot(9, LootType.Weapon, 0.125f),
                        new TierLoot(10, LootType.Weapon, 0.0625f),
                        new TierLoot(11, LootType.Weapon, 0.0625f),
                        new TierLoot(8, LootType.Armor, 0.25f),
                        new TierLoot(9, LootType.Armor, 0.25f),
                        new TierLoot(10, LootType.Armor, 0.125f),
                        new TierLoot(11, LootType.Armor, 0.125f),
                        new TierLoot(4, LootType.Ability, 0.125f),
                        new TierLoot(5, LootType.Ability, 0.0625f),
                        new TierLoot(5, LootType.Ring, 0.0625f)
                        )
               );
            db.Init("Hexxer",
                new State("base",
                    new ConditionalEffect(ConditionEffectIndex.Invincible)
                )
            );
            db.Init("DeHexxer",
                new State("base",
                    new ConditionalEffect(ConditionEffectIndex.Invincible)
                )
            );
        }
    }
}
