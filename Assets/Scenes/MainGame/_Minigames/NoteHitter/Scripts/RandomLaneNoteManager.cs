using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RandomLaneNoteManager : MiniGame
{
    [Header("上に置いてある元のノーツ")]
    [SerializeField]
    private RectTransform noteTemplate;

    [Header("左側の灰色の成功判定")]
    [SerializeField]
    private RectTransform leftHitArea;

    [Header("右側の灰色の成功判定")]
    [SerializeField]
    private RectTransform rightHitArea;

    [Header("成功時の効果音")]
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip successSE;

    [Header("成功時のエフェクト")]
    [SerializeField]
    private GameObject successEffectPrefab;

    // ★追加
    [Header("成功エフェクトの大きさ")]
    [SerializeField]
    private float successEffectScale = 5f;

    [Header("ノーツ設定")]
    [SerializeField]
    private int totalNoteCount = 6;

    [SerializeField]
    private float spawnInterval = 0.3f;

    [Header("基本の落下速度")]
    [SerializeField]
    private float baseFallSpeed = 200f;

    [Header("酔い度1につき増える速度")]
    [SerializeField]
    private float speedIncreasePerDrunkenness = 1f;

    [Header("最大落下速度")]
    [SerializeField]
    private float maxFallSpeed = 800f;

    [Header("上端から少し空ける距離")]
    [SerializeField]
    private float spawnTopMargin = 20f;

    [Header("判定を通過した後に消える距離")]
    [SerializeField]
    private float missDistance = 300f;

    [Header("Unityの再生ボタンでテスト")]
    [SerializeField]
    private bool autoStartForTest = true;

    [Header("テスト時の酔い度")]
    [SerializeField]
    private float testDrunkenness = 0f;

    private RectTransform noteParent;

    private List<RectTransform> activeNotes =
        new List<RectTransform>();

    private RectTransform currentNote;

    private int spawnedNoteCount = 0;
    private int resolvedNoteCount = 0;
    private int successCount = 0;
    private int clickCount = 0;
    private int pendingMissCount = 0;

    private bool gameFinished = false;
    private bool testMode = false;

    private float currentFallSpeed;

    private void Awake()
    {
        if (audioSource == null)
        {
            audioSource =
                GetComponent<AudioSource>();

            if (audioSource == null)
            {
                audioSource =
                    gameObject.AddComponent<AudioSource>();
            }
        }

        audioSource.playOnAwake = false;
    }

    private void Start()
    {
        FindObjects();

        if (autoStartForTest && !isPlaying)
        {
            Debug.Log(
                "2つ目のミニゲームをテスト開始"
            );

            testMode = true;
            isPlaying = true;

            OnStart(
                testDrunkenness
            );
        }
    }

    private void Update()
    {
        if (!isPlaying)
        {
            return;
        }

        if (gameFinished)
        {
            return;
        }

        MoveNotes();

        CheckMouseClick();
    }

    // =========================================
    // ミニゲーム開始
    // =========================================

    protected override void OnStart(float dp)
    {
        Debug.Log(
            "2つ目のミニゲーム開始　酔い度：" + dp
        );

        StopAllCoroutines();

        ClearAllNotes();

        spawnedNoteCount = 0;
        resolvedNoteCount = 0;
        successCount = 0;
        clickCount = 0;
        pendingMissCount = 0;

        currentNote = null;

        gameFinished = false;

        totalNoteCount = 6;

        FindObjects();

        CalculateFallSpeed(
            dp
        );

        if (noteTemplate == null)
        {
            Debug.LogError(
                "Note Templateが設定されていません"
            );

            EndMiniGame(false);

            return;
        }

        if (leftHitArea == null)
        {
            Debug.LogError(
                "Left Hit Areaが設定されていません"
            );

            EndMiniGame(false);

            return;
        }

        if (rightHitArea == null)
        {
            Debug.LogError(
                "Right Hit Areaが設定されていません"
            );

            EndMiniGame(false);

            return;
        }

        noteParent = transform as RectTransform;

        if (noteParent == null)
        {
            Debug.LogError(
                "Note Templateの親にRectTransformがありません"
            );

            EndMiniGame(false);

            return;
        }

        noteTemplate.gameObject.SetActive(
            false
        );

        StartCoroutine(
            SpawnNotes()
        );
    }

    // =========================================
    // 酔い度による速度
    // =========================================

    private void CalculateFallSpeed(float dp)
    {
        float drunkenness =
            Mathf.Max(
                dp,
                0f
            );

        currentFallSpeed =
            baseFallSpeed +
            (
                drunkenness *
                speedIncreasePerDrunkenness
            );

        currentFallSpeed =
            Mathf.Min(
                currentFallSpeed,
                maxFallSpeed
            );

        Debug.Log(
            "現在のノーツ速度：" +
            currentFallSpeed
        );
    }

    // =========================================
    // オブジェクトを探す
    // =========================================

    private void FindObjects()
    {
        if (noteTemplate == null)
        {
            GameObject obj =
                GameObject.Find(
                    "Note Template"
                );

            if (obj != null)
            {
                noteTemplate =
                    obj.GetComponent<RectTransform>();
            }
        }

        if (leftHitArea == null)
        {
            GameObject obj =
                GameObject.Find(
                    "Left Hit Area"
                );

            if (obj != null)
            {
                leftHitArea =
                    obj.GetComponent<RectTransform>();
            }
        }

        if (rightHitArea == null)
        {
            GameObject obj =
                GameObject.Find(
                    "Right Hit Area"
                );

            if (obj != null)
            {
                rightHitArea =
                    obj.GetComponent<RectTransform>();
            }
        }
    }

    // =========================================
    // ノーツ生成
    // =========================================

    private IEnumerator SpawnNotes()
    {
        for (
            int noteNumber = 1;
            noteNumber <= totalNoteCount;
            noteNumber++
        )
        {
            if (!isPlaying || gameFinished)
            {
                yield break;
            }

            if (pendingMissCount > 0)
            {
                pendingMissCount--;

                spawnedNoteCount++;
                resolvedNoteCount++;

                Debug.Log(
                    noteNumber +
                    "個目はミスクリックにより消費されました"
                );

                CheckGameEnd();

                if (gameFinished)
                {
                    yield break;
                }

                yield return new WaitForSeconds(
                    spawnInterval
                );

                continue;
            }

            CreateNote(
                noteNumber
            );

            // 1個が処理されるまで次を出さない
            while (
                currentNote != null &&
                isPlaying &&
                !gameFinished
            )
            {
                yield return null;
            }

            if (gameFinished)
            {
                yield break;
            }

            yield return new WaitForSeconds(
                spawnInterval
            );
        }

        CheckGameEnd();
    }

    // =========================================
    // 1個のノーツを作る
    // =========================================

    private void CreateNote(int noteNumber)
    {
        RectTransform newNote =
            Instantiate(
                noteTemplate,
                noteParent
            );

        newNote.gameObject.SetActive(
            true
        );

        bool isLeftLane =
            Random.Range(0, 2) == 0;

        RectTransform selectedHitArea;

        if (isLeftLane)
        {
            selectedHitArea =
                leftHitArea;

            newNote.name =
                "FallingNote_" +
                noteNumber +
                "_Left";
        }
        else
        {
            selectedHitArea =
                rightHitArea;

            newNote.name =
                "FallingNote_" +
                noteNumber +
                "_Right";
        }

        Vector3 hitAreaLocalPosition =
            noteParent.InverseTransformPoint(
                selectedHitArea.position
            );

        float halfNoteHeight =
            newNote.rect.height / 2f;

        float startY =
            noteParent.rect.yMax -
            halfNoteHeight -
            spawnTopMargin;

        newNote.localPosition =
            new Vector3(
                hitAreaLocalPosition.x,
                startY,
                0f
            );

        activeNotes.Add(
            newNote
        );

        currentNote =
            newNote;

        spawnedNoteCount++;

        Debug.Log(
            "生成数：" +
            spawnedNoteCount +
            " / " +
            totalNoteCount
        );

        if (isLeftLane)
        {
            Debug.Log(
                noteNumber +
                "個目：左レーン"
            );
        }
        else
        {
            Debug.Log(
                noteNumber +
                "個目：右レーン"
            );
        }
    }

    // =========================================
    // ノーツ移動
    // =========================================

    private void MoveNotes()
    {
        for (
            int i =
                activeNotes.Count - 1;
            i >= 0;
            i--
        )
        {
            RectTransform note =
                activeNotes[i];

            if (note == null)
            {
                activeNotes.RemoveAt(
                    i
                );

                continue;
            }

            note.localPosition +=
                Vector3.down *
                currentFallSpeed *
                Time.deltaTime;

            float hitAreaY =
                Mathf.Min(
                    leftHitArea.position.y,
                    rightHitArea.position.y
                );

            if (
                note.position.y <
                hitAreaY -
                missDistance
            )
            {
                Debug.Log(
                    "失敗：ノーツを逃しました"
                );

                ResolveMissedNote(
                    note
                );

                break;
            }
        }
    }

    // =========================================
    // ノーツを逃した
    // =========================================

    private void ResolveMissedNote(
        RectTransform note
    )
    {
        activeNotes.Remove(
            note
        );

        if (currentNote == note)
        {
            currentNote = null;
        }

        Destroy(
            note.gameObject
        );

        resolvedNoteCount++;

        CheckGameEnd();
    }

    // =========================================
    // クリック
    // =========================================

    private void CheckMouseClick()
    {
        if (Mouse.current == null)
        {
            return;
        }

        if (
            Mouse.current.leftButton
                .wasPressedThisFrame
        )
        {
            ProcessClick(
                "Left",
                leftHitArea
            );

            return;
        }

        if (
            Mouse.current.rightButton
                .wasPressedThisFrame
        )
        {
            ProcessClick(
                "Right",
                rightHitArea
            );
        }
    }

    // =========================================
    // クリック判定
    // =========================================

    private void ProcessClick(
        string requiredLane,
        RectTransform selectedHitArea
    )
    {
        if (gameFinished)
        {
            return;
        }

        if (clickCount >= totalNoteCount)
        {
            return;
        }

        clickCount++;

        if (currentNote == null)
        {
            Debug.Log(
                "失敗：ノーツがないタイミングでクリック"
            );

            pendingMissCount++;

            Debug.Log(
                "クリック数：" +
                clickCount +
                " / " +
                totalNoteCount
            );

            return;
        }

        RectTransform note =
            currentNote;

        bool correctLane =
            note.name.Contains(
                "_" + requiredLane
            );

        bool overlapping =
            IsOverlapping(
                note,
                selectedHitArea
            );

        // =====================================
        // 成功
        // =====================================

        if (
            correctLane &&
            overlapping
        )
        {
            Debug.Log(
                "成功：" +
                requiredLane +
                "クリック"
            );

            successCount++;

            PlaySuccessSE();

            // ★成功したときだけパーティクル
            PlaySuccessEffect(
                note
            );

            ResolveClickedNote(
                note
            );
        }

        // =====================================
        // 失敗
        // =====================================

        else
        {
            Debug.Log(
                "失敗：" +
                requiredLane +
                "クリック"
            );

            ResolveClickedNote(
                note
            );
        }

        Debug.Log(
            "クリック数：" +
            clickCount +
            " / " +
            totalNoteCount
        );
    }

    // =========================================
    // ノーツ削除
    // =========================================

    private void ResolveClickedNote(
        RectTransform note
    )
    {
        activeNotes.Remove(
            note
        );

        if (currentNote == note)
        {
            currentNote = null;
        }

        Destroy(
            note.gameObject
        );

        resolvedNoteCount++;

        CheckGameEnd();
    }

    // =========================================
    // 成功SE
    // =========================================

    private void PlaySuccessSE()
    {
        if (audioSource == null)
        {
            Debug.LogWarning(
                "Audio Sourceが設定されていません"
            );

            return;
        }

        if (successSE == null)
        {
            Debug.LogWarning(
                "Success SEが設定されていません"
            );

            return;
        }

        audioSource.PlayOneShot(
            successSE
        );
    }

    // =========================================
    // ★成功パーティクル
    // =========================================

    private void PlaySuccessEffect(
        RectTransform successfulNote
    )
    {
        if (successEffectPrefab == null)
        {
            Debug.LogWarning(
                "Success Effect Prefabが設定されていません"
            );

            return;
        }

        if (successfulNote == null)
        {
            Debug.LogWarning(
                "成功したノーツがありません"
            );

            return;
        }

        // ノーツが消える前に位置を保存
        Vector3 effectPosition =
            successfulNote.position;

        // パーティクルを生成
        GameObject effect =
            Instantiate(
                successEffectPrefab,
                effectPosition,
                Quaternion.identity
            );

        // =====================================
        // ★2つ目のゲームだけ大きくする
        // =====================================

        effect.transform.localScale =
            Vector3.one *
            successEffectScale;

        effect.SetActive(
            true
        );

        // Particle Systemを探す
        ParticleSystem particle =
            effect.GetComponent<
                ParticleSystem
            >();

        if (particle == null)
        {
            particle =
                effect.GetComponentInChildren<
                    ParticleSystem
                >(
                    true
                );
        }

        if (particle == null)
        {
            Debug.LogWarning(
                "Success Effect Prefabの中にParticle Systemがありません"
            );

            Destroy(
                effect
            );

            return;
        }

        // 一度停止してリセット
        particle.Stop(
            true,
            ParticleSystemStopBehavior
                .StopEmittingAndClear
        );

        particle.Clear(
            true
        );

        // 再生
        particle.Play(
            true
        );

        Debug.Log(
            "2つ目：成功パーティクルを再生しました"
        );

        // 3秒後に削除
        Destroy(
            effect,
            3f
        );
    }

    // =========================================
    // 終了確認
    // =========================================

    private void CheckGameEnd()
    {
        if (gameFinished)
        {
            return;
        }

        if (
            resolvedNoteCount <
            totalNoteCount
        )
        {
            return;
        }

        bool finalSuccess =
            successCount ==
            totalNoteCount;

        EndMiniGame(
            finalSuccess
        );
    }

    // =========================================
    // ミニゲーム終了
    // =========================================

    private void EndMiniGame(
        bool success
    )
    {
        if (gameFinished)
        {
            return;
        }

        gameFinished = true;

        StopAllCoroutines();

        ClearAllNotes();

        Debug.Log(
            "2つ目のミニゲーム終了"
        );

        Debug.Log(
            "成功数：" +
            successCount +
            " / " +
            totalNoteCount
        );

        if (testMode)
        {
            isPlaying = false;

            Debug.Log(
                success
                    ? "テスト結果：成功"
                    : "テスト結果：失敗"
            );

            return;
        }

        FinishGame(
            success
        );
    }

    // =========================================
    // 全ノーツ削除
    // =========================================

    private void ClearAllNotes()
    {
        for (
            int i =
                activeNotes.Count - 1;
            i >= 0;
            i--
        )
        {
            RectTransform note =
                activeNotes[i];

            if (note != null)
            {
                Destroy(
                    note.gameObject
                );
            }
        }

        activeNotes.Clear();

        currentNote = null;

        if (noteParent != null)
        {
            for (
                int i =
                    noteParent.childCount - 1;
                i >= 0;
                i--
            )
            {
                Transform child =
                    noteParent.GetChild(
                        i
                    );

                if (
                    child != null &&
                    child.name.StartsWith(
                        "FallingNote_"
                    )
                )
                {
                    Destroy(
                        child.gameObject
                    );
                }
            }
        }
    }

    // =========================================
    // 重なり判定
    // =========================================

    private bool IsOverlapping(
        RectTransform note,
        RectTransform hitArea
    )
    {
        if (
            note == null ||
            hitArea == null
        )
        {
            return false;
        }

        Vector3[] noteCorners =
            new Vector3[4];

        Vector3[] hitAreaCorners =
            new Vector3[4];

        note.GetWorldCorners(
            noteCorners
        );

        hitArea.GetWorldCorners(
            hitAreaCorners
        );

        Rect noteRect =
            new Rect(
                noteCorners[0].x,
                noteCorners[0].y,
                noteCorners[2].x -
                noteCorners[0].x,
                noteCorners[2].y -
                noteCorners[0].y
            );

        Rect hitAreaRect =
            new Rect(
                hitAreaCorners[0].x,
                hitAreaCorners[0].y,
                hitAreaCorners[2].x -
                hitAreaCorners[0].x,
                hitAreaCorners[2].y -
                hitAreaCorners[0].y
            );

        return noteRect.Overlaps(
            hitAreaRect
        );
    }
}