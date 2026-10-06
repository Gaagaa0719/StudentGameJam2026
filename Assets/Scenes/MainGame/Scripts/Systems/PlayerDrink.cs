using UnityEngine;

namespace Sakemottekoi.MainGame
{
    public class PlayerDrink : GameSystem
    {
        public override string Id => "PlayerDrink";

        public override OrderRule OrderRule => new OrderRelative(priority: 0);

        public override bool IsOneShot => false;

        public override void Execute()
        {
            AlcoholGlass glass = AlcholSelectionManager.Instance.PlayerGlass;
            if (glass == null) return;
            var amount = GameManager.Instance.DrunkennessSettings.GetAmount(glass.Level);
            GameManager.Instance.Player.AddDrunkenness(amount);
            GameObject.Destroy(glass.gameObject);
        }
    }
}
