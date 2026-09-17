using System.Collections.Generic;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    public static MiniGameManager Instance { private set; get; }

    [Header("登場するミニゲームのリスト")]
    [SerializeField]
    private List<MiniGame> miniGames = new();

    private void Awake()
    {
        Instance = this;
    }

    public MiniGame GetRandomOne()
    {
        if (miniGames == null || miniGames.Count == 0) return null;
        int index = Random.Range(0, miniGames.Count);
        return miniGames[index];
    }
}