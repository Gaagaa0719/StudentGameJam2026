
using System.Collections;
using UnityEngine;

namespace Sakemottekoi.MainGame
{
    public class WaterBucket : Item
    {
        public override string DisplayName => "水バケツ";

        public override string SimpleDescription => "水をかぶって気を保つ";

        public override string Description => $"水をかぶって気を保つ。酔い度を{recoveryAmount}回復する。";

        private float recoveryAmount;

        private void Awake()
        {
            DrunkennessSettings settings = GameManager.Instance.DrunkennessSettings;
            recoveryAmount = settings.GetAmount(DrunkennessLevel.Medium);
        }

        protected override IEnumerator InternalUse(Actor source)
        {
            source.RemoveDrunkenness(recoveryAmount);
            yield break;
        }
    }
}