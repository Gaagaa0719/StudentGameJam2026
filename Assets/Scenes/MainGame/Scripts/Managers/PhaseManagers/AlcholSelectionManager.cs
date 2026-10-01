using UnityEngine;

namespace Sakemottekoi.MainGame
{
    public class AlcholSelectionManager : PhaseManager<AlcholSelectionManager>
    {
        protected override GamePhase TargetPhase => GamePhase.AlcholSelection;

        public AlcoholGlass PlayerGlass => playerGlass;
        public AlcoholGlass EnemyGlass => enemyGlass;

        private AlcoholGlass playerGlass, enemyGlass;

        protected override void Awake()
        {
            base.Awake();
            AlcoholGlass.OnClick += (glass) => {
                if (GameManager.Instance.CurrentPhase != GamePhase.AlcholSelection) return;
                playerGlass = glass;
            };
        }

        protected override void StartPhase()
        {
            playerGlass = null;
            enemyGlass = null;
        }

        public async void TryEndPhase()
        {
            enemyGlass = GameManager.Instance.Enemy.SelectGlass();

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
                    playerGlass = null;
                }
                else
                {
                    enemyGlass = null;
                }
            }
            EndPhase();
        }
    }
}