using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Runtime.InteropServices.WindowsRuntime;
using System;



public class FeedbackBase : ScriptableObject
{
    
    [Header("CurveSettings")]
    public  AnimationCurve intensityCurve;
    public float duration=1f;
    public Action<float> evaluteAction;

    public Action feedbackFinishedExe;
    private  Sequence s= DOTween.Sequence();




    public FeedbackBase(FeedbackBase feedbackBase)
    {
        intensityCurve= feedbackBase.intensityCurve;
        duration  = feedbackBase.duration;        
    }
    
    public virtual void PushNeededComponent(List<Component> comp)
    {

    }
    
   public virtual void PerformeEffect(float val)
   {
        

   }

   public virtual  void  EvaluateTimeline()
   {
        s.Kill();
        s= DOTween.Sequence();
        
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

   public virtual FeedbackBase CloneMe()
   {
        return new FeedbackBase(this);
   }
}
