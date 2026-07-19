using UnityEngine;
using UnityEngine.InputSystem;

public class ClickNote : MonoBehaviour
{
    [Header("ê¨å˜éûÇÃå¯â âπ")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip successSE;

    private GameObject note;

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            note = GameObject.Find("NoteMover");

            if (note == null)
            {
                Debug.Log("é∏îs");
                return;
            }

            RectTransform noteRect =
                note.GetComponent<RectTransform>();

            if (HitArea.Instance != null &&
                HitArea.Instance.IsNoteInside(noteRect))
            {
                Debug.Log("ê¨å˜");

                if (audioSource != null &&
                    successSE != null)
                {
                    audioSource.PlayOneShot(successSE);
                }

                Destroy(note);
            }
            else
            {
                Debug.Log("é∏îs");
                Destroy(note);
            }
        }
    }
}