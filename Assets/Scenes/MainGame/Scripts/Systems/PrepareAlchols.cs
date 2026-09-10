using UnityEngine;

namespace Sakemottekoi.MainGame
{
    public class PrepareAlchols : GameSystem
    {
        public override string Id => "PrepareAlchols";

        public override bool IsOneShot => false;

        public override OrderRule OrderRule => new OrderRelative(priority: 1);

        private readonly Vector3[] AlcholPositions = new Vector3[] {
            new (-1.5f, -0.15f, -5),
            new (0, -0.15f, -5),
            new (1.5f, -0.15f, -5),
        };

        public override void Execute()
        {
            GameObject[] alchols = AlcholStockManager.Instance.GetRandomAlchol(3);
            for (int i = 0; i < alchols.Length; i++)
            {
                alchols[i].transform.position = AlcholPositions[i];
            }
        }
    }
}