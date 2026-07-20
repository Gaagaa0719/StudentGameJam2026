using System.Collections;
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

    [SerializeField]
    private AudioClip successSound;

    private void Awake()
    {
        instance = this;
        group = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if (!isPlaying) return;

        if (!Mouse.current.leftButton.wasPressedThisFrame) return;

        if (HitArea.Instance != null && HitArea.Instance.IsNoteInside(note))
        {
            audioSource.PlayOneShot(successSound);
            EndGame(true);
        }
        else EndGame(false);
    }

    override public IEnumerator StartGame(float DegreePoint)
    {
        hitArea.Init(DegreePoint);
        yield return base.StartGame(DegreePoint);
    }

    public void EndGame(bool isSuccess)
    {
        this.isSuccess = isSuccess;

        group.alpha = 0f;
        group.blocksRaycasts = false;
        group.interactable = false;

        isPlaying = false;
    }

    public bool GetIsPlaying()
    {
        return isPlaying;
    }
}
