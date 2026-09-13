using System.Collections;
using UnityEngine;

namespace Sakemottekoi.MainGame
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class FadeUIBase : MonoBehaviour
    {
        [SerializeField] protected float fadeTime = 1.0f;
        protected CanvasGroup group;

        protected abstract GamePhase TargetPhase { get; }

        private Coroutine fadeCoroutine;


        protected virtual void Awake()
        {
            group = GetComponent<CanvasGroup>();
            group.alpha = 0f;
            group.blocksRaycasts = false;

            GameManager.OnPhaseChanged += HandlePhaseChange;
        }

        private void HandlePhaseChange(GamePhase currentPhase)
        {
            if (currentPhase == TargetPhase)
            {
                // すでに表示されていれば無視
                if (group.alpha == 1f) return;

                if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
                fadeCoroutine = StartCoroutine(Show());
            }
            else
            {
                // すでに消えていれば無視
                if (group.alpha == 0f) return;

                if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
                fadeCoroutine = StartCoroutine(Hide());
            }
        }

        // フェードイン
        public IEnumerator Show()
        {
            OnBeforeShow();

            float timer = 0f;
            float startAlpha = group.alpha;
            while (timer < fadeTime)
            {
                timer += Time.deltaTime;
                group.alpha = Mathf.Lerp(startAlpha, 1f, timer / fadeTime);
                yield return null;
            }
            group.alpha = 1f;
            group.blocksRaycasts = true;

            OnAfterShow();
        }

        // フェードアウト
        public IEnumerator Hide()
        {
            OnBeforeHide();
            group.blocksRaycasts = false;

            float timer = 0f;
            float startAlpha = group.alpha;
            while (timer < fadeTime)
            {
                timer += Time.deltaTime;
                group.alpha = Mathf.Lerp(startAlpha, 0f, timer / fadeTime);
                yield return null;
            }
            group.alpha = 0f;

            OnAfterHide();
        }

        // --- 継承先で好きに書き換えられるフック関数 ---
        protected virtual void OnBeforeShow() { }
        protected virtual void OnAfterShow() { }
        protected virtual void OnBeforeHide() { }
        protected virtual void OnAfterHide() { }
    }
}