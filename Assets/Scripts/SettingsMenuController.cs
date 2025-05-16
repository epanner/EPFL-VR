using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class SettingsMenuController : MonoBehaviour
{
    [Header("Input")]
    // Drag in an InputActionReference bound to your menu/app button
    public InputActionReference menuAction;

    [Header("Locomotion Providers")]
    public ActionBasedContinuousMoveProvider continuousMove;
    public UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationProvider           teleportation;

    [Header("Settings UI")]
    public GameObject settingsCanvas; // your Settings Screen Canvas GameObject
    public float      distanceFromUser = 2f; 

    bool isOpen = false;

    void OnEnable()
    {
        menuAction.action.performed += OnMenuPressed;
        menuAction.action.Enable();
    }

    void OnDisable()
    {
        menuAction.action.performed -= OnMenuPressed;
        menuAction.action.Disable();
    }

    private void OnMenuPressed(InputAction.CallbackContext ctx)
    {
        ToggleSettings();
    }

    void ToggleSettings()
    {
        isOpen = !isOpen;

        // 1) Stop locomotion
        continuousMove.enabled    = !isOpen;
        teleportation.enabled     = !isOpen;

        // 2) Show/hide the Settings Canvas
        settingsCanvas.SetActive(isOpen);

        if (isOpen)
            PositionCanvasInFrontOfUser();
    }

    void PositionCanvasInFrontOfUser()
    {
        var cam = Camera.main.transform;
        var targetPos = cam.position + cam.forward * distanceFromUser;
        settingsCanvas.transform.position = targetPos;

        // Make it face the user
        var lookDir = settingsCanvas.transform.position - cam.position;
        settingsCanvas.transform.rotation = Quaternion.LookRotation(lookDir, Vector3.up);
    }
}