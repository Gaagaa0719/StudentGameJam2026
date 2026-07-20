using UnityEngine;

// アイテム追加用テストプログラム
public class BunbbleShot : MonoBehaviour, IItem
{
    public void ChangeGlassParams(ref float alcohol, ref float amount)
    {
        alcohol += Random.Range(12f, 20f);
    }
}
