using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using Sakemottekoi.Maingame;

namespace Sakemottekoi.Minigame.JustTimerStop
{
    public class JustTimerStopManager : MiniGame, InputActions.IMiniGameActions
    {
        [SerializeField] private DegitalTimer timer;
        [Header("成功したときの音")]
        [SerializeField] private AudioClip SuccessSound;
        [Header("失敗したときの音")]
        [SerializeField] private AudioClip FailureSound;

        private InputActions actions;
        private AudioSource SESource;

        private void Awake()
        {
            actions = new InputActions();
            actions.MiniGame.AddCallbacks(this);
            SESource = GameManager.GetSESource();
        }

        protected override void OnStart(float dp)
        {
            timer.Init();
            actions.Enable();
        }

        public async void OnMainAction(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            if (!actions.MiniGame.enabled) return;

            actions.Disable();
            float elapssedTime = timer.Stop();
            bool isSuccess = elapssedTime < 3.5f && elapssedTime > 2.5f;
            SESource.PlayOneShot(isSuccess ? SuccessSound : FailureSound);
            await Task.Delay(1000);
            FinishGame(isSuccess);
        }
    }
}