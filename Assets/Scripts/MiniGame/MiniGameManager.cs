using System.Collections.Generic;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    public static MiniGameManager instance;

    [Header("登場するミニゲームのリスト")]
    [SerializeField]
    private List<IMiniGameManager> miniGames = new List<IMiniGameManager>();

    private void Awake()
    {
        instance = this;
    }

    public IMiniGameManager GetRandomOne()
    {
        if (miniGames == null || miniGames.Count == 0) return null;
        int index = Random.Range(0, miniGames.Count);
        return miniGames[index];
    }
}