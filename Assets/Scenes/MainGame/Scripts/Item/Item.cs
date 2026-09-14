using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Sakemottekoi.MainGame
{
    public abstract class Item : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public static event Action<Item> OnHoverEnter;
        public static event Action<Item> OnHoverExit;
        public static event Action<Item> OnClicked;

        public abstract string DisplayName { get; }
        public abstract string Description { get; }

        [SerializeField] private Transform itemVisual;

        /// <summary>
        /// アイテムが使用された時の処理
        /// </summary>
        public abstract IEnumerator Use();

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