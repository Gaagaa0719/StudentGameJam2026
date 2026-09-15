
using System.Collections;

namespace Sakemottekoi.MainGame
{
    public class WaterBucket : Item
    {
        public override string DisplayName => "水バケツ";

        public override string SimpleDescription => "水をかぶって気を保つ";

        public override string Description => "";

        protected override IEnumerator InternalUse()
        {
            throw new System.NotImplementedException();
        }
    }
}