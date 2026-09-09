using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Sakemottekoi.MainGame
{
    public class ItemDraggable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private Vector3 defaultPos = Vector3.zero;
        private Transform defaultParent;
        private CanvasGroup group;
        private ItemMenu menu;
        private Transform dragOverlay;
        private AudioSource SESource;

        [SerializeField]
        private AudioClip dropIntoItemMenuSound;

        [SerializeField]
        private AudioClip dropIntoTrashSound;

        private void Start()
        {
            menu = ItemMenu.instance;
            group = GetComponent<CanvasGroup>();
            dragOverlay = GameObject.Find("DragOverlay").transform;

            SESource = GameManager.GetSESource();
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
            DropTo2D(eventData);
            group.blocksRaycasts = true;
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

            ResetPos();
        }

        private bool DropToItemMenu(GameObject hitObject)
        {
            if (ItemSelectionPhaseManager.SelectableItemCount <= ItemSelectionPhaseManager.SelectedItemCount) return false;
            if (menu.Contains(gameObject)) return false;
            if (!hitObject.CompareTag("ItemMenu")) return false;
            if (!menu.Add(gameObject)) return false;

            ItemSelectionPhaseManager.AddSelectedItemCount();
            if(dropIntoItemMenuSound) SESource.PlayOneShot(dropIntoItemMenuSound);

            ItemOptions.Instance.RestockItems();
            return true;
        }

        private bool DropToTrash(GameObject hitObject)
        {
            if (!hitObject.CompareTag("Trash")) return false;

            if (dropIntoTrashSound) SESource.PlayOneShot(dropIntoTrashSound);
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
}