using RotMG.Common;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;
using static RotMG.Game.Logic.Loots.TierLoot;

namespace RotMG.Game.Logic.Database
{
    class CandylandHuntingGrounds : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {

            db.Init("Fairy",
                new StayCloseToSpawn(1, 13),
                new Prioritize(
                    new Protect(0.8f, "Beefy Fairy", 15, 8,
                        6),
                    new Orbit(0.6f, 4, 7)
                ),
                new Wander(0.25f),
                new Shoot(10, 2, 30, 0, predictive: 1,
                    cooldown: 2000),
                new Shoot(10, 1, index: 0, predictive: 1, cooldown: 2000,
                    cooldownOffset: 1000)
            );

            db.Init("Beefy Fairy",
                new StayCloseToSpawn(1, 13),
                new Prioritize(
                    new Protect(0.8f, "Beefy Fairy", 15, 8,
                        6),
                    new Orbit(0.6f, 4, 7)
                ),
                new Wander(0.35f),
                new Shoot(10, 2, 30, 0, predictive: 1,
                    cooldown: 2000),
                new Shoot(10, 1, index: 0, predictive: 1, cooldown: 2000,
                    cooldownOffset: 1000)
            );

            db.Init("Big Creampuff",
                new Spawn("Small Creampuff", 4, 0, givesNoXp: false),
                new Shoot(10, 1, index: 0, predictive: 1, cooldown: 1400),
                new Shoot(4.4f, 5, 12, 1, predictive: 0.6f,
                    cooldown: 800),
                new Prioritize(
                    new Charge(1.4f, 11, 4200),
                    new StayBack(1, 4),
                    new StayBack(0.5f, 7)
                ),
                new StayCloseToSpawn(1.35f, 13),
                new Wander(0.2f)
            );

            db.Init("Small Creampuff",
                new Shoot(5, 3, 12, 1, predictive: 0.6f,
                    cooldown: 1000),
                new StayCloseToSpawn(1.3f, 13),
                new Prioritize(
                    new Charge(1.3f, 13, 2500),
                    new Protect(0.8f, "Big Creampuff", 15, 7,
                        6)
                ),
                new Wander(0.6f)
            );

            db.Init("Hard Candy",
                new Shoot(5, 3, 12, 0, predictive: 0.6f,
                    cooldown: 1000),
                new StayCloseToSpawn(1.3f, 13),
                new Prioritize(
                    new Charge(1.3f, 13, 2500)
                ),
                new Wander(0.6f)
            );

            db.Init("Unicorn",
                new Prioritize(
                    new Charge(1.4f, 11, 3800),
                    new StayBack(0.8f, 6)
                ),
                new State("Start",
                    new State("Shoot",
                        new Shoot(10, 1, index: 0, predictive: 1, cooldown: 200),
                        new TimedTransition("ShootPause", 850)
                    ),
                    new State("ShootPause",
                        new Shoot(4.5f, 3, 10, 1, predictive: 0.4f,
                            cooldown: 3000, cooldownOffset: 500),
                        new Shoot(4.5f, 3, 10, 1, predictive: 0.4f,
                            cooldown: 3000, cooldownOffset: 1000),
                        new Shoot(4.5f, 3, 10, 1, predictive: 0.4f,
                            cooldown: 3000, cooldownOffset: 1500),
                        new TimedTransition("Shoot", 1200)
                    )
                )
            );

            db.Init("Spilled Icecream",
                new StayBack(0.5f, 3, "Spilled Icecream"),
                new Prioritize(
                    new Charge(0.7f, 7, 3800),
                    new StayBack(0.8f, 6)
                ),
                new State("Start",
                    new State("Shoot",
                        new Shoot(10, 1, index: 0, predictive: 1, cooldown: 300),
                        new TimedTransition("ShootPause", 850)
                    ),
                    new State("ShootPause",
                        new Shoot(4.5f, 3, 10, 0, predictive: 0.4f,
                            cooldown: 3000, cooldownOffset: 500),
                        new Shoot(4.5f, 3, 10, 0, predictive: 0.4f,
                            cooldown: 3000, cooldownOffset: 1000),
                        new Shoot(4.5f, 3, 10, 0, predictive: 0.4f,
                            cooldown: 3000, cooldownOffset: 1500),
                        new TimedTransition("Shoot", 1200)
                    )
                )
            );

