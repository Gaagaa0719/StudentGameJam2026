using UnityEngine;
using System.Collections;

// 電球ミニゲーム全体を管理する
public class LightBulbGame : MiniGame
{
    // 3つの電球
    public LightBulb bulb1;
    public LightBulb bulb2;
    public LightBulb bulb3;

    // 現在のクリア回数
    int clearCount = 0;

    // 何回クリアしたらゲーム終了か
    // Inspectorから変更できる
    public int requiredClearCount = 3;

    // クリア後に待つ時間
    // Inspectorから変更できる
    public float waitTime = 1.0f;

    // 待機中かどうか
    bool isWaiting = false;


    // ミニゲーム開始
    protected override void OnStart(float value)
    {
        // 現在のクリア回数を0にする
        clearCount = 0;

        // 待機状態を解除
        isWaiting = false;

        // 最初の電球状態を作る
        ResetBulbs();
    }


    // 3つ全ての電球がONか確認する
    public void CheckClear()
    {
        // 3つ全部trueの場合
        if (bulb1.GetIsOn() &&
            bulb2.GetIsOn() &&
            bulb3.GetIsOn())
        {
            // クリア回数を1増やす
            clearCount++;

            Debug.Log("クリア回数：" + clearCount);


            // Inspectorで設定した回数までクリアした場合
            if (clearCount >= requiredClearCount)
            {
                // ミニゲーム終了
                FinishGame(true);
            }
            else
            {
                // まだ必要回数に達していない場合
                // 少し待ってから次へ進む
                StartCoroutine(WaitNextStage());
            }
        }
    }


    // クリア後に少し待つ
    IEnumerator WaitNextStage()
    {
        // 待機中にする
        isWaiting = true;

        // 3つの電球を白にする
        bulb1.SetWhite();
        bulb2.SetWhite();
        bulb3.SetWhite();

        // Inspectorで設定した時間待つ
        yield return new WaitForSeconds(waitTime);

        // 次の電球状態を作る
        ResetBulbs();

        // 再びクリックできるようにする
        isWaiting = false;
    }


    // 今クリックできるか確認する
    public bool CanClick()
    {
        // 待機中でなければtrue
        return !isWaiting;
    }


    // 電球をランダムな状態にする
    void ResetBulbs()
    {
        // ランダムでtrueかfalseを設定する
        bulb1.SetIsOn(Random.value > 0.5f);
        bulb2.SetIsOn(Random.value > 0.5f);
        bulb3.SetIsOn(Random.value > 0.5f);


        // 3つ全部ONだった場合
        if (bulb1.GetIsOn() &&
            bulb2.GetIsOn() &&
            bulb3.GetIsOn())
        {
            // 0～2からランダムで1つ選ぶ
            int random = Random.Range(0, 3);

            // 選ばれた電球をOFFにする
            if (random == 0)
            {
                bulb1.SetIsOn(false);
            }

            if (random == 1)
            {
                bulb2.SetIsOn(false);
            }

            if (random == 2)
            {
                bulb3.SetIsOn(false);
            }
        }
    }
}