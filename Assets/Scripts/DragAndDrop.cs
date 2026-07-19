using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DragAndDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    Camera mainCamera;
    Vector3 defaultPos = Vector3.zero;
    Transform defaultParent;
    CanvasGroup group;

    private void Start()
    {
        mainCamera = Camera.main;
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

        // 1. マウスのスクリーン座標から、3D空間に向かうレイ（光線）を生成
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        // 2. レイを飛ばして、オブジェクトのコライダーに衝突するか判定（距離は無限大: Mathf.Infinity）
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            GameObject hitObject = hit.collider.gameObject;
            if (hitObject.tag != "PlayerGlass")
            {
                ResetPos();
                return;
            }

            Glass glass = hitObject.GetComponent<Glass>();
            if (glass == null) {
                ResetPos();
                return;
            }

            glass.items.Enqueue(gameObject);
            transform.parent = null;
            transform.position = new Vector3(0, 0, 1000);
        }
        else
        {
            Debug.Log("何も検出されませんでした。");
            ResetPos();
        }
    }

    void ResetPos()
    {
        transform.parent = defaultParent;
        transform.position = defaultPos; ;
    }
}