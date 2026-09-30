using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Sakemottekoi.MainGame
{
    public class AlcoholGlass : MonoBehaviour, IPointerClickHandler
    {
        public static event Action<AlcoholGlass> OnClick;

        public DrunkennessLevel Level { private set; get; }

        public void SetAlcholType(DrunkennessLevel alcholType)
        {
            Level = alcholType;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke(this);
        }
    }
}