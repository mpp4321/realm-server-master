
//by GhostMaree
using RotMG.Common;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;
using static RotMG.Game.Logic.Loots.TierLoot;

namespace RotMG.Game.Logic.Database
{
    class SpriteWorld : IBehaviorDatabase
    {

        public void Init(BehaviorDb db)
        {
            db.Init("Native Fire Sprite",
                new State("base",
                    new Prioritize(
                        new StayAbove(speed: 0.4f, altitude: 95),
                        new Shoot(10, count: 2, shootAngle: 7, index: 0, cooldown: 300)
                    ),
                    new Wander(speed: 1.4f)
                ),
                new Threshold(0.01f,
                    new TierLoot(tier: 5, type: LootType.Weapon, chance: 0.4f),
                    new ItemLoot(item: "Magic Potion", chance: 0.05f)
                )
            );
            db.Init("Native Ice Sprite",
                new State("base",
                    new Prioritize(
                        new StayAbove(speed: 0.4f, altitude: 105),
                        new Wander(speed: 1.4f)
                    ),
                    new Shoot(10, count: 3, shootAngle: 7, index: 0, cooldown: 1000)
                ),
                new Threshold(0.01f,
                    new TierLoot(tier: 2, type: LootType.Ability, chance: 0.4f),
                    new ItemLoot(item: "Magic Potion", chance: 0.05f)
                )
            );
            db.Init("Native Magic Sprite",
                new State("base",
                    new Prioritize(
                        new StayAbove(speed: 0.4f, altitude: 115),
                        new Wander(speed: 1.4f)
                    ),
                    new Shoot(10, count: 4, shootAngle: 7, index: 0, cooldown: 1000)
                ),
                new Threshold(0.01f,
                    new TierLoot(tier: 6, type: LootType.Armor, chance: 0.4f),
                    new ItemLoot(item: "Magic Potion", chance: 0.05f)
                )
            );
            db.Init("Native Nature Sprite",
                new State("base",
                    new Shoot(10, count: 5, shootAngle: 7, index: 0, cooldown: 1000),
                    new Wander(speed: 1.6f)
                ),
                new Threshold(0.01f,
                    new ItemLoot(item: "Magic Potion", chance: 0.015f),
                    new ItemLoot(item: "Sprite Wand", chance: 0.015f),
                    new ItemLoot(item: "Ring of Greater Magic", chance: 0.4f)
                )
            );
            db.Init("Native Darkness Sprite",
                new State("base",
                    new Shoot(10, count: 5, shootAngle: 7, index: 0, cooldown: 1000),
                    new Wander(speed: 1.6f)
                ),
                new Threshold(0.01f,
                    new ItemLoot(item: "Health Potion", chance: 0.015f),
                    new ItemLoot(item: "Ring of Dexterity", chance: 0.4f)
                )
            );
            db.Init("Native Sprite God",
                new State("base",
                    new Prioritize(
                        new StayAbove(speed: 0.4f, altitude: 200),
                        new Wander(speed: 0.4f)
                    ),
                    new Shoot(12, count: 4, shootAngle: 10, index: 0, cooldown: 1000),
                    new Shoot(12, index: 1, predictive: 1, cooldown: 1000)
                ),
                new Threshold(0.01f,
                    new TierLoot(tier: 6, type: LootType.Weapon, chance: 0.4f),
                    new TierLoot(tier: 7, type: LootType.Weapon, chance: 0.25f),
                    new TierLoot(tier: 8, type: LootType.Weapon, chance: 0.25f),
                    new TierLoot(tier: 7, type: LootType.Armor, chance: 0.4f),
                    new TierLoot(tier: 8, type: LootType.Armor, chance: 0.25f),
                    new TierLoot(tier: 9, type: LootType.Armor, chance: 0.25f),
                    new TierLoot(tier: 4, type: LootType.Ring, chance: 0.125f),
                    new TierLoot(tier: 4, type: LootType.Ability, chance: 0.125f),
                    new ItemLoot(item: "Potion of Attack", chance: 0.1f)
                )
            );
            db.Init("Limon the Sprite God",
                    //new ChangeMusic("https://github.com/GhostRealm/GhostRealm.github.io/raw/master/music/Sprite.mp3"),
                    new State("start_the_fun",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new PlayerWithinTransition(dist: 11, targetState: "begin_teleport1", seeInvis: true)
                    ),
                    new State("begin_teleport1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Prioritize(
                            new StayCloseToSpawn(speed: 0.5f, range: 7),
                            new Wander(speed: 0.5f)
                        ),
                        new Flash(color: 0x00FF00, flashPeriod: 0.25f, flashRepeats: 8),
                        new TimedTransition(time: 2000, targetState: "teleport1")
                    ),
                    new State("teleport1",
                        new Prioritize(
                            new StayCloseToSpawn(speed: 1.6f, range: 7),
                            new Follow(speed: 6, acquireRange: 10, range: 2),
                            new Follow(speed: 0.3f, acquireRange: 10, range: 0.2f)
                        ),
                        new TimedTransition(time: 300, targetState: "circle_player")
                    ),
                    new State("circle_player",
                        new Shoot(8, count: 2, shootAngle: 10, index: 0, angleOffset: 0.7f, predictive: 0.4f, cooldown: 400),
                        new Shoot(8, count: 2, shootAngle: 180, index: 0, angleOffset: 0.7f, predictive: 0.4f, cooldown: 400),
                        new NotMovingTransition("boom") { SubIndex = 0 },
                        new Prioritize(
                            new StayCloseToSpawn(speed: 1.3f, range: 7),
                            new Orbit(speed: 1.8f, 4, acquireRange: 5, target: null),
                            new Follow(speed: 6, acquireRange: 10, range: 2),
                            new Follow(speed: 0.3f, acquireRange: 10, range: 0.2f)
                        ),
                        new State("boom",
                            new Shoot(8, count: 18, shootAngle: 20, index: 0, angleOffset: 0.4f, predictive: 0.4f, cooldown: 1500),
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new TimedTransition(time: 1000, targetState: "check_if_not_moving")
                        ),
                        new TimedTransition(time: 10000, targetState: "set_up_the_box")
                    ),
                    new State("set_up_the_box",
                        new TossObject(child: "Limon Element 1", range: 9.5f, angle: 315, cooldown: 1000000),
                        new TossObject(child: "Limon Element 2", range: 9.5f, angle: 225, cooldown: 1000000),
                        new TossObject(child: "Limon Element 3", range: 9.5f, angle: 135, cooldown: 1000000),
                        new TossObject(child: "Limon Element 4", range: 9.5f, angle: 45, cooldown: 1000000),
                        new TossObject(child: "Limon Element 1", range: 14, angle: 315, cooldown: 1000000),
                        new TossObject(child: "Limon Element 2", range: 14, angle: 225, cooldown: 1000000),
                        new TossObject(child: "Limon Element 3", range: 14, angle: 135, cooldown: 1000000),
                        new TossObject(child: "Limon Element 4", range: 14, angle: 45, cooldown: 1000000),
                        new TransitionFrom("set_up_the_box", "shielded1"),

                        new Grenade(2f, 100, 3, 0, 2000, 0, 0),
                        new Grenade(2f, 100, 3, 90, 2000, 0, 500),
                        new Grenade(2f, 100, 3, 180, 2000, 0, 1000),
                        new Grenade(2f, 100, 3, 270, 2000, 0, 1500),

                        new State("shielded1",
                            new Shoot(8, count: 1, predictive: 0.1f, cooldown: 1000),
                            new Shoot(8, count: 3, shootAngle: 120, angleOffset: 0.3f, predictive: 0.1f, cooldown: 500),
                            new TimedTransition(targetState: "shielded2", 1500)
                        ),
                        new State("shielded2",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Shoot(8, count: 3, shootAngle: 120, angleOffset: 0.3f, predictive: 0.2f, cooldown: 800),
                            new TimedTransition(targetState: "shielded1", 3500)
                        ),
                        new TimedTransition(time: 20000, targetState: "Summon_the_sprites")
                    ),
                    new State("Summon_the_sprites",
                        new StayCloseToSpawn(speed: 0.5f, range: 8),
                        new Wander(speed: 0.5f),
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Shoot(8, count: 3, shootAngle: 15, cooldown: 1300),
                        new Spawn(children: "Magic Sprite", maxChildren: 2, initialSpawn: 0, cooldown: 500),
                        new Spawn(children: "Ice Sprite", maxChildren: 1, initialSpawn: 0, cooldown: 500),
                        new TimedTransition(time: 11000, targetState: "begin_teleport1"),
                        new HealthTransition(threshold: 0.2f, targetState: "begin_teleport1")
                    ),
                    new DropPortalOnDeath(target: "Epic Glowing Portal", probability: 1),
                    new Threshold(0.0001f,
                    new TierLoot(tier: 6, type: LootType.Armor, chance: 0.4f),
                    new TierLoot(tier: 7, type: LootType.Armor, chance: 0.4f),
                    new TierLoot(tier: 3, type: LootType.Ability, chance: 0.25f),
                    new TierLoot(tier: 4, type: LootType.Ability, chance: 0.125f),
                    new TierLoot(tier: 5, type: LootType.Ability, chance: 0.0625f),
                    new TierLoot(tier: 3, type: LootType.Ring, chance: 0.25f),
                    new ItemLoot(item: "Potion of Dexterity", chance: 1.0f),
                    new ItemLoot(item: "Potion of Defense", chance: 0.3f),
                    new ItemLoot(item: "Sprite Wand", chance: 0.004f),
                    new ItemLoot(item: "Wine Cellar Incantation", chance: 0.05f),
                    new ItemLoot(item: "Staff of Extreme Prejudice", chance: 0.006f, threshold: 0.01f),
                    new ItemLoot(item: "Cloak of the Planewalker", chance: 0.006f, threshold: 0.01f),
                    new ItemLoot("Corporeal Shield", 0.02f),
                    new ItemLoot("Bracelet of the Sprites", 0.005f, r: new LootDef.RarityModifiedData(1.0f, 2, true))
                )
            );
            db.Init("Limon Element 1",
                new State("base",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new EntitiesNotExistsTransition(dist: 999, targetState: "Suicide", "Limon the Sprite God"),
                    new TransitionFrom("base", "Setup"),
                    new State("Setup",
                        new TimedTransition(time: 2000, targetState: "Attacking1")
                    ),
                    new State("Attacking1",
                        new Shoot(999, fixedAngle: 180, defaultAngle: 180, cooldown: 300),
                        new Shoot(999, fixedAngle: 90, defaultAngle: 90, cooldown: 300),
                        new TimedTransition(time: 6000, targetState: "Attacking2")
                    ),
                    new State("Attacking2",
                        new Shoot(999, fixedAngle: 180, defaultAngle: 180, cooldown: 300),
                        new Shoot(999, fixedAngle: 90, defaultAngle: 90, cooldown: 300),
                        new Shoot(999, fixedAngle: 135, defaultAngle: 135, cooldown: 300),
                        new TimedTransition(time: 6000, targetState: "Attacking3")
                    ),
                    new State("Attacking3",
                        new Shoot(999, fixedAngle: 180, defaultAngle: 180, cooldown: 300),
                        new Shoot(999, fixedAngle: 90, defaultAngle: 90, cooldown: 300),
                        new TimedTransition(time: 6000, targetState: "Setup")
                    ),
                    new State("Suicide",
                        new Suicide()
                    ),
                    new Decay(time: 20000)
                )
            );
            db.Init("Limon Element 2",
                new State("base",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new EntitiesNotExistsTransition(dist: 999, targetState: "Suicide", "Limon the Sprite God"),
                    new TransitionFrom("base", "Setup"),
                    new State("Setup",
                        new TimedTransition(time: 2000, targetState: "Attacking1")
                    ),
                    new State("Attacking1",
                        new Shoot(999, fixedAngle: 90, defaultAngle: 90, cooldown: 300),
                        new Shoot(999, fixedAngle: 0, defaultAngle: 0, cooldown: 300),
                        new TimedTransition(time: 6000, targetState: "Attacking2")
                    ),
                    new State("Attacking2",
                        new Shoot(999, fixedAngle: 90, defaultAngle: 90, cooldown: 300),
                        new Shoot(999, fixedAngle: 0, defaultAngle: 0, cooldown: 300),
                        new Shoot(999, fixedAngle: 45, defaultAngle: 45, cooldown: 300),
                        new TimedTransition(time: 6000, targetState: "Attacking3")
                    ),
                    new State("Attacking3",
                        new Shoot(999, fixedAngle: 90, defaultAngle: 90, cooldown: 300),
                        new Shoot(999, fixedAngle: 0, defaultAngle: 0, cooldown: 300),
                        new TimedTransition(time: 6000, targetState: "Setup")
                    ),
                    new State("Suicide",
                        new Suicide()
                    ),
                    new Decay(time: 20000)
                )
            );
            db.Init("Limon Element 3",
                new State("base",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new EntitiesNotExistsTransition(dist: 999, targetState: "Suicide", "Limon the Sprite God"),
                    new TransitionFrom("base", "Setup"),
                    new State("Setup",
                        new TimedTransition(time: 2000, targetState: "Attacking1")
                    ),
                    new State("Attacking1",
                        new Shoot(999, fixedAngle: 0, defaultAngle: 0, cooldown: 300),
                        new Shoot(999, fixedAngle: 270, defaultAngle: 270, cooldown: 300),
                        new TimedTransition(time: 6000, targetState: "Attacking2")
                    ),
                    new State("Attacking2",
                        new Shoot(999, fixedAngle: 0, defaultAngle: 0, cooldown: 300),
                        new Shoot(999, fixedAngle: 270, defaultAngle: 270, cooldown: 300),
                        new Shoot(999, fixedAngle: 315, defaultAngle: 315, cooldown: 300),
                        new TimedTransition(time: 6000, targetState: "Attacking3")
                    ),
                    new State("Attacking3",
                        new Shoot(999, fixedAngle: 0, defaultAngle: 0, cooldown: 300),
                        new Shoot(999, fixedAngle: 270, defaultAngle: 270, cooldown: 300),
                        new TimedTransition(time: 6000, targetState: "Setup")
                    ),
                    new State("Suicide",
                        new Suicide()
                    ),
                    new Decay(time: 20000)
                )
            );
            db.Init("Limon Element 4",
                new State("base",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new EntitiesNotExistsTransition(dist: 999, targetState: "Suicide", "Limon the Sprite God"),
                    new TransitionFrom("base", "Setup"),
                    new State("Setup",
                        new TimedTransition(time: 2000, targetState: "Attacking1")
                    ),
                    new State("Attacking1",
                        new Shoot(999, fixedAngle: 270, defaultAngle: 270, cooldown: 300),
                        new Shoot(999, fixedAngle: 180, defaultAngle: 180, cooldown: 300),
                        new TimedTransition(time: 6000, targetState: "Attacking2")
                    ),
                    new State("Attacking2",
                        new Shoot(999, fixedAngle: 270, defaultAngle: 270, cooldown: 300),
                        new Shoot(999, fixedAngle: 180, defaultAngle: 180, cooldown: 300),
                        new Shoot(999, fixedAngle: 225, defaultAngle: 225, cooldown: 300),
                        new TimedTransition(time: 6000, targetState: "Attacking3")
                    ),
                    new State("Attacking3",
                        new Shoot(999, fixedAngle: 270, defaultAngle: 270, cooldown: 300),
                        new Shoot(999, fixedAngle: 180, defaultAngle: 180, cooldown: 300),
                        new TimedTransition(time: 6000, targetState: "Setup")
                    ),
                    new State("Suicide",
                        new Suicide()
                    ),
                    new Decay(time: 20000)
                )
            );

