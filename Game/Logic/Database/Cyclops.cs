﻿using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.Database
{
    class Cyclops : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {

            db.Init("Cyclops God",
                new State("base",
                    new DropPortalOnDeath("Crawling Depths Portal", 0.1f),
                    new State("idle",
                        new PlayerWithinTransition(11, "blade_attack"),
                        new HealthTransition(0.8f, "blade_attack")
                        ),
                    new State("blade_attack",
                        new Prioritize(
                            new Follow(0.4f, range: 7),
                            new Wander(0.4f)
                            ),
                        new Shoot(10, index: 4, count: 1, shootAngle: 15, predictive: 0.5f, cooldown: 100000),
                        new Shoot(10, index: 4, count: 2, shootAngle: 10, predictive: 0.5f, cooldown: 100000,
                            cooldownOffset: 700),
                        new Shoot(10, index: 4, count: 3, shootAngle: 8.5f, predictive: 0.5f, cooldown: 100000,
                            cooldownOffset: 1400),
                        new Shoot(10, index: 4, count: 4, shootAngle: 7, predictive: 0.5f, cooldown: 100000,
                            cooldownOffset: 2100),
                        new TimedTransition("if_cloaked1", 4000)
                        ),
                    new State("if_cloaked1",
                        new Shoot(10, index: 4, count: 15, shootAngle: 24, fixedAngle: 8, cooldown: 1500,
                            cooldownOffset: 400),
                        new TimedTransition("wave_attack", 10000),
                        new PlayerWithinTransition(10.5f, "wave_attack")
                        ),
                    new State("wave_attack",
                        new Prioritize(
                            new Follow(0.6f, range: 5),
                            new Wander(0.6f)
                            ),
                        new Shoot(9, index: 0, cooldown: 700, cooldownOffset: 700),
                        new Shoot(9, index: 1, cooldown: 700, cooldownOffset: 700),
                        new Shoot(9, index: 2, cooldown: 700, cooldownOffset: 700),
                        new Shoot(9, index: 3, cooldown: 700, cooldownOffset: 700),
                        new TimedTransition("if_cloaked2", 3800)
                        ),
                    new State("if_cloaked2",
                        new Shoot(10, index: 4, count: 15, shootAngle: 24, fixedAngle: 8, cooldown: 1500,
                            cooldownOffset: 400),
                        new TimedTransition("idle", 10000),
                        new PlayerWithinTransition(10.5f, "idle")
                        ),
                    new Taunt(0.7f, 10000, "I will floss with your tendons!",
                        "I smell the blood of an Englishman!",
                        "I will suck the marrow from your bones!",
                        "You will be my food, {PLAYER}!",
                        "Blargh!!",
                        "Leave my castle!",
                        "More wine!"
                        ),
                    new StayCloseToSpawn(1.2f, 5),
                    new Spawn("Cyclops", maxChildren: 5, cooldown: 10000, givesNoXp: false),
                    new Spawn("Cyclops Warrior", maxChildren: 5, cooldown: 10000, givesNoXp: false),
                    new Spawn("Cyclops Noble", maxChildren: 5, cooldown: 10000, givesNoXp: false),
                    new Spawn("Cyclops Prince", maxChildren: 5, cooldown: 10000, givesNoXp: false),
                    new Spawn("Cyclops King", maxChildren: 5, cooldown: 10000, givesNoXp: false)
                    )
            );
            db.Init("Cyclops",
                new State("base",
                    new Prioritize(
                        new StayCloseToSpawn(1.2f, 5),
                        new Follow(1.2f, range: 1),
                        new Wander(0.4f)
                        ),
                    new Shoot(3)
                    ),
                new ItemLoot("Health Potion", 0.05f)
            );
            db.Init("Cyclops Warrior",
                new State("base",
                    new Prioritize(
                        new StayCloseToSpawn(1.2f, 5),
                        new Follow(1.2f, range: 1),
                        new Wander(0.4f)
                        ),
                    new Shoot(3)
                    ),
                new ItemLoot("Health Potion", 0.05f)
            );

            db.Init("Cyclops Noble",
                new State("base",
                    
                    new Prioritize(
                        new StayCloseToSpawn(1.2f, 5),
                        new Follow(1.2f, range: 1),
                        new Wander(0.4f)
                        ),
                    new Shoot(3)
                    ),
                new ItemLoot("Health Potion", 0.05f)
            );
            db.Init("Cyclops Prince",
                new State("base",
                    new Prioritize(
                        new StayCloseToSpawn(1.2f, 5),
                        new Follow(1.2f, range: 1),
                        new Wander(0.4f)
                        ),
                    new Shoot(3)
                    ),
                new ItemLoot("Mithril Dagger", 0.02f),
                new ItemLoot("Seal of the Divine", 0.01f),
                new ItemLoot("Health Potion", 0.05f)
            );
            db.Init("Cyclops King",
                new State("base",
                    new Prioritize(
                        new StayCloseToSpawn(1.2f, 5),
                        new Follow(1.2f, range: 1),
                        new Wander(0.4f)
                        ),
                    new Shoot(3)
                    ),
                new ItemLoot("Mithril Armor", 0.2f),
                new ItemLoot("Health Potion", 0.5f),
                new ItemLoot("Potion of Defense", 0.1f),
                new ItemLoot("Potion of Dexterity", 0.1f),
                new ItemLoot("Loins of the Cyclops God", 0.0005f, 0.01f),
                new ItemLoot("Cyclops Mace", 0.01f, 0.01f),
                new ItemLoot("Ancient Crown", 0.01f, 0.01f),
                new ItemLoot("Seal of Cyclops Command", 0.01f, 0.01f),
                new ItemLoot("Head of the Cyclops King", 0.01f, 0.01f)
            );

        }
    }
}
