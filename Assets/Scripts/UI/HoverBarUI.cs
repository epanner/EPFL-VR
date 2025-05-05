using UnityEngine;
using UnityEngine.UI;

public class HoverBarUI : MonoBehaviour
{
    [SerializeField] private Slider hoverBar;

    // Call this every frame from ArmHoverController
    public void SetHoverBarValue(float value)
    {
        hoverBar.value = Mathf.Clamp01(value);
    }
}
