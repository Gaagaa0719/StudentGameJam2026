using Cysharp.Threading.Tasks;
using NUnit.Framework.Interfaces;
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

            Item.OnHoverEnter += ShowDescription;
            Item.OnHoverExit += HideDescription;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Item.OnHoverEnter -= ShowDescription;
            Item.OnHoverExit -= HideDescription;
        }

        private void ShowDescription (Item item)
        {
            ItemNameText.text = item.DisplayName;
            ItemDescriptionText.text = item.SimpleDescription;
            FadeInAsync().Forget();
        }

        private void HideDescription(Item _)
        {
            FadeOutAsync().Forget();
        }

        protected override void OnAfterFadeOut()
        {
            Debug.Log("clean up");
            ItemNameText.text = "";
            ItemDescriptionText.text = "";
        }
    }
}