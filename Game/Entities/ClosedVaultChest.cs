using RotMG.Common;
using RotMG.Networking;

namespace RotMG.Game.Entities
{
    public class ClosedVaultChest : SellableEntity
    {
        public ClosedVaultChest() : base(0x0505)
        {
            Price = 100;
            Currency = Currency.Fame;
        }

        public override void Buy(Player player)
        {
            var result = ValidateCustomer(player);
            if (result != BuyResult.Ok)
            {
                SendFail(player, result);
                return;
            }

            var model = Database.UnlockVaultChest(player.Client.Account, Price, Currency);
            player.Client.Send(GameServer.BuyResult(0, BuyResult.Ok));
            var chest = new VaultChest(model);
            Parent.AddEntity(chest, Position);
            Parent.RemoveEntity(this);
        }
    }
}