using System.Collections;
using UnityEngine;

public enum GamePhase
{
    ItemSelection,
    Minigame,
    Prepare,
    Battle
}

namespace Sakemottekoi.Maingame
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { private set; get; }

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
            StartCoroutine(StartGameLoop());
        }

        private IEnumerator StartGameLoop()
        {
            isPlaying = true;
            while (isPlaying)
            {
                AlcholStockManager.Instance.ReplenishStock();
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