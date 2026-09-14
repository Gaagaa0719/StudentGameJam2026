using UnityEngine;

namespace Sakemottekoi.MainGame
{
    public class PhaseFadeUI : FadeUIBase
    {
        [SerializeField]
        private GamePhase targetPhase;

        protected virtual GamePhase TargetPhase => targetPhase;

        private Coroutine fadeCoroutine;

        protected override void Awake()
        {
            base.Awake();
            GameManager.OnPhaseChanged += HandlePhaseChange;
        }

        private void HandlePhaseChange(GamePhase currentPhase)
        {
            if (currentPhase == TargetPhase)
            {
                // すでに表示されていれば無視
                if (group.alpha == 1f) return;

                if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
                fadeCoroutine = StartCoroutine(Show());
            }
            else
            {
                // すでに消えていれば無視
                if (group.alpha == 0f) return;

                if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
                fadeCoroutine = StartCoroutine(Hide());
            }
        }
    }
}
