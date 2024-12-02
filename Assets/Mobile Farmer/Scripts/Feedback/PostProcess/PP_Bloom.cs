using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(menuName ="GAME/Feedback/PostProcess/PP_Bloom")]
public class PP_Bloom : FB_PostProcess
{
    private Bloom bloom;
    public override void OnFeedbackActiavte()
    {
        base.OnFeedbackActiavte();
        if(globalVolume.profile.TryGet(out bloom))
        {
           EvaluateTimeline();
        }
    }

    public override void PerformeEffect(float val)
    {  
        bloom.intensity.value=val;
    }

    public override void OnFeedbackDeactivate()
    {
        base.OnFeedbackDeactivate();

        
    }
}
