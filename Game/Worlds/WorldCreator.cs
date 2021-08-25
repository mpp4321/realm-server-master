using System;
using System.Linq;
using RotMG.Common;
using RotMG.Networking;

namespace RotMG.Game.Worlds
{
    public static class WorldCreator
    {
        private static readonly Type[] Worlds;

        static WorldCreator()
        {
            var type = typeof(World);
            Worlds = type.Assembly.GetTypes()
                .Where(t => type.IsAssignableFrom(t) && type != t).ToArray();
        }

        public static World TryGetWorld(Map map, WorldDesc desc, Client client)
        {
            foreach (var world in Worlds)
            {
                if (!world.Name.Equals(desc.Name))
                    continue;

                if (desc.Name == "Vault" || desc.Name == "GuildHall")
                    return (World) Activator.CreateInstance(world, map, desc, client);
                return (World) Activator.CreateInstance(world, map, desc);
            }

            return new World(map, desc);
        }
    }
}