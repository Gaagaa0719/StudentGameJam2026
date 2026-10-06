using System;
using UnityEngine;

namespace Sakemottekoi.MainGame
{
    public class ChangeAlcoholZeroOrLarge : GameSystem
    {
        public override string Id => "ChangeAlcoholZeroOrLarge";

        public override OrderRule OrderRule { get; }

        public override bool IsOneShot => true;

        private readonly Actor target;

        public ChangeAlcoholZeroOrLarge (Actor target)
        {
            this.target = target;
            OrderRule = target switch
            {
                Player => new OrderRelative(beforeId: "PlayerDrink"),
                Enemy => new OrderRelative(beforeId: "EnemyDrink"),
                _ => throw new ArgumentException("引数に渡されたActorの型が無効です")
            };
        }

        public override void Execute()
        {
            AlcoholGlass glass = target switch
            {
                Player => AlcholSelectionManager.Instance.PlayerGlass,
                Enemy => AlcholSelectionManager.Instance.EnemyGlass,
                _ => throw new ArgumentException("引数に渡されたActorの型が無効です")
            };

            if (glass == null) return;

            // 50%の確立で酔い度をZeroかHighにする
            float random = UnityEngine.Random.Range(0.0f, 1.0f);
            if (random < 0.5f) glass.SetAlcholLevel(DrunkennessLevel.Zero);
            else glass.SetAlcholLevel(DrunkennessLevel.High);
        }
    }
}
