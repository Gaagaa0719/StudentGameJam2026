using UnityEngine;
using UnityEngine.InputSystem;

public class TurnEndBell : MonoBehaviour
{
    GameManager manager;
    Animator animator;

    [SerializeField]
    private AudioClip bellSound;

    private void Start()
    {
        manager = GameManager.GetInstance();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (!Physics.Raycast(ray, out RaycastHit hit)) return;
            if (hit.collider.gameObject != gameObject) return;

            manager.SetWainting(false);
            animator.SetTrigger("push");
        }
    }
}