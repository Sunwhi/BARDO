using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightFader : MonoBehaviour
{
    [SerializeField] private Light2D targetLight;
    public float fadeDuration = 1.0f;
    public float maxIntensity = 1.5f;
    public float minIntensity = 0f;

    private void Start()
    {
        StartCoroutine(LightLoop());
    }

    IEnumerator LightLoop()
    {
        while (true)
        {
            // fade in
            yield return StartCoroutine(FadeLight(minIntensity, maxIntensity, fadeDuration));

            // 0.5초 대기
            yield return new WaitForSeconds(0.5f);

            // fade out
            yield return StartCoroutine(FadeLight(maxIntensity, minIntensity, fadeDuration));

            // 0.5초 대기
            yield return new WaitForSeconds(0.5f);
        } 
    }

    // minIntensity -> maxIntensity로 lerp
    IEnumerator FadeLight(float minIntensity, float maxIntensity, float fadeDuration)
    {
        float time = 0f;
        while(time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration; 

            targetLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, t);

            yield return null;
        }
        targetLight.intensity = maxIntensity;
    }
}
