using System;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Item: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static event Action<Item> OnHoverEnter;
    public static event Action<Item> OnHoverExit;

    public abstract void ChangeGlassParams(ref float alcohol, ref float amount);

    // アイテムがホバーされた時の処理
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHoverEnter?.Invoke(this);
    }

    // アイテムがホバーから外れた時の処理
    public void OnPointerExit(PointerEventData eventData)
    {
        OnHoverExit?.Invoke(this);
    }
}
