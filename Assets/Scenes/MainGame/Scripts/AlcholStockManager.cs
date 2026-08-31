using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Sakemottekoi.Maingame
{
    public enum AlcholType
    {
        High,
        Midium,
        Low
    }

    [System.Serializable]
    public class AlcholStockData
    {
        public int High = 0;
        public int Midium = 0;
        public int Low = 0;

        public AlcholStockData(int high, int midium, int low)
        {
            High = high;
            Midium = midium;
            Low = low;
        }

        public static AlcholStockData operator +(AlcholStockData data1, AlcholStockData data2)
        {
            return new AlcholStockData(data1.High + data2.High, data1.Midium + data2.Midium, data1.Low + data2.Low);
        }
    }

    [System.Serializable]
    public class ReplenishmentEntry
    {
        public int amount;
        public List<AlcholStockData> alcholStocks;
    }

    public class AlcholStockManager : MonoBehaviour
    {
        public static AlcholStockManager Instance { private set; get; }

        [Header("在庫補充の際に選ばれる候補")]
        [SerializeField] private List<ReplenishmentEntry> candidates = new();

        [Header("度数の高い酒")]
        [SerializeField] private GameObject HighAlcholGlass;

        [Header("度数普通くらいの酒")]
        [SerializeField] private GameObject MidiumAlcholGlass;

        [Header("度数の低い酒")]
        [SerializeField] private GameObject LowAlcholGlass;

        private AlcholStockData alcholStock = new(0, 0, 0);


        private void Awake()
        {
            Instance = this;
        }

        /// <summary>
        /// 在庫を補充する。
        /// </summary>
        public void ReplenishStock(int amount)
        {
            ReplenishmentEntry entry = candidates.Find(v => v.amount == amount)
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

            if(r < alcholStock.High)
            {
                alcholStock.High--;
                return AlcholType.High;
            }
            r -= alcholStock.High;

            if(r < alcholStock.Midium)
            {
                alcholStock.Midium--;
                return AlcholType.Midium;
            }
            r -= alcholStock.Midium;

            alcholStock.Low--;
            return AlcholType.Low;
        }

        public GameObject InstantiateAlchol(AlcholType type)
        {
            return type switch
            {
                AlcholType.High => Instantiate(HighAlcholGlass),
                AlcholType.Midium => Instantiate(MidiumAlcholGlass),
                AlcholType.Low => Instantiate(LowAlcholGlass),
                _ => throw new System.NotImplementedException()
            };
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
            return alcholStock.High + alcholStock.Midium + alcholStock.Low;
        }
    }
}