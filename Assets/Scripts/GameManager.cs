using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton reference
    public Lanes lanes;
    public int health = 3; // Player's health

    [Header("UIs")]
    public GameObject gameOverUI;

    [Header("Origins")]
    public GameObject startOrigin;
    public GameObject gameOrigin;

    [Header("Fade Settings")]
    public CanvasGroup fadeCanvas;
    public float fadeDuration = 1f;

    public void SwitchToGameWithFade()
    {
        StartCoroutine(FadeAndSwitchToGame());
    }

    IEnumerator FadeAndSwitchToGame()
    {
        // 1) Fade out
        yield return StartCoroutine(Fade(0f, 1f, fadeDuration * 0.5f));

        // 2) Switch origins
        if (startOrigin != null) startOrigin.SetActive(false);
        if (gameOrigin != null) gameOrigin.SetActive(true);

        // 3) Fade in
        yield return StartCoroutine(Fade(1f, 0f, fadeDuration * 0.5f));
    }
    
    public void SwitchToStartWithFade()
    {
        StartCoroutine(FadeAndSwitchToStart());
    }

    IEnumerator FadeAndSwitchToStart()
    {
        // 1) Fade out
        yield return StartCoroutine(Fade(0f, 1f, fadeDuration * 0.5f));

        // 2) Switch origins
        if (gameOrigin != null) gameOrigin.SetActive(false);
        if (startOrigin != null) startOrigin.SetActive(true);

        // 3) Fade in
        yield return StartCoroutine(Fade(1f, 0f, fadeDuration * 0.5f));
    }

    IEnumerator Fade(float from, float to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }
        fadeCanvas.alpha = to;
    }

    private void Awake()
    {
        Instance = this;
    }

    public void StartGame()
    {
        SwitchToGameWithFade();
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

    public void ReInitGame()
    {
        SwitchToStartWithFade();
        lanes.ReInitGame();
        health = 3; // Reset health

        if (gameOverUI != null)
            gameOverUI.SetActive(false); // Hide Game Over UI
    }
}
