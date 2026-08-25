using System.Collections;
using UnityEngine;

namespace Sakemottekoi.Minigame.JustTimerStop
{
    public class DegitalTimer : MonoBehaviour
    {
        [Header("一桁秒")]
        [SerializeField] private DigitDisplay oneSecond;
        [Header("小数第一位")]
        [SerializeField] private DigitDisplay firstDecimal;
        [Header("小数第二位")]
        [SerializeField] private DigitDisplay secondDecimal;

        [Header("タイマーの表示時間")]
        [SerializeField] private float visibleDuration = 1.5f;

        private bool isStoped = false;
        private Renderer[] digitRenderers;

        private float currentTime = 0f;

        private void Awake()
        {
            digitRenderers = GetComponentsInChildren<Renderer>();
        }

        public void Init()
        {
            isStoped = false;
            currentTime = 0f;
            oneSecond.UpdateDisplay(0);
            firstDecimal.UpdateDisplay(0);
            secondDecimal.UpdateDisplay(0);
            StartCoroutine(HideDigits());
        }

        private void Update()
        {
            if(isStoped)
            {
                return;
            }

            currentTime += Time.deltaTime;

            int seconds = Mathf.FloorToInt(currentTime % 10);
            int firstDecimalValue = Mathf.FloorToInt((currentTime * 10) % 10);
            int secondDecimalValue = Mathf.FloorToInt((currentTime * 100) % 10);

            oneSecond.UpdateDisplay(seconds);
            firstDecimal.UpdateDisplay(firstDecimalValue);
            secondDecimal.UpdateDisplay(secondDecimalValue);
        }

        private IEnumerator HideDigits()
        {
            while (currentTime < visibleDuration)
            {
                float alpha = Mathf.Lerp(1f, 0f, currentTime / visibleDuration);
                foreach (var renderer in digitRenderers)
                {
                    var color = renderer.material.color;
                    color.a = alpha;
                    renderer.material.color = color;
                }
                yield return null;
            }
        }
    }
}