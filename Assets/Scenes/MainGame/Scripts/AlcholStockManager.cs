using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sakemottekoi.Maingame
{
    public enum AlcholType
    {
        SuperHigh = 0,
        High = 1,
        Midium = 2,
        Low = 3
    }

    [System.Serializable]
    public class AlcholStockData
    {
        [EnumIndex(typeof(AlcholType))]
        public int[] stocks = new int[4];

        public int this[AlcholType type]
        {
            get => stocks[(int)type];
            set => stocks[(int)type] = value;
        }

        public static AlcholStockData operator +(AlcholStockData data1, AlcholStockData data2)
        {
            AlcholStockData result = new();
            for (int i = 0; i < result.stocks.Length; i ++)
            {
                result.stocks[i] = data1.stocks[i] + data1.stocks[i];
            }
            return result;
        }
    }

    [System.Serializable]
    public class RestockEntry
    {
        public int amount;
        public List<AlcholStockData> alcholStocks;
    }

    public class AlcholStockManager : MonoBehaviour
    {
        public static AlcholStockManager Instance { private set; get; }

        [Header("在庫補充の際に選ばれる候補")]
        [SerializeField] private List<RestockEntry> candidates = new();

        [Header("お酒用のプレファブ")]
        [SerializeField] private AlcholGlass alcholPrefab;

        [Header("酔い度の上昇量")]
        [SerializeField] private AlcholStockData alcholContent = new();

        private AlcholStockData alcholStock = new();


        private void Awake()
        {
            Instance = this;
        }

        /// <summary>
        /// 在庫を補充する。
        /// </summary>
        public void Restock(int amount)
        {
            RestockEntry entry = candidates.Find(v => v.amount == amount)
                ?? throw new System.Exception("求められた数量に一致する候補がありません。");

            AlcholStockData stockData = entry.alcholStocks[Random.Range(0, entry.alcholStocks.Count)];
            alcholStock += stockData;
        }

        /// <summary>
        /// 在庫からランダムに一つの酒を消費し、その種類を返す。
        /// </summary>
        public AlcholType ConsumeRandomStock()
        {
            int r = Random.Range(0, GetStockCount());

            if (r < alcholStock[AlcholType.SuperHigh])
            {
                alcholStock[AlcholType.SuperHigh]--;
                return AlcholType.SuperHigh;
            }
            r -= alcholStock[AlcholType.SuperHigh];

            if (r < alcholStock[AlcholType.High])
            {
                alcholStock[AlcholType.High]--;
                return AlcholType.High;
            }
            r -= alcholStock[AlcholType.High];

            if (r < alcholStock[AlcholType.Midium])
            {
                alcholStock[AlcholType.Midium]--;
                return AlcholType.Midium;
            }
            r -= alcholStock[AlcholType.Midium];

            alcholStock[AlcholType.Low]--;
            return AlcholType.Low;
        }

        public GameObject InstantiateAlchol(AlcholType type)
        {
            AlcholGlass alcholObj = Instantiate(alcholPrefab);
            alcholObj.content = alcholContent[type];
            return alcholObj.gameObject;
        }

        /// <summary>
        /// 要求された数の酒をゲームオブジェクトの配列として返す。
        /// </summary>
        /// <exception cref="System.Exception">在庫以上の量が要求された場合</exception>
        public GameObject[] GetRandomAlchol(int count)
        {
            int stockCount = GetStockCount();
            if (stockCount < count) throw new System.Exception("在庫以上の数の酒が要求されました。");

            GameObject[] alchols = new GameObject[count];
            for (int i = 0; i < count; i++)
            {
                AlcholType type = ConsumeRandomStock();
                alchols[i] = InstantiateAlchol(type);
            }

            return alchols;
        }

        /// <summary>
        /// 現在の在庫数を返す。
        /// </summary>
        public int GetStockCount()
        {
            return alcholStock.stocks.Sum();
        }
    }
}