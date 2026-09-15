using TMPro;
using UnityEngine;

namespace Sakemottekoi.MainGame
{
    public class ItemDescriptionView : FadeUIBase
    {
        [Header("アイテム名テキスト")]
        [SerializeField] private TextMeshProUGUI ItemNameText;

        [Header("アイテム詳細テキスト")]
        [SerializeField] private TextMeshProUGUI ItemDescriptionText;

        protected override void Awake()
        {
            base.Awake();

            Item.OnHoverEnter += (ItemData) =>
            {
                ItemNameText.text = ItemData.DisplayName;
                ItemDescriptionText.text = ItemData.SimpleDescription;

                FadeIn();
            };

            Item.OnHoverExit += (_) => FadeOut();
        }

        protected override void OnAfterFadeOut()
        {
            Debug.Log("clean up");
            ItemNameText.text = "";
            ItemDescriptionText.text = "";
        }
    }
}