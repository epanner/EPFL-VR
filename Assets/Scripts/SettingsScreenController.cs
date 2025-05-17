using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

/// <summary>
/// Controls the display of the settings screen. The settings panel
/// will only open if the XR Origin GameObject is active in the scene
/// and the user presses any button on a connected XR controller (via Input System).
/// The panel will be positioned in front of the user's view, and
/// game locomotion will pause while the settings panel is active.
/// </summary>
public class SettingsScreenController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The XR Origin GameObject that must be active for settings to show.")]
    public GameObject xrOriginGame;

    [Tooltip("The Settings Panel UI to show/hide. Should be a world-space canvas.")]
    public GameObject settingsPanel;

    [Tooltip("The user's main camera. Defaults to Camera.main if not set.")]
    public Camera userCamera;

    [Header("Buttons")]
    [Tooltip("Button to close the settings screen.")]
    public Button closeButton;

    [Header("Input Settings")]
    [Tooltip("Distance in meters in front of the user where the panel will appear.")]
    public float panelDistance = 1.5f;

    // One action with multiple bindings for any controller button click
    private InputAction anyClick;
    private float previousTimeScale = 1f;

    private void Awake()
    {
        // Ensure settings panel is hidden at start
        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        // Bind close callback
        if (closeButton != null)
            closeButton.onClick.AddListener(CloseSettings);

        // Use main camera if no override
        if (userCamera == null && Camera.main != null)
            userCamera = Camera.main;

        // Setup any-click action
        anyClick = new InputAction(
            name: "AnyControllerClick",
            InputActionType.Button,
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
        // Only open if XR Origin is active and settings are not already visible
        if (xrOriginGame != null && xrOriginGame.activeInHierarchy &&
            settingsPanel != null && !settingsPanel.activeSelf)
        {
            ShowSettings();
        }
    }

    private void ShowSettings()
    {
        // Position panel in front of user
        if (userCamera != null && settingsPanel != null)
        {
            Transform panelT = settingsPanel.transform;
            Vector3 forward = userCamera.transform.forward;
            Vector3 up = userCamera.transform.up;
            Vector3 targetPos = userCamera.transform.position + forward.normalized * panelDistance;

            panelT.position = targetPos;
            // Rotate to face user
            panelT.rotation = Quaternion.LookRotation(forward, up);
        }

        // Pause game locomotion by freezing time
        previousTimeScale = Time.timeScale;
        Time.timeScale = 0f;

        settingsPanel.SetActive(true);
        // Disable further opens until closed
        anyClick.performed -= OnAnyClick;
        anyClick.Disable();
    }

    private void CloseSettings()
    {
        if (settingsPanel != null && settingsPanel.activeSelf)
        {
            // Restore time scale
            Time.timeScale = previousTimeScale;

            settingsPanel.SetActive(false);
            // Re-enable to allow reopening
            anyClick.Enable();
            anyClick.performed += OnAnyClick;
        }
    }

    private void OnDestroy()
    {
        // Clean up input action
        if (anyClick != null)
        {
            anyClick.performed -= OnAnyClick;
            anyClick.Disable();
            anyClick.Dispose();
        }
        // Ensure time scale is restored if destroyed while paused
        Time.timeScale = previousTimeScale;
    }
}
