using UnityEngine;
using UnityEngine.UI;

public class BulbGameManager : MonoBehaviour
{
    [Header("電球のプレハブ")]
    [SerializeField] private GameObject tentouPrefab;  // 黄色のオブジェクト
    [SerializeField] private GameObject syoutouPrefab; // 灰色のオブジェクト

    [Header("生成する土台 (asi)")]
    [SerializeField] private RectTransform[] asiTransforms = new RectTransform[3];

    [Header("効果音の設定")]
    [SerializeField] private AudioSource audioSource;  // 効果音を鳴らすコンポーネント
    [SerializeField] private AudioClip clearSound;     // クリア時の効果音 (SE)

    [Header("数式用のパラメーター")]
    [SerializeField] private float drunkenness = 0f;    // 酔い度 (x) インスペクターから自由に変更可能

    private GameObject[] currentBulbs = new GameObject[3]; // 現在画面にある電球
    private bool[] bulbStates = new bool[3];                // 各電球のON/OFF状態（true=黄色, false=灰色）

    private int clearCount = 0;        // 現在のクリア回数
    private int maxClear = 3;          // 【数式で自動計算される目標クリア回数 (y)】
    private bool isGameFinished = false; // ゲームが完全に終了したかのフラグ

    void Start()
    {
        // 安全チェック
        if (tentouPrefab == null || syoutouPrefab == null || asiTransforms[0] == null)
        {
            Debug.LogError("インスペクターの設定が足りません！プレハブとasiを割り当ててください。");
            return;
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        ResetGame();
    }

    // ゲーム全体をはじめからリセットする関数
    public void ResetGame()
    {
        clearCount = 0;
        isGameFinished = false;

        // ★修正ポイント：ゲーム開始時・リセット時に酔い度(x)から必要回数(y)を計算
        maxClear = CalculateRequiredCount(drunkenness);
        Debug.Log($"<color=cyan>【ゲーム開始】酔い度(x): {drunkenness} ➔ 必要クリア回数(y): {maxClear}回</color>");

        SpawnStage();
    }

    // ★ご提示の数式を用いて必要クリア回数を導き出す関数
    private int CalculateRequiredCount(float x)
    {
        float y = 0f;

        if (x >= 0f && x < 300f)
        {
            // 0 <= x < 300 のとき： y = 3 + (x * 4 / 300)
            y = 3f + (x * 4f / 300f);
        }
        else if (x >= 300f)
        {
            // x >= 300 のとき： y = 7 + (x - 300) / x * 8
            y = 7f + ((x - 300f) / x) * 8f;
        }

        // 小数点以下を切り捨てて整数(int)にして返す
        return Mathf.FloorToInt(y);
    }

    // ステージを生成・リセットする関数
    public void SpawnStage()
    {
        for (int i = 0; i < currentBulbs.Length; i++)
        {
            if (currentBulbs[i] != null) Destroy(currentBulbs[i]);
        }

        int greyIndex = Random.Range(0, 3);

        for (int i = 0; i < 3; i++)
        {
            bool isYellow = (i != greyIndex);
            SpawnBulbAt(i, isYellow);
        }
    }

    // 指定した「asi」の上に電球を生成・配置する関数
    private void SpawnBulbAt(int index, bool isYellow)
    {
        if (currentBulbs[index] != null) Destroy(currentBulbs[index]);

        bulbStates[index] = isYellow;

        GameObject prefabToSpawn = isYellow ? tentouPrefab : syoutouPrefab;

        Transform parentAsi = asiTransforms[index];
        GameObject newBulb = Instantiate(prefabToSpawn, parentAsi);
        newBulb.name = $"Bulb_{index}_{(isYellow ? "tentou" : "syoutou")}";

        RectTransform rect = newBulb.GetComponent<RectTransform>();
        if (rect != null)
        {
            rect.anchoredPosition = Vector2.zero;
            rect.localRotation = Quaternion.identity;
            rect.localScale = Vector3.one;

            float asiHeight = asiTransforms[index].rect.height;
            float bulbHeight = rect.rect.height;
            rect.anchoredPosition = new Vector2(0, (asiHeight / 2) + (bulbHeight / 2));
        }

        Button btn = newBulb.GetComponent<Button>();
        if (btn == null)
        {
            btn = newBulb.AddComponent<Button>();
            btn.transition = Selectable.Transition.None;
        }

        btn.interactable = true;
        int currentIndex = index;
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => OnBulbClicked(currentIndex));

        currentBulbs[index] = newBulb;
    }

    // 電球がクリックされた時の処理
    private void OnBulbClicked(int index)
    {
        if (isGameFinished) return;

        // すでに黄色(tentou)だったら失敗
        if (bulbStates[index] == true)
        {
            GameOver();
            return;
        }

        // 灰色(false)だったので、黄色(true)にして再生成
        SpawnBulbAt(index, true);

        // クリア判定へ
        CheckClearCondition();
    }

    // 失敗（お手付き）時の処理
    private void GameOver()
    {
        Debug.Log("<color=red>【失敗！】最初からやり直しです。</color>");
        Invoke("ResetGame", 1.0f); // 失敗時は確認のため1秒残す
    }

    // クリア判定
    private void CheckClearCondition()
    {
        foreach (bool isYellow in bulbStates)
        {
            if (!isYellow) return;
        }

        // --- ステージクリア時の処理 ---
        clearCount++;
        Debug.Log($"ステージクリア！ ({clearCount} / {maxClear})");

        // クリア音を即座に鳴らす
        if (audioSource != null && clearSound != null)
        {
            audioSource.PlayOneShot(clearSound);
        }

        // 自動計算された目標回数（maxClear）に達したかチェック
        if (clearCount >= maxClear)
        {
            isGameFinished = true;
            Debug.Log($"<color=green>【大成功！】{maxClear}回連続クリア達成！</color>");
        }
        else
        {
            // すぐに次のステージへ移行
            SpawnStage();
        }
    }
}