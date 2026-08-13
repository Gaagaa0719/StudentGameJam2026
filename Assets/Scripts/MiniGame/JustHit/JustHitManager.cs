using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class JustHitManager : IMiniGameManager
{
    public static JustHitManager instance;

    [Header("成功時のSE")]
    [SerializeField]
    private AudioClip successSound;

    [Header("成功時のエフェクト")]
    [SerializeField]
    private GameObject successEffectPrefab;

    private RectTransform note;
    private HitArea hitArea;
    private AudioSource audioSource;

    private void Awake()
    {
        instance = this;

        group = GetComponent<CanvasGroup>();

        audioSource =
            GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource =
                gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
    }

    private void Start()
    {
        FindObjects();
    }

    private void FindObjects()
    {
        if (note == null)
        {
            GameObject noteObject =
                GameObject.Find("NoteMover");

            if (noteObject != null)
            {
                note =
                    noteObject.GetComponent<RectTransform>();
            }
        }

        if (hitArea == null)
        {
            hitArea =
                FindFirstObjectByType<HitArea>();
        }
    }

    private void Update()
    {
        if (!isPlaying)
        {
            return;
        }

        if (Mouse.current == null)
        {
            return;
        }

        if (
            Mouse.current.leftButton
                .wasPressedThisFrame
        )
        {
            CheckHit();
        }
    }

    private void CheckHit()
    {
        FindObjects();

        if (note == null)
        {
            Debug.Log(
                "失敗：NoteMoverが見つかりません"
            );

            EndGame(false);

            return;
        }

        if (hitArea == null)
        {
            Debug.Log(
                "失敗：HitAreaが見つかりません"
            );

            EndGame(false);

            return;
        }

        bool isOverlapping =
            hitArea.IsNoteInside(note);

        if (isOverlapping)
        {
            Debug.Log("成功");

            PlaySuccessSound();

            // 成功時だけエフェクト
            PlaySuccessEffect();

            EndGame(true);
        }
        else
        {
            Debug.Log("失敗");

            EndGame(false);
        }
    }

    private void PlaySuccessSound()
    {
        if (
            audioSource != null &&
            successSound != null
        )
        {
            audioSource.PlayOneShot(
                successSound
            );
        }
    }

    private void PlaySuccessEffect()
    {
        if (successEffectPrefab == null)
        {
            Debug.LogWarning(
                "Success Effect Prefabが設定されていません"
            );

            return;
        }

        if (note == null)
        {
            Debug.LogWarning(
                "NoteMoverが見つからないためエフェクトを出せません"
            );

            return;
        }

        // エフェクトを生成
        GameObject effect =
            Instantiate(
                successEffectPrefab
            );

        // ノーツと同じ位置にする
        effect.transform.position =
            note.position;

        // ParticleSystemを取得
        ParticleSystem particle =
            effect.GetComponent<ParticleSystem>();

        if (particle == null)
        {
            particle =
                effect.GetComponentInChildren<ParticleSystem>();
        }

        // 強制的に再生
        if (particle != null)
        {
            particle.Clear(true);
            particle.Play(true);

            Debug.Log(
                "成功エフェクトを再生しました"
            );
        }
        else
        {
            Debug.LogWarning(
                "PrefabにParticle Systemがありません"
            );
        }

        // 3秒後に削除
        Destroy(
            effect,
            3f
        );
    }

    public override IEnumerator StartGame(
        float DegreePoint
    )
    {
        FindObjects();

        if (hitArea != null)
        {
            hitArea.Init(
                DegreePoint
            );
        }

        yield return base.StartGame(
            DegreePoint
        );
    }

    public void EndGame(
        bool isSuccess
    )
    {
        this.isSuccess =
            isSuccess;

        isPlaying = false;
    }

    public bool GetIsPlaying()
    {
        return isPlaying;
    }
}