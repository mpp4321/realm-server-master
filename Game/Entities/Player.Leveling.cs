using System;
using System.Collections.Generic;
using System.Linq;
using RotMG.Common;
using RotMG.Networking;
using RotMG.Utils;

namespace RotMG.Game.Entities
{
    public partial class Player
    {
        public const int MaxLevel = 20;
        public const int EXPPerFame = 2000;

        public static int GetNextLevelEXP(int level)
        {
            return 50 + (level - 1) * 100;
        }

        public static int GetLevelEXP(int level)
        {
            if (level == 1) return 0;
            return 50 * (level - 1) + (level - 2) * (level - 1) * 50;
        }

        public static int GetNextClassQuestFame(int fame)
        {
            for (var i = 0; i < Stars.Length; i++)
            {
                if (fame >= Stars[i] && i == Stars.Length - 1)
                    return 0;
                if (fame < Stars[i])
                    return Stars[i];
            }
            return -1;
        }

        public void InitLevel(CharacterModel character)
        {
            if (character.Experience != 0) EXP = character.Experience;
            if (character.Fame != 0) CharFame = character.Fame;
            var classStat = Client.Account.Stats.GetClassStats(Type);
            NextClassQuestFame = GetNextClassQuestFame(classStat.BestFame > CharFame ? classStat.BestFame : CharFame);
            NextLevelEXP = GetNextLevelEXP(Level);
            GainEXP(0);
        }

        public bool GainEXP(int exp)
        {
            EXP += exp;

            var newFame = EXP / EXPPerFame;
            if (newFame != CharFame)
                CharFame = newFame;

            var classStat = Client.Account.Stats.GetClassStats(Type);
            var newClassQuestFame = GetNextClassQuestFame(classStat.BestFame > newFame ? classStat.BestFame : newFame);
            if (newClassQuestFame > NextClassQuestFame)
            {
                var notification = GameServer.Notification(Id, "Class Quest Complete!", 0xFF00FF00);
                foreach (var en in Parent.PlayerChunks.HitTest(Position, SightRadius))
                {
                    if (en is Player player && 
                        (player.Client.Account.Notifications || player.Equals(this)))
                        player.Client.Send(notification);
                }
                NextClassQuestFame = newClassQuestFame;
            }

            var levelledUp = false;
            if (EXP - GetLevelEXP(Level) >= NextLevelEXP && Level < MaxLevel)
            {
                levelledUp = true;
                Level++;
                NextLevelEXP = GetNextLevelEXP(Level);
                var stats = Resources.Type2Player[Type].Stats;
                for (var i = 0; i < stats.Length; i++)
                {
                    var min = stats[i].MinIncrease;
                    var max = stats[i].MaxIncrease;
                    Stats[i] += MathUtils.NextInt(min, max);
                    if (Stats[i] > stats[i].MaxValue)
                        Stats[i] = stats[i].MaxValue;
                }

                Hp = Stats[0];
                MP = Stats[1];

                if (Level == 20)
                {
                    var text = GameServer.Text("", 0, -1, 0, "", $"{Name} achieved level 20");
                    foreach (var player in Parent.Players.Values)
                        player.Client.Send(text);
                }

                RecalculateEquipBonuses();
                GetNextQuest(true);
            }

            TrySetSV(StatType.Exp, EXP - GetLevelEXP(Level));
            return levelledUp;
        }

        public void TryGetNextQuest(Entity enemy)
        {
            if (enemy == Quest)
            {
                if (enemy.Position.Distance(Position) <= SightRadius)
                {
                    foreach (var entity in Parent.PlayerChunks.HitTest(Position, SightRadius))
                    {
                        if (entity is Player player && player.Client.Account.Notifications)
                            player.Client.Send(GameServer.Notification(Id, "Quest Complete!", 0xFF00FF00));
                    }
                }

                GetNextQuest(true);
            }
        }

        public Entity Quest;
        public void GetNextQuest(bool prioritize)
        {
            if (Quest != null && !prioritize)
                return;
            
            var newQuest = FindQuest();
            if (newQuest == Quest)
                return;
            
            Client.Send(GameServer.QuestObjId(newQuest?.Id ?? -1));
            Quest = newQuest;
        }

        private Entity FindQuest(Vector2? destination = null)
        {
            Entity ret = null;
            double? bestScore = null;
            destination ??= Position;

            foreach (var i in Parent.Quests.Values
                .OrderBy(quest => quest.Position.Distance(destination.Value)))
            {
                if (i.Desc == null || !i.Desc.Quest) continue;

                if (!QuestDat.TryGetValue(i.Desc.Id, out var x))
                    continue;

                if (Level >= x.Item2 && Level <= x.Item3)
                {
                    var score = (20 - Math.Abs((i.Desc.Level == -1 ? 0 : i.Desc.Level) - Level)) * x.Item1 -   //priority * level diff
                                Position.Distance(i) / 100;    //minus 1 for every 100 tile distance
                    if (bestScore == null || score > bestScore)
                    {
                        bestScore = score;
                        ret = i;
                    }
                }
            }
            return ret;
        }
        
