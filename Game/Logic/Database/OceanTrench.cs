using RotMG.Common;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;

namespace RotMG.Game.Logic.Database
{
    public class OceanTrench : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {
            db.Init("Coral Gift",
                new State("Texture1",
                    new SetAltTexture(1),
                    new TimedTransition("Texture2", 500)
                ),
                new State("Texture2",
                    new SetAltTexture(2),
                    new TimedTransition("Texture0", 500)
                ),
                new State("Texture0",
                    new SetAltTexture(0),
                    new TimedTransition("Texture1", 500)
                ),
                new Threshold(0.01f,
                    new ItemLoot("Coral Juice", 0.8f),
                    new ItemLoot("Potion of Mana", 0.2f),
                    new ItemLoot("Wine Cellar Incantation", 0.02f)
                   /* new ItemLoot("Bronze Trinket", 0.005f),
                    new ItemLoot("Silver Trinket", 0.001f),
                    new ItemLoot("Gold Trinket", 0.0005f)*/
                ),
                new Threshold(0.02f,
                    new ItemLoot("Coral Bow", 0.025f),
                    new ItemLoot("Coral Venom Trap", 0.07f),
                    new ItemLoot("Coral Silk Armor", 0.05f)
                ),
                new Threshold(0.03f,
                    new ItemLoot("Coral Ring", 0.07f)
                    )
                /*new Threshold(0.05f,
                    new ItemLoot("Golden Touch of Thessal"),
                    new ItemLoot("Replica: Trident of the Sea"),
                    new ItemLoot("King Alexander's Treasure"),
                    new ItemLoot("Cane of the Deep Sea"),
                    new ItemLoot("Seaweed Sewn Raiment"),
                    new ItemLoot("Golden Heart of Thessal")
                    )*/
            );
            db.Init("Coral Bomb Big",
                new State("Spawning",
                    new TossObject("Coral Bomb Small", 1, 30, 500),
                    new TossObject("Coral Bomb Small", 1, 90, 500),
                    new TossObject("Coral Bomb Small", 1, 150, 500),
                    new TossObject("Coral Bomb Small", 1, 210, 500),
                    new TossObject("Coral Bomb Small", 1, 270, 500),
                    new TossObject("Coral Bomb Small", 1, 330, 500),
                    new TimedTransition("Attack", 500)
                ),
                new State("Attack",
                    new Shoot(4.4f, 5, fixedAngle: 0, shootAngle: 70),
                    new Suicide(0)
                )
            );
            db.Init("Coral Bomb Small",
                new Shoot(3.8f, 5, fixedAngle: 0, shootAngle: 70),
                new Suicide(0)
            );
            db.Init("Deep Sea Beast",
                new ChangeSize(11, 100),
                new Prioritize(
                    new StayCloseToSpawn(0.2f, 2),
                    new Follow(0.2f, 4, 1)
                ),
                new Shoot(1.8f, cooldown: 300),
                new Shoot(2.5f, 1, index: 1, cooldown: 1000),
                new Shoot(3.3f, 1, index: 2, cooldown: 1000),
                new Shoot(4.2f, 1, index: 3, cooldown: 1000)
            );
            db.Init("Thessal the Mermaid Goddess",
                HPScale.BOSS_HP_SCALE_DEFAULT(),
                new TransformOnDeath("Thessal the Mermaid Goddess Wounded", probability: 0.7f),
                new State("Start",
                    new Prioritize(
                        new Wander(0.3f),
                        new Follow(0.3f, 10, 2)
                    ),
                    new EntitiesNotExistsTransition(20, "Spawning Deep", "Deep Sea Beast"),
                    new HealthTransition(1, "Attack1")
                ),
                new State("Main",
                    new Prioritize(
                        new Wander(0.3f),
                        new Follow(0.3f, 10, 2)
                    ),
                    new TimedTransition("Attack1", 0)
                ),
                new State("Main 2",
                    new Prioritize(
                        new Wander(0.3f),
                        new Follow(0.3f, 10, 2)
                    ),
                    new TimedTransition("Attack2", 0)
                ),
                new State("Spawning Bomb",
                    new TossObject("Coral Bomb Big", angle: 45),
                    new TossObject("Coral Bomb Big", angle: 135),
                    new TossObject("Coral Bomb Big", angle: 225),
                    new TossObject("Coral Bomb Big", angle: 315),
                    new TimedTransition("Main", 1000)
                ),
                new State("Spawning Bomb Attack2",
                    new TossObject("Coral Bomb Big", angle: 45),
                    new TossObject("Coral Bomb Big", angle: 135),
                    new TossObject("Coral Bomb Big", angle: 225),
                    new TossObject("Coral Bomb Big", angle: 315),
                    new TimedTransition("Attack2", 1000)
                ),
                new State("Spawning Deep",
                    new TossObject("Deep Sea Beast", 14, 0),
                    new TossObject("Deep Sea Beast", 14, 90),
                    new TossObject("Deep Sea Beast", 14, 180),
                    new TossObject("Deep Sea Beast", 14, 270),
                    new TimedTransition("Start", 1000)
                ),
                new State("Attack1",
                    new HealthTransition(0.5f, "Attack2"),
                    //new TimedTransition(3000, "Trident", randomized: true),
                    new TimedRandomTransition(3000, "Thunder Swirl", "Super Trident", "Yellow Wall")
                ),
                new State("Thunder Swirl",
                    new Shoot(8.8f, 8, 360 / 8, cooldown: 200),
                    new TimedTransition("Thunder Swirl 2", 500)
                ),
                new State("Thunder Swirl 2",
                    new TossObject("Coral Bomb Big"),
                    new TimedTransition("Thunder Swirl 3", 500)
                ),
                new State("Thunder Swirl 3",
                    new Shoot(8.8f, 8, 360 / 8, cooldown: 200),
                    new TimedTransition("Main", 100)
                ),
                new State("Thunder Swirl Attack2",
                    new Shoot(8.8f, 16, 360 / 16, cooldown: 200),
                    new TimedTransition("Thunder Swirl 2 Attack2", 500)
                ),
                new State("Thunder Swirl 2 Attack2",
                    new TossObject("Coral Bomb Big"),
                    new TimedTransition("Thunder Swirl 3 Attack2", 500)
                ),
                new State("Thunder Swirl 3 Attack2",
                    new Shoot(8.8f, 16, 360 / 16, cooldown: 200),
                    new TimedTransition("Main 2", 100)
                ),
                //new State("Trident",
                //new Shoot(21, count: 8, shootAngle: 360 / 4, index: 1),
                //new TimedTransition(100, "Start")
                //),
                new State("Super Trident",
                    new Shoot(21, 2, 25, 2, angleOffset: 0, cooldown: 200),
                    new Shoot(21, 2, 25, 2, angleOffset: 90, cooldown: 200),
                    new Shoot(21, 2, 25, 2, angleOffset: 180, cooldown: 200),
                    new Shoot(21, 2, 25, 2, angleOffset: 270, cooldown: 200),
                    new TossObject("Coral Bomb Big"),
                    new TimedTransition("Super Trident 2", 250)
                ),
                new State("Super Trident 2",
                    new TossObject("Coral Bomb Big"),
                    new TimedTransition("Main", 100)
                ),
                new State("Super Trident Attack2",
                    new Shoot(21, 2, 25, 2, angleOffset: 0, cooldown: 200),
                    new Shoot(21, 2, 25, 2, angleOffset: 90, cooldown: 200),
                    new Shoot(21, 2, 25, 2, angleOffset: 180, cooldown: 200),
                    new Shoot(21, 2, 25, 2, angleOffset: 270, cooldown: 200),
                    new TossObject("Coral Bomb Big"),
                    new TimedTransition("Super Trident 2 Attack2", 250)
                ),
                new State("Super Trident 2 Attack2",
                    new TimedTransition("Super Trident 3 Attack2", 250)
                ),
                new State("Super Trident 3 Attack2",
                    new Shoot(21, 2, 25, 2, angleOffset: 0, cooldown: 200),
                    new Shoot(21, 2, 25, 2, angleOffset: 90, cooldown: 200),
                    new Shoot(21, 2, 25, 2, angleOffset: 180, cooldown: 200),
                    new Shoot(21, 2, 25, 2, angleOffset: 270, cooldown: 200),
                    new TossObject("Coral Bomb Big"),
                    new TimedTransition("Super Trident 4 Attack2", 250)
                ),
                new State("Super Trident 4 Attack2",
                    new TimedTransition("Main 2", 100)
                ),
                new State("Yellow Wall",
                    new Flash(0xFFFF00, .1f, 15),
                    new Prioritize(
                        new StayCloseToSpawn(0.3f, 1)
                    ),
                    new Shoot(18, 30, fixedAngle: 6, index: 3, cooldown: 300),
                    new TimedTransition("Yellow Wall 2", 500)
                ),
                new State("Yellow Wall 2",
                    new Flash(0xFFFF00, .1f, 15),
                    new Shoot(18, 30, fixedAngle: 6, index: 3, cooldown: 300),
                    new TimedTransition("Yellow Wall 3", 500)
                ),
                new State("Yellow Wall 3",
                    new Flash(0xFFFF00, .1f, 15),
                    new Shoot(18, 30, fixedAngle: 6, index: 3, cooldown: 300),
                    new TimedTransition("Main", 100)
                ),
                new State("Yellow Wall Attack2",
                    new Flash(0xFFFF00, .1f, 15),
                    new Prioritize(
                        new StayCloseToSpawn(0.3f, 1)
                    ),
                    new Shoot(18, 30, fixedAngle: 6, index: 3, cooldown: 300),
                    new TimedTransition("Yellow Wall 2 Attack2", 500)
                ),
                new State("Yellow Wall 2 Attack2",
                    new Flash(0xFFFF00, .1f, 15),
                    new Shoot(18, 30, fixedAngle: 6, index: 3, cooldown: 300),
                    new TimedTransition("Yellow Wall 3 Attack2", 500)
                ),
                new State("Yellow Wall 3 Attack2",
                    new Flash(0xFFFF00, .1f, 15),
                    new Shoot(18, 30, fixedAngle: 6, index: 3, cooldown: 300),
                    new TimedTransition("Main 2", 100)
                ),
                new State("Attack2",
                    //new TimedTransition(500, "Trident", randomized: true),
                    new TimedRandomTransition(500, "Spawning Bomb", "Thunder Swirl Attack2", "Super Trident Attack2", "Yellow Wall Attack2")
                ),
                new Threshold(0.01f,
                    new ItemLoot("Potion of Mana", 1f, min: 3)
                ),
                new Threshold(0.1f,
                    new ItemLoot("Coral Juice", 0.8f),
                    new ItemLoot("Coral Bow", 0.02f),
                    new ItemLoot("Rune of Elven Magic", 0.005f),
                    new ItemLoot("Coral Venom Trap", 0.01f),
                    new ItemLoot("Wine Cellar Incantation", 0.02f),
                    new ItemLoot("Coral Silk Armor", 0.05f),
                    new ItemLoot("Coral Ring", 0.05f)
                    /*new ItemLoot("Replica: Trident of the Sea", 0.00002f),
                    new ItemLoot("King Alexander's Treasure", 0.00002f),
                    new ItemLoot("Cane of the Deep Sea", 0.00002f),
                    new ItemLoot("Seaweed Sewn Raiment", 0.00002f),
                    new ItemLoot("Golden Heart of Thessal", 0.00002f),
                    new ItemLoot("Bronze Trinket", 0.2f),
                    new ItemLoot("Silver Trinket", 0.001f),
                    new ItemLoot("Gold Trinket", 0.0005f),
                    new ItemLoot("Campaign Point x25", 1),
                    new ItemLoot("Campaign Point x25", 1)*/
                )
            );
            /*db.Init("Thessal Dropper",
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new TransformOnDeath("Ocean Vent"),
                new State("Idle",
                    new EntitiesNotExistsTransition(100, "Thessal the Mermaid Goddess", "Suicide")
                ),
                new State("Suicide",
                    new Decay(0)
                )
            );*/
            /*db.Init("Coral Dropper",
                new State("Idle",
                    new EntitiesNotExistsTransition(100, "Thessal the Mermaid Goddess", "Suicide")
                ),
                new State("Suicide",
                    new TossObject("Coral Gift", 1, 45),
                    new Suicide(0)
                )
            );*/
            db.Init("Thessal the Mermaid Goddess Wounded",
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new Taunt("Danm, your alive?"),
                //new TimedTransition("Fail", 12000),
                new State("Texture1",
                    new SetAltTexture(1),
                    new TimedTransition("Prize", 250)
                ),
                new State("Texture2",
                    new SetAltTexture(0),
                    new TimedTransition("Texture1", 250)
                ),
                new State("Prize",
                    new Taunt("Thank you kind sailor."),
                    new TossObject("Coral Gift", 5, 45),
                    new TossObject("Coral Gift", 5, 135),
                    new TossObject("Coral Gift", 5, 235),
                    new TimedTransition("Suicide", 0)
                ),
                new State("Fail",
                    new Taunt("You speak LIES!!"),
                    new TimedTransition("Suicide", 0)
                ),
                new State("Suicide",
                    new Suicide(0)
                )
            );
            db.Init("Fishman Warrior",
                new State("Start",
                    new Prioritize(
                        new Follow(0.6f, 9, 2)
                    ),
                    new Orbit(0.6f, 5, 9),
                    new Shoot(9, 3, index: 0, shootAngle: 10, cooldown: 1000),
                    new Shoot(9, 6, fixedAngle: 0, index: 2, cooldown: 2000),
                    new NoPlayerWithinTransition(9, "Range Shoot")
                ),
                new State("Range Shoot",
                    new Prioritize(
                        new StayCloseToSpawn(0.2f, 3),
                        new Wander(0.3f)
                    ),
                    new Shoot(12, 1, index: 1, cooldownOffset: 250),
                    new PlayerWithinTransition(9, "Start")
                )
            );
            db.Init("Fishman",
                new Prioritize(
                    new Follow(0.7f, 9, 1)
                ),
                new Shoot(9, 1, index: 1, cooldown: 2000),
                new Shoot(9, 1, index: 0, cooldownOffset: 250, cooldown: 1000),
                new Shoot(9, 3, index: 0, shootAngle: 10, cooldownOffset: 500, cooldown: 700)
            );
            db.Init("Sea Mare",
                new Charge(1.0f, 8, 4000),
                new Wander(0.2f),
                new State("Shoot 1",
                    new Shoot(9, 3, index: 1, cooldown: 2000),
                    new TimedTransition( "Shoot 2", 5000)
                ),
                new State("Shoot 2",
                    new Shoot(10, 8, 45, 0, cooldownOffset: 500, cooldown: 2000),
                    new Shoot(10, 8, 45, angleOffset: 45, index: 0,
                        cooldownOffset: 1000, cooldown: 2000),
                    new Shoot(10, 8, 45, angleOffset: 135, index: 0,
                        cooldownOffset: 1500, cooldown: 2000),
                    new TimedTransition("Shoot 1", 3000)
                )
            );
            db.Init("Sea Horse",
                new Orbit(0.2f, 2, 10, "Sea Mare"),
                new Wander(0.2f),
                new State("Shoot 1",
                    new Shoot(9, 1, index: 0, cooldownOffset: 250, cooldown: 400),
                    new Shoot(9, 2, 5, 0, cooldownOffset: 500, cooldown: 800),
                    new Shoot(9, 3, 5, 0, cooldownOffset: 750, cooldown: 1200)
                )
            );
            db.Init("Giant Squid",
                new Shoot(10, 1, index: 0, cooldown: 1000),
                new Follow(0.4f, 12, 1),
                new State("Toss",
                    new TossObject("Ink Bubble"),
                    new TimedTransition("Toss 2", 200)
                ),
                new State("Toss 2",
                    new TossObject("Ink Bubble"),
                    new TimedTransition("Attack 1", 200)
                ),
                new State("Attack 1",
                    new Shoot(10, 4, 15, 1, cooldown: 250),
                    new TimedTransition("Toss", 10000)
                )
            );
            db.Init("Ink Bubble",
                new Shoot(10, 1, index: 0, cooldown: 100)
            );
            db.Init("Sea Slurp Home",
                new Spawn("Grey Sea Slurp", 8)
            );
            db.Init("Grey Sea Slurp",
                new StayCloseToSpawn(0.5f, 10),
                new State("Shoot and Move",
                    new Prioritize(
                        new Follow(0.3f, 10, 4),
                        new Wander(0.2f)
                    ),
                    new Shoot(8, 1, index: 0, cooldown: 300),
                    new TimedTransition("Wall Shoot", 900)
                ),
                new State("Wall Shoot",
                    new Shoot(8, 6, index: 1, fixedAngle: 2, cooldown: 750),
                    new TimedTransition("Shoot and Move", 1500)
                )
            );
        }
    }
}