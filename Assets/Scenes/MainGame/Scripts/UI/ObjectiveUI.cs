using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;

namespace Sakemottekoi.MainGame
{
    public struct ObjectiveFadeOption
    {
        public float fadeInTime;
        public float stayTime;
        public float fadeOutTime;

        public ObjectiveFadeOption(float fadeInTime = 0.5f, float stayTime = 2f, float fadeOutTime = 0.5f)
        {
            this.fadeInTime = fadeInTime;
            this.stayTime = stayTime;
            this.fadeOutTime = fadeOutTime;
        }
    }

    [RequireComponent(typeof(TextMeshProUGUI))]
    class ObjectiveUI : FadeUIBase
    {
        public static ObjectiveUI Instance;

        private TextMeshProUGUI textComp;

        protected override void Awake()
        {
            base.Awake();

            if (Instance == null)
                Instance = this;

            textComp = GetComponent<TextMeshProUGUI>();
        }

        public async UniTask ShowObjective(string text, ObjectiveFadeOption option = new())
        {
            await FadeOutAsync(option.fadeOutTime);
            textComp.text = text;
            await FadeInAsync(option.fadeInTime);
            await UniTask.Delay(TimeSpan.FromSeconds(option.stayTime), cancellationToken: this.GetCancellationTokenOnDestroy());
            await FadeOutAsync(option.fadeOutTime);
        }
    }
}
