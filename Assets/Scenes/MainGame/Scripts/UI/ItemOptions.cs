using System.Collections.Generic;
using UnityEngine;
using Sakemottekoi.MainGame;

public class ItemOptions : PhaseFadeUI
{
    public static ItemOptions Instance { private set; get; }

    protected override GamePhase TargetPhase => GamePhase.ItemSelection;

    [SerializeField]
    private List<RectTransform> itemHolders = new ();

    [SerializeField]
    private List<GameObject> lootItems = new ();

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }

    // アイテムのないホルダーにアイテムを設定する関数。
    public void RestockItems()
    {
        foreach (var holder in itemHolders)
        {
            // アイテムを持たないホルダーは対象外
            if (holder.childCount != 0) continue;
            SetRandomItem(holder);
        }
    }

    // 表示する前に初期化
    protected override void OnBeforeFadeIn()
    {
        foreach (var holder in itemHolders)
        {
            // 残っているアイテムを削除
            foreach (Transform child in holder.transform)
            {
                Destroy(child.gameObject);
            }

            SetRandomItem(holder);
        }
    }


    // 非表示にした後にアイテム削除
    protected override void OnAfterFadeOut()
    {
        RemoveItems();
    }

    // ホルダーにランダムなアイテムを設定する関数。
    private void SetRandomItem(Transform holder)
    {
        var item = Instantiate(lootItems[Random.Range(0, lootItems.Count)]);
        item.transform.SetParent(holder.transform, false);
        item.transform.position = holder.transform.position;
    }

    private void RemoveItems()
    {
        foreach (var holder in itemHolders)
        {
            foreach (Transform child in holder.transform)
            {
                if (!child.CompareTag("Item")) continue;
                Destroy(child.gameObject);
            }
        }
    }
}
