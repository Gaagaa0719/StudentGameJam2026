
using System.Collections;
using UnityEngine;


namespace Sakemottekoi.MainGame
{
    public enum GamePhase
    {
        Prepare,
        ItemSelection,
        Battle,
        Minigame,
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { private set; get; }

        public Phase PreparePhase = new Phase();

        public GamePhase CurrentPhase { private set; get; }

        [Header("BGMのオーディオソース")]
        private readonly AudioSource bgmSource;

        [Header("SEのオーディオソース")]
        private readonly AudioSource seSource;

        private bool isPlaying = false;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            AlcholStockManager.Instance.Restock(3);


            //StartCoroutine(StartGameLoop());
        }

        private IEnumerator StartGameLoop()
        {
            isPlaying = true;
            while (isPlaying)
            {
                AlcholStockManager.Instance.Restock(3);
                yield return null;
            }
        }

        static public AudioSource GetBGMSource()
        {
            return Instance.bgmSource;
        }

        static public AudioSource GetSESource()
        {
            return Instance.seSource;
        }
    }
}