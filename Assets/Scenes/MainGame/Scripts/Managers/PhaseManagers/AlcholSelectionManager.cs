namespace Sakemottekoi.MainGame
{
    public class AlcholSelectionManager : PhaseManager<AlcholSelectionManager>
    {
        protected override GamePhase TargetPhase => GamePhase.AlcholSelection;

        public AlcholGlass PlayerSelectedGlass { private set; get; }
        public AlcholGlass EnemySelectedGlass { private set; get; }

        protected override void Awake()
        {
            base.Awake();
            AlcholGlass.OnClicked += OnAlcholSelected;
        }

        private void OnAlcholSelected(AlcholGlass alcholGlass)
        {
            if (GameManager.Instance.CurrentPhase != TargetPhase) return;
            PlayerSelectedGlass = alcholGlass;
        }

        public async void TryEndPhase()
        {
            if(EnemySelectedGlass == null)
            {
                EnemySelectedGlass = PlayerSelectedGlass;
            }

            if (EnemySelectedGlass == PlayerSelectedGlass)
            {
                MiniGame miniGame = MiniGameManager.Instance.GetRandomOne();
                bool result = await miniGame.StartGameAsync(0);
                if(result)
                {

                }
                else
                {

                }
            }
            else EndPhase();
        }
    }
}