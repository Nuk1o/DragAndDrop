using UnityEngine;

namespace Inventory.Logic
{
    [CreateAssetMenu(fileName = "NewItem", menuName = "InventoryService/Item")]
    public class ItemData : ScriptableObject
    {
        public string itemName;
        public Sprite icon;
        public string description;
        public ItemType type;
        public bool isStackable = false;
    }
}