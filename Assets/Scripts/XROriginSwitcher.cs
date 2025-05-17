using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class XROriginSwitcher : MonoBehaviour
{
    [Header("Origins")]
    public GameObject startOrigin;
    public GameObject gameOrigin;

    [Header("Fade Settings")]
    public CanvasGroup fadeCanvas;   // assign your FadeImage's CanvasGroup here
    public float fadeDuration = 1f;

    /// <summary>
    /// Hook this up to your Play button OnClick.
    /// </summary>
    public void SwitchToGameWithFade()
    {
        StartCoroutine(FadeAndSwitch());
        GameManager.Instance.Start();
    }

    IEnumerator FadeAndSwitch()
    {
        // 1) Fade out
        yield return StartCoroutine(Fade(0f, 1f, fadeDuration * 0.5f));

        // 2) Switch origins
        if (startOrigin != null) startOrigin.SetActive(false);
        if (gameOrigin != null) gameOrigin.SetActive(true);

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
}