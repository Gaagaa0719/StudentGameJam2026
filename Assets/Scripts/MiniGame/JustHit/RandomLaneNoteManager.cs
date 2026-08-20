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

    // 現在の落下速度
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
    // MiniGame開始
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

        // =====================================
        // 酔い度から落下速度を計算
        // =====================================

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

        noteParent =
            noteTemplate.parent as RectTransform;

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
    // 酔い度による速度計算
    // =========================================

    private void CalculateFallSpeed(
        float dp
    )
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

        // 速くなりすぎないように制限
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

    private void FindObjects()
    {
        if (noteTemplate == null)
        {
            GameObject templateObject =
                GameObject.Find(
                    "Note Template"
                );

            if (templateObject != null)
            {
                noteTemplate =
                    templateObject.GetComponent<
                        RectTransform
                    >();
            }
        }

        if (leftHitArea == null)
        {
            GameObject leftObject =
                GameObject.Find(
                    "Left Hit Area"
                );

            if (leftObject != null)
            {
                leftHitArea =
                    leftObject.GetComponent<
                        RectTransform
                    >();
            }
        }

        if (rightHitArea == null)
        {
            GameObject rightObject =
                GameObject.Find(
                    "Right Hit Area"
                );

            if (rightObject != null)
            {
                rightHitArea =
                    rightObject.GetComponent<
                        RectTransform
                    >();
            }
        }
    }

    private IEnumerator SpawnNotes()
    {
        for (
            int noteNumber = 1;
            noteNumber <= totalNoteCount;
            noteNumber++
        )
        {
            if (
                !isPlaying ||
                gameFinished
            )
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

    private void CreateNote(
        int noteNumber
    )
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
                activeNotes.RemoveAt(i);
                continue;
            }

            // ★酔い度から計算した速度を使用
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

    private void ProcessClick(
        string requiredLane,
        RectTransform selectedHitArea
    )
    {
        if (gameFinished)
        {
            return;
        }

        if (
            clickCount >=
            totalNoteCount
        )
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

            PlaySuccessEffect(
                note
            );

            ResolveClickedNote(
                note
            );
        }
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
    }

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

    private void PlaySuccessSE()
    {
        if (
            audioSource == null ||
            successSE == null
        )
        {
            return;
        }

        audioSource.PlayOneShot(
            successSE
        );
    }

    private void PlaySuccessEffect(
        RectTransform successfulNote
    )
    {
        if (
            successEffectPrefab == null ||
            successfulNote == null
        )
        {
            return;
        }

        Vector3 effectPosition =
            successfulNote.position;

        GameObject effect =
            Instantiate(
                successEffectPrefab,
                effectPosition,
                Quaternion.identity
            );

        ParticleSystem particle =
            effect.GetComponent<
                ParticleSystem
            >();

        if (particle == null)
        {
            particle =
                effect.GetComponentInChildren<
                    ParticleSystem
                >();
        }

        if (particle != null)
        {
            particle.Clear(true);
            particle.Play(true);
        }

        Destroy(
            effect,
            3f
        );
    }

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

    private void EndMiniGame(
        bool success
    )
    {
        if (gameFinished)
        {
            return;
        }

        gameFinished =
            true;

        StopAllCoroutines();

        ClearAllNotes();

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
                    noteParent.GetChild(i);

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