using RotMG.Common;

namespace RotMG.Game.Entities
{
    // Having an entire class might be overkill
    public class VaultChest : Container
    {
        private VaultChestModel _model;
        
        public VaultChest(VaultChestModel model) : base(0x0504, -1, null)
        {
            Inventory = model.Inventory;
            _model = model;
            UpdateInventory();
        }

        public override void UpdateInventorySlot(int slot)
        {
            base.UpdateInventorySlot(slot);
            _model.Save();
        }
    }
}