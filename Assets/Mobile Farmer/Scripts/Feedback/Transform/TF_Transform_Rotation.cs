using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[CreateAssetMenu(menuName ="GAME/Feedback/Tranform/TF_Rotation")]
public class TF_Transform_Rotation : FB_Transform
{
   public override void OnFeedbackActiavte()
    {
        base.OnFeedbackActiavte();
        if(currentFeedbackManager)
        {
            evalutedVector=currentFeedbackManager.targetTramform.localEulerAngles;
           EvaluateTimeline();
        }
    }



    public override void EffectLocal()
    {
       currentFeedbackManager.targetTramform.localEulerAngles=evalutedVector;
        
    }


    public override void EffectGlobal()
    {
        currentFeedbackManager.targetTramform.eulerAngles=evalutedVector;
    }
}