using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Sakemottekoi.MainGame
{
    public class AlcoholGlass : MonoBehaviour, IPointerClickHandler
    {
        public static event Action<AlcoholGlass> OnClick;

        public DrunkennessLevel Level { private set; get; }

        private void Awake()
        {
            GameManager.OnGamePhaseEnded += ReturnToStock;
        }

        private void OnDestroy()
        {
            GameManager.OnGamePhaseEnded -= ReturnToStock;
        }

        public void ReturnToStock()
        {
            AlcholStockManager.Instance.RestockSpecific(Level, 1);
            Destroy(gameObject);
        }

        public void SetAlcholLevel(DrunkennessLevel alcholType)
        {
            Level = alcholType;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke(this);
        }
    }
}