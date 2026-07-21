using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Game.Result;

public enum GamePhase
{
    ItemSelection,
    Minigame,
    Prepare,
    Battle
}

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager GetInstance() { return instance; }

    public GamePhase currentPhase;

    [Header("BGMのオーディオソース")]
    public AudioSource bgmSource;

    [Header("SEのオーディオソース")]
    public AudioSource seSource;

    [Header("アイテム選択用UI")]
    [SerializeField]
    private ItemOptions options;

    [Header("敵キャラクタ")]
    [SerializeField]
    private Enemy enemy;

    [Header("プレイヤーキャラクタ")]
    [SerializeField]
    private Player player;

    [Header("勝利時の背景画像")]
    [SerializeField]
    private Sprite WinBackground;

    [Header("敗北時の背景画像")]
    [SerializeField]
    private Sprite LoseBackground;

    private bool isPlaying = false;
    private bool waiting = false;
    private GlassManager glassManager;
    private MiniGameManager miniGameManager;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        miniGameManager = MiniGameManager.instance;
        glassManager = GlassManager.instance;
        StartCoroutine(StartGameLoop());
    }

    private IEnumerator StartGameLoop()
    {
        isPlaying = true;
        while (isPlaying)
        {
            // グラス初期化
            glassManager.Init();

            // アイテム選択フェーズ開始
            currentPhase = GamePhase.ItemSelection;
            waiting = true;
            options.StartItemSelection();
            yield return new WaitWhile(() => waiting);
            StartCoroutine(options.EndItemSelection());

            // 戦闘準備フェーズ開始
            currentPhase = GamePhase.Prepare;
            waiting = true;
            yield return new WaitWhile(() => waiting);

            // 戦闘フェーズ開始。
            currentPhase = GamePhase.Battle;
            yield return StartCoroutine(nameof(Battle));

            if(enemy.GetDegreePoint() >= enemy.maxDegreePoint)
            {
                var data = new ResultSceneData
                {
                    ResultType = ResultType.Win,
                    CharacterName = "リリス",
                    WinBackgroundImage = WinBackground,
                    FinalScore = 1000,
                    Rank = ResultRank.S
                };
                ResultSceneLoader.SetData(data);
                SceneManager.LoadScene("ResultScene");
                yield break;
            }

            // ミニゲームフェーズ開始
            currentPhase = GamePhase.Minigame;
            IMiniGameManager miniGame = miniGameManager.GetRandomOne();

            miniGame.gameObject.SetActive(true);
            yield return StartCoroutine(miniGame.StartGame(player.GetDegreePoint()));

            bool isSuccessed = miniGame.GetIsSuccessed();
            if(!isSuccessed && player.GetDegreePoint() >= player.maxDegreePoint)
            {
                var data = new ResultSceneData
                {
                    ResultType = ResultType.Lose, // または ResultType.Lose
                    CharacterName = "リリス",
                    WinBackgroundImage = LoseBackground,  // 勝利時のみ使用。キャラごとに異なる酔いつぶれ画像
                    FinalScore = 1000,
                    Rank = ResultRank.S                   // ResultRank.E〜S。ランク算出ロジックは呼び出し側の責務
                };

                ResultSceneLoader.SetData(data);
                SceneManager.LoadScene("ResultScene");
            }

            if (isSuccessed) player.AddDegreePoint(-5);
            else player.AddDegreePoint(20);
            miniGame.gameObject.SetActive(false);
        }
    }

    // 戦闘処理
    private IEnumerator Battle()
    {
        currentPhase = GamePhase.Battle;

        // 互いのグラスを交換
        yield return StartCoroutine(glassManager.SwapGlass());

        Glass playerGlass = glassManager.PlayerGlass.GetComponent<Glass>();
        Glass enemyGlass = glassManager.EnemyGlass.GetComponent<Glass>();

        player.AddDegreePoint(enemyGlass.CalcDegreePoint());
        yield return new WaitForSeconds(1.0f);

        enemy.AddDegreePoint(playerGlass.CalcDegreePoint());
        yield return new WaitForSeconds(1.0f);
    }

    public void SetWainting (bool value)
    {
        waiting = value;
    }

    static public AudioSource GetBGMSource()
    {
        return instance.bgmSource;
    }

    static public AudioSource GetSESource()
    {
        return instance.seSource;
    }
}
