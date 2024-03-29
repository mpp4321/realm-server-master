﻿using RotMG.Common;
using RotMG.Networking;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Entities
{
    internal class NexusShop : SellableEntity
    {

        public ItemDataJson itemJson = null;

        public NexusShop(ushort type) : base(type)
        {
        }


        public override void Buy(Player player)
        {
            var account = player.Client.Account;

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

            Database.IncrementCurrency(account, -Price, Currency);
            player.Client.Send(GameServer.BuyResult(0, BuyResult.Ok));
        }
    }
}
