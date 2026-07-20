// WinResultPresenter.cs
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Game.Result
{
    public class WinResultPresenter : MonoBehaviour
    {
        [Header("参照")]
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private AudioSource _voiceAudioSource;
        [SerializeField] private TextMeshProUGUI _resultText;
        [SerializeField] private CanvasGroup _resultTextCanvasGroup;
        [SerializeField] private RectTransform _resultTextTargetPos;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private CanvasGroup _scoreTextCanvasGroup;
        [SerializeField] private Image _rankImage;
        [SerializeField] private CanvasGroup _rankImageCanvasGroup;
        [SerializeField] private CanvasGroup _buttonGroupCanvasGroup;
        [SerializeField] private Button _retryButton;
        [SerializeField] private Button _toTitleButton;

        [Header("評価ランク画像")]
        [SerializeField] private RankSpriteEntry[] _rankSprites;

        [Header("演出設定:勝利文字の拡大")]
        [SerializeField] private float _textPopDuration = 0.5f;
        [SerializeField] private float _textSettleDuration = 0.15f;
        [SerializeField] private Vector3 _textStartScale = Vector3.zero;
        [SerializeField] private Vector3 _textPeakScale = Vector3.one * 1.2f;
        [SerializeField] private Vector3 _textNormalScale = Vector3.one;

        [Header("演出設定:文字の左上移動")]
        [SerializeField] private float _textMoveDuration = 0.5f;
        [SerializeField] private Vector3 _textShrinkScale = Vector3.one * 0.5f;

        [Header("演出設定:スコアカウントアップ")]
        [SerializeField] private float _scoreFadeInDuration = 0.3f;
        [SerializeField] private float _scoreCountUpDuration = 1.0f;

        [Header("演出設定:評価スタンプ")]
        [SerializeField] private float _rankPopDuration = 0.35f;
        [SerializeField] private Vector3 _rankStartScale = Vector3.zero;
        [SerializeField] private Vector3 _rankPeakScale = Vector3.one * 1.3f;
        [SerializeField] private Vector3 _rankNormalScale = Vector3.one;

        [Header("演出設定:ボタン表示")]
        [SerializeField] private float _buttonFadeInDuration = 0.3f;

        [Header("シーン遷移設定")]
        [SerializeField] private string _titleSceneName = "SampleScene";
        [SerializeField] private string _gameplaySceneName = "Game";

        private Coroutine _sequenceCoroutine;
        private Vector3 _resultTextStartPos;

        [Serializable]
        public struct RankSpriteEntry
        {
            public ResultRank Rank;
            public Sprite Sprite;
        }

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
            // 1. 背景の一枚絵を表示
            _backgroundImage.sprite = data.WinBackgroundImage;
            _backgroundImage.enabled = true;

            // 2. 初期状態を非表示に揃える
            _resultTextCanvasGroup.alpha = 0f;
            _resultText.rectTransform.localScale = _textStartScale;
            _resultTextStartPos = _resultText.rectTransform.anchoredPosition3D;

            _scoreTextCanvasGroup.alpha = 0f;

            _rankImageCanvasGroup.alpha = 0f;
            _rankImage.rectTransform.localScale = _rankStartScale;

            _buttonGroupCanvasGroup.alpha = 0f;
            _buttonGroupCanvasGroup.interactable = false;
            _buttonGroupCanvasGroup.blocksRaycasts = false;

            // 3. キャラクターボイス再生
            if (data.CharacterVoice != null)
            {
                _voiceAudioSource.clip = data.CharacterVoice;
                _voiceAudioSource.Play();
                yield return new WaitForSeconds(data.CharacterVoice.length);
            }

            // 4. 「勝利」文字を勢いよく拡大表示
            yield return StartCoroutine(PlayResultTextPop());

            // 5. 「勝利」文字を左上へ縮小しながら移動
            yield return StartCoroutine(PlayResultTextMoveToCorner());

            // 6. スコアのフェードイン&カウントアップ
            yield return StartCoroutine(PlayScoreCountUp(data.FinalScore));

            // 7. 評価ランク画像をスタンプのように表示
            yield return StartCoroutine(PlayRankStamp(data.Rank));

            // 8. ボタンをフェードインし操作可能にする
            yield return StartCoroutine(PlayButtonsFadeIn());
        }

        private IEnumerator PlayResultTextPop()
        {
            _resultTextCanvasGroup.alpha = 1f;

            yield return TweenUtil.Run(_textPopDuration, TweenUtil.EaseOutBack, t =>
            {
                _resultText.rectTransform.localScale = Vector3.LerpUnclamped(_textStartScale, _textPeakScale, t);
            });

            yield return TweenUtil.Run(_textSettleDuration, null, t =>
            {
                _resultText.rectTransform.localScale = Vector3.Lerp(_textPeakScale, _textNormalScale, t);
            });
        }

        private IEnumerator PlayResultTextMoveToCorner()
        {
            Vector3 startPos = _resultTextStartPos;
            Vector3 targetPos = _resultTextTargetPos.anchoredPosition3D;
            Vector3 startScale = _resultText.rectTransform.localScale;

            yield return TweenUtil.Run(_textMoveDuration, TweenUtil.EaseInOutCubic, t =>
            {
                _resultText.rectTransform.anchoredPosition3D = Vector3.Lerp(startPos, targetPos, t);
                _resultText.rectTransform.localScale = Vector3.Lerp(startScale, _textShrinkScale, t);
            });
        }

        private IEnumerator PlayScoreCountUp(int finalScore)
        {
            _scoreText.text = "SCORE : 0";
            yield return TweenUtil.Run(_scoreFadeInDuration, null, t =>
            {
                _scoreTextCanvasGroup.alpha = t;
            });

            yield return TweenUtil.Run(_scoreCountUpDuration, null, t =>
            {
                int current = Mathf.RoundToInt(Mathf.Lerp(0, finalScore, t));
                _scoreText.text = $"SCORE : {current}";
            });

            _scoreText.text = $"SCORE : {finalScore}";
        }

        private IEnumerator PlayRankStamp(ResultRank rank)
        {
            _rankImage.sprite = GetRankSprite(rank);
            _rankImageCanvasGroup.alpha = 1f;

            // 0 → 1.3倍まで勢いよく拡大(スタンプ的な「ポン」)
            yield return TweenUtil.Run(_rankPopDuration, TweenUtil.EaseOutBack, t =>
            {
                _rankImage.rectTransform.localScale = Vector3.LerpUnclamped(_rankStartScale, _rankPeakScale, t);
            });

            // 1.3倍 → 等倍で着地
            yield return TweenUtil.Run(0.1f, null, t =>
            {
                _rankImage.rectTransform.localScale = Vector3.Lerp(_rankPeakScale, _rankNormalScale, t);
            });
        }

        private Sprite GetRankSprite(ResultRank rank)
        {
            foreach (var entry in _rankSprites)
            {
                if (entry.Rank == rank)
                {
                    return entry.Sprite;
                }
            }

            Debug.LogWarning($"ランク {rank} に対応するSpriteが設定されていません。");
            return null;
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

        // ボタンのOnClickから呼ぶ想定(手順8で配線)
        public void OnClickRetry()
        {
            Debug.Log($"[WinResultPresenter] Retryボタンが押されました。遷移先想定シーン: {_gameplaySceneName}");
            UnityEngine.SceneManagement.SceneManager.LoadScene(_gameplaySceneName);
        }

        public void OnClickToTitle()
        {
            Debug.Log($"[WinResultPresenter] タイトルに戻るボタンが押されました。遷移先想定シーン: {_titleSceneName}");
            UnityEngine.SceneManagement.SceneManager.LoadScene(_titleSceneName);
        }
    }
}