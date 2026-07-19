using UnityEngine;

// アイテム追加用テストプログラム
public class Banana : MonoBehaviour, IItem
{
    public void ChangeGlassParams(ref float alcohol, ref float amount)
    {
        alcohol += 10;
    }
}
