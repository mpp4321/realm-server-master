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

            var marketPos = GetRegion(Region.Note);
            if (marketPos != new IntPoint(0,0))
            {
                var type = Resources.Id2Object["Market Portal"].Type;
                var portal = new Portal(type, null);
                AddEntity(portal, marketPos.ToVector2() + .5f);
            }

            RealmSpawns = new List<IntPoint>(GetAllRegion(Region.RealmPortal));

            InitShops();

            SpawnRealms();
        }

        private void InitShops()
        {
            foreach (var shop1Point in GetAllRegion(Region.Store1))
            {
                var shopEntity = new NexusShop(0x01ca);

                //So client gets these values
                shopEntity.TrySetSV(StatType.MerchandiseType, (int) Resources.Id2Item["Backpack"].Type);
                shopEntity.TrySetSV(StatType.MerchandiseCount, 999);

                shopEntity.Price = 25;
                shopEntity.Currency = Currency.Fame;

                AddEntity(shopEntity, shop1Point.ToVector2());
            }

            foreach (var shop1Point in GetAllRegion(Region.Store2))
            {
                var shopEntity = new NexusShop(0x01ca);

                //So client gets these values
                shopEntity.TrySetSV(StatType.MerchandiseType, 0xc8a);
                shopEntity.TrySetSV(StatType.MerchandiseCount, 999);

                shopEntity.Price = 15;
                shopEntity.Currency = Currency.Fame;


                AddEntity(shopEntity, shop1Point.ToVector2());
            }

            var alternating = 0;
            foreach (var shop1Point in GetAllRegion(Region.Store3))
            {
                if((++alternating) % 2 == 1)
                {
                    CreateShopAt(shop1Point, 0xcbb, 100, Currency.Fame);
                } else {
                    CreateShopAt(shop1Point, Resources.Id2Item["Fishing Rod"].Type, 100, Currency.Fame);
                }
            }

            foreach(var shopPoint in GetAllRegion(Region.Store4))
            {
                if((++alternating) % 3 == 1)
                {
                    CreateShopAt(shopPoint, Resources.Id2Item["Realm Equipment Crystal"].Type, 25, Currency.Fame);
                } else if (alternating % 3 == 2) {
                    CreateShopAt(shopPoint, Resources.Id2Item["Oryx Equipment Crystal"].Type, 100, Currency.Fame);
                } else
                {
                    CreateShopAt(shopPoint, Resources.Id2Item["Legendary Transformation Bag"].Type, 1500, Currency.Fame);
                }
            }
        }

        private void CreateShopAt(IntPoint shop1Point, int type, int price, Currency currency, ItemDataJson itemDataJson = null)
        {
            var shopEntity = new NexusShop(0x01ca)
            {
                itemJson = itemDataJson
            };

            //So client gets these values
            shopEntity.TrySetSV(StatType.MerchandiseType, type);
            shopEntity.TrySetSV(StatType.MerchandiseCount, 999);

            shopEntity.Price = price;
            shopEntity.Currency = currency;

            AddEntity(shopEntity, shop1Point.ToVector2());
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