using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class Counter : MonoBehaviour
{
    [Header("カウンターの表示用UI")]
    [SerializeField]
    private TextMeshProUGUI display;

    [Header("カウンターアニメーション用のもう一つのUI")]
    [SerializeField]
    private TextMeshProUGUI display2;

    [SerializeField]
    private float animateTime = 0.5f;

    private float YDelta;
    private Animator animator;
    private bool isOpen = true;

    private int counter = 0;

    private void Start()
    {
        animator = GetComponent<Animator>();
        YDelta = display.transform.localPosition.y - display2.transform.localPosition.y;

        display.text = counter.ToString();
        display2.text = counter.ToString();
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (!Physics.Raycast(ray, out RaycastHit hit)) return;
            if (hit.collider.gameObject != gameObject) return;
            
            counter++;
            StartCoroutine(SetCounter(counter));
        }
    }

    IEnumerator SetCounter(int num)
    {
        display2.text = num.ToString();

        float moveDelta = YDelta / (animateTime * 60);
        for (int i = 0; i < animateTime * 60; i++)
        {
            display.transform.localPosition += new Vector3(0, moveDelta, 0);
            display2.transform.localPosition += new Vector3(0, moveDelta, 0);
            yield return null;
        }

        display.text = num.ToString();
        display.transform.localPosition -= new Vector3(0, YDelta, 0);
        display2.transform.localPosition -= new Vector3(0, YDelta, 0);
    }
}