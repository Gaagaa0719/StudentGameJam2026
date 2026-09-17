using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Sakemottekoi.MainGame
{
    public class AlcholGlass : MonoBehaviour, IPointerClickHandler
    {
        public static event Action<AlcholGlass> OnClick;

        public AlcholType Type { private set; get; }

        public void SetAlcholType(AlcholType alcholType)
        {
            Type = alcholType;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke(this);
        }
    }
}