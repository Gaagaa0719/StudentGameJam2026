using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ReactionMinigame : MiniGame
{
    // inspectorから画像を設定するための変数
    [SerializeField] private GameObject imageUI;


    // inspectorから!が出たときのSEを設定するための変数
    [SerializeField] private AudioClip exclamationSE;


    // inspectorから酒をかけたときに発生するSEを設定するための変数
    [SerializeField] private AudioClip alcoholSE;


    // クリックできる制限時間
    [SerializeField] private float clickLimit = 2f;


    // プレイヤーと敵の表示画像
    [SerializeField] private Image playerImage;
    [SerializeField] private Image enemyImage;


    // 勝敗後に表示する画像
    [SerializeField] private Sprite playerWinner;
    [SerializeField] private Sprite enemyWinner;
    [SerializeField] private Sprite playerLoser;
    [SerializeField] private Sprite enemyLoser;


    // すでに勝敗が決まっているか
    private bool isFinished;


    private void Start()
    {
        OnStart(0f);
    }


    // OnStartの呼び出し
    protected override void OnStart(float dp)
    {
        // 勝敗状態をリセット
        isFinished = false;

        StartCoroutine(WaitTime());
    }


    IEnumerator WaitTime()
    {
        AudioSource SEsource = GameManager.GetSESource();


        // 1～2秒の間でランダムに待つ
        float waitTime = Random.Range(1f, 2f);

        yield return new WaitForSeconds(waitTime);


        // 画像を表示するプログラム
        imageUI.SetActive(true);


        // !の音を出すプログラム
        SEsource.PlayOneShot(exclamationSE);


        // 画像が表示された時間を記録
        float startTime = Time.time;


        // 制限時間内の間、クリックされたか確認
        while (Time.time - startTime < clickLimit)
        {
            // 左クリックされた場合
            if (Input.GetMouseButtonDown(0))
            {
                // すでに勝敗が決まっていたら終了
                if (isFinished)
                {
                    yield break;
                }


                // 勝敗が決まった状態にする
                isFinished = true;


                // 画像を非表示
                imageUI.SetActive(false);


                // 酒をかけた時に流れる音を出すプログラム
                SEsource.PlayOneShot(alcoholSE);


                // プレイヤーと敵の画像を変更
                playerImage.sprite = playerWinner;
                enemyImage.sprite = enemyLoser;


                // Logに勝ちと出力
                Debug.Log("勝ち");


                // 勝利を伝える
                FinishGame(true);


                // コルーチンを終了
                yield break;
            }


            // 次のフレームまで待つ
            yield return null;
        }


        // すでに勝敗が決まっていたら終了
        if (isFinished)
        {
            yield break;
        }


        // 勝敗が決まった状態にする
        isFinished = true;


        // 画像を非表示
        imageUI.SetActive(false);


        // プレイヤーと敵の画像を変更
        playerImage.sprite = playerLoser;
        enemyImage.sprite = enemyWinner;


        // Logに負けと出力
        Debug.Log("負け");


        // 敗北を伝える
        FinishGame(false);


        // ランダムな数字を出す
        int RandomNumber = Random.Range(1, 5);


        // Logにランダムな数字を出力する
        Debug.Log(RandomNumber);


        // LogにStartと出力をする
        Debug.Log("Start");
    }
}