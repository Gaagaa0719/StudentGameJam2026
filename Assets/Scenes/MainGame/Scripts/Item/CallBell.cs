using System.Collections;

namespace Sakemottekoi.MainGame
{
    public class CallBell : Item
    {
        public override string DisplayName => "コールベル";

        public override string SimpleDescription => "渡されたお酒の酔い度上昇率を0か大にする。";

        public override string Description => "自分が次に渡されたお酒の酔い度上昇率を0or大にする。確率は互いに50%";

        protected override IEnumerator InternalUse(Actor source)
        {
            GameSystem system = new ChangeAlcoholZeroOrLarge(source);
            GameManager.Instance.BattlePhase.Add(system);
            yield return null;
        }
    }
}