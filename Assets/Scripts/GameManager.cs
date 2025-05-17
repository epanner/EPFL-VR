using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton reference

    public GameObject gameOverUI; // Drag your Game Over UI here
    public Lanes lanes;

    public int health = 3; // Player's health

    private void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        lanes.StartLanes(true);
    }

    public void PlayerHit(int damage = 1)
    {
        Debug.Log("Player Hit!");
        health -= damage;
        if (health <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        if (gameOverUI != null)
            gameOverUI.SetActive(true); // Show Game Over UI

        lanes.StartLanes(false);
    }

    public void ReRunGame()
    {
        lanes.ReInitGame();
        health = 3; // Reset health

        if (gameOverUI != null)
            gameOverUI.SetActive(false); // Hide Game Over UI

        lanes.StartLanes(true);
    }
}
