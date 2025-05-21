using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Music")]
    public AudioClip backgroundMusic;
    public AudioSource musicSource;

    [Header("SFX")]
    public AudioClip grabClip;
    public AudioClip shootClip;
    public AudioClip breakClip;
    public AudioClip gameOverClip;
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
        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        if (backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }

    public void PlayGrabSound()     => PlaySFX(grabClip);
    public void PlayShootSound()    => PlaySFX(shootClip);
    public void PlayBreakSound()    => PlaySFX(breakClip);
    public void PlayGameOverSound() => PlaySFX(gameOverClip);
}