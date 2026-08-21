// ResultSceneManager.cs
// シーンのエントリポイント。勝敗で処理を分岐するだけの薄い制御役。
using UnityEngine;

namespace Game.Result
{
    public class ResultSceneManager : MonoBehaviour
    {
        [SerializeField] private WinResultPresenter _winPresenter;
        [SerializeField] private LoseResultPresenter _losePresenter;

        [Header("デバッグ用（前シーンなしで単体テストする時に使用）")]
        [SerializeField] private bool _useDebugData = false;
        [SerializeField] private ResultSceneData _debugData;

        private void Start()
        {
            ResultSceneData data = _useDebugData ? _debugData : ResultSceneLoader.ConsumeData();

            if (data == null)
            {
                Debug.LogError("ResultSceneData が渡されていません。デバッグ実行の場合は _useDebugData をONにしてください。");
                return;
            }

            switch (data.ResultType)
            {
                case ResultType.Win:
                    _losePresenter.gameObject.SetActive(false);
                    _winPresenter.gameObject.SetActive(true);
                    _winPresenter.Play(data);
                    break;

                case ResultType.Lose:
                    _winPresenter.gameObject.SetActive(false);
                    _losePresenter.gameObject.SetActive(true);
                    _losePresenter.Play(data);
                    break;
            }
        }
    }
}