using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton reference

    public GameObject gameOverUI; // Drag your Game Over UI here
    public GameObject lanes;

    private void Awake()
    {
        Instance = this;
    }

    public void GameOver()
    {
        Debug.Log("Game Over!");
        Time.timeScale = 0f; // Freeze time
        if (gameOverUI != null)
            gameOverUI.SetActive(true); // Show Game Over UI
    }

    public void ReRunGame()
    {
        lanes.GetComponent<Lanes>().EndGame();
        lanes.GetComponent<Lanes>().ReInitGame();
        if (gameOverUI != null)
            gameOverUI.SetActive(false); // Hide Game Over UI
        Time.timeScale = 1.0f;
    }
}
