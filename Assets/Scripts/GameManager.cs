using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton reference
    public Lanes lanes;

    [Header("UIs")]
    public GameObject gameOverUI;
    public GameObject gamePausedUI;
    public GameObject gameUI;
    public GameObject welcomeUI;

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
    private Func<float, float> ScoreFunction { get; set; } = x => x * x;
    private TempLaneObject leftItem;
    private TempLaneObject rightItem;
    public float gameTimer = 0.0f;
    private int currentScore = 0;
    [HideInInspector] public int scoreRecord = 0;
    [HideInInspector] public int lastScore = 0;
    [HideInInspector] public int level;
    [HideInInspector] public bool inGame = false;
    private List<GameObject> toDestroy = new List<GameObject>();
    private Buzzer firstBuzzer = null;
    private Buzzer secondBuzzer = null;
    private void Awake()
    {
        Instance = this;
        AudioManager.Instance.PlaySpaceshipMusic();
    }

    private void Start()
    {
        welcomeUI.GetComponent<WelcomeUI>().SetLastScore(0);
        welcomeUI.GetComponent<WelcomeUI>().SetRecord(0);
    }

    public void StartGame(int level)
    {
        Time.timeScale = 1.0f;
        this.level = level;
        switch (level)
        {
            case 1:
                health = 15;
                ScoreFunction = x => 0.01f * x * x;
                break;
            case 2:
                health = 5;
                ScoreFunction = x => x * x;
                break;
            default:
                health = 3;
                ScoreFunction = x => x * x * Mathf.Log(1f + x);
                break;
        }
        SwitchToGameWithFade();
        gameOrigin.GetComponent<ArmHoverController>().Init(level);
        lanes.InitGame(level);
        AudioManager.Instance.PlayGameMusic();
        gameUI.GetComponent<GameUI>().InitUI(health, currentScore);
        gameUI.SetActive(true);
        inGame = true;
    }

    private void Update()
    {
        if (inGame)
        {
            gameTimer += Time.deltaTime;
            UpdateScoreUI();
            if (leftItem != null)
            {
                gameUI.GetComponent<GameUI>().SetLeftBarValue(leftItem.GetPercentage());
            }
            if (rightItem != null)
            {
                gameUI.GetComponent<GameUI>().SetRightBarValue(rightItem.GetPercentage());
            }
            if (firstBuzzer != null)
            {
                if (secondBuzzer == null)
                {
                    gameUI.GetComponent<GameUI>().SetBuzzerTimer(firstBuzzer.remainingTime);
                }
                else
                {
                    UnlockWall();
                }
            }
        }
    }

    public void GameOver()
    {
        inGame = false;
        lanes.StartLanes(false);
        gameUI.SetActive(false);
        gameOverUI.SetActive(true); // Show Game Over UI
        Time.timeScale = 0.0f;
        AudioManager.Instance.PauseGameMusic();
    }

    public void GamePaused()
    {
        inGame = false;
        lanes.StartLanes(false);
        gamePausedUI.SetActive(true);
        Time.timeScale = 0f;
        AudioManager.Instance.PauseGameMusic();
    }

    public void GameResumed()
    {
        Time.timeScale = 1f;
        lanes.StartLanes(true);
        inGame = true;
        AudioManager.Instance.ResumeGameMusic();
    }

    public void BackToStart()
    {
        Time.timeScale = 1f;
        Clean();
        SaveAndReInitCurrentScore();
        SwitchToStartWithFade();
        AudioManager.Instance.PlaySpaceshipMusic();
    }

    private void UpdateScoreUI()
    {
        currentScore = Mathf.RoundToInt(ScoreFunction(gameTimer));
        gameUI.GetComponent<GameUI>().SetScore(currentScore);
    }

    public void PlayerHit(int damage = 1)
    {
        AudioManager.Instance.PlayPlayerHitSound();
        BothControllerHaptics(0.2f, 0.1f);
        health -= damage;
        if (health <= 0)
        {
            GameOver();
        }
        gameUI.GetComponent<GameUI>().SetHealth(health);
    }

    public void BothControllerHaptics(float intensity, float duration)
    {
        leftInteractor?.SendHapticImpulse(intensity, duration);
        rightInteractor?.SendHapticImpulse(intensity, duration);
    }

    private void SaveAndReInitCurrentScore()
    {
        lastScore = currentScore;
        if (currentScore > scoreRecord)
        {
            scoreRecord = currentScore;
        }
        welcomeUI.GetComponent<WelcomeUI>().SetLastScore(lastScore);
        welcomeUI.GetComponent<WelcomeUI>().SetRecord(scoreRecord);
        currentScore = 0;
        gameTimer = 0.0f;
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

    public void GrenadeExploded()
    {
        Debug.Log("Grenade");
        BothControllerHaptics(0.8f, 0.2f);
    }

    public void AddToDestroy(GameObject item)
    {
        toDestroy.Add(item);
    }

    private void Clean()
    {
        lanes.CleanGame();
        foreach (GameObject item in toDestroy)
        {
            Destroy(item);
        }
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

    public void SetBuzzer(Buzzer buzzer)
    {
        if (firstBuzzer == null)
        {
            firstBuzzer = buzzer;
            gameUI.GetComponent<GameUI>().SetBuzzerTimerActive(true);
        }
        else if (firstBuzzer != buzzer)
        {
            secondBuzzer = buzzer;
        }
    }

    public void RemoveBuzzer()
    {
        firstBuzzer = null;
    }

    public void UnlockWall()
    {
        gameUI.GetComponent<GameUI>().SetBuzzerTimerActive(false);
        if (firstBuzzer != null) firstBuzzer.BothBuzzed();
        firstBuzzer = null;
        if (secondBuzzer != null) secondBuzzer.BothBuzzed();
        secondBuzzer = null;
        lanes.WallUnlocked();
    }
}
