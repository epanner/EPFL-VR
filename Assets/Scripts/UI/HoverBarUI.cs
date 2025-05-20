using UnityEngine;
using UnityEngine.UI;

public class HoverBarUI : MonoBehaviour
{
    [SerializeField] private Image hoverFill;

    // Call this every frame from ArmHoverController
    public void SetHoverBarValue(float value)
    {
        hoverFill.fillAmount = Mathf.Clamp01(value);
    }
}
