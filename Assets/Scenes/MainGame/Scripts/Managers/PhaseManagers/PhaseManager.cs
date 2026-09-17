using System;
using UnityEngine;

namespace Sakemottekoi.MainGame
{
    /// <summary>
    /// 継承する際は絶対に自分自身をTに代入すること！！！！！
    /// </summary>
    public abstract class PhaseManager<T>:  MonoBehaviour where T : PhaseManager<T>
    {
        public static event Action<string> OnErrorOccurred;
        public static T Instance { private set; get; }
        public bool IsFinished { protected set; get; } = false;
        protected abstract GamePhase TargetPhase { get; }

        protected virtual void Awake()
        {
            Instance = (T)this;
            GameManager.OnPhaseChanged += InternalStartPhase;
            Init();
        }

        protected virtual void OnDestroy()
        {
            GameManager.OnPhaseChanged -= InternalStartPhase;
        }

        // 初期化処理
        protected virtual void Init() { }

        // フェーズ開始処理
        private void InternalStartPhase(GamePhase phase) {
            if (phase != TargetPhase) return;
            IsFinished = false;
            StartPhase();
        }

        // 処理追加用
        protected virtual void StartPhase() { }

        // フェーズ終了処理
        public virtual void EndPhase() {
            IsFinished = true;
        }

        protected void RaiseError(string message)
        {
            OnErrorOccurred?.Invoke(message);
        }
    }
}
