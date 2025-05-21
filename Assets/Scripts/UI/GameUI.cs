using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private Image rightBar;
    [SerializeField] private Image leftBar;

    public void SetLeftBarValue(float value)
    {
        leftBar.fillAmount = Mathf.Clamp01(value);
    }

    public void SetLeftBarActive(bool active)
    {
        transform.Find("LeftProgressBar").gameObject.SetActive(active);
    }

    public void SetRightBarValue(float value)
    {
        rightBar.fillAmount = Mathf.Clamp01(value);
    }

    public void SetRightBarActive(bool active)
    {
        transform.Find("RightProgressBar").gameObject.SetActive(active);
    }
}
