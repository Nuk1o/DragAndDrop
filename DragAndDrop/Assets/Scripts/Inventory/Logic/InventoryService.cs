using System;
using System.Collections.Generic;

namespace Inventory.Logic
{
    public class InventoryService
    {
        public readonly InventorySlot[] InventorySlots;
        public int Capacity => InventorySlots.Length;
        public event Action OnChanged;

        public Dictionary<SortingType, BaseSort> Sorts;

        public InventoryService(int width, int height)
        {
            Sorts = new Dictionary<SortingType, BaseSort>()
            {
                { SortingType.Name, new BaseSortToName()},
                { SortingType.Type, new BaseSortToType()}
            };
            
            InventorySlots = new InventorySlot[width * height];
            for (var i = 0; i < InventorySlots.Length; i++)
            {
                InventorySlots[i] = new InventorySlot();
            }
        }
        
        public void SetItem(int index, ItemData item, int count = 1)
        {
            if (IsValidIndex(index)) 
                return;
    
            if (item == null)
            {
                InventorySlots[index].Clear();
            }
            else
            {
                if (!item.isStackable && count > 1)
                    count = 1;
                
                InventorySlots[index].Set(item, count);
            }

            OnChanged?.Invoke();
        }
        
        public void SetItem(int index, InventorySlot slotData)
        {
            SetItem(index, slotData.Item, slotData.Count);
        }

        public void UseItem(int index)
        {
            if (IsValidIndex(index))
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
            if (IsValidIndex(index))
                return;
    
            InventorySlots[index].Clear();
            OnChanged?.Invoke();
        }

        public void Sorting(SortingType sortingType)
        {
            var itemsToSort = new List<(ItemData item, int count)>();
    
            foreach (var slot in InventorySlots)
            {
                if (!slot.IsEmpty)
                {
                    itemsToSort.Add((slot.Item, slot.Count));
                }
            }
            
            itemsToSort = Sorts[sortingType].Sort(itemsToSort);

            foreach (var slot in InventorySlots)
            {
                slot.Clear();
            }

            for (var i = 0; i < itemsToSort.Count; i++)
            {
                var (item, count) = itemsToSort[i];
                InventorySlots[i].Set(item, count);
            }

            OnChanged?.Invoke();
        }

        private bool IsValidIndex(int index)
        {
            return index < 0 || index >= InventorySlots.Length;
        }
    }
}