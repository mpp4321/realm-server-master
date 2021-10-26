
using RotMG.Common;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;

namespace RotMG.Game.Logic.Database
{
    class Tomb : IBehaviorDatabase
    {

        public void Init(BehaviorDb db)
        {
            db.Init("Tomb Defender",
                    new State("idle",
                        new Taunt(true, cooldown: 0, "THIS WILL NOW BE YOUR TOMB!"),
                        new ConditionalEffect(ConditionEffectIndex.Armored, true),
                        new Prioritize(
                            new Orbit(.3f, 5, target: "Tomb Boss Anchor", radiusVariance: 0.5f),
                            new Wander(0.3f)
                        ),
                        new HealthTransition(.989f, "weakning")
                    ),
                    new State("weakning",
                        new Prioritize(
                            new Orbit(.3f, 4, target: "Tomb Boss Anchor"),
                            new Wander(0.3f)
                        ),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 2000),
                        new Taunt(true, cooldown: 0, "Impudence! I am an Immortal, I needn't waste time on you!"),
                        new Shoot(12, 20, index: 3, cooldown: 10000),
                        new Shoot(12, 20, index: 3, cooldown: 10000, cooldownOffset: 1000),
                        new HealthTransition(.979f, "active")
                    ),
                    new State("active",
                        new ConditionalEffect(ConditionEffectIndex.Armored, duration: 12000),
                        new Prioritize(
                            new Orbit(.3f, 4, target: "Tomb Boss Anchor"),
                            new Wander(0.3f)
                        ),
                        new Shoot(3.8f, 8, 45, 2, 0, 0, cooldown: 1200),
                        new Shoot(12, 3, 100, 1, 0, 0, cooldown: 5400, predictive: 0.6f),
                        new Shoot(0, 6, 60, 0, 0, 0, cooldown: 5000),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 2000),
                        new HealthTransition(.7f, "boomerang")
                    ),
                    new State("boomerang",
                        new ConditionalEffect(ConditionEffectIndex.Armored, duration: 10000),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 2000),
                        new Prioritize(
                            new Orbit(.325f, 4, target: "Tomb Boss Anchor"),
                            new Wander(0.325f)
                        ),
                        new Shoot(0, 6, shootAngle: 60, index: 0, cooldown: 8000),
                        new Shoot(12, 1, index: 0, cooldown: 3000, predictive: 0.6f),
                        new Shoot(3.8f, 8, index: 2, cooldown: 1200),
                        new Shoot(12, 4, 60, 1, cooldown: 6000, predictive: 0.6f),
                        new Shoot(12, 3, 10, 1, cooldown: 6000, predictive: 0.6f),
                        new HealthTransition(.55f, "double shot")
                    ),
                    new State("double shot",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 4000),
                        new ConditionalEffect(ConditionEffectIndex.Armored, duration: 10000),
                        new Prioritize(
                            new Orbit(.35f, 4, target: "Tomb Boss Anchor"),
                            new Wander(0.35f)
                        ),
                        new Shoot(3.8f, 9, shootAngle: 40, index: 2, cooldown: 1000),
                        new Shoot(12, 2, 5, 0, cooldown: 3200, predictive: 0.6f),
                        new Shoot(12, 4, 30, 1, cooldown: 6000, predictive: 0.6f),
                        new Shoot(12, 2, 10, 1, cooldown: 6000, predictive: 0.6f),
                        new HealthTransition(.4f, "artifacts")
                    ),
                    new State("artifacts",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 6000),
                        new ConditionalEffect(ConditionEffectIndex.Armored, duration: 10000),
                        new Prioritize(
                            new Orbit(.375f, 6, target: "Tomb Boss Anchor"),
                            new Wander(0.375f)
                        ),
                        new Shoot(3.8f, 10, shootAngle: 36, index: 2, cooldown: 1000),
                        new Shoot(12, 2, 5, 0, cooldown: 3000, predictive: 0.6f),
                        new Shoot(12, 6, 24, 1, cooldown: 6000, predictive: 0.6f),
                        new Shoot(12, 3, 10, 1, cooldown: 6000, predictive: 0.6f),
                        new Spawn("Pyramid Artifact 1", 3, 0, cooldown: 0),
                        new Spawn("Pyramid Artifact 2", 2, 0, cooldown: 0),
                        new Spawn("Pyramid Artifact 3", 1, 0, cooldown: 0),
                        new HealthTransition(.25f, "artifacts 2")
                    ),
                    new State("artifacts 2",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 6000),
                        new ConditionalEffect(ConditionEffectIndex.ArmorBroken, duration: 10000),
                        new Taunt(true, cooldown: 0, "My artifacts shall prove my wall of defense is impenetrable!"),
                        new Prioritize(
                            new Orbit(.4f, 6, target: "Tomb Boss Anchor"),
                            new Wander(0.4f)
                        ),
                        new Shoot(4, 10, shootAngle: 36, index: 2, cooldown: 900),
                        new Shoot(12, 3, 15, 0, cooldown: 2800, predictive: 0.6f),
                        new Shoot(12, 7, 24, 1, cooldown: 6000, predictive: 0.6f),
                        new Shoot(12, 3, 10, 1, cooldown: 6000, predictive: 0.6f),
                        new Spawn("Pyramid Artifact 1", 3, 0, cooldown: 0),
                        new Spawn("Pyramid Artifact 2", 2, 0, cooldown: 0),
                        new Spawn("Pyramid Artifact 3", 1, 0, cooldown: 0),
                        new HealthTransition(.06f, "rage")
                    ),
                    new State("rage",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 2000),
                        new ConditionalEffect(ConditionEffectIndex.Armored, duration: 5400),
                        new Taunt(true, cooldown: 0, "The end of your path is here!"),
                        new Prioritize(
                            new StayCloseToSpawn(0.8f, 7),
                            new Follow(0.5f, 10, 2.4f)
                        ),
                        new Spawn("Pyramid Artifact 1", 3, 0, cooldown: 0),
                        new Spawn("Pyramid Artifact 2", 2, 0, cooldown: 0),
                        new Spawn("Pyramid Artifact 3", 2, 0, cooldown: 0),
                        new Flash(0xFF0000, 10, 6000),
                        new Shoot(0, 6, 60, 0, cooldown: 8000),
                        new Shoot(20, 1, 60, 0, cooldown: 1400, predictive: 0.6f),
                        new Shoot(20, 12, 6, 4, cooldown: 800),
                        new Shoot(20, 7, 24, 1, cooldown: 5500, predictive: 0.6f),
                        new Shoot(20, 3, 10, 1, cooldown: 5500, predictive: 0.6f),
                        new Shoot(0, 5, 5, 4, 0, 15, cooldown: 400)
                    ),
                new Threshold(0.0001f,
                    new ItemLoot("Potion of Life", 1),
                    new ItemLoot("Ring of the Pyramid", 0.01f, threshold: 0.01f),
                    new ItemLoot("Tome of Holy Protection", 0.01f)
                //new ItemLoot("Sword of Royal Majesty", 0.006f),
                //new ItemLoot("Ring of Ancient Slaves", 0.006f),
                //new ItemLoot("Seal of the Royal Priest", 0.006f),
                //new ItemLoot("Unbound Temple Armor", 0.006f)
                )
            );
            db.Init("Tomb Support",
                new State("base",
                    new State("idle",
                        new Taunt(true, cooldown: 0, "ENOUGH OF YOUR VANDALISM!"),
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Prioritize(
                            new Orbit(.3f, 4.8f, target: "Tomb Boss Anchor"),
                            new Wander(0.3f)
                        ),
                        new HealthTransition(.989f, "weakning")
                    ),
                    new State("weakning",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 2000),
                        new ConditionalEffect(ConditionEffectIndex.Armored, duration: 12000),
                        new Shoot(12, 20, index: 7, cooldown: 10000),
                        new Shoot(12, 20, index: 7, cooldown: 10000, cooldownOffset: 1000),
                        new Taunt(cooldown: 0, "Impudence! I am an immortal, I needn't take your seriously."),
                        new Prioritize(
                            new Orbit(.3f, 4.8f, target: "Tomb Boss Anchor"),
                            new Wander(0.3f)
                        ),
                        new HealthTransition(.979f, "active")
                    ),
                    new State("active",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 2000),
                        new Prioritize(
                            new Orbit(.4f, 4.8f, target: "Tomb Boss Anchor"),
                            new Wander(0.4f)
                        ),
                        new Shoot(12, 6, index: 6, cooldown: 6000, predictive: 0.6f),
                        new Shoot(10, 3, 120, 1, 0, cooldown: 6000, cooldownOffset: 4000),
                        new Shoot(10, 4, 90, 2, 0, cooldown: 5000, cooldownOffset: 6000),
                        new Shoot(10, 5, 72, 3, 0, cooldown: 4000, cooldownOffset: 8000),
                        new Shoot(10, 6, 60, 4, 0, cooldown: 8000, cooldownOffset: 10000),
                        new Shoot(12, 1, index: 5, cooldown: 800),
                        new HealthTransition(.9f, "boomerang")
                    ),
                    new State("boomerang",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 2000),
                        new Prioritize(
                            new Orbit(.4f, 4.2f, target: "Tomb Boss Anchor"),
                            new Wander(0.4f)
                        ),
                        new HealEntity(10, "Tomb Defender", 100, cooldown: 500),
                        new Shoot(10, 4, 90, 2, 0, cooldown: 5000, cooldownOffset: 4200),
                        new Shoot(10, 5, 72, 3, 0, cooldown: 4000, cooldownOffset: 3400),
                        new Shoot(10, 6, 60, 4, 0, cooldown: 8000, cooldownOffset: 2800),
                        new HealthTransition(.7f, "paralyze")
                    ),
                    new State("paralyze",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 6000),
                        new Prioritize(
                            new Orbit(.45f, 4.2f, target: "Tomb Boss Anchor"),
                            new Wander(0.45f)
                        ),
                        new Shoot(12, 1, index: 6, cooldown: 5600, predictive: 0.6f),
                        new Shoot(10, 3, 120, 1, 0, cooldown: 5600, cooldownOffset: 2000),
                        new Shoot(10, 4, 90, 2, 0, cooldown: 4700, cooldownOffset: 3000),
                        new Shoot(10, 5, 72, 3, 0, cooldown: 3800, cooldownOffset: 4000),
                        new Shoot(10, 6, 60, 4, 0, cooldown: 7800, cooldownOffset: 5000),
                        new Shoot(12, 1, index: 5, cooldown: 800),
                        new HealthTransition(.5f, "artifacts")
                    ),
                    new State("artifacts",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 6000),
                        new Prioritize(
                            new Orbit(.5f, 4.2f, target: "Tomb Boss Anchor"),
                            new Wander(0.5f)
                        ),
                        new Shoot(12, 1, index: 6, cooldown: 5300, predictive: 0.6f),
                        new Shoot(10, 3, 120, 1, 0, cooldown: 5300),
                        new Shoot(10, 4, 90, 2, 0, cooldown: 4700),
                        new Shoot(10, 5, 72, 3, 0, cooldown: 3600),
                        new Shoot(10, 6, 60, 4, 0, cooldown: 7200),
                        new Shoot(12, 1, index: 5, cooldown: 700),
                        new HealEntity(10, "Tomb Attacker", 40, cooldown: 500),
                        new Spawn("Sphinx Artifact 1", 3, 0, cooldown: 0),
                        new Spawn("Sphinx Artifact 2", 3, 0, cooldown: 0),
                        new Spawn("Sphinx Artifact 3", 3, 0, cooldown: 0),
                        new HealthTransition(.3f, "double shoot")
                    ),
                    new State("double shoot",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 6000),
                        new Prioritize(
                            new Orbit(.4f, 4.2f, target: "Tomb Boss Anchor"),
                            new Wander(0.4f)
                        ),
                        new Shoot(12, 1, index: 6, cooldown: 5000, predictive: 0.6f),
                        new Shoot(10, 3, 120, 1, 0, cooldown: 5000),
                        new Shoot(10, 4, 90, 2, 0, cooldown: 4800),
                        new Shoot(10, 5, 72, 3, 0, cooldown: 3400),
                        new Shoot(10, 6, 60, 4, 0, cooldown: 6400),
                        new Shoot(12, 1, index: 5, cooldown: 600),
                        new Spawn("Sphinx Artifact 1", 3, 0, cooldown: 0),
                        new Spawn("Sphinx Artifact 2", 3, 0, cooldown: 0),
                        new Spawn("Sphinx Artifact 3", 3, 0),
                        new HealthTransition(.06f, "rage")
                    ),
                    new State("rage",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 2000),
                        new Flash(0xFF0000, 10, 6000),
                        new Taunt(true, cooldown: 0, "This cannot be! You shall not succeed!"),
                        new Prioritize(
                            new StayCloseToSpawn(0.8f, 10),
                            new Follow(0.65f, 10, 2.4f, duration: 30000, cooldown: 6000)
                        ),
                        new Spawn("Sphinx Artifact 1", 3, 0, cooldown: 0),
                        new Spawn("Sphinx Artifact 2", 3, 0, cooldown: 0),
                        new Spawn("Sphinx Artifact 3", 3, 0, cooldown: 0),
                        new Shoot(12, 2, shootAngle: 10, index: 6, cooldown: 4000, predictive: 0.6f),
                        new Shoot(12, 2, shootAngle: 15, index: 0, cooldown: 700, predictive: 0.6f),
                        new Shoot(10, 3, 120, 1, 0, cooldown: 5000),
                        new Shoot(10, 4, 90, 2, 0, cooldown: 3600),
                        new Shoot(10, 5, 72, 3, 0, cooldown: 2800),
                        new Shoot(10, 6, 60, 4, 0, cooldown: 6000),
                        new Shoot(12, 1, index: 5, cooldown: 600)
                    )
                ),
                new Threshold(0.00001f,
                    new ItemLoot("Potion of Life", 1),
                    new ItemLoot("Ring of the Sphinx", 0.01f)
                )
            );
            db.Init("Tomb Attacker",
                    new State("idle",
                        new Taunt(true, cooldown: 0, "YOU HAVE AWAKENED US!"),
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Prioritize(
                            new Orbit(.3f, 5.8f, target: "Tomb Boss Anchor"),
                            new Wander(0.3f)
                        ),
                        new HealthTransition(.989f, "weakning")
                    ),
                    new State("weakning",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 2000),
                        new Prioritize(
                            new Orbit(.3f, 5.8f, target: "Tomb Boss Anchor"),
                            new Wander(0.3f)
                        ),
                        new Shoot(12, 20, index: 3, cooldown: 10000),
                        new Shoot(12, 20, index: 3, cooldown: 10000, cooldownOffset: 1000),
                        new HealthTransition(.979f, "active")
                    ),
                    new State("active",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 2000),
                        new Prioritize(
                            new Orbit(.4f, 5.8f, target: "Tomb Boss Anchor"),
                            new Wander(0.4f)
                        ),
                        new Shoot(12, 2, 5, 2, cooldown: 600, predictive: 0.6f),
                        new Grenade(3, 120, 10, cooldown: 3500),
                        new Grenade(4, 120, 10, cooldown: 6000),
                        new Shoot(0, 6, shootAngle: 60, index: 0, cooldown: 5000),
                        new HealthTransition(.72f, "lets dance")
                    ),
                    new State("lets dance",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 6000),
                        new Prioritize(
                            new Orbit(.4f, 5.8f, target: "Tomb Boss Anchor"),
                            new Wander(0.4f)
                        ),
                        new Shoot(12, 2, 40, 0, cooldown: 2000, predictive: 0.6f),
                        new Shoot(12, 8, 45, 1, 0, cooldown: 5000),
                        new Shoot(12, 4, 80, 2, cooldown: 1000, predictive: 0.6f),
                        new Shoot(12, 1, 40, 0, cooldown: 1400),
                        new Shoot(12, 1, 80, 2, cooldown: 400, predictive: 0.6f),
                        new Grenade(3, 100, 10, cooldown: 4000),
                        new Grenade(4, 120, 10, cooldown: 6000),
                        new Spawn("Scarab", 4, 0),
                        new HealthTransition(.6f, "more muthafucka")
                    ),
                    new State("more muthafucka",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 6000),
                        new Prioritize(
                            new Orbit(.4f, 5, target: "Tomb Boss Anchor"),
                            new Wander(0.4f)
                        ),
                        new Shoot(12, 2, 40, 0, cooldown: 2000, predictive: 0.6f),
                        new Shoot(12, 10, 36, 1, 0, cooldown: 5000),
                        new Shoot(12, 5, 60, 2, cooldown: 1200, predictive: 0.6f),
                        new Shoot(12, 1, 40, 0, cooldown: 1200),
                        new Shoot(12, 1, 80, 2, cooldown: 600, predictive: 0.6f),
                        new Grenade(3, 100, 10, cooldown: 3750),
                        new Grenade(4, 120, 10, cooldown: 5750),
                        new Spawn("Scarab", 4, 0),
                        new HealthTransition(.4f, "artifacts")
                    ),
                    new State("artifacts",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 6000),
                        new Prioritize(
                            new Orbit(.5f, 5, target: "Tomb Boss Anchor"),
                            new Wander(0.5f)
                        ),
                        new Shoot(12, 4, 35, 0, cooldown: 2000, predictive: 0.6f),
                        new Shoot(12, 10, 36, 1, 0, cooldown: 15000),
                        new Shoot(12, 5, 60, 2, cooldown: 1200, predictive: 0.6f),
                        new Shoot(12, 1, 40, 0, cooldown: 1400),
                        new Shoot(12, 2, 15, 2, cooldown: 600, predictive: 0.6f),
                        new Grenade(3, 100, 10, cooldown: 3500),
                        new Grenade(4, 120, 10, cooldown: 5500),
                        new Grenade(6, 40, 10, cooldown: 2000),
                        new Spawn("Scarab", 4, 0),
                        new Spawn("Nile Artifact 1", 2, 0, cooldown: 0),
                        new Spawn("Nile Artifact 2", 2, 0, cooldown: 0),
                        new Spawn("Nile Artifact 3", 2, 0, cooldown: 0),
                        new HealthTransition(.2f, "artifacts 2")
                    ),
                    new State("artifacts 2",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 6000),
                        new Prioritize(
                            new Orbit(.55f, 5, target: "Tomb Boss Anchor"),
                            new Wander(0.55f)
                        ),
                        new Shoot(12, 4, 35, 0, cooldown: 1800, predictive: 0.6f),
                        new Shoot(12, 10, 36, 1, 0, cooldown: 14000),
                        new Shoot(12, 5, 60, 2, cooldown: 1200, predictive: 0.6f),
                        new Shoot(12, 1, 40, 0, cooldown: 1400),
                        new Shoot(12, 1, 15, 2, cooldown: 600, predictive: 0.6f),
                        new Grenade(3, 100, 10, cooldown: 3000),
                        new Grenade(4, 120, 10, cooldown: 5000),
                        new Spawn("Scarab", 4, 0),
                        new Spawn("Nile Artifact 1", 2, 0, cooldown: 0),
                        new Spawn("Nile Artifact 2", 2, 0, cooldown: 0),
                        new Spawn("Nile Artifact 3", 2, 0, cooldown: 0),
                        new HealthTransition(.06f, "rage")
                    ),
                    new State("rage",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 2000),
                        new Taunt(true, cooldown: 0, "Argh! You shall pay for your crimes!"),
                        new Flash(0xFF0000, 10, 6000),
                        new Prioritize(
                            new StayCloseToSpawn(2, 7),
                            new StayBack(0.9f, 1.8f, null)
                        ),
                        new Shoot(12, 2, 35, 0, cooldown: 1000, predictive: 0.6f),
                        new Shoot(12, 10, 36, 1, 0, cooldown: 4800),
                        new Shoot(12, 5, 42, 2, cooldown: 1200, predictive: 0.6f),
                        new Shoot(12, 2, 5, 0, cooldown: 1400),
                        new Shoot(12, 1, 15, 2, cooldown: 400, predictive: 0.6f),
                        new Grenade(3, 120, 12, cooldown: 3500),
                        new Grenade(4, 150, 12, cooldown: 5500),
                        new Spawn("Scarab", 4, 0),
                        new Spawn("Nile Artifact 1", 2, 0, cooldown: 0),
                        new Spawn("Nile Artifact 2", 2, 0, cooldown: 0),
                        new Spawn("Nile Artifact 3", 2, 0, cooldown: 0),
                        new Shoot(0, 1, 0, index: 5, 0, 15, cooldown: 400)
                    ),
                new Threshold(0.00001f,
                    new ItemLoot("Potion of Life", 1),
                    new ItemLoot("Ring of the Nile", 0.01f)
                )
            );
            //Minions
            db.Init("Pyramid Artifact 1",
                new State("base",
                    new Prioritize(
                        new Orbit(1, 2, target: "Tomb Defender", radiusVariance: 0.5f),
                        new Follow(0.85f, range: 1, duration: 5000, cooldown: 0)
                        ),
                    new Shoot(3, 3, 120, cooldown: 2500)
                    )
            );
            db.Init("Pyramid Artifact 2",
                new State("base",
                    new Prioritize(
                        new Orbit(1, 2, target: "Tomb Attacker", radiusVariance: 0.5f),
                        new Follow(0.85f, range: 1, duration: 5000, cooldown: 0)
                        ),
                    new Shoot(3, 3, 120, cooldown: 2500)
                    )
            );
            db.Init("Pyramid Artifact 3",
                new State("base",
                    new Prioritize(
                        new Orbit(1, 2, target: "Tomb Support", radiusVariance: 0.5f),
                        new Follow(0.85f, range: 1, duration: 5000, cooldown: 0)
                        ),
                    new Shoot(3, 3, 120, cooldown: 2500)
                    )
            );
            db.Init("Sphinx Artifact 1",
                    new State("base",
                        new Prioritize(
                            new Orbit(1, 2, target: "Tomb Defender", radiusVariance: 0.5f),
                            new Follow(0.85f, range: 1, duration: 5000, cooldown: 0)
                            ),
                        new Shoot(12, 3, 120, cooldown: 2500)
                        )
                );
            db.Init("Sphinx Artifact 2",
                new State("base",
                    new Prioritize(
                        new Orbit(1, 2, target: "Tomb Attacker", radiusVariance: 0.5f),
                        new Follow(0.85f, range: 1, duration: 5000, cooldown: 0)
                        ),
                    new Shoot(12, 3, 120, cooldown: 2500)
                    )
            );
            db.Init("Sphinx Artifact 3",
                new State("base",
                    new Prioritize(
                        new Orbit(1, 2, target: "Tomb Support", radiusVariance: 0.5f),
                        new Follow(0.85f, range: 1, duration: 5000, cooldown: 0)
                        ),
                    new Shoot(12, 3, 120, cooldown: 2500)
                    )
            );
            db.Init("Nile Artifact 1",
                new State("base",
                    new Prioritize(
                        new Orbit(1, 2, target: "Tomb Defender", radiusVariance: 0.5f),
                        new Follow(0.85f, range: 1, duration: 5000, cooldown: 0)
                        ),
                    new Shoot(12, 3, 120, cooldown: 2500)
                    )
            );
            db.Init("Nile Artifact 2",
                new State("base",
                    new Prioritize(
                        new Orbit(1, 2, target: "Tomb Attacker", radiusVariance: 0.5f),
                        new Follow(0.85f, range: 1, duration: 5000, cooldown: 0)
                        ),
                    new Shoot(12, 3, 120, cooldown: 2500)
                    )
            );
            db.Init("Nile Artifact 3",
                new State("base",
                    new Prioritize(
                        new Orbit(1, 2, target: "Tomb Support", radiusVariance: 0.5f),
                        new Follow(0.85f, range: 1, duration: 5000, cooldown: 0)
                        ),
                    new Shoot(12, 3, 120, cooldown: 2500)
                    )
            );
            db.Init("Tomb Defender Statue",
                    new State("base",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntitiesNotExistsTransition(1000, "checkActive", "Inactive Sarcophagus"),
                        new EntitiesNotExistsTransition(1000, "checkInactive", "Active Sarcophagus")
                        ),
                    new State("checkActive",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntitiesNotExistsTransition(1000, "ItsGoTime", "Active Sarcophagus")
                        ),
                    new State("checkInactive",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntitiesNotExistsTransition(1000, "ItsGoTime", "Inactive Sarcophagus")
                        ),
                    new State("ItsGoTime",
                        new Transform("Tomb Defender")
                        )
                   );
            db.Init("Tomb Support Statue",
                    new State("base",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntitiesNotExistsTransition(1000, "checkActive", "Inactive Sarcophagus"),
                        new EntitiesNotExistsTransition(1000, "checkInactive", "Active Sarcophagus")
                        ),
                    new State("checkActive",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntitiesNotExistsTransition(1000, "ItsGoTime", "Active Sarcophagus")
                        ),
                    new State("checkInactive",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntitiesNotExistsTransition(1000, "ItsGoTime", "Inactive Sarcophagus")
                        ),
                    new State("ItsGoTime",
                        new Transform("Tomb Support")
                        )
                    );
            db.Init("Tomb Attacker Statue",
                    new State("base",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntitiesNotExistsTransition(1000, "checkActive", "Inactive Sarcophagus"),
                        new EntitiesNotExistsTransition(1000, "checkInactive", "Active Sarcophagus")
                        ),
                    new State("checkActive",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntitiesNotExistsTransition(1000, "ItsGoTime", "Active Sarcophagus")
                        ),
                    new State("checkInactive",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntitiesNotExistsTransition(1000, "ItsGoTime", "Inactive Sarcophagus")
                        ),
                    new State("ItsGoTime",
                        new Transform("Tomb Attacker")
                        )
            );
            db.Init("Scarab",
                    new NoPlayerWithinTransition(7, "Idle"),
                    new PlayerWithinTransition(7, "Chase"),
                    new State("Idle",
                        new Wander(.1f)
                    ),
                    new State("Chase",
                        new Follow(1.5f, 7, 0),
                        new Shoot(3, index: 1, cooldown: 500)
                    )
            );
            db.Init("Tomb Boss Anchor",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new DropPortalOnDeath("Glowing Realm Portal", 100),
                    new State("Idle",
                        new EntitiesNotExistsTransition(300, "Death", "Tomb Support", "Tomb Attacker", "Tomb Defender",
                            "Active Sarcophagus", "Tomb Defender Statue", "Tomb Support Statue", "Tomb Attacker Statue")
                    ),
                    new State("Death",
                        new Decay(0)
                    )
            );
        }

    }
}
