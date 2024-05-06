using UnityEngine;
using static Assets.Scripts.Utils.TweenUtils;
using DG.Tweening;
using System;
using System.Collections.Generic;

public class TweenExampleCode : MonoBehaviour
{
    [Header("Tween Settings"), SerializeField]
    private List<Tween> tweens;

    private void Start()
    {
        TweenAll();
    }

    void Update()
    {
        // Press space to restart the tween
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DOTween.KillAll();
        }
    }
    private void TweenAll(int index = 0)
    {
        if (index < tweens.Count)
        {
            tweens[index].DoTween(() => TweenAll(index + 1));
            return;
        }
        CallBackExample();
    }

    private void CallBackExample()
    {
        Debug.Log("Tween complete!");
    }
}
