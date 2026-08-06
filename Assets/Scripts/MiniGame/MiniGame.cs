using UnityEngine;
using System.Collections;
using System.Threading.Tasks;

public abstract class MiniGame : MonoBehaviour
{
    private TaskCompletionSource<bool> GameResultTcs;

    /// <summary>
    /// ミニゲームを開始し、終了するまで待機して結果を返す。
    /// </summary>
    /// <param name="dp">酔い度</param>
    public Task<bool> StartGameAsync(float dp)
    {
        GameResultTcs = new TaskCompletionSource<bool>();

        // ゲーム開始処理
        gameObject.SetActive(true);
        OnStart(dp);

        return GameResultTcs.Task;
    }

    /// <summary>
    /// 派生クラスで実装するゲームの開始処理
    /// </summary>
    /// <param name="dp">酔い度</param>
    protected abstract void OnStart(float dp);

    /// <summary>
    /// ミニゲームの終了を終了し、結果を呼び出し元に返す。
    /// </summary>
    /// <param name="isSuccess">成功したかどうか</param>
    protected void FinishGame (bool isSuccess)
    {
        // まだ完了していない場合にのみ実行。
        if (GameResultTcs == null || GameResultTcs.Task.IsCompleted) return;

        // ゲーム終了処理
        gameObject.SetActive(false);
        GameResultTcs.SetResult(isSuccess);
    }
}
