using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Sakemottekoi.MainGame
{
    public class ItemUseForm : FadeUIBase
    {
        [SerializeField] Image itemIcon;
        [SerializeField] TextMeshProUGUI itemName;
        [SerializeField] TextMeshProUGUI itemDescription;

        [SerializeField] Button closeButton;
        [SerializeField] Button useButton;

        private UniTaskCompletionSource<bool> pending;

        protected override void Awake()
        {
            base.Awake();
            closeButton.onClick.AddListener(() => Submit(false));
            useButton.onClick.AddListener(() => Submit(true));
        }

        public UniTask<bool> ShowAsync(Item item)
        {
            if (pending != null)
                throw new InvalidOperationException("ItemUseForm is already showing.");
            pending = new UniTaskCompletionSource<bool>();

            // 必要な項目を設定
            itemIcon.sprite = item.ItemImage;
            itemName.text = item.DisplayName;
            itemDescription.text = item.Description;

            // 表示処理
            FadeIn();
            return pending.Task;
        }

        private void Submit(bool result)
        {
            if (pending == null) return;

            var completion = pending;
            pending = null;

            FadeOut();
            completion.TrySetResult(result);
        }
    }
}