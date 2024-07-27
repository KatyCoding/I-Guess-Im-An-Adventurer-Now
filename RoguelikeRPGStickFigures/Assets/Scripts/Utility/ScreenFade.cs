using System;
using System.Collections;
using UnityEngine;

public class ScreenFade : MonoBehaviour
{
    public static ScreenFade instance;
    [SerializeField] private SpriteRenderer sprite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
        sprite.color = Color.clear;
        sprite.gameObject.SetActive(false);
    }
    /// <summary>
    /// Duration in seconds
    /// </summary>
    /// <param name="color"></param>
    /// <param name="fadeDuration"></param>
    /// <param name="holdDuration"></param>
    /// <returns></returns>
    public void FadeToColorAndBack(Color color, float fadeDuration, float holdDuration, Action OnHoldBegin = null, Action OnComplete = null)
    {
        StartCoroutine(FadeRoutine(color, fadeDuration, holdDuration, OnHoldBegin, OnComplete));
    }
    private IEnumerator FadeRoutine(Color color, float fadeDuration, float holdDuration, Action OnHoldBegin = null, Action OnComplete = null)
    {
        sprite.gameObject.SetActive(true);
        sprite.color = Color.clear;
        float timer = 0;
        while (timer < fadeDuration)
        {
            yield return null;
            sprite.color = Color.Lerp(Color.clear, color, timer / fadeDuration);
            timer += Time.deltaTime;
        }
        sprite.color = color;
        OnHoldBegin?.Invoke();
        yield return new WaitForSeconds(holdDuration);
        timer = 0;
        while (timer < fadeDuration)
        {
            yield return null;
            sprite.color = Color.Lerp(color, Color.clear, timer / fadeDuration);
            timer += Time.deltaTime;
        }
        sprite.gameObject.SetActive(false);
        OnComplete?.Invoke();
    }
}
