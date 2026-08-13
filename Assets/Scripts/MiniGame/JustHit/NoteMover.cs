using UnityEngine;

public class NoteMover : MonoBehaviour
{
    [Header("ˆÚ“®‘¬“x")]
    [SerializeField] private float speed = 10200f;

    [Header("¶‚Ì•˜g")]
    [SerializeField] private RectTransform leftWall;

    [Header("‰E‚Ì•˜g")]
    [SerializeField] private RectTransform rightWall;

    private RectTransform noteRect;

    private JustHitManager manager;

    private Vector3 defaultPos;

    // 1 = ‰EA-1 = ¶
    private float direction = 1f;

    private void Awake()
    {
        noteRect = GetComponent<RectTransform>();

        defaultPos = transform.localPosition;
    }

    private void Start()
    {
        manager = JustHitManager.instance;
    }

    public void Init()
    {
        transform.localPosition = defaultPos;

        direction = 1f;
    }

    private void Update()
    {
        // Manager‚ª‘¶İ‚·‚éê‡‚¾‚¯
        // Playingó‘Ô‚ğŠm”F‚·‚é
        if (manager != null)
        {
            if (!manager.GetIsPlaying())
            {
                return;
            }
        }

        MoveNote();
        CheckWall();
    }

    private void MoveNote()
    {
        if (noteRect == null)
        {
            return;
        }

        noteRect.anchoredPosition +=
            Vector2.right *
            direction *
            speed *
            Time.deltaTime;
    }

    private void CheckWall()
    {
        if (leftWall == null)
        {
            Debug.LogWarning(
                "Left Wall‚É¶‘¤‚Ì•˜g‚ğİ’è‚µ‚Ä‚­‚¾‚³‚¢"
            );

            return;
        }

        if (rightWall == null)
        {
            Debug.LogWarning(
                "Right Wall‚É‰E‘¤‚Ì•˜g‚ğİ’è‚µ‚Ä‚­‚¾‚³‚¢"
            );

            return;
        }

        Rect noteWorldRect =
            GetWorldRect(noteRect);

        Rect leftWallRect =
            GetWorldRect(leftWall);

        Rect rightWallRect =
            GetWorldRect(rightWall);

        // =========================
        // ‰E‚Ì•˜g
        // =========================
        if (
            direction > 0f &&
            noteWorldRect.xMax >= rightWallRect.xMin
        )
        {
            direction = -1f;

            // •˜g‚Ì’†‚É‚ß‚è‚ñ‚¾•ª‚¾‚¯–ß‚·
            float difference =
                noteWorldRect.xMax -
                rightWallRect.xMin;

            noteRect.position -=
                new Vector3(
                    difference,
                    0f,
                    0f
                );

            Debug.Log(
                "‰E‚Ì•˜g‚Å’µ‚Ë•Ô‚è‚Ü‚µ‚½"
            );
        }

        // =========================
        // ¶‚Ì•˜g
        // =========================
        if (
            direction < 0f &&
            noteWorldRect.xMin <= leftWallRect.xMax
        )
        {
            direction = 1f;

            // •˜g‚Ì’†‚É‚ß‚è‚ñ‚¾•ª‚¾‚¯–ß‚·
            float difference =
                leftWallRect.xMax -
                noteWorldRect.xMin;

            noteRect.position +=
                new Vector3(
                    difference,
                    0f,
                    0f
                );

            Debug.Log(
                "¶‚Ì•˜g‚Å’µ‚Ë•Ô‚è‚Ü‚µ‚½"
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
            corners[2].x -
            corners[0].x,
            corners[2].y -
            corners[0].y
        );
    }
}