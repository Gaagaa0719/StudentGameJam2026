using UnityEngine;

// アイテム追加用テストプログラム
public class RedShot : MonoBehaviour, IItem
{
    public void ChangeGlassParams(ref float alcohol, ref float amount)
    {
        alcohol += 16;
    }
}
