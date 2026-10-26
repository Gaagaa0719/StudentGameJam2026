using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Sakemottekoi.MainGame
{
    public class AlcoholGlass : MonoBehaviour, IPointerClickHandler
    {
        public static event Action<AlcoholGlass> OnClicked;

        public DrunkennessLevel Level { private set; get; }
        public float Amount { private set; get; }

        private void Awake()
        {
            GameManager.OnGamePhaseEnded += ReturnToStock;
        }

        private void OnDestroy()
        {
            GameManager.OnGamePhaseEnded -= ReturnToStock;
        }

        public void Consume()
        {
            GameManager.OnGamePhaseEnded -= ReturnToStock;
            Destroy(gameObject);
        }

        public void ReturnToStock()
        {
            AlcholStockManager.Instance.RestockSpecific(Level, 1);
            Destroy(gameObject);
        }

        /// <summary>
        /// このグラスのタイプを設定する。<br/>
        /// 酔い度はタイプに対応した値に設定される。
        /// </summary>
        public void SetAlcholLevel(DrunkennessLevel alcholType)
        {
            Level = alcholType;
            Amount = GameManager.Instance.DrunkennessSettings.GetAmount(Level);
        }

        /// <summary>
        /// このグラスが与える酔い度を変更する
        /// </summary>
        public void SetAmount (float newAmount)
        {
            if (newAmount < 0) return;
            Amount = newAmount;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClicked?.Invoke(this);
        }
    }
}