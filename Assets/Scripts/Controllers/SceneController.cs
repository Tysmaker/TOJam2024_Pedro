using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;
using static Assets.Scripts.Utils.TweenUtils;
using DG.Tweening;
using Random = UnityEngine.Random;

public class SceneController : MonoBehaviour
{
    Tween tween;

    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] TMP_Text loadText;

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void FadeInCanvas(int sceneIndex)
    {
        FadeTo(canvasGroup, 1, 0.5f, () => DelayScreen(sceneIndex));
        FadeInText();
    }

    public void FadeInText()
    {
        TextMeshProUtils.FadeToText(loadText, 1, 0.5f); // Fading in the TextMeshPro text
        Delay(1f, FadeOutText);
    }

    public void FadeOutText()
    {
        TextMeshProUtils.FadeToText(loadText, 0, 0.5f); // Fading out the TextMeshPro text
        Delay(1f, FadeInText);
    }

    public void DelayScreen(int sceneIndex)
    {

        float randomDelay = Random.Range(3f, 5f);
        FadeInText();
        Delay(randomDelay, () => LoadScene(sceneIndex));
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

public static class TextMeshProUtils
{
    public static void FadeToText(TMP_Text textValue, float targetAlpha, float duration, Action onComplete = null, Ease ease = Ease.Linear)
    {
        textValue.DOFade(targetAlpha, duration).SetEase(ease).OnComplete(() => onComplete?.Invoke());
    }
}