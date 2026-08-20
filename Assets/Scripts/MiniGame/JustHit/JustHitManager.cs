using UnityEngine;
using UnityEngine.InputSystem;

public class JustHitManager : MiniGame
{
    [Header("赤いバー")]
    [SerializeField]
    private NoteMover noteMover;

    [Header("黄色い成功判定")]
    [SerializeField]
    private HitArea hitArea;

    [Header("成功時のSE")]
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip successSE;

    [Header("成功時のエフェクト")]
    [SerializeField]
    private GameObject successEffectPrefab;

    [Header("Sceneを直接再生するときのテスト設定")]
    [SerializeField]
    private bool autoStartForTest = true;

    [SerializeField]
    private float testDrunkenness = 50f;

    // 1ゲーム中に判定済みか
    private bool hasJudged = false;

    private void Awake()
    {
        FindObjects();

        if (audioSource == null)
        {
            audioSource =
                GetComponent<AudioSource>();

            if (audioSource == null)
            {
                audioSource =
                    gameObject.AddComponent<AudioSource>();
            }
        }

        audioSource.playOnAwake = false;
    }

    private void Start()
    {
        FindObjects();

        // ==============================
        // Sceneを直接Playした場合のテスト用
        // ==============================

        if (autoStartForTest && !isPlaying)
        {
            Debug.Log(
                "テストモードでJustHitを開始"
            );

            // StartGameAsyncを通していないので、
            // ここでisPlayingをONにする
            isPlaying = true;

            OnStart(
                testDrunkenness
            );
        }
    }

    private void Update()
    {
        if (!isPlaying)
        {
            return;
        }

        if (hasJudged)
        {
            return;
        }

        if (Mouse.current == null)
        {
            return;
        }

        // 左クリックされた瞬間
        if (
            Mouse.current.leftButton
                .wasPressedThisFrame
        )
        {
            Judge();
        }
    }

    // =====================================
    // MiniGame開始
    // =====================================

    protected override void OnStart(float dp)
    {
        Debug.Log(
            "JustHit開始　酔い度：" + dp
        );

        // 毎回必ず初期化
        hasJudged = false;

        FindObjects();

        // ==============================
        // 赤いバーを初期化
        // ==============================

        if (noteMover != null)
        {
            noteMover.ResetNote();
        }
        else
        {
            Debug.LogWarning(
                "NoteMoverが見つかりません"
            );
        }

        // ==============================
        // 黄色い判定を初期化
        // ==============================

        if (hitArea != null)
        {
            hitArea.Init(dp);
        }
        else
        {
            Debug.LogWarning(
                "HitAreaが見つかりません"
            );
        }
    }

    // =====================================
    // 必要なオブジェクトを探す
    // =====================================

    private void FindObjects()
    {
        if (noteMover == null)
        {
            noteMover =
                FindFirstObjectByType<NoteMover>();
        }

        if (hitArea == null)
        {
            hitArea =
                FindFirstObjectByType<HitArea>();
        }
    }

    // =====================================
    // クリックしたときの判定
    // =====================================

    private void Judge()
    {
        if (hasJudged)
        {
            return;
        }

        hasJudged = true;

        FindObjects();

        if (noteMover == null)
        {
            Debug.Log(
                "失敗：NoteMoverがありません"
            );

            EndMiniGame(false);

            return;
        }

        if (hitArea == null)
        {
            Debug.Log(
                "失敗：HitAreaがありません"
            );

            EndMiniGame(false);

            return;
        }

        RectTransform noteRect =
            noteMover.GetComponent<RectTransform>();

        if (noteRect == null)
        {
            Debug.Log(
                "失敗：NoteMoverにRectTransformがありません"
            );

            EndMiniGame(false);

            return;
        }

        // 赤いバーと黄色い範囲が
        // 重なっているか確認
        bool success =
            hitArea.IsNoteInside(
                noteRect
            );

        // ==============================
        // 成功
        // ==============================

        if (success)
        {
            Debug.Log("成功");

            PlaySuccessSE();

            PlaySuccessEffect(
                noteRect.position
            );

            EndMiniGame(true);
        }

        // ==============================
        // 失敗
        // ==============================

        else
        {
            Debug.Log("失敗");

            EndMiniGame(false);
        }
    }

    // =====================================
    // 成功SE
    // =====================================

    private void PlaySuccessSE()
    {
        if (
            audioSource == null ||
            successSE == null
        )
        {
            return;
        }

        audioSource.PlayOneShot(
            successSE
        );
    }

    // =====================================
    // 成功エフェクト
    // =====================================

    private void PlaySuccessEffect(
        Vector3 position
    )
    {
        if (successEffectPrefab == null)
        {
            return;
        }

        GameObject effect =
            Instantiate(
                successEffectPrefab,
                position,
                Quaternion.identity
            );

        ParticleSystem particle =
            effect.GetComponent<ParticleSystem>();

        if (particle == null)
        {
            particle =
                effect.GetComponentInChildren<
                    ParticleSystem
                >();
        }

        if (particle != null)
        {
            particle.Clear(true);

            particle.Play(true);
        }

        Destroy(
            effect,
            3f
        );
    }

    // =====================================
    // ゲーム終了
    // =====================================

    private void EndMiniGame(
        bool success
    )
    {
        // 二度目のクリックを禁止
        isPlaying = false;

        // 赤いバー停止
        if (noteMover != null)
        {
            noteMover.StopNote();
        }

        Debug.Log(
            success
                ? "ミニゲーム成功"
                : "ミニゲーム失敗"
        );

        // MiniGame側の終了処理を必ず呼ぶ
        FinishGame(success);
    }
}