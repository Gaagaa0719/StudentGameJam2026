using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class HitArea : MonoBehaviour
{
    public static HitArea Instance;

    [Header("酔い度 x")]
    [SerializeField] private float drunkenness = 0f;

    [Header("判定エリアの高さ")]
    [SerializeField] private float height = 100f;

    [Header("判定エリアの最低幅")]
    [SerializeField] private float minimumWidth = 20f;

    [Header("同じ幅にする当たり判定")]
    [SerializeField] private RectTransform hitBox;

    [Header("ランダム配置する範囲")]
    [SerializeField] private RectTransform randomArea;

    [Header("端から空ける距離")]
    [SerializeField] private float rightMargin = 10f;

    [SerializeField] private float centerMargin = 10f;

    private RectTransform hitAreaRect;

    private void Awake()
    {
        Instance = this;

        hitAreaRect = GetComponent<RectTransform>();

        // 数式から幅を計算して反映
        UpdateRange();

        // 配置範囲が設定されていなければ親を使用
        if (randomArea == null && transform.parent != null)
        {
            randomArea =
                transform.parent.GetComponent<RectTransform>();
        }
    }

    private void Start()
    {
        Canvas.ForceUpdateCanvases();

        RandomizePosition();
    }

    public void Init()
    {
        hitAreaRect = GetComponent<RectTransform>();
        UpdateRange();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    // 数式を使って成功判定と当たり判定の幅を変更
    private void UpdateRange()
    {
        if (hitAreaRect == null)
        {
            return;
        }

        float x = drunkenness;
        float range;

        // xがマイナスにならないようにする
        x = Mathf.Max(x, 0f);

        // 0以上100未満
        // 範囲 = 1000 - 5x
        if (x < 100f)
        {
            range = 1000f - (5f * x);
        }

        // 100以上300未満
        // 範囲 = 700 - 2x
        else if (x < 300f)
        {
            range = 700f - (2f * x);
        }

        // 300以上
        // 範囲 = 20 + 16000 ÷ (2x - 400)
        else
        {
            range =
                20f + (16000f / ((2f * x) - 400f));
        }

        // 幅が20未満にならないようにする
        range = Mathf.Max(range, minimumWidth);

        // 成功判定の幅と高さを変更
        hitAreaRect.sizeDelta =
            new Vector2(range, height);

        // 当たり判定も同じ幅と高さに変更
        if (hitBox != null)
        {
            hitBox.sizeDelta =
                new Vector2(range, height);
        }
    }

    // 右半分のランダムな場所へ移動
    public void RandomizePosition()
    {
        if (randomArea == null)
        {
            Debug.LogWarning(
                "ランダム配置する範囲が設定されていません"
            );

            return;
        }

        Canvas.ForceUpdateCanvases();

        Rect areaWorldRect =
            GetWorldRect(randomArea);

        Rect hitWorldRect =
            GetWorldRect(hitAreaRect);

        float halfHitWidth =
            hitWorldRect.width / 2f;

        // 配置範囲の中央
        float areaCenterX =
            areaWorldRect.center.x;

        // 右半分で置ける最小X座標
        float minimumX =
            areaCenterX
            + halfHitWidth
            + centerMargin;

        // 右端からはみ出さない最大X座標
        float maximumX =
            areaWorldRect.xMax
            - halfHitWidth
            - rightMargin;

        // 配置できる範囲がない場合
        if (maximumX < minimumX)
        {
            Debug.LogWarning(
                "右半分の配置範囲が狭いため、右側中央に配置します"
            );

            float fallbackX =
                areaCenterX
                + areaWorldRect.width / 4f;

            Vector3 fallbackPosition =
                hitAreaRect.position;

            fallbackPosition.x =
                fallbackX;

            hitAreaRect.position =
                fallbackPosition;

            // 当たり判定も同じ位置へ移動
            if (hitBox != null)
            {
                Vector3 hitBoxPosition =
                    hitBox.position;

                hitBoxPosition.x =
                    fallbackX;

                hitBox.position =
                    hitBoxPosition;
            }

            return;
        }

        float randomX =
            Random.Range(
                minimumX,
                maximumX
            );

        Vector3 newPosition =
            hitAreaRect.position;

        newPosition.x =
            randomX;

        hitAreaRect.position =
            newPosition;

        // 当たり判定も同じ位置へ移動
        if (hitBox != null)
        {
            Vector3 hitBoxPosition =
                hitBox.position;

            hitBoxPosition.x =
                randomX;

            hitBox.position =
                hitBoxPosition;
        }

        Debug.Log(
            "HitAreaを右半分に配置しました。X：" + randomX
        );
    }

    // ノーツと判定エリアが重なっているか
    public bool IsNoteInside(RectTransform noteRect)
    {
        if (noteRect == null)
        {
            Debug.LogWarning(
                "判定するノーツが見つかりません"
            );

            return false;
        }

        RectTransform judgmentRect =
            hitAreaRect;

        // 当たり判定が設定されている場合は、
        // 当たり判定側を使って重なりを調べる
        if (hitBox != null)
        {
            judgmentRect =
                hitBox;
        }

        Rect hitAreaWorldRect =
            GetWorldRect(judgmentRect);

        Rect noteWorldRect =
            GetWorldRect(noteRect);

        return hitAreaWorldRect.Overlaps(
            noteWorldRect
        );
    }

    private Rect GetWorldRect(RectTransform target)
    {
        Vector3[] corners =
            new Vector3[4];

        target.GetWorldCorners(corners);

        float rectWidth =
            corners[2].x - corners[0].x;

        float rectHeight =
            corners[2].y - corners[0].y;

        return new Rect(
            corners[0].x,
            corners[0].y,
            rectWidth,
            rectHeight
        );
    }
}