using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Stick : TempLaneObject
{
    private Transform head;
    private XRBaseInputInteractor interactor;
    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get the lanes component from the scene
        lanes = FindFirstObjectByType<Lanes>();

        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        switch (GameManager.Instance.level)
        {
            case 1:
                selfDestructionDelay = 15f;
                break;
            case 2:
                selfDestructionDelay = 10f;
                break;
            case 3:
                selfDestructionDelay = 5f;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.inGame) return;

        if (isGrabbed)
        {
            grabbingTime += Time.deltaTime;
        }
        if (grabbingTime > selfDestructionDelay)
        {
            Destroy(gameObject);
        }

        if (!isGrabbed) return;

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
        isGrabbed = true;
        head = Camera.main.transform;
        interactor = args.interactorObject as XRBaseInputInteractor;

        inLane = false;
        rb.isKinematic = false;
    }

    public void SelectExited(SelectExitEventArgs args)
    {
        isGrabbed = false;
        lanes.SetSpeedModifier(1);
        interactor = null;

        rb.isKinematic = true;
    }

}
