using UnityEngine;
using Assets.Scripts.Utils;
using DG.Tweening;
using Unity.VisualScripting;


#if UNITY_EDITOR
using UnityEditor;
using System.Linq;
using System;
using System.Threading.Tasks;
#endif
[Serializable]
public class Tween
{
    public enum TweenType
    {
        Move,
        Rotate,
        LookAt,
        Scale,
        Fade,
        Shake,
        Delay
    }
    public TweenType tweenType;
    public Transform transform;
    public Vector3 target;
    public float duration;
    public Ease ease;
    [Tooltip("-1 = infinite, 0 = no loops")]
    public int loops;
    public LoopType loopType;
    public Transform targetTransform;
    public CanvasGroup canvasGroup;
    public float targetAlpha;
    public float strength;
    public int vibrato;
    public float randomness;

    public void DoTween(Action onComplete = null)
    {
        switch (tweenType)
        {
            case TweenType.Move:
                TweenUtils.MoveTo(transform, target, duration, ease, loops, onComplete, loopType);
                break;
            case TweenType.Rotate:
                TweenUtils.RotateTo(transform, target, duration, ease, loops, loopType, onComplete);
                break;
            case TweenType.LookAt:
                TweenUtils.LookAt(transform, targetTransform, duration, onComplete, ease);
                break;
            case TweenType.Scale:
                TweenUtils.ScaleTo(transform, target, duration, onComplete, ease);
                break;
            case TweenType.Fade:
                TweenUtils.FadeTo(canvasGroup, target.x, duration, onComplete, ease);
                break;
            case TweenType.Shake:
                TweenUtils.Shake(transform, strength, vibrato, randomness, duration, onComplete);
                break;
            case TweenType.Delay:
                TweenUtils.Delay(duration, onComplete);
                break;
            default:
                break;
        }
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(Tween))]
public class TweenDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUILayout.Space();
        EditorGUI.BeginProperty(position, label, property);

        EditorGUI.PropertyField(position, property.FindPropertyRelative("tweenType"));
        Tween.TweenType tweenType = (Tween.TweenType)property.FindPropertyRelative("tweenType").enumValueIndex;

        position.y += EditorGUIUtility.singleLineHeight * 1.2f;

        EditorGUI.indentLevel++;
        

        switch (tweenType)
        {
            case Tween.TweenType.Move:
                EditorGUI.PropertyField(position, property.FindPropertyRelative("transform"));
                position.y += EditorGUIUtility.singleLineHeight * 1.1f;
                EditorGUI.PropertyField(position, property.FindPropertyRelative("target"));
                position.y += EditorGUIUtility.singleLineHeight * 1.1f;
                EditorGUI.PropertyField(position, property.FindPropertyRelative("duration"));
                position.y += EditorGUIUtility.singleLineHeight * 1.1f;
                EditorGUI.PropertyField(position, property.FindPropertyRelative("ease"));
                position.y += EditorGUIUtility.singleLineHeight * 1.1f;
                EditorGUI.PropertyField(position, property.FindPropertyRelative("loops"));
                position.y += EditorGUIUtility.singleLineHeight * 1.1f;
                EditorGUI.PropertyField(position, property.FindPropertyRelative("loopType"));
                EditorGUILayout.Space(105);
                break;
            case Tween.TweenType.Rotate:
                EditorGUI.PropertyField(position, property.FindPropertyRelative("transform"));
                position.y += EditorGUIUtility.singleLineHeight * 1.1f;
                EditorGUI.PropertyField(position, property.FindPropertyRelative("target"));
                position.y += EditorGUIUtility.singleLineHeight * 1.1f;
                EditorGUI.PropertyField(position, property.FindPropertyRelative("duration"));
                position.y += EditorGUIUtility.singleLineHeight * 1.1f;
                EditorGUI.PropertyField(position, property.FindPropertyRelative("ease"));
                position.y += EditorGUIUtility.singleLineHeight * 1.1f;
                EditorGUI.PropertyField(position, property.FindPropertyRelative("loops"));
                position.y += EditorGUIUtility.singleLineHeight * 1.1f;
                EditorGUI.PropertyField(position, property.FindPropertyRelative("loopType"));
                EditorGUILayout.Space(105);
                break;
            case Tween.TweenType.LookAt:
                EditorGUI.PropertyField(position, property.FindPropertyRelative("transform"));
                position.y += EditorGUIUtility.singleLineHeight * 1.1f;
                EditorGUI.PropertyField(position, property.FindPropertyRelative("targetTransform"));
                position.y += EditorGUIUtility.singleLineHeight * 1.1f;
                EditorGUI.PropertyField(position, property.FindPropertyRelative("duration"));
                position.y += EditorGUIUtility.singleLineHeight * 1.1f;
                EditorGUI.PropertyField(position, property.FindPropertyRelative("ease"));
                EditorGUILayout.Space(65);
                break;
            case Tween.TweenType.Scale:
                EditorGUI.PropertyField(position, property.FindPropertyRelative("transform"));
                position.y += EditorGUIUtility.singleLineHeight * 1.1f;
                EditorGUI.PropertyField(position, property.FindPropertyRelative("target"));
                position.y += EditorGUIUtility.singleLineHeight * 1.1f;
                EditorGUI.PropertyField(position, property.FindPropertyRelative("duration"));
                position.y += EditorGUIUtility.singleLineHeight * 1.1f;
                EditorGUI.PropertyField(position, property.FindPropertyRelative("ease"));
                EditorGUILayout.Space(65);
                break;
            case Tween.TweenType.Fade:
                EditorGUI.PropertyField(position, property.FindPropertyRelative("canvasGroup"));
                position.y += EditorGUIUtility.singleLineHeight * 1.1f;
                EditorGUI.PropertyField(position, property.FindPropertyRelative("targetAlpha"));
                position.y += EditorGUIUtility.singleLineHeight * 1.1f;
                EditorGUI.PropertyField(position, property.FindPropertyRelative("duration"));
                position.y += EditorGUIUtility.singleLineHeight * 1.1f;
                EditorGUI.PropertyField(position, property.FindPropertyRelative("ease"));
                EditorGUILayout.Space(65);
                break;
            case Tween.TweenType.Shake:
                EditorGUI.PropertyField(position, property.FindPropertyRelative("transform"));
                position.y += EditorGUIUtility.singleLineHeight * 1.1f;
                EditorGUI.PropertyField(position, property.FindPropertyRelative("strength"));
                position.y += EditorGUIUtility.singleLineHeight * 1.1f;
                EditorGUI.PropertyField(position, property.FindPropertyRelative("vibrato"));
                position.y += EditorGUIUtility.singleLineHeight * 1.1f;
                EditorGUI.PropertyField(position, property.FindPropertyRelative("randomness"));
                position.y += EditorGUIUtility.singleLineHeight * 1.1f;
                EditorGUI.PropertyField(position, property.FindPropertyRelative("duration"));
                position.y += EditorGUIUtility.singleLineHeight * 1.1f;
                EditorGUI.PropertyField(position, property.FindPropertyRelative("ease"));
                EditorGUILayout.Space(105);
                break;
            case Tween.TweenType.Delay:
                EditorGUI.PropertyField(position, property.FindPropertyRelative("duration"));
                EditorGUILayout.Space(5);
                break;
            default:
                break;
        }
        EditorGUI.EndProperty();
        EditorGUI.indentLevel--;
        EditorGUILayout.Space(10);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // You might need to calculate the height based on the number of fields drawn.
        return base.GetPropertyHeight(property, label);
    }
}
#endif
