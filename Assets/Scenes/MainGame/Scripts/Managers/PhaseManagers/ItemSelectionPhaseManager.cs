namespace Sakemottekoi.MainGame
{
    public class ItemSelectionPhaseManager: PhaseManager<ItemSelectionPhaseManager>
    {
        protected override GamePhase TargetPhase => GamePhase.ItemSelection;

        // フェーズ中に選択可能なアイテム最大数
        public static int SelectableItemCount { private set; get; } = 4;
        // フェーズ中に選択したアイテムの数
        public static int SelectedItemCount { private set; get; } = 0;

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
