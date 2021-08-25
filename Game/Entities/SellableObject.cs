using System.Collections.Generic;
using RotMG.Common;
using RotMG.Networking;

namespace RotMG.Game.Entities
{
    public enum BuyResult : byte
    {
        Ok,
        IsGuest,
        InsufficientRank,
        InsufficientFunds,
        IsTestMap,
        Uninitialized,
        TransactionFailed,
        BeingPurchased,
        Admin
    }

    public interface ISellable
    {
        public void Buy(Player player);
    }

    public class SellableStatic : StaticObject, ISellable
    {
        private int _price;
        public int Price
        {
            get => _price;
            set => TrySetSV(StatType.MerchandisePrice, _price = value);
        }

        private Currency _currency;
        public Currency Currency
        {
            get => _currency;
            set => TrySetSV(StatType.MerchandiseCurrency, _currency = value);
        }

        private int _rankRequired;
        public int RankRequired
        {
            get => _rankRequired;
            set => TrySetSV(StatType.MerchandiseRankReq, _rankRequired = value);
        }

        public SellableStatic(ushort type) : base(type)
        {
            Price = 0;
            Currency = Currency.Fame;
        }

        public virtual void Buy(Player player)
        {
            SendFail(player, BuyResult.Uninitialized);
        }

        protected BuyResult ValidateCustomer(Player player)
        {
            if (Parent.Name.Contains("Dreamland"))
                return BuyResult.IsTestMap;
            if (player.NumStars < RankRequired)
                return BuyResult.InsufficientRank;

            if (player.GetCurrency(Currency) < Price)
                return BuyResult.InsufficientFunds;

            return BuyResult.Ok;
        }

        protected void SendFail(Player player, BuyResult message)
        {
            player.Client.Send(GameServer.BuyResult(1, message));
        }
    }
    
    public class SellableEntity : Entity, ISellable
    {
        private int _price;
        public int Price
        {
            get => _price;
            set => TrySetSV(StatType.MerchandisePrice, _price = value);
        }

        private Currency _currency;
        public Currency Currency
        {
            get => _currency;
            set => TrySetSV(StatType.MerchandiseCurrency, _currency = value);
        }

        private int _rankRequired;
        public int RankRequired
        {
            get => _rankRequired;
            set => TrySetSV(StatType.MerchandiseRankReq, _rankRequired = value);
        }

        public SellableEntity(ushort type) : base(type)
        {
            Price = 0;
            Currency = Currency.Fame;
        }

        public virtual void Buy(Player player)
        {
            SendFail(player, BuyResult.Uninitialized);
        }

        protected BuyResult ValidateCustomer(Player player)
        {
            if (Parent.Name.Contains("Dreamland"))
                return BuyResult.IsTestMap;
            if (player.NumStars < RankRequired)
                return BuyResult.InsufficientRank;

            if (player.GetCurrency(Currency) < Price)
                return BuyResult.InsufficientFunds;

            return BuyResult.Ok;
        }

        protected void SendFail(Player player, BuyResult message)
        {
            player.Client.Send(GameServer.BuyResult(1, message));
        }
    }
}