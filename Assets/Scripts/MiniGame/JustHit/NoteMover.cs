using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class NoteMover : MonoBehaviour
{
    [Header("移動速度")]
    [SerializeField]
    private float speed = 10200f;

    [Header("左の黒枠")]
    [SerializeField]
    private RectTransform leftWall;

    [Header("右の黒枠")]
    [SerializeField]
    private RectTransform rightWall;

    private RectTransform noteRect;

    private Vector2 defaultPosition;

    // 1 = 右、-1 = 左
    private float direction = 1f;

    // 動いてよいか
    private bool canMove = false;

    private void Awake()
    {
        noteRect =
            GetComponent<RectTransform>();

        // 最初の位置を保存
        defaultPosition =
            noteRect.anchoredPosition;
    }

    private void Start()
    {
        // Sceneを直接再生した場合でも
        // とりあえず動くようにする
        canMove = true;

        direction = 1f;
    }

    private void Update()
    {
        if (!canMove)
        {
            return;
        }

        MoveNote();

        CheckWall();
    }

    // ============================
    // ミニゲーム開始時
    // ============================

    public void ResetNote()
    {
        if (noteRect == null)
        {
            noteRect =
                GetComponent<RectTransform>();
        }

        // 初期位置
        noteRect.anchoredPosition =
            defaultPosition;

        // 右向きから再スタート
        direction = 1f;

        // 移動開始
        canMove = true;

        Debug.Log(
            "赤いバーをリセットして移動開始"
        );
    }

    // ============================
    // ミニゲーム終了時
    // ============================

    public void StopNote()
    {
        canMove = false;

        Debug.Log(
            "赤いバーを停止"
        );
    }

    // ============================
    // 移動
    // ============================

    private void MoveNote()
    {
        noteRect.anchoredPosition +=
            Vector2.right *
            direction *
            speed *
            Time.deltaTime;
    }

    // ============================
    // 黒枠で反射
    // ============================

    private void CheckWall()
    {
        if (leftWall == null)
        {
            Debug.LogWarning(
                "Left Wallが設定されていません"
            );

            return;
        }

        if (rightWall == null)
        {
            Debug.LogWarning(
                "Right Wallが設定されていません"
            );

            return;
        }

        Rect noteRectWorld =
            GetWorldRect(noteRect);

        Rect leftWallRect =
            GetWorldRect(leftWall);

        Rect rightWallRect =
            GetWorldRect(rightWall);

        // 右の黒枠に到達
        if (
            direction > 0f &&
            noteRectWorld.xMax >=
            rightWallRect.xMin
        )
        {
            direction = -1f;

            float difference =
                noteRectWorld.xMax -
                rightWallRect.xMin;

            noteRect.position -=
                new Vector3(
                    difference,
                    0f,
                    0f
                );

            Debug.Log(
                "右の黒枠で跳ね返りました"
            );
        }

        // 左の黒枠に到達
        if (
            direction < 0f &&
            noteRectWorld.xMin <=
            leftWallRect.xMax
        )
        {
            direction = 1f;

            float difference =
                leftWallRect.xMax -
                noteRectWorld.xMin;

            noteRect.position +=
                new Vector3(
                    difference,
                    0f,
                    0f
                );

            Debug.Log(
                "左の黒枠で跳ね返りました"
            );
        }
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
            corners[2].x - corners[0].x,
            corners[2].y - corners[0].y
        );
    }
}