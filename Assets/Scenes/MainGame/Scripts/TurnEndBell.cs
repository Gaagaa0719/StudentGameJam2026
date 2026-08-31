using UnityEngine;
using UnityEngine.InputSystem;

namespace Sakemottekoi.Maingame
{
    public class TurnEndBell : MonoBehaviour
    {
        GameManager manager;
        Animator animator;

        [SerializeField]
        private AudioSource SESource;

        [SerializeField]
        private AudioClip bellSound;

        private void Start()
        {
            manager = GameManager.Instance;
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

                if (!Physics.Raycast(ray, out RaycastHit hit)) return;
                if (hit.collider.gameObject != gameObject) return;

                SESource.PlayOneShot(bellSound);
                //manager.SetWainting(false);
                animator.SetTrigger("push");
            }
        }
    }
}