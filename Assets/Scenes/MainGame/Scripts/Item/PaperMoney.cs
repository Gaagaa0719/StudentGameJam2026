using System.Collections;

namespace Sakemottekoi.MainGame
{
    public class PaperMoney : Item
    {
        public override string DisplayName => "紙幣";

        public override string SimpleDescription => "提示されている中から酒を一つ選び、選んだ酒のアルコール度数がわかる";

        public override string Description => "";

        protected override IEnumerator InternalUse()
        {
            throw new System.NotImplementedException();
        }
    }
}