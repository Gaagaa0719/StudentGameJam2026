
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Sakemottekoi.MainGame
{
    public class EnemyDrink : GameSystem<BattleContext>
    {
        public override string Id => "EnemyDrink";

        public override OrderRule OrderRule => new OrderRelative();

        public override bool IsOneShot => false;

        public override UniTask ExecuteAsync(BattleContext context)
        {
            AlcoholGlass glass = context.enemyDrink;
            if (glass == null) return UniTask.CompletedTask;
            GameManager.Instance.Enemy.AddDrunkenness(glass.Amount);
            return UniTask.CompletedTask;
        }
    }
}
