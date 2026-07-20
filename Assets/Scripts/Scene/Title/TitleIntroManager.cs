using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TitleIntroManager : MonoBehaviour
{
    [Header("Fade Panel")]
    [SerializeField]
    private Image fadePanel; // シーン開始時に使う黒フェード用Image（TitleTransitionManagerと共用可）


    [Header("Timing")]
    [SerializeField]
    private float fadeInDuration = 1.0f; // 画面が黒から明けるまでの時間


    [Header("Logo")]
    [SerializeField]
    private TitleLogoController titleLogo; // 画面フェード完了後にロゴのフェードを開始させる


    private void Start()
    {
        StartCoroutine(IntroRoutine());
    }


    private IEnumerator IntroRoutine()
    {
        // 画面フェードイン（黒→透明）
        if (fadePanel != null)
        {
            Color c = fadePanel.color;
            c.a = 1.0f; // シーン開始時は真っ黒から
            fadePanel.color = c;

            float t = 0f;
            while (t < fadeInDuration)
            {
                t += Time.deltaTime;
                c.a = 1.0f - Mathf.Clamp01(t / fadeInDuration);
                fadePanel.color = c;
                yield return null;
            }

            c.a = 0.0f;
            fadePanel.color = c;
        }

        // 画面フェードが終わってからロゴのフェードを開始
        if (titleLogo != null)
        {
            titleLogo.BeginFade();
        }
    }
}