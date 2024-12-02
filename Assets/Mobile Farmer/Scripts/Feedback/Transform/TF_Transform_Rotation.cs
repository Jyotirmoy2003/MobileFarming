using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[CreateAssetMenu(menuName ="GAME/Feedback/Tranform/TF_Rotation")]
public class TF_Transform_Rotation : FB_Transform
{
   public override void OnFeedbackActiavte()
    {
        base.OnFeedbackActiavte();
        if(targetTranform)
        {
            evalutedVector=targetTranform.localEulerAngles;
           EvaluateTimeline();
        }
    }



    public override void EffectLocal()
    {
       targetTranform.localEulerAngles=evalutedVector;
        
    }


    public override void EffectGlobal()
    {
        targetTranform.eulerAngles=evalutedVector;
    }
}
