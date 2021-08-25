using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Game.SetPieces;
using RotMG.Utils;

namespace RotMG.Game.Worlds
{
    public partial class Realm
    {
        private struct TauntData
        {
            public string[] Spawn;
            public string[] NumberOfEnemies;
            public string[] Final;
            public string[] Killed;
        }
        
        #region "Taunt data"
        private static readonly Dictionary<string, TauntData> CriticalEnemies = 
            new Dictionary<string, TauntData>
        {
            {
                "Lich", new TauntData()
                {
                    NumberOfEnemies = new[]
                    {
                        "I am invincible while my {COUNT} Liches still stand!",
                        "My {COUNT} Liches will feast on your essence!"
                    },
                    Final = new[]
                    {
                        "My final Lich shall consume your souls!",
                        "My final Lich will protect me forever!"
                    }
                }
            },
            {
                "Ent Ancient", new TauntData()
                {
                    NumberOfEnemies = new[]
                    {
                        "Mortal scum! My {COUNT} Ent Ancients will defend me forever!",
                        "My forest of {COUNT} Ent Ancients is all the protection I need!"
                    },
                    Final = new[]
                    {
                        "My final Ent Ancient will destroy you all!",
                        "My final Ent Ancient shall crush you!"
                    }
                }
            },
            {
                "Oasis Giant", new TauntData()
                {
                    NumberOfEnemies = new[]
                    {
                        "My {COUNT} Oasis Giants will feast on your flesh!",
                        "You have no hope against my {COUNT} Oasis Giants!"
                    },
                    Final = new[]
                    {
                        "A powerful Oasis Giant still fights for me!",
                        "You will never defeat me while an Oasis Giant remains!"
                    }
                }
            },
            {
                "Phoenix Lord", new TauntData()
                {
                    NumberOfEnemies = new[]
                    {
                        "Maggots! My {COUNT} Phoenix Lord will burn you to ash!",
                        "My {COUNT} Phoenix Lords will serve me forever!"
                    },
                    Final = new[]
                    {
                        "My final Phoenix Lord will never fall!",
                        "My last Phoenix Lord will blacken your bones!"
                    }
                }
            },
            {
                "Ghost King", new TauntData()
                {
                    NumberOfEnemies = new[]
                    {
                        "My {COUNT} Ghost Kings give me more than enough protection!",
                        "Pathetic humans! My {COUNT} Ghost Kings shall destroy you utterly!"
                    },
                    Final = new[]
                    {
                        "A mighty Ghost King remains to guard me!",
                        "My final Ghost King is untouchable!"
                    }
                }
            },
            {
                "Cyclops God", new TauntData()
                {
                    NumberOfEnemies = new[]
                    {
                        "Cretins! I have {COUNT} Cyclops Gods to guard me!",
                        "My {COUNT} powerful Cyclops Gods will smash you!"
                    },
                    Final = new[]
                    {
                        "My last Cyclops God will smash you to pieces!",
                        "My final Cyclops God shall crush your puny skulls!"
                    }
                }
            },
            {
                "Red Demon", new TauntData()
                {
                    NumberOfEnemies = new[]
                    {
                        "Fools! There is no escape from my {COUNT} Red Demons!",
                        "My legion of {COUNT} Red Demons live only to serve me!"
                    },
                    Final = new[]
                    {
                        "My final Red Demon is unassailable!",
                        "A Red Demon still guards me!"
                    }
                }
            },

            {
                "Skull Shrine", new TauntData()
                {
                    Spawn = new[]
                    {
                        "Your futile efforts are no match for a Skull Shrine!"
                    },
                    NumberOfEnemies = new[]
                    {
                        "Insects!  {COUNT} Skull Shrines still protect me",
                        "You hairless apes will never overcome my {COUNT} Skull Shrines!",
                        "You frail humans will never defeat my {COUNT} Skull Shrines!",
                        "Miserable worms like you cannot stand against my {COUNT} Skull Shrines!",
                        "Imbeciles! My {COUNT} Skull Shrines make me invincible!"
                    },
                    Final = new[]
                    {
                        "Pathetic fools!  A Skull Shrine guards me!",
                        "Miserable scum!  My Skull Shrine is invincible!"
                    },
                    Killed = new[]
                    {
                        "You defaced a Skull Shrine!  Minions, to arms!",
                        "{PLAYER} razed one of my Skull Shrines -- I WILL HAVE MY REVENGE!",
                        "{PLAYER}, you will rue the day you dared to defile my Skull Shrine!",
                        "{PLAYER}, you contemptible pig! Ruining my Skull Shrine will be the last mistake you ever make!",
                        "{PLAYER}, you insignificant cur! The penalty for destroying a Skull Shrine is death!"
                    }
                }
            },
            {
                "Cube God", new TauntData()
                {
                    Spawn = new[]
                    {
                        "Your meager abilities cannot possibly challenge a Cube God!"
                    },
                    NumberOfEnemies = new[]
                    {
                        "Filthy vermin! My {COUNT} Cube Gods will exterminate you!",
                        "Loathsome slugs! My {COUNT} Cube Gods will defeat you!",
                        "You piteous cretins! {COUNT} Cube Gods still guard me!",
                        "Your pathetic rabble will never survive against my {COUNT} Cube Gods!",
                        "You feeble creatures have no hope against my {COUNT} Cube Gods!"
                    },
                    Final = new[]
                    {
                        "Worthless mortals! A mighty Cube God defends me!",
                        "Wretched mongrels!  An unconquerable Cube God is my bulwark!"
                    },
                    Killed = new[]
                    {
                        "You have dispatched my Cube God, but you will never escape my Realm!",
                        "{PLAYER}, you pathetic swine! How dare you assault my Cube God?",
                        "{PLAYER}, you wretched dog! You killed my Cube God!",
                        "{PLAYER}, you may have destroyed my Cube God but you will never defeat me!",
                        "I have many more Cube Gods, {PLAYER}!",
                    }
                }
            },
            {
                "Pentaract", new TauntData()
                {
                    Spawn = new[]
                    {
                        "Behold my Pentaract, and despair!"
                    },
                    NumberOfEnemies = new[]
                    {
                        "Wretched creatures! {COUNT} Pentaracts remain!",
                        "You detestable humans will never defeat my {COUNT} Pentaracts!",
                        "My {COUNT} Pentaracts will protect me forever!",
                        "Your weak efforts will never overcome my {COUNT} Pentaracts!",
                        "Defiance is useless! My {COUNT} Pentaracts will crush you!"
                    },
                    Final = new[]
                    {
                        "I am invincible while my Pentaract stands!",
                        "Ignorant fools! A Pentaract guards me still!"
                    },
                    Killed = new[]
                    {
                        "That was but one of many Pentaracts!",
                        "You have razed my Pentaract, but you will die here in my Realm!",
                        "{PLAYER}, you lowly scum!  You'll regret that you ever touched my Pentaract!",
                        "{PLAYER}, you flea-ridden animal! You destoryed my Pentaract!",
                        "{PLAYER}, by destroying my Pentaract you have sealed your own doom!"
                    }
                }
            },
            {
                "Grand Sphinx", new TauntData()
                {
                    Spawn = new[]
                    {
                        "At last, a Grand Sphinx will teach you to respect!"
                    },
                    NumberOfEnemies = new[]
                    {
                        "You dull-spirited apes! You shall pose no challenge for {COUNT} Grand Sphinxes!",
                        "Regret your choices, blasphemers! My {COUNT} Grand Sphinxes will teach you respect!",
                        "My {COUNT} Grand Sphinxes protect my Chamber with their lives!",
                        "My Grand Sphinxes will bewitch you with their beauty!"
                    },
                    Final = new[]
                    {
                        "A Grand Sphinx is more than a match for this rabble.",
                        "You festering rat-catchers! A Grand Sphinx will make you doubt your purpose!",
                        "Gaze upon the beauty of the Grand Sphinx and feel your last hopes drain away."
                    },
                    Killed = new[]
                    {
                        "The death of my Grand Sphinx shall be avenged!",
                        "My Grand Sphinx, she was so beautiful. I will kill you myself, {PLAYER}!",
                        "My Grand Sphinx had lived for thousands of years! You, {PLAYER}, will not survive the day!",
                        "{PLAYER}, you up-jumped goat herder! You shall pay for defeating my Grand Sphinx!",
                        "{PLAYER}, you pestiferous lout! I will not forget what you did to my Grand Sphinx!",
                        "{PLAYER}, you foul ruffian! Do not think I forget your defiling of my Grand Sphinx!"
                    }
                }
            },
            {
                "Lord of the Lost Lands", new TauntData()
                {
                    Spawn = new[]
                    {
                        "Cower in fear of my Lord of the Lost Lands!",
                        "My Lord of the Lost Lands will make short work of you!"
                    },
                    NumberOfEnemies = new[]
                    {
                        "Cower before your destroyer! You stand no chance against {COUNT} Lords of the Lost Lands!",
                        "Your pathetic band of fighters will be crushed under the might feet of my {COUNT} Lords of the Lost Lands!",
                        "Feel the awesome might of my {COUNT} Lords of the Lost Lands!",
                        "Together, my {COUNT} Lords of the Lost Lands will squash you like a bug!",
                        "Do not run! My {COUNT} Lords of the Lost Lands only wish to greet you!"
                    },
                    Final = new[]
                    {
                        "Give up now! You stand no chance against a Lord of the Lost Lands!",
                        "Pathetic fools! My Lord of the Lost Lands will crush you all!",
                        "You are nothing but disgusting slime to be scraped off the foot of my Lord of the Lost Lands!"
                    },
                    Killed = new[]
                    {
                        "How dare you foul-mouthed hooligans treat my Lord of the Lost Lands with such indignity!",
                        "What trickery is this?! My Lord of the Lost Lands was invincible!",
                        "You win this time, {PLAYER}, but mark my words:  You will fall before the day is done.",
                        "{PLAYER}, I will never forget you exploited my Lord of the Lost Lands' weakness!",
                        "{PLAYER}, you have done me a service! That Lord of the Lost Lands was not worthy of serving me.",
                        "You got lucky this time {PLAYER}, but you stand no chance against me!",
                    }
                }
            },
            {
                "Hermit God", new TauntData()
                {
                    Spawn = new[]
                    {
                        "My Hermit God's thousand tentacles shall drag you to a watery grave!"
                    },
                    NumberOfEnemies = new[]
                    {
                        "You will make a tasty snack for my Hermit Gods!",
                        "I will enjoy watching my {COUNT} Hermit Gods fight over your corpse!"
                    },
                    Final = new[]
                    {
                        "You will be pulled to the bottom of the sea by my mighty Hermit God.",
                        "Flee from my Hermit God, unless you desire a watery grave!",
                        "My Hermit God awaits more sacrifices for the majestic Thessal.",
                        "My Hermit God will pull you beneath the waves!",
                        "You will make a tasty snack for my Hermit God!",
                    },
                    Killed = new[]
                    {
                        "This is preposterous!  There is no way you could have defeated my Hermit God!",
                        "You were lucky this time, {PLAYER}!  You will rue this day that you killed my Hermit God!",
                        "You naive imbecile, {PLAYER}! Without my Hermit God, Dreadstump is free to roam the seas without fear!",
                        "My Hermit God was more than you'll ever be, {PLAYER}. I will kill you myself!",
                    }
                }
            },
            {
                "Ghost Ship", new TauntData()
                {
                    Spawn = new[]
                    {
                        "My Ghost Ship will terrorize you pathetic peasants!",
                        "A Ghost Ship has entered the Realm."
                    },
                    Final = new[]
                    {
                        "My Ghost Ship will send you to a watery grave.",
                        "You filthy mongrels stand no chance against my Ghost Ship!",
                        "My Ghost Ship's cannonballs will crush your pathetic Knights!"
                    },
                    Killed = new[]
                    {
                        "My Ghost Ship will return!",
                        "Alas, my beautiful Ghost Ship has sunk!",
                        "{PLAYER}, you foul creature.  I shall see to your death personally!",
                        "{PLAYER}, has crossed me for the last time! My Ghost Ship shall be avenged.",
                        "{PLAYER} is such a jerk!",
                        "How could a creature like {PLAYER} defeat my dreaded Ghost Ship?!",
                        "The spirits of the sea will seek revenge on your worthless soul, {PLAYER}!"
                    }
                }
            },
            {
                "Dragon Head", new TauntData()
                {
                    Spawn = new[]
                    {
                        "The Rock Dragon has been summoned.",
                        "Beware my Rock Dragon. All who face him shall perish.",
                    },
                    Final = new[]
                    {
                        "My Rock Dragon will end your pathetic existence!",
                        "Fools, no one can withstand the power of my Rock Dragon!",
                        "The Rock Dragon will guard his post until the bitter end.",
                        "The Rock Dragon will never let you enter the Lair of Draconis.",
                    },
                    Killed = new[]
                    {
                        "My Rock Dragon will return!",
                        "The Rock Dragon has failed me!",
                        "{PLAYER} knows not what he has done.  That Lair was guarded for the Realm's own protection!",
                        "{PLAYER}, you have angered me for the last time!",
                        "{PLAYER} will never survive the trials that lie ahead.",
                        "A filthy weakling like {PLAYER} could never have defeated my Rock Dragon!!!",
                        "You shall not live to see the next sunrise, {PLAYER}!",
                    }
                }
            },
            {
                "shtrs Defense System", new TauntData()
                {
                    Spawn = new[]
                    {
                        "The Shatters has been discovered!?!",
                        "The Forgotten King has raised his Avatar!",
                    },
                    Final = new[]
                    {
                        "Attacking the Avatar of the Forgotten King would be...unwise.",
                        "Kill the Avatar, and you risk setting free an abomination.",
                        "Before you enter the Shatters you must defeat the Avatar of the Forgotten King!",
                    },
                    Killed = new[]
                    {
                        "The Avatar has been defeated!",
                        "How could simpletons kill The Avatar of the Forgotten King!?",
                        "{PLAYER} has unleashed an evil upon this Realm.",
                        "{PLAYER}, you have awoken the Forgotten King. Enjoy a slow death!",
                        "{PLAYER} will never survive what lies in the depths of the Shatters.",
                        "Enjoy your little victory while it lasts, {PLAYER}!"
                    }
                }
            },
            {
                "Zombie Horde", new TauntData()
                {
                    Spawn = new[]
                    {
                        "At last, my Zombie Horde will eradicate you like the vermin that you are!",
                        "The full strength of my Zombie Horde has been unleashed!",
                        "Let the apocalypse begin!",
                        "Quiver with fear, peasants, my Zombie Horde has arrived!",
                    },
                    Final = new[]
                    {
                        "A small taste of my Zombie Horde should be enough to eliminate you!",
                        "My Zombie Horde will teach you the meaning of fear!",
                    },
                    Killed = new[]
                    {
                        "The death of my Zombie Horde is unacceptable! You will pay for your insolence!",
                        "{PLAYER}, I will kill you myself and turn you into the newest member of my Zombie Horde!",
                    }
                }
            }
        };
        #endregion
        #region "Spawn Data"
        private static readonly Dictionary<Terrain, Tuple<int, Tuple<string, double>[]>> RegionMobs =
            new Dictionary<Terrain, Tuple<int, Tuple<string, double>[]>>()
        {
            { Terrain.ShoreSand, Tuple.Create(
                100, new []
                {
                    Tuple.Create("Pirate", 0.3),
                    Tuple.Create("Piratess", 0.1),
                    Tuple.Create("Snake", 0.2),
                    Tuple.Create("Scorpion Queen", 0.4),
                })
            },
            { Terrain.ShorePlains, Tuple.Create(
                150, new []
                {
                    Tuple.Create("Bandit Leader", 0.4),
                    Tuple.Create("Red Gelatinous Cube", 0.2),
                    Tuple.Create("Purple Gelatinous Cube", 0.2),
                    Tuple.Create("Green Gelatinous Cube", 0.2),
                })
            },
            { Terrain.LowPlains, Tuple.Create(
                200, new []
                {
                    Tuple.Create("Hobbit Mage", 0.5),
                    Tuple.Create("Undead Hobbit Mage", 0.4),
                    Tuple.Create("Sumo Master", 0.1),
                })
            },
            { Terrain.LowForest, Tuple.Create(
                200, new []
                {
                    Tuple.Create("Elf Wizard", 0.2),
                    Tuple.Create("Goblin Mage", 0.2),
                    Tuple.Create("Easily Enraged Bunny", 0.3),
                    Tuple.Create("Forest Nymph", 0.3),
                })
            },
            { Terrain.LowSand, Tuple.Create(
                200, new []
                {
                    Tuple.Create("Sandsman King", 0.4),
                    Tuple.Create("Giant Crab", 0.2),
                    Tuple.Create("Sand Devil", 0.4),
                })
            },
            { Terrain.MidPlains, Tuple.Create(
                150, new []
                {
                    Tuple.Create("Fire Sprite", 0.1),
                    Tuple.Create("Ice Sprite", 0.1),
                    Tuple.Create("Magic Sprite", 0.1),
                    Tuple.Create("Pink Blob", 0.07),
                    Tuple.Create("Gray Blob", 0.07),
                    Tuple.Create("Earth Golem", 0.04),
                    Tuple.Create("Paper Golem", 0.04),
                    Tuple.Create("Big Green Slime", 0.08),
                    Tuple.Create("Swarm", 0.05),
                    Tuple.Create("Wasp Queen", 0.2),
                    Tuple.Create("Shambling Sludge", 0.03),
                    Tuple.Create("Orc King", 0.06),
                    Tuple.Create("Candy Gnome", 0.02)
                })
            },
            { Terrain.MidForest, Tuple.Create(
                150, new []
                {
                    Tuple.Create("Dwarf King", 0.3),
                    Tuple.Create("Metal Golem", 0.05),
                    Tuple.Create("Clockwork Golem", 0.05),
                    Tuple.Create("Werelion", 0.1),
                    Tuple.Create("Horned Drake", 0.3),
                    Tuple.Create("Red Spider", 0.1),
                    Tuple.Create("Black Bat", 0.1)
                })
            },
            { Terrain.MidSand, Tuple.Create(
                300, new []
                {
                    Tuple.Create("Desert Werewolf", 0.25),
                    Tuple.Create("Fire Golem", 0.1),
                    Tuple.Create("Darkness Golem", 0.1),
                    Tuple.Create("Sand Phantom", 0.2),
                    Tuple.Create("Nomadic Shaman", 0.25),
                    Tuple.Create("Great Lizard", 0.1),
                })
            },
            { Terrain.HighPlains, Tuple.Create(
                300, new []
                {
                    Tuple.Create("Shield Orc Key", 0.2),
                    Tuple.Create("Urgle", 0.2),
                    Tuple.Create("Undead Dwarf God", 0.6)
                })
            },
            { Terrain.HighForest, Tuple.Create(
                300, new []
                {
                    Tuple.Create("Ogre King", 0.4),
                    Tuple.Create("Dragon Egg", 0.1),
                    Tuple.Create("Lizard God", 0.5),
                    Tuple.Create("Beer God", 0.1)
                })
            },
            { Terrain.HighSand, Tuple.Create(
                250, new []
                {
                    Tuple.Create("Minotaur", 0.4),
                    Tuple.Create("Flayer God", 0.4),
                    Tuple.Create("Flamer King", 0.2)
                })
            },
            { Terrain.Mountains, Tuple.Create(
                100, new []
                {
                    Tuple.Create("White Demon", 0.1),
                    Tuple.Create("Sprite God", 0.11),
                    Tuple.Create("Medusa", 0.1),
                    Tuple.Create("Ent God", 0.1),
                    Tuple.Create("Beholder", 0.1),
                    Tuple.Create("Flying Brain", 0.1),
                    Tuple.Create("Slime God", 0.09),
                    Tuple.Create("Ghost God", 0.09),
                    Tuple.Create("Rock Bot", 0.05),
                    Tuple.Create("Djinn", 0.09),
                    Tuple.Create("Leviathan", 0.09),
                    Tuple.Create("Arena Headless Horseman", 0.04)
                })
            },
        };
        #endregion
        private readonly List<Tuple<string, ISetPiece>> _events = new List<Tuple<string, ISetPiece>>()
        {
            Tuple.Create("Skull Shrine", (ISetPiece) new SkullShrine()),
            Tuple.Create("Cube God", (ISetPiece) new CubeGod()),
            Tuple.Create("Pentaract", (ISetPiece) new Pentaract()),
            Tuple.Create("Grand Sphinx", (ISetPiece) new Sphinx()),
            Tuple.Create("Lord of the Lost Lands", (ISetPiece) new LordoftheLostLands()),
            Tuple.Create("Hermit God", (ISetPiece) new Hermit()),
            Tuple.Create("Ghost Ship", (ISetPiece) new GhostShip()),
        };

