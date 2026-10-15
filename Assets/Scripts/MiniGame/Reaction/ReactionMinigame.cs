using System.Collections;
using UnityEngine;

public class ReactionMinigame : MiniGame
{
    //inspectorから画像を設定するための変数
    [SerializeField] private GameObject imageUI;


    //inspectorから!が出たときのSEを設定するための変数
    [SerializeField] private AudioClip exclamationSE;


    //inspectorから酒をかけたときに発生するSEを設定するための変数
    [SerializeField] private AudioClip alcoholSE;


    private void Start()
    {
        OnStart(0f);
    }

    // OnStartの呼び出し
    protected override void OnStart(float dp)
    {
        StartCoroutine(WaitTime());
    }

    IEnumerator WaitTime()
    {

        AudioSource SEsource = GameManager.GetSESource();


        // 1～2秒の間でランダムに待つ
        float waitTime = Random.Range(1f,2f);

        yield return new WaitForSeconds(waitTime);


        //画像を表示するプログラム
        imageUI.SetActive(true);


        //!の音を出すプログラム
        SEsource.PlayOneShot(exclamationSE);


        //クリックされたら画像を非表示にするプログラム
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        imageUI.SetActive(false);


        //酒をかけた時に流れる音を出すプログラム
        SEsource.PlayOneShot(alcoholSE);


        // ランダムな数字を出す
        int RandomNumber = Random.Range(1, 5);


        //Logにランダムな数字を出力する
        Debug.Log(RandomNumber);


        // LogにStartと出力をする
        Debug.Log("Start");

    }

}


