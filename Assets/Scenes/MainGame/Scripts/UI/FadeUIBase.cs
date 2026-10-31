using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace Sakemottekoi.MainGame
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class FadeUIBase : MonoBehaviour
    {
        [SerializeField] protected float fadeInTime = 1.0f;
        [SerializeField] protected float fadeOutTime = 1.0f;
        protected CanvasGroup group;

        private CancellationTokenSource cts;

        // 現在フェードして向かっている先の透明度（-1f はフェード停止中）
        private float targetAlpha = -1f;

        protected virtual void Awake()
        {
            group = GetComponent<CanvasGroup>();
            group.alpha = 0f;
            group.blocksRaycasts = false;
        }

        protected virtual void OnDestroy()
        {
            cts?.Cancel();
        }

        public async UniTask FadeInAsync(float fadeTime = -1)
        {
            // 既に完全に表示されているか、またはフェード中なら何もしない
            if (Mathf.Approximately(group.alpha, 1f) || Mathf.Approximately(targetAlpha, 1f))
                return;

            // キャンセルトークン初期化
            cts?.Cancel();
            var fadeCts = new CancellationTokenSource();
            cts = fadeCts;

            OnBeforeFadeIn();

            targetAlpha = 1f;
            if (fadeTime < 0) fadeTime = fadeInTime;

            float timer = 0f;
            float startAlpha = group.alpha;

            try
            {
                while (timer < fadeTime)
                {
                    timer += Time.deltaTime;
                    group.alpha = Mathf.Lerp(startAlpha, 1f, timer / fadeTime);
                    bool canceled = await UniTask
                            .NextFrame(cancellationToken: fadeCts.Token)
                            .SuppressCancellationThrow();
                    if (canceled) return;
                }
                group.alpha = 1f;
                group.blocksRaycasts = true;
            }
            finally
            {
                fadeCts.Dispose();
                if (cts == fadeCts)
                {
                    cts = null;
                    targetAlpha = -1f;
                }
            }

            OnAfterFadeIn();
        }

        public async UniTask FadeOutAsync(float fadeTime = -1)
        {
            // 既に完全に非表示か、またはフェード中なら何もしない
            if (Mathf.Approximately(group.alpha, 0f) || Mathf.Approximately(targetAlpha, 0f))
                return;

            // トークン初期化
            cts?.Cancel();
            var fadeCts = new CancellationTokenSource();
            cts = fadeCts;

            OnBeforeFadeOut();
            targetAlpha = 0f;

            group.blocksRaycasts = false;
            if (fadeTime < 0) fadeTime = fadeOutTime;

            float timer = 0f;
            float startAlpha = group.alpha;

            try
            {
                while (timer < fadeTime)
                {
                    timer += Time.deltaTime;
                    group.alpha = Mathf.Lerp(startAlpha, 0f, timer / fadeTime);
                    bool canceled = await UniTask
                            .NextFrame(cancellationToken: fadeCts.Token)
                            .SuppressCancellationThrow();
                    if (canceled) return;
                }
                group.alpha = 0f;
            }
            finally
            {
                fadeCts.Dispose();
                if(cts == fadeCts)
                {
                    cts = null;
                    targetAlpha = -1f;
                }
            }

            OnAfterFadeOut();
        }

        // --- 継承先で好きに書き換えられるフック関数 ---
        protected virtual void OnBeforeFadeIn() { }
        protected virtual void OnAfterFadeIn() { }
        protected virtual void OnBeforeFadeOut() { }
        protected virtual void OnAfterFadeOut() { }
    }
}