using RotMG.Common;
using RotMG.Game;
using RotMG.Game.Entities;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace RotMG.Networking
{
    public static partial class GameServer
    {
        public enum PacketId
        {
            Failure,
            CreateSuccess,
            Create,
            PlayerShoot,
            Move,
            PlayerText,
            Text,
            ServerPlayerShoot,
            Damage,
            Update,
            Notification,
            NewTick,
            InvSwap,
            UseItem,
            ShowEffect,
            Hello,
            Goto,
            InvDrop,
            InvResult,
            Reconnect,
            MapInfo,
            Load,
            Teleport,
            UsePortal,
            Death,
            Buy,
            BuyResult,
            Aoe,
            PlayerHit,
            EnemyHit,
            AoeAck,
            ShootAck,
            SquareHit,
            EditAccountList,
            AccountList,
            QuestObjId,
            CreateGuild,
            GuildResult,
            GuildRemove,
            GuildInvite,
            AllyShoot,
            EnemyShoot,
            Escape,
            InvitedToGuild,
            JoinGuild,
            ChangeGuildRank,
            PlaySound,
            Reskin,
            GotoAck,
            TradeRequest,
            TradeRequested,
            TradeStart,
            ChangeTrade,
            TradeChanged,
            CancelTrade,
            TradeDone,
            AcceptTrade,
            TradeAccepted,
            SwitchMusic
        }

        public static void Read(Client client, int id, byte[] data)
        {
#if DEBUG
            if (id != (int)PacketId.Move)
                Program.Print(PrintType.Debug, $"Packet received <{(PacketId)id}> <{string.Join(" ,",data.Select(k => k.ToString()).ToArray())}>");
#endif

            if (!client.Active)
            {
#if DEBUG
                Program.Print(PrintType.Error, "Didn't process packet, client not active");
#endif
                return;
            }

            using (var rdr = new PacketReader(new MemoryStream(data)))
            {
                switch ((PacketId)id)
                {
                    case PacketId.Hello:
                        Hello(client, rdr);
                        break;
                    case PacketId.Create:
                        Create(client, rdr);
                        break;
                    case PacketId.Load:
                        Load(client, rdr);
                        break;
                    case PacketId.Move:
                        Move(client, rdr);
                        break;
                    case PacketId.InvSwap:
                        InvSwap(client, rdr);
                        break;
                    case PacketId.ShootAck:
                        ShootAck(client, rdr);
                        break;
                    case PacketId.AoeAck:
                        AoeAck(client, rdr);
                        break;
                    case PacketId.PlayerHit:
                        PlayerHit(client, rdr);
                        break;
                    case PacketId.SquareHit:
                        SquareHit(client, rdr);
                        break;
                    case PacketId.PlayerShoot:
                        PlayerShoot(client, rdr);
                        break;
                    case PacketId.EnemyHit:
                        EnemyHit(client, rdr);
                        break;
                    case PacketId.PlayerText:
                        PlayerText(client, rdr);
                        break;
                    case PacketId.EditAccountList:
                        EditAccountList(client, rdr);
                        break;
                    case PacketId.UseItem:
                        UseItem(client, rdr);
                        break;
                    case PacketId.GotoAck:
                        GotoAck(client, rdr);
                        break;
                    case PacketId.Escape:
                        Escape(client, rdr);
                        break;
                    case PacketId.InvDrop:
                        InvDrop(client, rdr);
                        break;
                    case PacketId.UsePortal:
                        UsePortal(client, rdr);
                        break;
                    case PacketId.Buy:
                        Buy(client, rdr);
                        break;
                    case PacketId.Teleport:
                        Teleport(client, rdr);
                        break;
                    case PacketId.CreateGuild:
                        CreateGuild(client, rdr);
                        break;
                    case PacketId.GuildRemove:
                        GuildRemove(client, rdr);
                        break;
                    case PacketId.GuildInvite:
                        GuildInvite(client, rdr);
                        break;
                    case PacketId.JoinGuild:
                        JoinGuild(client, rdr);
                        break;
                    case PacketId.ChangeGuildRank:
                        ChangeGuildRank(client, rdr);
                        break;
                    case PacketId.TradeRequest:
                        TradeRequest(client, rdr);
                        break;
                    case PacketId.ChangeTrade:
                        ChangeTrade(client, rdr);
                        break;
                    case PacketId.CancelTrade:
                        CancelTrade(client, rdr);
                        break;
                    case PacketId.AcceptTrade:
                        AcceptTrade(client, rdr);
                        break;
                }
            }
        }

        private static void ChangeTrade(Client client, PacketReader rdr)
        {
            var offer = new bool[rdr.ReadByte()];
            for (var i = 0; i < offer.Length; i++)
                offer[i] = rdr.ReadBoolean();
            client.Player.ChangeTrade(offer);
        }

        private static void TradeRequest(Client client, PacketReader rdr)
        {
            var name = rdr.ReadString();
            client.Player.TradeRequest(name);
        }

        // possibly check trade partner
        private static void CancelTrade(Client client, PacketReader rdr)
        {
            client.Player.TradeDone(Player.TradeResult.Canceled);
        }

        private static void AcceptTrade(Client client, PacketReader rdr)
        {
            var myOffer = new bool[rdr.ReadByte()];
            for (var i = 0; i < myOffer.Length; i++)
                myOffer[i] = rdr.ReadBoolean();
            var theirOffer = new bool[rdr.ReadByte()];
            for (var i = 0; i < theirOffer.Length; i++)
                theirOffer[i] = rdr.ReadBoolean();
            client.Player.AcceptTrade(myOffer, theirOffer);
        }

        private static void CreateGuild(Client client, PacketReader rdr)
        {
            var name = rdr.ReadString();
            var account = client.Account;

            if (account.Stats.Fame < 1000)
                return;

            if (!string.IsNullOrEmpty(account.GuildName))
                return;

            var createResult = Database.CreateGuild(name, out var guild);
            if (createResult != GuildCreateStatus.Success)
            {
                client.Send(GuildResult(false, createResult.ToString()));
                return;
            }

            var addResult = Database.AddGuildMember(guild, client.Account, true);
            if (addResult != AddGuildMemberStatus.Success)
            {
                client.Send(GuildResult(false, addResult.ToString()));
                return;
            }
            
            Database.IncrementCurrency(account, -1000, Currency.Fame);
            client.Player.GuildName = guild.Name;
            client.Player.GuildRank = 40;
            
            client.Send(GuildResult(true, "Success!"));
        }

        private static void JoinGuild(Client client, PacketReader rdr)
        {
            var guildName = rdr.ReadString();
            if (client.Player.GuildInvite == null)
            {
                client.Player.SendError("You have not been invited to a guild");
                return;
            }

            if (client.Player.GuildInvite != guildName)
            {
                client.Player.SendError("You have not been invited to join " + guildName);
                return;
            }

            var guild = Database.GetGuild(guildName);
            if (guild == null)
            {
                client.Player.SendError("Guild " + guildName + " does not exist");
                return;
            }

            var addResult = Database.AddGuildMember(guild, client.Account);
            if (addResult != AddGuildMemberStatus.Success)
            {
                client.Player.SendError($"Could not join guild. ({addResult})");
                return;
            }

            client.Player.GuildName = guild.Name;
            client.Player.GuildRank = 0;
        }

        private static void GuildInvite(Client client, PacketReader rdr)
        {
            var playerName = rdr.ReadString();
            if (client.Account.GuildRank < 20)
                return;
            
            // probably better to send accountId from client
            Client otherClient = null;
            foreach (var connectedClient in Manager.Clients.Values)
            {
                if (connectedClient.Player.Name == playerName)
                    otherClient = connectedClient;
            }

            if (otherClient == null)
            {
                client.Player.SendError("Player not found");
                return;
            }

            if (!string.IsNullOrEmpty(otherClient.Account.GuildName))
            {
                client.Player.SendError("Player already in a guild");
                return;
            }

            otherClient.Player.GuildInvite = client.Account.GuildName;
            otherClient.Send(InvitedToGuild(client.Account.Name, client.Account.GuildName));
        }

        private static void GuildRemove(Client client, PacketReader rdr)
        {
            var name = rdr.ReadString();
            if (client.Account.Name == name)
            {
                if (!Database.RemoveFromGuild(client.Account))
                {
                    client.Send(GuildResult(false, "Guild not found"));
                    return;
                }

                client.Player.GuildName = "";
                client.Player.GuildRank = 0;
                return;
            }

            if (!Database.AccountExists(name, out var otherAccount))
            {
                client.Send(GuildResult(false, "Player not found"));
                return;
            }

            if (client.Account.GuildRank >= 20 &&
                client.Account.GuildName == otherAccount.GuildName &&
                client.Account.GuildRank > otherAccount.GuildRank)
            {
                if (!Database.RemoveFromGuild(otherAccount))
                {
                    client.Send(GuildResult(false, "Guild not found"));
                    return;
                }

                var otherClientId = Manager.AccountIdToClientId[otherAccount.Id];
                if (Manager.Clients.TryGetValue(otherClientId, out var otherClient))
                {
                    otherClient.Player.GuildName = "";
                    otherClient.Player.GuildRank = 0;
                }

                client.Send(GuildResult(true, "Success!"));
                return;
            }
            
            client.Send(GuildResult(false, "Insufficient privileges"));
        }

        private static void ChangeGuildRank(Client client, PacketReader rdr)
        {
            var name = rdr.ReadString();
            var rank = rdr.ReadByte();
            
            if (!Database.AccountExists(name, out var otherAccount))
            {
                client.Send(GuildResult(false, "Player not found"));
                return;
            }

            if (string.IsNullOrEmpty(client.Account.GuildName) ||
                client.Account.GuildRank < 20 ||
                client.Account.GuildRank <= otherAccount.GuildRank ||
                client.Account.GuildRank < rank ||
                rank == 40 ||
                client.Account.GuildName != otherAccount.GuildName)
            {
                client.Send(GuildResult(false, "No Permission"));
                return;
            }

            if (otherAccount.GuildRank == rank)
            {
                client.Send(GuildResult(false, "Player is already that rank"));
                return;
            }

            if (!Database.ChangeGuildRank(otherAccount, rank))
            {
                client.Send(GuildResult(false, "Failed to change rank"));
                return;
            }
            
            var otherClientId = Manager.AccountIdToClientId[otherAccount.Id];
            if (Manager.Clients.TryGetValue(otherClientId, out var otherClient))
            {
                otherClient.Player.GuildRank = rank;
            }
            
            client.Send(GuildResult(true, "Success!"));
        }

        private static void Teleport(Client client, PacketReader rdr)
        {
            var time = rdr.ReadInt32();
            var objectId = rdr.ReadInt32();
            client.Player.EntityTeleport(time, objectId);
        }

        private static void Buy(Client client, PacketReader rdr)
        {
            var objectId = rdr.ReadInt32();
            var en = client.Player.Parent.GetEntity(objectId);

            if (!(en is ISellable))
            {
#if DEBUG
                Program.Print(PrintType.Error, "Tried buying from non sellable object");
                return;
#endif
            }
            
            (en as ISellable).Buy(client.Player);    
        }

        private static void UsePortal(Client client, PacketReader rdr)
        {
            var objectId = rdr.ReadInt32();
            client.Player.UsePortal(objectId);
        }

        private static void InvDrop(Client client, PacketReader rdr)
        {
            var slot = rdr.ReadByte();
            client.Player.DropItem(slot);
        }

        private static void Escape(Client client, PacketReader rdr)
        {
            client.Active = false;
            client.Player.FameStats.Escapes++;
            if (client.Player.Hp <= 10)
                client.Player.FameStats.NearDeathEscapes++;
            client.Send(Reconnect(Manager.NexusId));
            Manager.AddTimedAction(2000, client.Disconnect);
        }

        private static void GotoAck(Client client, PacketReader rdr)
        {
            var time = rdr.ReadInt32(); 
            client.Player.TryGotoAck(time);
        }

        private static void UseItem(Client client, PacketReader rdr)
        {
            var time = rdr.ReadInt32();
            var slot = new SlotData(rdr);
            var usePos = new Vector2(rdr);
            client.Player.TryUseItem(time, slot, usePos);
        }

        private static void EditAccountList(Client client, PacketReader rdr)
        {
            var accountListId = rdr.ReadInt32();
            var add = rdr.ReadBoolean();
            var objectId = rdr.ReadInt32();
            var en = client.Player.Parent.GetEntity(objectId);
            if (en != null && en is Player target) 
            {
                if (target.AccountId == client.Player.AccountId)
                    return;

                switch (accountListId)
                {
                    case 0: //Lock
                        if (add) client.Account.LockedIds.Add(target.AccountId);
                        else client.Account.LockedIds.Remove(target.AccountId);
                        client.Send(AccountList(0, client.Account.LockedIds));
                        break;
                    case 1: //Ignore
                        if (add) client.Account.IgnoredIds.Add(target.AccountId);
                        else client.Account.IgnoredIds.Remove(target.AccountId);
                        client.Send(AccountList(1, client.Account.IgnoredIds));
                        break;
                }
            }
        }

        private static void PlayerText(Client client, PacketReader rdr)
        {
            var text = rdr.ReadString(); 
            client.Player.Chat(text);
        }

        private static void EnemyHit(Client client, PacketReader rdr)
        {
            var time = rdr.ReadInt32();
            var bulletId = rdr.ReadInt32();
            var targetId = rdr.ReadInt32(); 
            client.Player.TryHitEnemy(time, bulletId, targetId);
        }

        private static void PlayerShoot(Client client, PacketReader rdr)
        {
            var time = rdr.ReadInt32();
            var pos = new Vector2(rdr);
            var angle = rdr.ReadSingle();
            var ability = rdr.ReadBoolean();
            var numShots = rdr.PeekChar() != -1 ? rdr.ReadByte() : (byte)1; 
            client.Player.TryShoot(time, pos, angle, ability, numShots);
        }

        private static void SquareHit(Client client, PacketReader rdr)
        {
            var time = rdr.ReadInt32();
            var bulletId = rdr.ReadInt32(); 
            client.Player.TryHitSquare(time, bulletId);
        }

        private static void PlayerHit(Client client, PacketReader rdr)
        {
            var bulletId = rdr.ReadInt32(); 
            client.Player.TryHit(bulletId);
        }

        private static void ShootAck(Client client, PacketReader rdr)
        {
            var time = rdr.ReadInt32(); 
            client.Player.TryShootAck(time);
        }

        private static void AoeAck(Client client, PacketReader rdr)
        {
            var time = rdr.ReadInt32();
            var pos = new Vector2(rdr); 
            client.Player.TryAckAoe(time, pos);
        }

        private static void Hello(Client client, PacketReader rdr)
        {
            var buildVersion = rdr.ReadString();
            var gameId = rdr.ReadInt32();
            var username = rdr.ReadString();
            var password = rdr.ReadString();
            var mapJson = rdr.ReadBytes(rdr.ReadInt32());

            if (client.State == ProtocolState.Handshaked) //Only allow Hello to be processed once.
            {
                var acc = Database.Verify(username, password, client.IP);
                if (acc == null)
                {
                    client.Send(Failure(0, "Invalid account."));
                    Manager.AddTimedAction(1000, client.Disconnect);
                    return;
                }

                if (acc.Banned)
                {
                    client.Send(Failure(0, "Banned."));
                    Manager.AddTimedAction(1000, client.Disconnect);
                    return;
                }

                if (!acc.Ranked && gameId == Manager.EditorId)
                {
                    client.Send(Failure(0, "Not ranked."));
                    Manager.AddTimedAction(1000, client.Disconnect);
                }

                Manager.GetClient(acc.Id)?.Disconnect();

                if (Database.IsAccountInUse(acc))
                {
                    client.Send(Failure(0, "Account in use!"));
                    Manager.AddTimedAction(1000, client.Disconnect);
                    return;
                }

                client.Account = acc;
                client.Account.Connected = true;
                client.Account.LastSeen = Database.UnixTime();
                client.Account.Save();
                client.TargetWorldId = gameId;

                Manager.AccountIdToClientId[client.Account.Id] = client.Id;
                var world = Manager.GetWorld(gameId, client);

#if DEBUG
                if (client.TargetWorldId == Manager.EditorId)
                {
                    Program.Print(PrintType.Debug, "Loading editor world");
                    var map = new JSMap(Encoding.UTF8.GetString(mapJson));
                    world = new World(map, Resources.Worlds["Dreamland"]);
                    client.TargetWorldId = Manager.AddWorld(world);
                }
#endif

                if (world == null)
                {
                    client.Send(Failure(0, "Invalid world!"));
                    Manager.AddTimedAction(1000, client.Disconnect);
                    return;
                }

                var seed = (uint)MathUtils.NextInt(1, int.MaxValue - 1);
                client.Random = new wRandom(seed);
                client.Send(MapInfo(world.Width, world.Height, world.Name, world.DisplayName, seed, world.Background, world.ShowDisplays, world.AllowTeleport, world.Music));
                client.State = ProtocolState.Awaiting; //Allow the processing of Load/Create.
            }
        }

        private static void Create(Client client, PacketReader rdr)
        {
            int classType = rdr.ReadInt16();
            int skinType = rdr.ReadInt16();

            if (client.State == ProtocolState.Awaiting)
            {
                var character = Database.CreateCharacter(client.Account, classType, skinType);
                if (character == null)
                {
                    client.Send(Failure(0, "Failed to create character."));
                    client.Disconnect();
                    return;
                }

                var world = Manager.GetWorld(client.TargetWorldId, client);
                client.Character = character;
                client.Player = new Player(client);
                client.State = ProtocolState.Connected;
                client.Send(CreateSuccess(world.AddEntity(client.Player, world.GetSpawnRegion().ToVector2()), client.Character.Id));
            }
        }

        private static void Load(Client client, PacketReader rdr)
        {
            var charId = rdr.ReadInt32();

            if (client.State == ProtocolState.Awaiting)
            {
                var character = Database.LoadCharacter(client.Account, charId);
                if (character == null || character.Dead)
                {
                    client.Send(Failure(0, "Failed to load character."));
                    client.Disconnect();
                    return;
                }

                var world = Manager.GetWorld(client.TargetWorldId, client);
                client.Character = character;
                client.Player = new Player(client);
                client.State = ProtocolState.Connected;
                client.Send(CreateSuccess(world.AddEntity(client.Player, world.GetSpawnRegion().ToVector2()), client.Character.Id));
            }
        }

        private static void Move(Client client, PacketReader rdr)
        {
            var time = rdr.ReadInt32();
            var position = new Vector2(rdr); 
            client.Player.TryMove(time, position);
        }

        private static void InvSwap(Client client, PacketReader rdr)
        {
            var slot1 = new SlotData(rdr);
            var slot2 = new SlotData(rdr); 
            client.Player.SwapItem(slot1, slot2);
        }

        public static int Write(Client client, byte[] buffer, int offset, byte[] packet)
        {
            var stream = new MemoryStream(buffer, offset + 4, buffer.Length - offset - 4);
            stream.Write(packet);
            var length = (int)stream.Position;
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(length + 5)), 0, buffer, offset, 4);
            return length + 5;
        }

        public static byte[] SwitchMusic(string song)
        {
            using (var wtr = new PacketWriter(new MemoryStream()))
            {
                wtr.Write((byte)PacketId.SwitchMusic);
                wtr.Write(song);
                return (wtr.BaseStream as MemoryStream).ToArray();
            }
        }

        public static byte[] TradeStart(string name, TradeItem[] myItems, TradeItem[] theirItems)
        {
            using (var wtr = new PacketWriter(new MemoryStream()))
            {
                wtr.Write((byte)PacketId.TradeStart);
                wtr.Write((byte)myItems.Length);
                foreach (var item in myItems)
                    item.Write(wtr);
                wtr.Write(name);
                wtr.Write((byte)theirItems.Length);
                foreach (var item in theirItems)
                    item.Write(wtr);
                return (wtr.BaseStream as MemoryStream).ToArray();
            }
        }

        public static byte[] TradeRequested(string name)
        {
            using (var wtr = new PacketWriter(new MemoryStream()))
            {
                wtr.Write((byte)PacketId.TradeRequested);
                wtr.Write(name);
                return (wtr.BaseStream as MemoryStream).ToArray();
            }
        }

        public static byte[] TradeChanged(bool[] offer)
        {
            using (var wtr = new PacketWriter(new MemoryStream()))
            {
                wtr.Write((byte)PacketId.TradeChanged);
                wtr.Write((byte)offer.Length);
                foreach (var item in offer)
                    wtr.Write(item);
                return (wtr.BaseStream as MemoryStream).ToArray();
            }
        }

        public static byte[] TradeAccepted(bool[] myOffer, bool[] theirOffer)
        {
            using (var wtr = new PacketWriter(new MemoryStream()))
            {
                wtr.Write((byte)PacketId.TradeAccepted);
                wtr.Write((byte)myOffer.Length);
                foreach (var item in myOffer)
                    wtr.Write(item);
                wtr.Write((byte)theirOffer.Length);
                foreach (var item in theirOffer)
                    wtr.Write(item);
                return (wtr.BaseStream as MemoryStream).ToArray();
            }
        }

        public static byte[] TradeDone(Player.TradeResult result)
        {
            using (var wtr = new PacketWriter(new MemoryStream()))
            {
                wtr.Write((byte)PacketId.TradeDone);
                wtr.Write((byte)result);
                return (wtr.BaseStream as MemoryStream).ToArray();
            }
        }

        public static byte[] MapInfo(int width, int height, string name, string displayName, uint seed, int background, bool showDisplays, bool allowPlayerTeleport, string music)
        {
            using (var wtr = new PacketWriter(new MemoryStream()))
            {
                wtr.Write((byte)PacketId.MapInfo);
                wtr.Write(width);
                wtr.Write(height);
                wtr.Write(name);
                wtr.Write(displayName);
                wtr.Write(seed);
                wtr.Write(background);
                wtr.Write(showDisplays);
                wtr.Write(allowPlayerTeleport);
                wtr.Write(music);
                return (wtr.BaseStream as MemoryStream).ToArray();
            }
        }

        public static byte[] InvResult(int result)
        {
            using (var wtr = new PacketWriter(new MemoryStream()))
            {
                wtr.Write((byte)PacketId.InvResult);
                wtr.Write(result);
                return (wtr.BaseStream as MemoryStream).ToArray();
            }
        }

        public static byte[] BuyResult(byte result, BuyResult message)
        {
            using (var wtr = new PacketWriter(new MemoryStream()))
            {
                wtr.Write((byte)PacketId.BuyResult);
                wtr.Write(result);
                wtr.Write((byte)message);
                return (wtr.BaseStream as MemoryStream).ToArray();
            }
        }

        public static byte[] GuildResult(bool success, string message)
        {
            using (var wtr = new PacketWriter(new MemoryStream()))
            {
                wtr.Write((byte)PacketId.GuildResult);
                wtr.Write(success);
                wtr.Write(message);
                return (wtr.BaseStream as MemoryStream).ToArray();
            }
        }

        public static byte[] InvitedToGuild(string name, string guildName)
        {
            using (var wtr = new PacketWriter(new MemoryStream()))
            {
                wtr.Write((byte)PacketId.InvitedToGuild);
                wtr.Write(name);
                wtr.Write(guildName);
                return (wtr.BaseStream as MemoryStream).ToArray();
            }
        }

        public static byte[] Failure(int errorId, string description)
        {
            using (var wtr = new PacketWriter(new MemoryStream()))
            {
                wtr.Write((byte)PacketId.Failure);
                wtr.Write(errorId);
                wtr.Write(description);
                return (wtr.BaseStream as MemoryStream).ToArray();
            }
        }

        public static byte[] CreateSuccess(int objectId, int charId)
        {
            using (var wtr = new PacketWriter(new MemoryStream()))
            {
                wtr.Write((byte)PacketId.CreateSuccess);
                wtr.Write(objectId);
                wtr.Write(charId);
                return (wtr.BaseStream as MemoryStream).ToArray();
            }
        }

        public static byte[] Update(List<TileData> tiles, List<ObjectDefinition> adds, List<ObjectDrop> drops)
        {
            using (var wtr = new PacketWriter(new MemoryStream()))
            {
                wtr.Write((byte)PacketId.Update);
                wtr.Write((short)tiles.Count);
                foreach (var k in tiles)
                    k.Write(wtr);

                wtr.Write((short)adds.Count);
                foreach (var k in adds)
                    k.Write(wtr);

                wtr.Write((short)drops.Count);
                foreach (var k in drops)
                    k.Write(wtr);

                return (wtr.BaseStream as MemoryStream).ToArray();
            }
        }

        public static byte[] NewTick(List<ObjectStatus> statuses, Dictionary<StatType, object> playerStats)
        {
            using (var wtr = new PacketWriter(new MemoryStream()))
            {
                wtr.Write((byte)PacketId.NewTick);
                wtr.Write((short)statuses.Count);
                foreach (var k in statuses)
                    k.Write(wtr);
                if (playerStats.Count > 0)
                {
                    wtr.Write((byte)playerStats.Count);
                    foreach (var k in playerStats)
                    {
                        wtr.Write((byte)k.Key);
                        if (ObjectStatus.IsStringStat(k.Key))
                            wtr.Write((string)k.Value);
                        else
                            wtr.Write((int)k.Value);
                    }
                }
                return (wtr.BaseStream as MemoryStream).ToArray();
            }
        }

        public static byte[] EnemyShoot(int bulletId, int ownerId, byte bulletType, Vector2 startPos, float angle, short damage, byte numShots, float angleInc)
        {
            using (var wtr = new PacketWriter(new MemoryStream()))
            {
                wtr.Write((byte)PacketId.EnemyShoot);
                wtr.Write(bulletId);
                wtr.Write(ownerId);
                wtr.Write(bulletType);
                startPos.Write(wtr);
                wtr.Write(angle);
                wtr.Write(damage);
                if (numShots > 1)
                {
                    wtr.Write(numShots);
                    wtr.Write(angleInc);
                }
                return (wtr.BaseStream as MemoryStream).ToArray();
            }
        }

        public static byte[] ShowEffect(ShowEffectIndex effect, int targetObjectId, uint color, Vector2 pos1 = new Vector2(), Vector2 pos2 = new Vector2())
        {
            using (var wtr = new PacketWriter(new MemoryStream()))
            {
                wtr.Write((byte)PacketId.ShowEffect);
                wtr.Write((byte)effect);
                wtr.Write(targetObjectId);
                wtr.Write((int)color);
                pos1.Write(wtr);
                if (pos2.X != 0 || pos2.Y != 0)
                    pos2.Write(wtr);
                return (wtr.BaseStream as MemoryStream).ToArray();
            }
        }

        public static byte[] Goto(int objectId, Vector2 pos)
        {
            using (var wtr = new PacketWriter(new MemoryStream()))
            {
                wtr.Write((byte)PacketId.Goto);
                wtr.Write(objectId);
                pos.Write(wtr);
                return (wtr.BaseStream as MemoryStream).ToArray();
            }
        }

        public static byte[] Aoe(Vector2 pos, float radius, int damage, ConditionEffectIndex effect, uint color)
        {
            using (var wtr = new PacketWriter(new MemoryStream()))
            {
                wtr.Write((byte)PacketId.Aoe);
                pos.Write(wtr);
                wtr.Write(radius);
                wtr.Write((short)damage);
                wtr.Write((byte)effect);
                wtr.Write((int)color);
                return (wtr.BaseStream as MemoryStream).ToArray();
            }
        }

        public static byte[] Damage(int targetId, ConditionEffectIndex[] effects, int damage)
        {
            using (var wtr = new PacketWriter(new MemoryStream()))
            {
                wtr.Write((byte)PacketId.Damage);
                wtr.Write(targetId);
                wtr.Write((byte)effects.Length);
                for (var i = 0; i < effects.Length; i++)
                    wtr.Write((byte)effects[i]);
                wtr.Write((ushort)damage);
                return (wtr.BaseStream as MemoryStream).ToArray();
            }
        }

        public static byte[] Death(int accountId, int charId, string killer)
        {
            using (var wtr = new PacketWriter(new MemoryStream()))
            {
                wtr.Write((byte)PacketId.Death);
                wtr.Write(accountId);
                wtr.Write(charId);
                wtr.Write(killer);
                return (wtr.BaseStream as MemoryStream).ToArray();
            }
        }

        public static byte[] AllyShoot(int ownerId, int containerType, float angle)
        {
            using (var wtr = new PacketWriter(new MemoryStream()))
            {
                wtr.Write((byte)PacketId.AllyShoot);
                wtr.Write(ownerId);
                wtr.Write((short)containerType);
                wtr.Write(angle);
                return (wtr.BaseStream as MemoryStream).ToArray();
            }
        }
        
        public static byte[] PlaySound(string sound)
        {
            using (var wtr = new PacketWriter(new MemoryStream()))
            {
                wtr.Write((byte)PacketId.PlaySound);
                wtr.Write(sound);
                return (wtr.BaseStream as MemoryStream).ToArray();
            }
        }

        public static byte[] Text(string name, int objectId, int numStars, int bubbleTime, string recipient, string text)
        {
            using (var wtr = new PacketWriter(new MemoryStream()))
            {
                wtr.Write((byte)PacketId.Text);
                wtr.Write(name);
                wtr.Write(objectId);
                wtr.Write(numStars);
                wtr.Write((byte)bubbleTime);
                wtr.Write(recipient);
                wtr.Write(text);
                return (wtr.BaseStream as MemoryStream).ToArray();
            }
        }

        public static byte[] AccountList(int accountListId, List<int> accountIds)
        {
            using (var wtr = new PacketWriter(new MemoryStream()))
            {
                wtr.Write((byte)PacketId.AccountList);
                wtr.Write(accountListId);
                wtr.Write((short)accountIds.Count);
                for (var i = 0; i < accountIds.Count; i++)
                    wtr.Write(accountIds[i]);
                return (wtr.BaseStream as MemoryStream).ToArray();
            }
        }

        public static byte[] ServerPlayerShoot(int bulletId, int ownerId, int containerType, Vector2 startPos, float angle, float angleInc, List<Projectile> projs)
        {
            using (var wtr = new PacketWriter(new MemoryStream()))
            {
                wtr.Write((byte)PacketId.ServerPlayerShoot);
                wtr.Write(bulletId);
                wtr.Write(ownerId);
                wtr.Write((short)containerType);
                startPos.Write(wtr);
                wtr.Write(angle);
                wtr.Write(angleInc);
                wtr.Write((byte)projs.Count);
                for (var i = 0; i < projs.Count; i++)
                    wtr.Write((short)projs[i].Damage);
                return (wtr.BaseStream as MemoryStream).ToArray();
            }
        }

        public static byte[] Reconnect(int gameId)
        {
            using (var wtr = new PacketWriter(new MemoryStream()))
            {
                wtr.Write((byte)PacketId.Reconnect);
                wtr.Write(gameId);
                return (wtr.BaseStream as MemoryStream).ToArray();
            }
        }

        public static byte[] Notification(int objectId, string text, uint color)
        {
            using (var wtr = new PacketWriter(new MemoryStream()))
            {
                wtr.Write((byte)PacketId.Notification);
                wtr.Write(objectId);
                wtr.Write(text);
                wtr.Write((int)color);
                return (wtr.BaseStream as MemoryStream).ToArray();
            }
        }

        public static byte[] QuestObjId(int objectId)
        {
            using (var wtr = new PacketWriter(new MemoryStream()))
            {
                wtr.Write((byte)PacketId.QuestObjId);
                wtr.Write(objectId);
                return (wtr.BaseStream as MemoryStream).ToArray();
            }
        }

        public static byte[] PolicyFile = _policyFile();
        static byte[] _policyFile()
        {
            using (var wtr = new PacketWriter(new MemoryStream()))
            {
                wtr.WriteNullTerminatedString(
                    @"<cross-domain-policy>" +
                    @"<allow-access-from domain=""*"" to-ports=""*"" />" +
                    @"</cross-domain-policy>");
                wtr.Write((byte)'\r');
                wtr.Write((byte)'\n');
                return (wtr.BaseStream as MemoryStream).ToArray();
            }
        }
    }
}