        static readonly Dictionary<string, Tuple<int, int, int>> QuestDat =
            new Dictionary<string, Tuple<int, int, int>>()  //Priority, Min, Max
        {
            // wandering quest enemies
            { "Scorpion Queen",                 Tuple.Create(1, 1, 6) },
            { "Bandit Leader",                  Tuple.Create(1, 1, 6) },
            { "Hobbit Mage",                    Tuple.Create(3, 3, 8) },
            { "Undead Hobbit Mage",             Tuple.Create(3, 3, 8) },
            { "Giant Crab",                     Tuple.Create(3, 3, 8) },
            { "Desert Werewolf",                Tuple.Create(3, 3, 8) },
            { "Sandsman King",                  Tuple.Create(4, 4, 9) },
            { "Goblin Mage",                    Tuple.Create(4, 4, 9) },
            { "Elf Wizard",                     Tuple.Create(4, 4, 9) },
            { "Dwarf King",                     Tuple.Create(5, 5, 10) },
            { "Swarm",                          Tuple.Create(6, 6, 11) },
            { "Shambling Sludge",               Tuple.Create(6, 6, 11) },
            { "Great Lizard",                   Tuple.Create(7, 7, 12) },
            { "Wasp Queen",                     Tuple.Create(8, 7, 20) },
            { "Horned Drake",                   Tuple.Create(8, 7, 20) },

            // setpiece bosses
            { "Deathmage",                      Tuple.Create(5, 6, 11) },
            { "Great Coil Snake",               Tuple.Create(6, 6, 12) },
            { "Lich",                           Tuple.Create(8, 6, 20) },
            { "Actual Lich",                    Tuple.Create(8, 7, 20) },
            { "Ent Ancient",                    Tuple.Create(9, 7, 20) },
            { "Actual Ent Ancient",             Tuple.Create(9, 7, 20) },
            { "Oasis Giant",                    Tuple.Create(10, 8, 20) },
            { "Phoenix Lord",                   Tuple.Create(10, 9, 20) },
            { "Ghost King",                     Tuple.Create(11,10, 20) },
            { "Actual Ghost King",              Tuple.Create(11,10, 20) },
            { "Cyclops God",                    Tuple.Create(12,10, 20) },
            { "Kage Kami",                      Tuple.Create(12,10, 20) },
            { "Red Demon",                      Tuple.Create(13,15, 20) },

                // events
            { "Skull Shrine",                   Tuple.Create(14,15, 20) },
            { "Pentaract",                      Tuple.Create(14,15, 20) },
            { "Cube God",                       Tuple.Create(14,15, 20) },
            { "Grand Sphinx",                   Tuple.Create(14,15, 20) },
            { "Lord of the Lost Lands",         Tuple.Create(14,15, 20) },
            { "Hermit God",                     Tuple.Create(14,15, 20) },
            { "Ghost Ship",                     Tuple.Create(14,15, 20) },

            // dungeon bosses
            { "Evil Chicken God",               Tuple.Create(15,1, 20) },
            { "Bonegrind the Butcher",          Tuple.Create(15,1, 20) },
            { "Dreadstump the Pirate King",     Tuple.Create(15,1, 20) },
            { "Mama Megamoth",                  Tuple.Create(15,1, 20) },
            { "Arachna the Spider Queen",       Tuple.Create(15,1, 20) },
            { "Stheno the Snake Queen",         Tuple.Create(15,1, 20) },
            { "Mixcoatl the Masked God",        Tuple.Create(15,1, 20) },
            { "Limon the Sprite God",           Tuple.Create(15,1, 20) },
            { "Septavius the Ghost God",        Tuple.Create(15,1, 20) },
            { "Davy Jones",                     Tuple.Create(15,1, 20) },
            { "Lord Ruthven",                   Tuple.Create(15,1, 20) },
            { "Archdemon Malphas",              Tuple.Create(15,1, 20) },
            { "Thessal the Mermaid Goddess",    Tuple.Create(15,1, 20) },
            { "Dr Terrible",                    Tuple.Create(15,1, 20) },
            { "Horrific Creation",              Tuple.Create(15,1, 20) },
            { "Masked Party God",               Tuple.Create(15,1, 20) },
            { "Oryx Stone Guardian Left",       Tuple.Create(15,1, 20) },
            { "Oryx Stone Guardian Right",      Tuple.Create(15,1, 20) },
            { "Oryx the Mad God 1",             Tuple.Create(15,1, 20) },
            { "Oryx the Mad God 2",             Tuple.Create(15,1, 20) },
            { "Gigacorn",                       Tuple.Create(15,1, 20) },
            { "Desire Troll",                   Tuple.Create(15,1, 20) },
            { "Spoiled Creampuff",              Tuple.Create(15,1, 20) },
            { "MegaRototo",                     Tuple.Create(15,1, 20) },
            { "Swoll Fairy",                    Tuple.Create(15,1, 20) },
            { "Troll 3",                        Tuple.Create(15,1, 20) },
            { "Arena Ghost Bride",              Tuple.Create(15,1, 20) },
            { "Arena Statue Left",              Tuple.Create(15,1, 20) },
            { "Arena Statue Right",             Tuple.Create(15,1, 20) },
            { "Arena Grave Caretaker",          Tuple.Create(15,1, 20) },
            { "Ghost of Skuld",                 Tuple.Create(15,1, 20) },
            { "Tomb Defender",                  Tuple.Create(15,1, 20) },
            { "Tomb Support",                   Tuple.Create(15,1, 20) },
            { "Tomb Attacker",                  Tuple.Create(15,1, 20) },
            { "Active Sarcophagus",             Tuple.Create(15,1, 20) },
            { "shtrs Bridge Sentinel",          Tuple.Create(15,1, 20) },
            { "shtrs The Forgotten King",       Tuple.Create(15,1, 20) },
            { "shtrs Twilight Archmage",        Tuple.Create(15,1, 20) },
        };
    }
}
