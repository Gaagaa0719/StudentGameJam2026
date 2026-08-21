using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSettingsController : MonoBehaviour
{
    [Header("Audio Mixer")]
    [SerializeField]
    private AudioMixer audioMixer; // MainAudioMixerをアタッチ


    [Header("Sliders")]
    [SerializeField]
    private Slider bgmSlider;

    [SerializeField]
    private Slider seSlider;


    [Header("Slide SE")]
    [SerializeField]
    private AudioSource seAudioSource; // スライドSE再生用（SE側のMixerグループに繋いだAudioSource）

    [SerializeField]
    private AudioClip slideSE; // スライダーを動かしているときのSE

    [SerializeField]
    private float slideSEInterval = 0.15f; // SEを鳴らす最短間隔（連続で鳴りすぎないようにする）

    private float slideSETimer = 0f;


    // PlayerPrefsに保存する際のキー名
    private const string BGM_KEY = "BGMVolume";
    private const string SE_KEY = "SEVolume";

    // Mixerに公開したパラメータ名（STEP1でリネームしたもの）
    private const string BGM_PARAM = "BGMVolume";
    private const string SE_PARAM = "SEVolume";


    private void Start()
    {
        // 保存された値を読み込む（初回は1.0=最大音量とする）
        float savedBGM = PlayerPrefs.GetFloat(BGM_KEY, 1.0f);
        float savedSE = PlayerPrefs.GetFloat(SE_KEY, 1.0f);

        if (bgmSlider != null)
        {
            bgmSlider.value = savedBGM;
            bgmSlider.onValueChanged.AddListener(OnBGMSliderChanged);
        }

        if (seSlider != null)
        {
            seSlider.value = savedSE;
            seSlider.onValueChanged.AddListener(OnSESliderChanged);
        }

        // 起動時点のMixerにも反映しておく
        ApplyBGMVolume(savedBGM);
        ApplySEVolume(savedSE);
    }


    private void Update()
    {
        // SE用タイマーを進める
        if (slideSETimer > 0f)
        {
            slideSETimer -= Time.deltaTime;
        }
    }


    // BGMスライダーが動かされた
    private void OnBGMSliderChanged(float value)
    {
        ApplyBGMVolume(value);
        PlayerPrefs.SetFloat(BGM_KEY, value);
        PlaySlideSE();
    }


    // SEスライダーが動かされた
    private void OnSESliderChanged(float value)
    {
        ApplySEVolume(value);
        PlayerPrefs.SetFloat(SE_KEY, value);
        PlaySlideSE();
    }


    private void ApplyBGMVolume(float sliderValue)
    {
        if (audioMixer != null)
        {
            audioMixer.SetFloat(BGM_PARAM, ConvertToDecibel(sliderValue));
        }
    }


    private void ApplySEVolume(float sliderValue)
    {
        if (audioMixer != null)
        {
            audioMixer.SetFloat(SE_PARAM, ConvertToDecibel(sliderValue));
        }
    }


    // スライダー操作中のSEを、鳴らしすぎないよう間隔を空けて再生する
    private void PlaySlideSE()
    {
        if (seAudioSource == null || slideSE == null) return;
        if (slideSETimer > 0f) return;

        seAudioSource.PlayOneShot(slideSE);
        slideSETimer = slideSEInterval;
    }


    // スライダー値(0～1)をMixer用のデシベル値に変換
    private float ConvertToDecibel(float sliderValue)
    {
        // 0の場合はLog10がマイナス無限大になるため、下限を設けて無音にする
        if (sliderValue <= 0.0001f)
        {
            return -80f; // ほぼ無音
        }

        return Mathf.Log10(sliderValue) * 20f;
    }
}