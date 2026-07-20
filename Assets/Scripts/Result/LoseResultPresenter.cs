// LoseResultPresenter.cs
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Game.Result
{
    public class LoseResultPresenter : MonoBehaviour
    {
        [Header("参照")]
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private AudioSource _bgmAudioSource;
        [SerializeField] private TextMeshProUGUI _resultText;
        [SerializeField] private CanvasGroup _resultTextCanvasGroup;
        [SerializeField] private RectTransform _resultTextStartPos;
        [SerializeField] private RectTransform _resultTextTargetPos;
        [SerializeField] private CanvasGroup _buttonGroupCanvasGroup;
        [SerializeField] private Button _retryButton;
        [SerializeField] private Button _toTitleButton;

        [Header("敗北時の固定背景画像")]
        [SerializeField] private Sprite _fixedBackgroundSprite; // ゴミ箱に吐く後ろ姿の固定画像

        [Header("BGM")]
        [SerializeField] private AudioClip _loseBgmClip;

        [Header("演出設定:敗北文字のフェード&落下")]
        [SerializeField] private float _textFallDuration = 0.6f;

        [Header("演出設定:ボタン表示")]
        [SerializeField] private float _buttonFadeInDuration = 0.3f;

        [Header("シーン遷移設定")]
        [SerializeField] private string _titleSceneName = "TitleScene";
        [SerializeField] private string _gameplaySceneName = "SampleScene";

        private Coroutine _sequenceCoroutine;

        public void Play(ResultSceneData data)
        {
            if (_sequenceCoroutine != null)
            {
                StopCoroutine(_sequenceCoroutine);
            }
            _sequenceCoroutine = StartCoroutine(PlaySequence(data));
        }

        private IEnumerator PlaySequence(ResultSceneData data)
        {
            // 1. 背景は固定画像を使用(データからは受け取らない)
            _backgroundImage.sprite = _fixedBackgroundSprite;
            _backgroundImage.enabled = true;

            // BGM再生開始
            if (_loseBgmClip != null)
            {
                _bgmAudioSource.clip = _loseBgmClip;
                _bgmAudioSource.Play();
            }

            // 2. 初期状態を非表示に揃える
            _resultTextCanvasGroup.alpha = 0f;
            _resultText.rectTransform.anchoredPosition3D = _resultTextStartPos.anchoredPosition3D;

            _buttonGroupCanvasGroup.alpha = 0f;
            _buttonGroupCanvasGroup.interactable = false;
            _buttonGroupCanvasGroup.blocksRaycasts = false;

            // 3. 「敗北…」文字をフェードしながら上から落ちてくるように表示
            yield return StartCoroutine(PlayResultTextFall());

            // 4. ボタンをフェードインし操作可能にする
            yield return StartCoroutine(PlayButtonsFadeIn());
        }

        private IEnumerator PlayResultTextFall()
        {
            Vector3 startPos = _resultTextStartPos.anchoredPosition3D;
            Vector3 targetPos = _resultTextTargetPos.anchoredPosition3D;

            yield return TweenUtil.Run(_textFallDuration, TweenUtil.EaseInOutCubic, t =>
            {
                _resultTextCanvasGroup.alpha = t;
                _resultText.rectTransform.anchoredPosition3D = Vector3.Lerp(startPos, targetPos, t);
            });
        }

        private IEnumerator PlayButtonsFadeIn()
        {
            yield return TweenUtil.Run(_buttonFadeInDuration, null, t =>
            {
                _buttonGroupCanvasGroup.alpha = t;
            });

            _buttonGroupCanvasGroup.interactable = true;
            _buttonGroupCanvasGroup.blocksRaycasts = true;
        }

        public void OnClickRetry()
        {
            Debug.Log($"[LoseResultPresenter] Retryボタンが押されました。遷移先想定シーン: {_gameplaySceneName}");
            UnityEngine.SceneManagement.SceneManager.LoadScene(_gameplaySceneName);
        }

        public void OnClickToTitle()
        {
            Debug.Log($"[LoseResultPresenter] タイトルに戻るボタンが押されました。遷移先想定シーン: {_titleSceneName}");
            UnityEngine.SceneManagement.SceneManager.LoadScene(_titleSceneName);
        }
    }
}