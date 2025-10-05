using Inventory.Logic;
using TMPro;
using UnityEngine;
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
        [SerializeField] private InfoPanelUI _infoPanelUI;
        [SerializeField] private Transform _slotsContainer;
        [SerializeField] private GridLayoutGroup _gridLayoutGroup;
        [SerializeField] private TooltipUI _tooltipUI;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private TMP_Dropdown _soringDropdown;
        [SerializeField] private SortingType[] _sortingOptions;

        internal Logic.InventoryService InventoryService;
        private ItemSlotUI[] _slotUIs;
        private int? _draggedFromSlot = null;
        private bool _isDragging = false;

        private void Awake()
        {
            if (InventoryService == null)
                InventoryService = new Logic.InventoryService(_width, _height);
            _gridLayoutGroup.constraintCount = _width;

            CreateSlots();
            UpdateAllSlots();

            InventoryService.OnChanged += UpdateAllSlots;

            if (_tooltipUI != null)
                ItemSlotUI.SetTooltip(_tooltipUI);
        }

        private void Start()
        {
            _soringDropdown.onValueChanged.AddListener(OnSortedItems); 
        }

        private void OnSortedItems(int index)
        {
            if (index < _sortingOptions.Length)
                InventoryService.Sorting(_sortingOptions[index]);
        }

        private void OnDestroy()
        {
            if (InventoryService != null)
                InventoryService.OnChanged -= UpdateAllSlots;
        }

        private void CreateSlots()
        {
            _slotUIs = new ItemSlotUI[InventoryService.Capacity];
            for (var i = 0; i < InventoryService.Capacity; i++)
            {
                var slotUI = Instantiate(_slotPrefab, _slotsContainer);
                slotUI.Initialize(i, InventoryService, _canvas);

                var index = i;
                slotUI.OnDoubleClicked += () => UseItem(index);
                slotUI.OnClicked += () => ShowInfoItem(index);
                slotUI.OnDragStart += OnDragStart;
                slotUI.OnDroppedOn += HandleDrop;
                slotUI.OnDropRequested += InventoryService.DropItem;

                _slotUIs[i] = slotUI;
            }
        }

        private void ShowInfoItem(int index)
        {
            var slot = InventoryService.InventorySlots[index];
            if (slot.IsEmpty)
                return;
            var item = slot.Item;
            _infoPanelUI.SetInfoItem(item.icon,item.itemName,item.description);
            _infoPanelUI.gameObject.SetActive(true);
        }

        private void OnDragStart(int slot) 
        { 
            _isDragging = true; 
            _draggedFromSlot = slot; 
        }

        private void UseItem(int index)
        {
            var slot = InventoryService.InventorySlots[index];
            if (slot.IsEmpty)
                return;
            Debug.Log($"Использовал {slot.Item?.name}");
            InventoryService.UseItem(index);
        }

        private void UpdateAllSlots()
        {
            if (_isDragging)
                return;
            
            foreach (var slotUI in _slotUIs)
            {
                slotUI.UpdateDisplay();
            }
            _infoPanelUI.gameObject.SetActive(false);
        }

        private void HandleDrop(int toSlotIndex)
        {
            _isDragging = false;
            
            if (_draggedFromSlot == null) return;

            var fromSlotIndex = _draggedFromSlot.Value;
            _draggedFromSlot = null;

            if (fromSlotIndex == toSlotIndex) return;

            var fromSlot = InventoryService.InventorySlots[fromSlotIndex];
            var toSlot = InventoryService.InventorySlots[toSlotIndex];

            if (fromSlot.IsEmpty) return;

            if (toSlot.IsEmpty)
            {
                InventoryService.SetItem(toSlotIndex, fromSlot.Item, fromSlot.Count);
                InventoryService.SetItem(fromSlotIndex, null);
            }
            else if (toSlot.Item == fromSlot.Item && toSlot.Item.isStackable)
            {
                InventoryService.SetItem(toSlotIndex, toSlot.Item, toSlot.Count + fromSlot.Count);
                InventoryService.SetItem(fromSlotIndex, null);
            }
            else
            {
                var tempItem = toSlot.Item;
                var tempCount = toSlot.Count;
                InventoryService.SetItem(toSlotIndex, fromSlot.Item, fromSlot.Count);
                InventoryService.SetItem(fromSlotIndex, tempItem, tempCount);
            }
        }
    }
}