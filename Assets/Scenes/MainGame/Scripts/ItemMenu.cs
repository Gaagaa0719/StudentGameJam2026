using Sakemottekoi.MainGame;
using System.Collections.Generic;
using UnityEngine;

public class ItemMenu : MonoBehaviour {
    public static ItemMenu Instance { private set; get; }

    [Header("アイテム表示位置用のアンカー達\n上から順番に詰めて表示される。")]
    public List<GameObject> displayAnchors = new List<GameObject>();

    private readonly List<GameObject> items = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
        ItemDraggable.OnDroppedUI += DroppedOnItem;
    }

    private void DroppedOnItem(DroppedEvent dropEvent)
    {
        // 自分にドロップされてない場合拒否
        if (dropEvent.DroppedOn != gameObject) return;
        // 取得したアイテム数が取得可能アイテム数以上だったら拒否
        if (ItemSelectionPhaseManager.SelectableItemCount <= ItemSelectionPhaseManager.SelectedItemCount) return;
        // すでにアイテムメニューに含まれていたら拒否
        if (items.Contains(dropEvent.Dropped)) return;

        // メニュー追加の試み
        bool result = Add(dropEvent.Dropped);
        if (!result) return;

        // 選択したアイテム数を加算
        ItemSelectionPhaseManager.AddSelectedItemCount();
        // 減った分のアイテムを補充
        ItemOptions.Instance.RestockItems();

        // 所有権が移動したことをマーク
        dropEvent.IsOwnerMoved = true;
    }

    public bool Contains(GameObject item)
    {
        return items.Contains(item);
    }

    // アイテム追加処理
    public bool Add(GameObject item)
    {
        // displayAnchorの数が足りない場合
        if (displayAnchors.Count <= items.Count) return false; 

        items.Add(item);
        GameObject displayAnchor = displayAnchors[items.Count - 1];

        // 親子設定
        item.transform.SetParent(displayAnchor.transform, false);
        item.transform.localPosition = Vector3.zero;
        item.transform.localScale = Vector3.one;

        return true;
    }

    // アイテム削除処理
    public void Remove(GameObject removeItem)
    {
        // リストに存在しない場合
        if (!items.Contains(removeItem)) return;

        items.Remove(removeItem);

        for (int i = 0; i < items.Count; i++)
        {
            GameObject item = items[i];
            item.transform.SetParent(displayAnchors[i].transform, false);
            item.transform.localPosition = Vector3.zero;
        }
    }
}