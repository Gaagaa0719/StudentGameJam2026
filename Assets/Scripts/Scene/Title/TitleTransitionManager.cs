using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleTransitionManager : MonoBehaviour
{
    public enum Direction
    {
        Top,
        Bottom,
        Left,
        Right,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }

    [System.Serializable]
    public class CharacterEntry
    {
        public RectTransform rectTransform;   // キャラクターのRectTransform（最終位置に配置しておく）
        public Direction direction;           // どの方向から飛び出してくるか
        public AudioClip voiceClip;           // ボイス（未用意ならnullのままでOK）
    }


    [Header("Characters (8体)")]
    [SerializeField]
    private CharacterEntry[] characters;


    [Header("Fade Panel")]
    [SerializeField]
    private Image fadePanel; // 既存のFadePanelのImage（黒、初期Alpha=0を想定）


    [Header("Audio")]
    [SerializeField]
    private AudioSource audioSource; // ボイス再生用（未設定でも動作します）


    [Header("Timing")]
    [SerializeField]
    private float dashDuration = 0.2f;      // キャラが「しゅっ」と登場する時間

    [SerializeField]
    private float offscreenDistance = 1500f; // 画面外とみなす距離

    [SerializeField]
    private float holdDuration = 1.0f;      // 登場後、ボイス再生も含めて待機する時間

    [SerializeField]
    private float fadeDuration = 0.5f;      // フェードアウトにかける時間


    private bool isPlaying = false;

    // Inspectorで配置した「最終的にいてほしい位置」を記録しておく配列
    private Vector2[] endPositions;


    private void Awake()
    {
        // 起動時に一度だけ、最終位置（Inspectorで配置した位置）を記録
        endPositions = new Vector2[characters.Length];

        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i].rectTransform != null)
            {
                endPositions[i] = characters[i].rectTransform.anchoredPosition;
            }
        }
    }


    private void Start()
    {
        // シーン開始時点では、演出が始まるまでキャラクターを画面外に退避させておく
        for (int i = 0; i < characters.Length; i++)
        {
            var entry = characters[i];
            if (entry.rectTransform == null) continue;

            Vector2 offset = GetDirectionOffset(entry.direction) * offscreenDistance;
            entry.rectTransform.anchoredPosition = endPositions[i] + offset;
        }
    }


    /// 演出を再生し、完了後に指定シーンへ遷移する

    public void PlayTransition(string sceneName)
    {
        if (isPlaying) return;
        StartCoroutine(TransitionRoutine(sceneName));
    }


    private IEnumerator TransitionRoutine(string sceneName)
    {
        isPlaying = true;

        // キャラクターを「しゅっ」と登場させる（全員同時、画面外→最終位置）
        float t = 0f;
        while (t < dashDuration)
        {
            t += Time.deltaTime;
            float ratio = Mathf.Clamp01(t / dashDuration);
            float eased = 1f - Mathf.Pow(1f - ratio, 3f); // ease-out

            for (int i = 0; i < characters.Length; i++)
            {
                var entry = characters[i];
                if (entry.rectTransform == null) continue;

                Vector2 offset = GetDirectionOffset(entry.direction) * offscreenDistance;
                Vector2 startPos = endPositions[i] + offset;
                entry.rectTransform.anchoredPosition = Vector2.LerpUnclamped(startPos, endPositions[i], eased);
            }

            yield return null;
        }

        // 位置を確実に補正
        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i].rectTransform != null)
            {
                characters[i].rectTransform.anchoredPosition = endPositions[i];
            }
        }

        // ボイス再生（未設定のクリップはスキップ）
        if (audioSource != null)
        {
            foreach (var entry in characters)
            {
                if (entry.voiceClip != null)
                {
                    audioSource.PlayOneShot(entry.voiceClip);
                }
            }
        }

        // 少し待機（ボイスの尺や間を持たせる）
        yield return new WaitForSeconds(holdDuration);

        // 画面フェードアウト
        if (fadePanel != null)
        {
            float fadeT = 0f;
            Color c = fadePanel.color;

            while (fadeT < fadeDuration)
            {
                fadeT += Time.deltaTime;
                c.a = Mathf.Clamp01(fadeT / fadeDuration);
                fadePanel.color = c;
                yield return null;
            }

            c.a = 1f;
            fadePanel.color = c;
        }

        // シーン遷移
        SceneManager.LoadScene(sceneName);
    }


    private Vector2 GetDirectionOffset(Direction dir)
    {
        switch (dir)
        {
            case Direction.Top: return new Vector2(0f, 1f);
            case Direction.Bottom: return new Vector2(0f, -1f);
            case Direction.Left: return new Vector2(-1f, 0f);
            case Direction.Right: return new Vector2(1f, 0f);
            case Direction.TopLeft: return new Vector2(-1f, 1f).normalized;
            case Direction.TopRight: return new Vector2(1f, 1f).normalized;
            case Direction.BottomLeft: return new Vector2(-1f, -1f).normalized;
            case Direction.BottomRight: return new Vector2(1f, -1f).normalized;
            default: return Vector2.zero;
        }
    }
}