        //none of these count minions spawned
        private readonly Dictionary<Terrain, int> _enemyMaxCount = new Dictionary<Terrain, int>();
        private readonly Dictionary<Terrain, int> _enemyCount = new Dictionary<Terrain, int>();
        private readonly Dictionary<Terrain, List<Enemy>> _enemies = new Dictionary<Terrain, List<Enemy>>();
        private Dictionary<string, int> _criticalEnemyCounts;

        private void InitMobs()
        {
            _criticalEnemyCounts = SetPieces.SetPieces.ApplySetPieces(this);
            
            foreach (var (terrain, mobs) in RegionMobs)
            {
                var maxEnemyCount = Map.Terrains[terrain].Count / mobs.Item1;
                _enemyMaxCount[terrain] = maxEnemyCount;
                _enemyCount[terrain] = 0;

                for (var i = 0; i < maxEnemyCount; i++)
                {
                    var objType = GetRandomObjectType(mobs.Item2);
                    
                    if (objType == 0)
                        continue;

                    _enemyCount[terrain] += Spawn(Resources.Type2Object[objType], terrain);
                    
                    if (_enemyCount[terrain] >= maxEnemyCount)
                        break;
                }
            }
        }

        private void SpawnEvent(string name, ISetPiece setPiece)
        {
            Vector2 point;
            var allPoints = Map.Terrains
                .Where(t => t.Key >= Terrain.Mountains && t.Key <= Terrain.MidForest)
                .Select(t => t.Value).ToArray();
            do
            {
                var terrainPoints = allPoints[MathUtils.Next(allPoints.Length)];
                point = terrainPoints[MathUtils.Next(terrainPoints.Count)].ToVector2();
            } while (!IsUnblocked(point) || PlayerNearby(point));
            
            point -= (setPiece.Size - 1) / 2;
            setPiece.RenderSetPiece(this, point.ToIntPoint());
            if (!_criticalEnemyCounts.ContainsKey(name))
                _criticalEnemyCounts[name] = 0;
            _criticalEnemyCounts[name]++;
        }
        
