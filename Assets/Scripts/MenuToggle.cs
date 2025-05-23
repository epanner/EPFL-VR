using UnityEngine;
using UnityEngine.InputSystem;

public class MenuToggle : MonoBehaviour
{
    public InputActionReference showMenuAction;

    private void OnEnable()
    {
        showMenuAction.action.performed += OnMenuPressed;
        showMenuAction.action.Enable();
    }

    private void OnDisable()
    {
        showMenuAction.action.performed -= OnMenuPressed;
        showMenuAction.action.Disable();
    }

    private void OnMenuPressed(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.inGame)
        {
            GameManager.Instance.GamePaused();
        }
        
    }
}
