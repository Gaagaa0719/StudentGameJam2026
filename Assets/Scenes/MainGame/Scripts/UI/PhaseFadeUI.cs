using UnityEngine;

namespace Sakemottekoi.MainGame
{
    public class PhaseFadeUI : FadeUIBase
    {
        [SerializeField]
        private GamePhase targetPhase;

        protected virtual GamePhase TargetPhase => targetPhase;

        protected override void Awake()
        {
            base.Awake();
            GameManager.OnPhaseChanged += HandlePhaseChange;
        }

        private void HandlePhaseChange(GamePhase currentPhase)
        {
            if (currentPhase == TargetPhase) FadeIn();
            else FadeOut();
        }
    }
}
