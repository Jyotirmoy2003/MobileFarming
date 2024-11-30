using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Runtime.InteropServices.WindowsRuntime;
using System;



public class FeedbackBase : ScriptableObject
{
    public  AnimationCurve intensityCurve;
    public float duration=1f;
    public Action<float> evaluteAction;

    public Action feedbackFinishedExe;
    
    public virtual void PushNeededComponent(Component comp)
    {

    }
    
   public virtual void PerformeEffect(float val)
   {
        

   }

   public  void  EvaluateTimeline()
   {
        Sequence s= DOTween.Sequence();
        
        s.Append(DOVirtual.Float(0, 1f, duration, v =>ExecuteCurve(v) )).onComplete=EndExe;
   }
    void ExecuteCurve(float v)
    {
        float evaluatedValue=intensityCurve.Evaluate(v);
        evaluteAction?.Invoke(evaluatedValue);
    }
    void EndExe()=>feedbackFinishedExe?.Invoke();

   public virtual void OnFeedbackActiavte(){

   }
   public virtual void OnFeedbackDeactivate(){

   }
}
