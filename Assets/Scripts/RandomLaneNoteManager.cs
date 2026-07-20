using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RandomLaneNoteManager : MonoBehaviour
{
    [Header("上に置いてある元のノーツ")]
    public RectTransform noteTemplate;

    [Header("左側の灰色の成功判定")]
    public RectTransform leftHitArea;

    [Header("右側の灰色の成功判定")]
    public RectTransform rightHitArea;

    [Header("成功時の効果音")]
    public AudioSource audioSource;
    public AudioClip successSE;

    [Header("ノーツ設定")]
    public int totalNoteCount = 6;
    public float spawnInterval = 1f;
    public float fallSpeed = 200f;

    [Header("判定を通過した後に消える距離")]
    public float missDistance = 300f;

    private RectTransform noteParent;

    private List<RectTransform> activeNotes =
        new List<RectTransform>();

    private int clickCount = 0;
    private int spawnedNoteCount = 0;

    // ノーツがない状態でミスクリックした回数
    private int pendingMissCount = 0;

    private bool gameFinished = false;

    void Start()
    {
        Debug.Log("ノーツゲーム開始");

        totalNoteCount = 6;

        if (noteTemplate == null)
        {
            Debug.LogError(
                "Note Templateにノーツを入れてください"
            );

            enabled = false;
            return;
        }

        if (leftHitArea == null)
        {
            Debug.LogError(
                "Left Hit Areaに左成功判定を入れてください"
            );

            enabled = false;
            return;
        }

        if (rightHitArea == null)
        {
            Debug.LogError(
                "Right Hit Areaに右成功判定を入れてください"
            );

            enabled = false;
            return;
        }

        noteParent =
            noteTemplate.parent as RectTransform;

        if (noteParent == null)
        {
            Debug.LogError(
                "ノーツの親にRectTransformがありません"
            );

            enabled = false;
            return;
        }

        // 元のノーツは見本なので非表示
        noteTemplate.gameObject.SetActive(false);

        StartCoroutine(SpawnNotes());
    }

    void Update()
    {
        if (gameFinished)
        {
            return;
        }

        MoveNotes();
        CheckMouseClick();
    }

    IEnumerator SpawnNotes()
    {
        for (int i = 0; i < totalNoteCount; i++)
        {
            if (gameFinished)
            {
                yield break;
            }

            CreateNote(i + 1);

            yield return new WaitForSeconds(
                spawnInterval
            );
        }

        Debug.Log(
            "6個すべてのノーツを生成しました"
        );
    }

    void CreateNote(int noteNumber)
    {
        RectTransform newNote =
            Instantiate(
                noteTemplate,
                noteParent
            );

        newNote.gameObject.SetActive(true);

        bool isLeftLane =
            Random.Range(0, 2) == 0;

        RectTransform selectedHitArea;

        if (isLeftLane)
        {
            selectedHitArea = leftHitArea;

            newNote.name =
                "FallingNote_" +
                noteNumber +
                "_Left";
        }
        else
        {
            selectedHitArea = rightHitArea;

            newNote.name =
                "FallingNote_" +
                noteNumber +
                "_Right";
        }

        Vector3 hitAreaLocalPosition =
            noteParent.InverseTransformPoint(
                selectedHitArea.position
            );

        Vector3 startLocalPosition =
            noteTemplate.localPosition;

        newNote.localPosition =
            new Vector3(
                hitAreaLocalPosition.x,
                startLocalPosition.y,
                startLocalPosition.z
            );

        activeNotes.Add(newNote);

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
                "個目：左側にノーツ生成"
            );
        }
        else
        {
            Debug.Log(
                noteNumber +
                "個目：右側にノーツ生成"
            );
        }

        // ノーツがないときにミスクリックしていた場合
        if (pendingMissCount > 0)
        {
            pendingMissCount--;

            Debug.Log(
                "先ほどのミスクリックにより、このノーツを消しました"
            );

            activeNotes.Remove(newNote);

            Destroy(
                newNote.gameObject
            );
        }
    }

    void MoveNotes()
    {
        for (
            int i = activeNotes.Count - 1;
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

            note.localPosition +=
                Vector3.down *
                fallSpeed *
                Time.deltaTime;

            float hitAreaY =
                Mathf.Min(
                    leftHitArea.position.y,
                    rightHitArea.position.y
                );

            if (
                note.position.y <
                hitAreaY - missDistance
            )
            {
                Debug.Log(
                    "失敗：ノーツを逃しました"
                );

                activeNotes.RemoveAt(i);

                Destroy(
                    note.gameObject
                );
            }
        }
    }

    void CheckMouseClick()
    {
        if (Mouse.current == null)
        {
            return;
        }

        if (
            clickCount >= totalNoteCount
        )
        {
            return;
        }

        if (
            Mouse.current.leftButton
                .wasPressedThisFrame
        )
        {
            CheckLaneClick(
                leftHitArea,
                "Left"
            );
        }
        else if (
            Mouse.current.rightButton
                .wasPressedThisFrame
        )
        {
            CheckLaneClick(
                rightHitArea,
                "Right"
            );
        }
    }

    void CheckLaneClick(
        RectTransform selectedHitArea,
        string requiredLane
    )
    {
        if (gameFinished)
        {
            return;
        }

        clickCount++;

        RectTransform successfulNote = null;

        // 判定内にある正しいレーンのノーツを探す
        for (
            int i = 0;
            i < activeNotes.Count;
            i++
        )
        {
            RectTransform note =
                activeNotes[i];

            if (note == null)
            {
                continue;
            }

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
                successfulNote = note;
                break;
            }
        }

        if (successfulNote != null)
        {
            if (requiredLane == "Left")
            {
                Debug.Log(
                    "成功：左クリック"
                );
            }
            else
            {
                Debug.Log(
                    "成功：右クリック"
                );
            }

            PlaySuccessSE();

            activeNotes.Remove(
                successfulNote
            );

            Destroy(
                successfulNote.gameObject
            );
        }
        else
        {
            if (requiredLane == "Left")
            {
                Debug.Log(
                    "失敗：左クリック"
                );
            }
            else
            {
                Debug.Log(
                    "失敗：右クリック"
                );
            }

            RemoveNextNote();
        }

        Debug.Log(
            "クリック数：" +
            clickCount +
            " / " +
            totalNoteCount
        );

        if (
            clickCount >= totalNoteCount
        )
        {
            FinishGame();
        }
    }

    void PlaySuccessSE()
    {
        if (
            audioSource == null ||
            successSE == null
        )
        {
            Debug.LogWarning(
                "Audio SourceまたはSuccess SEが設定されていません"
            );

            return;
        }

        audioSource.PlayOneShot(
            successSE
        );
    }

    void RemoveNextNote()
    {
        RectTransform nextNote = null;

        // 一番下にあるノーツを探す
        for (
            int i = 0;
            i < activeNotes.Count;
            i++
        )
        {
            RectTransform note =
                activeNotes[i];

            if (note == null)
            {
                continue;
            }

            if (
                nextNote == null ||
                note.position.y <
                nextNote.position.y
            )
            {
                nextNote = note;
            }
        }

        if (nextNote != null)
        {
            Debug.Log(
                "ミスクリックしたため、次のノーツを消しました"
            );

            activeNotes.Remove(
                nextNote
            );

            Destroy(
                nextNote.gameObject
            );
        }
        else
        {
            // 現在ノーツがない場合、
            // 次に生成されたノーツを消す
            pendingMissCount++;

            Debug.Log(
                "ミスクリックしたため、次に生成されるノーツを消します"
            );
        }
    }

    void FinishGame()
    {
        gameFinished = true;

        StopAllCoroutines();

        Debug.Log(
            "ノーツゲーム終了：6回クリックしました"
        );

        // 残ったノーツをすべて消す
        for (
            int i = activeNotes.Count - 1;
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
    }

    bool IsOverlapping(
        RectTransform note,
        RectTransform hitArea
    )
    {
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