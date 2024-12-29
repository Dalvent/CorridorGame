using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFade: MonoBehaviour
{
    public Image FadeImage;

    public void FadeIn(float fadeTime)
    {
        StartCoroutine(Fade(fadeTime, 1f));
    }

    public void FadeOut(float fadeTime)
    {
        StartCoroutine(Fade(fadeTime, 0f));
    }

    private IEnumerator Fade(float fadeTime, float targetAlpha)
    {
        float startAlpha = FadeImage.color.a;
        float time = 0;

        while (time < fadeTime)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeTime);
            SetAlpha(alpha);
            yield return null;
        }

        SetAlpha(targetAlpha);
    }

    private void SetAlpha(float alpha)
    {
        if (FadeImage != null)
        {
            Color color = FadeImage.color;
            color.a = alpha;
            FadeImage.color = color;
        }
    }
}