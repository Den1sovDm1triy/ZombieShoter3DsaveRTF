using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
public static class AnimTweenManager 
{
    public static void DoScaleShake(Transform objTransform, float scaleAmount, float time)
    {
        objTransform.DOScale(scaleAmount, time) 
                .SetEase(Ease.OutElastic)
                .SetUpdate(true)
                .OnComplete(() =>
                {                   
                    objTransform.DOScale(1f, time);
                });
    }
     public static void DoScaleShake(Transform objTransform, float scaleAmount, float time, Action CompleteAction)
    {
        objTransform.DOScale(scaleAmount, time) 
                .SetEase(Ease.OutElastic)
                .SetUpdate(true)
                .OnComplete(() =>
                {                   
                    objTransform.DOScale(1f, time);
                    CompleteAction();
                });
    }


    public static void DoScaleShakeY(Transform objTransform, float scaleAmount, float time)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(objTransform.DOScaleY(scaleAmount, time / 2)); 
        sequence.Join(objTransform.DOScaleX(scaleAmount * 2f, time / 2)); 
        sequence.Append(objTransform.DOScaleX(1f, time / 2)); 
        sequence.Join(objTransform.DOScaleY(1f, time / 2)); 
    }
    public static void DoMoveToPoint(Transform objTransform, Vector3 endPos, float time, Action CompleteAction)
    {
        objTransform.DOMove(endPos, time).OnComplete(() =>
        {
            CompleteAction();
        });
    }
    public static void DoMoveToPoint(Transform objTransform, Vector3 endPos, float time)
    {
        objTransform.DOMove(endPos, time);
    }



}
