using RotMG.Common;
using RotMG.Networking;

namespace RotMG.Game.Entities
{
    public class GuildMerchant : SellableStatic
    {
        private readonly int[] _hallTypes = {0x736, 0x737, 0x738};
        private readonly int[] _hallPrices = {10000, 100000, 250000};
        private readonly int[] _hallLevels = {1, 2, 3};

        private readonly int _upgradeLevel;

        public GuildMerchant(ushort type) : base(type)
        {
            Currency = Currency.GuildFame;
            Price = int.MaxValue;
            for (var i = 0; i < _hallTypes.Length; i++)
            {
                if (type != _hallTypes[i])
                    continue;

                Price = _hallPrices[i];
                _upgradeLevel = _hallLevels[i];
            }
        }

        public override void Buy(Player player)
        {
            var account = player.Client.Account;
            var guild = Database.GetGuild(account.GuildName);

            if (guild == null || account.GuildRank < 30)
            {
                player.SendError("No permission");
                return;
            }

            if (guild.Fame < Price)
            {
                SendFail(player, BuyResult.InsufficientFunds);
                return;
            }

            if (!Database.ChangeGuildLevel(guild, _upgradeLevel))
            {
                player.SendError("Internal server error");
                return;
            }
            
            Database.IncrementCurrency(guild, -Price);
            player.Client.Send(GameServer.BuyResult(0, BuyResult.Ok));
        }
    }
}