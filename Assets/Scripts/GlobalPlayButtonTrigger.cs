using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GlobalPlayButtonTrigger : MonoBehaviour
{
    [Tooltip("Drag your world-space Play Button here")]
    public Button playButton;

    // One action with multiple bindings for any controller button click
    private InputAction anyClick;

    private void Awake()
    {
        anyClick = new InputAction(
            name: "AnyControllerClick",
            InputActionType.Button,
            // start with just the left‐trigger; we’ll add more below
            binding: "<XRController>{LeftHand}/trigger",
            interactions: "press"
        );

        anyClick.AddBinding("<XRController>{RightHand}/trigger");
        anyClick.AddBinding("<XRController>{LeftHand}/primaryButton");
        anyClick.AddBinding("<XRController>{LeftHand}/secondaryButton");
        anyClick.AddBinding("<XRController>{RightHand}/primaryButton");
        anyClick.AddBinding("<XRController>{RightHand}/secondaryButton");

        anyClick.performed += OnAnyClick;
        anyClick.Enable();
    }

    private void OnAnyClick(InputAction.CallbackContext ctx)
    {
        // Fire the Play button if it's set up
        if (playButton != null && playButton.interactable)
            playButton.onClick.Invoke();

        // Now disable so it only ever fires once
        anyClick.performed -= OnAnyClick;
        anyClick.Disable();

        // Optional: destroy this component if you no longer need it
        Destroy(this);
    }

    private void OnDestroy()
    {
        // Clean up in case the object is destroyed before click
        anyClick.performed -= OnAnyClick;
        anyClick.Disable();
        anyClick.Dispose();
    }
}