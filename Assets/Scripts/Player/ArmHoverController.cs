using UnityEngine;

public class ArmHoverController : MonoBehaviour
{
    public Transform headTransform;
    public Transform leftHandTransform;
    public Transform rightHandTransform;

    public float liftHeight = 1.0f;
    public float liftDuration = 0.5f;
    public float handDistanceThreshold = 0.4f; // Horizontal distance needed (meters)
    public float heightTolerance = 0.2f;      // How strict vertical alignment must be (meters)

    private Vector3 basePosition;
    private float targetYOffset = 0f;

    private void Start()
    {
        basePosition = transform.position;
    }

    private void Update()
    {
        if (ArmsInTPosition())
        {
            targetYOffset = liftHeight;
        }
        else
        {
            targetYOffset = 0f;
        }

        // Smoothly move XR Rig up or down
        Vector3 desiredPosition = basePosition + Vector3.up * targetYOffset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * liftDuration);
    }

    private bool ArmsInTPosition()
    {
        Vector3 headPos = headTransform.position;
        Vector3 leftPos = leftHandTransform.position;
        Vector3 rightPos = rightHandTransform.position;

        // Check vertical alignment
        bool leftHeightOk = Mathf.Abs(leftPos.y - headPos.y) < heightTolerance;
        bool rightHeightOk = Mathf.Abs(rightPos.y - headPos.y) < heightTolerance;

        float leftDist = Mathf.Abs(leftPos.x - headPos.x);
        float rightDist = Mathf.Abs(rightPos.x - headPos.x);

        // Check horizontal spread
        bool leftFarEnough = leftDist > handDistanceThreshold;
        bool rightFarEnough = rightDist > handDistanceThreshold;

        // Optional: Make sure hands aren't too far forward/backward
        bool leftDepthOk = Mathf.Abs(leftPos.z - headPos.z) < handDistanceThreshold;
        bool rightDepthOk = Mathf.Abs(rightPos.z - headPos.z) < handDistanceThreshold;

        return leftHeightOk && rightHeightOk && leftFarEnough && rightFarEnough && leftDepthOk && rightDepthOk;
    }
}
