using RotMG.Common;
using RotMG.Game;
using RotMG.Game.Entities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace RotMG.Networking
{
    public enum ProtocolState
    {
        Handshaked, //Indicates that the client has initialized a connection and is now waiting for Hello packet.
        Awaiting, //Received Hello and is now waiting for a Load/Create packet to put the player in game.
        Connected, //Indicates that the client is now fully initialized and is in game.
        Disconnected //Packets received will no longer be processed and the server will disconnect the client.
    }

    public class Client
    {
        public ProtocolState State;
        public int Id;
        public int TargetWorldId;
        public string IP;

        public AccountModel Account;
        public CharacterModel Character;
        public Player Player;
        public wRandom Random;
        public bool Active; //Used in escape to stop incoming packets (so you don't die)
        public bool IsReconnecting = false;
        public int DCTime;

        private Socket _socket;
        private Queue<byte[]> _pending;
        private SendState _send;
        private ReceiveState _receive;

        public Client(SendState send, ReceiveState receive)
        {
            _pending = new Queue<byte[]>();
            _send = send;
            _receive = receive;
        }

        public bool HasEffectsEnabled()
        {
            return Account?.Effects ?? false;
        }
        public void DisconnectNoAddBack() // clears all individual client data without pushing instance back
        {
            // Still
            try
            {
                //Save what's needed
                if (Account != null)
                {
                    Account.Connected = false;
                    Account.LastSeen = Database.UnixTime();
                    Account.Save();
                    Manager.AccountIdToClientId.Remove(Account.Id, out var value);
                }
            } catch (Exception ex)
            {
                Program.Print(PrintType.Error, ex.ToString());
                Manager.AccountIdToClientId.Remove(Account.Id, out var value);
            }

            try
            {
                //Save what's needed
                if(Player != null)
                {
                    Player.TradeDone(Player.TradeResult.Canceled);
                    Player.SaveToCharacter();

                    if (!Character.Dead) //Already saved during death.
                    {
                        Database.SaveCharacter(Character);
                    }
                }

            }
            catch (Exception ex)
            {
                Program.Print(PrintType.Error, ex.ToString());
            }

            if(Player?.Parent != null)
                Player.Parent.RemoveEntity(Player);

            if(State != ProtocolState.Disconnected)
            {
                try
                {
                    _socket.Shutdown(SocketShutdown.Both);
                    _socket.Close();
                }
                catch(Exception ex)
                {
                    Program.Print(PrintType.Error, ex);
                }

                //Push back client to queue. Once!
                GameServer.AddBack(this);
            }

            //Shutdown socket
            State = ProtocolState.Disconnected;

            //Clear data 
            Active = false;
            IsReconnecting = false;

            Program.PushWork(() =>
            {
                if(this != null && IP != null)
                    Manager.ClientsByIp[IP]?.Remove(this);
            });
            
            _send.Reset();
            _receive.Reset();
            _pending.Clear();
            Account = null;
            CleanupPet();
            Player = null;
            Character = null;
            Random = null;
            TargetWorldId = -1;
        }

        public void Disconnect() //Disconnects, clears all individual client data and pushes the instance back to the server queue.
        {
            // clear data
            DisconnectNoAddBack();

            // remove client
            Manager.RemoveClient(this);
        }

        private void CleanupPet()
        {
            if (Player?.Pet != null)
            {
                Player.Pet.Parent?.RemoveEntity(Player.Pet);
            }
        }

        public void BeginHandling(Socket socket, string ip)
        {
            _socket = socket;
            _socket.Blocking = false;

            State = ProtocolState.Handshaked;
            IP = ip;
            Active = true;
            DCTime = -1;

            Manager.AddClient(this, ip);
        }

        public void Tick()
        {
            try
            {
                if (!_socket.Connected)
                {
                    Disconnect();
                    return;
                }

                StartReceive();
                StartSend();
            }
#if DEBUG
            catch (Exception ex)
            {
                Program.Print(PrintType.Error, ex.ToString());
                Disconnect();
            }
#endif
#if RELEASE
            catch 
            { 
                Disconnect(); 
            }
#endif
        }

        public void Send(byte[] packet)
        {
            _pending.Enqueue(packet);
        }

        private void StartReceive()
        {
            while(true)
            {
                switch (_receive.State)
                {
                    case SocketEventState.Awaiting:
                        if (_socket.Available >= GameServer.PrefixLength)
                        {
                            _socket.Receive(_receive.PacketBytes, GameServer.PrefixLength, SocketFlags.None);
                            _receive.PacketLength = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(_receive.PacketBytes, 0));
                            _receive.State = SocketEventState.InProgress;
                            // removing recursion because of overflow StartReceive();
                            continue;
                        }
                        return;
                    case SocketEventState.InProgress:
                        if (_receive.PacketLength == 1014001516) //Hacky policy file..
                        {
                            _socket.Send(GameServer.PolicyFile, 0, GameServer.PolicyFile.Length, SocketFlags.None);
                            return;
                        }

                        if (_receive.PacketLength < GameServer.PrefixLength ||
                            _receive.PacketLength > GameServer.BufferSize)
                        {
                            Disconnect();
                            return;
                        }

                        if (_socket.Available + GameServer.PrefixLength >= _receive.PacketLength) //Full packet now arrived. Time to process it.
                        {
                            if (_socket.Available != 0)
                                _socket.Receive(_receive.PacketBytes, GameServer.PrefixLength, _receive.PacketLength - GameServer.PrefixLength, SocketFlags.None);
                            GameServer.Read(this, _receive.GetPacketId(), _receive.GetPacketBody());
                            _receive.Reset();
                        }

                        //remove recursion because of overflow StartReceive();
                        continue;
                }
            }
        }

        private void StartSend()
        {
            switch (_send.State)
            {
                case SocketEventState.Awaiting:
                    if (_pending.TryDequeue(out var packet))
                    {
                        _send.PacketBytes = packet;
                        _send.PacketLength = packet.Length;
                        _send.State = SocketEventState.InProgress;
                        StartSend();
                    }
                    break;
                case SocketEventState.InProgress:
                    Buffer.BlockCopy(_send.PacketBytes, 0, _send.Data, GameServer.PrefixLengthWithId, _send.PacketLength);
                    Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(_send.PacketLength + GameServer.PrefixLengthWithId)), 0, _send.Data, 0, GameServer.PrefixLengthWithId);
                    var written = _socket.Send(_send.Data, _send.BytesWritten, _send.PacketLength + GameServer.PrefixLengthWithId - _send.BytesWritten, SocketFlags.None);
                    if (written < _send.PacketLength + GameServer.PrefixLengthWithId)
                        _send.BytesWritten += written;
                    else
                        _send.Reset();
                    StartSend();
                    break;
            }
        }
    }

    public class wRandom
    {
        private uint _seed;

        public wRandom(uint seed)
        {
            _seed = seed;
        }

        public void Drop(int count)
        {
            for (var i = 0; i < count; i++)
                Gen();
        }

        public uint NextIntRange(uint min, uint max)
        {
            return min == max ? min : min + Gen() % (max - min);
        }
        public float NextFloat()
        {
            return 2 * (Gen() >> 8) / (1 << 24);
        }

        private uint Gen()
        {
            var lb = 16807 * (_seed & 0xFFFF);
            var hb = 16807 * (_seed >> 16);
            lb = lb + ((hb & 32767) << 16);
            lb = lb + (hb >> 15);
            if (lb > 2147483647)
            {
                lb = lb - 2147483647;
            }
            return _seed = lb;
        }
    }
}
