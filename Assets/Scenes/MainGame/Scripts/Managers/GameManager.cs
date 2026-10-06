
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

        /// <summary>
        /// フェーズ変更時に呼ばれるイベント
        /// </summary>
        public static event Action<GamePhase> OnPhaseChanged;

        /// <summary>
        /// ゲームの一連のフェーズが終わったタイミングで呼ばれるイベント
        /// </summary>
        public static event Action OnGamePhaseEnded;

        public readonly Phase PreparePhase = new(new GameSystem[]
        {
            new PrepareAlchols()
        });

        public Phase BattlePhase = new(new GameSystem[]
        {
            new PlayerDrink(),
            new EnemyDrink(),
        });

        public GamePhase CurrentPhase { private set; get; }

        [Header("酒の酔い度上昇量")]
        [SerializeField] public DrunkennessSettings DrunkennessSettings;

        [Header("プレイヤー")]
        [SerializeField] public Player Player;

        [Header("敵キャラクター")]
        [SerializeField] public Enemy Enemy;

        [Header("BGMのオーディオソース")]
        [SerializeField] private AudioSource BGMSource;

        [Header("SEのオーディオソース")]
        [SerializeField] private AudioSource SESource;

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
                ChangePhase(GamePhase.ItemSelection);
                // 新しいアイテムを取得するまで待つ。
                yield return new WaitUntil(() => ItemSelectionPhaseManager.Instance.IsFinished);
                AlcholStockManager.Instance.Restock(5);

                while (2 < AlcholStockManager.Instance.GetStockCount())
                {
                    ChangePhase(GamePhase.Prepare);
                    yield return PreparePhase.Run();


                    ChangePhase(GamePhase.ItemUse);
                    // アイテム使用フェーズが終わった合図を待つ
                    yield return new WaitUntil(() => ItemUsePhaseManager.Instance.IsFinished);

                    ChangePhase(GamePhase.AlcholSelection);
                    // アルコールが選ばれるのを待つ。
                    yield return new WaitUntil(() => AlcholSelectionManager.Instance.IsFinished);

                    ChangePhase(GamePhase.Battle);
                    yield return BattlePhase.Run();

                    OnGamePhaseEnded?.Invoke();
                }
                AlcholStockManager.Instance.Clear();
            }
        }

        private void ChangePhase(GamePhase newPhase)
        {
            CurrentPhase = newPhase;
            OnPhaseChanged?.Invoke(newPhase);
        }

        static public AudioSource GetBGMSource()
        {
            return Instance.BGMSource;
        }

        static public AudioSource GetSESource()
        {
            return Instance.SESource;
        }
    }
}