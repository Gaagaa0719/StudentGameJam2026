
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using UnityEngine;

namespace Sakemottekoi.MainGame
{
    public class EnemyDrink : GameSystem<BattleContext>
    {
        public override string Id => "EnemyDrink";

        public override OrderRule OrderRule => new OrderRelative();

        public override bool IsOneShot => false;

        public override async UniTask ExecuteAsync(CancellationToken token, BattleContext context)
        {
            AlcoholGlass glass = context.enemyDrink;
            if (glass == null) return;
            Transform transform = glass.transform;

            // ちょうどいい位置まで移動
            await transform.DOMove(new Vector3(1f, 2.5f, -1f), 1f).ToUniTask(cancellationToken: token);

            // 少し待機
            await UniTask.WaitForSeconds(0.2f, cancellationToken: token);

            // 飲むアニメーション
            await transform.DORotateQuaternion(Quaternion.Euler(0f, 0f, 35f), 0.5f).ToUniTask(cancellationToken: token);

            // 少し待機
            await UniTask.WaitForSeconds(0.2f, cancellationToken: token);

            // 使用済みマーク
            glass.Consume();
            GameManager.Instance.Enemy.AddDrunkenness(glass.Amount);

            // 少し待機
            await UniTask.WaitForSeconds(0.2f, cancellationToken: token);
        }
    }
}
