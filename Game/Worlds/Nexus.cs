using System.Collections.Generic;
using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Utils;

namespace RotMG.Game.Worlds
{
    public sealed class Nexus : World
    {
        private static int CurrentRealms;
        private static List<IntPoint> RealmSpawns;
        
        public Nexus(Map map, WorldDesc desc) : base(map, desc)
        {
            var vaultPos = GetRegion(Region.VaultPortal);
            if (vaultPos != new IntPoint(0, 0))
            {
                var type = Resources.Id2Object["Vault Portal"].Type;
                var portal = new Portal(type, null);
                AddEntity(portal, vaultPos.ToVector2() + .5f);
            }

            var guildPos = GetRegion(Region.GuildPortal);
            if (guildPos != new IntPoint(0, 0))
            {
                var type = Resources.Id2Object["Guild Hall Portal"].Type;
                var portal = new Portal(type, null);
                AddEntity(portal, guildPos.ToVector2() + .5f);
            }

            RealmSpawns = new List<IntPoint>(GetAllRegion(Region.RealmPortal));
            SpawnRealms();
        }

        public override void RemoveEntity(Entity en)
        {
            if (en is Portal portal)
                if (portal.Desc.Id == "Nexus Portal")
                {
                    --CurrentRealms;
                    RealmSpawns.Add(en.Position.ToIntPoint());
                    SpawnRealms();
                }
            base.RemoveEntity(en);
        }

        private void SpawnRealms()
        {
            while (CurrentRealms < Settings.MaxRealms && RealmSpawns.Count > 0)
            {
                var type = Resources.Id2Object["Nexus Portal"].Type;
                var portal = new Portal(type, null);
                var world = Manager.AddWorld(Resources.PortalId2World[type]);
                world.Portal = portal;
                portal.WorldInstance = world;
                portal.Name = world.DisplayName + " (0)";
                var index = MathUtils.Next(RealmSpawns.Count);
                var pos = RealmSpawns[index].ToVector2();
                RealmSpawns.RemoveAt(index);
                AddEntity(portal, pos + .5f);
                ++CurrentRealms;
            }
        }
    }
}