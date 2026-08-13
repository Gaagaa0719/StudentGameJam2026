using UnityEngine;
using UnityEngine.InputSystem;

public class ClickNote : MonoBehaviour
{
    [Header("成功時の効果音")]
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip successSE;

    private GameObject note;

    private JustHitManager manager;

    private bool managerWarningShown = false;

    private void Start()
    {
        FindManager();
    }

    private void Update()
    {
        // =========================
        // ManagerがNullなら探す
        // =========================

        if (manager == null)
        {
            FindManager();

            // それでも見つからない場合
            if (manager == null)
            {
                if (!managerWarningShown)
                {
                    Debug.LogWarning(
                        "JustHitManagerが見つかりません"
                    );

                    managerWarningShown = true;
                }

                return;
            }
        }

        // =========================
        // ゲーム中か確認
        // =========================

        if (!manager.isPlaying)
        {
            return;
        }

        // Mouseが存在しない場合
        if (Mouse.current == null)
        {
            return;
        }

        // =========================
        // 左クリック
        // =========================

        if (
            Mouse.current.leftButton
                .wasPressedThisFrame
        )
        {
            // 1回クリックしたら
            // それ以上クリックを受け付けない
            manager.isPlaying = false;

            // =========================
            // NoteMoverを探す
            // =========================

            note =
                GameObject.Find(
                    "NoteMover"
                );

            if (note == null)
            {
                Debug.Log(
                    "失敗：NoteMoverが見つかりません"
                );

                EndGame();

                return;
            }

            RectTransform noteRect =
                note.GetComponent<RectTransform>();

            if (noteRect == null)
            {
                Debug.Log(
                    "失敗：NoteMoverにRectTransformがありません"
                );

                Destroy(note);

                EndGame();

                return;
            }

            // =========================
            // HitAreaを確認
            // =========================

            if (HitArea.Instance == null)
            {
                Debug.LogWarning(
                    "HitAreaが見つかりません"
                );

                Destroy(note);

                EndGame();

                return;
            }

            // =========================
            // 成功判定
            // =========================

            bool success =
                HitArea.Instance
                    .IsNoteInside(noteRect);

            if (success)
            {
                Debug.Log("成功");

                // ノーツを消す
                Destroy(note);

                // =====================
                // 成功SE
                // =====================

                if (
                    audioSource != null &&
                    successSE != null
                )
                {
                    audioSource.PlayOneShot(
                        successSE
                    );

                    Invoke(
                        nameof(EndGame),
                        successSE.length
                    );
                }
                else
                {
                    EndGame();
                }
            }

            // =========================
            // 失敗
            // =========================

            else
            {
                Debug.Log("失敗");

                Destroy(note);

                EndGame();
            }
        }
    }

    // =============================
    // JustHitManagerを探す
    // =============================

    private void FindManager()
    {
        // まずInstanceを確認
        manager =
            JustHitManager.instance;

        // InstanceがまだNullなら
        // シーンから直接探す
        if (manager == null)
        {
            manager =
                FindFirstObjectByType<
                    JustHitManager
                >();
        }

        if (manager != null)
        {
            managerWarningShown = false;

            Debug.Log(
                "JustHitManagerを取得しました"
            );
        }
    }

    // =============================
    // ゲーム終了
    // =============================

    private void EndGame()
    {
#if UNITY_EDITOR

        UnityEditor.EditorApplication
            .isPlaying = false;

#else

        Application.Quit();

#endif
    }
}