using UnityEngine;

namespace Sakemottekoi.MainGame
{
    public abstract class PhaseManager:  MonoBehaviour
    {
        public bool IsFinished { protected set; get; } = false;
        protected abstract GamePhase TargetPhase { get; }

        protected virtual void Awake()
        {
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
    }
}
