using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class ItemSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler,
        IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private TMP_Text _countText;
        [SerializeField] private float _doubleClickTime = 0.3f;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Button _dropButton;

        private int _slotIndex;
        private float _lastClickTime;
        private Logic.InventoryService _inventoryService;
        private static TooltipUI _tooltip;

        public event Action OnDoubleClicked;
        public event Action OnClicked;
        public event Action<int> OnDropRequested;
        public event Action<int> OnDragStart;
        public event Action<int> OnDroppedOn; 
        
        private static ItemSlotUI _draggedSlot;
        private GameObject _draggedIcon;

        public static void SetTooltip(TooltipUI tooltip) => _tooltip = tooltip;

        public void Initialize(int index, Logic.InventoryService inventoryService,Canvas canvas = null)
        {
            _slotIndex = index;
            _inventoryService = inventoryService;
            _canvas = canvas;
            
            _dropButton?.gameObject.SetActive(!_inventoryService.InventorySlots[_slotIndex].IsEmpty);
            
            if (_dropButton != null)
            {
                _dropButton.onClick.AddListener(OnDropButtonClicked);
            }
            
            UpdateDisplay();
        }

        public void UpdateDisplay()
        {
            var slot = _inventoryService.InventorySlots[_slotIndex];
            var hasItem = !slot.IsEmpty;

            _iconImage.enabled = hasItem;
            if (hasItem)
            {
                _iconImage.sprite = slot.Item.icon;

                if (_countText == null)
                    return;

                _countText.text = slot.Count > 1 ? slot.Count.ToString() : "";
                _countText.gameObject.SetActive(slot.Count > 1);
            }
            else if (_countText != null)
            {
                _countText.gameObject.SetActive(false);
                _tooltip?.Hide();
            }
            
            _dropButton?.gameObject.SetActive(hasItem);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            var slot = _inventoryService.InventorySlots[_slotIndex];

            if (slot.IsEmpty || _tooltip == null)
                return;

            var mousePos = eventData.position + new Vector2(10, 10);
            _tooltip.Show(slot.Item, mousePos);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _tooltip?.Hide();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            var timeSinceLastClick = Time.time - _lastClickTime;
            _lastClickTime = Time.time;

            if (timeSinceLastClick < _doubleClickTime)
            {
                OnDoubleClicked?.Invoke();
            }
            else
            {
                OnClicked?.Invoke();
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_inventoryService.InventorySlots[_slotIndex].IsEmpty)
                return;
            OnDragStart?.Invoke(_slotIndex);

            _draggedSlot = this;

            _draggedIcon = new GameObject("Dragged Icon");
            var image = _draggedIcon.AddComponent<Image>();
            image.sprite = _iconImage.sprite;
            image.raycastTarget = false;
            image.SetNativeSize();
            image.color = new Color(1, 1, 1, 0.7f);

            _draggedIcon.transform.SetParent(_canvas.transform, false);
            _draggedIcon.transform.SetAsLastSibling();
            _draggedIcon.SetActive(true);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_draggedIcon != null)
                _draggedIcon.transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_draggedIcon != null)
            {
                Destroy(_draggedIcon);
                _draggedIcon = null;
            }
            _draggedSlot = null;
        }

        public void OnDrop(PointerEventData eventData)
        {
            OnDroppedOn?.Invoke(_slotIndex);
        }
        
        private void OnDropButtonClicked()
        {
            OnDropRequested?.Invoke(_slotIndex);
        }
        
        private void OnDestroy()
        {
            if (_dropButton != null)
                _dropButton.onClick.RemoveListener(OnDropButtonClicked);
        }
    }
}