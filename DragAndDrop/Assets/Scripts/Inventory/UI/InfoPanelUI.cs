using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class InfoPanelUI : MonoBehaviour
    {
        [SerializeField] private Image _imageItem;
        [SerializeField] private TMP_Text _nameItem;
        [SerializeField] private TMP_Text _descriptionItem;

        public void SetInfoItem(Sprite image, string itemName, string itemDescription)
        {
            _imageItem.sprite = image;
            _nameItem.text = itemName;
            _descriptionItem.text = itemDescription;
        }
    }
}