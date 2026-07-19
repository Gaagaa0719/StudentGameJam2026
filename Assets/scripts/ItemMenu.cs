using System.Collections.Generic;
using UnityEngine;

public class ItemMenu : MonoBehaviour {
    [Header("アイテム表示位置用のアンカー達\n上から順番に詰めて表示される。")]
    public List<GameObject> displayAnchors = new List<GameObject>();

    private readonly List<GameObject> items = new List<GameObject>();

    [Header("アイテムのドラッグ移動させたくないときに疎外するためのプロテクター")]
    public GameObject protector;

    // アイテム追加処理
    public void Add(GameObject item)
    {
        // displayAnchorの数が足りない場合
        if (displayAnchors.Count <= items.Count) return; 

        items.Add(item);
        GameObject displayAnchor = displayAnchors[items.Count - 1];

        // 親子設定
        item.transform.SetParent(displayAnchor.transform, false);
        item.transform.localPosition = Vector3.zero;
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

    // プロテクターオブジェクト有効化
    public void EnableProtector()
    {
        protector.SetActive(true);
    }


    // プロテクターオブジェクト無効化
    public void DisableProtector()
    {
        protector.SetActive(false);
    }
}