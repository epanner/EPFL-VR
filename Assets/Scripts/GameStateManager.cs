using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

// at the top of GameStateManager.cs
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class GameStateManager : MonoBehaviour
{
    [Header("Locomotion Providers")]
    // Change this:
    // public ActionBasedContinuousMoveProvider continuousMove;
    // to:
    public DynamicMoveProvider continuousMove;

    public UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationProvider teleportProvider;

    
    [Header("Welcome UI")]
    // Drag your “Welcome Screen” root here so we can hide it when the game starts
    public GameObject welcomeCanvas;

    
    [Header("Settings UI")]
    public GameObject settingsCanvas;      // your Settings Screen (World-space Canvas)
    public float      settingsDistance = 2f;

    // Internal state
    bool gameStarted = false;
    bool settingsOpen = false;

    // “Any button” action
    InputAction anyButton;

    void Awake()
    {
        // Build the “any button” InputAction
        anyButton = new InputAction(type: InputActionType.Button);
        anyButton.AddBinding("<XRController>{LeftHand}/trigger");
        anyButton.AddBinding("<XRController>{RightHand}/trigger");
        anyButton.AddBinding("<XRController>{LeftHand}/primaryButton");
        anyButton.AddBinding("<XRController>{LeftHand}/secondaryButton");
        anyButton.AddBinding("<XRController>{RightHand}/primaryButton");
        anyButton.AddBinding("<XRController>{RightHand}/secondaryButton");
        anyButton.AddBinding("<XRController>{LeftHand}/gripButton");
        anyButton.AddBinding("<XRController>{RightHand}/gripButton");
        anyButton.AddBinding("<XRController>/menu");

        // Don’t enable it yet; only after game start
        anyButton.performed += OnAnyButton;
    }

    void OnDestroy()
    {
        anyButton.performed -= OnAnyButton;
        anyButton.Disable();
        anyButton.Dispose();
    }

    /// <summary>
    /// Call this from your Play button’s OnClick once the game actually begins.
    /// </summary>
    public void OnGameStarted()
    {
        if (gameStarted) return;
        gameStarted = true;

        // hide welcome UI
        if (welcomeCanvas != null)
            welcomeCanvas.SetActive(false);

        // hide settings if showing
        settingsCanvas.SetActive(false);

        // Delay the enabling of the listener
        StartCoroutine(EnableAnyButtonNextFrame());
    }
    IEnumerator EnableAnyButtonNextFrame()
    {
        // wait until the end of frame so the Play-button click can't double‐fire
        yield return null;
        anyButton.Enable();
    }

    void OnAnyButton(InputAction.CallbackContext ctx)
    {
        if (!gameStarted) return;   // ignore presses until game has started

        // Toggle settings UI
        settingsOpen = !settingsOpen;
        settingsCanvas.SetActive(settingsOpen);

        // Pause/resume locomotion
        continuousMove.enabled   = !settingsOpen;
        teleportProvider.enabled = !settingsOpen;

        if (settingsOpen)
            PositionSettingsInFront();
    }

    void PositionSettingsInFront()
    {
        var cam = Camera.main.transform;
        settingsCanvas.transform.position = cam.position + cam.forward * settingsDistance;
        settingsCanvas.transform.rotation = Quaternion.LookRotation(
            settingsCanvas.transform.position - cam.position,
            Vector3.up
        );
    }
}