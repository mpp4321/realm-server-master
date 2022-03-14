using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;
/*MaRSot#6469*/

namespace RotMG.Game.Logic.Database
{
    class Shatters
    {
        private _ LostHalls = () => Behav()
            db.Init("LH Marble Colossus",
                new State("base",
                    new DropPortalOnDeath("0xb024", 100, 0),
                    new State("A-P",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new SetAltTexture(1),
                        new Spawn("LH Marble Colossus Anchor", 1, 1, 2000000000),
                        new PlayerWithinTransition(3, "I")
                        ),
                    new State("I",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new Timed(4000, new SetAltTexture(2)),
                        new Timed(6000, new SetAltTexture(3)),
                        new Timed(8000, new SetAltTexture(4)),
                        new Timed(10000, new SetAltTexture(0), new Flash(0xffffff, 0.6f, 17)),
                        new TimedTransition("P-1", 16600)
                        ),
                    new State("P-1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 1000),
                        new ConditionalEffect(ConditionEffectIndex.Armored, true),
                        new Taunt("Look upon my mighty bulwark."),
                        new TossObject("LH Colossus Pillar Summoner", 10, 45, 2000000000, 200, true),
                        new TossObject("LH Colossus Pillar Summoner", 10, 135, 2000000000, 200, true),
                        new TossObject("LH Colossus Pillar Summoner", 10, 225, 2000000000, 200, true),
                        new TossObject("LH Colossus Pillar Summoner", 10, 315, 2000000000, 200, true),
                        new Shoot(0, 12, 30, 0, 15, defaultAngle: 0, cooldownOffset: 800, cooldown: 3200),
                        new TimedTransition("P-1-01", 1000)
                        ),
                    new State("P-1-01",
                        new ConditionalEffect(ConditionEffectIndex.Armored, true),
                        new Shoot(0, 12, 30, 0, fixedAngle: 15, cooldownOffset: 1400, cooldown: 3200),
                        new Shoot(0, 12, 30, 0, fixedAngle: 0, cooldownOffset: 3000, cooldown: 3200),
                        new HealthTransition(0.95f, "P-2")
                        ),
                    new State("P-2",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 2000),
                        new Taunt("You doubt my strength? FATUUS! I will destroy you!"),
                        new Follow(0.5f, 15, 2),
                        new Shoot(0, 8, 45, 2, cooldownOffset: 1200, cooldown: 3000),
                        new Shoot(0, 5, 3, 1, cooldownOffset: 1200, cooldown: 1600),
                        new Shoot(0, 5, 3, 1, angleOffset: 90, cooldownOffset: 1200, cooldown: 1600),
                        new Shoot(0, 5, 3, 1, angleOffset: 180, cooldownOffset: 1200, cooldown: 1600),
                        new Shoot(0, 5, 3, 1, angleOffset: 270, cooldownOffset: 1200, cooldown: 1600),
                        new TimedTransition("P-2-01", 2000)
                        ),
                    new State("P-2-01",
                        new Follow(0.5f, 15, 2),
                        new Shoot(0, 8, 45, 2, cooldownOffset: 1800, cooldown: 3000),
                        new Shoot(0, 5, 3, 1, cooldownOffset: 400, cooldown: 1600),
                        new Shoot(0, 5, 3, 1, angleOffset: 90, cooldownOffset: 400, cooldown: 1600),
                        new Shoot(0, 5, 3, 1, angleOffset: 180, cooldownOffset: 400, cooldown: 1600),
                        new Shoot(0, 5, 3, 1, angleOffset: 270, cooldownOffset: 400, cooldown: 1600),
                        new HealthTransition(0.85f, "PR-3")
                        ),
                    new State("PR-3",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new ConditionalEffect(ConditionEffectIndex.Armored, true),
                        new ReturnToSpawn(0.6f, 0),
                        new TimedTransition(2000,"P-3")
                        ),
                    new State("P-3",
                        new ConditionalEffect(ConditionEffectIndex.Armored, true),
                        new Taunt("I cast you off!"),
                        new Spawn("LH Colossus Rock 1", 1, 1, 5000),
                        new Spawn("LH Colossus Rock 2", 1, 1, 5000),
                        new Spawn("LH Colossus Rock 3", 1, 1, 5000),
                        new Shoot(0, 12, 7, 1, fixedAngle: 0, cooldown: 5600),
                        new Shoot(0, 12, 7, 1, fixedAngle: 90, cooldownOffset: 1400, cooldown: 5600),
                        new Shoot(0, 12, 7, 1, fixedAngle: 180, cooldownOffset: 2600, cooldown: 5600),
                        new Shoot(0, 12, 7, 1, fixedAngle: 270, cooldownOffset: 4000, cooldown: 5600),
                        new HealthTransition(0.80f, "P-4")
                        ),
                    new State("P-4",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 2000),
                        new ConditionalEffect(ConditionEffectIndex.Armored, true),
                        new Taunt("Your fervent attacks are no match for my strength! BEGONE!"),
                        new Spawn("LH Marble Colossus Laser A", 1, 1, 2000000000),
                        new Shoot(0, 3, 30, 5, cooldownOffset: 1400, cooldown: 2800),
                        new Shoot(0, 16, 22.5f, 4, -10, cooldownOffset: 1400, cooldown: 2400),
                        new TimedTransition("P-4-01", 2000)
                        ),
                    new State("P-4-01",
                        new ConditionalEffect(ConditionEffectIndex.Armored, true),
                        new Shoot(0, 3, 30, 5, cooldownOffset: 2200, cooldown: 2800),
                        new Shoot(0, 16, 22.5f, 4, -10, cooldownOffset: 1800, cooldown: 2400),
                        new HealthTransition(0.75f, "P-5")
                        ),
                    new State("P-5",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 2000),
                        new Taunt("INSOLENTIA! Darkness will consume you"),
                        new Orbit(0.6f, 4, 10, "LH Marble Colossus Anchor", 0.5f),
                        new Timed(1400, new Grenade(5, 120, 10, cooldown: 2000)),
                        new Shoot(0, 1, 6, cooldownOffset: 1200, cooldown: 2000),
                        new Shoot(0, 2, 180, 7, cooldownOffset: 1200, cooldown: 400)
                        ),
                    new State("P-5-01",
                        new Orbit(0.6f, 4, 10, "LH Marble Colossus Anchor", 0.5f),
                        new Grenade(5, 120, 10, cooldown: 2000),
                        new Shoot(0, 1, 6, cooldownOffset: 1200, cooldown: 2000),
                        new Shoot(0, 2, 180, 7, cooldown: 400),
                        new HealthTransition(0.70f, "PR-6")
                        ),
                    new State("PR-6",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Taunt("Brace for your demise!"),
                        new ReturnToSpawn(0.6f, 0),
                        new TimedTransition("P-6", 2000)
                        ),
                    new State("P-6",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 600),
                        new ConditionalEffect(ConditionEffectIndex.Armored, true),
                        new Spawn("LH Colossus Rock 4", 1, 1, 5000),
                        new Spawn("LH Colossus Rock 5", 1, 1, 5000),
                        new Spawn("LH Colossus Rock 6", 1, 1, 5000),
                        new Spawn("LH Colossus Rock 7", 1, 1, 5000),
                        new Shoot(0, 4, 90, 8, rotateAngle: 9, defaultAngle: 0, cooldown: 400)
                        ),
                    new State("P-6-01",
                        new Shoot(0, 4, 90, 8, rotateAngle: 9, defaultAngle: 0, cooldown: 400),
                        new HealthTransition(0.60f, "P-7")
                        ),
                    new State("P-7",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new Taunt("Futility!"),
                        new Flash(0x0000ff, 0.3f, 6000),
                        new Spawn("LH Marble Core 1", 1, 1, 2000000000),
                        new Spawn("LH Colossus Rock 8", 1, 1, 4000),
                        new Spawn("LH Colossus Rock 9", 1, 1, 4000),
                        new Shoot(0, 36, 10, 10, 0, cooldownOffset: 1800, cooldown: 2000),
                        new Shoot(0, 36, 10, 9, 5, cooldownOffset: 1800, cooldown: 2600),
                        new EntitiesNotExistsTransition(20, "S-1", "LH Marble Core 1")
                        ),
                    new State("S-1",
                        new Follow(0.4f, 15, 2),
                        new Spawn("LH Colossus Rock 10", 1, 1, 2000000000),
                        new State("P-8",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 2000),
                            new Taunt("Call of voice, for naught. Plea of mercy, for naught. None may enter this chamber and live!"),
                            new Shoot(0, 16, 22.5f, 11, defaultAngle: 0, cooldownOffset: 1200, cooldown: 4000),
                            new HealthTransition(0.55f, "P-8-1")
                            ),
                        new State("P-8-1",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 2000),
                            new Taunt("SANGUIS! OSSE! CARO! Feel it rend from your body!"),
                            new Order(3, "LH Colossus Colossus Rock 10", "R-TR-2"),
                            new Shoot(0, 4, index: 6, cooldownOffset: 1200, cooldown: 1600),
                            new Shoot(0, 16, 22.5f, 11, cooldownOffset: 1200, cooldown: 4000),
                            new HealthTransition(0.50f, "P-8-2")
                            ),
                        new State("P-8-2",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 2000),
                            new Taunt("PESTIS! The darkness consumes!"),
                            new Shoot(0, 3, 30, 12, cooldownOffset: 1200, cooldown: 2000),
                            new Shoot(0, 4, index: 6, cooldownOffset: 2000, cooldown: 1600),
                            new Shoot(0, 16, 22.5f, 11, cooldownOffset: 1200, cooldown: 4000),
                            new HealthTransition(0.45f, "PR-9")
                            )
                        ),
                    new State("PR-9",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new Taunt("Enough! Pillars, serve your purpose!"),
                        new ReturnToSpawn(0.6f, 0),
                        new Order(15, "LH Colossus Pillar", "PL-S"),
                        new Order(15, "LH Colossus Pillar Summoner", "S-P-2-A"),
                        new TimedTransition("P-9", 2000)
                        ),
                    new State("P-9",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 2000),
                        new Orbit(0.6f, 8, 20, "LH Marble Colossus Anchor", 0.5f),
                        new Spawn("LH Colossus Rock 11", 1, 1, 2000000000),
                        new Duration(new Shoot(0, 2, 16, 13, cooldownOffset: 3000, cooldown: 1200), 5400),
                        new Duration(new Shoot(0, 3, 16, 13, cooldownOffset: 4200, cooldown: 1200), 6600),
                        new Shoot(0, 4, 16, 13, cooldownOffset: 6600, cooldown: 1200),
                        new Shoot(0, 5, 16, 13, cooldownOffset: 7800, cooldown: 1200),
                        new HealthTransition(0.40f, "S-2")
                        ),
                    new State("S-2",
                        new Flash(0x0000ff, 0.3f, 6000),
                        new State("PR-10",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                            new Taunt("Perish, blights upon this realm!"),
                            new Order(15, "LH Colossus Rock 11", "R-TR-4"),
                            new Spawn("LH Marble Core 2", 1, 1, 2000000000),
                            new Spawn("LH Marble Core 3", 1, 1, 2000000000),
                            new Spawn("LH Colossus Rock 12", 1, 1, 2000000000),
                            new ReturnToSpawn(0.6f, 0),
                            new TimedTransition("P-10", 2000)
                            ),
                        new State("P-10",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                            new Shoot(0, 3, 27, 13, cooldown: 2800),
                            new Shoot(0, 36, 10, 10, 0, cooldownOffset: 400, cooldown: 1600),
                            new EntitiesNotExistsTransition(30, "P-11", "LH Marble Core 2", "LH Marble Core 3")
                            )
                        ),
                    new State("P-11",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 2000),
                        new ConditionalEffect(ConditionEffectIndex.Armored, true),
                        new Taunt("You have seen your last glimpse of sunlight!"),
                        new Order(15, "LH Colossus Rock 11", "R-TR-4"),
                        new Order(15, "LH Colossus Rock 12", "R-TR-4"),
                        new Shoot(0, 3, 120, 3, rotateAngle: 3, defaultAngle: 0, cooldownOffset: 200, cooldown: 200),
                        new HealthTransition(0.35f, "S-3")
                        ),
                     new State("S-3",
                        new Flash(0x0000ff, 0.3f, 6000),
                        new State("PR-12",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                            new Taunt("PATI! The prohibited arts allow untold power!"),
                            new Spawn("LH Marble Core 4", 1, 0, 2000000000),
                            new Spawn("LH Marble Core 5", 1, 0, 2000000000),
                            new TimedTransition("P-12", 2000)
                            ),
                        new State("P-12",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Timed(3600, new Charge(4, 12, 4800)),
                            new Shoot(0, 16, 22.5f, 20, 0, cooldownOffset: 1600, cooldown: 4800),
                            new Shoot(0, 16, 22.5f, 20, 10, cooldownOffset: 2000, cooldown: 4800),
                            new Shoot(0, 16, 22.5f, 20, 0, cooldownOffset: 2400, cooldown: 4800),
                            new Shoot(0, 3, 27, 13, predictive: 0.1f, cooldownOffset: 2600, cooldown: 4800),
                            new Shoot(0, 3, 27, 13, predictive: 0.1f, cooldownOffset: 3800, cooldown: 4800),
                            new Shoot(0, 3, 27, 13, predictive: 0.1f, cooldownOffset: 4800, cooldown: 4800),
                            new EntitiesNotExistsTransition(20, "PR-13", "LH Marble Core 5")
                            )
                        ),
                    new State("PR-13",
                        new Taunt("It is my duty to protect these catacombs! You dare threaten my purpose?"),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new ConditionalEffect(ConditionEffectIndex.Armored, true),
                        new ReturnToSpawn(0.6f, 0),
                        new TimedTransition("P-13", 2000)
                        ),
                    new State("P-13",
                        new ConditionalEffect(ConditionEffectIndex.Armored, true),
                        new Shoot(0, 4, index: 6, cooldown: 5000),
                        new Shoot(0, 9, 40, 15, rotateAngle: 20, defaultAngle: 180, cooldown: 3200),
                        new Shoot(0, 9, 40, 15, rotateAngle: 20, defaultAngle: 175, cooldownOffset: 800, cooldown: 3200),
                        new Shoot(0, 9, 40, 15, rotateAngle: 20, defaultAngle: 185, cooldownOffset: 800, cooldown: 3200),
                        new Shoot(0, 18, 20, 15, rotateAngle: 20, defaultAngle: 180, cooldownOffset: 1600, cooldown: 3200),
                        new Shoot(0, 9, 40, 15, rotateAngle: 20, defaultAngle: 355, cooldownOffset: 2400, cooldown: 3200),
                        new Shoot(0, 9, 40, 15, rotateAngle: 20, defaultAngle: 5, cooldownOffset: 2400, cooldown: 3200),
                        new HealthTransition(0.25f, "P-14")
                        ),
                    new State("P-14",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 2000),
                        new ConditionalEffect(ConditionEffectIndex.Armored, true),
                        new Taunt("Magia saps from my body… My immense physical strength STILL REMAINS!"),
                        new Spawn("LH Colossus Rock 1", 1, 1, 5000),
                        new Spawn("LH Colossus Rock 2", 1, 1, 5000),
                        new Spawn("LH Colossus Rock 3", 1, 1, 5000),
                        new Shoot(0, 3, 27, 13, cooldown: 5000),
                        new HealthTransition(0.20f, "P-15")
                        ),
                    new State("P-15",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 2000),
                        new ConditionalEffect(ConditionEffectIndex.Armored, true),
                        new Taunt("Fear the halls!"),
                        new Spawn("LH Colossus Rock 4", 2, 1, 5000),
                        new Spawn("LH Colossus Rock 5", 2, 1, 5000),
                        new Spawn("LH Colossus Rock 6", 2, 1, 5000),
                        new Spawn("LH Colossus Rock 7", 2, 1, 5000),
                        new Shoot(0, 3, 27, 13, cooldownOffset: 800, cooldown: 5000),
                        new Shoot(0, 36, 10, 9, 0, cooldownOffset: 800, cooldown: 5600),
                        new HealthTransition(0.15f, "P-16")
                        ),
                    new State("P-16",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 2000),
                        new ConditionalEffect(ConditionEffectIndex.Armored, true),
                        new Taunt("I… I am… Dying…"),
                        new Spawn("LH Colossus Rock 8", 2, 1, 5000),
                        new Spawn("LH Colossus Rock 9", 2, 1, 5000),
                        new Spawn("LH Colossus Rock 13", 1, 0, 2000000000),
                        new Shoot(0, 3, 27, 13, cooldownOffset: 800, cooldown: 5000),
                        new Shoot(0, 36, 10, 9, 0, cooldownOffset: 800, cooldown: 5600),
                        new HealthTransition(0.10f, "P-17")
                        ),
                    new State("P-17",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 2000),
                        new ConditionalEffect(ConditionEffectIndex.Armored, true),
                        new Taunt("You… YOU WILL COME WITH ME!"),
                        new Flash(0x0000ff, 0.3f, 6000),
                        new Order(5, "LH Marble Colossus Anchor", "A-S"),
                        new Spawn("LH Marble Core 6", 1, 0, 2000000000),
                        new Spawn("LH Marble Core 7", 1, 0, 2000000000),
                        new Spawn("LH Marble Core 8", 1, 0, 2000000000),
                        new Spawn("LH Colossus Rock 14", 1, 0, 2000000000),
                        new Spawn("LH Marble Colossus Laser B", 1, 0, 2000000000),
                        new Shoot(0, 3, 27, 13, cooldownOffset: 800, cooldown: 5000),
                        new Shoot(0, 36, 10, 9, 0, cooldownOffset: 800, cooldown: 5600),
                        new EntitiesNotExistsTransition(20, "P-18", "LH Marble Core 6", "LH Marble Core 7", "LH Marble Core 8")
                        ),
                    new State("P-18",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new Taunt("No, I cannot…"),
                        new SetAltTexture(4),
                        new Duration(new Shoot(0, 16, 22.5f, 20, rotateAngle: 18, defaultAngle: 0, cooldownOffset: 200, cooldown: 400), 3800),
                        
                        new Timed(3800, new SetAltTexture(3), new Duration(new Shoot(0, 12, 7, 1, rotateAngle: 90, defaultAngle: 0, cooldownOffset: 3800, cooldown: 400), 3800)),
                        new Timed(7600, new SetAltTexture(2), new Duration(new Shoot(0, 4, 90, 8, rotateAngle: 9, defaultAngle: 0, cooldownOffset: 7600, cooldown: 400), 3800)),
                        new Timed(10600, new SetAltTexture(1), new HealSelf(10000, 5000000)),
                        new Timed(15600, new SetAltTexture(2), new Taunt("...!")),
                        new Timed(17200, new SetAltTexture(3)),
                        new Timed(18800, new SetAltTexture(4)),
                        new TimedTransition("P-19", 22600)
                        ),
                    new State("P-19",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 2000),
                        new SetAltTexture(0),
                        new Taunt("I CANNOT FAIL MY PURPOSE!"),
                        new Timed(800, new Follow(0.5f, 15, 2)),
                        new Order(15, "LH Colossus Pillar Summoner", "S-P-3-A"),
                        new Shoot(0, 8, 5, 4, cooldownOffset: 800, cooldown: 1000),
                        new Shoot(0, 8, 5, 4, angleOffset: 90, cooldownOffset: 800, cooldown: 1000),
                        new Shoot(0, 8, 5, 4, angleOffset: 180, cooldownOffset: 800, cooldown: 1000),
                        new Shoot(0, 8, 5, 4, angleOffset: 270, cooldownOffset: 800, cooldown: 1000),
                        new HealthTransition(0.80f, "C-1")
                        ),
                    new State("C-1",
                        new SetAltTexture(5),
                        new TimedRandomTransition(0, false, "P-20-A", "PR-20-B", "P-20-C")
                        ),
                    new State("P-20-A",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 2000),
                        new Timed(800, new Follow(0.5f, 15, 2)),
                        new Shoot(0, 8, 5, 23, cooldownOffset: 800, cooldown: 1000),
                        new Shoot(0, 8, 5, 23, angleOffset: 90, cooldownOffset: 800, cooldown: 1000),
                        new Shoot(0, 8, 5, 23, angleOffset: 180, cooldownOffset: 800, cooldown: 1000),
                        new Shoot(0, 8, 5, 23, angleOffset: 270, cooldownOffset: 800, cooldown: 1000),
                        new HealthTransition(0.60f, "C-2"),
                        new TimedRandomTransition(12000, false, "PR-20-B", "P-20-C")
                        ),
                    new State("PR-20-B",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new ReturnToSpawn(0.6f, 0),
                        new NotMovingTransition("P-20-B", 200)
                        ),
                    new State("P-20-B",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 1000),
                        new ConditionalEffect(ConditionEffectIndex.Armored, true),
                        new Shoot(0, 9, 40, 22, rotateAngle: 20, defaultAngle: 180, cooldown: 6400),
                        new Shoot(0, 9, 40, 22, rotateAngle: 20, defaultAngle: 175, cooldownOffset: 800, cooldown: 6400),
                        new Shoot(0, 9, 40, 22, rotateAngle: 20, defaultAngle: 185, cooldownOffset: 800, cooldown: 6400),
                        new Shoot(0, 18, 20, 22, rotateAngle: 20, defaultAngle: 180, cooldownOffset: 1600, cooldown: 6400),
                        new Shoot(0, 9, 40, 22, rotateAngle: 20, defaultAngle: 355, cooldownOffset: 2400, cooldown: 6400),
                        new Shoot(0, 9, 40, 22, rotateAngle: 20, defaultAngle: 5, cooldownOffset: 2400, cooldown: 6400),
                        new HealthTransition(0.60f, "C-2"),
                        new TimedRandomTransition(12800, false, "P-20-A", "P-20-C")
                        ),
                    new State("P-20-C",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 2000),
                        new Duration(new Orbit(0.6f, 10, 3, "LH Marble Colossus Anchor", 0.5f, 0.5f), 1400),
                        new Timed(1400, new Duration(new Wander(0.5f), 1400)),
                        new Timed(2800, new Duration(new Orbit(0.6f, 10, 6, "LH Marble Colossus Anchor", 0.5f, 0.5f), 1400)),
                        new Timed(4200, new Duration(new Wander(0.5f), 1400)),
                        new Timed(5600, new ReturnToSpawn(0.5f, 0)),
                        new Shoot(0, 16, 22.5f, 21, cooldown: 2000),
                        new HealthTransition(0.60f, "C-2"),
                        new NotMovingTransition("C-1")
                        ),
                    new State("C-2",
                        new SetAltTexture(6),
                        new Order(3, "LH Colossus Pillar Summoner", "S-P-4-A"),
                        new TimedRandomTransition(0, false, "P-21-A", "PR-21-B", "P-21-C")
                        ),
                    new State("P-21-A",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 2000),
                        new Timed(800, new Follow(0.5f, 15, 2)),
                        new Shoot(0, 8, 5, 23, cooldownOffset: 800, cooldown: 1000),
                        new Shoot(0, 8, 5, 23, angleOffset: 90, cooldownOffset: 800, cooldown: 1000),
                        new Shoot(0, 8, 5, 23, angleOffset: 180, cooldownOffset: 800, cooldown: 1000),
                        new Shoot(0, 8, 5, 23, angleOffset: 270, cooldownOffset: 800, cooldown: 1000),
                        new HealthTransition(0.40f, "C-3"),
                        new TimedRandomTransition(12000, false, "PR-21-B", "P-21-C")
                        ),
                    new State("PR-21-B",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new ReturnToSpawn(0.6f, 0),
                        new NotMovingTransition("P-21-B", 200)
                        ),
                    new State("P-21-B",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 1000),
                        new ConditionalEffect(ConditionEffectIndex.Armored, true),
                        new Shoot(0, 9, 40, 22, rotateAngle: 20, defaultAngle: 180, cooldown: 6400),
                        new Shoot(0, 9, 40, 22, rotateAngle: 20, defaultAngle: 175, cooldownOffset: 800, cooldown: 6400),
                        new Shoot(0, 9, 40, 22, rotateAngle: 20, defaultAngle: 185, cooldownOffset: 800, cooldown: 6400),
                        new Shoot(0, 18, 20, 22, rotateAngle: 20, defaultAngle: 180, cooldownOffset: 1600, cooldown: 6400),
                        new Shoot(0, 9, 40, 22, rotateAngle: 20, defaultAngle: 355, cooldownOffset: 2400, cooldown: 6400),
                        new Shoot(0, 9, 40, 22, rotateAngle: 20, defaultAngle: 5, cooldownOffset: 2400, cooldown: 6400),
                        new HealthTransition(0.40f, "C-3"),
                        new TimedRandomTransition(12800, false, "P-21-A", "P-21-C")
                        ),
                    new State("P-21-C",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 2000),
                        new Duration(new Orbit(0.6f, 10, 3, "LH Marble Colossus Anchor", 0.5f, 0.5f), 1400),
                        new Timed(1400, new Duration(new Wander(0.5f), 1400)),
                        new Timed(2800, new Duration(new Orbit(0.6f, 10, 6, "LH Marble Colossus Anchor", 0.5f, 0.5f), 1400)),
                        new Timed(4200, new Duration(new Wander(0.5f), 1400)),
                        new Timed(5600, new ReturnToSpawn(0.5f, 0)),
                        new Shoot(0, 16, 22.5f, 21, cooldown: 2000),
                        new HealthTransition(0.40f, "C-3"),
                        new NotMovingTransition("C-2")
                        ),
                    new State("C-3",
                        new SetAltTexture(7),
                        new TimedRandomTransition(0, false, "P-22-A", "PR-22-B", "P-22-C")
                        ),
                    new State("P-22-A",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 2000),
                        new Timed(800, new Follow(0.5f, 15, 2)),
                        new Shoot(0, 8, 5, 23, cooldownOffset: 800, cooldown: 1000),
                        new Shoot(0, 8, 5, 23, angleOffset: 90, cooldownOffset: 800, cooldown: 1000),
                        new Shoot(0, 8, 5, 23, angleOffset: 180, cooldownOffset: 800, cooldown: 1000),
                        new Shoot(0, 8, 5, 23, angleOffset: 270, cooldownOffset: 800, cooldown: 1000),
                        new HealthTransition(0.20f, "C-4"),
                        new TimedRandomTransition(12000, false, "PR-22-B", "P-22-C")
                        ),
                    new State("PR-22-B",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new ReturnToSpawn(0.6f, 0),
                        new NotMovingTransition("P-22-B", 200)
                        ),
                    new State("P-22-B",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 1000),
                        new ConditionalEffect(ConditionEffectIndex.Armored, true),
                        new Shoot(0, 9, 40, 22, rotateAngle: 20, defaultAngle: 180, cooldown: 6400),
                        new Shoot(0, 9, 40, 22, rotateAngle: 20, defaultAngle: 175, cooldownOffset: 800, cooldown: 6400),
                        new Shoot(0, 9, 40, 22, rotateAngle: 20, defaultAngle: 185, cooldownOffset: 800, cooldown: 6400),
                        new Shoot(0, 18, 20, 22, rotateAngle: 20, defaultAngle: 180, cooldownOffset: 1600, cooldown: 6400),
                        new Shoot(0, 9, 40, 22, rotateAngle: 20, defaultAngle: 355, cooldownOffset: 2400, cooldown: 6400),
                        new Shoot(0, 9, 40, 22, rotateAngle: 20, defaultAngle: 5, cooldownOffset: 2400, cooldown: 6400),
                        new HealthTransition(0.20f, "C-4"),
                        new TimedRandomTransition(12800, false, "P-22-A", "P-22-C")
                        ),
                    new State("P-22-C",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 2000),
                        new Duration(new Orbit(0.6f, 10, 3, "LH Marble Colossus Anchor", 0.5f, 0.5f), 1400),
                        new Timed(1400, new Duration(new Wander(0.5f), 1400)),
                        new Timed(2800, new Duration(new Orbit(0.6f, 10, 6, "LH Marble Colossus Anchor", 0.5f, 0.5f), 1400)),
                        new Timed(4200, new Duration(new Wander(0.5f), 1400)),
                        new Timed(5600, new ReturnToSpawn(0.5f, 0)),
                        new Shoot(0, 16, 22.5f, 21, cooldown: 2000),
                        new HealthTransition(0.20f, "C-4"),
                        new NotMovingTransition("C-3")
                        ),
                    new State("C-4",
                        new SetAltTexture(8),
                        new TimedRandomTransition(0, false, "P-23-A", "PR-23-B", "P-23-C")
                        ),
                    new State("P-23-A",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 2000),
                        new Timed(800, new Follow(0.5f, 15, 2)),
                        new Shoot(0, 8, 5, 23, cooldownOffset: 800, cooldown: 1000),
                        new Shoot(0, 8, 5, 23, angleOffset: 90, cooldownOffset: 800, cooldown: 1000),
                        new Shoot(0, 8, 5, 23, angleOffset: 180, cooldownOffset: 800, cooldown: 1000),
                        new Shoot(0, 8, 5, 23, angleOffset: 270, cooldownOffset: 800, cooldown: 1000),
                        new HealthTransition(0.05f, "PR-D"),
                        new TimedRandomTransition(12000, false, "PR-23-B", "P-23-C")
                        ),
                    new State("PR-23-B",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new ReturnToSpawn(0.6f, 0),
                        new NotMovingTransition("P-23-B", 200)
                        ),
                    new State("P-23-B",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 1000),
                        new ConditionalEffect(ConditionEffectIndex.Armored, true),
                        new Shoot(0, 9, 40, 22, rotateAngle: 20, defaultAngle: 180, cooldown: 6400),
                        new Shoot(0, 9, 40, 22, rotateAngle: 20, defaultAngle: 175, cooldownOffset: 800, cooldown: 6400),
                        new Shoot(0, 9, 40, 22, rotateAngle: 20, defaultAngle: 185, cooldownOffset: 800, cooldown: 6400),
                        new Shoot(0, 18, 20, 22, rotateAngle: 20, defaultAngle: 180, cooldownOffset: 1600, cooldown: 6400),
                        new Shoot(0, 9, 40, 22, rotateAngle: 20, defaultAngle: 355, cooldownOffset: 2400, cooldown: 6400),
                        new Shoot(0, 9, 40, 22, rotateAngle: 20, defaultAngle: 5, cooldownOffset: 2400, cooldown: 6400),
                        new HealthTransition(0.05f, "PR-D"),
                        new TimedRandomTransition(12800, false, "P-23-A", "P-23-C")
                        ),
                    new State("P-23-C",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 2000),
                        new Duration(new Orbit(0.6f, 10, 3, "LH Marble Colossus Anchor", 0.5f, 0.5f), 1400),
                        new Timed(1400, new Duration(new Wander(0.5f), 1400)),
                        new Timed(2800, new Duration(new Orbit(0.6f, 10, 6, "LH Marble Colossus Anchor", 0.5f, 0.5f), 1400)),
                        new Timed(4200, new Duration(new Wander(0.5f), 1400)),
                        new Timed(5600, new ReturnToSpawn(0.5f, 0)),
                        new Shoot(0, 16, 22.5f, 21, cooldown: 2000),
                        new HealthTransition(0.05f, "PR-D"),
                        new NotMovingTransition("C-4")
                        ),
                    new State("PR-D",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new ReturnToSpawn(1.5f, 0),
                        new TimedTransition("C-4", 200)
                        ),
                    new State("P-D",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new Timed(1000, new Taunt("I feel myself... Slipping... Into the void... It is so... Dark...")),
                        new Flash(0x4B0082, 1, 10),
                        new SetAltTexture(8),
                        new Spawn("LH Void Rift Spawner", 1, 1, 2000000000),
                        new Shoot(0, 45, 8, 16, angleOffset: 90, cooldownOffset: 7400, cooldown: 200),
                        new Shoot(0, 45, 8, 17, angleOffset: 90, cooldownOffset: 7400, cooldown: 200),
                        new Shoot(0, 30, 12, 18, angleOffset: 90, cooldownOffset: 7400, cooldown: 200),
                        new Decay(7400)
                        )
                    ),
                    new Threshold(0.001f,
                        new ItemLoot("Potion of Health", 1),
                        new ItemLoot("Potion of Mana", 1),
                        new ItemLoot("Potion of Health", 0.25f),
                        new ItemLoot("Potion of Mana", 0.25f),
                        new ItemLoot("Potion of Attack", 1),
                        new ItemLoot("Potion of Defense", 1),
                        new TierLoot(12, LootType.Weapon, 0.4f),
                        new TierLoot(13, LootType.Weapon, 0.2f),
                        new TierLoot(14, LootType.Weapon, 0.05f),
                        new TierLoot(6, LootType.Ability, 0.2f),
                        new TierLoot(13, LootType.Armor, 0.2f),
                        new TierLoot(14, LootType.Armor, 0.05f),
                        new TierLoot(6, LootType.Ring, 0.2f),
                        new ItemLoot("Ring of Decades", 0.025f),
                        new ItemLoot("Sword of the Colossus", 0.025f),
                        new ItemLoot("Breastplate of New Life", 0.025f),
                        new ItemLoot("Marble Seal", 0.025f),
                        new ItemLoot("Magical Lodestone", 0.025f)
                    )
                    
            )
            db.Init("LH Void Rift Spawner",
                new State("base",
                    new State("S-R",
                        new Order(15, "LH Colossus Pillar Summoner", "S-S"),
                        new TossObject("LH Rift 10", 1, 180, 99999999, 200, true),
                        new TossObject("LH Rift 11", 0, 0, 99999999, 200, true),
                        new TossObject("LH Rift 15", 1, 135, 99999999, 200, true),
                        new TossObject("LH Rift 16", 1, 90, 99999999, 200, true),
                        new TossObject("LH Rift 8", 3, 200, 99999999, 2600, true),
                        new TossObject("LH Rift 9", 2, 208, 99999999, 2600, true),
                        new TossObject("LH Rift A", 1, 226, 99999999, 2600, true),
                        new TossObject("LH Rift B", 1, 180, 99999999, 2600, true),
                        new TossObject("LH Rift C", 1, 270, 99999999, 2600, true),
                        new TossObject("LH Rift D", 1, 314, 99999999, 2600, true),
                        new TossObject("LH Rift E", 3, 180, 99999999, 2600, true),
                        new TossObject("LH Rift F", 2, 180, 99999999, 2600, true),
                        new TossObject("LH Rift 13", 3, 160, 99999999, 2600, true),
                        new TossObject("LH Rift 14", 2, 154, 99999999, 2600, true),
                        new TossObject("LH Rift 1B", 2, 90, 99999999, 2600, true),
                        new TossObject("LH Rift 21", 1, 0, 99999999, 2600, true),
                        new TossObject("LH Rift 0", 4, 216, 99999999, 5000, true),
                        new TossObject("LH Rift 1", 4, 206, 99999999, 5000, true),
                        new TossObject("LH Rift 2", 3, 212, 99999999, 5000, true),
                        new TossObject("LH Rift 3", 3, 226, 99999999, 5000, true),
                        new TossObject("LH Rift 4", 2, 270, 99999999, 5000, true),
                        new TossObject("LH Rift 5", 2, 295, 99999999, 5000, true),
                        new TossObject("LH Rift 6", 2, 315, 99999999, 5000, true),
                        new TossObject("LH Rift 7", 3, 325, 99999999, 5000, true),
                        new TossObject("LH Rift 12", 3, 72, 99999999, 5000, true),
                        new TossObject("LH Rift 17", 1, 46, 99999999, 5000, true),
                        new TossObject("LH Rift 18", 4, 154, 99999999, 5000, true),
                        new TossObject("LH Rift 19", 3, 146, 99999999, 5000, true),
                        new TossObject("LH Rift 1A", 2, 134, 99999999, 5000, true),
                        new TossObject("LH Rift 1C", 2, 66, 99999999, 5000, true),
                        new TossObject("LH Rift 1D", 2, 45, 99999999, 5000, true),
                        new TossObject("LH Rift 1E", 4, 144, 99999999, 5000, true),
                        new TossObject("LH Rift 1F", 3, 124, 99999999, 5000, true),
                        new TossObject("LH Rift 20", 3, 90, 99999999, 5000, true),
                        new TossObject("LH Rift 22", 3, 56, 99999999, 5000, true),
                        new TossObject("LH Rift 23", 3, 46, 99999999, 5000, true),
                        new Decay(5000)
                        )
                    )
            )
            db.Init("LH Colossus Pillar Summoner",//This object takes orders from the Colossus and spawns One pillar and orders it to have the correct phase (x4)
                new State("base",
                    new OnParentDeathTransition("S-S"),
                    new State("S-P-1-A",
                        new Spawn("LH Colossus Pillar", 1, 1, 30000),
                        new EntitiesWithinTransition(3, "LH Colossus Pillar", "S-P-1-B")
                        ),
                    new State("S-P-1-B",
                        new Spawn("LH Colossus Pillar", 0, 1, 5000),
                        new TimedTransition("S-P-1-A", 6000)
                        ),
                    new State("S-P-2-A",
                        new Order(3, "LH Colossus Pillar", "PL-TR-B"),
                        new Spawn("LH Colossus Pillar", 1, 1, 30000),
                        new EntitiesWithinTransition(3, "LH Colossus Pillar", "S-P-2-B")
                        ),
                    new State("S-P-2-B",
                        new Spawn("LH Colossus Pillar", 0, 1, 5000),
                        new TimedTransition("S-P-1-A", 6000)
                        ),
                    new State("S-P-3-A",
                        new Order(3, "LH Colossus Pillar", "PL-TR-C"),
                        new Spawn("LH Colossus Pillar", 1, 1, 30000),
                        new EntitiesWithinTransition(3, "LH Colossus Pillar", "S-P-3-B")
                        ),
                    new State("S-P-3-B",
                        new Spawn("LH Colossus Pillar", 0, 1, 5000),
                        new TimedTransition("S-P-1-A", 6000)
                        ),
                    new State("S-P-4-A",
                        new Order(3, "LH Colossus Pillar", "PL-TR-D"),
                        new Spawn("LH Colossus Pillar", 1, 1, 30000),
                        new EntitiesWithinTransition(3, "LH Colossus Pillar", "S-P-4-B")
                        ),
                    new State("S-P-4-B",
                        new Spawn("LH Colossus Pillar", 0, 1, 5000),
                        new TimedTransition("S-P-1-A", 6000)
                        ),
                    new State("S-S",
                        new Timed(1000, new Order(15, "LH Colossus Pillar", targetState:"S-S")),
                        new Decay(1000)
                        )
                    )
            )
            db.Init("LH Colossus Pillar",
                new State("base",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 1800),
                    new Decay(25000),
                    new State("PL-TR-A",
                        new TimedRandomTransition(0, false, "PL-0-A", "PL-1-A", "PL-2-A", "PL-3-A", "PL-4-A", "PL-5-A", "PL-6-A", "PL-7-A")
                        ),
                    new State("PL-TR-B",
                        new TimedRandomTransition(0, false, "PL-0-B", "PL-1-B", "PL-2-B", "PL-3-B", "PL-4-B", "PL-5-B", "PL-6-B", "PL-7-B")
                        ),
                    new State("PL-TR-C",
                        new SetAltTexture(8),
                        new TimedRandomTransition(1800, false, "PL-8-A", "PL-8-B", "PL-8-C")
                        ),
                    new State("PL-TR-D",
                        new SetAltTexture(9),
                        new TimedRandomTransition(1800, false, "PL-9-A", "PL-9-B", "PL-9-C")
                        ),
                    new State("PL-0-A",
                        new Shoot(0, 8, 45, 0, cooldownOffset: 600, cooldown: 2000)
                        ),
                    new State("PL-1-A",
                        new SetAltTexture(1),
                        new Shoot(0, 8, 45, 1, cooldownOffset: 600, cooldown: 2000)
                        ),
                    new State("PL-2-A",
                        new SetAltTexture(2),
                        new Shoot(0, 8, 45, 2, cooldownOffset: 600, cooldown: 2000)
                        ),
                    new State("PL-3-A",
                        new SetAltTexture(3),
                        new Shoot(0, 8, 45, 3, cooldownOffset: 600, cooldown: 2000)
                        ),
                    new State("PL-4-A",
                        new SetAltTexture(4),
                        new Shoot(0, 8, 45, 4, cooldownOffset: 600, cooldown: 2000)
                        ),
                    new State("PL-5-A",
                        new SetAltTexture(5),
                        new Shoot(0, 8, 45, 5, cooldownOffset: 600, cooldown: 2000)
                        ),
                    new State("PL-6-A",
                        new SetAltTexture(6),
                        new Shoot(0, 8, 45, 6, cooldownOffset: 600, cooldown: 2000)
                        ),
                    new State("PL-7-A",
                        new SetAltTexture(7),
                        new Shoot(0, 8, 45, 7, cooldownOffset: 600, cooldown: 2000)
                        ),
                    new State("PL-0-B",
                        new Shoot(0, 8, 45, 0, rotateAngle: 10, cooldownOffset: 600, cooldown: 2000),
                        new TimedRandomTransition(12600, false, "PL-0-C", "PL-0-D")
                        ),
                    new State("PL-1-B",
                        new SetAltTexture(1),
                        new Shoot(0, 8, 45, 1, rotateAngle: 10, cooldownOffset: 600, cooldown: 2000),
                        new TimedRandomTransition(12600, false, "PL-1-C", "PL-1-D")
                        ),
                    new State("PL-2-B",
                        new SetAltTexture(2),
                        new Shoot(0, 8, 45, 2, rotateAngle: 10, cooldownOffset: 600, cooldown: 2000),
                        new TimedRandomTransition(12600, false, "PL-2-C", "PL-2-D")
                        ),
                    new State("PL-3-B",
                        new SetAltTexture(3),
                        new Shoot(0, 8, 45, 3, rotateAngle: 10, cooldownOffset: 600, cooldown: 2000),
                        new TimedRandomTransition(12600, false, "PL-3-C", "PL-3-D")
                        ),
                    new State("PL-4-B",
                        new SetAltTexture(4),
                        new Shoot(0, 8, 45, 4, rotateAngle: 10, cooldownOffset: 600, cooldown: 2000),
                        new TimedRandomTransition(12600, false, "PL-4-C", "PL-4-D")
                        ),
                    new State("PL-5-B",
                        new SetAltTexture(5),
                        new Shoot(0, 8, 45, 5, rotateAngle: 10, cooldownOffset: 600, cooldown: 2000),
                        new TimedRandomTransition(12600, false, "PL-5-C", "PL-5-D")
                        ),
                    new State("PL-6-B",
                        new SetAltTexture(6),
                        new Shoot(0, 8, 45, 6, rotateAngle: 10, cooldownOffset: 600, cooldown: 2000),
                        new TimedRandomTransition(12600, false, "PL-6-C", "PL-6-D")
                        ),
                    new State("PL-7-B",
                        new SetAltTexture(7),
                        new Shoot(0, 8, 45, 7, rotateAngle: 10, cooldownOffset: 600, cooldown: 2000),
                        new TimedRandomTransition(12600, false, "PL-7-C", "PL-7-D")
                        ),
                    new State("PL-0-C",
                        new Shoot(0, 12, 30, 0, cooldownOffset: 600, cooldown: 2000),
                        new TimedRandomTransition(12600, false, "PL-0-B", "PL-0-D")
                        ),
                    new State("PL-1-C",
                        new SetAltTexture(1),
                        new Shoot(0, 12, 30, 1, cooldownOffset: 600, cooldown: 2000),
                        new TimedRandomTransition(12600, false, "PL-1-B", "PL-1-D")
                        ),
                    new State("PL-2-C",
                        new SetAltTexture(2),
                        new Shoot(0, 12, 30, 2, cooldownOffset: 600, cooldown: 2000),
                        new TimedRandomTransition(12600, false, "PL-2-B", "PL-2-D")
                        ),
                    new State("PL-3-C",
                        new SetAltTexture(3),
                        new Shoot(0, 12, 30, 3, cooldownOffset: 600, cooldown: 2000),
                        new TimedRandomTransition(12600, false, "PL-3-B", "PL-3-D")
                        ),
                    new State("PL-4-C",
                        new SetAltTexture(4),
                        new Shoot(0, 12, 30, 4, cooldownOffset: 600, cooldown: 2000),
                        new TimedRandomTransition(12600, false, "PL-4-B", "PL-4-D")
                        ),
                    new State("PL-5-C",
                        new SetAltTexture(5),
                        new Shoot(0, 12, 30, 5, cooldownOffset: 600, cooldown: 2000),
                        new TimedRandomTransition(12600, false, "PL-5-B", "PL-5-D")
                        ),
                    new State("PL-6-C",
                        new SetAltTexture(6),
                        new Shoot(0, 12, 30, 6, cooldownOffset: 600, cooldown: 2000),
                        new TimedRandomTransition(12600, false, "PL-6-B", "PL-6-D")
                        ),
                    new State("PL-7-C",
                        new SetAltTexture(7),
                        new Shoot(0, 12, 30, 7, cooldownOffset: 600, cooldown: 2000),
                        new TimedRandomTransition(12600, false, "PL-7-B", "PL-7-D")
                        ),
                    new State("PL-0-D",
                        new Shoot(0, 2, 180, 0, rotateAngle: 30, defaultAngle: 0, cooldownOffset: 600, cooldown: 200),
                        new TimedRandomTransition(12600, false, "PL-0-B", "PL-0-C")
                        ),
                    new State("PL-1-D",
                        new SetAltTexture(1),
                        new Shoot(0, 2, 180, 1, rotateAngle: 30, defaultAngle: 0, cooldownOffset: 600, cooldown: 200),
                        new TimedRandomTransition(12600, false, "PL-1-B", "PL-1-C")
                        ),
                    new State("PL-2-D",
                        new SetAltTexture(2),
                        new Shoot(0, 2, 180, 2, rotateAngle: 30, defaultAngle: 0, cooldownOffset: 600, cooldown: 200),
                        new TimedRandomTransition(12600, false, "PL-2-B", "PL-2-C")
                        ),
                    new State("PL-3-D",
                        new SetAltTexture(3),
                        new Shoot(0, 2, 180, 3, rotateAngle: 30, defaultAngle: 0, cooldownOffset: 600, cooldown: 200),
                        new TimedRandomTransition(12600, false, "PL-3-B", "PL-3-C")
                        ),
                    new State("PL-4-D",
                        new SetAltTexture(4),
                        new Shoot(0, 2, 180, 4, rotateAngle: 30, defaultAngle: 0, cooldownOffset: 600, cooldown: 200),
                        new TimedRandomTransition(12600, false, "PL-4-B", "PL-4-C")
                        ),
                    new State("PL-5-D",
                        new SetAltTexture(5),
                        new Shoot(0, 2, 180, 5, rotateAngle: 30, defaultAngle: 0, cooldownOffset: 600, cooldown: 200),
                        new TimedRandomTransition(12600, false, "PL-5-B", "PL-5-C")
                        ),
                    new State("PL-6-D",
                        new SetAltTexture(6),
                        new Shoot(0, 2, 180, 6, rotateAngle: 30, defaultAngle: 0, cooldownOffset: 600, cooldown: 200),
                        new TimedRandomTransition(12600, false, "PL-6-B", "PL-6-C")
                        ),
                    new State("PL-7-D",
                        new SetAltTexture(7),
                        new Shoot(0, 2, 180, 7, rotateAngle: 30, defaultAngle: 0, cooldownOffset: 600, cooldown: 200),
                        new TimedRandomTransition(12600, false, "PL-7-B", "PL-7-C")
                        ),
                    new State("base",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 1200),
                        new ConditionalEffect(ConditionEffectIndex.Armored, true),
                        new State("PL-8-A",
                            new Shoot(0, 14, 360/14, 16, 0, cooldown: 1800),
                            new TimedRandomTransition(0, false, "PL-8-B", "PL-8-C")
                            ),
                        new State("PL-8-B",
                            new Shoot(0, 6, 60, 18, defaultAngle: 0, cooldown: 1200),
                            new TimedRandomTransition(0, false, "PL-8-A", "PL-8-C")
                            ),
                        new State("PL-8-C",
                            new Shoot(0, 2, 180, 17, rotateAngle: 15, defaultAngle: 0, cooldown: 200),
                            new TimedRandomTransition(0, false, "PL-8-A", "PL-8-B")
                            )
                        ),
                    new State("base",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new State("PL-9-A",
                            new Shoot(0, 14, 360/14, 19, 0, cooldown: 1800),
                            new TimedRandomTransition(0, false, "PL-9-B", "PL-9-C")
                            ),
                        new State("PL-9-B",
                            new Shoot(0, 6, 60, 20, defaultAngle: 0, cooldown: 1200),
                            new TimedRandomTransition(0, false, "PL-9-A", "PL-9-C")
                            ),
                        new State("PL-9-C",
                            new Shoot(0, 2, 180, 21, rotateAngle: 15,  defaultAngle: 0, cooldown: 200),
                            new TimedRandomTransition(0, false, "PL-9-A", "PL-9-B")
                            )
                        ),
                new State("PL-S",
                    new Flash(0xffffff, .6f, 5),
                    new Decay(3000)
                    )
                )
            )
            db.Init("LH Colossus Rock 1",
                new State("base",
                    new State("R-C",
                        new TimedRandomTransition (200, false, "R-1-M-1", "R-1-M-2")
                        ),
                    new State("R-1-M-1",
                        new Order(3, "LH Colossus Rock 2", "R-M-1"),
                        new Order(3, "LH Colossus Rock 3", "R-M-1"),
                        new MoveLine(0.6f, 0),
                        new TimedTransition("R-C-M-1", 800)
                        ),
                    new State("R-1-M-2",
                        new Order(3, "LH Colossus Rock 2", "R-M-2"),
                        new Order(3, "LH Colossus Rock 3", "R-M-2"),
                        new MoveLine(0.6f, 90),
                        new TimedTransition("R-C-M-2", 800)
                        ),
                    new State("R-C-M-1",
                        new Charge(0.6f, 15, 200),
                        new PlayerWithinTransition(1, "R-B"),
                        new TimedTransition("R-B", 5000)
                        ),
                    new State("R-C-M-2",
                        new Charge(0.6f, 15, 200),
                        new PlayerWithinTransition(1, "R-B"),
                        new TimedTransition("R-B", 5000)
                        ),
                    new State("R-B",
                        new SetAltTexture(1),
                        new Flash(0xFF0000, 0.2f, 3),
                        new Charge(0.6f, 1, 1000),
                        new Shoot(0, 8, 45, 0, cooldownOffset: 800, cooldown: 400),
                        new Decay(800)
                        )
                    )
            )
            db.Init("LH Colossus Rock 2",
                new State("base",
                    new State("R-W",
                        new Protect(0.6f, "LH Colossus Rock 1", 3)
                        ),
                    new State("R-M-1",
                        new MoveLine(0.6f, 120),
                        new TimedTransition("R-C-M-1", 800)
                        ),
                    new State("R-M-2",
                        new MoveLine(0.6f, 210),
                        new TimedTransition("R-C-M-2", 800)
                        ),
                    new State("R-C-M-1",
                        new Charge(0.6f, 15, 200),
                        new PlayerWithinTransition(1, "R-B"),
                        new TimedTransition("R-B", 5000)
                        ),
                    new State("R-C-M-2",
                        new Charge(0.6f, 15, 200),
                        new PlayerWithinTransition(1, "R-B"),
                        new TimedTransition("R-B", 5000)
                        ),
                    new State("R-B",
                        new SetAltTexture(1),
                        new Flash(0xFF0000, 0.2f, 3),
                        new Charge(0.6f, 1, 1000),
                        new Shoot(0, 8, 45, 0, cooldownOffset: 800, cooldown: 400),
                        new Decay(800)
                    )
                )
            )
            db.Init("LH Colossus Rock 3",
                new State("base",
                    new State("R-W",
                        new Protect(0.6f, "LH Colossus Rock 1", 3)
                        ),
                    new State("R-M-1",
                        new MoveLine(0.6f, 240),
                        new TimedTransition("R-C-M-1", 800)
                        ),
                    new State("R-M-2",
                        new MoveLine(0.6f, 300),
                        new TimedTransition("R-C-M-2", 800)
                        ),
                    new State("R-C-M-1",
                        new Charge(0.6f, 15, 200),
                        new PlayerWithinTransition(1, "R-B"),
                        new TimedTransition("R-B", 5000)
                        ),
                    new State("R-C-M-2",
                        new Charge(0.6f, 15, 200),
                        new PlayerWithinTransition(1, "R-B"),
                        new TimedTransition("R-B", 5000)
                        ),
                    new State("R-B",
                        new SetAltTexture(1),
                        new Flash(0xFF0000, 0.2f, 3),
                        new Charge(0.6f, 1, 1000),
                        new Shoot(0, 8, 45, 0, cooldownOffset: 800, cooldown: 400),
                        new Decay(800)
                        )
                    )
            )
            db.Init("LH Colossus Rock 4",// only 2 moves to make
                new State("base",
                    new State("R-C",
                        new TimedRandomTransition (200, false, "R-4-M-1", "R-4-M-2")
                        ),
                    new State("R-M-1",
                        new Order(3, "LH Colossus Rock 5", "R-M-1"),
                        new Order(3, "LH Colossus Rock 6", "R-M-1"),
                        new Order(3, "LH Colossus Rock 7", "R-M-1"),
                        new MoveLine(0.4f, 0),
                        new TimedTransition("R-C-M-1", 1000)
                        ),
                    new State("R-M-2",
                        new Order(3, "LH Colossus Rock 5", "R-M-2"),
                        new Order(3, "LH Colossus Rock 6", "R-M-2"),
                        new Order(3, "LH Colossus Rock 7", "R-M-2"),
                        new MoveLine(0.5f, 90),
                        new TimedTransition("R-C-M-2", 1000)
                        ),
                    new State("R-C-M-1",
                        new Charge(0.5f, 15, 200),
                        new PlayerWithinTransition(1, "R-B"),
                        new TimedTransition("R-B", 5000)
                        ),
                    new State("R-C-M-2",
                        new Charge(0.5f,15, 200),
                        new PlayerWithinTransition(1, "R-B"),
                        new TimedTransition("R-B", 5000)
                        ),
                    new State("R-B",
                        new SetAltTexture(1),
                        new Flash(0xFF0000, 0.2f, 3),
                        new Charge(0.5f, 1, 1000),
                        new Shoot(0, 8, 45, 0, cooldownOffset: 800, cooldown: 400),
                        new Decay(800)
                        )
                    )
            )
            db.Init("LH Colossus Rock 5",
                new State("base",
                    new State("R-W",
                        new Protect(0.4f, "LH Colossus Rock 1", 3)
                        ),
                    new State("R-M-1",
                        new MoveLine(0.5f, 90),
                        new TimedTransition("R-C-M-1", 1000)
                        ),
                    new State("R-M-2",
                        new MoveLine(0.5f, 180),
                        new TimedTransition("R-C-M-2", 1000)
                        ),
                    new State("R-C-M-1",
                        new Charge(0.5f, 15, 200),
                        new PlayerWithinTransition(1, "R-B"),
                        new TimedTransition("R-B", 5000)
                        ),
                    new State("R-C-M-2",
                        new Charge(0.5f, 15, 200),
                        new PlayerWithinTransition(1, "R-B"),
                        new TimedTransition("R-B", 5000)
                        ),
                    new State("R-B",
                        new SetAltTexture(1),
                        new Flash(0xFF0000, 0.2f, 3),
                        new Charge(0.5f, 1, 1000),
                        new Shoot(0, 8, 45, 0, cooldownOffset: 800, cooldown: 400),
                        new Decay(800)
                        )
                    )
            )
            db.Init("LH Colossus Rock 6",
                new State("base",
                    new State("R-W",
                        new Protect(0.5f, "LH Colossus Rock 1", 3)
                        ),
                    new State("R-M-1",
                        new MoveLine(0.5f, 180),
                        new TimedTransition("R-C-M-1", 1000)
                        ),
                    new State("R-M-2",
                        new MoveLine(0.5f, 270),
                        new TimedTransition("R-C-M-2", 1000)
                        ),
                    new State("R-C-M-1",
                        new Charge(0.5f, 15, 200),
                        new PlayerWithinTransition(1, "Rock-Break-1"),
                        new TimedTransition("R-B", 5000)
                        ),
                    new State("R-C-M-2",
                        new Charge(0.5f, 15, 200),
                        new PlayerWithinTransition(1, "R-B"),
                        new TimedTransition("R-B", 5000)
                        ),
                    new State("R-B",
                        new SetAltTexture(1),
                        new Flash(0xFF0000, 0.2f, 3),
                        new Charge( 0.5f, 1, 1000),
                        new Shoot(0, 8, 45, 0, cooldownOffset: 800, cooldown: 400),
                        new Decay(800)
                        )
                    )
            )
            db.Init("LH Colossus Rock 7",
                new State("base",
                    new State("R-W",
                        new Protect(0.5f, "LH Colossus Rock 1", 3)
                        ),
                    new State("R-M-1",
                        new MoveLine(0.5f, 270),
                        new TimedTransition("R-C-M-1", 1000)
                        ),
                    new State("R-M-2",
                        new MoveLine(0.5f, 360),
                        new TimedTransition("R-C-M-2", 1000)
                        ),
                    new State("R-C-M-1",
                        new Charge(0.5f, 15, 200),
                        new PlayerWithinTransition(1, "R-B"),
                        new TimedTransition("R-B", 5000)
                        ),
                    new State("R-C-M-2",
                        new Charge(0.5f, 15, 200),
                        new PlayerWithinTransition(1, "R-B"),
                        new TimedTransition("R-B", 5000)
                        ),
                    new State("R-B",
                        new SetAltTexture(1),
                        new Flash(0xFF0000, 0.2f, 3),
                        new Charge(0.5f, 1, 1000),
                        new Shoot(0, 8, 45, 0, cooldownOffset: 800, cooldown: 400),
                        new Decay(800)
                        )
                    )
            )
            db.Init("LH Colossus Rock 8",
                new State("base",
                    new State("R-C",
                        new TimedRandomTransition(50, false, "R-8-R", "R-8-L", "R-8-U", "R-8-D")
                        ),
                    new State("R-8-R",
                        new Order(3, "LH Colossus Rock 9", "R-9-R"),
                        new Duration(new MoveLine(0.8f, 22.5f), 200),
                        new Timed(200, new Duration(new MoveLine(0.8f, 45), 200)),
                        new Timed(400, new Duration(new MoveLine(0.8f, 67.5f), 200)),
                        new Timed(600, new Duration(new MoveLine(0.8f, 90), 200)),
                        new Timed(800, new Charge(0.7f, 25, 200)),
                        new PlayerWithinTransition(1, "R-B"),
                        new TimedTransition("R-B", 6200)
                        ),
                    new State("R-8-L",
                        new Order(3, "LH Colossus Rock 9", "R-9-L"),
                        new Duration(new MoveLine(0.8f, 202.5f), 200),
                        new Timed(200, new Duration(new MoveLine(0.8f, 225), 200)),
                        new Timed(400, new Duration(new MoveLine(0.8f, 247.5f), 200)),
                        new Timed(600, new Duration(new MoveLine(0.8f, 270), 200)),
                        new Timed(800, new Charge(0.7f, 25, 200)),
                        new PlayerWithinTransition(1, "R-B"),
                        new TimedTransition("R-B", 6200)
                        ),
                    new State("R-8-U",
                        new Order(3, "LH Colossus Rock 9", "R-9-U"),
                        new Duration(new MoveLine(0.8f, 292.5f), 200),
                        new Timed(200, new Duration(new MoveLine(0.8f, 315), 200)),
                        new Timed(400, new Duration(new MoveLine(0.8f, 337.5f), 200)),
                        new Timed(600, new Duration(new MoveLine(0.8f, 360), 200)),
                        new Timed(800, new Charge(0.7f, 25, 200)),
                        new PlayerWithinTransition(1, "R-B"),
                        new TimedTransition("R-B", 6200)
                        ),
                    new State("R-8-D",
                        new Order(3, "LH Colossus Rock 9", "R-9-D"),
                        new Duration(new MoveLine(0.8f, 112.5f), 200),
                        new Timed(200, new Duration(new MoveLine(135, 135), 200)),
                        new Timed(400, new Duration(new MoveLine(157.5f, 157.5f), 200)),
                        new Timed(600, new Duration(new MoveLine(170, 180), 200)),
                        new Timed(800, new Charge(0.7f, 25, 200)),
                        new PlayerWithinTransition(1, "R-B"),
                        new TimedTransition("R-B", 6200)
                        ),
                    new State("R-B",
                        new SetAltTexture(1),
                        new Flash(0xFF0000, 0.2f, 3),
                        new Shoot(0, 8, 45, 0, cooldownOffset: 800, cooldown: 400),
                        new Decay(800)
                        )
                    )
            )
            db.Init("LH Colossus Rock 9",
                new State("base",
                    new State("R-W",
                        new Protect(0.8f, "LH Colossus Rock 8", 3)
                        ),
                    new State("R-9-R",
                        new Duration(new MoveLine(0.8f, 337.5f), 200),
                        new Timed(200, new Duration(new MoveLine(0.8f, 315), 200)),
                        new Timed(400, new Duration(new MoveLine(0.8f, 292.5f), 200)),
                        new Timed(600, new Duration(new MoveLine(0.8f, 270), 200)),
                        new Timed(800, new Charge(0.7f, 25, 200)),
                        new PlayerWithinTransition(1, "R-B"),
                        new TimedTransition("R-B", 6200)
                        ),
                    new State("R-8-L",
                        new Duration(new MoveLine(0.8f, 157.5f), 200),
                        new Timed(200, new Duration(new MoveLine(0.8f, 135), 200)),
                        new Timed(400, new Duration(new MoveLine(0.8f, 112.5f), 200)),
                        new Timed(600, new Duration(new MoveLine(0.8f, 90), 200)),
                        new Timed(800, new Charge(0.7f, 25, 200)),
                        new PlayerWithinTransition(1, "R-B"),
                        new TimedTransition("R-B", 6200)
                        ),
                    new State("R-8-U",
                        new Duration(new MoveLine(0.8f, 247.5f), 200),
                        new Timed(200, new Duration(new MoveLine(0.8f, 225), 200)),
                        new Timed(400, new Duration(new MoveLine(0.8f, 202.5f), 200)),
                        new Timed(600, new Duration(new MoveLine(0.8f, 180), 200)),
                        new Timed(800, new Charge(0.7f, 25, 200)),
                        new PlayerWithinTransition(1, "R-B"),
                        new TimedTransition("R-B", 6200)
                        ),
                    new State("R-8-D",
                        new Duration(new MoveLine(0.8f, 90), 200),
                        new Timed(200, new Duration(new MoveLine(0.8f, 67.5f), 200)),
                        new Timed(400, new Duration(new MoveLine(0.8f, 45), 200)),
                        new Timed(600, new Duration(new MoveLine(0.8f, 22.5f), 200)),
                        new Timed(800, new Charge(0.7f, 25, 200)),
                        new PlayerWithinTransition(1, "R-B"),
                        new TimedTransition("R-B", 6200)
                        ),
                    new State("R-B",
                        new SetAltTexture(1),
                        new Flash(0xFF0000, 0.2f, 3),
                        new Shoot(0, 8, 45, 0, cooldownOffset: 800, cooldown: 400),
                        new Decay(800)
                        )
                    )
            )
            db.Init("LH Colossus Rock 10",
                new State("base",
                    new State("R-TR",
                        new TimedRandomTransition(0, false, "R-0-A", "R-1-A", "R-2-A", "R-3-A", "R-4-A", "R-5-A", "R-6-A", "R-7-A")
                        ),
                    new State("R-0-A",
                        new SetAltTexture(0),
                        new Shoot(0, 2, 180, 0, rotateAngle: 10, defaultAngle: 0, cooldown: 200),
                        new TimedRandomTransition(5200, false, "R-1-A", "R-2-A", "R-3-A", "R-4-A", "R-5-A", "R-6-A", "R-7-A")
                        ),
                    new State("R-1-A",
                        new SetAltTexture(1),
                        new Shoot(0, 2, 180, 1, rotateAngle: 10, defaultAngle: 0, cooldown: 200),
                        new TimedRandomTransition(5200, false, "R-0-A", "R-2-A", "R-3-A", "R-4-A", "R-5-A", "R-6-A", "R-7-A")
                        ),
                    new State("R-2-A",
                        new SetAltTexture(2),
                        new Shoot(0, 2, 180, 2, rotateAngle: 10, defaultAngle: 0, cooldown: 200),
                        new TimedRandomTransition(5200, false, "R-0-A", "R-1-A", "R-3-A", "R-4-A", "R-5-A", "R-6-A", "R-7-A")
                        ),
                    new State("R-3-A",
                        new SetAltTexture(3),
                        new Shoot(0, 2, 180, 3, rotateAngle: 10, defaultAngle: 0, cooldown: 200),
                        new TimedRandomTransition(5200, false, "R-0-A", "R-1-A", "R-0-A", "R-4-A", "R-5-A", "R-6-A", "R-7-A")
                        ),
                    new State("R-4-A",
                        new SetAltTexture(4),
                        new Shoot(0, 2, 180, 4, rotateAngle: 10, defaultAngle: 0, cooldown: 200),
                        new TimedRandomTransition(5200, false, "R-0-A", "R-1-A", "R-2-A", "R-3-A", "R-5-A", "R-6-A", "R-7-A")
                        ),
                    new State("R-5-A",
                        new SetAltTexture(5),
                        new Shoot(0, 2, 180, 5, rotateAngle: 10, defaultAngle: 0, cooldown: 200),
                        new TimedRandomTransition(5200, false, "R-0-A", "R-1-A", "R-2-A", "R-3-A", "R-4-A", "R-6-A", "R-7-A")
                        ),
                    new State("R-6-A",
                        new SetAltTexture(6),
                        new Shoot(0, 2, 180, 6, rotateAngle: 10, defaultAngle: 0, cooldown: 200),
                        new TimedRandomTransition(5200, false, "R-0-A", "R-1-A", "R-2-A", "R-3-A", "R-4-A", "R-5-A", "R-7-A")
                        ),
                    new State("R-7-A",
                        new SetAltTexture(7),
                        new Shoot(0, 2, 180, 7, rotateAngle: 10, defaultAngle: 0, cooldown: 200),
                        new TimedRandomTransition(5200, false, "R-0-A", "R-1-A", "R-2-A", "R-3-A", "R-4-A", "R-5-A", "R-6-A")
                        ),
                    new State("R-TR-2",
                        new TimedRandomTransition(0, false, "R-0-B", "R-1-B", "R-2-B", "R-3-B", "R-4-B", "R-5-B", "R-6-B", "R-7-B")
                        ),
                    new State("R-0-B",
                        new SetAltTexture(0),
                        new Shoot(0, 2, 180, 0, rotateAngle: 10, defaultAngle: 0, cooldown: 200),
                        new EntityHealthTransition(20, "LH Marble Colossus", 0.45f, "R-S-0"),
                        new TimedRandomTransition(5200, false, "R-1-B", "R-2-B", "R-3-B", "R-4-B", "R-5-B", "R-6-B", "R-7-B")
                        ),
                    new State("R-1-B",
                        new SetAltTexture(1),
                        new Shoot(0, 2, 180, 1, rotateAngle: 10, defaultAngle: 0, cooldown: 200),
                        new EntityHealthTransition(20, "LH Marble Colossus", 0.45f, "R-S-1"),
                        new TimedRandomTransition(5200, false, "R-0-B", "R-2-B", "R-3-B", "R-4-B", "R-5-B", "R-6-B", "R-7-B")
                        ),
                    new State("R-2-B",
                        new SetAltTexture(2),
                        new Shoot(0, 2, 180, 2, rotateAngle: 10, defaultAngle: 0, cooldown: 200),
                        new EntityHealthTransition(20, "LH Marble Colossus", 0.45f, "R-S-2"),
                        new TimedRandomTransition(5200, false, "R-0-B", "R-1-B", "R-3-B", "R-4-B", "R-5-B", "R-6-B", "R-7-B")
                        ),
                    new State("R-3-B",
                        new SetAltTexture(3),
                        new Shoot(0, 2, 180, 3, rotateAngle: 10, defaultAngle: 0, cooldown: 200),
                        new EntityHealthTransition(20, "LH Marble Colossus", 0.45f, "R-S-3"),
                        new TimedRandomTransition(5200, false, "R-0-B", "R-1-B", "R-0-B", "R-4-B", "R-5-B", "R-6-B", "R-7-B")
                        ),
                    new State("R-4-B",
                        new SetAltTexture(4),
                        new Shoot(0, 2, 180, 4, rotateAngle: 10, defaultAngle: 0, cooldown: 200),
                        new EntityHealthTransition(20, "LH Marble Colossus", 0.45f, "R-S-4"),
                        new TimedRandomTransition(5200, false, "R-0-B", "R-1-B", "R-2-B", "R-3-B", "R-5-B", "R-6-B", "R-7-B")
                        ),
                    new State("R-5-B",
                        new SetAltTexture(5),
                        new Shoot(0, 2, 180, 5, rotateAngle: 10, defaultAngle: 0, cooldown: 200),
                        new EntityHealthTransition(20, "LH Marble Colossus", 0.45f, "R-S-5"),
                        new TimedRandomTransition(5200, false, "R-0-B", "R-1-B", "R-2-B", "R-3-B", "R-4-B", "R-6-B", "R-7-B")
                        ),
                    new State("R-6-B",
                        new SetAltTexture(6),
                        new Shoot(0, 2, 180, 6, rotateAngle: 10, defaultAngle: 0, cooldown: 200),
                        new EntityHealthTransition(20, "LH Marble Colossus", 0.45f, "R-S-6"),
                        new TimedRandomTransition(5200, false, "R-0-B", "R-1-B", "R-2-B", "R-3-B", "R-4-B", "R-5-B", "R-7-B")
                        ),
                    new State("R-7-B",
                        new SetAltTexture(7),
                        new Shoot(0, 2, 180, 7, rotateAngle: 10, defaultAngle: 0, cooldown: 200),
                        new EntityHealthTransition(20, "LH Marble Colossus", 0.45f, "R-S-7"),
                        new TimedRandomTransition(5200, false, "R-0-B", "R-1-B", "R-2-B", "R-3-B", "R-4-B", "R-5-B", "R-6-B")
                        ),
                    new State("R-S-0",
                        new Flash(0x000000, 0.20f, 3),
                        new Shoot(0, 8, 45, 0, cooldownOffset: 400, cooldown: 200),
                        new Decay(600)
                        ),
                    new State("R-S-1",
                        new Flash(0x000000, 0.20f, 3),
                        new Shoot(0, 8, 45, 1, cooldownOffset: 400, cooldown: 200),
                        new Decay(600)
                        ),
                    new State("R-S-2",
                        new Flash(0x000000, 0.20f, 3),
                        new Shoot(0, 8, 45, 2, cooldownOffset: 400, cooldown: 200),
                        new Decay(600)
                        ),
                    new State("R-S-3",
                        new Flash(0x000000, 0.20f, 3),
                        new Shoot(0, 8, 45, 3, cooldownOffset: 400, cooldown: 200),
                        new Decay(600)
                        ),
                    new State("R-S-4",
                        new Flash(0x000000, 0.20f, 3),
                        new Shoot(0, 8, 45, 4, cooldownOffset: 400, cooldown: 200),
                        new Decay(600)
                        ),
                    new State("R-S-5",
                        new Flash(0x000000, 0.20f, 3),
                        new Shoot(0, 8, 45, 5, cooldownOffset: 400, cooldown: 200),
                        new Decay(600)
                        ),
                    new State("R-S-6",
                        new Flash(0x000000, 0.20f, 3),
                        new Shoot(0, 8, 45, 6, cooldownOffset: 400, cooldown: 200),
                        new Decay(600)
                        ),
                    new State("R-S-7",
                        new Flash(0x000000, 0.20f, 3),
                        new Shoot(0, 8, 45, 7, cooldownOffset: 600, cooldown: 200),
                        new Decay(600)
                        )
                    )
            )
            db.Init("LH Colossus Rock 11",
                new State("base",
                    new State("R-TR-3",
                        new TimedRandomTransition(0, false, "R-0-A", "R-1-A", "R-2-A", "R-3-A", "R-4-A", "R-5-A", "R-6-A", "R-7-A")
                        ),
                    new State("R-0-A",
                        new SetAltTexture(0),
                        new Shoot(0, 6, 60, 0, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 200),
                        new TimedRandomTransition(5400, false, "R-1-A", "R-2-A", "R-3-A", "R-4-A", "R-5-A", "R-6-A", "R-7-A")
                        ),
                    new State("R-1-A",
                        new SetAltTexture(1),
                        new Shoot(0, 6, 60, 1, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 200),
                        new TimedRandomTransition(5400, false, "R-0-A", "R-2-A", "R-3-A", "R-4-A", "R-5-A", "R-6-A", "R-7-A")
                        ),
                    new State("R-2-A",
                        new SetAltTexture(2),
                        new Shoot(0, 6, 60, 2, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 200),
                        new TimedRandomTransition(5400, false, "R-0-A", "R-1-A", "R-3-A", "R-4-A", "R-5-A", "R-6-A", "R-7-A")
                        ),
                    new State("R-3-A",
                        new SetAltTexture(3),
                        new Shoot(0, 6, 60, 3, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 200),
                        new TimedRandomTransition(5400, false, "R-0-A", "R-1-A", "R-2-A", "R-4-A", "R-5-A", "R-6-A", "R-7-A")
                        ),
                    new State("R-4-A",
                        new SetAltTexture(4),
                        new Shoot(0, 6, 60, 4, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 200),
                        new TimedRandomTransition(5400, false, "R-0-A", "R-1-A", "R-2-A", "R-3-A", "R-5-A", "R-6-A", "R-7-A")
                        ),
                    new State("R-5-A",
                        new SetAltTexture(5),
                        new Shoot(0, 6, 60, 5, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 200),
                        new TimedRandomTransition(5400, false, "R-0-A", "R-1-A", "R-2-A", "R-3-A", "R-4-A", "R-6-A", "R-7-A")
                        ),
                    new State("R-6-A",
                        new SetAltTexture(6),
                        new Shoot(0, 6, 60, 6, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 200),
                        new TimedRandomTransition(5400, false, "R-0-A", "R-1-A", "R-2-A", "R-3-A", "R-4-A", "R-5-A", "R-7-A")
                        ),
                    new State("R-7-A",
                        new SetAltTexture(7),
                        new Shoot(0, 6, 60, 7, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 200),
                        new TimedRandomTransition(5400, false, "R-0-A", "R-1-A", "R-2-A", "R-3-A", "R-4-A", "R-5-A", "R-6-A")
                        ),
                    new State("R-TR-4",
                        new TimedRandomTransition(0, false, "R-0-2", "R-1-2", "R-2-2", "R-3-2", "R-4-2", "R-5-2", "R-6-2", "R-7-2")
                        ),
                    new State("R-0-B",
                        new SetAltTexture(0),
                        new Orbit(0.4f, 7, 20, "LH Marble Colossus Anchor"),
                        new Shoot(0, 3, 120, 0, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 200),
                        new TimedRandomTransition(5400, false, "R-1-B", "R-2-B", "R-3-B", "R-4-B", "R-5-B", "R-6-B", "R-7-B")
                        ),
                    new State("R-1-B",
                        new SetAltTexture(1),
                        new Orbit(0.4f, 7, 20, "LH Marble Colossus Anchor"),
                        new Shoot(0, 3, 120, 1, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 200),
                        new TimedRandomTransition(5400, false, "R-0-B", "R-2-B", "R-3-B", "R-4-B", "R-5-B", "R-6-B", "R-7-B")
                        ),
                    new State("R-2-B",
                        new SetAltTexture(2),
                        new Orbit(0.4f, 7, 20, "LH Marble Colossus Anchor"),
                        new Shoot(0, 3, 120, 2, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 200),
                        new TimedRandomTransition(5400, false, "R-0-B", "R-1-B", "R-3-B", "R-4-B", "R-5-B", "R-6-B", "R-7-B")
                        ),
                    new State("R-3-B",
                        new SetAltTexture(3),
                        new Orbit(0.4f, 7, 20, "LH Marble Colossus Anchor"),
                        new Shoot(0, 3, 120, 3, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 200),
                        new TimedRandomTransition(5400, false, "R-0-B", "R-1-B", "R-2-B", "R-4-B", "R-5-B", "R-6-B", "R-7-B")
                        ),
                    new State("R-4-B",
                        new SetAltTexture(4),
                        new Orbit(0.4f, 7, 20, "LH Marble Colossus Anchor"),
                        new Shoot(0, 3, 120, 4, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 200),
                        new TimedRandomTransition(5400, false, "R-0-B", "R-1-B", "R-2-B", "R-3-B", "R-5-B", "R-6-B", "R-7-B")
                        ),
                    new State("R-5-B",
                        new SetAltTexture(5),
                        new Orbit(0.4f, 7, 20, "LH Marble Colossus Anchor"),
                        new Shoot(0, 3, 120, 5, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 200),
                        new TimedRandomTransition(5400, false, "R-0-B", "R-1-B", "R-2-B", "R-3-B", "R-4-B", "R-6-B", "R-7-B")
                        ),
                    new State("R-6-B",
                        new SetAltTexture(6),
                        new Orbit(0.4f, 7, 20, "LH Marble Colossus Anchor"),
                        new Shoot(0, 3, 120, 6, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 200),
                        new TimedRandomTransition(5400, false, "R-0-B", "R-1-B", "R-2-B", "R-3-B", "R-4-B", "R-5-B", "R-7-B")
                        ),
                    new State("R-7-B",
                        new SetAltTexture(7),
                        new Orbit(0.4f, 7, 20, "LH Marble Colossus Anchor"),
                        new Shoot(0, 3, 120, 7, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 200),
                        new TimedRandomTransition(5400, false, "R-0-B", "R-1-B", "R-2-B", "R-3-B", "R-4-B", "R-5-B", "R-6-B")
                        ),
                    new State("R-TR-5",
                        new TimedRandomTransition(5000, false, "R-0-C", "R-1-C", "R-2-C", "R-3-C", "R-4-C", "R-5-C", "R-6-C", "R-7-C")
                        ),
                    new State("R-0-C",
                        new SetAltTexture(0),
                        new Orbit(0.6f, 1, 20, "LH Marble Colossus Anchor", 0.5f),
                        new Shoot(0, 3, 120, 0, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 400, cooldown: 400),
                        new EntityHealthTransition(20, "LH Marble Colossus", 0.35f, "R-S-0"),
                        new TimedRandomTransition(5400, false, "R-1-C", "R-2-C", "R-3-C", "R-4-C", "R-5-C", "R-6-C", "R-7-C")
                        ),
                    new State("R-1-C",
                        new SetAltTexture(1),
                        new Orbit(0.6f, 1, 20, "LH Marble Colossus Anchor", 0.5f),
                        new Shoot(0, 3, 120, 1, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 400),
                        new EntityHealthTransition(20, "LH Marble Colossus", 0.35f, "R-S-1"),
                        new TimedRandomTransition(5400, false, "R-0-C", "R-1-C", "R-2-C", "R-3-C", "R-4-C", "R-5-C", "R-6-C", "R-7-C")
                        ),
                    new State("R-2-C",
                        new SetAltTexture(2),
                        new Orbit(0.6f, 1, 0, "LH Marble Colossus Anchor", 0.5f),
                        new Shoot(0, 3, 120, 2, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 400),
                        new EntityHealthTransition(20, "LH Marble Colossus", 0.35f, "R-S-2"),
                        new TimedRandomTransition(5400, false, "R-0-C", "R-1-C", "R-2-C", "R-3-C", "R-4-C", "R-5-C", "R-6-C", "R-7-C")
                        ),
                    new State("R-3-C",
                        new SetAltTexture(3),
                        new Orbit(0.6f, 1, 20, "LH Marble Colossus Anchor", 0.5f),
                        new Shoot(0, 3, 120, 3, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 400),
                        new EntityHealthTransition(20, "LH Marble Colossus", 0.35f, "R-S-3"),
                        new TimedRandomTransition(5400, false, "R-0-C", "R-1-C", "R-2-C", "R-3-C", "R-4-C", "R-5-C", "R-6-C", "R-7-C")
                        ),
                    new State("R-4-C",
                        new SetAltTexture(4),
                        new Orbit(0.6f, 1, 20, "LH Marble Colossus Anchor", 0.5f),
                        new Shoot(0, 3, 120, 4, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 400),
                        new EntityHealthTransition(20, "LH Marble Colossus", 0.35f, "R-S-4"),
                        new TimedRandomTransition(5400, false, "R-0-C", "R-1-C", "R-2-C", "R-3-C", "R-4-C", "R-5-C", "R-6-C", "R-7-C")
                        ),
                    new State("R-5-C",
                        new SetAltTexture(5),
                        new Orbit(0.6f, 1, 20, "LH Marble Colossus Anchor", 0.5f),
                        new Shoot(0, 3, 120, 5, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 400),
                        new EntityHealthTransition(20, "LH Marble Colossus", 0.35f, "R-S-5"),
                        new TimedRandomTransition(5400, false, "R-0-C", "R-1-C", "R-2-C", "R-3-C", "R-4-C", "R-5-C", "R-6-C", "R-7-C")
                        ),
                    new State("R-6-C",
                        new SetAltTexture(6),
                        new Orbit(0.6f, 1, 20, "LH Marble Colossus Anchor", 0.5f),
                        new Shoot(0, 3, 120, 6, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 400),
                        new EntityHealthTransition(20, "LH Marble Colossus", 0.35f, "R-S-6"),
                        new TimedRandomTransition(5400, false, "R-0-C", "R-1-C", "R-2-C", "R-3-C", "R-4-C", "R-5-C", "R-6-C", "R-7-C")
                        ),
                    new State("R-7-C",
                        new SetAltTexture(7),
                        new Orbit(0.6f, 1, 20, "LH Marble Colossus Anchor", 0.5f),
                        new Shoot(0, 3, 120, 7, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 400),
                        new EntityHealthTransition(20, "LH Marble Colossus", 0.35f, "R-S-7"),
                        new TimedRandomTransition(5400, false, "R-0-C", "R-1-C", "R-2-C", "R-3-C", "R-4-C", "R-5-C", "R-6-C")
                        ),
                    new State("R-S-0",
                        new Flash(0x000000, 0.20f, 3),
                        new Shoot(0, 8, 45, 0, cooldownOffset: 600, cooldown: 200),
                        new Decay(600)
                        ),
                    new State("R-S-1",
                        new Flash(0x000000, 0.20f, 3),
                        new Shoot(0, 8, 45, 1, cooldownOffset: 600, cooldown: 200),
                        new Decay(600)
                        ),
                    new State("R-S-2",
                        new Flash(0x000000, 0.20f, 3),
                        new Shoot(0, 8, 45, 2, cooldownOffset: 600, cooldown: 200),
                        new Decay(600)
                        ),
                    new State("R-S-3",
                        new Flash(0x000000, 0.20f, 3),
                        new Shoot(0, 8, 45, 3, cooldownOffset: 600, cooldown: 200),
                        new Decay(600)
                        ),
                    new State("R-S-4",
                        new Flash(0x000000, 0.20f, 3),
                        new Shoot(0, 8, 45, 4, cooldownOffset: 600, cooldown: 200),
                        new Decay(600)
                        ),
                    new State("R-S-5",
                        new Flash(0x000000, 0.20f, 3),
                        new Shoot(0, 8, 45, 5, cooldownOffset: 600, cooldown: 200),
                        new Decay(600)
                        ),
                    new State("R-S-6",
                        new Flash(0x000000, 0.20f, 3),
                        new Shoot(0, 8, 45, 6, cooldownOffset: 600, cooldown: 200),
                        new Decay(600)
                        ),
                    new State("R-S-7",
                        new Flash(0x000000, 0.20f, 3),
                        new Shoot(0, 8, 45, 7, cooldownOffset: 600, cooldown: 200),
                        new Decay(600)
                        )
                    )
            )
            db.Init("LH Colossus Rock 12",
                new State("base",
                    new OnParentDeathTransition("R-S"),
                    new State("R-TR-4",
                        new TimedRandomTransition(0, false, "R-0-A", "R-1-A", "R-2-A", "R-3-A", "R-4-A", "R-5-A", "R-6-A", "R-7-A")
                        ),
                    new State("R-0-A",
                        new SetAltTexture(0),
                        new Orbit(0.6f, 6, 20, "LH Marble Colossus Anchor", 0.5f, orbitClockwise: true),
                        new Shoot(0, 3, 120, 0, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 200),
                        new TimedRandomTransition(5400, false, "R-1-A", "R-2-A", "R-3-A", "R-4-A", "R-5-A", "R-6-A", "R-7-A")
                        ),
                    new State("R-1-A",
                        new SetAltTexture(1),
                        new Orbit(0.6f, 6, 20, "LH Marble Colossus Anchor", 0.5f, orbitClockwise: true),
                        new Shoot(0, 3, 120, 1, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 200),
                        new TimedRandomTransition(5400, false, "R-0-A", "R-1-A", "R-3-A", "R-4-A", "R-5-A", "R-6-A", "R-7-A")
                        ),
                    new State("R-2-A",
                        new SetAltTexture(2),
                        new Orbit(0.6f, 6, 20, "LH Marble Colossus Anchor", 0.5f, orbitClockwise: true),
                        new Shoot(0, 3, 120, 2, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 200),
                        new TimedRandomTransition(5400, false, "R-0-A", "R-1-A", "R-3-A", "R-4-A", "R-5-A", "R-6-A", "R-7-A")
                        ),
                    new State("R-3-A",
                        new SetAltTexture(3),
                        new Orbit(0.6f, 6, 20, "LH Marble Colossus Anchor", 0.5f, orbitClockwise: true),
                        new Shoot(0, 3, 120, 3, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 200),
                        new TimedRandomTransition(5400, false, "R-0-A", "R-1-A", "R-2-A", "R-4-A", "R-5-A", "R-6-A", "R-7-A")
                        ),
                    new State("R-4-A",
                        new SetAltTexture(4),
                        new Orbit(0.6f, 6, 20, "LH Marble Colossus Anchor", 0.5f, orbitClockwise: true),
                        new Shoot(0, 3, 120, 4, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 200),
                        new TimedRandomTransition(5400, false, "R-0-A", "R-1-A", "R-2-A", "R-3-A", "R-5-A", "R-6-A", "R-7-A")
                        ),
                    new State("R-5-A",
                        new SetAltTexture(5),
                        new Orbit(0.6f, 6, 20, "LH Marble Colossus Anchor", 0.5f, orbitClockwise: true),
                        new Shoot(0, 3, 120, 5, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 200),
                        new TimedRandomTransition(5400, false, "R-0-A", "R-1-A", "R-2-A", "R-3-A", "R-4-A", "R-6-A", "R-7-A")
                        ),
                    new State("R-6-A",
                        new SetAltTexture(6),
                        new Orbit(0.6f, 6, 20, "LH Marble Colossus Anchor", 0.5f, orbitClockwise: true),
                        new Shoot(0, 3, 120, 6, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 200),
                        new TimedRandomTransition(5400, false, "R-0-A", "R-1-A", "R-2-A", "R-3-A", "R-4-A", "R-5-A", "R-7-A")
                        ),
                    new State("R-7-A",
                        new SetAltTexture(7),
                        new Orbit(0.6f, 6, 20, "LH Marble Colossus Anchor", 0.5f, orbitClockwise: true),
                        new Shoot(0, 3, 120, 7, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 200),
                        new TimedRandomTransition(5400, false, "R-0-A", "R-1-A", "R-2-A", "R-3-A", "R-4-A", "R-5-A", "R-6-A")
                        ),
                    new State("R-TR-5",
                        new TimedRandomTransition(0, false, "R-0-B", "R-1-B", "R-2-B", "R-3-B", "R-4-B", "R-5-B", "R-6-B", "R-7-B")
                        ),
                    new State("R-0-B",
                        new SetAltTexture(0),
                        new Orbit(0.6f, 1, 20, "LH Marble Colossus Anchor", 0.5f),
                        new Shoot(0, 3, 120, 0, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 400),
                        new EntityHealthTransition(20, "LH Marble Colossus", 0.35f, "R-S-0"),
                        new TimedRandomTransition(5400, false, "R-0-B", "R-2-B", "R-3-B", "R-4-B", "R-5-B", "R-6-B", "R-7-B")
                        ),
                    new State("R-1-B",
                        new SetAltTexture(1),
                        new Orbit(0.6f, 1, 20, "LH Marble Colossus Anchor", 0.5f),
                        new Shoot(0, 3, 120, 1, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 400),
                        new EntityHealthTransition(20, "LH Marble Colossus", 0.35f, "R-S-1"),
                        new TimedRandomTransition(5400, false, "R-0-B", "R-1-B", "R-3-B", "R-4-B", "R-5-B", "R-6-B", "R-7-B")
                        ),
                    new State("R-2-B",
                        new SetAltTexture(2),
                        new Orbit(0.6f, 1, 20, "LH Marble Colossus Anchor", 0.5f),
                        new Shoot(0, 3, 120, 2, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 400),
                        new EntityHealthTransition(20, "LH Marble Colossus", 0.35f, "R-S-2"),
                        new TimedRandomTransition(5400, false, "R-0-B", "R-1-B", "R-2-B", "R-4-B", "R-5-B", "R-6-B", "R-7-B")
                        ),
                    new State("R-3-B",
                        new SetAltTexture(3),
                        new Orbit(0.6f, 1, 20, "LH Marble Colossus Anchor", 0.5f),
                        new Shoot(0, 3, 120, 3, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 400),
                        new EntityHealthTransition(20, "LH Marble Colossus", 0.35f, "R-S-3"),
                        new TimedRandomTransition(5400, false, "R-0-B", "R-1-B", "R-2-B", "R-3-B", "R-5-B", "R-6-B", "R-7-B")
                        ),
                    new State("R-4-B",
                        new SetAltTexture(4),
                        new Orbit(0.6f, 1, 20, "LH Marble Colossus Anchor", 0.5f),
                        new Shoot(0, 3, 120, 4, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 400),
                        new EntityHealthTransition(20, "LH Marble Colossus", 0.35f, "R-S-4"),
                        new TimedRandomTransition(5400, false, "R-0-B", "R-1-B", "R-2-B", "R-3-B", "R-5-B", "R-6-B", "R-7-B")
                        ),
                    new State("R-5-B",
                        new SetAltTexture(5),
                        new Orbit(0.6f, 1, 20, "LH Marble Colossus Anchor", 0.5f),
                        new Shoot(0, 3, 120, 5, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 400),
                        new EntityHealthTransition(20, "LH Marble Colossus", 0.35f, "R-S-5"),
                        new TimedRandomTransition(5400, false, "R-0-B", "R-1-B", "R-2-B", "R-3-B", "R-4-B", "R-6-B", "R-7-B")
                        ),
                    new State("R-6-B",
                        new SetAltTexture(6),
                        new Orbit(0.6f, 1, 20, "LH Marble Colossus Anchor", 0.5f),
                        new Shoot(0, 3, 120, 6, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 400),
                        new EntityHealthTransition(20, "LH Marble Colossus", 0.35f, "R-S-6"),
                        new TimedRandomTransition(5400, false, "R-0-B", "R-1-B", "R-2-B", "R-3-B", "R-4-B", "R-5-B", "R-7-B")
                        ),
                    new State("R-7-B",
                        new SetAltTexture(7),
                        new Orbit(0.6f, 1, 20, "LH Marble Colossus Anchor", 0.5f),
                        new Shoot(0, 3, 120, 7, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 400),
                        new EntityHealthTransition(20, "LH Marble Colossus", 0.35f, "R-S-7"),
                        new TimedRandomTransition(5400, false, "R-0-B", "R-1-B", "R-2-B", "R-3-B", "R-4-B", "R-5-B", "R-6-B")
                        ),
                    new State("R-S-0",
                        new Flash(0x000000, 0.20f, 3),
                        new Shoot(0, 8, 45, 0, cooldownOffset: 600, cooldown: 200),
                        new Decay(600)
                        ),
                    new State("R-S-1",
                        new Flash(0x000000, 0.20f, 3),
                        new Shoot(0, 8, 45, 1, cooldownOffset: 600, cooldown: 200),
                        new Decay(600)
                        ),
                    new State("R-S-2",
                        new Flash(0x000000, 0.20f, 3),
                        new Shoot(0, 8, 45, 2, cooldownOffset: 600, cooldown: 200),
                        new Decay(600)
                        ),
                    new State("R-S-3",
                        new Flash(0x000000, 0.20f, 3),
                        new Shoot(0, 8, 45, 3, cooldownOffset: 600, cooldown: 200),
                        new Decay(600)
                        ),
                    new State("R-S-4",
                        new Flash(0x000000, 0.20f, 3),
                        new Shoot(0, 8, 45, 4, cooldownOffset: 600, cooldown: 200),
                        new Decay(600)
                        ),
                    new State("R-S-5",
                        new Flash(0x000000, 0.20f, 3),
                        new Shoot(0, 8, 45, 5, cooldownOffset: 600, cooldown: 200),
                        new Decay(600)
                        ),
                    new State("R-S-6",
                        new Flash(0x000000, 0.20f, 3),
                        new Shoot(0, 8, 45, 6, cooldownOffset: 600, cooldown: 200),
                        new Decay(600)
                        ),
                    new State("R-S-7",
                        new Flash(0x000000, 0.20f, 3),
                        new Shoot(0, 8, 45, 7, cooldownOffset: 600, cooldown: 200),
                        new Decay(600)
                        )
                    )
            )
            db.Init("LH Colossus Rock 13",
                new State("base",
                    new OnParentDeathTransition("R-13-S"),
                    new State("R-TR-5",
                        new TimedRandomTransition(0, false, "R-0", "R-1", "R-2", "R-3", "R-4", "R-5", "R-6", "R-7")
                        ),
                    new State("R-0",
                        new SetAltTexture(0),
                        new Orbit(0.6f, 1, 20, "LH Marble Colossus Anchor", 0.5f),
                        new Shoot(0, 4, 90, 0, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 400),
                        new EntitiesNotExistsTransition(20, "R-S-0", "LH Marble Core 6", "LH Marble Core 7", "LH Marble Core 8"),
                        new TimedRandomTransition(5400, false, "R-1", "R-2", "R-3", "R-4", "R-5", "R-6", "R-7")
                        ),
                    new State("R-1",
                        new SetAltTexture(1),
                        new Orbit(0.6f, 1, 20, "LH Marble Colossus Anchor", 0.5f),
                        new Shoot(0, 4, 90, 1, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 400),
                        new EntitiesNotExistsTransition(20, "R-S-1", "LH Marble Core 6", "LH Marble Core 7", "LH Marble Core 8"),
                        new TimedRandomTransition(5400, false, "R-0", "R-2", "R-3", "R-4", "R-5", "R-6", "R-7")
                        ),
                    new State("R-2",
                        new SetAltTexture(2),
                        new Orbit(0.6f, 1, 20, "LH Marble Colossus Anchor", 0.5f),
                        new Shoot(0, 4, 90, 2, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 400),
                        new EntitiesNotExistsTransition(20, "R-S-2", "LH Marble Core 6", "LH Marble Core 7", "LH Marble Core 8"),
                        new TimedRandomTransition(5400, false, "R-0", "R-1", "R-3", "R-4", "R-5", "R-6", "R-7")
                        ),
                    new State("R-3",
                        new SetAltTexture(3),
                        new Orbit(0.6f, 1, 20, "LH Marble Colossus Anchor", 0.5f),
                        new Shoot(0, 4, 90, 3, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 400),
                        new EntitiesNotExistsTransition(20, "R-S-3", "LH Marble Core 6", "LH Marble Core 7", "LH Marble Core 8"),
                        new TimedRandomTransition(5400, false, "R-0", "R-1", "R-2", "R-4", "R-5", "R-6", "R-7")
                        ),
                    new State("R-4",
                        new SetAltTexture(4),
                        new Orbit(0.6f, 1, 20, "LH Marble Colossus Anchor", 0.5f),
                        new Shoot(0, 4, 90, 4, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 400),
                        new EntitiesNotExistsTransition(20, "R-S-4", "LH Marble Core 6", "LH Marble Core 7", "LH Marble Core 8"),
                        new TimedRandomTransition(5400, false, "R-0", "R-1", "R-2", "R-3", "R-5", "R-6", "R-7")
                        ),
                    new State("R-5",
                        new SetAltTexture(5),
                        new Orbit(0.6f, 1, 20, "LH Marble Colossus Anchor", 0.5f),
                        new Shoot(0, 4, 90, 5, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 400),
                        new EntitiesNotExistsTransition(20, "R-S-5", "LH Marble Core 6", "LH Marble Core 7", "LH Marble Core 8"),
                        new TimedRandomTransition(5400, false, "R-0", "R-1", "R-2", "R-3", "R-4", "R-6", "R-7")
                        ),
                    new State("R-6",
                        new SetAltTexture(6),
                        new Orbit(0.6f, 1, 20, "LH Marble Colossus Anchor", 0.5f),
                        new Shoot(0, 4, 90, 6, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 400),
                        new EntitiesNotExistsTransition(20, "R-S-6", "LH Marble Core 6", "LH Marble Core 7", "LH Marble Core 8"),
                        new TimedRandomTransition(5400, false, "R-0", "R-1", "R-2", "R-3", "R-4", "R-5", "R-7")
                        ),
                    new State("R-7",
                        new SetAltTexture(7),
                        new Orbit(0.6f, 1, 20, "LH Marble Colossus Anchor", 0.5f),
                        new Shoot(0, 4, 90, 7, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 400),
                        new EntitiesNotExistsTransition(20, "R-S-7", "LH Marble Core 6", "LH Marble Core 7", "LH Marble Core 8"),
                        new TimedRandomTransition(5400, false, "R-0", "R-1", "R-2", "R-3", "R-4", "R-5", "R-6")
                        ),
                    new State("R-S-0",
                        new Flash(0x000000, 0.20f, 3),
                        new Shoot(0, 8, 45, 0, cooldownOffset: 600, cooldown: 200),
                        new Decay(600)
                        ),
                    new State("R-S-1",
                        new Flash(0x000000, 0.20f, 3),
                        new Shoot(0, 8, 45, 1, cooldownOffset: 600, cooldown: 200),
                        new Decay(600)
                        ),
                    new State("R-S-2",
                        new Flash(0x000000, 0.20f, 3),
                        new Shoot(0, 8, 45, 2, cooldownOffset: 600, cooldown: 200),
                        new Decay(600)
                        ),
                    new State("R-S-3",
                        new Flash(0x000000, 0.20f, 3),
                        new Shoot(0, 8, 45, 3, cooldownOffset: 600, cooldown: 200),
                        new Decay(600)
                        ),
                    new State("R-S-4",
                        new Flash(0x000000, 0.20f, 3),
                        new Shoot(0, 8, 45, 4, cooldownOffset: 600, cooldown: 200),
                        new Decay(600)
                        ),
                    new State("R-S-5",
                        new Flash(0x000000, 0.20f, 3),
                        new Shoot(0, 8, 45, 5, cooldownOffset: 600, cooldown: 200),
                        new Decay(600)
                        ),
                    new State("R-S-6",
                        new Flash(0x000000, 0.20f, 3),
                        new Shoot(0, 8, 45, 6, cooldownOffset: 600, cooldown: 200),
                        new Decay(600)
                        ),
                    new State("R-S-7",
                        new Flash(0x000000, 0.20f, 3),
                        new Shoot(0, 8, 45, 7, cooldownOffset: 600, cooldown: 200),
                        new Decay(600)
                        )
                    )
            )
            db.Init("LH Colossus Rock 14",
                new State("base",
                    new OnParentDeathTransition("R-S"),
                    new State("R-TR-5",
                        new TimedRandomTransition(0, false, "R-0", "R-1", "R-2", "R-3", "R-4", "R-5", "R-6", "R-7")
                        ),
                    new State("R-0",
                        new SetAltTexture(0),
                        new Orbit(0.6f, 1, 20, "LH Marble Colossus Anchor", 0.5f),
                        new Shoot(0, 4, 90, 0, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 400),
                        new EntitiesNotExistsTransition(20, "R-S-0", "LH Marble Core 6", "LH Marble Core 7", "LH Marble Core 8"),
                        new TimedRandomTransition(5400, false, "R-1", "R-2", "R-3", "R-4", "R-5", "R-6", "R-7")
                        ),
                    new State("R-1",
                        new SetAltTexture(1),
                        new Orbit(0.6f, 1, 20, "LH Marble Colossus Anchor", 0.5f),
                        new Shoot(0, 4, 90, 1, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 400),
                        new EntitiesNotExistsTransition(20, "R-S-1", "LH Marble Core 6", "LH Marble Core 7", "LH Marble Core 8"),
                        new TimedRandomTransition(5400, false, "R-0", "R-2", "R-3", "R-4", "R-5", "R-6", "R-7")
                        ),
                    new State("R-2",
                        new SetAltTexture(2),
                        new Orbit(0.6f, 1, 20, "LH Marble Colossus Anchor", 0.5f),
                        new Shoot(0, 4, 90, 2, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 400),
                        new EntitiesNotExistsTransition(20, "R-S-2", "LH Marble Core 6", "LH Marble Core 7", "LH Marble Core 8"),
                        new TimedRandomTransition(5400, false, "R-0", "R-1", "R-3", "R-4", "R-5", "R-6", "R-7")
                        ),
                    new State("R-3",
                        new SetAltTexture(3),
                        new Orbit(0.6f, 1, 20, "LH Marble Colossus Anchor", 0.5f),
                        new Shoot(0, 4, 90, 3, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 400),
                        new EntitiesNotExistsTransition(20, "R-S-3", "LH Marble Core 6", "LH Marble Core 7", "LH Marble Core 8"),
                        new TimedRandomTransition(5400, false, "R-0", "R-1", "R-2", "R-4", "R-5", "R-6", "R-7")
                        ),
                    new State("R-4",
                        new SetAltTexture(4),
                        new Orbit(0.6f, 1, 20, "LH Marble Colossus Anchor", 0.5f),
                        new Shoot(0, 4, 90, 4, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 400),
                        new EntitiesNotExistsTransition(20, "R-S-4", "LH Marble Core 6", "LH Marble Core 7", "LH Marble Core 8"),
                        new TimedRandomTransition(5400, false, "R-0", "R-1", "R-2", "R-3", "R-5", "R-6", "R-7")
                        ),
                    new State("R-5",
                        new SetAltTexture(5),
                        new Orbit(0.6f, 1, 20, "LH Marble Colossus Anchor", 0.5f),
                        new Shoot(0, 4, 90, 5, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 400),
                        new EntitiesNotExistsTransition(20, "R-S-5", "LH Marble Core 6", "LH Marble Core 7", "LH Marble Core 8"),
                        new TimedRandomTransition(5400, false, "R-0", "R-1", "R-2", "R-3", "R-4", "R-6", "R-7")
                        ),
                    new State("R-6",
                        new SetAltTexture(6),
                        new Orbit(0.6f, 1, 20, "LH Marble Colossus Anchor", 0.5f),
                        new Shoot(0, 4, 90, 6, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 400),
                        new EntitiesNotExistsTransition(20, "R-S-6", "LH Marble Core 6", "LH Marble Core 7", "LH Marble Core 8"),
                        new TimedRandomTransition(5400, false, "R-0", "R-1", "R-2", "R-3", "R-4", "R-5", "R-7")
                        ),
                    new State("R-7",
                        new SetAltTexture(7),
                        new Orbit(0.6f, 1, 20, "LH Marble Colossus Anchor", 0.5f),
                        new Shoot(0, 4, 90, 7, rotateAngle: 10, defaultAngle: 0, cooldownOffset: 200, cooldown: 400),
                        new EntitiesNotExistsTransition(20, "R-S-7", "LH Marble Core 6", "LH Marble Core 7", "LH Marble Core 8"),
                        new TimedRandomTransition(5400, false, "R-0", "R-1", "R-2", "R-3", "R-4", "R-5", "R-6")
                        ),
                    new State("R-S-0",
                        new Flash(0x000000, 0.20f, 3),
                        new Shoot(0, 8, 45, 0, cooldownOffset: 600, cooldown: 200),
                        new Decay(600)
                        ),
                    new State("R-S-1",
                        new Flash(0x000000, 0.20f, 3),
                        new Shoot(0, 8, 45, 1, cooldownOffset: 600, cooldown: 200),
                        new Decay(600)
                        ),
                    new State("R-S-2",
                        new Flash(0x000000, 0.20f, 3),
                        new Shoot(0, 8, 45, 2, cooldownOffset: 600, cooldown: 200),
                        new Decay(600)
                        ),
                    new State("R-S-3",
                        new Flash(0x000000, 0.20f, 3),
                        new Shoot(0, 8, 45, 3, cooldownOffset: 600, cooldown: 200),
                        new Decay(600)
                        ),
                    new State("R-S-4",
                        new Flash(0x000000, 0.20f, 3),
                        new Shoot(0, 8, 45, 4, cooldownOffset: 600, cooldown: 200),
                        new Decay(600)
                        ),
                    new State("R-S-5",
                        new Flash(0x000000, 0.20f, 3),
                        new Shoot(0, 8, 45, 5, cooldownOffset: 600, cooldown: 200),
                        new Decay(600)
                        ),
                    new State("R-S-6",
                        new Flash(0x000000, 0.20f, 3),
                        new Shoot(0, 8, 45, 6, cooldownOffset: 600, cooldown: 200),
                        new Decay(600)
                        ),
                    new State("R-S-7",
                        new Flash(0x000000, 0.20f, 3),
                        new Shoot(0, 8, 45, 7, cooldownOffset: 600, cooldown: 200),
                        new Decay(600)
                        )
                    )
            )
            db.Init("LH Marble Core 1",
            new State("base",
                new State("base",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 2000),
                    new Orbit(0.5f, 8, 20, "LH Marble Colossus Anchor")
                    )
                )
            )
            db.Init("LH Marble Core 2",
            new State("base",
                new State("base",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 2000),
                    new Orbit(0.5f, 8, 20, "LH Marble Colossus Anchor", orbitClockwise: true)
                    )
                )
            )
            db.Init("LH Marble Core 3",
            new State("base",
                new State("base",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 2000),
                    new Orbit(0.5f, 8, 20, "LH Marble Colossus Anchor")
                    )
                )
            )
            db.Init("LH Marble Core 4",
            new State("base",
                new State("base",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 2000),
                    new Orbit(0.5f, 8, 20, "LH Marble Colossus Anchor", orbitClockwise: true)
                    )
                )
            )
            db.Init("LH Marble Core 5",
            new State("base",
                new State("base",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 2000),
                    new Orbit(0.3f, 2, 20, "LH Marble Colossus", orbitClockwise: true)
                    )
                )
            )
            db.Init("LH Marble Core 6",
            new State("base",
                new State("base",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 2000),
                    new Orbit(0.5f, 11, 20, "LH Marble Colossus Anchor", orbitClockwise: true)
                    )
                )
            )
            db.Init("LH Marble Core 7",
                new State("base",
                    new State("base",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 2000),
                        new Orbit(0.5f, 9, 20, "LH Marble Colossus Anchor")
                        )
                    )
            )
            db.Init("LH Marble Core 8",
            new State("base",
                new State("base",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 2000),
                    new Orbit(0.5f, 7, 20, "LH Marble Colossus Anchor", orbitClockwise: true)
                    )
                )
            )
            db.Init("LH Marble Colossus Laser A",
                new State("base",
                    new Shoot(0, 6, 60, 0, rotateAngle: 3, defaultAngle: 0, cooldownOffset: 200, cooldown: 200),
                    new EntityHealthTransition(5,"LH Marble Colossus", 0.75f, "L-S"),
                    new State("L-S",
                        new Suicide()
                        )
                    )
            )
            db.Init("LH Marble Colossus Laser B",
                new State("base",
                    new EntitiesNotExistsTransition(15, "L-S", "LH Marble Core 6", "LH Marble Core 7", "LH Marble Core 8"),
                    new State("C-W",
                        new Shoot(0, 6, 60, 0, rotateAngle: 3, defaultAngle: 60, cooldown: 200),
                        new TimedTransition("C-C-W", 5000)
                        ),
                    new State("C-C-W",
                        new Shoot(0, 6, 60, 0, rotateAngle: -3, defaultAngle: 60, cooldown: 200),
                        new TimedTransition("C-W", 5000)
                        ),
                    new State("L-S",
                        new Suicide()
                        )
                    )
            )
            ;
    }
}
