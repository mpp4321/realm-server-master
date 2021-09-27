using RotMG.Common;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Conditionals;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;
using static RotMG.Game.Logic.LootDef;
using ItemType = RotMG.Game.Logic.Loots.TierLoot.LootType;

namespace RotMG.Game.Logic.Database
{
    public class PirateCave : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {

            db.EveryInit = new IBehavior[]
            {
                new TierLoot(1, ItemType.Weapon, 0.2f),
                new TierLoot(2, ItemType.Weapon, 0.1f),
                new TierLoot(3, ItemType.Weapon, 0.05f),
                new TierLoot(1, ItemType.Armor, 0.2f),
                new TierLoot(2, ItemType.Armor, 0.1f),
                new TierLoot(3, ItemType.Armor, 0.05f),
                new TierLoot(1, ItemType.Ring, 0.2f),
                new TierLoot(2, ItemType.Ring, 0.1f),
                new TierLoot(3, ItemType.Ring, 0.05f),
                new TierLoot(1, ItemType.Ability, 0.1f),
                new TierLoot(2, ItemType.Ability, 0.05f),
            };

            db.Init("Dreadstump the Pirate King",
                new State("base",
                    new State("Attack",
                    new Shoot(index: 1, predictive: 0.3f, cooldown: 750),
                    new Shoot(count: 4, shootAngle: 45, cooldown: 500),
                    new Shoot(index: 2, predictive: 0.6f, cooldown: 1500, cooldownVariance: 500, effect: new ConditionEffectIndex[] { ConditionEffectIndex.Dazed }, effect_duration: 1200),
                        new Prioritize(
                            new StayBack(0.5f, 6)
                        ),
                        new PlayerWithinTransition(2f, "Charge")
                    ),
                new State("Charge",
                    new Follow(1f, acquireRange: 10, range: 0),
                    new Shoot(count: 6, shootAngle: 13, predictive: 1, effect: new ConditionEffectIndex[] { ConditionEffectIndex.Stunned }, effect_duration: 2500),
                    new Shoot(count: 4, shootAngle: 65, cooldown: 350),
                    new TimedTransition("Attack", 5000)
                    )
                ),
                new DropPortalOnDeath("Realm Portal", 1f),
                new Threshold(0.01f, new ItemLoot("Sturdy Pegleg", 0.1f), new TierLoot(4, ItemType.Ring, 1.0f, r: new RarityModifiedData(1f, 2)),
                
                    new TierLoot(4, ItemType.Armor, 0.3f, r: new RarityModifiedData(1f, 1)),
                    new TierLoot(5, ItemType.Armor, 0.4f, r: new RarityModifiedData(1f, 1))
                
                )
            );
            db.Init("Pirate Lieutenant",
                new State("wander",
                    new Wander(0.3f),
                    new PlayerWithinTransition(8, "Attack")
                    ),
                new State("Attack",
                    new Prioritize(
                            new StayBack(1, 3),
                            new Wander(0.2f)
                        ),
                    new Shoot(cooldown: 750, angleOffset: 6, cooldownVariance: 100)
                    )
            );
            db.Init("Pirate Commander",
                new State("wander",
                    new Wander(0.3f),
                    new PlayerWithinTransition(8, "Attack")
                    ),
                new State("Attack",
                    new Follow(0.4f, acquireRange: 8, range: 6),
                    new Shoot(count: 3, cooldown: 1500, shootAngle: 30, angleOffset: 35, cooldownVariance: 300, effect: new ConditionEffectIndex[] { ConditionEffectIndex.Dazed }, effect_duration: 650)
                )
            );
            db.Init("Pirate Captain",
                new State("wander",
                    new Wander(0.3f),
                    new PlayerWithinTransition(8, "Attack")
                    ),
                new State("Attack",
                    new Follow(acquireRange: 8, range: 2, speed: 1.2f),
                    new Shoot(cooldown: 500, cooldownVariance: 200)
                )
            );
            db.Init("Pirate Admiral",
                new State("wander",
                    new Wander(0.3f),
                    new PlayerWithinTransition(10f, "Attack")
                    ),
                new State("Attack",
                    new Follow(speed: 0.7f, duration: 3000, cooldown: 700),
                    new Shoot(cooldown: 500, cooldownVariance: 300)
                )
            );
            db.Init("Cave Pirate Brawler",
                new State("wander",
                    new Wander(0.5f),
                    new PlayerWithinTransition(10f, "Attack")
                    ),
                new State("Attack",
                    new Follow(0.4f, duration: 2500, cooldown: 1500),
                    new Shoot(cooldown: 300, cooldownVariance: 150)
                )
            );
            db.Init("Cave Pirate Sailor",
                new State("wander",
                    new Wander(0.7f),
                    new PlayerWithinTransition(10.0f, "Attack")
                    ),
                new State("Attack",
                new Shoot(cooldown: 800, cooldownVariance: 300, angleOffset: 12, predictive: 0.3f),
                new Prioritize(
                    new Follow(speed: 0.4f, duration: 2750, cooldown: 1200)
                    )
                )
            );
            db.Init("Cave Pirate Macaw",
                    new State("wander",
                        new Wander(1.2f),
                        new PlayerWithinTransition(10.0f, "Attack")
                    ),
                    new State("Attack",
                    new Shoot(cooldown: 750, angleOffset: 5, predictive: 0.3f),
                    new Prioritize(
                        new Orbit(0.4f, radius: 1.5f)
                        )
                    )
            );
            db.Init("Cave Pirate Parrot",
                new State("wander",
                        new Wander(1.2f),
                        new PlayerWithinTransition(10.0f, "Attack")
                    ),
                new State("Attack",
                    new Shoot(cooldown: 750, angleOffset: 5, predictive: 0.3f),
                    new Prioritize(
                        new Orbit(0.4f, radius: 1.5f)
                        )
                    )
            );
            db.Init("Cave Pirate Monkey",
                new State("everything",
                    new Wander(1.2f),
                    new Shoot(cooldown: 1000, cooldownVariance: 500, angleOffset: 45, predictive: 1, effect: new ConditionEffectIndex[] { ConditionEffectIndex.Confused }, effect_duration: 1500)
                )
            );
        }
    }
}
