using System.Collections;

namespace Sakemottekoi.MainGame
{
    public class PaperMoney : Item
    {
        public override string DisplayName => "紙幣";

        public override string SimpleDescription => "提示されている中から酒を一つ選び、選んだ酒のアルコール度数がわかる";

        public override string Description => "提示された3つのお酒から一つ選び、選んだお酒に一度浸して取り出す。取り出した紙幣に火を点け燃え具合でそのお酒の酔い度がわかる。\n小は燃えず。中は少し、大はとても燃える。";

        protected override IEnumerator InternalUse()
        {
            throw new System.NotImplementedException();
        }
    }
}