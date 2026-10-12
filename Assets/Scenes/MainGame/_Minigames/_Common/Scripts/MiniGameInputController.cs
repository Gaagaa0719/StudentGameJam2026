using UnityEngine;
using UnityEngine.InputSystem;

public class MiniGameInputController : MonoBehaviour
{
    public static MiniGameInputController Instance { get; private set; }

    [SerializeField] private InputActionReference primary;
    [SerializeField] private InputActionReference secondary;

    private MiniGame activeMinigame;

    private void Awake()
    {
        Instance = this;
    }

    public void SetActiveMinigame(MiniGame minigame)
    {
        activeMinigame = minigame;
    }

    private void OnEnable()
    {
        primary.action.performed += OnPrimary;
        secondary.action.performed += OnSecondary;

        primary.action.Enable();
        secondary.action.Enable();
    }

    private void OnDisable()
    {
        primary.action.performed -= OnPrimary;
        secondary.action.performed -= OnSecondary;

        primary.action.Disable();
        secondary.action.Disable();
    }

    private void OnPrimary(InputAction.CallbackContext context)
    {
        activeMinigame?.OnPrimary();
    }

    private void OnSecondary(InputAction.CallbackContext context)
    {
        activeMinigame?.OnSecondary();
    }
}