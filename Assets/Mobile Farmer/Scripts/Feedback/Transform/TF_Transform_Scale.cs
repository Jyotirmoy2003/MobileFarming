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
        if(currentFeedbackManager.targetTramform)
        {
            evalutedVector=currentFeedbackManager.targetTramform.localScale;
           EvaluateTimeline();
        }
    }


    public override void PerformeEffect(float val)
    {
        
    }

    public override void EffectLocal()
    {
       currentFeedbackManager.targetTramform.localScale=evalutedVector;
        
    }


    public override void EffectGlobal()
    {
        currentFeedbackManager.targetTramform.localScale=evalutedVector;
    }


}




