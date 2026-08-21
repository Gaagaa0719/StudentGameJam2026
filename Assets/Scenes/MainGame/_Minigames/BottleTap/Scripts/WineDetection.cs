using UnityEngine;

public class WineDetection : MonoBehaviour
{
    [Header("クリア判定の閾値")]
    [SerializeField]
    private int clearThreshold = 200;
    private int wineCount = 0;

    public void Init ()
    {
        wineCount = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Wine")) return;
        wineCount++;

        if(wineCount >= clearThreshold)
        {
            BottleTapGame.Instance.EndGame(true);
        }
    }
}