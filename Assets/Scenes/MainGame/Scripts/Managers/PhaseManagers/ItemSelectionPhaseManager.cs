namespace Sakemottekoi.MainGame
{
    public class ItemSelectionPhaseManager: PhaseManager
    {
        public static ItemSelectionPhaseManager Instance;
        protected override GamePhase TargetPhase => GamePhase.ItemSelection;

        // フェーズ中に選択可能なアイテム最大数
        public static int SelectableItemCount { private set; get; } = 4;
        // フェーズ中に選択したアイテムの数
        public static int SelectedItemCount { private set; get; } = 0;

        protected override void Awake()
        {
            Instance = this;
            base.Awake();
        }

        public static void AddSelectedItemCount()
        {
            SelectedItemCount++;
        }

        protected override void StartPhase()
        {
            SelectedItemCount = 0;
        }
    }
}
