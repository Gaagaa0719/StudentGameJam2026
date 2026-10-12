using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using Sakemottekoi.MainGame;

namespace Sakemottekoi.Minigame.JustTimerStop
{
    public class JustTimerStopManager : MiniGame
    {
        [SerializeField] private DegitalTimer timer;
        [Header("成功したときの音")]
        [SerializeField] private AudioClip SuccessSound;
        [Header("失敗したときの音")]
        [SerializeField] private AudioClip FailureSound;

        private AudioSource SESource;
        private bool isClicked = false;

        protected override void OnStart(float dp)
        {
            timer.Init();
            isClicked = false;

            if(SESource == null) SESource = GameManager.GetSESource();
        }

        public async override void OnPrimary()
        {
            if (isClicked) return;

            isClicked = true;
            float elapssedTime = timer.Stop();
            bool isSuccess = elapssedTime < 3.5f && elapssedTime > 2.5f;
            SESource.PlayOneShot(isSuccess ? SuccessSound : FailureSound);
            await Task.Delay(1000);
            FinishGame(isSuccess);
        }
    }
}