using UnityEngine;

public class NoteMover : MonoBehaviour
{
    [Header("移動速度")]
    [SerializeField] private float speed = 10200f;

    private RectTransform noteRect;
    private RectTransform moveAreaRect;

    // 1 = 右、-1 = 左
    private float direction = 1f;

    private void Awake()
    {
        noteRect = GetComponent<RectTransform>();

        // NoteMoverの親を移動範囲として使用
        moveAreaRect = transform.parent.GetComponent<RectTransform>();
    }

    private void Update()
    {
        MoveNote();
        CheckWall();
    }

    private void MoveNote()
    {
        noteRect.anchoredPosition +=
            Vector2.right * direction * speed * Time.deltaTime;
    }

    private void CheckWall()
    {
        if (moveAreaRect == null)
        {
            Debug.LogWarning("NoteMoverの親にRectTransformがありません");
            return;
        }

        Rect noteWorldRect = GetWorldRect(noteRect);
        Rect moveAreaWorldRect = GetWorldRect(moveAreaRect);

        // ノーツの右端が移動範囲の右端に到達
        if (direction > 0f &&
            noteWorldRect.xMax >= moveAreaWorldRect.xMax)
        {
            direction = -1f;

            // 壁の外に出ないように位置を戻す
            float difference =
                noteWorldRect.xMax - moveAreaWorldRect.xMax;

            noteRect.position -=
                new Vector3(difference, 0f, 0f);

            Debug.Log("右の黒い壁で跳ね返りました");
        }

        // ノーツの左端が移動範囲の左端に到達
        if (direction < 0f &&
            noteWorldRect.xMin <= moveAreaWorldRect.xMin)
        {
            direction = 1f;

            // 壁の外に出ないように位置を戻す
            float difference =
                moveAreaWorldRect.xMin - noteWorldRect.xMin;

            noteRect.position +=
                new Vector3(difference, 0f, 0f);

            Debug.Log("左の黒い壁で跳ね返りました");
        }
    }

    private Rect GetWorldRect(RectTransform target)
    {
        Vector3[] corners = new Vector3[4];
        target.GetWorldCorners(corners);

        return new Rect(
            corners[0].x,
            corners[0].y,
            corners[2].x - corners[0].x,
            corners[2].y - corners[0].y
        );
    }
}