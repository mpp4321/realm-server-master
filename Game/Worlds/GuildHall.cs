using RotMG.Common;
using RotMG.Networking;

namespace RotMG.Game.Worlds
{
    public sealed class GuildHall : World
    {
        private readonly string _guildName;
        
        public GuildHall(Map map, WorldDesc desc, Client client) : base(map, desc)
        {
            if (client == null)
                return;
            
            _guildName = client.Account.GuildName;
            var guild = Database.GetGuild(_guildName);
            OverwriteMap(Resources.Worlds[desc.Name].Maps[guild.Level]);
        }

        public override World GetInstance(Client client)
        {
            foreach (var world in Manager.Worlds.Values)
            {
                if (!(world is GuildHall) || (world as GuildHall)._guildName != client.Account.GuildName)
                    continue;

                return world;
            }

            var w = Manager.AddWorld(Resources.Worlds[Name], client);
            w.IsTemplate = false;
            return w;
        }

        public override bool AllowedAccess(Client client)
        {
            return base.AllowedAccess(client) && client.Account.GuildName == _guildName;
        }
    }
}