using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Sakemottekoi.MainGame
{
    public class DroppedEvent
    {
        public DroppedEvent(GameObject dropped, GameObject droppedOn)
        {
            Dropped = dropped;
            DroppedOn = droppedOn;
            IsOwnerMoved = false;
        }

        /// <summary>
        /// ドロップされたオブジェクト
        /// </summary>
        public GameObject Dropped { private set; get; }

        /// <summary>
        /// ドロップした先のUIオブジェクト
        /// </summary>
        public GameObject DroppedOn { private set; get; }

        /// <summary>
        /// 所有権が移ったかのフラグ
        /// </summary>
        public bool IsOwnerMoved { set; get; } 
    }

    [RequireComponent(typeof(CanvasGroup))]
    public class ItemDraggable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public static event Action<DroppedEvent> OnDroppedUI;

        // リセット処理用
        private Vector3 defaultPos = Vector3.zero;
        private Transform defaultParent;

        private CanvasGroup group;
        private Transform dragOverlay;
        private bool allowDrag = false; // ドラッグが許可されているかを保存しておく

        [Header("ドラッグ可能なフェーズ")]
        [SerializeField]
        private List<GamePhase> draggablePhases = new();

        private void Start()
        {
            group = GetComponent<CanvasGroup>();
            dragOverlay = GameObject.Find("DragOverlay").transform;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            // 今のフェーズ中にドラッグが許可されているかを確認する。
            allowDrag = draggablePhases.FindIndex(v => v == GameManager.Instance.CurrentPhase) != -1;
            if (!allowDrag) return;

            group.blocksRaycasts = false;
            defaultPos = transform.position;
            defaultParent = transform.parent;
            transform.SetParent(dragOverlay, true);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!allowDrag) return;
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!allowDrag) return;
            DropToUI(eventData);
            group.blocksRaycasts = true;
        }

        private void DropToUI(PointerEventData eventData)
        {
            List<RaycastResult> results = new();
            EventSystem.current.RaycastAll(eventData, results);

            if (results.Count == 0)
            {
                ResetPos();
                return;
            }
            GameObject droppedOn = results[0].gameObject;

            DroppedEvent dropEvent = new DroppedEvent(gameObject, droppedOn);
            OnDroppedUI?.Invoke(dropEvent);
            if(!dropEvent.IsOwnerMoved) ResetPos(); // 所有権が誰にも移らなかったら元の位置に戻す。
        }

        private void ResetPos()
        {
            transform.SetParent(defaultParent, true);
            transform.position = defaultPos;
        }
    }
}