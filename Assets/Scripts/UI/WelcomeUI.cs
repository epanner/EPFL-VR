using TMPro;
using UnityEngine;

public class WelcomeUI : MonoBehaviour
{
    public void SetLastScore(int lastScore)
    {
        transform.Find("ScoresPanel/LastScore").GetComponent<TextMeshProUGUI>().SetText("Last Score: " + lastScore);
    }

    public void SetRecord(int record)
    {
        transform.Find("ScoresPanel/Record").GetComponent<TextMeshProUGUI>().SetText("Record: " + record);
    }
}
