
using System.Collections;

namespace Sakemottekoi.MainGame
{
    public class SecretMenuSpirytus : Item
    {
        public override string DisplayName => "裏メニュー「スピリタス」";

        public override string SimpleDescription => "マスターに頼み、選んだ酒の酔い度を極大にする";

        public override string Description => throw new System.NotImplementedException();

        public override IEnumerator Use()
        {
            throw new System.NotImplementedException();
        }
    }
}