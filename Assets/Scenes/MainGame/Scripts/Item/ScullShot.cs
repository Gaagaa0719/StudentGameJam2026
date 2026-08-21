using UnityEngine;

// アイテム追加用テストプログラム
public class ScullShot : MonoBehaviour, IItem
{
    public void ChangeGlassParams(ref float alcohol, ref float amount)
    {
        alcohol *= 1.5f;
    }
}
