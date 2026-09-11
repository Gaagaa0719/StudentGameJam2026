using TMPro;
using UnityEngine;

namespace Sakemottekoi.MainGame
{
    public class ItemDescriptionView : MonoBehaviour
    {
        [Header("アイテム名テキスト")]
        [SerializeField] private TextMeshProUGUI ItemNameText;

        [Header("アイテム詳細テキスト")]
        [SerializeField] private TextMeshProUGUI ItemDescriptionText;

        private void Awake()
        {
            Item.OnHoverEnter += (ItemData) =>
            {
                ItemNameText.text = ItemData.DisplayName;
                ItemDescriptionText.text = ItemData.Description;
            };

            Item.OnHoverExit += (_) =>
            {
                ItemNameText.text = "";
                ItemDescriptionText.text = "";
            };
        }
    }
}