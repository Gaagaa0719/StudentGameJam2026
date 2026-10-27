using UnityEngine;
using Cysharp.Threading.Tasks;

public abstract class MiniGame : MonoBehaviour
{
    [Header("ゲームに対応したUIオブジェクト")]
    [SerializeField] GameObject UI;

    protected bool isPlaying = false;
    private UniTaskCompletionSource<bool> pending;

    /// <summary>
    /// ミニゲームを開始し、終了するまで待機して結果を返す。
    /// </summary>
    /// <param name="dp">酔い度</param>
    public UniTask<bool> StartGameAsync(float dp)
    {
        // ミニゲームが複数起動しないように
        if (pending != null) return pending.Task;

        MiniGameInputController.Instance.SetActiveMinigame(this);
        pending = new UniTaskCompletionSource<bool>();

        // ゲーム開始処理
        gameObject.SetActive(true);
        UI.SetActive(true);
        isPlaying = true;
        OnStart(dp);

        return pending.Task;
    }

    /// <summary>
    /// 派生クラスで実装するゲームの開始処理
    /// </summary>
    /// <param name="dp">酔い度</param>
    protected abstract void OnStart(float dp);

    /// <summary>
    /// ミニゲームを終了し、結果を呼び出し元に返す。
    /// </summary>
    /// <param name="isSuccess">成功したかどうか</param>
    protected void FinishGame (bool isSuccess)
    {
        // まだ完了していない場合にのみ実行。
        if (pending == null) return;

        // ゲーム終了処理
        isPlaying = false;
        UI.SetActive(false);
        gameObject.SetActive(false);

        var completion = pending;
        pending = null;
        completion.TrySetResult(isSuccess);
    }


    /// <summary>
    /// ミニゲームの主となる入力に対する処理
    /// </summary>
    public abstract void OnPrimary();

    /// <summary>
    /// ミニゲームの二つ目の入力に対する処理
    /// </summary>
    public virtual void OnSecondary() { }
}
