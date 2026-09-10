using System;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Item: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static event Action<Item> OnHoverEnter;
    public static event Action<Item> OnHoverExit;

    [SerializeField] private Transform itemVisual;

    public abstract void ChangeGlassParams(ref float alcohol, ref float amount);

    // アイテムがホバーされた時の処理
    public void OnPointerEnter(PointerEventData eventData)
    {
        itemVisual.localScale = new Vector3(1.1f, 1.1f, 1f);
        OnHoverEnter?.Invoke(this);
    }

    // アイテムがホバーから外れた時の処理
    public void OnPointerExit(PointerEventData eventData)
    {
        itemVisual.localScale = Vector3.one;
        OnHoverExit?.Invoke(this);
    }
}
