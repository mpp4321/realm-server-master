using System;
using System.Collections.Generic;
using System.Linq;
using RotMG.Common;
using RotMG.Networking;

namespace RotMG.Game.Entities
{
    public partial class Player
    {
        public enum TradeResult : byte
        {
            Successful,
            Canceled,
            Error
        }
        
        private const int TradeCooldown = 3000;
        
        public Player TradePartner;
        public Player PotentialPartner;
        public HashSet<int> TradedWith;
        public HashSet<int> PendingTrades;
        public bool[] Trade;
        public bool TradeAccepted;
        public int NextTradeTime;

        public void TradeRequest(string name)
        {
            if (name == Name)
            {
                SendError("Can not trade with yourself");
                return;
            }

            if (TradePartner != null)
            {
                SendError("Already trading");
                return;
            }

            var partner = Parent.Players
                .Where(x => x.Value.Name == name)
                .Select(x => x.Value).FirstOrDefault();
            if (partner == null)
            {
                SendError(name + " not found");
                return;
            }

            if (partner.Client.Account.IgnoredIds.Contains(AccountId))
                return;

            if (partner.TradePartner != null)
            {
                SendError(name + " is already trading");
                return;
            }

            if (PotentialPartner == null || !PotentialPartner.Equals(partner))
            {
                partner.PotentialPartner = this;
                if (!PendingTrades.Contains(partner.AccountId))
                {
                    Manager.AddTimedAction(20000, () => {TradeTimeout(partner);});
                    PendingTrades.Add(partner.AccountId);
                }
                partner.Client.Send(GameServer.TradeRequested(Name));
                SendInfo($"Trade request sent to {partner.Name}");
            }
            else
            {
                TradePartner = partner;
                Trade = new bool[12];
                TradeAccepted = false;
                PotentialPartner = null;
                TradedWith.Add(partner.AccountId);
                partner.TradePartner = this;
                partner.Trade = new bool[12];
                partner.TradeAccepted = false;
                partner.PotentialPartner = null;
                partner.TradedWith.Add(AccountId);
                
                var myItems = new TradeItem[12];
                var theirItems = new TradeItem[12];
                for (var i = 0; i < 12; i++)
                {
                    myItems[i] = new TradeItem
                    {
                        Item = Inventory[i],
                        ItemData = ItemDatas[i],
                        SlotType = Resources.Type2Player[Type].SlotTypes[i],
                        Included = false,
                        Tradeable = Inventory[i] != -1 && i >= 4 &&
                                    !Resources.Type2Item[(ushort) Inventory[i]].Soulbound &&
                                    Client.Account.Ranked == TradePartner.Client.Account.Ranked
                    };

                    theirItems[i] = new TradeItem
                    {
                        Item = partner.Inventory[i],
                        ItemData = partner.ItemDatas[i],
                        SlotType = Resources.Type2Player[partner.Type].SlotTypes[i],
                        Included = false,
                        Tradeable = partner.Inventory[i] != -1 && i >= 4 &&
                                    !Resources.Type2Item[(ushort) partner.Inventory[i]].Soulbound &&
                                    Client.Account.Ranked == TradePartner.Client.Account.Ranked
                    };
                }
                
                Client.Send(GameServer.TradeStart(partner.Name, myItems, theirItems));
                partner.Client.Send(GameServer.TradeStart(Name, theirItems, myItems));
            }
        }

        public void AcceptTrade(bool[] myOffer, bool[] theirOffer)
        {
            if (TradePartner == null)
            {
#if DEBUG
                Program.Print(PrintType.Error, $"{Name} tried trading without a partner");
#endif
                TradeDone(TradeResult.Canceled);
                return;
            }

            if (Manager.TotalTime < NextTradeTime)
            {
                SendError("Too early to accept trade");
                return;
            }
            
            if (TradeAccepted)
            {
#if DEBUG
                Program.Print(PrintType.Error, "Trade already accepted");
#endif
                return;
            }

            if (VerifyTrade(myOffer, this))
            {
                Program.Print(PrintType.Error, $"{Name} tried to trade a Soulbound item");
            }

            if (VerifyTrade(theirOffer, TradePartner))
            {
                Program.Print(PrintType.Error, $"{TradePartner.Name} tried to trade a Soulbound item");
            }

            Trade = myOffer;
            if (TradePartner.Trade.SequenceEqual(theirOffer))
            {
                var mySelectedTotal = 0;
                var theirSelectedTotal = 0;
                for (var i = 4; i < MaxSlotsWithoutBackpack; i++)
                {
                    if (myOffer[i])
                        mySelectedTotal++;

                    if (theirOffer[i])
                        theirSelectedTotal++;
                }

                if (mySelectedTotal > TradePartner.GetTotalFreeInventorySlots() + theirSelectedTotal ||
                    theirSelectedTotal > GetTotalFreeInventorySlots() + mySelectedTotal)
                    return;
                
                TradeAccepted = true;
                TradePartner.Client.Send(GameServer.TradeAccepted(theirOffer, myOffer));

                if (TradeAccepted && TradePartner.TradeAccepted)
                {
                    if (Client.Account.Ranked == TradePartner.Client.Account.Ranked)
                    {
                        DoTrade();
                    }
                    else
                    {
                        TradePartner.TradeDone(TradeResult.Canceled);
                        TradeDone(TradeResult.Canceled);
                    }
                }
            }
        }

