using UnityEngine;

public class AlcholStockManager : MonoBehaviour
{
    public static AlcholStockManager Instance { private set; get; }

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 在庫を補充する。
    /// </summary>
    public void ReplenishStock()
    {

    }

    /// <summary>
    /// 要求された数の酒をゲームオブジェクトの配列として返す。
    /// </summary>
    /// <exception cref="System.Exception">在庫以上の量が要求された場合</exception>
    public GameObject[] GetRandomAlchol(int count)
    {
        if (GetStockCount() < count) throw new System.Exception("在庫以上の数の酒が要求されました。");
        return new GameObject[count];
    }

    /// <summary>
    /// 現在の在庫数を返す。
    /// </summary>
    public int GetStockCount()
    {
        return 0;
    }
}
