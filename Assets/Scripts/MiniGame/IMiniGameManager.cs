using System.Collections;
using UnityEngine;

public class IMiniGameManager : MonoBehaviour
{
    public bool isPlaying = false;
    protected bool isSuccess = false;
    public CanvasGroup group;

    public IEnumerator StartGame()
    {
        isPlaying = true;

        group.alpha = 1f;
        group.blocksRaycasts = true;
        group.interactable = true;

        yield return new WaitWhile(() => isPlaying);
    }

    public bool GetIsSuccessed()
    {
        return isSuccess;
    }
}
