using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Mathf;

public class ScreenShaker : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve animationCurve;

    [SerializeField]
    [Range(0f, 1f)]
    private float duration = 0.3f;

    [Range(0f, 3f)]
    [SerializeField]
    private float intensity = 1f;

    public void ScreenShake()
    {
        this.StartCoroutine(this.ScreenShakeCoroutine());
    }

    private IEnumerator ScreenShakeCoroutine()
    {
        Vector3 originalPos = transform.position;

        float startShake = Time.time;
        float endShake = startShake + duration;

        float noiseSeed = Random.value * 1000;
        float cameraJiggle = intensity * 100;

        while (Time.time < endShake)
        {
            float normalizedTime = (Time.time - startShake) / duration;
            float offsetx = PerlinNoise(noiseSeed + Time.time * cameraJiggle, 0);
            float offsety = PerlinNoise(0, noiseSeed + Time.time * cameraJiggle);

            Vector3 offset = new Vector2(offsetx, offsety)
                             * animationCurve.Evaluate(normalizedTime)
                             * intensity;

            transform.position = originalPos + offset;
            yield return null;
        }

    }
}
