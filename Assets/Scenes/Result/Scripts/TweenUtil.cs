// TweenUtil.cs
// DOTweenを使わないための簡易コルーチンベースのトゥイーンヘルパー
using System;
using System.Collections;
using UnityEngine;

namespace Game.Result
{
    public static class TweenUtil
    {
        /// <summary>
        /// duration秒かけて0→1のtを進め、毎フレームonUpdateに渡す。
        /// easeがnullの場合は等速(Linear)。
        /// </summary>
        public static IEnumerator Run(float duration, Func<float, float> ease, Action<float> onUpdate)
        {
            if (duration <= 0f)
            {
                onUpdate?.Invoke(1f);
                yield break;
            }

            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                float eased = ease != null ? ease(t) : t;
                onUpdate?.Invoke(eased);
                yield return null;
            }

            onUpdate?.Invoke(ease != null ? ease(1f) : 1f);
        }

        public static float EaseOutBack(float t)
        {
            const float c1 = 1.70158f;
            const float c3 = c1 + 1f;
            float t1 = t - 1f;
            return 1f + c3 * t1 * t1 * t1 + c1 * t1 * t1;
        }

        public static float EaseInOutCubic(float t)
        {
            return t < 0.5f
                ? 4f * t * t * t
                : 1f - Mathf.Pow(-2f * t + 2f, 3f) / 2f;
        }
    }
}