using System;
using System.Collections.Generic;
using System.IO;
using RotMG.Common;
using RotMG.Networking;
using System.Linq;
using System.Text.RegularExpressions;
using RotMG.Game.SetPieces;
using RotMG.Game.Worlds;
using RotMG.Utils;

namespace RotMG.Game.Entities
{
    public partial class Player
    {
        private const int ChatCooldownMS = 200;

        public int LastChatTime;

        private readonly string[] _unrankedCommands =
        {
            "commands", "g", "guild", "tell", "allyshots", "allydamage", "effects", "sounds", "vault", "realm",
            "notifications", "online", "who", "server", "pos", "loc", "where", "find", "fame", "famestats", "stats",
            "trade", "currentsong", "song"
        };

        private readonly string[] _rankedCommands =
        {
            "announce", "announcement", "legendary", "roll", "disconnect", "dcAll", "dc", "songs", "changesong",
            "terminate", "stop", "gimme", "give", "gift", "closerealm", "rank", "create", "spawn", "killall",
            "setpiece", "max", "tq", "god", "eff", "effect", "ban", "unban", "mute", "unmute"
        };

        public void SendInfo(string text) => Client.Send(GameServer.Text("", 0, -1, 0, "", text));
        public void SendError(string text) => Client.Send(GameServer.Text("*Error*", 0, -1, 0, "", text));
        public void SendHelp(string text) => Client.Send(GameServer.Text("*Help*", 0, -1, 0, "", text));
        public void SendClientText(string text) => Client.Send(GameServer.Text("*Client*", 0, -1, 0, "", text));

        private bool PassFilter(string text)
        {
            return true;
        }
        
