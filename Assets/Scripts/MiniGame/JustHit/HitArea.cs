using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class HitArea : MonoBehaviour
{
    public static HitArea Instance;

    [Header("判定エリアの高さ")]
    [SerializeField]
    private float height = 100f;

    [Header("左の黒枠")]
    [SerializeField]
    private RectTransform leftWall;

    [Header("右の黒枠")]
    [SerializeField]
    private RectTransform rightWall;

    [Header("中央から離す距離")]
    [SerializeField]
    private float centerMargin = 10f;

    [Header("右の黒枠から離す距離")]
    [SerializeField]
    private float rightMargin = 10f;

    private RectTransform hitAreaRect;

    private float drunkenness = 0f;

    private void Awake()
    {
        Instance = this;

        hitAreaRect =
            GetComponent<RectTransform>();
    }

    public void Init(float dp)
    {
        if (hitAreaRect == null)
        {
            hitAreaRect =
                GetComponent<RectTransform>();
        }

        drunkenness = dp;

        // 幅を変更
        UpdateRange();

        Canvas.ForceUpdateCanvases();

        // 毎回位置も変更
        RandomizePosition();

        Debug.Log(
            "黄色い判定を初期化しました"
        );
    }

    // =====================================
    // 酔い度 → 判定幅
    // =====================================

    private void UpdateRange()
    {
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
                    (
                        (2f * x)
                        - 400f
                    )
                );
        }

        range =
            Mathf.Max(
                range,
                20f
            );

        hitAreaRect.sizeDelta =
            new Vector2(
                range,
                height
            );

        Debug.Log(
            "判定幅：" + range
        );
    }

    // =====================================
    // 右半分へランダム配置
    // =====================================

    private void RandomizePosition()
    {
        if (
            leftWall == null ||
            rightWall == null
        )
        {
            Debug.LogWarning(
                "HitAreaのLeft Wall / Right Wallが未設定です"
            );

            return;
        }

        Canvas.ForceUpdateCanvases();

        Rect leftRect =
            GetWorldRect(
                leftWall
            );

        Rect rightRect =
            GetWorldRect(
                rightWall
            );

        Rect hitRect =
            GetWorldRect(
                hitAreaRect
            );

        float gameLeft =
            leftRect.xMax;

        float gameRight =
            rightRect.xMin;

        float centerX =
            (
                gameLeft +
                gameRight
            ) / 2f;

        float halfWidth =
            hitRect.width / 2f;

        // 中央より右
        float minimumX =
            centerX +
            centerMargin +
            halfWidth;

        // 黒枠からはみ出さない
        float maximumX =
            gameRight -
            rightMargin -
            halfWidth;

        if (maximumX < minimumX)
        {
            Debug.LogWarning(
                "黄色い判定が大きすぎて右半分に入りません"
            );

            float safeX =
                gameRight -
                rightMargin -
                halfWidth;

            SetXPosition(
                safeX
            );

            return;
        }

        float randomX =
            Random.Range(
                minimumX,
                maximumX
            );

        SetXPosition(
            randomX
        );

        Debug.Log(
            "黄色い判定のX：" +
            randomX
        );
    }

    private void SetXPosition(
        float x
    )
    {
        Vector3 position =
            hitAreaRect.position;

        position.x = x;

        hitAreaRect.position =
            position;
    }

    // =====================================
    // 赤いバーとの重なり
    // =====================================

    public bool IsNoteInside(
        RectTransform noteRect
    )
    {
        if (
            noteRect == null ||
            hitAreaRect == null
        )
        {
            return false;
        }

        Rect hitRect =
            GetWorldRect(
                hitAreaRect
            );

        Rect noteRectWorld =
            GetWorldRect(
                noteRect
            );

        return hitRect.Overlaps(
            noteRectWorld
        );
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