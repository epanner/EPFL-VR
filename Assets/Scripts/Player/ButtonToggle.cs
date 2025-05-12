using UnityEngine;
using UnityEngine.UI;

public class ButtonToggle : MonoBehaviour
{
    private bool isOn = false;
    public Text TextMeshProUGUI; // or use TextMeshProUGUI if you're using TextMeshPro

    public void ToggleState()
    {
        isOn = !isOn;

        if (isOn)
        {
            TextMeshProUGUI.text = "ON";
            // Optional: Change color or other visuals
        }
        else
        {
            TextMeshProUGUI.text = "OFF";
        }
    }
}