        public void Chat(string text)
        {
            if (text.Length <= 0 || text.Length > 128)
            {
#if DEBUG
                Program.Print(PrintType.Error, "Text too short or too long");
#endif
                Client.Disconnect();
                return;
            }

            var validText = Regex.Replace(text, @"[^a-zA-Z0-9`!@#$%^&* ()_+|\-=\\{}\[\]:"";'<>?,./]", "");
            if (validText.Length <= 0)
            {
                SendError("Invalid text.");
                return;
            }

            if (LastChatTime + ChatCooldownMS > Manager.TotalTimeUnsynced)
            {
                SendError("Message sent too soon after previous one.");
                return;
            }

            if (!PassFilter(validText))
            {
                Client.Disconnect();
                return;
            }

            LastChatTime = Manager.TotalTimeUnsynced;

            if (validText[0] == '/')
            {
                var s = validText.Split(' ');
                var j = new string[s.Length - 1];
                for (var i = 1; i < s.Length; i++)
                    j[i - 1] = s[i];
                var command = s[0];
                var input = string.Join(' ', j);
                switch (command.ToLower())
                {
                    case "/ban":
                        if (Client.Account.Ranked)
                        {
                            if (j.Length == 0)
                            {
                                SendError("Usage: /ban <name>");
                                return;
                            }

                            if (!Database.AccountExists(j[0], out var account))
                                SendError($"Player {j[0]} doesn't exist");
                            account.Banned = true;
                            account.Save();
                            SendInfo(account.Name + " has been banned");
                        }
                        break;
                    case "/unban":
                        if (Client.Account.Ranked)
                        {
                            if (j.Length == 0)
                            {
                                SendError("Usage: /unban <name>");
                                return;
                            }
                            
                            if (!Database.AccountExists(j[0], out var account))
                                SendError($"Player {j[0]} doesn't exist");
                            account.Banned = false;
                            account.Save();
                            SendInfo(account.Name + " has been unbanned");
                        }
                        break;
                    case "/mute":
                        if (Client.Account.Ranked)
                        {
                            if (j.Length == 0)
                            {
                                SendError("Usage: /mute <name>");
                                return;
                            }

                            if (!Database.AccountExists(j[0], out var account))
                                SendError($"Player {j[0]} doesn't exist");
                            account.Muted = true;
                            account.Save();
                            SendInfo(account.Name + " has been muted");
                        }
                        break;
                    case "/unmute":
                        if (Client.Account.Ranked)
                        {
                            if (j.Length == 0)
                            {
                                SendError("Usage: /unmute <name>");
                                return;
                            }

                            if (!Database.AccountExists(j[0], out var account))
                                SendError($"Player {j[0]} doesn't exist");
                            account.Muted = false;
                            account.Save();
                            SendInfo(account.Name + " has been unmuted");
                        }
                        break;
                    case "/song":
                    case "/currentsong":
                        SendInfo("Current Song: " + Parent.Music);
                        break;
                    case "/songs":
                        if (Client.Account.Ranked)
                        {
                            var songs = Directory.EnumerateFiles(Resources
                                .CombineResourcePath("Web/music/"), "*", SearchOption.AllDirectories)
                                .Select(x =>
                                {
                                    var s = x.Split("/").Last();
                                    return s.Substring(0, s.Length - 4);
                                })
                                .Aggregate("Song choices: ", (c, p) => c + p + ", ");
                            SendInfo(songs.Substring(0, songs.Length - 2));
                        }
                        break;
                    case "/changesong":
                        if (Client.Account.Ranked)
                        {
                            if (j.Length == 0 || string.IsNullOrEmpty(j[0]))
                            {
                                SendError("Usage: /changesong <name>");
                                return;
                            }

                            if (!Resources.WebFiles.ContainsKey($"/music/{j[0]}.mp3"))
                            {
                                SendError("Song " + j[0] + " not found");
                                return;
                            }

                            Parent.Music = j[0];

                            foreach (var player in Parent.Players.Values)
                            {
                                player.SendInfo("World music changed to " + j[0]);
                                player.Client.Send(GameServer.SwitchMusic(j[0]));
                            }
                        }
                        break;
                    case "/commands":
                        if (!Client.Account.Ranked)
                        {
                            SendInfo(string.Join(", ", _unrankedCommands));
                            return;
                        }
                        SendInfo(string.Join(", ", _unrankedCommands.Concat(_rankedCommands)));
                        break;
                    case "/trade":
                        if (j.Length == 0)
                        {
                            if (PotentialPartner != null)
                            {
                                TradeRequest(PotentialPartner.Name);
                                return;
                            }
                            
                            SendError("No pending trades");
                            return;
                        }
                        var partner = Manager.GetPlayer(j[0]);
                        if (partner == null)
                        {
                            SendError("Player " + j[0] + " not found");
                            return;
                        }
                        TradeRequest(partner.Name);
                        break;
                    case "/announce":
                    case "/announcement":
                        if (Client.Account.Ranked)
                        {
                            if (j.Length == 0 || string.IsNullOrWhiteSpace(input))
                            {
                                SendError("Usage: /announce <message>");
                                return;
                            }
                            var announce = GameServer.Text("", 0, -1, 0, "", "<ANNOUNCEMENT> " + input);
                            foreach (var client in Manager.Clients.Values)
                                client.Send(announce);
                        }
                        break;
                    case "/g":
                    case "/guild":
                        if (string.IsNullOrEmpty(Client.Account.GuildName))
                        {
                            SendError("Not in a guild");
                            return;
                        }
                        var guild = GameServer.Text(Name, Id, NumStars, 5, "*Guild*", input);
                        
                        foreach (var client in Manager.Clients.Values)
                        {
                            if (client.Account.GuildName == Client.Account.GuildName)
                                client.Send(guild);
                        }
                        break;
                    case "/tell":
                        if (j.Length == 0 || string.IsNullOrEmpty(j[0]) || string.Equals(j[0], Client.Account.Name, StringComparison.CurrentCultureIgnoreCase))
                            return;

                        var recipient = Manager.GetPlayer(j[0]);
                        if (recipient == null)
                        {
                            SendError("Player not online");
                            return;
                        }

                        var message = string.Join(' ', j, 1, j.Length - 1);
                        var tell = GameServer.Text(Name, Id, NumStars, 5, recipient.Name, message);
                        recipient.Client.Send(tell);
                        Client.Send(tell);
                        break;
                    case "/legendary":
                        if (Client.Account.Ranked)
                        {
                            if (j.Length == 0)
                            {
                                SendError("Usage: /legendary <slot>");
                                return;
                            }
                            var slot = int.Parse(j[0]);
                            if (Inventory[slot] != -1)
                            {
                                var i = Inventory[slot];
                                var roll = Resources.Type2Item[(ushort)i].Roll();
                                while ((roll.Item2 & ItemData.T7) == 0)
                                    roll = Resources.Type2Item[(ushort)i].Roll();
                                ItemDatas[slot] = !roll.Item1 ? -1 : (int)roll.Item2;
                                UpdateInventorySlot(slot);
                                RecalculateEquipBonuses();
                            }
                        }
                        break;
                    case "/roll":
                        if (Client.Account.Ranked)
                        {
                            for (var k = 0; k < 20; k++)
                            {
                                if (Inventory[k] != -1)
                                {
                                    var roll = Resources.Type2Item[(ushort)Inventory[k]].Roll();
                                    var i = Inventory[k];
                                    ItemDatas[k] = !roll.Item1 ? -1 : (int)roll.Item2;
                                    UpdateInventorySlot(k);
                                    RecalculateEquipBonuses();
                                }
                            }
                        }
                        break;
                    case "/disconnect":
                    case "/dcAll":
                    case "/dc":
                        if (Client.Account.Ranked)
                        {
                            foreach (var c in Manager.Clients.Values.ToArray())
                            {
                                try { c.Disconnect(); }
                                catch { }
                            }
                        }
                        break;
                    case "/terminate":
                    case "/stop":
                        if (Client.Account.Ranked)
                        {
                            Program.StartTerminating();
                            return;
                        }
                        break;
                    case "/gimme":
                    case "/give":
                        if (Client.Account.Ranked)
                        {
                            if (j.Length == 0)
                            {
                                SendError("Usage: /give <item name>");
                                return;
                            }
                            if (Resources.IdLower2Item.TryGetValue(input.ToLower(), out var item))
                            {
                                if (GiveItem(item.Type))
                                    SendInfo("Success");
                                else SendError("No inventory slots");
                            }
                            else SendError($"Item <{input}> not found in GameData");
                        }
                        break;
                    case "/gift":
                        if (Client.Account.Ranked)
                        {
                            if (j.Length == 0)
                            {
                                SendError("Usage: /gift <player> <item name>");
                                return;
                            }

                            if (Database.AccountExists(j[0], out var acc))
                            {
                                j[0] = "";
                                var itemString = string.Join(' ', j).Trim().ToLower();
                                if (Resources.IdLower2Item.TryGetValue(itemString, out var gift))
                                {
                                    Database.AddGift(acc, gift.Type);
                                    SendInfo("Success");
                                }
                                else SendError($"Item <{itemString}> not found in GameData");
                            }
                            else SendError($"Player <{j[0]}> doesn't exist");
                        }

                        break;
                    case "/closerealm":
                        if (Client.Account.Ranked)
                        {
                            if (!(Parent is Realm))
                            {
                                SendError("Must be in a realm to close it");
                                return;
                            }

                            (Parent as Realm).Close();
                        }
                        break;
                    case "/rank":
                        if (Client.Account.Ranked)
                        {
                            if (j.Length == 0)
                            {
                                SendError("Usage: /rank <player>");
                                return;
                            }
                            if (Database.AccountExists(j[0], out var acc))
                            {
                                acc.Ranked = true;
                                acc.Save();
                                SendInfo(acc.Name + " has been ranked");
                            }
                            else SendError($"Player <{j[0]}> doesn't exist");
                        }
                        break;
                    case "/create":
                    case "/spawn":
                        if (Client.Account.Ranked)
                        {
                            if (string.IsNullOrWhiteSpace(input))
                            {
                                SendError("Usage: /spawn <count> <entity>");
                                return;
                            }

                            int spawnCount;
                            if (!int.TryParse(j[0], out spawnCount))
                                spawnCount = -1;
                            if (Resources.IdLower2Object.TryGetValue(
                                (spawnCount == -1 ? input : string.Join(' ', j.Skip(1))).ToLower(), out var desc))
                            {
                                if (spawnCount == -1) spawnCount = 1;
                                if (desc.Player || desc.Static)
                                {
                                    SendError("Can't spawn this entity");
                                    return;
                                }

                                SendInfo($"Spawning <{spawnCount}> <{desc.DisplayId}> in 2 seconds");
                                var pos = Position;
                                Manager.AddTimedAction(2000, () =>
                                {
                                    for (var i = 0; i < spawnCount; i++)
                                    {
                                        var entity = Resolve(desc.Type);
                                        Parent?.AddEntity(entity, pos);
                                    }
                                });
                            }
                            else
                            {
                                SendError($"Entity <{input}> not found in Game Data");
                            }
                        }
                        break;
                    case "/killall":
                        if (Client.Account.Ranked)
                        {
                            var count = 0;
                            foreach (var entity in Parent.Entities.Values.ToArray())
                            {
                                if (entity is Enemy enemy && (string.IsNullOrWhiteSpace(input) || string.Equals(entity.Desc.Id, input, StringComparison.CurrentCultureIgnoreCase)))
                                {
                                    enemy.Death(this);
                                    count++;
                                }
                            }

                            SendInfo($"Killed {count} entities");
                        }
                        break;
                    case "/setpiece":
                        if (Client.Account.Ranked)
                        {
                            if (string.IsNullOrWhiteSpace(input))
                            {
                                var type = typeof(ISetPiece);
                                var types = type.Assembly.GetTypes()
                                    .Where(t => type.IsAssignableFrom(t) && !t.IsAbstract);
                                var msg = types.Aggregate(
                                    "Valid SetPieces: ", (c, p) => c + p.Name + ", ");
                                SendInfo(msg.Substring(0, msg.Length - 2));
                                return;
                            }

                            if (Parent is Nexus)
                            {
                                SendError("Not allowed in Nexus");
                                return;
                            }

                            try
                            {
                                var setPiece = (ISetPiece) Activator.CreateInstance(System.Type.GetType(
                                    "RotMG.Game.SetPieces." + input, true, true));
                                setPiece?.RenderSetPiece(Parent, (Position + 1).ToIntPoint());
                            }
                            catch (Exception)
                            {
                                SendError("Invalid SetPiece");
                            }
                        }
                        break;
                    case "/max":
                        if (Client.Account.Ranked)
                        {
                            for (var i = 0; i < Stats.Length; i++)
                                Stats[i] = (Desc as PlayerDesc).Stats[i].MaxValue;
                            UpdateStats();
                            SendInfo("Maxed");
                        }
                        break;
                    case "/tq":
                        if (Client.Account.Ranked)
                        {
                            if (Quest == null)
                            {
                                SendError("No quest to teleport to");
                                return;
                            }

                            EntityTeleport(_clientTime, Quest.Id, true);
                        }

                        break;
                    case "/god":
                        if (Client.Account.Ranked)
                        {
                            ApplyConditionEffect(ConditionEffectIndex.Invincible,
                                HasConditionEffect(ConditionEffectIndex.Invincible) ? 0 : -1);
                            SendInfo($"Godmode set to {HasConditionEffect(ConditionEffectIndex.Invincible)}");
                        }

                        break;
                    case "/eff":
                    case "/effect":
                        if (Client.Account.Ranked)
                        {

                            if (string.IsNullOrWhiteSpace(input))
                            {
                                SendError("Invalid effect");
                                return;
                            }

                            ConditionEffectIndex eff;
                            if (int.TryParse(input, out var effect))
                            {
                                eff = (ConditionEffectIndex) effect;
                            }
                            else if (!Enum.TryParse(input, true, out eff))
                            {
                                SendError("Invalid effect");
                                return;
                            }

                            if (eff == ConditionEffectIndex.Nothing)
                                return;
                            if (HasConditionEffect(eff))
                            {
                                RemoveConditionEffect(eff);
                                SendInfo("Removed condition effect " + eff);
                            }
                            else
                            {
                                ApplyConditionEffect(eff, -1);
                                SendInfo("Applied condition effect " + eff);
                            }
                        }

                        break;
                    case "/allyshots":
                        Client.Account.AllyShots = !Client.Account.AllyShots;
                        SendInfo($"Ally shots set to {Client.Account.AllyShots}");
                        break;
                    case "/allydamage":
                        Client.Account.AllyDamage = !Client.Account.AllyDamage;
                        SendInfo($"Ally damage set to {Client.Account.AllyDamage}");
                        break;
                    case "/effects":
                        Client.Account.Effects = !Client.Account.Effects;
                        SendInfo($"Effects set to {Client.Account.Effects}");
                        break;
                    case "/sounds":
                        Client.Account.Sounds = !Client.Account.Sounds;
                        SendInfo($"Sounds set to {Client.Account.Sounds}");
                        break;
                    case "/notifications":
                        Client.Account.Notifications = !Client.Account.Notifications;
                        SendInfo($"Notifications set to {Client.Account.Notifications}");
                        break;
                    case "/online":
                    case "/who":
                        SendInfo($"" +
                            $"<{Manager.Clients.Values.Count(k => k.Player != null)} Player(s)> " +
                            $"<{string.Join(", ", Manager.Clients.Values.Where(k => k.Player != null).Select(k => k.Player.Name))}>");
                        break;
                    case "/server":
                    case "/pos":
                    case "/loc":
                        SendInfo(this.ToString());
                        break;
                    case "/where":
                    case "/find":
                        var findTarget = Manager.GetPlayer(input);
                        if (findTarget == null) SendError("Couldn't find player");
                        else SendInfo(findTarget.ToString());
                        break;
                    case "/vault":
                        Client.Send(GameServer.Reconnect(Manager.VaultId));
                        break;
                    case "/realm":
                        var realmIds = new List<int>(Manager.Realms.Keys);
                        Realm realm = null;
                        while (realmIds.Count > 0 && realm == null)
                        {
                            var id = realmIds[MathUtils.Next(realmIds.Count)];
                            realm = Manager.GetWorld(id, Client) as Realm;
                            if (realm.Closed)
                            {
                                realm = null;
                                realmIds.Remove(id);
                            }
                        }
                        if (realm == null)
                        {
                            SendInfo("No available realms");
                            return;
                        }
                        Client.Send(GameServer.Reconnect(realm.Id));
                        break;
                    case "/fame":
                    case "/famestats":
                    case "/stats":
                        SaveToCharacter();
                        var fameStats = Database.CalculateStats(Client.Account, Client.Character);
                        SendInfo($"Active: {FameStats.MinutesActive} minutes");
                        SendInfo($"Shots: {FameStats.Shots}");
                        SendInfo($"Accuracy: {(int)((float)FameStats.ShotsThatDamage / FameStats.Shots * 100f)}% ({FameStats.ShotsThatDamage}/{FameStats.Shots})");
                        SendInfo($"Abilities Used: {FameStats.AbilitiesUsed}");
                        SendInfo($"Tiles Seen: {FameStats.TilesUncovered}");
                        SendInfo($"Monster Kills: {FameStats.MonsterKills} ({FameStats.MonsterAssists} Assists, {(int)((float)FameStats.MonsterKills / (FameStats.MonsterKills + FameStats.MonsterAssists) * 100f)}% Final Blows)");
                        SendInfo($"God Kills: {FameStats.GodKills} ({(int)((float)FameStats.GodKills / FameStats.MonsterKills * 100f)}%) ({FameStats.GodKills}/{FameStats.MonsterKills})");
                        SendInfo($"Oryx Kills: {FameStats.OryxKills} ({(int)((float)FameStats.OryxKills / FameStats.MonsterKills * 100f)}%) ({FameStats.OryxKills}/{FameStats.MonsterKills})");
                        SendInfo($"Cube Kills: {FameStats.CubeKills} ({(int)((float)FameStats.CubeKills / FameStats.MonsterKills * 100f)}%) ({FameStats.CubeKills}/{FameStats.MonsterKills})");
                        SendInfo($"Cyan Bags: {FameStats.CyanBags}");
                        SendInfo($"Blue Bags: {FameStats.BlueBags}");
                        SendInfo($"White Bags: {FameStats.WhiteBags}");
                        SendInfo($"Damage Taken: {FameStats.DamageTaken}");
                        SendInfo($"Damage Dealt: {FameStats.DamageDealt}");
                        SendInfo($"Teleports: {FameStats.Teleports}");
                        SendInfo($"Potions Drank: {FameStats.PotionsDrank}");
                        SendInfo($"Quests Completed: {FameStats.QuestsCompleted}");
                        SendInfo($"Pirate Caves Completed: {FameStats.PirateCavesCompleted}");
                        SendInfo($"Spider Dens Completed: {FameStats.SpiderDensCompleted}");
                        SendInfo($"Snake Pits Completed: {FameStats.SnakePitsCompleted}");
                        SendInfo($"Sprite Worlds Completed: {FameStats.SpriteWorldsCompleted}");
                        SendInfo($"Undead Lairs Completed: {FameStats.UndeadLairsCompleted}");
                        SendInfo($"Abyss Of Demons Completed: {FameStats.AbyssOfDemonsCompleted}");
                        SendInfo($"Tombs Completed: {FameStats.TombsCompleted}");
                        SendInfo($"Escapes: {FameStats.Escapes}");
                        SendInfo($"Near Death Escapes: {FameStats.NearDeathEscapes}");
                        SendInfo($"Party Member Level Ups: {FameStats.LevelUpAssists}");
                        foreach (var bonus in fameStats.Bonuses)
                            SendHelp($"{bonus.Name}: +{bonus.Fame}");
                        SendInfo($"Base Fame: {fameStats.BaseFame}");
                        SendInfo($"Total Fame: {fameStats.TotalFame}");
                        break;
                    default:
                        SendError("Unknown command");
                        break;
                }
                return;
            }

            if (Client.Account.Muted)
            {
                SendError("You are muted");
                return;
            }

            var name = Client.Account.Ranked ? "@" + Name : Name;
            var packet = GameServer.Text(name, Id, NumStars, 5, "", validText);

            foreach (var player in Parent.Players.Values)
                if (!player.Client.Account.IgnoredIds.Contains(AccountId))
                    player.Client.Send(packet);
        }
    }
}
