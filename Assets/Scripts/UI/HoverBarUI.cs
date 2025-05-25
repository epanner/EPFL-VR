using UnityEngine;
using UnityEngine.UI;

public class HoverBarUI : MonoBehaviour
{
    [SerializeField] private Image hoverFill;

    public void SetHoverBarValue(float value)
    {
        hoverFill.fillAmount = Mathf.Clamp01(value);
    }
}
