using System;
namespace Sakemottekoi.MainGame
{
    public class BattleContext
    {
        public readonly AlcoholGlass playerDrink;
        public readonly AlcoholGlass enemyDrink;

        /// <param name="_playerDrink">プレイヤーが飲むグラス</param>
        /// <param name="_enemyDrink">敵が飲むグラス</param>
        public BattleContext(AlcoholGlass _playerDrink, AlcoholGlass _enemyDrink)
        {
            playerDrink = _playerDrink;
            enemyDrink = _enemyDrink;
        }

        /// <summary>
        /// 渡されたアクタークラスに応じた酒を返してくれる
        /// </summary>
        public AlcoholGlass GetGlass(Actor actor)
        {
            return actor switch
            {
                Player => playerDrink,
                Enemy => enemyDrink,
                _ => throw new InvalidOperationException("存在しないアクタークラスです。")
            };
        }
    }
}
