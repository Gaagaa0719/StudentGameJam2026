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
    ItemMenu menu;
    Transform dragOverlay;

    private void Start()
    {
        menu = ItemMenu.instance;
        mainCamera = Camera.main;
        gameManager = GameManager.GetInstance();
        group = GetComponent<CanvasGroup>();
        dragOverlay = GameObject.Find("DragOverlay").transform;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        group.blocksRaycasts = false;
        defaultPos = transform.position;
        defaultParent = transform.parent;
        transform.SetParent(dragOverlay, true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        switch (gameManager.currentPhase)
        {
            case GamePhase.Prepare:
                DropTo3D();
                break;

            case GamePhase.ItemSelection:
                DropTo2D(eventData);
                break;

            default:
                ResetPos();
                break;
        }

        group.blocksRaycasts = true;
    }

    private void DropTo3D()
    {
        // 1. マウスのスクリーン座標から、3D空間に向かうレイ（光線）を生成
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        // 2. レイを飛ばして、オブジェクトのコライダーに衝突するか判定（距離は無限大: Mathf.Infinity）
        if (!Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            ResetPos();
            return;
        }

        GameObject hitObject = hit.collider.gameObject;

        if (DropToGlass(hitObject)) return;
        ResetPos();
    }

    private bool DropToGlass (GameObject hitObject)
    {
        if(!hitObject.CompareTag("PlayerGlass")) return false;
        if(!menu.Contains(gameObject)) return false;

        Glass glass = hitObject.GetComponent<Glass>();
        if (glass == null) return false;

        menu.Remove(gameObject);

        glass.items.Enqueue(gameObject);
        transform.SetParent(null);
        transform.position = new Vector3(0, 0, 1000);
        return true;
    }

    private void DropTo2D(PointerEventData eventData)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        if (results.Count == 0)
        {
            Debug.Log("ドロップした位置にUIはありませんでした。");
            ResetPos();
            return;
        }
        GameObject droppedOn = results[0].gameObject;

        if (DropToItemMenu(droppedOn)) return;
        if (DropToTrash(droppedOn)) return;
        if (DropToItemOption(droppedOn)) return;

        ResetPos();
    }

    private bool DropToItemOption(GameObject hitObject)
    {
        if (!menu.Contains(gameObject)) return false;
        if (hitObject.transform.childCount > 2) return false;
        if (!hitObject.CompareTag("ItemOption")) return false;

        menu.Remove(gameObject);
        ItemOptions.addedItemCount--;
        gameObject.transform.SetParent(hitObject.transform, false);
        gameObject.transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
        return true;
    }

    private bool DropToItemMenu(GameObject hitObject)
    {
        if (ItemOptions.selectableItemCount <= ItemOptions.addedItemCount) return false;
        if (menu.Contains(gameObject)) return false;
        if (!hitObject.CompareTag("ItemMenu")) return false;
        if (!menu.Add(gameObject)) return false;

        ItemOptions.addedItemCount++;
        return true;
    }

    private bool DropToTrash(GameObject hitObject)
    {
        if (!hitObject.CompareTag("Trash")) return false;
        Destroy(gameObject);
        return true;
    }

    private void ResetPos()
    {
        transform.SetParent(defaultParent, false);
        transform.position = defaultPos;
        transform.localScale = Vector3.one;
    }
}