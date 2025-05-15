using UnityEngine;

public class ArmHoverController : MonoBehaviour
{
    public Transform headTransform;
    public Transform leftHandTransform;
    public Transform rightHandTransform;

    public float liftHeight = 1.0f;
    public float liftDuration = 1.0f;
    public float handDistanceThreshold = 0.3f; // Horizontal distance needed (meters)
    public float minHandsHeight = -0.2f;      // How above the head the hands should be (meters) (negative means below)

    private float baseYLevel;
    private float targetYOffset = 0f;

    // Hover energy bar
    [SerializeField] private HoverBarUI hoverBarUI;

    public float hoverCharge = 0f; // Beginning hover charge
    public float drainRate = 0.1f;
    public float rechargeRate = 0.01f;
    private bool needsRefill = true;

    private void Start()
    {
        baseYLevel = transform.position.y;
        hoverBarUI.SetHoverBarValue(hoverCharge);
    }

    private void Update()
    {
        if (needsRefill)
        {
            targetYOffset = 0f;
            hoverCharge += rechargeRate * Time.deltaTime;
            if (hoverCharge >= 1.0f)
            {
                needsRefill = false;
            }
        }
        else if (ArmsInTPosition())
        {
            targetYOffset = liftHeight;
            hoverCharge -= drainRate * Time.deltaTime;
            if (hoverCharge <= 0f)
            {
                needsRefill = true;
            }
        }
        else
        {
            if (hoverCharge < 0.5f)
            {
                needsRefill = true;
            }
            targetYOffset = 0f;
            hoverCharge += rechargeRate * Time.deltaTime;
        }

        // Smoothly move XR Rig up or down
        Vector3 desiredPosition = transform.position;
        desiredPosition.y = baseYLevel + targetYOffset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * liftDuration);

        // Update HoverEnergy Bar
        hoverCharge = Mathf.Clamp01(hoverCharge);
        hoverBarUI.SetHoverBarValue(hoverCharge);
    }

    private bool ArmsInTPosition()
    {
        Vector3 headPos = headTransform.position;
        Vector3 leftPos = leftHandTransform.position;
        Vector3 rightPos = rightHandTransform.position;

        // Check vertical alignment
        bool leftHeightOk = leftPos.y - headPos.y > minHandsHeight;
        bool rightHeightOk = rightPos.y - headPos.y > minHandsHeight;

        float leftDist = Mathf.Abs(leftPos.x - headPos.x);
        float rightDist = Mathf.Abs(rightPos.x - headPos.x);

        // Check horizontal spread
        bool leftFarEnough = leftDist > handDistanceThreshold;
        bool rightFarEnough = rightDist > handDistanceThreshold;

        return leftHeightOk && rightHeightOk && leftFarEnough && rightFarEnough;
    }
}
