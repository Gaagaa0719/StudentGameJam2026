using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Sakemottekoi.MainGame
{
    public class TableBell : MonoBehaviour, IPointerClickHandler
    {
        static event Action OnClick;

        Animator animator;

        [SerializeField]
        private AudioSource SESource;

        [SerializeField]
        private AudioClip bellSound;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke();
            SESource.PlayOneShot(bellSound);
            animator.SetTrigger("push");
        }
    }
}