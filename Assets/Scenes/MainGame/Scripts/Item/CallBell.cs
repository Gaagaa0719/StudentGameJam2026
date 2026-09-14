
using System.Collections;

namespace Sakemottekoi.MainGame
{
    public class CallBell : Item
    {
        public override string DisplayName => "コールベル";

        public override string SimpleDescription => "渡されたお酒を提示された中で選ばれていない物と交換する";

        public override string Description => throw new System.NotImplementedException();

        public override IEnumerator Use()
        {
            throw new System.NotImplementedException();
        }
    }
}