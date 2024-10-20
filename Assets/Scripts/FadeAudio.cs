using UnityEngine;
using System.Collections;

public class FadeAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public float fadeDuration = 2.0f;

    private float targetVolume;
    private float initialVolume;

    void Start()
    {
        if (audioSource != null)
        {
            targetVolume = audioSource.volume;
            initialVolume = audioSource.volume;
        }
    }

    public void StartFadeIn()
    {
        StartCoroutine(FadeIn());
    }

    public void StartFadeOut()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeIn()
    {
        float currentTime = 0;
        audioSource.volume = 0;

        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0, targetVolume, currentTime / fadeDuration);
            yield return null;
        }

        audioSource.volume = targetVolume;
    }

    private IEnumerator FadeOut()
    {
        float currentTime = 0;

        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(initialVolume, 0, currentTime / fadeDuration);
            yield return null;
        }

        audioSource.volume = 0;
        audioSource.Stop();
    }
}
