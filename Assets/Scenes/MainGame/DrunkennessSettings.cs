using Sakemottekoi.MainGame;
using UnityEngine;

namespace Sakemottekoi.MainGame
{
    [CreateAssetMenu(
        fileName = "DrunkennessSettings",
        menuName = "Sakemottekoi/Drunkenness Settings"
    )]
    public class DrunkennessSettings : ScriptableObject
    {
        [SerializeField] private float low;
        [SerializeField] private float medium;
        [SerializeField] private float high;
        [SerializeField] private float veryHigh;

        public float GetAmount(DrunkennessLevel level)
        {
            return level switch
            {
                DrunkennessLevel.Low => low,
                DrunkennessLevel.Medium => medium,
                DrunkennessLevel.High => high,
                DrunkennessLevel.VeryHigh => veryHigh,
                _ => throw new System.ArgumentOutOfRangeException(nameof(level))
            };
        }
    }
}