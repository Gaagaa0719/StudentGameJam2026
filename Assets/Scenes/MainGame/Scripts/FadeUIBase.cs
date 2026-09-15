using System.Collections;
using UnityEngine;

namespace Sakemottekoi.MainGame
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class FadeUIBase : MonoBehaviour
    {
        [SerializeField] protected float fadeTime = 1.0f;
        protected CanvasGroup group;

        private Coroutine fadeCoroutine;

        // 現在フェードして向かっている先の透明度（-1f はフェード停止中）
        private float targetAlpha = -1f;

        protected virtual void Awake()
        {
            group = GetComponent<CanvasGroup>();
            group.alpha = 0f;
            group.blocksRaycasts = false;
            targetAlpha = 0f;
        }

        public Coroutine Show()
        {
            // 既に完全に表示されているか、または「表示に向けてフェード中」なら何もしない
            if (Mathf.Approximately(group.alpha, 1f) || Mathf.Approximately(targetAlpha, 1f))
                return fadeCoroutine;

            return PlayFade(1f, InternalShow());
        }

        public Coroutine Hide()
        {
            // 既に完全に非表示か、または「非表示に向けてフェード中」なら何もしない
            if (Mathf.Approximately(group.alpha, 0f) || Mathf.Approximately(targetAlpha, 0f))
                return fadeCoroutine;

            return PlayFade(0f, InternalHide());
        }

        // コルーチンのキャンセルと再生成、目標値の設定
        private Coroutine PlayFade(float target, IEnumerator routine)
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }

            targetAlpha = target;
            fadeCoroutine = StartCoroutine(routine);
            return fadeCoroutine;
        }

        private IEnumerator InternalShow()
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

            targetAlpha = -1f;
            fadeCoroutine = null;

            OnAfterShow();
        }

        private IEnumerator InternalHide()
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

            targetAlpha = -1f;
            fadeCoroutine = null;

            OnAfterHide();
        }

        // --- 継承先で好きに書き換えられるフック関数 ---
        protected virtual void OnBeforeShow() { }
        protected virtual void OnAfterShow() { }
        protected virtual void OnBeforeHide() { }
        protected virtual void OnAfterHide() { }
    }
}