using UnityEngine;

public class BottleTapGame : MiniGame
{
    public static BottleTapGame Instance;

    [SerializeField] private WineDetection wineDetection;
    [SerializeField] private WineBottle wineBottle;

    private void Awake()
    {
        Instance = this;
    }

    protected override void OnStart(float dp)
    {
        wineDetection.Init();
        wineBottle.Init();
    }

    public void EndGame(bool isSuccess)
    {
        foreach (var wine in GameObject.FindGameObjectsWithTag("Wine")) Destroy(wine);
        FinishGame(isSuccess);
    }
}
