using Inventory.Logic;
using TMPro;
using UnityEngine;

namespace Inventory.UI
{
    public class TooltipUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _itemNameText;
        [SerializeField] private TMP_Text _descriptionText;

        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = (RectTransform)transform;
        }

        public void Show(ItemData item, Vector2 position)
        {
            if (item == null) return;

            _itemNameText.text = item.itemName;
            _descriptionText.text = item.description;

            gameObject.SetActive(true);
            _rectTransform.position = position;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}