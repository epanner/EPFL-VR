using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Music")]
    public AudioClip spaceshipMusic;   // ← assign for XR Origin_Start
    public AudioClip gameMusic;        // ← assign for XR Origin_Game
    public AudioSource musicSource;

    [Header("SFX")]
    public AudioClip grabClip;
    public AudioClip shootClip;
    public AudioClip breakClip;
    public AudioClip gameOverClip;
    public AudioClip uiClickClip;
    public AudioClip playerHitClip;
    public AudioSource sfxSource;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        PlaySpaceshipMusic();  // Start in menu context
    }

    public void PlaySpaceshipMusic()
    {
        PlayMusic(spaceshipMusic);
    }

    public void PlayGameMusic()
    {
        PlayMusic(gameMusic);
    }

    public void PauseGameMusic()
    {
        musicSource.Pause();
    }

    public void ResumeGameMusic()
    {
        musicSource.Play();
    }

    private void PlayMusic(AudioClip clip)
    {
        if (clip == null || musicSource == null) return;

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
            sfxSource.PlayOneShot(clip);
    }

    public void PlayGrabSound() => PlaySFX(grabClip);
    public void PlayShootSound() => PlaySFX(shootClip);
    public void PlayBreakSound() => PlaySFX(breakClip);
    public void PlayGameOverSound() => PlaySFX(gameOverClip);
    public void PlayUIClickSound() => PlaySFX(uiClickClip);
    public void PlayPlayerHitSound() => PlaySFX(playerHitClip);
}