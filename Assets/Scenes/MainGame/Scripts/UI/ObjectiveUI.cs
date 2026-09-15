
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Sakemottekoi.MainGame
{
    [Serializable]
    public class PhaseObjectiveData
    {
        public GamePhase Phase;
        public string ObjectiveText;
    }

    [RequireComponent(typeof(TextMeshProUGUI))]
    class ObjectiveUI : FadeUIBase
    {
        [Header("表示時間")]
        [SerializeField]
        private float stayTime;

        [Header("各フェーズの目的テキスト")]
        [SerializeField]
        private List<PhaseObjectiveData> phaseObjectives = new();

        private TextMeshProUGUI textComp;
        private Coroutine coroutine;

        protected override void Awake()
        {
            base.Awake();
            textComp = GetComponent<TextMeshProUGUI>();
            GameManager.OnPhaseChanged += (phase) => {
                PhaseObjectiveData objectiveData = phaseObjectives.Find(v => v.Phase == phase);
                if (objectiveData == null) return;
                if (coroutine != null) return;
                coroutine = StartCoroutine(ShowObjective(objectiveData));
            };
        }

        private IEnumerator ShowObjective(PhaseObjectiveData data)
        {
            textComp.text = data.ObjectiveText;
            FadeIn();
            yield return new WaitForSeconds(stayTime);
            FadeOut();
            coroutine = null;
        }
    }
}
