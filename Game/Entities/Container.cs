using System;
using RotMG.Common;

namespace RotMG.Game.Entities
{

    public interface IContainer
    {
        public int[] Inventory { get; set; }
        public ItemDataJson[] ItemDatas { get; set; }
        public void UpdateInventory();
        public void UpdateInventorySlot(int slot);
        public bool ValidSlot(int slot);
    }

    public class Container : Entity, IContainer
    {
        public const int MaxSlots = 8;

        public const ushort BrownBag = 0x0500;
        public const ushort PurpleBag = 0x0506;
        public const ushort CyanBag = 0x0507;
        public const ushort BlueBag = 0x0508;
        public const ushort SmallWhiteBag = 0x0509;
        public const ushort WhiteBag = 0x5002;

        public static ushort FromBagType(int bagType) 
        {
            switch (bagType) 
            {
                case 0: return BrownBag;
                case 1: return PurpleBag;
                case 2: return CyanBag;
                case 3: return BlueBag;
                case 4: return SmallWhiteBag;
                case 5: return WhiteBag;
            }
            throw new Exception("Invalid bag type");
        }
        
        private int _ownerId = -1;
        public int OwnerId
        {
            get => _ownerId;
            set => TrySetSV(StatType.OwnerAccountId, _ownerId = value);
        }
        
        public int[] Inventory { get; set; }
        public ItemDataJson[] ItemDatas { get; set; }

        public Container(ushort type, int ownerId, int? lifetime) : base(type, lifetime)
        {
            OwnerId = ownerId;
            Inventory = new int[MaxSlots];
            ItemDatas = new ItemDataJson[MaxSlots];
            for (var i = 0; i < MaxSlots; i++)
            {
                Inventory[i] = -1;
                ItemDatas[i] = new ItemDataJson() { Meta = 0 };
            }
        }

        public override void Tick()
        {
            if (Lifetime == null)
            {
                base.Tick();
                return;
            }
            
            var disappear = true;
            for (var i = 0; i < MaxSlots; i++)
                if (Inventory[i] != -1)
                {
                    disappear = false;
                    break;
                }

            if (disappear)
            {
                Parent.RemoveEntity(this);
                return;
            }

            base.Tick();
        }

        public bool ValidSlot(int slot)
        {
            if (slot < 0 || slot >= MaxSlots)
                return false;
            return true;
        }

        public void UpdateInventory()
        {
            for (var k = 0; k < MaxSlots; k++)
                UpdateInventorySlot(k);
        }

        public virtual void UpdateInventorySlot(int slot)
        {
#if DEBUG
            if (slot < 0 || slot >= MaxSlots)
                throw new Exception("Out of bounds slot update attempt.");
#endif
            switch (slot)
            {
                case 0: 
                    SetSV(StatType.Inventory0, Inventory[0]);
                    SetSV(StatType.ItemData0, ItemDatas[0]);
                    break;
                case 1: 
                    SetSV(StatType.Inventory1, Inventory[1]);
                    SetSV(StatType.ItemData1, ItemDatas[1]);
                    break;
                case 2: 
                    SetSV(StatType.Inventory2, Inventory[2]);
                    SetSV(StatType.ItemData2, ItemDatas[2]);
                    break;
                case 3: 
                    SetSV(StatType.Inventory3, Inventory[3]);
                    SetSV(StatType.ItemData3, ItemDatas[3]);
                    break;
                case 4: 
                    SetSV(StatType.Inventory4, Inventory[4]);
                    SetSV(StatType.ItemData4, ItemDatas[4]);
                    break;
                case 5: 
                    SetSV(StatType.Inventory5, Inventory[5]);
                    SetSV(StatType.ItemData5, ItemDatas[5]);
                    break;
                case 6: 
                    SetSV(StatType.Inventory6, Inventory[6]);
                    SetSV(StatType.ItemData6, ItemDatas[6]);
                    break;
                case 7:
                    SetSV(StatType.Inventory7, Inventory[7]);
                    SetSV(StatType.ItemData7, ItemDatas[7]);
                    break;
            }
        }
    }
}