        private int Spawn(ObjectDesc desc, Terrain terrain)
        {
            Vector2 point;
            var terrainPoints = Map.Terrains[terrain];
            var ret = 0;
            var num = 1;

            if (desc.SpawnData != null)
            {
                num = (int) GetNormal(desc.SpawnData.Mean, desc.SpawnData.StdDev);
                
                if (num > desc.SpawnData.Max)
                    num = desc.SpawnData.Max;
                else if (num < desc.SpawnData.Min) 
                    num = desc.SpawnData.Min;
            }
            
            do
            {
                point = terrainPoints[MathUtils.Next(terrainPoints.Count)].ToVector2();
            } while (!IsUnblocked(point) || PlayerNearby(point));

            for (var i = 0; i < num; i++)
            {
                var entity = Entity.Resolve(desc.Type);
                ((Enemy) entity).Terrain = terrain;
                AddEntity(entity, point);
                _enemies[terrain].Add(entity as Enemy);
                ret++;
            }

            return ret;
        }

        private void EnsurePopulation()
        {
            foreach (var terrain in Map.Terrains.Keys)
            {
                if (terrain == Terrain.None)
                    continue;
                
                if (_enemyCount[terrain] > _enemyMaxCount[terrain] * 1.5f)
                {
                    foreach (var enemy in _enemies[terrain])
                    {
                        if (enemy.GetNearestPlayer(Player.SightRadius) != null)
                            continue;
                            
                        RemoveEntity(enemy);
                        _enemyCount[terrain]--;
                        if (_enemyCount[terrain] <= _enemyMaxCount[terrain])
                            break;
                    }
                    continue;
                }
                if (_enemyCount[terrain] < _enemyMaxCount[terrain] * .75f)
                {
                    while (_enemyCount[terrain] < _enemyMaxCount[terrain])
                    {
                        var type = GetRandomObjectType(RegionMobs[terrain].Item2);
                        if (type == 0)
                            continue;

                        _enemyCount[terrain] += Spawn(Resources.Type2Object[type], terrain);
                    }
                }
            }
        }

