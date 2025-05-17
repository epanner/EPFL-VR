using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.InputSystem;  // for InputActionReference

[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor))]
public class UIRaycastClicker : MonoBehaviour
{
    [Tooltip("The Input Action Reference for your UI.Click (press) action")]
    public InputActionReference uiClickAction;

    UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor _ray;
    GameObject      _currentHovered;
    PointerEventData _pointerData;

    void Awake()
    {
        _ray = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor>();
        _pointerData = new PointerEventData(EventSystem.current);
    }

    void OnEnable()
    {
        uiClickAction.action.performed += OnClickPerformed;
        uiClickAction.action.Enable();
    }

    void OnDisable()
    {
        uiClickAction.action.performed -= OnClickPerformed;
        uiClickAction.action.Disable();
    }

    void Update()
    {
        // each frame, ask XRRayInteractor for its UI hit
        if (_ray.TryGetCurrentUIRaycastResult(out var uiResult))
        {
            // highlight / hover transition
            if (uiResult.gameObject != _currentHovered)
            {
                if (_currentHovered != null)
                    ExecuteEvents.Execute(_currentHovered, _pointerData, ExecuteEvents.pointerExitHandler);

                _currentHovered = uiResult.gameObject;
                ExecuteEvents.Execute(_currentHovered, _pointerData, ExecuteEvents.pointerEnterHandler);
            }
        }
        else if (_currentHovered != null)
        {
            ExecuteEvents.Execute(_currentHovered, _pointerData, ExecuteEvents.pointerExitHandler);
            _currentHovered = null;
        }
    }

    void OnClickPerformed(InputAction.CallbackContext _)
    {
        if (_currentHovered != null)
        {
            // send pointer click
            ExecuteEvents.Execute(_currentHovered, _pointerData, ExecuteEvents.pointerClickHandler);
        }
    }
}