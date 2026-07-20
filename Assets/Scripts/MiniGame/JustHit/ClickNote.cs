using UnityEngine;
using UnityEngine.InputSystem;

public class ClickNote : MonoBehaviour
{
    [Header("成功時の効果音")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip successSE;

    private GameObject note;

    // 一度クリックしたか
    private JustHitManager manager;

    private void Awake()
    {
        manager = JustHitManager.instance;
    }

    private void Update()
    {
        Debug.Log(manager);
        if (!manager.isPlaying) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // クリック受付終了
            manager.isPlaying = false;

            note = GameObject.Find("NoteMover");

            if (note == null)
            {
                Debug.Log("失敗");
                EndGame();
                return;
            }

            RectTransform noteRect =
                note.GetComponent<RectTransform>();

            if (HitArea.Instance != null && HitArea.Instance.IsNoteInside(noteRect))
            {
                Debug.Log("成功");

                if (audioSource != null && successSE != null)
                {
                    audioSource.Play();

                    Destroy(note);

                    Invoke(nameof(EndGame), successSE.length);
                }
                else
                {
                    Destroy(note);
                    EndGame();
                }
            }
            else
            {
                Debug.Log("失敗");
                Destroy(note);
                EndGame();
            }
        }
    }

    private void EndGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}