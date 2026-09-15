using System.Collections;
using UnityEngine;

namespace Sakemottekoi.MainGame
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class FadeUIBase : MonoBehaviour
    {
        [SerializeField] protected float fadeInTime = 1.0f;
        [SerializeField] protected float fadeOutTime = 1.0f;
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

        public Coroutine FadeIn()
        {
            // 既に完全に表示されているか、または「表示に向けてフェード中」なら何もしない
            if (Mathf.Approximately(group.alpha, 1f) || Mathf.Approximately(targetAlpha, 1f))
                return fadeCoroutine;

            return PlayFade(1f, InternalFadeIn());
        }

        public Coroutine FadeOut()
        {
            // 既に完全に非表示か、または「非表示に向けてフェード中」なら何もしない
            if (Mathf.Approximately(group.alpha, 0f) || Mathf.Approximately(targetAlpha, 0f))
                return fadeCoroutine;

            return PlayFade(0f, InternalFadeOut());
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

        private IEnumerator InternalFadeIn()
        {
            OnBeforeFadeIn();

            float timer = 0f;
            float startAlpha = group.alpha;
            while (timer < fadeInTime)
            {
                timer += Time.deltaTime;
                group.alpha = Mathf.Lerp(startAlpha, 1f, timer / fadeInTime);
                yield return null;
            }
            group.alpha = 1f;
            group.blocksRaycasts = true;

            targetAlpha = -1f;
            fadeCoroutine = null;

            OnAfterFadeIn();
        }

        private IEnumerator InternalFadeOut()
        {
            OnBeforeFadeOut();
            group.blocksRaycasts = false;

            float timer = 0f;
            float startAlpha = group.alpha;
            while (timer < fadeOutTime)
            {
                timer += Time.deltaTime;
                group.alpha = Mathf.Lerp(startAlpha, 0f, timer / fadeOutTime);
                yield return null;
            }
            group.alpha = 0f;

            targetAlpha = -1f;
            fadeCoroutine = null;

            OnAfterFadeOut();
        }

        // --- 継承先で好きに書き換えられるフック関数 ---
        protected virtual void OnBeforeFadeIn() { }
        protected virtual void OnAfterFadeIn() { }
        protected virtual void OnBeforeFadeOut() { }
        protected virtual void OnAfterFadeOut() { }
    }
}