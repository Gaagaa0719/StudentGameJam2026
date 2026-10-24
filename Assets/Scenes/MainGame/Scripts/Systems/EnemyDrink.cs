
using UnityEngine;

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
            GameManager.Instance.Enemy.AddDrunkenness(glass.Amount);
            GameObject.Destroy(glass.gameObject);
        }
    }
}
