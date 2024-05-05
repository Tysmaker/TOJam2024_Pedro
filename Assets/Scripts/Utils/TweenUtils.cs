using System;
using UnityEngine;
using DG.Tweening;

public static class TweenUtils 
{
    // Move to target position with duration and calls action when complete
    public static void MoveTo(Transform transform, Vector3 targetPosition, float duration, Action onComplete = null)
    {
        transform.DOMove(targetPosition, duration).OnComplete(() => onComplete?.Invoke());
    }

    // Rotate to target rotation with duration and calls action when complete
    public static void RotateTo(Transform transform, Vector3 targetRotation, float duration, Action onComplete = null)
    {
        transform.DORotate(targetRotation, duration).OnComplete(() => onComplete?.Invoke());
    }

    // Look at target with duration and calls action when complete
    public static void LookAt(Transform transform, Transform target, float duration, Action onComplete = null)
    {
        transform.DOLookAt(target.position, duration).OnComplete(() => onComplete?.Invoke());
    }

    // Scale to target scale with duration and calls action when complete
    public static void ScaleTo(Transform transform, Vector3 targetScale, float duration, Action onComplete = null)
    {
        transform.DOScale(targetScale, duration).OnComplete(() => onComplete?.Invoke());
    }

    // Fade to target alpha with duration and calls action when complete
    public static void FadeTo(CanvasGroup canvasGroup, float targetAlpha, float duration, Action onComplete = null)
    {
        canvasGroup.DOFade(targetAlpha, duration).OnComplete(() => onComplete?.Invoke());
    }

    // Delay for duration and calls action when complete
    public static void Delay(float duration, Action onComplete = null)
    {
        DOVirtual.DelayedCall(duration, () => onComplete?.Invoke());
    }
}
