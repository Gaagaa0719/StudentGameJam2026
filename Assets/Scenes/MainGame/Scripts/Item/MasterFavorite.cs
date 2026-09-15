using System.Collections;

namespace Sakemottekoi.MainGame
{
    public class MasterFavorite : Item
    {
        public override string DisplayName => "マスターの好物";

        public override string SimpleDescription => "渡されたお酒をマスターが代わりに飲んでくれる";

        public override string Description => "";

        protected override IEnumerator InternalUse()
        {
            throw new System.NotImplementedException();
        }
    }
}