
using System.Collections;

namespace Sakemottekoi.MainGame
{
    public class UnbalancedScale : Item
    {
        public override string DisplayName => "揺れている天秤";

        public override string SimpleDescription => "提示された酒の中で、一番強い酒もしくは一番弱い酒が分かる";

        public override string Description => throw new System.NotImplementedException();

        public override IEnumerator Use()
        {
            throw new System.NotImplementedException();
        }
    }
}