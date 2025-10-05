using System;

namespace Inventory.Logic
{
    public class Inventory
    {
        public readonly InventorySlot[] InventorySlots;
        public int Capacity => InventorySlots.Length;
        public event Action OnChanged;

        public Inventory(int width, int height)
        {
            InventorySlots = new InventorySlot[width * height];
            for (var i = 0; i < InventorySlots.Length; i++)
            {
                InventorySlots[i] = new InventorySlot();
            }
        }
        
        public void SetItemAt(int index, ItemData item, int count = 1)
        {
            if (index < 0 || index >= InventorySlots.Length) 
                return;
    
            if (item == null)
            {
                InventorySlots[index].Clear();
            }
            else
            {
                if (!item.isStackable && count > 1) count = 1;
                InventorySlots[index].Set(item, count);
            }

            OnChanged?.Invoke();
        }

        public void UsedItem(int index)
        {
            if (index < 0 || index >= InventorySlots.Length)
                return;

            var slot = InventorySlots[index];
            if (slot.IsEmpty) 
                return;

            if (slot.Count > 1)
            {
                slot.Count -= 1;
            }
            else
            {
                slot.Clear();
            }

            OnChanged?.Invoke();
        }

        public void DropItem(int index)
        {
            if (index < 0 || index >= InventorySlots.Length)
                return;
    
            InventorySlots[index].Clear();
            OnChanged?.Invoke();
        }
    }
}