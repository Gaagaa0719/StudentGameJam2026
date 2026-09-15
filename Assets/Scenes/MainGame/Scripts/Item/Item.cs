using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sakemottekoi.MainGame
{
    public abstract class Item : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        /// <summary>
        /// アイテムがホバーされた際に発火するイベント
        /// </summary>
        public static event Action<Item> OnHoverEnter;

        /// <summary>
        /// アイテムのホバーが外れた際に発火するイベント
        /// </summary>
        public static event Action<Item> OnHoverExit;

        /// <summary>
        /// アイテムがマウスにクリックされた際に発火するイベント
        /// </summary>
        public static event Action<Item> OnClicked;

        /// <summary>
        /// アイテムが使用された際に発火するイベント
        /// </summary>
        public static event Action<Item> OnUsed;

        public Sprite ItemImage { private set;  get; }

        public abstract string DisplayName { get; }
        public abstract string SimpleDescription { get; }
        public abstract string Description { get; }

        [SerializeField] private Transform itemVisual;

        private void Awake()
        {
            if (!itemVisual) return;
            ItemImage = itemVisual.GetComponent<Image>().sprite;
        }

        /// <summary>
        /// アイテムを使用する時に使う関数
        /// </summary>
        public IEnumerator Use() {
            OnUsed?.Invoke(this);
            yield return InternalUse();
        }

        /// <summary>
        /// アイテムが使用された時に実行される処理を書く関数
        /// </summary>
        protected abstract IEnumerator InternalUse();

        /// <summary>
        /// アイテムがホバーされた時の処理
        /// </summary>
        public void OnPointerEnter(PointerEventData eventData)
        {
            itemVisual.localScale = new Vector3(1.1f, 1.1f, 1f);
            OnHoverEnter?.Invoke(this);
        }

        /// <summary>
        /// アイテムがホバーから外れた時の処理
        /// </summary>
        public void OnPointerExit(PointerEventData eventData)
        {
            itemVisual.localScale = Vector3.one;
            OnHoverExit?.Invoke(this);
        }

        /// <summary>
        /// アイテムがクリックされた時の処理
        /// </summary>
        public void OnPointerClick(PointerEventData eventData)
        {
            OnClicked?.Invoke(this);
        }
    }
}