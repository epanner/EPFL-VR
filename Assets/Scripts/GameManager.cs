using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton reference
    public Lanes lanes;

    [Header("UIs")]
    public GameObject gameOverUI;
    public GameObject gameUI;

    [Header("Controllers")]
    public XRBaseInputInteractor leftInteractor;
    public XRBaseInputInteractor rightInteractor;

    [Header("Origins")]
    public GameObject startOrigin;
    public GameObject gameOrigin;

    [Header("Fade Settings")]
    public CanvasGroup fadeCanvas;
    public float fadeDuration = 1f;
    private int health = 3;
    private TempLaneObject leftItem;
    private TempLaneObject rightItem;

    private void Update()
    {
        if (leftItem != null)
        {
            gameUI.GetComponent<GameUI>().SetLeftBarValue(leftItem.GetPercentage());
        }
        if (rightItem != null)
        {
            gameUI.GetComponent<GameUI>().SetRightBarValue(rightItem.GetPercentage());
        }
    }

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
        AudioManager.Instance.PlaySpaceshipMusic();
    }

    public void StartGame(int level)
    {
        switch (level)
        {
            case 1:
                health = 15;
                break;
            case 2:
                health = 5;
                break;
            default:
                health = 3;
                break;
        }
        SwitchToGameWithFade();
        AudioManager.Instance.PlayGameMusic();
        gameOrigin.GetComponent<ArmHoverController>().Init(level);
        lanes.InitGame(level);
        gameUI.SetActive(true);
    }

    public void PlayerHit(int damage = 1)
    {
        Debug.Log("Player Hit! " + health + " live(s) left...");
        BothControllerHaptics(0.2f, 0.1f);
        AudioManager.Instance.PlayPlayerHitSound();
        health -= damage;
        if (health <= 0)
        {
            GameOver();
        }
    }

    public void GrenadeExploded()
    {
        Debug.Log("Grenade");
        BothControllerHaptics(0.8f, 0.2f);
    }

    public void BothControllerHaptics(float intensity, float duration)
    {
        leftInteractor?.SendHapticImpulse(intensity, duration);
        rightInteractor?.SendHapticImpulse(intensity, duration);
    }

    private void GameOver()
    {
        gameUI.SetActive(false);
        gameOverUI.SetActive(true); // Show Game Over UI
        AudioManager.Instance?.PlayGameOverSound();

        lanes.StartLanes(false);

    }

    public void BackToStart()
    {
        SwitchToStartWithFade();
        lanes.CleanGame();

        if (gameOverUI != null)
            gameOverUI.SetActive(false); // Hide Game Over UI
    }

    public void SetLeftItem(TempLaneObject item)
    {
        leftItem = item;
        gameUI.GetComponent<GameUI>().SetLeftBarActive(true);
        AudioManager.Instance.PlayGrabSound();
    }

    public void SetRightItem(TempLaneObject item)
    {
        rightItem = item;
        gameUI.GetComponent<GameUI>().SetRightBarActive(true);
        AudioManager.Instance.PlayGrabSound();
    }

    public void RemoveLeftItem()
    {
        leftItem = null;
        gameUI.GetComponent<GameUI>().SetLeftBarActive(false);
    }

    public void RemoveRightItem()
    {
        rightItem = null;
        gameUI.GetComponent<GameUI>().SetRightBarActive(false);
    }
}
