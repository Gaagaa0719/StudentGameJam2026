using UnityEngine;
using UnityEngine.UI;

public class TitleLogoController : MonoBehaviour
{
    private Image img;
    private RectTransform rTras;

    //  メンバ変数
    //  フェード用
    [Header("Fade Settings")]
    [SerializeField]
    private float fadeDuration = 2.0f;

    private float fadeTime = 0.0f;
    private bool isFadeStarted = false;
    private bool isFadeFinished = false;


    //  浮遊用
    [Header("Float Settings")]
    [SerializeField]
    private float floatAmplitude = 15.0f;

    [SerializeField]
    private float floatSpeed = 1.0f;

    private Vector2 startPos;


    //  初期化
    private void Start()
    {
        Init();
    }


    //  初期化
    private void Init()
    {
        img = GetComponent<Image>();
        rTras = GetComponent<RectTransform>();

        Color color = img.color;
        color.a = 0.0f;
        img.color = color;

        fadeTime = 0.0f;
        isFadeStarted = false;
        isFadeFinished = false;

        startPos = rTras.anchoredPosition;
    }


    /// <summary>
    /// 外部から呼び出すことで、ロゴのフェードインを開始する
    /// </summary>
    public void BeginFade()
    {
        if (isFadeStarted) return;

        isFadeStarted = true;
        fadeTime = 0.0f;
        isFadeFinished = false;
    }


    //  更新
    private void Update()
    {
        if (isFadeStarted && !isFadeFinished)
        {
            Fade();
        }

        Float();
    }


    //  フェード処理
    private void Fade()
    {
        //  フェード時間加算
        fadeTime += Time.deltaTime;

        //  フェード率計算
        float t = fadeTime / fadeDuration;
        t = Mathf.Clamp01(t);

        Color color = img.color;
        color.a = t;
        img.color = color;

        if (t >= 1.0f) isFadeFinished = true;
    }


    //  浮遊処理
    private void Float()
    {
        float offsetY = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        rTras.anchoredPosition = startPos + Vector2.up * offsetY;
    }
}