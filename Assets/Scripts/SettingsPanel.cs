using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class SettingsPanel : MonoBehaviour
{
    private bool sounds = true;
    private float speedModifier = 1.0f;
    private bool laneTeleport = false;
    private bool snapRotation = true;

    public Slider speedSlider;
    public TextMeshProUGUI soundButtonText;
    public TextMeshProUGUI teleportButtonText;
    public TextMeshProUGUI snapRotationButtonText;

    public ControllerInputActionManager controller1;
    public ControllerInputActionManager controller2;

    public Lanes lanes;

    public DynamicMoveProvider moveProvider1;
    public DynamicMoveProvider moveProvider2;
    private float defaultSpeed = 2.5f;

    public AudioListener listener1;
    public AudioListener listener2;

    void Start()
    {
        speedSlider.value = speedModifier;
    }

    void Update()
    {

    }

    public void ToggleSounds()
    {
        sounds = !sounds;
        soundButtonText.text = sounds ? "Enabled" : "Disabled";

        listener1.enabled = sounds;
        listener2.enabled = sounds;
    }

    public void SpeedUpdated()
    {
        speedModifier = speedSlider.value;

        moveProvider1.moveSpeed = defaultSpeed * speedModifier;
        moveProvider2.moveSpeed = defaultSpeed * speedModifier;
    }

    public void ToggleSnapRotation()
    {
        snapRotation = !snapRotation;
        snapRotationButtonText.text = snapRotation ? "Snap" : "Continuous";
        if (snapRotation)
        {
            controller1.smoothTurnEnabled = false;
            controller2.smoothTurnEnabled = false;
        }
        else
        {
            controller1.smoothTurnEnabled = true;
            controller2.smoothTurnEnabled = true;
        }
    }

    public void ToggleLaneTeleport()
    {
        laneTeleport = !laneTeleport;
        lanes.EnableTeleportPads(laneTeleport);
        teleportButtonText.text = laneTeleport ? "Enabled" : "Disabled";
    }
}
