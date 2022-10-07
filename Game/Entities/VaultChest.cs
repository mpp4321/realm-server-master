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
            ItemDatas = model.ItemDatas.Select(a => a == null ? new ItemDataJson() { Meta = -1 } : ItemDesc.ParseOrDefault(a)).ToArray();
            _model = model;

            if(model != null && model.Data != null)
                UpdateInventory();
        }

        public override void UpdateInventorySlot(int slot)
        {
            base.UpdateInventorySlot(slot);
            _model.ItemDatas = ItemDatas.Select(a => ItemDesc.ExportItemDataJson(a)).ToArray();
            _model.Save();
        }
    }
}