            db.Init("Wishing Troll",
                new State("BaseAttack",
                    new Shoot(10, 3, 15, 0, predictive: 1,
                        cooldown: 1400),
                    new Grenade(radius: 5, damage: 100, range: 8, cooldown: 3000),
                    new Shoot(10, 1, index: 0, predictive: 1, cooldown: 2000),
                    new State("Choose",
                        new TimedRandomTransition(3800, "Run", "Attack")
                    ),
                    new State("Run",
                        new StayBack(1.1f, 10),
                        new TimedRandomTransition(1200, "Choose")
                    ),
                    new State("Attack",
                        new Charge(1.2f, 11, 1000),
                        new TimedRandomTransition(1000, "Choose")
                    ),
                    new HealthTransition(0.6f, "NextAttack")
                ),
                new State("NextAttack",
                    new Shoot(10, 5, 10, 0, predictive: 0.5f,
                        angleOffset: 0.4f, cooldown: 2000),
                    new Shoot(10, 1, index: 0, predictive: 1, cooldown: 2000),
                    new Shoot(10, 3, 15, 0, predictive: 1,
                        angleOffset: 1, cooldown: 4000),
                    new Grenade(radius: 5, damage: 100, range: 8, cooldown: 3000),
                    new State("Choose2",
                        new TimedRandomTransition(3800, "Run2", "Attack2")
                    ),
                    new State("Run2",
                        new StayBack(1.5f, 10),
                        new TimedTransition("Choose2", 1500),
                        new PlayerWithinTransition(3.5f, "Boom", seeInvis: false)
                    ),
                    new State("Attack2",
                        new Charge(1.2f, 11, 1000),
                        new TimedTransition("Choose2", 1000),
                        new PlayerWithinTransition(3.5f, "Boom", seeInvis: false)
                    ),
                    new State("Boom",
                        new Shoot(0, 20, 18, 1, cooldown: 2000),
                        new TimedTransition("Choose2", 200)
                    )
                ),
                new StayCloseToSpawn(1.5f, 15),
                new Prioritize(
                    new Follow(1, 11, 5)
                ),
                new Wander(0.4f)
            );

            db.Init("Candy Gnome",
                new State("Ini",
                    new Wander(0.4f),
                    new PlayerWithinTransition(14, "Main", seeInvis: true)
                ),
                new State("Main",
                    new Follow(1.4f, 17),
                    new TimedTransition("Flee", 1600)
                ),
                new State("Flee",
                    new PlayerWithinTransition(11, "RunAwayMed", seeInvis: true),
                    new PlayerWithinTransition(8, "RunAwayMedFast", seeInvis: true),
                    new PlayerWithinTransition(5, "RunAwayFast", seeInvis: true),
                    new PlayerWithinTransition(16, "RunAwaySlow", seeInvis: true)
                ),
                new State("RunAwayFast",
                    new StayBack(1.9f, 30),
                    new TimedRandomTransition(1000, "RunAwayMedFast", "RunAwayMed", "RunAwaySlow")
                ),
                new State("RunAwayMedFast",
                    new StayBack(1.45f, 30),
                    new TimedRandomTransition(1000, "RunAwayMed", "RunAwaySlow")
                ),
                new State("RunAwayMed",
                    new StayBack(1.1f, 30),
                    new TimedRandomTransition(1000, "RunAwayMedFast", "RunAwaySlow")
                ),
                new State("RunAwaySlow",
                    new StayBack(0.8f, 30),
                    new TimedRandomTransition(1000, "RunAwayMedFast", "RunAwayMed")
                ),
                new DropPortalOnDeath("Candyland Portal"),
                new Threshold(0.01f,
                    new ItemLoot("Rock Candy", 0.15f),
                    new ItemLoot("Red Gumball", 0.15f),
                    new ItemLoot(item: "Realm Equipment Crystal", 0.05f),
                    new ItemLoot("Purple Gumball", 0.15f),
                    new ItemLoot("Blue Gumball", 0.15f),
                    new ItemLoot("Green Gumball", 0.15f),
                    new ItemLoot("Yellow Gumball", 0.15f)
                )
            );

