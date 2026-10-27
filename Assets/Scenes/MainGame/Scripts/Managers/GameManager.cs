using Cysharp.Threading.Tasks;
using System;
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

        public Phase<BattleContext> BattlePhase = new(new GameSystem<BattleContext>[]
        {
            new EnemyDrink(),
            new PlayerDrink(),
        });

        public GamePhase CurrentPhase { private set; get; }

        [Header("酒の酔い度上昇量")]
        public DrunkennessSettings DrunkennessSettings;

        [Header("プレイヤー")]
        public Player Player;

        [Header("敵キャラクター")]
        public Enemy Enemy;

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
            StartGameLoop().Forget();
        }

        private async UniTask StartGameLoop()
        {
            isPlaying = true;
            while (isPlaying)
            {
                ChangePhase(GamePhase.ItemSelection);
                // 新しいアイテムを取得するまで待つ。
                await ItemSelectionManager.Instance.StartPhaseAsync();
                AlcholStockManager.Instance.Restock(5);

                while (2 < AlcholStockManager.Instance.GetStockCount())
                {
                    ChangePhase(GamePhase.Prepare);
                    await PreparePhase.Run();

                    ChangePhase(GamePhase.ItemUse);
                    // アイテム使用が終わった合図を待つ
                    await ItemUsePhaseManager.Instance.StartPhaseAsync();

                    ChangePhase(GamePhase.AlcholSelection);
                    // アルコールが選ばれるのを待つ。
                    var enemyDrink = await AlcoholSelectionManager.Instance.StartPhaseAsync(); // 敵に飲ませるグラス
                    var playerDrink = Enemy.SelectGlass(); // プレイヤーが飲むグラス

                    // 同じアルコールが選ばれた場合の処理
                    if (playerDrink == enemyDrink)
                    {
                        var minigame = MiniGameManager.Instance.GetRandomOne();
                        var result = await minigame.StartGameAsync(Player.Drunkenness);
                        if(result) playerDrink = null;
                        else enemyDrink = null;
                    }

                    ChangePhase(GamePhase.Battle);
                    await BattlePhase.Run(new BattleContext(playerDrink, enemyDrink));

                    // 使用済みマーク
                    if(enemyDrink) enemyDrink.Consume();
                    if(playerDrink) playerDrink.Consume();

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