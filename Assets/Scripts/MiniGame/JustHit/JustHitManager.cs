using System.Collections;
using UnityEditor.Experimental.GraphView;
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
            Invoke(nameof(EndGame), 0.5f);
        }
        else EndGame(false);
    }

    override public IEnumerator StartGame(float DegreePoint)
    {
        //note.GetComponent<NoteMover>().Init();
        hitArea.Init(DegreePoint);
        yield return StartCoroutine(base.StartGame(DegreePoint));
    }

    public void EndGame(bool isSuccess = true)
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
