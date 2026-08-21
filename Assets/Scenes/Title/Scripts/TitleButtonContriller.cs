using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TitleButtonController : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerClickHandler
{
    public enum ButtonType
    {
        StartButton,
        SettingButton,
        ExitButton
    }

    [Header("Button Type")]
    [SerializeField]
    private ButtonType buttonType;


    [Header("Hover Frame")]
    [SerializeField]
    private Image hoverFrame; // ボタンより一回り大きい枠用Image（Raycast Targetはオフにしておく）


    [Header("Colors")]
    [SerializeField]
    private Color hoverColor = Color.yellow;

    [SerializeField]
    private Color clickColor = Color.red;


    [Header("SE")]
    [SerializeField]
    private AudioSource audioSource; // SE再生用（このボタンと同じGameObjectか、共通のものをアタッチ）

    [SerializeField]
    private AudioClip hoverSE; // カーソルが乗ったときのSE

    [SerializeField]
    private AudioClip clickSE; // クリックしたときのSE


    [Header("Transition")]
    [SerializeField]
    private TitleTransitionManager transitionManager; // 演出～シーン遷移を管理するマネージャー

    [SerializeField]
    private string nextSceneName = "SampleScene"; // Start押下後に遷移するシーン名


    [Header("Setting Panel")]
    [SerializeField]
    private GameObject settingPanel; // SettingButton押下時に開閉するパネル（SettingButton用にのみ設定）


    private bool isClicking = false;


    private void Start()
    {
        SetFrameVisible(false);
    }


    // カーソルが乗った
    public void OnPointerEnter(PointerEventData eventData)
    {
        SetFrameColor(hoverColor);
        SetFrameVisible(true);
        PlaySE(hoverSE);
    }


    // カーソルが離れた
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isClicking)
        {
            SetFrameVisible(false);
        }
    }


    // クリック
    public void OnPointerClick(PointerEventData eventData)
    {
        isClicking = true;

        SetFrameColor(clickColor);
        SetFrameVisible(true);
        PlaySE(clickSE);

        ExecuteButton();
    }


    private void SetFrameColor(Color color)
    {
        if (hoverFrame != null)
        {
            Color c = color;
            c.a = 1f; // 色そのものはそのまま使う。表示/非表示はAlphaで別管理
            hoverFrame.color = c;
        }
    }


    private void SetFrameVisible(bool visible)
    {
        if (hoverFrame != null)
        {
            Color c = hoverFrame.color;
            c.a = visible ? 1f : 0f;
            hoverFrame.color = c;
        }
    }


    private void PlaySE(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }


    /// 外部（CloseButtonなど）から呼び出し、枠の状態を通常に戻す

    public void ResetFrameState()
    {
        isClicking = false;
        SetFrameVisible(false);
    }


    private void ExecuteButton()
    {
        switch (buttonType)
        {
            case ButtonType.StartButton:

                Debug.Log("Start Button");

                // 演出後にシーン遷移
                if (transitionManager != null)
                {
                    transitionManager.PlayTransition(nextSceneName);
                }
                else
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
                }

                break;


            case ButtonType.SettingButton:

                Debug.Log("Setting Button");

                // 設定パネルの表示/非表示を切り替え
                if (settingPanel != null)
                {
                    settingPanel.SetActive(!settingPanel.activeSelf);
                }

                break;


            case ButtonType.ExitButton:

                Debug.Log("Exit Button");

                // ゲーム終了
                Application.Quit();

                break;
        }
    }
}