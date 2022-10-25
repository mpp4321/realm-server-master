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
using static RotMG.Game.Logic.Loots.TierLoot;

namespace RotMG.Game.Logic.Database
{
    public class AncientHermit : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {
            db.Init("Ancient Hermit God",
                HPScale.BOSS_HP_SCALE_DEFAULT(),
                new State("base",
                    new TransferDamageOnDeath("Ancient Hermit God Drop"),
                    new OrderOnDeath(20, "Ancient Hermit God Tentacle Spawner", "Die"),
                    new OrderOnDeath(20, "Ancient Hermit God Drop", "Die"),
                    new TransitionFrom("base", "spawn tentacle"),
                    new State("Spawn Tentacle",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new SetAltTexture(2),
                        new Order(20, "Ancient Hermit God Tentacle Spawner", "Tentacle"),
                        new EntitiesWithinTransition( 20, "Hermit God Tentacle", "Sleep")
                    ),
                    new State("Sleep",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Order(20, "Ancient Hermit God Tentacle Spawner", "Minions"),
                        new TimedTransition("Waiting", 1000)
                        ),
                    new State("Waiting",
                        new SetAltTexture(3),
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new EntitiesNotExistsTransition(20, "Wake", "Hermit God Tentacle")
                        ),
                    new State("Wake",
                        new SetAltTexture(2),
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new TossObject("Hermit Minion", 10, angle: 0, throwEffect: true),
                        new TossObject("Hermit Minion", 10, angle: 45, throwEffect: true),
                        new TossObject("Hermit Minion", 10, angle: 90, throwEffect: true),
                        new TossObject("Hermit Minion", 10, angle: 135, throwEffect: true),
                        new TossObject("Hermit Minion", 10, angle: 180, throwEffect: true),
                        new TossObject("Hermit Minion", 10, angle: 225, throwEffect: true),
                        new TossObject("Hermit Minion", 10, angle: 270, throwEffect: true),
                        new TossObject("Hermit Minion", 10, angle: 315, throwEffect: true),
                        new TimedTransition("Spawn Whirlpool", 100)
                        ),
                    new State("Spawn Whirlpool",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Order(20, "Ancient Hermit God Tentacle Spawner", "Whirlpool"),
                        new EntitiesWithinTransition(20, "Ancient Whirlpool", "Attack1")
                        ),
                    new State("Attack1",
                        new SetAltTexture(0),
                        new Prioritize(
                            new Wander(0.3f),
                            new StayCloseToSpawn(0.5f, 5)
                            ),
                        new Shoot(20, count: 3, shootAngle: 5, cooldown: 300),
                        new TimedTransition("Attack2", 6000)
                        ),
                    new State("Attack2",
                        new Prioritize(
                            new Wander(0.3f),
                            new StayCloseToSpawn(0.5f, 5)
                            ),
                        new Order(20, "Whirlpool", "Die"),
                        new Shoot(20, count: 1, defaultAngle: 0, fixedAngle: 0, rotateAngle: 45, index: 1,
                            cooldown: 1000),
                        new Shoot(20, count: 1, defaultAngle: 0, fixedAngle: 180, rotateAngle: 45, index: 1,
                            cooldown: 1000),
                        new TimedTransition("Spawn Tentacle", 6000)
                        )
                    )
            );

            db.Init("Ancient Hermit God Tentacle Spawner",
                new State("base",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("wait"),
                    new State("Minions",
                        new Reproduce("Hermit Minion", 40, 20, cooldown: 100),
                        new TimedTransition("wait", 200)
                    ),
                    new State("Tentacle",
                        new Reproduce("Hermit God Tentacle", 3, 1, cooldown: 100),
                        new TimedTransition("wait", 200)
                    ),
                    new State("Whirlpool",
                        new Reproduce("Ancient Whirlpool", 3, 1, cooldown: 100),
                        new TimedTransition("OTMinions", 200)
                    ), 
                    new State("OTMinions", 
                        new Spawn("Fishman", 1, 1, 0.3f, cooldown: 1000, dispersion: 1.0f),
                        new Spawn("Fishman Warrior", 1, 1, 0.3f, cooldown: 1000, dispersion: 1.0f),
                        new Spawn("Sea Horse", 1, 1, 0.5f, cooldown: 1000, dispersion: 1.0f),
                        new Spawn("Sea Mare", 1, 1, 0.2f, cooldown: 1000, dispersion: 1.0f),
                        new Spawn("Ink Bubble", 1, 1, 0.5f, cooldown: 1000, dispersion: 1.0f),
                        new TimedTransition("wait", 100)
                    )
                    )
                );

            db.Init("Ancient Whirlpool",
                new State("base",
                    new State("Attack",
                        new EntitiesNotExistsTransition(100, "Die", "Hermit God"),
                        new Prioritize(
                            new Orbit(0.3f, 6, 10, "Hermit God")
                        ),
                        new Flash(0xFF33FF00, 1.0, 1),
                        new Shoot(0, 1, fixedAngle: 0, rotateAngle: 30, cooldown: 400),
                        new Shoot(0, 1, fixedAngle: 30, rotateAngle: 30, cooldown: 400)
                        ),
                    new State("Die",
                        new Shoot(0, 8, fixedAngle: 360 / 8, cooldown: 10000),
                        new Suicide()
                        )
                    )
            );
            db.Init("Ancient Hermit God Drop",
                new State("base",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new State("Waiting"),
                    new State("Die",
                        new Suicide()
                    )
                ),
                new Threshold(0.03f,
                    new TierLoot(11, LootType.Weapon, 0.2f, r: new(0.8f, 4, true)),
                    new TierLoot(11, LootType.Armor, 0.2f, r: new(0.8f, 4, true)),
                    new TierLoot(10, LootType.Weapon, 0.35f, r: new(0.8f, 4, true)),
                    new TierLoot(10, LootType.Armor, 0.35f, r: new(0.8f, 4, true)),
                    new ItemLoot("King of the Jellyfish", 0.01f),
                    new ItemLoot("Potion of Wisdom", 0.25f),
                    new ItemLoot("Potion of Dexterity", 0.15f),
                    new ItemLoot("Potion of Defense", 0.25f),
                    new ItemLoot("Potion of Wisdom", 0.15f),
                    new Threshold(0.01f, LootTemplates.CrystalsHardRegular()),
                    new ItemLoot("Coral Bow", 0.15f, r: new(1.0f, 1, true)),
                    new ItemLoot("Helm of the Juggernaut", 0.025f, r: new(1.0f, 0, true))
                )
            );

        }
    }
}
