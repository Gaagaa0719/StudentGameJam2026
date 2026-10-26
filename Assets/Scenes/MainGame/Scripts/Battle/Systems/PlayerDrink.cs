using UnityEngine;

namespace Sakemottekoi.MainGame
{
    public class PlayerDrink : GameSystem<BattleContext>
    {
        public override string Id => "PlayerDrink";

        public override OrderRule OrderRule => new OrderRelative();

        public override bool IsOneShot => false;

        public override void Execute(BattleContext context)
        {
            AlcoholGlass glass = context.playerDrink;
            if (glass == null) return;
            GameManager.Instance.Player.AddDrunkenness(glass.Amount);
        }
    }
}
