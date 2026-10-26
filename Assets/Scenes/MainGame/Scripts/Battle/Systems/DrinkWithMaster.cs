using System;

namespace Sakemottekoi.MainGame
{
    public class DrinkWithMaster : GameSystem<BattleContext>
    {
        public override string Id { get; }

        public override OrderRule OrderRule { get; } = new OrderRelative(afterId: "PlayerDrink");

        public override bool IsOneShot => true;

        private readonly Actor source;

        public DrinkWithMaster(Actor source)
        {
            Id = source switch
            {
                Player => "PMasterDrink",
                Enemy => "EMasterDrink",
                _ => throw new ArgumentException("引数に渡されたActorの型が無効です")
            };

            if (source is Enemy)
            {
                OrderRule = new OrderRelative(beforeId: "PlayerDrink", afterId: "EnemyDrink");
            }

            this.source = source;
        }

        public override void Execute(BattleContext context)
        {
            AlcoholGlass glass = context.GetGlass(source);
            if (glass == null) return;
            glass.SetAmount(glass.Amount / 2);
        }
    }
}
