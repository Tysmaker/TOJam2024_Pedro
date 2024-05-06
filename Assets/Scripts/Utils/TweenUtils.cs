using System;
using UnityEngine;
using DG.Tweening;

namespace Assets.Scripts.Utils
{
    public static class TweenUtils
    {
        // Move to target position with duration and calls action when complete
        public static void MoveTo(Transform transform, Vector3 targetPosition, float duration, Ease ease = Ease.Linear, int loops = 0, Action onComplete = null, LoopType loopType = LoopType.Restart)
        {
            transform.DOMove(targetPosition, duration).SetEase(ease).SetLoops(loops, loopType).OnComplete(() => onComplete?.Invoke()).SetId(transform);
        }

        // Rotate to target rotation with duration and calls action when complete
        public static void RotateTo(Transform transform, Vector3 targetRotation, float duration, Ease ease = Ease.Linear, int loops = 0, LoopType loopType = LoopType.Restart, Action onComplete = null)
        {
            transform.DORotate(targetRotation, duration).SetEase(ease).SetLoops(loops, loopType).OnComplete(() => onComplete?.Invoke());
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
        public static void Shake(Transform transform, float strength, int vibrato, float randomness, float duration, Action onComplete = null, Ease ease = Ease.Linear)
        {
            transform.DOShakePosition(duration, strength, vibrato, randomness).SetEase(ease).OnComplete(() => onComplete?.Invoke());
        }

        // Delay for duration and calls action when complete
        public static void Delay(float duration, Action onComplete = null)
        {
            DOVirtual.DelayedCall(duration, () => onComplete?.Invoke());
        }
    }
}