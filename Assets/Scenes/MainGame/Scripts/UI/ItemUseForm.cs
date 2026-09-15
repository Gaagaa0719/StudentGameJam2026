using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace Sakemottekoi.MainGame
{
    public class ItemUseForm : FadeUIBase
    {
        [SerializeField] Image ItemIcon;
        [SerializeField] TextMeshProUGUI ItemName;
        [SerializeField] TextMeshProUGUI ItemDescription;

        [SerializeField] Button CloseButton;
        [SerializeField] Button UseButton;

        private Item showingItem;

        protected override void Awake()
        {
            base.Awake();
            Item.OnClicked += ShowForm;
            CloseButton.onClick.AddListener(() => StartCoroutine(Hide()));
            UseButton.onClick.AddListener(() => { if (showingItem != null) StartCoroutine(showingItem.Use()); });
        }

        private void ShowForm(Item item)
        {
            if (GameManager.Instance.CurrentPhase != GamePhase.ItemUse) return;
            ItemIcon.sprite = item.ItemImage;
            ItemName.text = item.DisplayName;
            ItemDescription.text = item.Description;

            showingItem = item;

            StartCoroutine(Show());
        }
    }
}