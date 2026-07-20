using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glass : MonoBehaviour
{
    [SerializeField]
    public float alcohol = 2;
    [SerializeField]
    public float amount = 5;

    private GlassManager manager = null;

    public Queue<GameObject> items = new Queue<GameObject>();

    private void Start()
    {
        manager = GlassManager.GetInstance();
    }

    public int CalcDegreePoint()
    {
        //foreach (var item in items)
        //{
        //    IItem itemComp = item.GetComponent<IItem>();
        //    itemComp.ChangeGlassParams(ref alcohol, ref amount);
        //}

        return (int)(amount * alcohol);
    }

    // 1秒かけてグラスを相手の方へ移動させる。
    public IEnumerator MoveToOtherSide ()
    {
        // 相手側のグラスの位置を取得
        Vector3 targetPos;
        if(gameObject.CompareTag("PlayerGlass")) targetPos = manager.EnemyGlassLocation;
        else targetPos = manager.PlayerGlassLocation;
        targetPos.x = transform.position.x;

        // 1frameあたりに移動量
        Vector3 moveDelta = (targetPos - transform.position) / 60;
        for (int i = 0; i < 60; i++)
        {
            transform.position += moveDelta;
            yield return null;
        }
    }
}
