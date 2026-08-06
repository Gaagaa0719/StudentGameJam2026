using UnityEngine;

public class SampleMinigame : MiniGame
{
    protected override void OnStart(float dp) {}

    /// <summary>
    /// ボタンが押されたときに呼び出す関数
    /// </summary>
    public void OnClick()
    {
        FinishGame(true);
    }
}
