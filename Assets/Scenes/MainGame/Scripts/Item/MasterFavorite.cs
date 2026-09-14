using System.Collections;

namespace Sakemottekoi.MainGame
{
    public class MasterFavorite : Item
    {
        public override string DisplayName => "マスターの好物";

        public override string SimpleDescription => "渡されたお酒をマスターが代わりに飲んでくれる";

        public override string Description => throw new System.NotImplementedException();

        public override IEnumerator Use()
        {
            throw new System.NotImplementedException();
        }
    }
}