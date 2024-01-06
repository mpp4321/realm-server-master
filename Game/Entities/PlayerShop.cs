using RotMG.Common;
using RotMG.Networking;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Entities
{
    public class PlayerShop : SellableEntity
    {

        public ItemDataJson itemJson = null;

        public bool Sold = false;

        public int AccountId;
        public int MarketId;

        public PlayerShop(ushort type, int accId, int marketId) : base(type)
        {
            AccountId = accId;
            MarketId = marketId; 
        }


        public override void Buy(Player player)
        {
            if (Sold) return;
            var account = player.Client.Account;

            if(!Database.IsValidMarketPost(AccountId, MarketId))
            {
                Parent.RemoveEntity(this);
                player.SendInfo("This post has sold or been removed.");
                return;
            }

            if (account.Stats.Fame < Price)
            {
                SendFail(player, BuyResult.InsufficientFunds);
                return;
            }


            int slot;
            if((slot = player.GetFreeInventorySlot()) != -1)
            {

                player.Inventory[slot] = (int) SVs[StatType.MerchandiseType];
                if(itemJson != null)
                {
                    player.ItemDatas[slot] = itemJson;
                }
            } else
            {

                player.SendInfo("Inventory full, sent to gift chest.");
                Database.AddGift(account, (int) SVs[StatType.MerchandiseType], itemJson);

            }

            player.UpdateInventory();

            Database.RemoveMarketPost(AccountId, MarketId);
            Parent.RemoveEntity(this);
            Sold = true;
            Database.IncrementCurrency(account, -Price, Currency);
            Database.IncrementCurrency(new AccountModel(AccountId), Price, Currency);
            player.Client.Send(GameServer.BuyResult(0, BuyResult.Ok));
        }
    }
}
