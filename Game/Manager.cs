using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Game.Logic;
using RotMG.Networking;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using RotMG.Game.Worlds;
using System.Collections.Concurrent;

namespace RotMG.Game
{
    public static class Manager
    {
        public const int NexusId = -1;
        public const int RealmId = -2;
        public const int GuildId = -3;
        public const int EditorId = -4;
        public const int VaultId = -5;

        public static int NextWorldId;
        public static int NextClientId;

        // AppServer / Client needs access? might be causing account login issues...
        public static ConcurrentDictionary<int, int> AccountIdToClientId;
        public static ConcurrentDictionary<int, Client> Clients;
        public static ConcurrentDictionary<string, List<Client>> ClientsByIp;
        public static Dictionary<int, World> Worlds;
        public static Dictionary<int, World> Realms;
        public static List<Tuple<int, Action>> Timers;
        public static List<Action> PostponedActions;

        public static BehaviorDb Behaviors;
        public static Stopwatch TickWatch;

        public static int TotalTicks;
        public static int TotalTime;
        public static int TotalTimeUnsynced;
        public static int TickDelta;
        public static int LastTickTime;

        public static void Init()
        {
            Player.InitSightCircle();
            Player.InitSightRays();

            TickWatch = Stopwatch.StartNew();
            AccountIdToClientId = new ConcurrentDictionary<int, int>();
            Clients = new ConcurrentDictionary<int, Client>();
            ClientsByIp = new ConcurrentDictionary<string, List<Client>>();
            Worlds = new Dictionary<int, World>();
            Realms = new Dictionary<int, World>();
            Timers = new List<Tuple<int, Action>>();
            PostponedActions = new();

            Behaviors = new BehaviorDb();

            AddWorld(Resources.Worlds["Nexus"], NexusId);
            AddWorld(Resources.Worlds["Vault"], VaultId);
            AddWorld(Resources.Worlds["GuildHall"], GuildId);
        }

        public static void ReInitBehaviors()
        {
            Behaviors = new BehaviorDb();
        }

        public static World AddWorld(WorldDesc desc, Client client = null)
        {
            return AddWorld(desc, ++NextWorldId, client);
        }

        public static World AddWorld(WorldDesc desc, int id, Client client = null)
        {
            var world = WorldCreator.TryGetWorld(desc.Maps[MathUtils.Next(desc.Maps.Length)], desc, client);
            world.Id = id;
            Worlds[world.Id] = world;
            if (world is Realm)
                Realms[world.Id] = world;
#if DEBUG
            Program.Print(PrintType.Debug, $"Added World ID <{world.Id}> <{desc.Name}:{desc.DisplayName}>");
#endif
            return world;
        }

        public static int AddWorld(World world)
        {
            world.Id = ++NextWorldId;
            Worlds[world.Id] = world;
            return world.Id;
        }

        public static void RemoveWorld(World world)
        {
            if (!world.IsTemplate)
            {

                if (world.Portal?.Parent == null) {
                    var v = world.Portal?.LastWorldID ?? -1;
                    if(v != -1 && Worlds.ContainsKey(v))
                    {
                        StartOfTickAction(() => Worlds[v].RemoveEntity(world.Portal));
                    }
                } else
                    StartOfTickAction(() => world.Portal?.Parent?.RemoveEntity(world.Portal));
            }
            StartOfTickAction(() => world.Dispose());

            Worlds.Remove(world.Id);
            if (world is Realm)
                Realms.Remove(world.Id);
#if DEBUG
            Program.Print(PrintType.Debug, $"Removed World ID <{world.Id}> <{world.DisplayName}>");
#endif
        }

        public static World GetWorld(int id, Client client)
        {
            if (Worlds.TryGetValue(id, out var world))
                return world.GetInstance(client);
            return null;
        }

        public static Player GetPlayer(int accountid)
        {
            foreach (var client in Clients.Values)
                if (client.Player != null)
                    if (client.Player.AccountId == accountid)
                        return client.Player;
            return null;
        }

        public static Player GetPlayer(string name)
        {
            foreach (var client in Clients.Values)
                if (client.Player != null)
                    if (client.Player.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                        return client.Player;
            return null;
        }

        public static void AddClient(Client client, string ip)
        {
#if DEBUG
            if (client == null)
                throw new Exception("Client is null.");
#endif
            client.Id = ++NextClientId;
            Clients[client.Id] = client;
            if (!ClientsByIp.ContainsKey(ip))
            {
                ClientsByIp[ip] = new List<Client>();
            } 
            ClientsByIp[ip].Add(client);
        }

        public static void RemoveClient(Client client)
        {
#if DEBUG
            if (client == null)
                throw new Exception("Client is null.");
#endif
            Clients.Remove(client.Id, out var value);
        }

        public static Client GetClient(int accountId)
        {
            if (AccountIdToClientId.TryGetValue(accountId, out var clientId))
            {
                if(Clients.TryGetValue(clientId, out var client))
                {
                    return client;
                } else
                {
                    AccountIdToClientId.Remove(accountId, out var value);
                    return null;
                }
            }
            return null;
        }

        public static void AddTimedAction(int time, Action action)
        {
            Timers.Add(Tuple.Create(TotalTicks + TicksFromTime(time), action));
        }

        public static int TicksFromTime(int time)
        {
#if DEBUG
            if (time / (float)Settings.MillisecondsPerTick != time / Settings.MillisecondsPerTick)
                throw new Exception("Time out of sync with tick rate.");
#endif
            return time / Settings.MillisecondsPerTick;
        }

        public static void Announce(string text)
        {
            var announce = GameServer.Text("", 0, -1, 0, "", text);
            foreach(var client in Clients.Values)
            {
                if (client == null) continue;

                client.Send(announce);
            }
        }

        public static void Tick()
        {
            TotalTimeUnsynced = (int)TickWatch.ElapsedMilliseconds;

            foreach (var client in Clients.Values.ToArray())
            {
                client.Tick();
            }

            if ((int)TickWatch.ElapsedMilliseconds - LastTickTime >= Settings.MillisecondsPerTick - TickDelta)
            {
                LastTickTime = (int)TickWatch.ElapsedMilliseconds;

                var handlingActions = PostponedActions.ToArray();
                PostponedActions.Clear();
                foreach (var action in handlingActions)
                {
                    action();
                }

                foreach (var timer in Timers.ToArray())
                    if (timer.Item1 == TotalTicks)
                    {
                        timer.Item2();
                        Timers.Remove(timer);
                    }

                foreach (var world in Worlds.Values.ToArray())
                    world.Tick();

                TickDelta = (int)(TickWatch.ElapsedMilliseconds - LastTickTime);
                TotalTime += Settings.MillisecondsPerTick;
                TotalTicks++;
            }
        }

        /// <summary>
        ///  Runs an action on the beginning of a tick. Useful if you want to avoid nullifying a value in the middle of a process. 
        /// </summary>
        /// <param name="value">The action to postpone.</param>
        public static void StartOfTickAction(Action value)
        {
            PostponedActions.Add(value);
        }
    }
}
