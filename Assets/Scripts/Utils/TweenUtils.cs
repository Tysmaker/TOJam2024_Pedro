using System;
using UnityEngine;
using DG.Tweening;

public static class TweenUtils
{
    // Move to target position with duration and calls action when complete
    public static void MoveTo(Transform transform, Vector3 targetPosition, float duration, Action onComplete = null, Ease ease = Ease.Linear)
    {

    }

    // Rotate to target rotation with duration and calls action when complete
    public static void RotateTo(Transform transform, Vector3 targetRotation, float duration, Action onComplete = null, Ease ease = Ease.Linear)
    {
        transform.DORotate(targetRotation, duration).SetEase(ease).OnComplete(() => onComplete?.Invoke());
    }

    // Look at target with duration and calls action when complete
    public static void LookAt(Transform transform, Transform target, float duration, Action onComplete = null, Ease ease = Ease.Linear)
    {
        transform.DOLookAt(target.position, duration).SetEase(ease).OnComplete(() => onComplete?.Invoke());
    }

    // Scale to target scale with duration and calls action when complete
    public static void ScaleTo(Transform transform, Vector3 targetScale, float duration, Action onComplete = null, Ease ease = Ease.Linear)
    {
        transform.DOScale(targetScale, duration).SetEase(ease).OnComplete(() => onComplete?.Invoke());
    }

    // Fade to target alpha with duration and calls action when complete
    public static void FadeTo(CanvasGroup canvasGroup, float targetAlpha, float duration, Action onComplete = null, Ease ease = Ease.Linear)
    {
        canvasGroup.DOFade(targetAlpha, duration).SetEase(ease).OnComplete(() => onComplete?.Invoke());
    }

    // Shake transform with strength, vibrato, randomness and duration
    public static void Shake(Transform transform, float strength, int vibrato, float randomness, float duration, Action onComplete = null)
    {
        transform.DOShakePosition(duration, strength, vibrato, randomness).OnComplete(() => onComplete?.Invoke());
    }

    // Delay for duration and calls action when complete
    public static void Delay(float duration, Action onComplete = null)
    {
        DOVirtual.DelayedCall(duration, () => onComplete?.Invoke());
    }
}
