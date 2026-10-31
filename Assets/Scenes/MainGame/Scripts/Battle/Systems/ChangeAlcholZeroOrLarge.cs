using UnityEngine;

namespace Sakemottekoi.MainGame
{
    public class ChangeAlcoholZeroOrLarge : GameSystem<BattleContext>
    {
        public override string Id { get; }

        public override OrderRule OrderRule { get; } = new OrderRelative(beforeId: "PlayerDrink");

        public override bool IsOneShot => true;

        private readonly Actor source;

        public ChangeAlcoholZeroOrLarge (Actor source)
        {
            Id = source switch
            {
                Player => "PChangeAlcoholZeroOrLarge",
                Enemy => "EChangeAlcoholZeroOrLarge",
                _ => throw new System.ArgumentException("引数に渡されたActorの型が無効です")
            };

            if (source is Enemy)
            {
                OrderRule = new OrderRelative(afterId: "PlayerDrink", beforeId: "EnemyDrink");
            }

            this.source = source;
        }

        public override void Execute(BattleContext context)
        {
            AlcoholGlass glass = context.GetGlass(source);
            if (glass == null) return;

            // 50%の確立で酔い度をZeroかHighにする
            if (Random.value < 0.5f) glass.SetDrunkenness(DrunkennessLevel.Zero);
            else glass.SetDrunkenness(DrunkennessLevel.High);
        }
    }
}
