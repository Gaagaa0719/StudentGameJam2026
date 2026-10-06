using System;

namespace Sakemottekoi.MainGame
{
    public class DrinkWithMaster : GameSystem
    {
        public override string Id { get; }

        public override OrderRule OrderRule => new OrderRelative(priority: 1);

        public override bool IsOneShot => true;

        public DrinkWithMaster(Actor source)
        {
            Id = source switch
            {
                Player => "PlayerDrink",
                Enemy => "EnemyDrink",
                _ => throw new ArgumentException("引数に渡されたActorの型が無効です")
            };
        }

        public override void Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}
