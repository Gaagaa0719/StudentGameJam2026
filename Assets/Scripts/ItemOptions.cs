using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class ItemOptions : MonoBehaviour
{
    [SerializeField]
    private List<RectTransform> itemHolders = new List<RectTransform>();

    [SerializeField]
    private List<GameObject> lootItems = new List<GameObject>();

    [SerializeField]
    private float animateTime = 1.0f;

    private List<int> selectedChildlenIndex = new List<int>();

    private CanvasGroup group;

    private void Start()
    {
        group = GetComponent<CanvasGroup>();
    }

    public void StartItemSelection()
    {
        Init();
    }

    private void Init()
    {
        foreach (var holder in itemHolders)
        {
            foreach(Transform child in holder.transform)
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

    public void Confirm()
    {
        if (selectedChildlenIndex.Count != 2)
        {
            Debug.Log("2つ選択したください。");
            return;
        }

            foreach (var index in selectedChildlenIndex)
        {
            GameObject child = transform.GetChild(index).gameObject;
            Debug.Log(child.name + "を選択しました。");
        }
        StartCoroutine(nameof(BecomeInvisible), animateTime);
    }

    public void ChooseItem(int index)
    {
        if (selectedChildlenIndex.Contains(index))
        {
            selectedChildlenIndex.Remove(index);
            return;
        }

        if (selectedChildlenIndex.Count < 2)
        {
            selectedChildlenIndex.Add(index);
            return;
        }

        Debug.Log("もう選択できません。");
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
