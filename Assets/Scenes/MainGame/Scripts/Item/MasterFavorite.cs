using System.Collections;

namespace Sakemottekoi.MainGame
{
    public class MasterFavorite : Item
    {
        public override string DisplayName => "マスターの好物";

        public override string SimpleDescription => "渡されたお酒をマスターが半分飲んでくれる。酔い度上昇量半減。";

        public override string Description => "渡されたお酒をマスターが半分飲んでくれる。酔い度上昇量半減。";

        protected override IEnumerator InternalUse(Actor source)
        {
            throw new System.NotImplementedException();
        }
    }
} 