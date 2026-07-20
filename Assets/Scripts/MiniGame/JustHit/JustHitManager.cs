using UnityEngine;
using UnityEngine.InputSystem;

public class JustHitManager : IMiniGameManager
{
    public static JustHitManager instance;

    [SerializeField]
    private RectTransform note;

    [SerializeField]
    private HitArea hitArea;

    [SerializeField]
    private AudioSource audioSource;

    private void Awake()
    {
        instance = this;
        Debug.Log(instance);
        group = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if (!isPlaying) return;

        if (!Mouse.current.leftButton.wasPressedThisFrame) return;

        isPlaying = false;

        if (HitArea.Instance != null && HitArea.Instance.IsNoteInside(note))
        {
            audioSource.Play();
            EndGame(true);
        }
        else EndGame(false);
    }

    public void EndGame(bool isSuccess)
    {
        this.isSuccess = isSuccess;

        group.alpha = 0f;
        group.blocksRaycasts = false;
        group.interactable = false;
    }

    public bool GetIsPlaying()
    {
        return isPlaying;
    }
}
