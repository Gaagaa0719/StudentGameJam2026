
namespace Sakemottekoi.MainGame
{
    public class EnemyDrink : GameSystem
    {
        public override string Id => "EnemyDrink";

        public override OrderRule OrderRule => new OrderRelative(priority: 0);

        public override bool IsOneShot => false;

        public override void Execute()
        {
            AlcoholGlass glass = AlcholSelectionManager.Instance.EnemyGlass;
            if (glass == null) return;
            var amount = GameManager.Instance.DrunkennessSettings.GetAmount(glass.Level);
            GameManager.Instance.Enemy.AddDrunkenness(amount);
            AlcholStockManager.Instance.Consume(glass.Level);
        }
    }
}
