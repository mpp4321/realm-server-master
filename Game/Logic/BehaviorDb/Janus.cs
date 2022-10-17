using RotMG.Common;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;
using RoTMG.Game.Logic.Transitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wServer.logic;
using static RotMG.Game.Logic.Loots.TierLoot;

namespace RotMG.Game.Logic.Database
{
	public class Janus : IBehaviorDatabase
	{
		public void Init(BehaviorDb db)
		{
			db.Init("md dwGenerator",
				new ConditionalEffect(ConditionEffectIndex.Invincible)
		);
			db.Init("md LightKey",
				new State("base",
					// new ConditionalEffect(ConditionEffectIndex.Invincible),
					new State("Bullet15",
						new Shoot(10, count: 15, index: 0, cooldown: 800),
						new EntitiesWithinTransition(9999, "md dwGenerator", "MoveToJanus")
				),
					new State("MoveToJanus",
						new MoveTo(0.5f, 0, 6, true),
						new TimedTransition("Bullet8", 2000)
				),
					new State("Bullet8",
						new Shoot(10, count: 8, index: 1, cooldown: 1500, cooldownVariance: 500),
						new HealEntity(20, "md Janus the Doorwarden", 2000, cooldown: 9999),
						new EntitiesNotExistsTransition(9999, "MoveAwayJanus", "md dwGenerator")
				),
					new State("MoveAwayJanus",
						new MoveTo(0.5f, 0, -6, true),
						new TimedTransition("Bullet15", 2000)
				)
			)
		);
			db.Init("md LightKey 2",
				new State("base",
					//new ConditionalEffect(ConditionEffectIndex.Invincible),
					new State("Bullet15",
						new Shoot(10, count: 15, index: 0, cooldown: 800),
						new EntitiesWithinTransition(9999, "md dwGenerator", "MoveToJanus")
				),
					new State("MoveToJanus",
						new MoveTo(0.5f, 0, -6, true),
						new TimedTransition("Bullet8", 2000)
				),
					new State("Bullet8",
						new Shoot(10, count: 8, index: 1, cooldown: 1500, cooldownVariance: 500),
						new HealEntity(20, "md Janus the Doorwarden", 2000, cooldown: 9999),
						new EntitiesNotExistsTransition(9999, "MoveAwayJanus", "md dwGenerator")
				),
					new State("MoveAwayJanus",
						new MoveTo(0.5f, 0, 6, true),
						new TimedTransition("Bullet15", 2000)
				)
			)
		);
			db.Init("md LightKey 3",
				new State("base",
					//new ConditionalEffect(ConditionEffectIndex.Invincible),
					new State("Bullet15",
						new Shoot(10, count: 15, index: 0, cooldown: 800),
						new EntitiesWithinTransition(9999, "md dwGenerator", "MoveToJanus")
				),
					new State("MoveToJanus",
						new MoveTo(0.5f, -6, 0, true),
						new TimedTransition("Bullet8", 2000)
				),
					new State("Bullet8",
						new Shoot(10, count: 8, index: 1, cooldown: 1500, cooldownVariance: 500),
						new HealEntity(20, "md Janus the Doorwarden", 2000, cooldown: 9999),
						new EntitiesNotExistsTransition(9999, "MoveAwayJanus", "md dwGenerator")
				),
					new State("MoveAwayJanus",
						new MoveTo(0.5f, 6, 0, true),
						new TimedTransition("Bullet15", 2000)
				)
			)
		);
			db.Init("md LightKey 4",
				new State("base",
					//  new ConditionalEffect(ConditionEffectIndex.Invincible),
					new State("Bullet15",
						new Shoot(10, count: 15, index: 0, cooldown: 800),
						new EntitiesWithinTransition(9999, "md dwGenerator", "MoveToJanus")
				),
					new State("MoveToJanus",
						new MoveTo(0.5f, 6, 0, true),
						new TimedTransition("Bullet8", 2000)
				),
					new State("Bullet8",
						new Shoot(10, count: 8, index: 1, cooldown: 1500, cooldownVariance: 500),
						new HealEntity(20, "md Janus the Doorwarden", 2000, cooldown: 9999),
						new EntitiesNotExistsTransition(9999, "MoveAwayJanus", "md dwGenerator")
				),
					new State("MoveAwayJanus",
						new MoveTo(0.5f, -6, 0, true),
						new TimedTransition("Bullet15", 2000)
				)
			)
		);
			db.Init("md DarkKey",
				new State("base",
					//new ConditionalEffect(ConditionEffectIndex.Invincible),
					new State("Bullet15",
						new Shoot(10, count: 15, index: 0, cooldown: 800),
						new EntitiesWithinTransition(9999, "md dwGenerator", "MoveToJanus")
				),
					new State("MoveToJanus",
						new MoveTo(0.5f, 6, 6, true),
						new TimedTransition("Bullet8", 2000)
				),
					new State("Bullet8",
						new Shoot(10, count: 8, index: 1, cooldown: 1500, cooldownVariance: 500),
						new HealEntity(20, "md Janus the Doorwarden", 2000, cooldown: 9999),
						new EntitiesNotExistsTransition(9999, "MoveAwayJanus", "md dwGenerator")
				),
					new State("MoveAwayJanus",
						new MoveTo(0.5f, -6, -6, true),
						new TimedTransition("Bullet15", 2000)
				)
			)
		);
			db.Init("md DarkKey 2",
				new State("base",
					//new ConditionalEffect(ConditionEffectIndex.Invincible),
					new State("Bullet15",
						new Shoot(10, count: 15, index: 0, cooldown: 800),
						new EntitiesWithinTransition(9999, "md dwGenerator", "MoveToJanus")
				),
					new State("MoveToJanus",
						new MoveTo(0.5f, -6, -6, true),
						new TimedTransition("Bullet8", 2000)
				),
					new State("Bullet8",
						new Shoot(10, count: 8, index: 1, cooldown: 1000, cooldownVariance: 100),
						new HealEntity(20, "md Janus the Doorwarden", 2000, cooldown: 9999),
						new EntitiesNotExistsTransition(9999, "MoveAwayJanus", "md dwGenerator")
				),
					new State("MoveAwayJanus",
						new MoveTo(0.5f, 6, 6, true),
						new TimedTransition("Bullet15", 2000)
				)
			)
		);
			db.Init("md DarkKey 3",
				new State("base",
					//  new ConditionalEffect(ConditionEffectIndex.Invincible),
					new State("Bullet15",
						new Shoot(10, count: 15, index: 0, cooldown: 800),
						new EntitiesWithinTransition(9999, "md dwGenerator", "MoveToJanus")
				),
					new State("MoveToJanus",
						new MoveTo(0.5f, 6, -6, true),
						new TimedTransition("Bullet8", 2000)
				),
					new State("Bullet8",
						new Shoot(10, count: 8, index: 1, cooldown: 1500, cooldownVariance: 500),
						new HealEntity(20, "md Janus the Doorwarden", 2000, cooldown: 9999),
						new EntitiesNotExistsTransition(9999, "MoveAwayJanus", "md dwGenerator")
				),
					new State("MoveAwayJanus",
						new MoveTo(0.5f, -6, 6, true),
						new TimedTransition("Bullet15", 2000)
				)
			)
		);
			db.Init("md DarkKey 4",
				new State("base",
					// new ConditionalEffect(ConditionEffectIndex.Invincible),
					new State("Bullet15",
						new Shoot(10, count: 15, index: 0, cooldown: 800),
						new EntitiesWithinTransition(9999, "md dwGenerator", "MoveToJanus")
				),
					new State("MoveToJanus",
						new MoveTo(0.5f, -6, 6, true),
						new TimedTransition("Bullet8", 2000)
				),
					new State("Bullet8",
						new Shoot(10, count: 8, index: 1, cooldown: 1500, cooldownVariance: 500),
						new HealEntity(20, "md Janus the Doorwarden", 2000, cooldown: 9999),
						new EntitiesNotExistsTransition(9999, "MoveAwayJanus", "md dwGenerator")
				),
					new State("MoveAwayJanus",
						new MoveTo(0.5f, 6, -6, true),
						new TimedTransition("Bullet15", 2000)
				)
			)
		);

			db.Init("BD Portal Spawner 5",
				new State("base",
					new ClearRegionOnDeath(Region.Note),
					new ConditionalEffect(ConditionEffectIndex.Invincible),
					new TransitionFrom("base", "idle"),
					new State("idle",
						new EntitiesNotExistsTransition(9999, "activate", "Suit of Armor")
				),
					new State("activate",
						new Taunt(broadcast: true, cooldown: 0, "You thrash His Lordship's castle, kill his brothers and challenge us. Come, come if you dare."),
						new Suicide()
				)
			)
		);

			db.Init("md Janus the Doorwarden",
				new State("base",
					//new DropPortalOnDeath("Puppet Encore Portal", 1, 120),
					new HealthTransition(0.15f, "ragetime")
					{
						SubIndex = 0
					},
					new State("idle",
						new EntitiesNotExistsTransition(9999, "activate", "BD Portal Spawner 5")
				),
					new State("activate",
						new PlayerWithinTransition(8, "taunt")
				),
					new State("taunt",
						new Flash(0xFF0000, 2, 2),
						new ConditionalEffect(ConditionEffectIndex.Invincible),
						new Taunt(cooldown: 0, "You bare witness to Janus, Doorwarden of Oryx. You will soon regret your decisions, your soul sealed away --forever"),
						new TimedTransition("tosskeys", 5000)
				),
					new State("tosskeys",
						new Taunt(cooldown: 0, "Keys, protect your master"),
						new ConditionalEffect(ConditionEffectIndex.Invincible),
						new TossObject("md LightKey 3", 8, 0, cooldown: 9999999),
						new TossObject("md DarkKey 2", 10, 45, cooldown: 9999999),
						new TossObject("md LightKey 2", 8, 90, cooldown: 9999999),
						new TossObject("md DarkKey 3", 10, 135, cooldown: 9999999),
						new TossObject("md LightKey 4", 8, 180, cooldown: 9999999),
						new TossObject("md DarkKey", 10, 225, cooldown: 9999999),
						new TossObject("md LightKey", 8, 270, cooldown: 9999999),
						new TossObject("md DarkKey 4", 10, 315, cooldown: 9999999),
						new TimedTransition("base1", 2000)
				),
					new State("base1",
						new EntitiesNotExistsTransition(9999, "warn1", "md DarkKey", "md DarkKey 2", "md DarkKey 3", "md DarkKey 4", "md LightKey", "md LightKey 2", "md LightKey 3", "md LightKey 4"),
						new ConditionalEffect(ConditionEffectIndex.Invincible),
						new TransitionFrom("base1", "ringattacks"),
						new State("ringattacks",
							new RemoveEntity(9999, "md dwGenerator"),
							new ConditionalEffect(ConditionEffectIndex.Invulnerable),
							new Shoot(10, 10, index: 0, cooldown: 100),
							new TimedTransition("keyshelp", 10000)
					),
						new State("keyshelp",
							new Spawn("md dwGenerator", 1, 1, cooldown: 999999),
							new TimedTransition("ringattacks", 11000)
					)
				),
					new State("warn1",
						new Flash(0xFF0000, 1, 2),
						new ConditionalEffect(ConditionEffectIndex.Invincible),
						new Taunt(1.00f, "Up and Down!"),
						new TimedTransition("UpDown", 3000)
				),
					new State("UpDown",
						new Shoot(10, count: 8, fixedAngle: 0, shootAngle: 6, index: 1, cooldown: 50),
						new Shoot(10, count: 8, fixedAngle: 180, shootAngle: 6, index: 1, cooldown: 50),
						new Grenade(5f, 200, 3, 90f, cooldown: 3000),
						new Grenade(5f, 200, 3, 270f, cooldown: 3000),
						new TimedTransition("warn2", 5000)
				),
					new State("warn2",
						new Flash(0xFF0000, 1, 2),
						new ConditionalEffect(ConditionEffectIndex.Invincible),
						new Taunt(1.00f, "Left and Right!"),
						new TimedTransition("LeftRight", 3000)
				),
					new State("LeftRight",
						new Shoot(10, count: 8, fixedAngle: 90, shootAngle: 6, index: 1, cooldown: 50),
						new Shoot(10, count: 8, fixedAngle: 270, shootAngle: 6, index: 1, cooldown: 50),
						new Grenade(5f, 200, 3, 0f, cooldown: 3000),
						new Grenade(5f, 200, 3, 180f, cooldown: 3000),
						new TimedTransition("warn3", 5000)
				),
					new State("warn3",
						new Flash(0xFF0000, 1, 2),
						new Taunt(1.00f, "Quick! Up and Down!"),
						new ConditionalEffect(ConditionEffectIndex.Invincible),
						new TimedTransition("UpDown2", 3000)
				),
					new State("UpDown2",
						new Shoot(10, count: 8, fixedAngle: 0, shootAngle: 6, index: 1, cooldown: 50),
						new Shoot(10, count: 8, fixedAngle: 180, shootAngle: 6, index: 1, cooldown: 50),
						new Grenade(5f, 200, 3, 90f, cooldown: 3000),
						new Grenade(5f, 200, 3, 270f, cooldown: 3000),
						new TimedTransition("warn4", 5000)
				),
					new State("warn4",
						new Flash(0xFF0000, 1, 2),
						new ConditionalEffect(ConditionEffectIndex.Invincible),
						new Taunt(1.00f, "Hurry! Left and Right!"),
						new TimedTransition("LeftRight2", 3000)
				),
					new State("LeftRight2",
						new Shoot(10, count: 8, fixedAngle: 90, shootAngle: 6, index: 1, cooldown: 50),
						new Shoot(10, count: 8, fixedAngle: 270, shootAngle: 6, index: 1, cooldown: 50),
						new Grenade(5f, 200, 3, 0f, cooldown: 3000),
						new Grenade(5f, 200, 3, 180f, cooldown: 3000),
						new TimedTransition("tosskeys", 5000)
				),
					//spookyrage weird movement
					new State("ragetime",
						new TransitionFrom("ragetime", "1"),
						new State("1",
							new Prioritize(
								new Wander(0.4f),
								new Follow(0.6f, 8, 1)
						),
							new Shoot(10, 10, index: 0, cooldown: 200, cooldownVariance: 200),
							new TimedTransition("2", 6000)
					),
						new State("2",
							new StayBack(1, 6),
							new Shoot(10, 10, index: 0, cooldown: 100, cooldownVariance: 100),
							new TimedTransition("1", 6000)
					)
				)
			),
				new Threshold(0.02f,
					new ItemLoot("Potion of Vitality", 1.0f, min: 8),
					new ItemLoot("Oryx Equipment Crystal", 0.15f),
					new ItemLoot("Realm Equipment Crystal", 1.0f),
					new ItemLoot("Realm Equipment Crystal", 1.0f),
					new TierLoot(9, LootType.Weapon, 1f, r: new LootDef.RarityModifiedData(1.0f, 3, true)),
					new TierLoot(5, LootType.Ability, 0.9f),
					new TierLoot(9, LootType.Armor, 0.9f),
					new TierLoot(10, LootType.Weapon, 0.05f, r: new LootDef.RarityModifiedData(1.0f, 3, true)),
					new TierLoot(5, LootType.Ring, 0.8f),
					new ItemLoot("Bow of Janus rage", 0.025f),
					new ItemLoot("Eye of Janus", 0.025f),
					new ItemLoot("Angel's Halo", 0.025f),
					new ItemLoot("Key of Rage", 0.025f),
					new ItemLoot("Warden Armor", 0.025f),
					new ItemLoot("Timeworn Daggers", 0.025f)
			)
		);
		}
	}
}
