using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotMG.Game.Worlds
{
    public sealed class Market : World
    {
        private uint t = 0;
        
        public Market(Map map, WorldDesc desc) : base(map, desc)
        {
            InitShops();
        }

        public override void Tick()
        {
            base.Tick();
            t++;
            if (t % 600 == 0)
                InitShops();
        }

        private void InitShops()
        {
            List<Entity> toRemove = new List<Entity>();
            foreach(var entity in Entities.Values)
            {
                if(entity is PlayerShop)
                {
                    toRemove.Add(entity);
                }
            }
            foreach (var tr in toRemove)
                RemoveEntity(tr);
            var marketShopItems = Database.GetAllMarketPosts().ToList();
            foreach (var shop1Point in GetAllRegion(Region.Store1))
            {
                if (marketShopItems.Count() == 0) break;
                var indx = MathUtils.Next(marketShopItems.Count);
                var randomChoice = marketShopItems[indx];
                marketShopItems.RemoveAt(indx);
                CreateShopAt(randomChoice.AccountId, randomChoice.Id, shop1Point, randomChoice.Item, randomChoice.Price, Currency.Fame, randomChoice.Json);
            }
        }

        private void CreateShopAt(int accountId, int marketId, IntPoint shop1Point, int type, int price, Currency currency, ItemDataJson json=null)
        {
            var shopEntity = new PlayerShop(0x01ca, accountId, marketId)
            {
                itemJson = json
            };

            //So client gets these values
            shopEntity.SetSV(StatType.MerchandiseType, type);
            shopEntity.SetSV(StatType.MerchandiseCount, 999);
            // The parsers handles null technically
            shopEntity.SetSV(StatType.MerchantItemData, json);

            shopEntity.Price = price;
            shopEntity.Currency = currency;

            AddEntity(shopEntity, shop1Point.ToVector2());
        }
    }
}
