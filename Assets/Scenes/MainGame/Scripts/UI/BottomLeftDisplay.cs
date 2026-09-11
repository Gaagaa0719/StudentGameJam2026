using UnityEngine;

namespace Sakemottekoi.MainGame
{
    public class BottomLeftDisplay : MonoBehaviour
    {
        [Header("アイテム詳細表示UI")]
        [SerializeField] private GameObject ItemDescriptionView;

        [Header("ログ表示UI")]
        [SerializeField] private GameObject LogView;

        private void Awake()
        {
            // アイテムにホバーされた時にログを非表示,アイテム詳細を表示
            Item.OnHoverEnter += (_) =>
            {
                ItemDescriptionView.SetActive(true);
                LogView.SetActive(false);
            };

            // アイテムのホバーが外れた時にログを表示,アイテム詳細を非表示
            Item.OnHoverExit += (_) =>
            {
                ItemDescriptionView.SetActive(false);
                LogView.SetActive(true);
            };
        }
    }
}