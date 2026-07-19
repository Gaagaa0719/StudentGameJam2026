using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class HitArea : MonoBehaviour
{
    public static HitArea Instance;

    [Header("判定エリアの大きさ")]
    [SerializeField] private float width = 200f;
    [SerializeField] private float height = 100f;

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

        // 黄色い見た目と成功判定を同じ大きさにする
        hitAreaRect.sizeDelta = new Vector2(width, height);

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

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
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

        Rect areaWorldRect = GetWorldRect(randomArea);
        Rect hitWorldRect = GetWorldRect(hitAreaRect);

        float halfHitWidth = hitWorldRect.width / 2f;

        // 配置範囲の中央
        float areaCenterX = areaWorldRect.center.x;

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

            fallbackPosition.x = fallbackX;

            hitAreaRect.position = fallbackPosition;

            return;
        }

        float randomX = Random.Range(
            minimumX,
            maximumX
        );

        Vector3 newPosition =
            hitAreaRect.position;

        newPosition.x = randomX;

        hitAreaRect.position = newPosition;

        Debug.Log(
            "HitAreaを右半分に配置しました。X：" + randomX
        );
    }

    // ノーツとHitAreaが重なっているか
    public bool IsNoteInside(RectTransform noteRect)
    {
        if (noteRect == null)
        {
            Debug.LogWarning(
                "判定するノーツが見つかりません"
            );

            return false;
        }

        Rect hitAreaWorldRect =
            GetWorldRect(hitAreaRect);

        Rect noteWorldRect =
            GetWorldRect(noteRect);

        return hitAreaWorldRect.Overlaps(
            noteWorldRect
        );
    }

    private Rect GetWorldRect(RectTransform target)
    {
        Vector3[] corners = new Vector3[4];

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