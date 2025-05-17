using UnityEngine;
using UnityEngine.InputSystem;

public class AnyButtonShowSettings : MonoBehaviour
{
    [Header("Assign your world-space Settings Canvas here")]
    public GameObject settingsCanvas;  
    [Header("How far in front of the HMD to place it (meters)")]
    public float      distanceFromHead = 2f;

    // We'll build one InputAction with multiple bindings:
    private InputAction anyButton;

    void Awake()
    {
        // Create an action that fires on first press of any of these:
        anyButton = new InputAction(type: InputActionType.Button);
        anyButton.AddBinding("<XRController>{LeftHand}/trigger");
        anyButton.AddBinding("<XRController>{RightHand}/trigger");
        anyButton.AddBinding("<XRController>{LeftHand}/gripButton");
        anyButton.AddBinding("<XRController>{RightHand}/gripButton");
        anyButton.AddBinding("<XRController>{LeftHand}/primaryButton");
        anyButton.AddBinding("<XRController>{RightHand}/primaryButton");
        anyButton.AddBinding("<XRController>{LeftHand}/secondaryButton");
        anyButton.AddBinding("<XRController>{RightHand}/secondaryButton");
        anyButton.AddBinding("<XRController>/menu");  // universal menu

        anyButton.performed += OnAnyButton;
        anyButton.Enable();

        // Hide it initially
        if (settingsCanvas != null)
            settingsCanvas.SetActive(false);
    }

    private void OnAnyButton(InputAction.CallbackContext ctx)
    {
        // Show the canvas
        if (settingsCanvas != null)
        {
            PositionInFront();
            settingsCanvas.SetActive(true);
        }

        // Stop listening so it only ever shows once
        anyButton.performed -= OnAnyButton;
        anyButton.Disable();
    }

    private void PositionInFront()
    {
        var cam = Camera.main.transform;
        settingsCanvas.transform.position = cam.position + cam.forward * distanceFromHead;
        settingsCanvas.transform.rotation = Quaternion.LookRotation(
            settingsCanvas.transform.position - cam.position,
            Vector3.up
        );
    }

    private void OnDestroy()
    {
        anyButton.Disable();
        anyButton.Dispose();
    }
}