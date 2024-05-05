using UnityEngine;
using static Assets.Scripts.Utils.TweenUtils;
using DG.Tweening;

public class TweenExampleCode : MonoBehaviour
{
    [Header("Move Tween Settings"), SerializeField]
    private Vector3 targetDestination = new Vector3(5, 0, 0);
    [SerializeField]
    private float moveDuration = 2f;
    [SerializeField]
    private Ease moveEase = Ease.Linear;

    [Header("Rotate Tween Settings"), SerializeField]
    private Vector3 targetRotation = new Vector3(0, 0, 90);
    [SerializeField]
    private float rotateDuration = 2f;
    [SerializeField]
    private Ease rotateEase = Ease.Linear;

    private void Start()
    {
        // Move the object to the target position in 2 seconds
        Debug.Log("Tweening to target position");
        Move();
    }

    private void Move()
    {
        MoveTo(transform, targetDestination, moveDuration, Rotate, moveEase);
    }
    private void Rotate()
    {
        RotateTo(transform, targetRotation, rotateDuration, CallBackExample, rotateEase);
    }

    // Add more tween methods bellow here to test

    private void CallBackExample()
    {
        Debug.Log("Tween complete!");
    }
}
