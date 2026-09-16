using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Sakemottekoi.MainGame
{
    public class AlcholGlass : MonoBehaviour, IPointerClickHandler
    {
        public static event Action<AlcholGlass> OnClicked;

        // 酒の度数
        public float content = 2;

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClicked?.Invoke(this);
        }
    }
}