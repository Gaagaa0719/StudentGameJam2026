using System.Collections;
using UnityEngine;

namespace Sakemottekoi.MainGame
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class FadeUIBase : MonoBehaviour
    {
        [SerializeField] protected float fadeTime = 1.0f;
        protected CanvasGroup group;


        protected virtual void Awake()
        {
            group = GetComponent<CanvasGroup>();
        }

        // フェードイン
        public IEnumerator Show()
        {
            OnBeforeShow();

            float timer = 0f;
            while (timer < fadeTime)
            {
                timer += Time.deltaTime;
                group.alpha = timer / fadeTime;
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
            while (timer < fadeTime)
            {
                timer += Time.deltaTime;
                group.alpha = 1f - (timer / fadeTime);
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