            db.Init("MegaRototo",
                    HPScale.BOSS_HP_SCALE_DEFAULT(),
                    new Reproduce(children: "Tiny Rototo", densityRadius: 6, densityMax: 3, cooldown: 7000),
                    new State("Follow",
                        new Prioritize(
                            new Follow(speed: 0.45f, acquireRange: 11, range: 5),
                            new Wander(speed: 0.4f)
                        ),
                        new Shoot(12, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 1400),
                        new Shoot(12, count: 4, shootAngle: 90, index: 1, cooldown: 1400),
                        new Follow(speed: 0.45f, acquireRange: 11, range: 5),
                        new TimedRandomTransition(4300, "FlameThrower", "StayBack")
                    ),
                    new State("StayBack",
                        new Shoot(12, count: 3, shootAngle: 16, index: 1, predictive: 0.6f, cooldown: 1200),
                        new Shoot(12, count: 3, shootAngle: 16, index: 0, predictive: 0.9f, cooldown: 600),
                        new StayBack(speed: 0.5f, distance: 10, entity: null),
                        new TimedTransition("Follow", 2400)
                    ),
                    new State("FlameThrower",
                        new TransitionFrom("Flamethrower", "FB1"),
                        new TimedTransition("Follow", 4000),
                        new State("FB1",
                            new Shoot(12, count: 2, shootAngle: 16, index: 2, cooldown: 1, cooldownOffset: 400),
                            new Shoot(12, count: 1, index: 3, cooldown: 1, cooldownOffset: 400),
                            new TimedTransition("FB2", 2000)
                        ),
                        new State("FB2",
                            new Shoot(12, count: 2, shootAngle: 16, index: 3, cooldown: 1, cooldownOffset: 400),
                            new Shoot(12, count: 1, index: 2, cooldown: 1, cooldownOffset: 400)
                        )
                    ),
                new Threshold(0.01f,
                    new ItemLoot(item: "Ring Pop", 0.02f),
                    new ItemLoot(item: "Rock Candy", 0.08f),
                    new ItemLoot(item: "Candy-Coated Armor", 0.01f),
                    new ItemLoot(item: "Red Gumball", 0.15f),
                    new ItemLoot(item: "Purple Gumball", 0.15f),
                    new ItemLoot(item: "Blue Gumball", 0.15f),
                    new ItemLoot(item: "Realm Equipment Crystal", 0.05f),
                    new ItemLoot(item: "Green Gumball", 0.15f),
                    new ItemLoot(item: "Yellow Gumball", 0.15f),
                    new ItemLoot(item: "Pixie-Enchanted Sword", 0.01f),
                    new TierLoot(tier: 7, type: LootType.Weapon, 0.25f),
                    new TierLoot(tier: 6, type: LootType.Weapon, 0.4f),
                    new TierLoot(tier: 8, type: LootType.Armor, 0.25f),
                    new TierLoot(tier: 7, type: LootType.Armor, 0.4f),
                    new TierLoot(tier: 3, type: LootType.Ability, 0.25f),
                    new TierLoot(tier: 4, type: LootType.Ability, 0.125f),
                    new TierLoot(tier: 3, type: LootType.Ring, 0.25f),
                    new TierLoot(tier: 4, type: LootType.Ring, 0.125f)
                )
            );
            db.Init("Spoiled Creampuff",
                    HPScale.BOSS_HP_SCALE_DEFAULT(),
                    new Spawn(children: "Big Creampuff", maxChildren: 2, initialSpawn: 0, givesNoXp: false),
                    new Reproduce(children: "Big Creampuff", densityRadius: 24, densityMax: 2, cooldown: 25000),
                    new Shoot(10, count: 1, index: 0, predictive: 1, cooldown: 1400),
                    new Shoot(10, count: 5, shootAngle: 12, index: 1, predictive: 0.6f, cooldown: 800),
                    new Prioritize(
                        new Charge(speed: 1.4f, range: 11, coolDown: 4200),
                        new StayBack(speed: 1, distance: 4, entity: null),
                        new StayBack(speed: 0.5f, distance: 7, entity: null)
                    ),
                    new StayCloseToSpawn(speed: 1.35f, range: 13),
                    new Wander(speed: 0.4f),
                new Threshold(0.01f,
                    new ItemLoot(item: "Potion of Attack", 0.1f),
                    new ItemLoot(item: "Potion of Defense", 0.1f),
                    new ItemLoot(item: "Ring Pop", 0.02f),
                    new ItemLoot(item: "Rock Candy", 0.08f),
                    new ItemLoot(item: "Candy-Coated Armor", 0.005f),
                    new ItemLoot(item: "Realm Equipment Crystal", 0.05f),
                    new ItemLoot(item: "Red Gumball", 0.15f),
                    new ItemLoot(item: "Purple Gumball", 0.15f),
                    new ItemLoot(item: "Blue Gumball", 0.15f),
                    new ItemLoot(item: "Green Gumball", 0.15f),
                    new ItemLoot(item: "Yellow Gumball", 0.15f),
                    new ItemLoot(item: "Seal of the Enchanted Forest", 0.01f),
                    new TierLoot(tier: 7, type: LootType.Weapon, 0.25f),
                    new TierLoot(tier: 6, type: LootType.Weapon, 0.4f),
                    new TierLoot(tier: 8, type: LootType.Armor, 0.25f),
                    new TierLoot(tier: 7, type: LootType.Armor, 0.4f),
                    new TierLoot(tier: 3, type: LootType.Ability, 0.25f),
                    new TierLoot(tier: 4, type: LootType.Ability, 0.125f),
                    new TierLoot(tier: 3, type: LootType.Ring, 0.25f),
                    new TierLoot(tier: 4, type: LootType.Ring, 0.125f)
                )
            );
            db.Init("Desire Troll",
                    HPScale.BOSS_HP_SCALE_DEFAULT(),
                    new State("BaseAttack",
                        new Shoot(10, count: 3, shootAngle: 15, index: 0, predictive: 1, cooldown: 1400),
                        new Grenade(5, damage: 160, radius: 8, cooldown: 3000),
                        new Shoot(10, count: 1, index: 1, predictive: 1, cooldown: 2000),
                        new State("Choose",
                            new TimedRandomTransition(3800, "Run", "Attack")
                        ),
                        new State("Run",
                            new StayBack(speed: 1.1f, distance: 10, entity: null),
                            new TimedTransition(time: 1200, targetState: "Choose")
                        ),
                        new State("Attack",
                            new Charge(speed: 1.2f, range: 11, coolDown: 1000),
                            new TimedTransition(time: 1000, targetState: "Choose")
                        ),
                        new HealthTransition(threshold: 0.6f, targetState: "NextAttack")
                    ),
                    new State("NextAttack",
                        new Shoot(10, count: 5, shootAngle: 10, index: 2, predictive: 0.5f, angleOffset: 0.4f, cooldown: 2000),
                        new Shoot(10, count: 1, index: 1, predictive: 1, cooldown: 2000),
                        new Shoot(10, count: 3, shootAngle: 15, index: 0, predictive: 1, angleOffset: 1, cooldown: 4000),
                        new Grenade(8, damage: 200, cooldown: 3000),
                        new State("Choose2",
                            new TimedRandomTransition(3800, "Run2", "Attack2")
                        ),
                        new State("Run2",
                            new StayBack(speed: 1.5f, distance: 10, entity: null),
                            new TimedTransition(time: 1500, targetState: "Choose2"),
                            new PlayerWithinTransition(dist: 3.5f, targetState: "Boom", seeInvis: false)
                        ),
                        new State("Attack2",
                            new Charge(speed: 1.2f, range: 11, coolDown: 1000),
                            new TimedTransition(time: 1000, targetState: "Choose2"),
                            new PlayerWithinTransition(dist: 3.5f, targetState: "Boom", seeInvis: false)
                        ),
                        new State("Boom",
                            new Shoot(0, count: 20, shootAngle: 18, index: 3, cooldown: 2000),
                            new TimedTransition(time: 200, targetState: "Choose2")
                        )
                    ),
                    new StayCloseToSpawn(speed: 1.5f, range: 15),
                    new Prioritize(
                        new Follow(speed: 1, acquireRange: 11, range: 5),
                        new Wander(speed: 0.4f)
                    ),
                new Threshold(0.01f,
                    new ItemLoot(item: "Potion of Attack", 0.1f),
                    new ItemLoot(item: "Potion of Wisdom", 0.1f),
                    new ItemLoot(item: "Ring Pop", 0.02f),
                    new ItemLoot(item: "Rock Candy", 0.08f),
                    new ItemLoot(item: "Candy-Coated Armor", 0.01f),
                    new ItemLoot(item: "Red Gumball", 0.15f),
                    new ItemLoot(item: "Realm Equipment Crystal", 0.05f),
                    new ItemLoot(item: "Purple Gumball", 0.15f),
                    new ItemLoot(item: "Blue Gumball", 0.15f),
                    new ItemLoot(item: "Green Gumball", 0.15f),
                    new ItemLoot(item: "Yellow Gumball", 0.15f),
                    new TierLoot(tier: 7, type: LootType.Weapon, 0.25f),
                    new TierLoot(tier: 6, type: LootType.Weapon, 0.4f),
                    new TierLoot(tier: 8, type: LootType.Armor, 0.25f),
                    new TierLoot(tier: 7, type: LootType.Armor, 0.4f),
                    new TierLoot(tier: 3, type: LootType.Ability, 0.25f),
                    new TierLoot(tier: 4, type: LootType.Ability, 0.125f),
                    new TierLoot(tier: 3, type: LootType.Ring, 0.25f),
                    new TierLoot(tier: 4, type: LootType.Ring, 0.125f)
                )
            );
            db.Init("Swoll Fairy",
                    HPScale.BOSS_HP_SCALE_DEFAULT(),
                    new Spawn(children: "Fairy", maxChildren: 2, initialSpawn: 0, cooldown: 10000, givesNoXp: false),
                    new StayCloseToSpawn(speed: 0.6f, range: 13),
                    new Prioritize(
                        new Follow(speed: 0.3f, acquireRange: 10, range: 5)
                    ),
                    new Wander(speed: 0.25f),
                    new State("Shoot",
                        new Shoot(11, count: 2, shootAngle: 30, index: 0, predictive: 1, cooldown: 400),
                        new TimedTransition(time: 3000, targetState: "Pause")
                    ),
                    new State("ShootB",
                        new Shoot(11, count: 8, shootAngle: 45, index: 1, cooldown: 500),
                        new TimedTransition(time: 3000, targetState: "Pause")
                    ),
                    new State("Pause",
                        new TimedRandomTransition(1000, "Shoot", "ShootB")
                    ),
                new Threshold(0.01f,
                    new ItemLoot(item: "Potion of Defense", 0.1f),
                    new ItemLoot(item: "Potion of Wisdom", 0.1f),
                    new ItemLoot(item: "Ring Pop", 0.02f),
                    new ItemLoot(item: "Wine Cellar Incantation", 0.05f),
                    new ItemLoot(item: "Rock Candy", 0.08f),
                    new ItemLoot(item: "Candy-Coated Armor", 0.01f),
                    new ItemLoot(item: "Red Gumball", 0.15f),
                    new ItemLoot(item: "Purple Gumball", 0.15f),
                    new ItemLoot(item: "Blue Gumball", 0.15f),
                    new ItemLoot(item: "Green Gumball", 0.15f),
                    new ItemLoot(item: "Yellow Gumball", 0.15f),
                    new ItemLoot(item: "Realm Equipment Crystal", 0.05f),
                    new ItemLoot(item: "Fairy Plate", 0.01f),
                    new TierLoot(tier: 7, type: LootType.Weapon, 0.25f),
                    new TierLoot(tier: 6, type: LootType.Weapon, 0.4f),
                    new TierLoot(tier: 8, type: LootType.Armor, 0.25f),
                    new TierLoot(tier: 7, type: LootType.Armor, 0.4f),
                    new TierLoot(tier: 3, type: LootType.Ability, 0.25f),
                    new TierLoot(tier: 4, type: LootType.Ability, 0.125f),
                    new TierLoot(tier: 3, type: LootType.Ring, 0.25f),
                    new TierLoot(tier: 4, type: LootType.Ring, 0.125f)
                )
            );
            db.Init("Gigacorn",
                    HPScale.BOSS_HP_SCALE_DEFAULT(),
                    new StayCloseToSpawn(speed: 1, range: 13),
                    new Prioritize(
                        new Charge(speed: 1.4f, range: 24, coolDown: 3800),
                        new StayBack(speed: 0.8f, distance: 6, entity: null),
                        new Wander(speed: 0.4f)
                    ),
                    new State("Start",
                        new State("Shoot",
                            new Shoot(10, count: 1, index: 0, predictive: 1, cooldown: 200),
                            new TimedTransition(time: 2850, targetState: "ShootPause")
                        ),
                        new State("ShootPause",
                            new Shoot(10, count: 3, shootAngle: 10, index: 1, predictive: 0.4f, cooldown: 3000, cooldownOffset: 500),
                            new Shoot(10, count: 3, shootAngle: 10, index: 1, predictive: 0.4f, cooldown: 3000, cooldownOffset: 1000),
                            new Shoot(10, count: 3, shootAngle: 10, index: 1, predictive: 0.4f, cooldown: 3000, cooldownOffset: 1500),
                            new TimedTransition(time: 5700, targetState: "Shoot")
                        )
                    ),
                new Threshold(0.01f,
                    new ItemLoot(item: "Potion of Attack", 0.1f),
                    new ItemLoot(item: "Potion of Wisdom", 0.1f),
                    new ItemLoot(item: "Ring Pop", 0.02f),
                    new ItemLoot(item: "Rock Candy", 0.08f),
                    new ItemLoot(item: "Candy-Coated Armor", 0.01f),
                    new ItemLoot(item: "Red Gumball", 0.15f),
                    new ItemLoot(item: "Purple Gumball", 0.15f),
                    new ItemLoot(item: "Blue Gumball", 0.15f),
                    new ItemLoot(item: "Green Gumball", 0.15f),
                    new ItemLoot(item: "Yellow Gumball", 0.15f),
                    new ItemLoot(item: "Fairy Plate", 0.01f),
                    new ItemLoot(item: "Realm Equipment Crystal", 0.05f),
                    new TierLoot(tier: 7, type: LootType.Weapon, 0.25f),
                    new TierLoot(tier: 6, type: LootType.Weapon, 0.4f),
                    new TierLoot(tier: 8, type: LootType.Armor, 0.25f),
                    new TierLoot(tier: 7, type: LootType.Armor, 0.4f),
                    new TierLoot(tier: 3, type: LootType.Ability, 0.25f),
                    new TierLoot(tier: 4, type: LootType.Ability, 0.125f),
                    new TierLoot(tier: 3, type: LootType.Ring, 0.25f),
                    new TierLoot(tier: 4, type: LootType.Ring, 0.125f)
                )
            );
            db.Init("Candyland Boss Spawner",
                    new DropPortalOnDeath("Glowing Realm Portal", 100, 200),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("Ini",
                        new NoPlayerWithinTransition(dist: 8, targetState: "Ini2")
                    ),
                    new State("Ini2",
                        new TimedTransition("Creampuff", 1000)
                    ),
                    new State("Done",
                        new Taunt("Candyland Has Been Completed, all bosses have been killed. Use this portal to leave the Candyland."),
                        new TimedTransition("dead", 1000)
                         ),
                    new State("dead",
                        new Suicide()
                    ),
                    new State("Creampuff",
                        new Spawn(children: "Spoiled Creampuff", maxChildren: 1, initialSpawn: 0, givesNoXp: true),
                        new EntitiesNotExistsTransition(3000, "Unicorn", "Gumball Machine", "Swoll Fairy", "MegaRototo", "Desire Troll", "Gigacorn", "Spoiled Creampuff")
                    ),
                    new State("Unicorn",
                        new Spawn(children: "Gigacorn", maxChildren: 1, initialSpawn: 0, givesNoXp: true),
                        new EntitiesNotExistsTransition(3000, "Troll", "Gumball Machine", "Swoll Fairy", "MegaRototo", "Desire Troll", "Gigacorn", "Spoiled Creampuff")
                    ),
                    new State("Troll",
                        new Spawn(children: "Desire Troll", maxChildren: 1, initialSpawn: 0, givesNoXp: true),
                        new EntitiesNotExistsTransition(3000, "Rototo", "Gumball Machine", "Swoll Fairy", "MegaRototo", "Desire Troll", "Gigacorn", "Spoiled Creampuff")
                    ),
                    new State("Rototo",
                        new Spawn(children: "MegaRototo", maxChildren: 1, initialSpawn: 0, givesNoXp: true),
                       new EntitiesNotExistsTransition(3000, "Fairy", "Gumball Machine", "Swoll Fairy", "MegaRototo", "Desire Troll", "Gigacorn", "Spoiled Creampuff")
                    ),
                    new State("Fairy",
                        new Spawn(children: "Swoll Fairy", maxChildren: 1, initialSpawn: 0, givesNoXp: true),
                        new EntitiesNotExistsTransition(3000, "Gumball Machine", "Gumball Machine", "Swoll Fairy", "MegaRototo", "Desire Troll", "Gigacorn", "Spoiled Creampuff")
                    ),
                    new State("Gumball Machine",
                        new Spawn(children: "Gumball Machine", maxChildren: 1, initialSpawn: 5, givesNoXp: true),
                        new TimedTransition(time: 3000, targetState: "Done")
                    )
            );
            db.Init("Gumball Machine",
                new State("base",
                    new Threshold(0.01f,
                        new ItemLoot(item: "Ring Pop", 0.01f),
                        new ItemLoot(item: "Rock Candy", 0.15f),
                        new ItemLoot(item: "Red Gumball", 0.15f),
                        new ItemLoot(item: "Purple Gumball", 0.15f),
                        new ItemLoot(item: "Blue Gumball", 0.15f),
                        new ItemLoot(item: "Green Gumball", 0.15f),
                        new ItemLoot(item: "Yellow Gumball", 0.15f)
                    )
                )
            );
            
