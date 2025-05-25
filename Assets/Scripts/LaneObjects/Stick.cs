using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Stick : LaneObject
{
    private Transform head;
    private bool held = false;
    private XRBaseInputInteractor interactor; // Store the interactor for haptics

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get the lanes component from the scene
        lanes = FindFirstObjectByType<Lanes>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!held) return;

        // Calculate the angle of the object relative to the camera
        Vector3 direction = head.position - transform.position;

        // Get the up vector of the object
        Vector3 stickUp = transform.up;

        // // Project the stick up vector onto the plane defined by the camera's forward vector
        // Vector3 projectedStickUp = Vector3.ProjectOnPlane(stickUp, head.forward);

        // // Calculate the angle between the projection and global up
        // float angle = Vector3.SignedAngle(Vector3.up, projectedStickUp, head.forward);

        // get angle difference between stick up and world up, in world relation
        float angle = Vector3.SignedAngle(Vector3.up, stickUp, head.forward);

        float percentage = Mathf.Abs(angle) / 90f;

        // Set the speed modifier based on the angle
        lanes.SetSpeedModifier(1 - percentage);

        // Send haptic feedback based on 1 - percentage
        if (interactor != null)
        {
            float intensity = Mathf.Clamp01(1 - percentage);
            float duration = 0.05f; // Short pulse
            interactor.SendHapticImpulse(intensity, duration);
        }
    }

    public void SelectEntered(SelectEnterEventArgs args)
    {
        held = true;
        head = Camera.main.transform;
        interactor = args.interactorObject as XRBaseInputInteractor;
    }

    public void SelectExited(SelectExitEventArgs args)
    {
        held = false;
        lanes.SetSpeedModifier(1);
        interactor = null;
    }

}
