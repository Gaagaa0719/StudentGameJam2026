
using Cysharp.Threading.Tasks;
using System.Collections;

namespace Sakemottekoi.MainGame
{
    public class SecretMenuSpirytus : Item
    {
        public override string DisplayName => "裏メニュー「スピリタス」";

        public override string SimpleDescription => "マスターに頼み、選んだ酒の酔い度を極大にする";

        public override string Description => "マスターに頼み、選んだ酒の酔い度を極大にする";

        protected async override UniTask InternalUse(Actor source)
        {
            var glass = await AlcoholSelectionManager.Instance.StartPhaseAsync();
            glass.SetDrunkenness(DrunkennessLevel.VeryHigh);
        }
    }
}