        private void DoTrade()
        {
            var myItems = new List<Tuple<int, int>>();
            var theirItems = new List<Tuple<int, int>>();

            if (TradePartner == null || !TradePartner.Parent.Equals(Parent))
            {
#if DEBUG
                Program.Print(PrintType.Error, "Invalid trade target");
#endif
                TradeDone(TradeResult.Canceled);
                return;
            }

            if (!TradeAccepted || !TradePartner.TradeAccepted)
            {
#if DEBUG
                Program.Print(PrintType.Error, "No trade consent");
#endif
                return;
            }

            for (var i = 4; i < Trade.Length; i++)
            {
                if (Trade[i])
                {
                    myItems.Add(Tuple.Create(Inventory[i], ItemDatas[i]));
                    Inventory[i] = -1;
                }

                if (TradePartner.Trade[i])
                {
                    theirItems.Add(Tuple.Create(TradePartner.Inventory[i], TradePartner.ItemDatas[i]));
                    TradePartner.Inventory[i] = -1;
                }
            }

            foreach (var item in myItems)
            {
                for (var i = 4; i < MaxSlotsWithoutBackpack; i++)
                {
                    if (TradePartner.Inventory[i] == -1 ||
                        TradePartner.Trade[i])
                    {
                        TradePartner.Inventory[i] = item.Item1;
                        TradePartner.ItemDatas[i] = item.Item2;
                        TradePartner.Trade[i] = false;
                        break;
                    }
                }
            }

            foreach (var item in theirItems)
            {
                for (var i = 4; i < MaxSlotsWithoutBackpack; i++)
                {
                    if (Inventory[i] == -1 ||
                        Trade[i])
                    {
                        Inventory[i] = item.Item1;
                        ItemDatas[i] = item.Item2;
                        Trade[i] = false;
                        break;
                    }
                }
            }
            UpdateInventory();
            TradePartner.UpdateInventory();
            
            SaveToCharacter();
            TradePartner.SaveToCharacter();
            
            TradeDone(TradeResult.Successful);
        }

        public void ChangeTrade(bool[] newOffer)
        {
            if (TradePartner == null)
            {
#if DEBUG
                Program.Print(PrintType.Error, $"{Name} tried trading without a trade partner");
#endif
                return;
            }

            var triedSoulbound = VerifyTrade(newOffer, this);

            TradeAccepted = false;
            TradePartner.TradeAccepted = false;
            Trade = newOffer;
            NextTradeTime = Manager.TotalTime + TradeCooldown;
            
            TradePartner.Client.Send(GameServer.TradeChanged(Trade));

            if (triedSoulbound)
            {
                Program.Print(PrintType.Error, $"{Name} tried to trade a Soulbound item");
                SendError("Can not trade Soulbound items");
            }
        }

        public void TradeDone(TradeResult result)
        {
            Client.Send(GameServer.TradeDone(result));
            TradePartner?.Client.Send(GameServer.TradeDone(result));
            ResetTrade();
        }

        private bool VerifyTrade(bool[] trade, Player player)
        {
            var hadSoulbound = false;
            for (var i = 0; i < trade.Length; i++)
            {
                if (trade[i] && Resources.Type2Item[(ushort) player.Inventory[i]].Soulbound)
                {
                    hadSoulbound = true;
                    trade[i] = false;
                }
            }

            return hadSoulbound;
        }

        private void ResetTrade()
        {
            if (TradePartner != null)
            {
                TradePartner.TradePartner = null;
                TradePartner.Trade = null;
                TradePartner.TradeAccepted = false;
                PendingTrades.Remove(TradePartner.AccountId);
            }

            TradePartner = null;
            Trade = null;
            TradeAccepted = false;
            NextTradeTime = 0;
        }

        private void TradeTimeout(Player partner)
        {
            if (TradedWith.Contains(partner.AccountId))
            {
                TradedWith.Remove(partner.AccountId);
                partner.TradedWith.Remove(AccountId);
                return;
            }

            SendInfo($"Trade to {partner.Name} has timed out");
            PendingTrades.Remove(partner.AccountId);
        }
    }
}