            db.Init("Black Sprite Tree",
                    new State("idle", new ConditionalEffect(ConditionEffectIndex.Invincible)),
                    new State("shoot",
                        new ShootAt("Epic Limon the Sprite God", 99, 1, 0)
                    )
                );

            db.Init("Epic Limon the Sprite God",
                      new DropPortalOnDeath("Realm Portal"),
                      new Shoot(50f, 1, 0f, 1, cooldown: 3000, cooldownVariance: 1000),
                      new State("shootLikeDumb", 
                          new HealthTransition(0.5f, "black towers shoot"),
                          new State("1",
                              new Wander(1),
                              new Shoot(50, count: 8, shootAngle: 60, index: 0, predictive: 1, cooldown: 600),
                              new TimedTransition("2", 11750)
                              ),
                          new State("2",
                              new ConditionalEffect(ConditionEffectIndex.Invincible),
                              new Shoot(50, count: 8, shootAngle: 90, index: 0, predictive: 1, cooldown: 600),
                              new Shoot(80, count: 4, shootAngle: 90, index: 0, predictive: 4, cooldown: 600),
                              new TimedTransition("3", 3750)
                              ),
                          new State("3",
                              new Shoot(80, count: 4, shootAngle: 90, index: 0, predictive: 4, cooldown: 600),
                              new Shoot(85, count: 4, shootAngle: 90, index: 0, predictive: 2, cooldown: 600),
                              new Shoot(90, count: 4, shootAngle: 90, index: 0, predictive: 1, cooldown: 600),
                              new TossObject("Native Sprite God", 10, cooldown: 11000),
                              new TimedTransition("1", 10750)
                              )
                      ),
                      new State("black towers shoot",
                          new OrderOnEntry(99f, "Black Sprite Tree", "shoot"),
                          new MoveTo(1f, 37, 38),
                          //Will auto make it 360f / 8
                          new Shoot(99f, count: 8, shootAngle: null, index: 0, defaultAngle: 0f, rotateAngle: 15f, cooldown: 600, cooldownVariance: 250, cooldownOffset: 2000),
                          new Shoot(50f, 2, shootAngle: 30f, 0, 1, cooldown: 1500, cooldownVariance: 500, predictive: 1),
                          new TransitionFrom("black towers shoot", "defendmode"),
                          new State("defendmode",
                            new ConditionalEffect(ConditionEffectIndex.Armored),
                            new EntitiesNotExistsTransition(99, "weakened", "Black Sprite Tree")
                          ),
                          new State("weakened")
                      ),
                      new Threshold(0.01f,
                      new TierLoot(tier: 9, type: LootType.Armor, chance: 0.4f),
                      new TierLoot(tier: 10, type: LootType.Armor, chance: 0.4f),
                      new TierLoot(tier: 3, type: LootType.Ability, chance: 0.25f),
                      new TierLoot(tier: 4, type: LootType.Ability, chance: 0.125f),
                      new TierLoot(tier: 5, type: LootType.Ability, chance: 0.0625f),
                      new TierLoot(tier: 5, type: LootType.Ring, chance: 0.25f),
                      new ItemLoot(item: "Potion of Dexterity", chance: 0.3f),
                      new ItemLoot(item: "Potion of Dexterity", chance: 1f),
                      new ItemLoot(item: "Potion of Dexterity", chance: 1f),
                      new ItemLoot(item: "Potion of Dexterity", chance: 0.3f),
                      new ItemLoot(item: "Potion of Defense", chance: 0.1f),
                      new ItemLoot(item: "Sprite Wand", chance: 0.004f, r: new LootDef.RarityModifiedData(1.0f, 8, true)),
                      new ItemLoot(item: "Sprite World Key", 0.02f),
                      new ItemLoot(item: "Staff of Extreme Prejudice", chance: 0.006f, threshold: 0.01f, r: new LootDef.RarityModifiedData(1.2f, 2, true)),
                      new ItemLoot(item: "Cloak of the Planewalker", chance: 0.006f, threshold: 0.01f, r: new LootDef.RarityModifiedData(1.2f, 2, true)),
                      new ItemLoot("Corporeal Shield", 0.02f, r: new LootDef.RarityModifiedData(1.2f, 2, true)),
                      new ItemLoot("Bracelet of the Sprites", 0.005f, r: new LootDef.RarityModifiedData(1.0f, 4, true)),
                      new ItemLoot("Piece of Havoc", 0.003f)
                  )
              );


            ;
        }
    }
}
