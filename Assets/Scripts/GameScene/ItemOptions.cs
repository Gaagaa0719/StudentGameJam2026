using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class ItemOptions : MonoBehaviour
{
    public static int selectableItemCount = 0;
    public static int addedItemCount = 0;

    [SerializeField]
    private List<RectTransform> itemHolders = new List<RectTransform>();

    [SerializeField]
    private List<GameObject> lootItems = new List<GameObject>();

    [SerializeField]
    private float animateTime = 1.0f;

    [SerializeField]
    private int _selectableItemCount = 2;

    private List<int> selectedChildlenIndex = new List<int>();

    private CanvasGroup group;

    private void Start()
    {
        selectableItemCount = _selectableItemCount;
        group = GetComponent<CanvasGroup>();
    }

    public void StartItemSelection()
    {
        foreach (var holder in itemHolders)
        {
            foreach (Transform child in holder.transform)
            {
                if (child.tag != "item") continue;
                Destroy(child.gameObject);
            }

            var item = Instantiate(lootItems[Random.Range(0, lootItems.Count)]);
            item.transform.SetParent(holder.transform, false);
            item.transform.position = holder.transform.position;
        }

        StartCoroutine(nameof(BecomeVisible), animateTime);
    }

    public IEnumerator EndItemSelection()
    {
        yield return StartCoroutine(nameof(BecomeInvisible), animateTime);
        RemoveItems();
        ItemOptions.addedItemCount = 0;
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

    private IEnumerator BecomeVisible(float animateTime)
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
