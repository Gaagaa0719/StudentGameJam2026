using UnityEngine;

namespace Sakemottekoi.MainGame
{
    public class PrepareAlchols : GameSystem
    {
        public override string Id => "PrepareAlchols";

        public override bool IsOneShot => false;

        public override OrderRule OrderRule => new OrderRelative(priority: 1);

        private Vector3[] AlcholPositions = new Vector3[3];

        public PrepareAlchols(Vector3 firstAlcholLoc, Vector3 secondAlcholLoc, Vector3 thirdAlcholLoc)
        {
            AlcholPositions[0] = firstAlcholLoc;
            AlcholPositions[1] = secondAlcholLoc;
            AlcholPositions[2] = thirdAlcholLoc;
        }

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