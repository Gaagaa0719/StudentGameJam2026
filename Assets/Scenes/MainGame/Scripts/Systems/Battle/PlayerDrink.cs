using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Sakemottekoi.MainGame
{
    public class PlayerDrink : GameSystem<BattleContext>
    {
        public override string Id => "PlayerDrink";

        public override OrderRule OrderRule => new OrderRelative();

        public override bool IsOneShot => false;

        public override async UniTask ExecuteAsync(BattleContext context)
        {
            AlcoholGlass glass = context.playerDrink;
            if (glass == null) return;
            Transform transform = glass.transform;
            
            // カメラの目の前まで移動
            await transform.DOMove(new Vector3(0f, 0.4f, -9f), 1f);

            // 少し待機
            await UniTask.WaitForSeconds(0.2f);

            // 飲むアニメーション
            await transform.DORotateQuaternion(Quaternion.Euler(-30f, 0f, 0f), 0.5f);

            // 少し待機
            await UniTask.WaitForSeconds(0.2f);

            // 使用済みマーク
            glass.Consume();
            GameManager.Instance.Player.AddDrunkenness(glass.Amount);

            // 少し待機
            await UniTask.WaitForSeconds(0.2f);
        }
    }
}
