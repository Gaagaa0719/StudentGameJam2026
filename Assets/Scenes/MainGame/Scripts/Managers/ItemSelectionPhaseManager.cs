using System;
using UnityEngine;

namespace Sakemottekoi.MainGame
{
    public class ItemSelectionPhaseManager: MonoBehaviour
    {
        public static ItemSelectionPhaseManager Instance { private set; get; }

        // フェーズ中に選択可能なアイテム最大数
        public static int SelectableItemCount { private set; get; } = 4;
        // フェーズ中に選択したアイテムの数
        public static int SelectedItemCount { private set; get; } = 0;

        public static event Action OnStartItemSlection;
        public static event Action OnEndItemSlection;



        public bool IsFinished { private set; get; } = false;

        public static void AddSelectedItemCount()
        {
            SelectedItemCount++;
        }

        private void Awake()
        {
            Instance = this;
            GameManager.OnPhaseChanged += StartPhase;
        }

        private void StartPhase(GamePhase gamePhase)
        {
            if (gamePhase != GamePhase.ItemSelection) return;
            SelectedItemCount = 0;
            OnStartItemSlection?.Invoke();
            IsFinished = false;
        }

        public void EndPhase()
        {
            OnEndItemSlection?.Invoke();
            IsFinished = true;
        }
    }
}
