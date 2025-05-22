using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private Image rightBar;
    [SerializeField] private Image leftBar;

    public void InitUI(int health, int score)
    {
        SetHealth(health);
        SetScore(score);
    }

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

    public void SetHealth(int health)
    {
        transform.Find("LifeCount").GetComponent<TextMeshProUGUI>().SetText("Life: " + health);
    }
    public void SetScore(int score)
    {
        transform.Find("Score").GetComponent<TextMeshProUGUI>().SetText("Score: " + score);
    }
}
