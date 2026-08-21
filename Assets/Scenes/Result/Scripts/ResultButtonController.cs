// ResultButtonController.cs
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Game.Result
{
    public class ResultButtonController : MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerClickHandler
    {
        public enum ButtonType
        {
            RetryButton,
            ToTitleButton
        }

        [Header("Button Type")]
        [SerializeField]
        private ButtonType buttonType;

        [Header("Hover Frame")]
        [SerializeField]
        private Image hoverFrame;

        [Header("Colors")]
        [SerializeField]
        private Color hoverColor = Color.yellow;

        [SerializeField]
        private Color clickColor = Color.red;

        [Header("SE")]
        [SerializeField]
        private AudioSource audioSource;

        [SerializeField]
        private AudioClip hoverSE;

        [SerializeField]
        private AudioClip clickSE;

        [Header("Click Target")]
        [SerializeField]
        private WinResultPresenter winPresenter;
        [SerializeField]
        private LoseResultPresenter losePresenter;

        [Header("Button Group Lock")]
        [SerializeField]
        private CanvasGroup buttonGroupCanvasGroup; // 自分が押されたら、これごと両方のボタンを操作不可にする

        private bool isClicking = false;

        private void Start()
        {
            SetFrameVisible(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            SetFrameColor(hoverColor);
            SetFrameVisible(true);
            PlaySE(hoverSE);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!isClicking)
            {
                SetFrameVisible(false);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (isClicking) return; // 連打防止

            isClicking = true;

            // 自分が押された瞬間、ボタングループ全体を操作不可にする
            // (もう片方のボタンも含めて押せなくする)
            if (buttonGroupCanvasGroup != null)
            {
                buttonGroupCanvasGroup.interactable = false;
                buttonGroupCanvasGroup.blocksRaycasts = false;
            }

            SetFrameColor(clickColor);
            SetFrameVisible(true);
            PlaySE(clickSE);

            StartCoroutine(ExecuteButtonAfterSE());
        }

        private IEnumerator ExecuteButtonAfterSE()
        {
            if (clickSE != null)
            {
                yield return new WaitForSeconds(clickSE.length);
            }

            ExecuteButton();
        }

        private void SetFrameColor(Color color)
        {
            if (hoverFrame != null)
            {
                Color c = color;
                c.a = 1f;
                hoverFrame.color = c;
            }
        }

        private void SetFrameVisible(bool visible)
        {
            if (hoverFrame != null)
            {
                Color c = hoverFrame.color;
                c.a = visible ? 1f : 0f;
                hoverFrame.color = c;
            }
        }

        private void PlaySE(AudioClip clip)
        {
            if (audioSource != null && clip != null)
            {
                audioSource.PlayOneShot(clip);
            }
        }

        public void ResetFrameState()
        {
            isClicking = false;
            SetFrameVisible(false);
        }

        private void ExecuteButton()
        {
            switch (buttonType)
            {
                case ButtonType.RetryButton:

                    if (winPresenter != null)
                    {
                        winPresenter.OnClickRetry();
                    }
                    else if (losePresenter != null)
                    {
                        losePresenter.OnClickRetry();
                    }

                    break;

                case ButtonType.ToTitleButton:

                    if (winPresenter != null)
                    {
                        winPresenter.OnClickToTitle();
                    }
                    else if (losePresenter != null)
                    {
                        losePresenter.OnClickToTitle();
                    }

                    break;
            }
        }
    }
}