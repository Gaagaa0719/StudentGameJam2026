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

        public ObjectiveFadeOption(float fadeInTime, float stayTime, float fadeOutTime)
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

        public UniTask ShowObjective(string text)
        {
            return ShowObjective(text, new ObjectiveFadeOption(0.5f, 2f, 0.5f));
        }

        public async UniTask ShowObjective(string text, ObjectiveFadeOption option)
        {
            await FadeOutAsync(option.fadeOutTime);
            textComp.text = text;
            await FadeInAsync(option.fadeInTime);
            await UniTask.Delay(TimeSpan.FromSeconds(option.stayTime), cancellationToken: this.GetCancellationTokenOnDestroy());
            await FadeOutAsync(option.fadeOutTime);
        }
    }
}
