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
        public static Dictionary<int, int> AccountIdToClientId;
        public static Dictionary<int, Client> Clients;
        public static Dictionary<int, World> Worlds;
        public static Dictionary<int, World> Realms;
        public static List<Tuple<int, Action>> Timers;
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
            AccountIdToClientId = new Dictionary<int, int>();
            Clients = new Dictionary<int, Client>();
            Worlds = new Dictionary<int, World>();
            Realms = new Dictionary<int, World>();
            Timers = new List<Tuple<int, Action>>();

            Behaviors = new BehaviorDb();

            AddWorld(Resources.Worlds["Nexus"], NexusId);
            AddWorld(Resources.Worlds["Vault"], VaultId);
            AddWorld(Resources.Worlds["GuildHall"], GuildId);
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
                world.Portal?.Parent.RemoveEntity(world.Portal);
            world.Dispose();
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

        public static Player GetPlayer(string name)
        {
            foreach (var client in Clients.Values)
                if (client.Player != null)
                    if (client.Player.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                        return client.Player;
            return null;
        }

        public static void AddClient(Client client)
        {
#if DEBUG
            if (client == null)
                throw new Exception("Client is null.");
#endif
            client.Id = ++NextClientId;
            Clients[client.Id] = client;
        }

        public static void RemoveClient(Client client)
        {
#if DEBUG
            if (client == null)
                throw new Exception("Client is null.");
#endif
            Clients.Remove(client.Id);
        }

        public static Client GetClient(int accountId)
        {
            if (AccountIdToClientId.TryGetValue(accountId, out var clientId))
                return Clients[clientId];
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

        public static void Tick()
        {
            TotalTimeUnsynced = (int)TickWatch.ElapsedMilliseconds;

            foreach (var client in Clients.Values.ToArray())
                client.Tick();

            if ((int)TickWatch.ElapsedMilliseconds - LastTickTime >= Settings.MillisecondsPerTick - TickDelta)
            {
                LastTickTime = (int)TickWatch.ElapsedMilliseconds;

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
    }
}
