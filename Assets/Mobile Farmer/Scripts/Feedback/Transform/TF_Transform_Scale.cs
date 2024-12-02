using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName ="GAME/Feedback/Tranform/TF_Scale")]
public class TF_Transform_Scale : FB_Transform
{
    
    public override void OnFeedbackActiavte()
    {
        base.OnFeedbackActiavte();
        if(targetTranform)
        {
            evalutedVector=targetTranform.localScale;
           EvaluateTimeline();
        }
    }


    public override void PerformeEffect(float val)
    {
        
    }

    public override void EffectLocal()
    {
       targetTranform.localScale=evalutedVector;
        
    }


    public override void EffectGlobal()
    {
        targetTranform.localScale=evalutedVector;
    }


}


