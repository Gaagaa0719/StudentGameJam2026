// ResultSceneData.cs
using UnityEngine;

namespace Game.Result
{
    public enum ResultType
    {
        Win,
        Lose
    }

    public enum ResultRank
    {
        E,
        D,
        C,
        B,
        A,
        S
    }

    [System.Serializable]
    public class ResultSceneData
    {
        public ResultType ResultType;

        // 対戦相手の女の子情報
        public string CharacterName;

        // 勝利時の背景(酔いつぶれ画像)。キャラごとに異なるため前シーンから受け取る。
        // 敗北時はLoseResultPresenter側で固定画像を使用するため、ここは使用しない。
        public Sprite WinBackgroundImage;

        public AudioClip CharacterVoice;   // 勝利時のみ使用

        public int FinalScore;
        public ResultRank Rank;
    }

    public static class ResultSceneLoader
    {
        public static ResultSceneData PendingData { get; private set; }

        public static void SetData(ResultSceneData data)
        {
            PendingData = data;
        }

        public static ResultSceneData ConsumeData()
        {
            var data = PendingData;
            PendingData = null;
            return data;
        }
    }
}