using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Sakemottekoi.MainGame;

public class ItemOptions : MonoBehaviour
{
    public static ItemOptions Instance { private set; get; }

    [SerializeField]
    private List<RectTransform> itemHolders = new ();

    [SerializeField]
    private List<GameObject> lootItems = new ();

    [SerializeField]
    private float animateTime = 1.0f;

    private CanvasGroup group;

    private void Awake()
    {
        Instance = this;
        group = GetComponent<CanvasGroup>();

        ItemSelectionPhaseManager.OnStartItemSlection += Init;
        ItemSelectionPhaseManager.OnEndItemSlection += () => StartCoroutine(nameof(CleanUp));
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

    private void Init()
    {
        foreach (var holder in itemHolders)
        {
            foreach (Transform child in holder.transform)
            {
                if (!child.CompareTag("item")) continue;
                Destroy(child.gameObject);
            }

            SetRandomItem(holder);
        }

        StartCoroutine(nameof(BecomeVisible));
    }

    private IEnumerator CleanUp()
    {
        yield return StartCoroutine(nameof(BecomeInvisible), animateTime);
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

    private IEnumerator BecomeVisible()
    {
        float alphaDelta = 1 / (animateTime * 60);
        for (int i = 0; i < (animateTime * 60); i++)
        {
            group.alpha += alphaDelta;
            yield return null;
        }
        group.blocksRaycasts = true;
    }

    private IEnumerator BecomeInvisible(float animateTime)
    {
        group.blocksRaycasts = false;
        float alphaDelta = 1 / (animateTime * 60);
        for (int i = 0; i < (animateTime * 60); i++)
        {
            group.alpha -= alphaDelta;
            yield return null;
        }
    }
}
