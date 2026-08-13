using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class HitArea : MonoBehaviour
{
    public static HitArea Instance;

    [Header("酔い度 x")]
    [SerializeField] private float drunkenness = 0f;

    [Header("判定エリアの高さ")]
    [SerializeField] private float height = 100f;

    [Header("左側の黒枠")]
    [SerializeField] private RectTransform leftWall;

    [Header("右側の黒枠")]
    [SerializeField] private RectTransform rightWall;

    [Header("中央から離す距離")]
    [SerializeField] private float centerMargin = 10f;

    [Header("右の黒枠から離す距離")]
    [SerializeField] private float rightMargin = 10f;

    private RectTransform hitAreaRect;

    private void Awake()
    {
        Instance = this;

        hitAreaRect =
            GetComponent<RectTransform>();

        UpdateRange();
    }

    private void Start()
    {
        Canvas.ForceUpdateCanvases();

        RandomizePosition();
    }

    public void Init(float degreePoint)
    {
        drunkenness = degreePoint;

        UpdateRange();

        Canvas.ForceUpdateCanvases();

        RandomizePosition();
    }

    private void Update()
    {
        UpdateRange();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    // ====================================
    // 酔い度によって黄色い判定エリアを変更
    // ====================================

    private void UpdateRange()
    {
        if (hitAreaRect == null)
        {
            return;
        }

        float x =
            Mathf.Max(
                drunkenness,
                0f
            );

        float range;

        // 0以上100未満
        if (x < 100f)
        {
            range =
                1000f -
                (5f * x);
        }

        // 100以上300未満
        else if (x < 300f)
        {
            range =
                700f -
                (2f * x);
        }

        // 300以上
        else
        {
            range =
                20f +
                (
                    16000f /
                    ((2f * x) - 400f)
                );
        }

        // 最低幅20
        range =
            Mathf.Max(
                range,
                20f
            );

        // 黄色い見た目そのものを判定範囲にする
        hitAreaRect.sizeDelta =
            new Vector2(
                range,
                height
            );
    }

    // ====================================
    // 黄色い判定を右半分へランダム配置
    // ====================================

    public void RandomizePosition()
    {
        if (
            leftWall == null ||
            rightWall == null
        )
        {
            Debug.LogWarning(
                "左右の黒枠が設定されていません"
            );

            return;
        }

        Canvas.ForceUpdateCanvases();

        Rect leftRect =
            GetWorldRect(leftWall);

        Rect rightRect =
            GetWorldRect(rightWall);

        Rect yellowRect =
            GetWorldRect(hitAreaRect);

        // 左右の黒枠の内側
        float gameLeft =
            leftRect.xMax;

        float gameRight =
            rightRect.xMin;

        // ゲームエリアの中央
        float gameCenter =
            (gameLeft + gameRight) / 2f;

        // 黄色いエリアの半分の幅
        float halfWidth =
            yellowRect.width / 2f;

        // 中央より右側
        float minimumX =
            gameCenter +
            centerMargin +
            halfWidth;

        // 右側の黒枠からはみ出さない
        float maximumX =
            gameRight -
            rightMargin -
            halfWidth;

        if (maximumX < minimumX)
        {
            Debug.LogWarning(
                "黄色い判定エリアが大きすぎます"
            );

            float safeX =
                gameRight -
                halfWidth -
                rightMargin;

            SetXPosition(safeX);

            return;
        }

        float randomX =
            Random.Range(
                minimumX,
                maximumX
            );

        SetXPosition(randomX);

        Debug.Log(
            "HitArea配置 X：" +
            randomX
        );
    }

    private void SetXPosition(float x)
    {
        Vector3 position =
            hitAreaRect.position;

        position.x = x;

        hitAreaRect.position =
            position;
    }

    // ====================================
    // 赤いノーツと黄色いエリアの重なり判定
    // ====================================

    public bool IsNoteInside(
        RectTransform noteRect
    )
    {
        if (noteRect == null)
        {
            return false;
        }

        // 黄色い成功判定そのものを使う
        Rect yellowRect =
            GetWorldRect(
                hitAreaRect
            );

        Rect noteWorldRect =
            GetWorldRect(
                noteRect
            );

        bool overlapping =
            yellowRect.Overlaps(
                noteWorldRect
            );

        return overlapping;
    }

    private Rect GetWorldRect(
        RectTransform target
    )
    {
        Vector3[] corners =
            new Vector3[4];

        target.GetWorldCorners(
            corners
        );

        return new Rect(
            corners[0].x,
            corners[0].y,
            corners[2].x -
            corners[0].x,
            corners[2].y -
            corners[0].y
        );
    }
}