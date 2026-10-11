using System.Collections;
using UnityEngine;

public class ReactionMinigame : MiniGame
{
    //inspectorから画像を設定するための変数
    [SerializeField] private GameObject imageUI;

    //inspectorから音を設定するための変数

    // OnStartの呼び出し
    protected override void OnStart(float dp)
    {
        StartCoroutine(WaitTime());
    }

    IEnumerator WaitTime()
    {
        // 1～4秒の間でランダムに待つ
        float waitTime = Random.Range(1f,4f);

        yield return new WaitForSeconds(waitTime);


        //画像を表示するプログラム
        
        imageUI.SetActive(true);

        //クリックされたら画像を非表示にするプログラム



        //（仮）ランダム秒数、画像を表示にするプログラム//
        float WaitTime = Random.Range(1f, 5f);

        yield return new WaitForSeconds(WaitTime);



        imageUI.SetActive(false);

        //音を出すプログラム


        // ランダムな数字を出す
        int RandomNumber = Random.Range(1, 5);

        //Logにランダムな数字を出力する
        Debug.Log(RandomNumber);

        // LogにStartと出力をする
        Debug.Log("Start");
    }

}


