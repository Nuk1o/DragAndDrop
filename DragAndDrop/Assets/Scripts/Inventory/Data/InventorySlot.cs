namespace Inventory.Logic
{
    public class InventorySlot
    {
        public ItemData Item;
        public int Count;
        public bool IsEmpty => Item == null || Count <= 0;

        public void Clear()
        {
            Item = null;
            Count = 0;
        }

        public void Set(ItemData item, int count = 1)
        {
            Item = item;
            Count = count;
        }
    }
}