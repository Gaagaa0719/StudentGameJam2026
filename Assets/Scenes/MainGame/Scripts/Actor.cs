using System;
using UnityEngine;

namespace Sakemottekoi.MainGame
{
    public abstract class Actor : MonoBehaviour
    {
        public static event Action<Actor> OnDrunkennessChanged;

        [SerializeField]
        private float maxDrunkenness = 100;

        public float Drunkenness { protected set; get; } = 0;

        public float MaxDrunkenness => maxDrunkenness;


        public void AddDrunkenness(float value)
        {
            SetDrunkenness(Drunkenness + value);
        }

        public void RemoveDrunkenness(float value)
        {
            SetDrunkenness(Drunkenness - value);
        }

        public void SetDrunkenness(float value)
        {
            Drunkenness = value;
            OnDrunkennessChanged?.Invoke(this);
        }
    }
}