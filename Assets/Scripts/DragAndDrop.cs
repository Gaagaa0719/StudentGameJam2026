using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DragAndDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    Camera mainCamera;
    Vector3 defaultPos = Vector3.zero;
    Transform defaultParent;
    CanvasGroup group;
    GameManager gameManager;

    [SerializeField]
    private ItemMenu menu;

    private void Start()
    {
        mainCamera = Camera.main;
        gameManager = GameManager.GetInstance();
        group = GetComponent<CanvasGroup>();
        defaultPos = transform.position;
        defaultParent = transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        group.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // ドロップ終了時：当たり判定を元に戻す
        group.blocksRaycasts = true;
        switch (gameManager.currentPhase)
        {
            case GamePhase.Prepare:
                DropTo3D();
                break;

            case GamePhase.ItemSelection:
                DropTo3D();
                break;

            default:
                ResetPos();
                break;
        }
    }

    private void DropTo3D()
    {
        // 1. マウスのスクリーン座標から、3D空間に向かうレイ（光線）を生成
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        // 2. レイを飛ばして、オブジェクトのコライダーに衝突するか判定（距離は無限大: Mathf.Infinity）
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.CompareTag("Glass")) DropToGlass(hitObject);
            if (hitObject.CompareTag("Enemy")) DropToEnemy(hitObject);
        }
        else
        {
            Debug.Log("何も検出されませんでした。");
            ResetPos();
        }
    }
    private void DropTo2D(PointerEventData eventData)
    {

    }

    private bool DropToItemMenu(GameObject hitObject)
    {
        if (!hitObject.CompareTag("ItemMenu")) return false;

        menu.Add(gameObject);
        return true;
    }

    private bool DropToTrash (GameObject hitObject)
    {
        if (!hitObject.CompareTag("Trash")) return false;
        Destroy(gameObject);
        return true;
    }

    private bool DropToEnemy (GameObject hitObject)
    {
        Enemy enemy = hitObject.GetComponent<Enemy>();
        if (enemy == null)
        {
            ResetPos();
            return false;
        }

        return true;
    }

    private bool DropToGlass (GameObject hitObject)
    {
        Glass glass = hitObject.GetComponent<Glass>();
        if (glass == null)
        {
            ResetPos();
            return false;
        }

        glass.items.Enqueue(gameObject);
        transform.parent = null;
        transform.position = new Vector3(0, 0, 1000);
        return true;
    }

    private void ResetPos()
    {
        transform.parent = defaultParent;
        transform.position = defaultPos; ;
    }
}