            db.Init("Tiny Rototo",
                    new Prioritize(
                        new Orbit(speed: 1.2f, 4, acquireRange: 10, target: "MegaRototo"),
                        new Protect(speed: 0.8f, protectee: "Rototo", acquireRange: 15, protectionRange: 7, reprotectRange: 6)
                    ),
                    new State("Main",
                        new TransitionFrom("Main", "Unaware"),
                        new State("Unaware",
                            new Prioritize(
                                new Orbit(speed: 0.4f, 2.6f, acquireRange: 8, target: "Rototo", speedVariance: 0.2f, radiusVariance: 0.2f, orbitClockwise: true),
                                new Wander(speed: 0.35f)
                            ),
                            new PlayerWithinTransition(dist: 3.4f, targetState: "Attack"),
                            new HealthTransition(threshold: 0.999f, targetState: "Attack")
                            ),
                        new State("Attack",
                            new Shoot(0, count: 4, shootAngle: 90, index: 1, defaultAngle: 45, cooldown: 1400),
                            new Shoot(0, count: 4, shootAngle: 90, index: 0, cooldown: 1400),
                            new Prioritize(
                                new Follow(speed: 0.8f, acquireRange: 8, range: 3, duration: 3000, cooldown: 2000),
                                new Charge(speed: 1.35f, range: 11, coolDown: 1000),
                                new Wander(speed: 0.35f)
                            )
                        )
                    )
            );
            db.Init("Butterfly",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new StayCloseToSpawn(speed: 0.3f, range: 6),
                    new State("Moving",
                        new Wander(speed: 0.25f),
                        new PlayerWithinTransition(dist: 6, targetState: "Follow")
                    ),
                    new State("Follow",
                        new Prioritize(
                            new StayBack(speed: 0.23f, distance: 1.2f, entity: null),
                            new Orbit(speed: 0.2f, 1.6f, acquireRange: 3)
                        ),
                        new Follow(speed: 0.2f, acquireRange: 7, range: 3),
                        new Wander(speed: 0.2f),
                        new NoPlayerWithinTransition(dist: 4, targetState: "Moving")
                    )
            );
            db.Init("Rototo",
                new Prioritize(
                        new Orbit(speed: 0.4f, 4, acquireRange: 10, target: "MegaRototo"),
                        new Protect(speed: 0.8f, protectee: "Rototo", acquireRange: 15, protectionRange: 7, reprotectRange: 6)
                    ),
                    new State("Main",
                        new TransitionFrom("Main", "Unaware"),
                        new State("Unaware",
                            new Prioritize(
                                new Orbit(speed: 0.4f, 2.6f, acquireRange: 8, target: "Rototo", speedVariance: 0.2f, radiusVariance: 0.2f, orbitClockwise: true),
                                new Wander(speed: 0.35f)
                            ),
                            new PlayerWithinTransition(dist: 3.4f, targetState: "Attack"),
                            new HealthTransition(threshold: 0.999f, targetState: "Attack")
                            ),
                        new State("Attack",
                            new Shoot(0, count: 4, shootAngle: 90, index: 1, defaultAngle: 45, cooldown: 1400),
                            new Shoot(0, count: 4, shootAngle: 90, index: 0, cooldown: 1400),
                            new Prioritize(
                                new Follow(speed: 0.3f, acquireRange: 8, range: 3, duration: 3000, cooldown: 2000),
                                new Charge(speed: 0.5f, range: 11, coolDown: 1000),
                                new Wander(speed: 0.35f)
                            )
                        )
                    )

            );
        }

    }
}
