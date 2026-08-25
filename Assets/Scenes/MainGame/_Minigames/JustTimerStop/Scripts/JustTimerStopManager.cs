using UnityEngine;

namespace Sakemottekoi.Minigame.JustTimerStop
{
    public class JustTimerStopManager : MiniGame
    {
        [SerializeField] private DegitalTimer timer;

        protected override void OnStart(float dp)
        {
            timer.Init();
        }
    }
}