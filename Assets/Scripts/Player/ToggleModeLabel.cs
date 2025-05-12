using UnityEngine;
using UnityEngine.UI;        // for Button
using TMPro;                 // ← add this!

public class ToggleModeLabel : MonoBehaviour
{
    [Header("UI References")]
    public Button           targetButton;
    public TextMeshProUGUI  labelText;     // ← use TextMeshProUGUI

    [Header("Toggle Labels")]
    public string firstLabel  = "Snap";
    public string secondLabel = "Continuous";

    bool showingFirst = true;

    void Awake()
    {
        if (targetButton == null || labelText == null)
        {
            Debug.LogError("Assign the Button and TMP Text in the Inspector.");
            enabled = false;
            return;
        }

        labelText.text = firstLabel;
        targetButton.onClick.AddListener(Toggle);
    }

    void OnDestroy()
    {
        if (targetButton != null)
            targetButton.onClick.RemoveListener(Toggle);
    }

    void Toggle()
    {
        showingFirst = !showingFirst;
        labelText.text = showingFirst ? firstLabel : secondLabel;
    }
}