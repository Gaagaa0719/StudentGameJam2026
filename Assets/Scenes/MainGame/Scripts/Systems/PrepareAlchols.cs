using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using UnityEngine;

namespace Sakemottekoi.MainGame
{
    public class PrepareAlchols : GameSystem
    {
        public override string Id => "PrepareAlchols";

        public override bool IsOneShot => false;

        public override OrderRule OrderRule => new OrderRelative();

        private readonly Vector3[] AlcholPositions = new Vector3[] {
            new (-1.5f, -1.15f, -5),
            new (0, -1.15f, -5),
            new (1.5f, -1.15f, -5),
        };

        public override async UniTask ExecuteAsync(CancellationToken token)
        {
            GameObject[] alchols = AlcholStockManager.Instance.GetRandomAlchols(3);
            for (int i = 0; i < alchols.Length; i++)
            {
                alchols[i].transform.position = AlcholPositions[i];
            }

            await UniTask.WhenAll(
                alchols[0].transform.DOLocalMoveY(-0.15f, 0.5f).ToUniTask(cancellationToken: token),
                alchols[1].transform.DOLocalMoveY(-0.15f, 0.5f).ToUniTask(cancellationToken: token),
                alchols[2].transform.DOLocalMoveY(-0.15f, 0.5f).ToUniTask(cancellationToken: token)
            );
        }
    }
}