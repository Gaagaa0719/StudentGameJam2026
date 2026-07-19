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

    [SerializeField]
    private ItemOptions options;

    [SerializeField]
    Enemy enemy;


    [SerializeField]
    Player player;

    private void Start()
    {
        instance = this;
        glassManager = GlassManager.instance;

        StartGame();
    }

    // ゲーム開始処理
    private void StartGame()
    {
        StartRound();
    }

    // ラウンド初期化処理
    private void StartRound()
    {
        glassManager.Init();
        options.StartItemSelection();
    }

    public void TurnEnd ()
    {
        StartCoroutine(EndRound());
    }

    // ラウンド終了処理
    public IEnumerator EndRound()
    {
        yield return StartCoroutine(glassManager.SwapGlass());
        Glass playerGlass = glassManager.PlayerGlass.GetComponent<Glass>();
        Glass enemyGlass = glassManager.EnemyGlass.GetComponent<Glass>();
        player.AddDegreePoint(enemyGlass.CalcDegreePoint());
        yield return new WaitForSeconds(1.0f);
        enemy.AddDegreePoint(playerGlass.CalcDegreePoint());
        yield return new WaitForSeconds(1.0f);

        if (enemy.GetDegreePoint() >= enemy.maxDegreePoint) {
            SceneManager.LoadScene("win");
        }

        StartRound();
    }
}
