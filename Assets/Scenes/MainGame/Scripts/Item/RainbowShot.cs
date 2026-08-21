using UnityEngine;

// アイテム追加用テストプログラム
public class RainbowShot : MonoBehaviour, IItem
{
    public void ChangeGlassParams(ref float alcohol, ref float amount)
    {
        amount *= 1.1f; // 酔い度を10%上昇
    }
}
