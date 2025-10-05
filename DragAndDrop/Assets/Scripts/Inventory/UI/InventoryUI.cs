using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [Header("Width and Height")]
        [SerializeField] private int _width;
        [SerializeField] private int _height;
        [Space]
        [SerializeField] private ItemSlotUI _slotPrefab;
        [SerializeField] private Transform _slotsContainer;
        [SerializeField] private GridLayoutGroup _gridLayoutGroup;
        [SerializeField] private TooltipUI _tooltipUI;
        [SerializeField] private Canvas _canvas;

        internal Logic.Inventory _inventory;
        private ItemSlotUI[] _slotUIs;
        private int? _draggedFromSlot = null;
        private bool _isDragging = false;

        private void Awake()
        {
            if (_inventory == null)
                _inventory = new Logic.Inventory(_width, _height);
            _gridLayoutGroup.constraintCount = _width;

            CreateSlots();
            UpdateAllSlots();

            _inventory.OnChanged += UpdateAllSlots;

            if (_tooltipUI != null)
                ItemSlotUI.SetTooltip(_tooltipUI);
        }

        private void OnDestroy()
        {
            if (_inventory != null)
                _inventory.OnChanged -= UpdateAllSlots;
        }

        private void CreateSlots()
        {
            _slotUIs = new ItemSlotUI[_inventory.Capacity];
            for (var i = 0; i < _inventory.Capacity; i++)
            {
                var slotUI = Instantiate(_slotPrefab, _slotsContainer);
                slotUI.Initialize(i, _inventory, _canvas);

                var index = i;
                slotUI.OnDoubleClicked += () => UseItem(index);
                slotUI.OnDragStart += OnDragStart;
                slotUI.OnDroppedOn += HandleDrop;
                slotUI.OnDropRequested += _inventory.DropItem;

                _slotUIs[i] = slotUI;
            }
        }
        
        private void OnDragStart(int slot) 
        { 
            _isDragging = true; 
            _draggedFromSlot = slot; 
        }

        private void UseItem(int index)
        {
            var slot = _inventory.InventorySlots[index];
            if (slot.IsEmpty)
                return;
            Debug.Log($"Использовал {slot.Item?.name}");
            _inventory.UsedItem(index);
        }

        private void UpdateAllSlots()
        {
            if (_isDragging)
                return;
            
            foreach (var slotUI in _slotUIs)
            {
                slotUI.UpdateDisplay();
            }
        }

        private void HandleDrop(int toSlotIndex)
        {
            _isDragging = false;
            
            if (_draggedFromSlot == null) return;

            var fromSlotIndex = _draggedFromSlot.Value;
            _draggedFromSlot = null;

            if (fromSlotIndex == toSlotIndex) return;

            var fromSlot = _inventory.InventorySlots[fromSlotIndex];
            var toSlot = _inventory.InventorySlots[toSlotIndex];

            if (fromSlot.IsEmpty) return;

            if (toSlot.IsEmpty)
            {
                _inventory.SetItemAt(toSlotIndex, fromSlot.Item, fromSlot.Count);
                _inventory.SetItemAt(fromSlotIndex, null);
            }
            else if (toSlot.Item == fromSlot.Item && toSlot.Item.isStackable)
            {
                _inventory.SetItemAt(toSlotIndex, toSlot.Item, toSlot.Count + fromSlot.Count);
                _inventory.SetItemAt(fromSlotIndex, null);
            }
            else
            {
                var tempItem = toSlot.Item;
                var tempCount = toSlot.Count;
                _inventory.SetItemAt(toSlotIndex, fromSlot.Item, fromSlot.Count);
                _inventory.SetItemAt(fromSlotIndex, tempItem, tempCount);
            }
        }
    }
}