using System.Linq;
using UnityEngine;

namespace Sakemottekoi.MainGame
{
    public class AlcholSelectionManager : PhaseManager<AlcholSelectionManager>
    {
        protected override GamePhase TargetPhase => GamePhase.AlcholSelection;

        private AlcholGlass playerGlass, enemyGlass;

        protected override void Awake()
        {
            base.Awake();
            AlcholGlass.OnClick += (glass) => { playerGlass = glass; };
        }

        public async void TryEndPhase()
        {
            var alcholGlasses = GameObject.FindGameObjectsWithTag("Glass").Select(v => v.GetComponent<AlcholGlass>()).ToArray();
            enemyGlass = alcholGlasses[Random.Range(0, alcholGlasses.Length)];
            enemyGlass = playerGlass;

            if (playerGlass == null || enemyGlass == null)
            {
                Debug.LogError("グラスが選択されていません！");
                RaiseError("グラスを選んでください！");
                return;
            }

            if(playerGlass == enemyGlass)
            {
                bool result = await MiniGameManager.Instance.GetRandomOne().StartGameAsync(0f);
                if(result)
                {
                    Debug.Log("ミニゲームに勝利しました！");
                    return;
                }
                else
                {
                    Debug.Log("ミニゲームに敗北しました！");
                    return;
                }
            }

            var manager = AlcholStockManager.Instance;
            manager.Consume(playerGlass.Type);
            manager.Consume(enemyGlass.Type);
            EndPhase();
        }
    }
}