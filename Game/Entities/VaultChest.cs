using RotMG.Common;
using System.Linq;

namespace RotMG.Game.Entities
{
    // Having an entire class might be overkill
    public class VaultChest : Container
    {
        private VaultChestModel _model;
        public VaultChest(VaultChestModel model) : base(0x0504, -1, null)
        {
            Inventory = model.Inventory;
            ItemDatas = model.ItemDatas;
            _model = model;

            if(model != null && model.Data != null)
                UpdateInventory();
        }

        public override void UpdateInventorySlot(int slot)
        {
            _model.ItemDatas = ItemDatas;
            base.UpdateInventorySlot(slot);
            _model.Save();
        }
    }
}