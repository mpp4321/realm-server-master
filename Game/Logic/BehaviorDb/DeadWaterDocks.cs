using RotMG.Common;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;
using static RotMG.Game.Logic.Loots.TierLoot;

namespace RotMG.Game.Logic.Database
{
    class DeadWaterDocks : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {
			db.Init("Deadwater Docks Parrot",
				new State("base",
					new EntitiesNotExistsTransition(90000, "rip", "Jon Bilgewater the Pirate King"),
					new State("CircleOrWander",
						new Prioritize(
							new Orbit(0.55f, 2, 5, "Parrot Cage"),
							new Wander(0.12f)
							)
						),
					new State("Orbit&HealJon",
						new Orbit(0.55f, 2, 20, "Jon Bilgewater the Pirate King"),
						new HealSelf(cooldown: 2000, amount: 100)

						),
					new State("rip",
						new Suicide()
						)
					)
				);
			db.Init("Parrot Cage",
				new State("base",
					new ConditionalEffect(ConditionEffectIndex.Invincible),
					new State("NoSpawn"
						),
					new State("SpawnParrots",
						new Reproduce("Deadwater Docks Parrot", densityRadius: 5, densityMax: 5, cooldown: 2500)
						)
					)
				);
			db.Init("Bottled Evil Water",
				new State("base",
					new State("water",
						new TimedTransition("drop", 2000)
                    ),
					new State("drop",
                        new GroundTransform("Evil Water"),
                    new Decay(100)
            )));
			db.Init("Deadwater Docks Lieutenant",
				new State("base",
					new Follow(1, 8, 1),
					new Shoot(8, 1, 10, cooldown: 1000),
					new TossObject("Bottled Evil Water", angle: null, cooldown: 6750)
					),
				new ItemLoot("Magic Potion", 0.1f),
				new ItemLoot("Health Potion", 0.1f)
				);
			db.Init("Deadwater Docks Veteran",
				new State("base",
					new Follow(0.8f, 8, 1),
					new Shoot(8, 1, 10, cooldown: 500)
					),
				new TierLoot(10, LootType.Weapon, 0.05f),
				new ItemLoot("Magic Potion", 0.1f),
				new ItemLoot("Health Potion", 0.1f)
				);
			db.Init("Deadwater Docks Admiral",
				new State("base",
					new Follow(0.6f, 8, 1),
					new Shoot(8, 3, 10, cooldown: 1325)
					),
				new ItemLoot("Magic Potion", 0.1f),
				new ItemLoot("Health Potion", 0.1f)
				);
			db.Init("Deadwater Docks Brawler",
				new State("base",
					new Follow(1.12f, 8, 1),
					new Shoot(8, 1, 10, cooldown: 350)
					),
				new ItemLoot("Magic Potion", 0.1f),
				new ItemLoot("Health Potion", 0.1f)
				);
			db.Init("Deadwater Docks Sailor",
				new State("base",
					new Follow(0.9f, 8, 1),
					new Shoot(8, 1, 10, cooldown: 525)
					),
				new ItemLoot("Magic Potion", 0.1f),
				new ItemLoot("Health Potion", 0.1f)
				);
			db.Init("Deadwater Docks Commander",
				new State("base",
					new Follow(0.90f, 8, 1),
					new Shoot(8, 1, 10, cooldown: 900),
					new TossObject("Bottled Evil Water", angle: null, cooldown: 8750)
					),
				new ItemLoot("Magic Potion", 0.1f),
				new ItemLoot("Health Potion", 0.1f)
				);
			db.Init("Deadwater Docks Captain",
				new State("base",
					new Follow(0.47f, 8, 1),
					new Shoot(8, 1, 10, cooldown: 3500)
					),
				new ItemLoot("Magic Potion", 0.1f),
				new ItemLoot("Health Potion", 0.1f)
				);

			db.Init("Jon Bilgewater the Pirate King",
                HPScale.BOSS_HP_SCALE_DEFAULT(),
                new State("default",
                    new PlayerWithinTransition(8, "base")
                ),
                new State("base",
                    new Order(90, "Parrot Cage", "SpawnParrots"),
                    new HealthTransition(0.75f, "gotoSpawn"),
					new TransitionFrom("base", "coinphase"),
                    new State("coinphase",
                        new Wander(0.11f),
                        new Shoot(10, count: 1, index: 0, cooldown: 200),
                        new TimedTransition("cannonballs", 4500)
                    ),
                    new State("cannonballs",
                        new Follow(0.32f, 8, cooldown: 1000),
                        new Shoot(10, count: 7, shootAngle: 30, index: 1, cooldown: 2100),
                        new Shoot(10, count: 7, shootAngle: 30, angleOffset: 180f, index: 1, cooldown: 2100, cooldownOffset: 2100),
                        new TimedTransition("coinphase", 5000)
                    )
                ),
                new State("gotoSpawn",
                    new ReturnToSpawn(speed: 0.52f),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new ConditionalEffect(ConditionEffectIndex.StunImmune),
                    new TimedTransition("blastcannonballs", 3500)
                ),
                new State("blastcannonballs",
                    new ConditionalEffect(ConditionEffectIndex.StunImmune),
                    new Order(90, "Deadwater Docks Parrot", "CircleOrWander"),
                    new Shoot(10, count: 7, shootAngle: 30, index: 1, cooldown: 1700),
                    new Shoot(10, count: 7, shootAngle: 30, angleOffset: 180f, index: 1, cooldown: 1700, cooldownOffset: 1700),
                    new TimedTransition("parrotcircle", 6000)
                    ),
                new State("parrotcircle",
                    new ConditionalEffect(ConditionEffectIndex.StunImmune),
                    new Order(90, "Deadwater Docks Parrot", "Orbit&HealJon"),
                    new TimedTransition("blastcannonballs", 6000)
                ),
				new Threshold(0.01f,
					LootTemplates.CrystalsHardRegular()
                ),
				new Threshold(0.01f,
					LootTemplates.Eggs(0.25f)
                ),
				new Threshold(0.01f,
					new ItemLoot("Potion of Speed", 1.0f),
					new ItemLoot("Potion of Dexterity", 1.0f),
					new ItemLoot("Ghastly Equipment Crystal", 0.2f),
					new TierLoot(12, LootType.Weapon, 0.1f),
					new TierLoot(5, LootType.Ability, 0.1f),
					new TierLoot(11, LootType.Armor, 0.1f),
					new TierLoot(12, LootType.Armor, 0.15f),
					new TierLoot(11, LootType.Weapon, 0.25f),
					new TierLoot(4, LootType.Ring, 0.1f),
					new TierLoot(5, LootType.Ring, 0.05f),
					new ItemLoot("Pirate King's Cutlass", 0.05f),
					new ItemLoot("Cannon", 0.025f),
					new ItemLoot("Dark Water Dagger", 0.01f)
					//new ItemLoot("Deadwater Docks Key", 0.01f)
					)
				);
		}
    }
}
