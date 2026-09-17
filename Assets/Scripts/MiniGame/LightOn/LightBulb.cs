using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// 電球1個の動きを管理する
public class LightBulb : MonoBehaviour, IPointerClickHandler
{
    // 電球の現在の状態
    // false = OFF
    // true = ON
    bool isOn = false;

    // ゲーム全体を管理するスクリプト
    public LightBulbGame game;

    // この電球のImage
    public Image bulbImage;


    // Unityで最初に読み込まれたとき
    void Start()
    {
        // 最初の状態に合わせて色を変更
        UpdateVisual();
    }


    // この電球がクリックされたとき
    public void OnPointerClick(PointerEventData eventData)
    {
        // 待機中ならクリックできない
        if (!game.CanClick())
        {
            return;
        }

        // trueとfalseを入れ替える
        isOn = !isOn;

        // 色を更新する
        UpdateVisual();

        // 現在の状態をConsoleに表示
        Debug.Log(gameObject.name + " : " + isOn);

        // 3つ全部ONになったか確認
        game.CheckClear();
    }


    // 現在のON/OFFを返す
    public bool GetIsOn()
    {
        return isOn;
    }


    // 外部からON/OFFを変更する
    public void SetIsOn(bool state)
    {
        // 状態を変更
        isOn = state;

        // 色も更新
        UpdateVisual();
    }


    // 電球の状態に合わせて色を変更する
    void UpdateVisual()
    {
        // ONの場合
        if (isOn)
        {
            // 黄色
            bulbImage.color = Color.yellow;
        }
        else
        {
            // OFFの場合
            // 灰色
            bulbImage.color = Color.gray;
        }
    }


    // クリア後の待機中に電球を白にする
    public void SetWhite()
    {
        bulbImage.color = Color.white;
    }
}