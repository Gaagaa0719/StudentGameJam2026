
using System.Collections;
using UnityEngine;


namespace Sakemottekoi.MainGame
{
    public enum GamePhase
    {
        Prepare,
        ItemSelection,
        AlcholSelection,
        Battle,
        Minigame,
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { private set; get; }

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
                AlcholStockManager.Instance.Restock(5);
                while(2 < AlcholStockManager.Instance.GetStockCount())
                {
                    CurrentPhase = GamePhase.Prepare;
                    yield return PreparePhase.Run();

                    CurrentPhase = GamePhase.ItemSelection;
                    // アイテム使用フェーズが終わった合図を待つ

                    CurrentPhase = GamePhase.AlcholSelection;
                    // アルコールが選ばれるのを待つ。
                    // 選択に被りがある場合、ミニゲームを実行する。
                        // ミニゲームの勝者が敗者の飲むアルコールを選ぶのを待つ。

                    CurrentPhase = GamePhase.Battle;
                    yield return BattlePhase.Run();
                }
            }
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