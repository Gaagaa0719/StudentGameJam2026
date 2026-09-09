using System.Collections;
using UnityEngine;

namespace Sakemottekoi.MainGame
{
    public class ItemUsePhaseManager : MonoBehaviour
    {
        public static ItemUsePhaseManager Instance { private set; get; }
        public static bool IsItemUsePhase => GameManager.Instance.CurrentPhase == GamePhase.ItemUse;

        public bool IsFinished { private set; get; } = false;


        private void Awake()
        {
            Instance = this;
            GameManager.OnPhaseChanged += StartItemUsePhase;
        }

        private void StartItemUsePhase(GamePhase gamePhase)
        {
            if(gamePhase != GamePhase.ItemUse) return;
            IsFinished = false;
        }
    }
}