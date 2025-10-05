using System;
using System.Collections.Generic;

namespace Inventory.Logic
{
    public class InventoryService
    {
        public readonly InventorySlot[] InventorySlots;
        public int Capacity => InventorySlots.Length;
        public event Action OnChanged;

        public InventoryService(int width, int height)
        {
            InventorySlots = new InventorySlot[width * height];
            for (var i = 0; i < InventorySlots.Length; i++)
            {
                InventorySlots[i] = new InventorySlot();
            }
        }
        
        public void SetItem(int index, ItemData item, int count = 1)
        {
            if (!IsValidIndex(index)) 
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

        public void UseItem(int index)
        {
            if (!IsValidIndex(index))
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
            if (!IsValidIndex(index))
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

            switch (sortingType)
            {
                case SortingType.Name:
                    itemsToSort.Sort((a, b) => 
                        string.Compare(a.item.name, b.item.name, StringComparison.OrdinalIgnoreCase));
                    break;
                case SortingType.Type:
                    itemsToSort.Sort((a, b) => a.item.type.CompareTo(b.item.type));
                    break;
                case SortingType.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sortingType), sortingType, null);
            }

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