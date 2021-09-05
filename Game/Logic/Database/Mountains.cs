using RotMG.Common;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Conditionals;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;
using static RotMG.Game.Logic.Loots.TierLoot;

namespace RotMG.Game.Logic.Database
{
    public class Mountains : IBehaviorDatabase
    {
        //TimedTransition\((\d+),\s(.+)\)
        //TimedTransition($2, $1)

        public void Init(BehaviorDb db)
        {
            db.Init("Lucky Ent God",
                 new State(
                    "base",
                     //new DropPortalOnDeath("Woodland Labyrinth", 100),
                     new Prioritize(
                         new StayAbove(1, 200),
                         new Follow(1, range: 7),
                         new Wander(0.4f)
                         ),
                     new Shoot(12, 5, 10, predictive: 1, cooldown: 1250)

                     ),
                 new Threshold(0.18f,
                     new ItemLoot("Potion of Defense", 1),
                     new ItemLoot("Potion of Attack", 1),
                     new TierLoot(6, LootType.Weapon, 0.2f),
                     new TierLoot(7, LootType.Weapon, 0.2f),
                     new TierLoot(8, LootType.Weapon, 0.2f),
                     new TierLoot(7, LootType.Armor, 0.2f),
                     new TierLoot(8, LootType.Armor, 0.2f),
                     new TierLoot(9, LootType.Armor, 0.2f),
                     new TierLoot(4, LootType.Ability, 0.2f)
                     )
             );
            db.Init("Lucky Djinn",
                  new State(
                      //new DropPortalOnDeath("The Crawling Depths", 100),
                      "base",
                      new State("Idle",
                          new Prioritize(
                              new StayAbove(1, 200),
                              new Wander(0.8f)
                              ),
                          new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                          new PlayerWithinTransition(8, "Attacking")
                          ),
                      new State("Attacking",
                          new State("Bullet",
                              new Shoot(1, 4, cooldown: 10000, fixedAngle: 90, cooldownOffset: 0, shootAngle: 90),
                              new Shoot(1, 4, cooldown: 10000, fixedAngle: 100, cooldownOffset: 200, shootAngle: 90),
                              new Shoot(1, 4, cooldown: 10000, fixedAngle: 110, cooldownOffset: 400, shootAngle: 90),
                              new Shoot(1, 4, cooldown: 10000, fixedAngle: 120, cooldownOffset: 600, shootAngle: 90),
                              new Shoot(1, 4, cooldown: 10000, fixedAngle: 130, cooldownOffset: 800, shootAngle: 90),
                              new Shoot(1, 4, cooldown: 10000, fixedAngle: 140, cooldownOffset: 1000, shootAngle: 90),
                              new Shoot(1, 4, cooldown: 10000, fixedAngle: 150, cooldownOffset: 1200, shootAngle: 90),
                              new Shoot(1, 4, cooldown: 10000, fixedAngle: 160, cooldownOffset: 1400, shootAngle: 90),
                              new Shoot(1, 4, cooldown: 10000, fixedAngle: 170, cooldownOffset: 1600, shootAngle: 90),
                              new Shoot(1, 4, cooldown: 10000, fixedAngle: 180, cooldownOffset: 1800, shootAngle: 90),
                              new Shoot(1, 8, cooldown: 10000, fixedAngle: 180, cooldownOffset: 2000, shootAngle: 45),
                              new Shoot(1, 4, cooldown: 10000, fixedAngle: 180, cooldownOffset: 0, shootAngle: 90),
                              new Shoot(1, 4, cooldown: 10000, fixedAngle: 170, cooldownOffset: 200, shootAngle: 90),
                              new Shoot(1, 4, cooldown: 10000, fixedAngle: 160, cooldownOffset: 400, shootAngle: 90),
                              new Shoot(1, 4, cooldown: 10000, fixedAngle: 150, cooldownOffset: 600, shootAngle: 90),
                              new Shoot(1, 4, cooldown: 10000, fixedAngle: 140, cooldownOffset: 800, shootAngle: 90),
                              new Shoot(1, 4, cooldown: 10000, fixedAngle: 130, cooldownOffset: 1000, shootAngle: 90),
                              new Shoot(1, 4, cooldown: 10000, fixedAngle: 120, cooldownOffset: 1200, shootAngle: 90),
                              new Shoot(1, 4, cooldown: 10000, fixedAngle: 110, cooldownOffset: 1400, shootAngle: 90),
                              new Shoot(1, 4, cooldown: 10000, fixedAngle: 100, cooldownOffset: 1600, shootAngle: 90),
                              new Shoot(1, 4, cooldown: 10000, fixedAngle: 90, cooldownOffset: 1800, shootAngle: 90),
                              new Shoot(1, 4, cooldown: 10000, fixedAngle: 90, cooldownOffset: 2000, shootAngle: 22.5f),
                              new TimedTransition("Wait", 2000)
                              ),
                          new State("Wait",
                              new Follow(0.7f, range: 0.5f),
                              new Flash(0xff00ff00, 0.1f, 20),
                              new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                              new TimedTransition("Bullet", 2000)
                              ),
                          new NoPlayerWithinTransition(13, "Idle"),
                          new HealthTransition(0.5f, "FlashBeforeExplode")
                          ),
                      new State("FlashBeforeExplode",
                          new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                          new Flash(0xff0000, 0.3f, 3),
                          new TimedTransition("Explode", 1000)
                          ),
                      new State("Explode",
                          new Shoot(8, 10, 36, fixedAngle: 0),
                          new Suicide()
                          )
                      ),
                  new Threshold(0.18f,
                      new ItemLoot("Potion of Defense", 0.05f)
                      )
              );
            db.Init("White Demon",
                new State(
                    "base",
                    //new DropPortalOnDeath("Abyss of Demons Portal", .3),
                    new Prioritize(
                        new StayAbove(1, 200),
                        new Follow(1, range: 7),
                        new Wander(0.4f)
                        ),
                    new Shoot(10, count: 3, shootAngle: 20, predictive: 1, cooldown: 500),
                    new Reproduce(densityMax: 3)
                    ),
                new Threshold(0.01f,
                    new ItemLoot("Potion of Vitality", 0.05f)
                    )
            );
            db.Init("Sprite God",
                new State(
                    "base",
                    new Prioritize(
                        new StayAbove(1, 200),
                        new Wander(0.4f)
                        ),
                    new Shoot(12, index: 0, count: 4, shootAngle: 10, cooldown: 1000),
                    new Shoot(10, index: 1, predictive: 1, cooldown: 5000),
                    new Reproduce(densityMax: 3),
                    new Reproduce("Sprite Child", 5, 5, coolDown: new wServer.logic.Cooldown(5000, 0))
                    ),
                //new Threshold(0.01f,
                //    LootTemplates.MountainDrop()
                //    ),
                new Threshold(0.01f,
                    new ItemLoot("Potion of Attack", 0.05f)
                )
            );
            db.Init("Sprite Child",
                new State(
                    "base",
                    new Prioritize(
                        new StayAbove(1, 200),
                        new Protect(0.4f, "Sprite God", protectionRange: 1),
                        new Wander(0.4f)
                        )
                    //new DropPortalOnDeath("Glowing Portal", .4)
                    )
            );
        }
    }
}
