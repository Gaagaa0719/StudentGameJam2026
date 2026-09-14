using System.Collections;

namespace Sakemottekoi.MainGame
{
    public class PaperMoney : Item
    {
        public override string DisplayName => "紙幣";

        public override string SimpleDescription => "提示されている中から酒を一つ選び、選んだ酒のアルコール度数がわかる";

        public override string Description => throw new System.NotImplementedException();

        public override IEnumerator Use()
        {
            throw new System.NotImplementedException();
        }
    }
}