using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static RotMG.Game.Logic.Loots.TierLoot;

namespace RotMG.Game.Logic.Database
{
    class EpicSpiderDen : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {
            db.Init("Crawling Depths Egg Sac",
                new State("base",
                    new PlayerWithinTransition(2.5f, "pop"),
                    new HealthTransition(0.85f, "pop")
                ),
                new State("pop",
                    new Spawn("Crawling Spider Hatchling", 12, 0.4f, 1000)
            ));
            db.Init("Crawling Spider Hatchling",
                new ConditionalEffect(Common.ConditionEffectIndex.Armored, false, 2500),
                new ConditionalEffect(Common.ConditionEffectIndex.Slowed, false, 2500),
                new State("base",
                new Wander(0.4f),
                new StayBack(0.4f, 0.5f, "Crawling Spider Hatchling"),
                    new Prioritize(
                        new Follow(0.8f, 7, 3, 5000, 2000)
                        ),
                    new Shoot(8, 2, 12, 1, predictive: 0.6f, cooldownVariance: 250, cooldown: 750),
                    new Shoot(10, predictive: 0.85f, cooldownOffset: 450, cooldownVariance: 350, cooldown: 800)
            ));
            db.Init("Crawling Green Spider",
                new Wander(0.6f),
                new State("base",
                    new Follow(1.2f, 12, 3.5f, 6000, 2500),
                    new Shoot(6, 3, 14, cooldownVariance: 500, cooldown: 1500),
                    new Shoot(12, predictive: 1, cooldownOffset: 450, cooldownVariance: 500, cooldown: 800)
            ));
            db.Init("Crawling Grey Spider",
                new Wander(0.8f),
                new State("base",
                    new Shoot(9, 1, predictive: 0.8f, cooldownOffset: 450, cooldownVariance: 350, cooldown: 1000),
                    new TimedTransition("charge", 3000)
                ),
                new State("charge",
                    new Charge(1.4f, 8, 1500),
                    new Follow(1.2f, 5, 0, 1500),
                    new Shoot(6, 6, 360 / 6, 1, cooldown: 100),
                    new TimedTransition("base", 1500)
            ));
            db.Init("Crawling Red Spotted Spider",
                new Wander(0.4f),
                new StayBack(1f, 3, "Crawling Red Spotted Spider"),
                new Follow(2, range: 6, duration: 500, cooldown: 1500),
                new State("base",
                    new Shoot(9, 2, 18, predictive: 0.5f, cooldownOffset: 450, cooldown: 700)
            ));
            db.Init("Crawling Grey Spotted Spider",
                new Wander(0.4f),
                new Follow(1.2f, range: 3, duration: 2500, cooldown: 4000),
                new State("base",
                    new Shoot(6, 3, 24, predictive: 0.3f, cooldownOffset: 450, cooldown: 800)
            ));
            db.Init("Silver Son of Arachna Giant Egg Sac",
                new TransferDamageOnDeath("Son of Arachna"),
                new TransformOnDeath("Crawling Grey Spider", 4, 4),
                new State("base",
                    new PlayerWithinTransition(6, "spawn", true)
                ),
                new State("spawn",
                    new Spawn("Crawling Grey Spider", 3, 1, 5000)
                )
            );
            db.Init("Yellow Son of Arachna Giant Egg Sac",
                new TransformOnDeath("Crawling Grey Spotted Spider", 4, 4),
                new State("base",
                    new PlayerWithinTransition(6, "spawn", true)
                ),
                new State("spawn",
                    new Spawn("Crawling Grey Spotted Spider", 3, 1, 999999)
                )
            );
            db.Init("Blue Son of Arachna Giant Egg Sac",
                new TransferDamageOnDeath("Son of Arachna"),
                new TransformOnDeath("Crawling Spider Hatchling", 8, 8),
                new State("base",
                    new PlayerWithinTransition(6, "spawn", true)
                ),
                new State("spawn",
                    new Spawn("Crawling Spider Hatchling", 8, 1, 999999)
                )
            );
            db.Init("Red Son of Arachna Giant Egg Sac",
                new TransferDamageOnDeath("Son of Arachna"),
                new TransformOnDeath("Crawling Red Spotted Spider", 4, 4),
                new State("base",
                    new PlayerWithinTransition(6, "spawn", true)
                ),
                new State("spawn",
                    new Spawn("Crawling Red Spotted Spider", 3, 1, 999999)
                )
            );
            db.Init("Son of Arachna",
                new State("sleep",
                    new ConditionalEffect(Common.ConditionEffectIndex.Armored),
                    new PlayerWithinTransition(16, "halfwoke", true)
                ),
                new State("halfwoke",
                    new SetAltTexture(0),
                    new HealthTransition(0.7f, "awake"),
                    new EntitiesNotExistsTransition(99, "awake", "Red Son of Arachna Giant Egg Sac", "Blue Son of Arachna Giant Egg Sac", "Silver Son of Arachna Giant Egg Sac"),
                    new Orbit(0.5f, 2.5f, 99, "Epic Arachna Web Spoke Anchor", 0.75f, 1),
                    new Wander(0.3f),
                    new Shoot(99, 4, 25, 8, angleOffset: 90),
                    new Shoot(99, 4, 25, 7, angleOffset: -90),
                    new TimedRandomTransition(3000, "blackS", "redS", "blueS")
                ),
                new State("blueS",
                    new SetAltTexture(3),
                    new Shoot(99, 4, 25, 7, angleOffset: 90),
                    new Shoot(99, 4, 25, 8, angleOffset: -90),
                    new Shoot(99, 2, -26, 4, cooldown: 20),
                    new Charge(1.4f, 99),
                    new TimedTransition("halfwoke", 1200),
                    new PlayerWithinTransition(2, "halfwoke")
                ),
                new State("redS",
                    new SetAltTexture(1),
                    new Orbit(2f, 7, 99, "Epic Arachna Web Spoke Anchor"),
                    new Shoot(99, 1, index: 3, fixedAngle: 0f, rotateAngle: 12f, cooldown: 350),
                    new Shoot(99, 1, index: 3, fixedAngle: 45f, rotateAngle: 12f, cooldown: 350),
                    new Shoot(99, 1, index: 3, fixedAngle: 90f, rotateAngle: 12f, cooldown: 350),
                    new Shoot(99, 1, index: 3, fixedAngle: 135f, rotateAngle: 12f, cooldown: 350),
                    new Shoot(99, 1, index: 3, fixedAngle: 180f, rotateAngle: 12f, cooldown: 350),
                    new Shoot(99, 1, index: 3, fixedAngle: 225f, rotateAngle: 12f, cooldown: 350),
                    new Shoot(99, 1, index: 3, fixedAngle: 270f, rotateAngle: 12f, cooldown: 350),
                    new Shoot(99, 1, index: 3, fixedAngle: 315f, rotateAngle: 12f, cooldown: 350),
                    new TimedTransition("halfwoke", 3500),
                    new PlayerWithinTransition(4, "halfwoke")
                ),
                new State("blackS",
                    new SetAltTexture(2),
                    new Shoot(99, 3, 14, 2, predictive: 0.6f, cooldown: 800),
                    new Shoot(99, 4, 25, 7, angleOffset: 90),
                    new Shoot(99, 4, 25, 8, angleOffset: -90),
                    new Charge(1.4f, 99),
                    new TimedTransition("halfwoke", 1200),
                    new PlayerWithinTransition(3, "halfwoke")
                ),
                new State("awake",
                    new Shoot(16, 6, 360 / 6, 6, 0f, 6f, cooldown: 250),
                    new TimedRandomTransition(3000, "chase", "wander", "stayback")
                ),
                new State("stayback",
                    new Wander(0.2f),
                    new StayBack(1.3f, 5),
                    new Shoot(16, 8, 360 / 8, 0, 0f, 30f, cooldown: 800),
                    new Shoot(16, 1, index: 1, predictive: 0.7f, cooldownVariance: 250, cooldown: 1000),
                    new TimedRandomTransition(6000, "redBack", "blueOrbit", "wander")
                ),
                new State("wander",
                    new Wander(0.7f),
                    new StayBack(0.6f, 2),
                    new Shoot(16, 6, 360 / 6, 6, 0f, 6f, cooldownOffset: 600, cooldown: 150),
                    new Shoot(16, 1, index: 1, predictive: 1, cooldownOffset: 600, cooldownVariance: 350, cooldown: 1200),
                    new TimedRandomTransition(4000, "blackChase", "roto", "stayback")
                ),
                new State("chase",
                    new Wander(0.3f),
                    new Follow(0.8f, 16, 4, 2000, 700),
                    new Charge(1.2f, 16, 4000),
                    new Shoot(16, 1, index: 1, predictive: 0.5f, cooldownVariance: 350, cooldown: 1200),
                    new Shoot(99, 4, 25, 7, angleOffset: 90),
                    new Shoot(99, 4, 25, 8, angleOffset: -90),
                    new TimedRandomTransition(8000, "blackChase", "roto", "stayback")
                ),
                new State("roto",
                    new Orbit(1, 5, 99, "Epic Arachna Web Spoke Anchor"),
                    new StayBack(0.7f, 1.75f),
                    new Shoot(99, 4, 25, 7, angleOffset: 90),
                    new Shoot(99, 4, 25, 8, angleOffset: -90),
                    new Shoot(16, 1, index: 1, predictive: 0.5f, cooldownVariance: 350, cooldown: 1200),
                    new TimedRandomTransition(6000, "redBack", "blueOrbit", "wander")
                ),
                new State("blackChase",
                    new SetAltTexture(2),
                    new StayBack(0.7f, 1.75f),
                    new Follow(0.7f, 14, 3.5f, 1400, 600),
                    new Charge(1.3f, 10, 2400),
                    new Shoot(99, 4, 25, 7, angleOffset: 90),
                    new Shoot(99, 4, 25, 8, angleOffset: -90),
                    new Shoot(9, 5, 26, 2, cooldown: 1400, cooldownVariance: 800),
                    new TimedRandomTransition(2500, "stayback", "wander")
                ),
                new State("redBack",
                    new SetAltTexture(1),
                    new StayBack(1.6f, 4.5f),
                    new Wander(0.3f),
                    new Shoot(99, 6, 360 / 8, 3, fixedAngle: 0f, rotateAngle: 23f, cooldownVariance: 400, cooldown: 1600),
                    new Shoot(99, 4, 25, 8, angleOffset: 90),
                    new Shoot(99, 4, 25, 7, angleOffset: -90),
                    new TimedRandomTransition(5000, "chase", "stayback", "wander")
                ),
                new State("blueOrbit",
                    new SetAltTexture(3),
                    new StayBack(0.7f, 1.75f),
                    new Orbit(0.8f, 5, 16, speedVariance: 0.2f, radiusVariance: 1.5f, targetPlayers: true),
                    new Shoot(99, 2, -26, 4, cooldown: 20),
                    new Shoot(99, 4, 25, 8, angleOffset: 90),
                    new Shoot(99, 4, 25, 7, angleOffset: -90),
                    new TimedRandomTransition(3500, "roto", "stayback", "wander")
                ),
                new Threshold(0.01f,
                    new ItemLoot("Sunsteel Katana", 0.02f, r: new LootDef.RarityModifiedData(1.2f, 1, true)),
                    new TierLoot(10, LootType.Weapon, 0.55f),
                    new TierLoot(10, LootType.Armor, 0.55f),
                    new TierLoot(5, LootType.Ability, 0.55f),
                    new TierLoot(5, LootType.Ring, 0.55f),
                    new ItemLoot("Potion of Dexterity", 1.0f),
                    new ItemLoot("Potion of Dexterity", 1.0f),
                    new ItemLoot("Will of the Den", 0.0125f, r: new LootDef.RarityModifiedData(1.2f, 1, true)),
                    new ItemLoot("Arachnid Garment", 0.0125f, r: new LootDef.RarityModifiedData(1.2f, 1, true)),
                    new ItemLoot("Viral Egg", 0.0125f, r: new LootDef.RarityModifiedData(1.2f, 1, true)),
                    new ItemLoot("Spider Generator", 0.005f)
                    //new ItemLoot("Arachnid Miasma", 0.0125f, r: new LootDef.RarityModifiedData(1.2f, 1, true))
                )
            );
            db.Init("Epic Arachna Web Spoke 1",
                new ConditionalEffect(Common.ConditionEffectIndex.Invincible),
                new State("base",
                    new ShootAt("Epic Arachna Web Spoke 2", 8, 1),
                    new ShootAt("Epic Arachna Web Spoke 8", 8, 1)
                )
            );
            db.Init("Epic Arachna Web Spoke 3",
                new ConditionalEffect(Common.ConditionEffectIndex.Invincible),
                new State("base",
                    new ShootAt("Epic Arachna Web Spoke 2", 8, 1),
                    new ShootAt("Epic Arachna Web Spoke 4", 8, 1)
                )
            );
            db.Init("Epic Arachna Web Spoke 5",
                new ConditionalEffect(Common.ConditionEffectIndex.Invincible),
                new State("base",
                    new ShootAt("Epic Arachna Web Spoke 4", 8, 1),
                    new ShootAt("Epic Arachna Web Spoke 6", 8, 1)
                )
            );
            db.Init("Epic Arachna Web Spoke 7",
                new ConditionalEffect(Common.ConditionEffectIndex.Invincible),
                new State("base",
                    new ShootAt("Epic Arachna Web Spoke 8", 8, 1),
                    new ShootAt("Epic Arachna Web Spoke 6", 8, 1)
                )
            ); 
            db.Init("Epic Arachna Web Spoke 2",
                new ConditionalEffect(Common.ConditionEffectIndex.Invincible),
                 new State("base",
                     new ShootAt("Epic Arachna Web Spoke Anchor", 12, 1)
                 )
             );
            db.Init("Epic Arachna Web Spoke 4",
                new ConditionalEffect(Common.ConditionEffectIndex.Invincible),
                 new State("base",
                     new ShootAt("Epic Arachna Web Spoke Anchor", 12, 1)
                 )
             );
            db.Init("Epic Arachna Web Spoke 6",
                new ConditionalEffect(Common.ConditionEffectIndex.Invincible),
                 new State("base",
                     new ShootAt("Epic Arachna Web Spoke Anchor", 12, 1)
                 )
             );
            db.Init("Epic Arachna Web Spoke 8",
                new ConditionalEffect(Common.ConditionEffectIndex.Invincible),
                 new State("base",
                     new ShootAt("Epic Arachna Web Spoke Anchor", 12, 1)
                 )
             );
            db.Init("Epic Arachna Web Spoke Anchor",
                new ConditionalEffect(Common.ConditionEffectIndex.Invincible),
                new MoveLine(1f, 90, 1),
                new State("base",
                    new TimedTransition("nothing")
                    )
            );
        }
    }
}
