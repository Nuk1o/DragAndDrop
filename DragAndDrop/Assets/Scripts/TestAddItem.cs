using UnityEngine;
using Inventory.Logic;
using Inventory.UI;

public class TestAddItem : MonoBehaviour
{
    [SerializeField] private ItemData _testItem;
    [SerializeField] private ItemData _testItem2;
    [SerializeField] private ItemData _testItem3;
    [SerializeField] private ItemData _testItem4;
    [SerializeField] private ItemData _testItem5;
    [SerializeField] private ItemData _testItem6;
    [SerializeField] private InventoryUI _inventoryUI; 

    private void Start()
    {
        _inventoryUI.InventoryService.SetItem(0, _testItem, 1);
        _inventoryUI.InventoryService.SetItem(1, _testItem2, 2);
        _inventoryUI.InventoryService.SetItem(2, _testItem3, 1);
        _inventoryUI.InventoryService.SetItem(8, _testItem4, 8);
        _inventoryUI.InventoryService.SetItem(12, _testItem6, 1);
    }
}