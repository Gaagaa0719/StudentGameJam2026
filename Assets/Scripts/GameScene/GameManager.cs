using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private GlassManager glassManager;

    private MiniGameManager miniGameManager;

    [SerializeField]
    private ItemOptions options;

    [SerializeField]
    Enemy enemy;

    [SerializeField]
    Player player;

    private bool isPlaying = false;

    private bool waiting = false;

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

            // ミニゲームフェーズ開始
            currentPhase = GamePhase.Minigame;
            IMiniGameManager miniGame = miniGameManager.GetRandomOne();

            miniGame.gameObject.SetActive(true);
            yield return StartCoroutine(miniGame.StartGame());

            if(miniGame.GetIsSuccessed())
            {
                // ミニゲーム成功時の処理
                player.AddDegreePoint(-5);
            }
            else
            {
                // ミニゲーム失敗時の処理
                player.AddDegreePoint(20);
            }

            miniGame.gameObject.SetActive(false);
        }
    }

    public void SetWainting (bool value)
    {
        waiting = value;
    }

    // ラウンド終了処理
    public IEnumerator Battle()
    {
        currentPhase = GamePhase.Battle;
        
        // aaa
        yield return StartCoroutine(glassManager.SwapGlass());
        
        Glass playerGlass = glassManager.PlayerGlass.GetComponent<Glass>();
        Glass enemyGlass = glassManager.EnemyGlass.GetComponent<Glass>();

        player.AddDegreePoint(enemyGlass.CalcDegreePoint());
        yield return new WaitForSeconds(1.0f);

        enemy.AddDegreePoint(playerGlass.CalcDegreePoint());
        yield return new WaitForSeconds(1.0f);
    }
}
