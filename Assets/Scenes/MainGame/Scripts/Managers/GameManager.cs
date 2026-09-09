
using System;
using System.Collections;
using UnityEngine;


namespace Sakemottekoi.MainGame
{
    public enum GamePhase
    {
        Prepare,
        ItemSelection,
        ItemUse,
        AlcholSelection,
        Battle,
        Minigame,
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { private set; get; }

        public static event Action<GamePhase> OnPhaseChanged;

        public readonly Phase PreparePhase = new(new GameSystem[]
        {
            new PrepareAlchols()
        });

        public Phase BattlePhase = new();

        public GamePhase CurrentPhase { private set; get; }

        [Header("BGMのオーディオソース")]
        private readonly AudioSource bgmSource;

        [Header("SEのオーディオソース")]
        private readonly AudioSource seSource;

        private bool isPlaying = false;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            StartCoroutine(StartGameLoop());
        }

        private IEnumerator StartGameLoop()
        {
            isPlaying = true;
            while (isPlaying)
            {
                // 新しいアイテムを取得するまで待つ。
                yield return new WaitUntil();

                AlcholStockManager.Instance.Restock(5);
                while(2 < AlcholStockManager.Instance.GetStockCount())
                {
                    ChangePhase(GamePhase.Prepare);
                    yield return PreparePhase.Run();

                    ChangePhase(GamePhase.ItemUse);
                    // アイテム使用フェーズが終わった合図を待つ
                    yield return new WaitUntil(() => ItemUsePhaseManager.Instance.IsFinished);

                    ChangePhase(GamePhase.AlcholSelection);
                    // アルコールが選ばれるのを待つ。
                    yield return new WaitUntil(() => AlcholSelectionManager.Instance.IsAlcholSelected);

                    ChangePhase(GamePhase.Battle);
                    yield return BattlePhase.Run();
                }
            }
        }

        private void ChangePhase(GamePhase newPhase)
        {
            CurrentPhase = newPhase;
            OnPhaseChanged?.Invoke(newPhase);
        }

        static public AudioSource GetBGMSource()
        {
            return Instance.bgmSource;
        }

        static public AudioSource GetSESource()
        {
            return Instance.seSource;
        }
    }
}