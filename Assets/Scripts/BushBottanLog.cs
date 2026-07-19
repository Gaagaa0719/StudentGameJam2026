using UnityEngine;
using UnityEngine.InputSystem;

public class MyGlassClick : MonoBehaviour
{
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    Debug.Log("ターンをスキップしますか？");
                }
            }
        }
    }
}