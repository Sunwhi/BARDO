using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ColorFader : MonoBehaviour
{
    [Header("Color Fade")]
    [SerializeField] private SpriteRenderer targetSprite;
    public float cfadeDuration = 1.0f;
    [Range(0f, 1f)]public float maxAlpha = 1f;
    [Range(0f, 1f)]public float minAlpha = 0.4f;

    private void Start()
    {
        StartCoroutine(ColorLoop());
    }

    IEnumerator ColorLoop()
    {
        while (true)
        {
            yield return StartCoroutine(FadeAlpha(minAlpha, maxAlpha, cfadeDuration));

            // 0.5초 대기
            yield return new WaitForSeconds(0.2f);

            // fade out
            yield return StartCoroutine(FadeAlpha(maxAlpha, minAlpha, cfadeDuration));

            // 0.5초 대기
            yield return new WaitForSeconds(0.2f);
        } 
    }

    IEnumerator FadeAlpha(float minAlpha, float maxAlpha, float fadeDuration)
    {
        float time = 0f;

        Color current = targetSprite.color;
        current.a = minAlpha;
        targetSprite.color = current;

        while(time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;

            current.a = Mathf.Lerp(minAlpha, maxAlpha, t);
            targetSprite.color = current;

            yield return null;
        }
        current.a = maxAlpha;
        targetSprite.color = current;

    }
}
