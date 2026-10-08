using System;
using UnityEngine;

namespace Sakemottekoi.MainGame
{
    public class DrinkWithMaster : GameSystem
    {
        public override string Id { get; }

        public override OrderRule OrderRule { get; } = new OrderRelative(priority: 1);

        public override bool IsOneShot => true;

        private readonly Actor source;

        public DrinkWithMaster(Actor source)
        {
            Id = source switch
            {
                Player => "PlayerDrink",
                Enemy => "EnemyDrink",
                _ => throw new ArgumentException("引数に渡されたActorの型が無効です")
            };

            if (source is Enemy)
            {
                OrderRule = new OrderRelative(afterId: "PlayerDrink", priority: 1);
            }

            this.source = source;
        }

        public override void Execute()
        {
            AlcoholGlass glass = source switch
            {
                Player => AlcholSelectionManager.Instance.PlayerGlass,
                Enemy => AlcholSelectionManager.Instance.EnemyGlass,
                _ => throw new ArgumentException("Sourceの型が無効です")
            };
            if (glass == null) return;

            var amount = GameManager.Instance.DrunkennessSettings.GetAmount(glass.Level);
            source.AddDrunkenness(amount / 2);
            GameObject.Destroy(glass.gameObject);
        }
    }
}
