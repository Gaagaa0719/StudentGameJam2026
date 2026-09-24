using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sakemottekoi.MainGame
{
    public class DrunkennessBar : MonoBehaviour
    {
        [SerializeField]
        private Actor target;

        [SerializeField]
        private Image DrunkennessBarUI;

        [SerializeField]
        private TextMeshProUGUI DrunkennessText;

        private void Awake()
        {
            Actor.OnDrunkennessChanged += UpdateBar;
        }

        private void Start()
        {
            Debug.Log(target);
            DrunkennessText.text = $"{target.Drunkenness}/{target.MaxDrunkenness}";
        }

        public void UpdateBar(Actor emitter)
        {
            if (emitter != target) return;
            DrunkennessBarUI.fillAmount = emitter.Drunkenness / emitter.MaxDrunkenness;
            DrunkennessText.text = $"{emitter.Drunkenness}/{emitter.MaxDrunkenness}";
        }
    }
}