        private void OryxTaunt()
        {
            if (Closed)
                return;

            var (enemyName, count) = _criticalEnemyCounts.ElementAt(MathUtils.Next(_criticalEnemyCounts.Count));
            var tauntData = CriticalEnemies[enemyName];
            if (count == 0)
                return;

            if (count == 1 && tauntData.Final != null ||
                tauntData.Final != null && tauntData.NumberOfEnemies == null)
            {
                var tauntMessages = tauntData.Final;
                var message = tauntMessages[MathUtils.Next(tauntMessages.Length)];
                foreach (var player in Players.Values)
                    player.SendInfo(message);
            }
            else
            {
                var tauntMessages = tauntData.NumberOfEnemies;
                if (tauntMessages == null)
                    return;
                
                var message = tauntMessages[MathUtils.Next(tauntMessages.Length)];
                message = message.Replace("{COUNT}", count.ToString());
                foreach (var player in Players.Values)
                    player.SendInfo(message);
            }
        }

        public void EnemyKilled(Enemy enemy, Player killer)
        {
            if (enemy.Terrain != Terrain.None)
            {
                _enemyCount[enemy.Terrain]--;
                _enemies[enemy.Terrain].Remove(enemy);
            }

            if (enemy.Desc == null || !enemy.Desc.Quest)
                return;

            if (!CriticalEnemies.TryGetValue(enemy.Desc.Id, out var tauntData))
                return;
            
            _criticalEnemyCounts[enemy.Desc.Id]--;
            if (_criticalEnemyCounts[enemy.Desc.Id] == 0)
                _criticalEnemyCounts.Remove(enemy.Desc.Id);
            
            if (_criticalEnemyCounts.Count == 0)
                Close();

            if (tauntData.Killed != null)
            {
                var killedMessages = tauntData.Killed;
                if (killer == null)
                    killedMessages = killedMessages.Where(m => !m.Contains("{PLAYER}")).ToArray();

                if (killedMessages.Length > 0)
                {
                    var message = killedMessages[MathUtils.Next(killedMessages.Length)];
                    message = message.Replace("{PLAYER}", (killer != null) ? killer.Name : "");
                    foreach (var player in Players.Values)
                        player.SendInfo(message);
                }
            }

            if (!MathUtils.Chance(.25f))
                return;

            var evt = _events[MathUtils.Next(_events.Count)];
            if (Resources.Id2Object[evt.Item1].PerRealmMax == 1)
                _events.Remove(evt);
            SpawnEvent(evt.Item1, evt.Item2);

            if (!CriticalEnemies.TryGetValue(evt.Item1, out tauntData))
                return;

            if (tauntData.Spawn != null)
            {
                var spawnMessages = tauntData.Spawn;
                var message = spawnMessages[MathUtils.Next(spawnMessages.Length)];
                foreach (var player in Players.Values)
                    player.SendInfo(message);
            }
        }
        
        private static double GetUniform()
        {
            // 0 <= u < 2^32
            var u = (uint)(MathUtils.NextFloat() * uint.MaxValue);
            // The magic number below is 1/(2^32 + 2).
            // The result is strictly between 0 and 1.
            return (u + 1.0) * 2.328306435454494e-10;
        }

        private static double GetNormal()
        {
            // Use Box-Muller algorithm
            var u1 = GetUniform();
            var u2 = GetUniform();
            var r = Math.Sqrt(-2.0 * Math.Log(u1));
            var theta = 2.0 * Math.PI * u2;
            return r * Math.Sin(theta);
        }

        private static double GetNormal(double mean, double standardDeviation)
        {
            return mean + standardDeviation * GetNormal();
        }

        private ushort GetRandomObjectType(IEnumerable<Tuple<string, double>> spawnInfo)
        {
            var threshold = MathUtils.NextFloat();
            double n = 0;
            ushort objType = 0;
            foreach (var (name, density) in spawnInfo)
            {
                n += density;
                if (n > threshold)
                {
                    objType = Resources.Id2Object[name].Type;
                    break;
                }
            }
            return objType;
        }
    }
}