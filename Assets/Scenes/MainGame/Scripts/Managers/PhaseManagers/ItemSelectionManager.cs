using UnityEngine;

namespace Sakemottekoi.MainGame
{
    public class ItemSelectionManager: InputPhaseManager<ItemSelectionManager>
    {
        [Tooltip("フェーズ中に選択可能なアイテム最大数")]
        [SerializeField] private int selectableItemAmount;
        private int selectedItemAmount = 0;

        public int SelectableItemAmount => selectableItemAmount;
        public int SelectedItemAmount => selectedItemAmount;

        public void AddSelectedItemAmount()
        {
            selectedItemAmount++;
        }

        // 選択したアイテム数の初期化
        protected override void OnPhaseStart()
        {
            selectedItemAmount = 0;
        }
    }
}
