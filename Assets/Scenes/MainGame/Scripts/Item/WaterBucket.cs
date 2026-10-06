
using System.Collections;

namespace Sakemottekoi.MainGame
{
    public class WaterBucket : Item
    {
        public override string DisplayName => "水バケツ";

        public override string SimpleDescription => "水をかぶって気を保つ";

        public override string Description => "水をかぶって気を保ち、自身の酔い度を中下げる";

        protected override IEnumerator InternalUse(Actor source)
        {
            throw new System.NotImplementedException();
        }
    }
}