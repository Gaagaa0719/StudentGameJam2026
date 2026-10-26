using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Sakemottekoi.MainGame
{
    public class TableBell : MonoBehaviour, IPointerClickHandler
    {
        public static event Action OnClicked;

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
            OnClicked?.Invoke();
            SESource.PlayOneShot(bellSound);
            animator.SetTrigger("push");
        }
    }
}