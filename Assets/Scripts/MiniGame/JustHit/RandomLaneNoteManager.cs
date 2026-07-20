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

    [Header("ノーツ設定")]
    public int totalNoteCount = 5;
    public float spawnInterval = 1f;
    public float fallSpeed = 200f;

    [Header("判定を通過した後に消える距離")]
    public float missDistance = 300f;

    private RectTransform noteParent;

    private List<RectTransform> activeNotes =
        new List<RectTransform>();

    void Start()
    {
        Debug.Log("ノーツゲーム開始");

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

        StartCoroutine(SpawnNotes());
    }

    void Update()
    {
        MoveNotes();
        CheckMouseClick();
    }

    IEnumerator SpawnNotes()
    {
        for (int i = 0; i < totalNoteCount; i++)
        {
            CreateNote(i + 1);

            yield return new WaitForSeconds(
                spawnInterval
            );
        }
    }

    void CreateNote(int noteNumber)
    {
        RectTransform newNote =
            Instantiate(
                noteTemplate,
                noteParent
            );

        newNote.gameObject.SetActive(true);

        newNote.name =
            "FallingNote_" + noteNumber;

        bool isLeftLane =
            Random.Range(0, 2) == 0;

        RectTransform selectedHitArea;

        if (isLeftLane)
        {
            selectedHitArea = leftHitArea;
            newNote.name += "_Left";
        }
        else
        {
            selectedHitArea = rightHitArea;
            newNote.name += "_Right";
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
                Destroy(note.gameObject);
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
            Mouse.current
                .leftButton
                .wasPressedThisFrame
        )
        {
            CheckLaneClick(
                leftHitArea,
                "Left"
            );
        }

        if (
            Mouse.current
                .rightButton
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
        string requiredLane)
    {
        RectTransform successfulNote = null;

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
                    requiredLane
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
        }
    }

    bool IsOverlapping(
        RectTransform note,
        RectTransform hitArea)
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