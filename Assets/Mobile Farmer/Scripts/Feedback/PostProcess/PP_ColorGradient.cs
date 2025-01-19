using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(menuName ="GAME/Feedback/PostProcess/PP_ColorGradient")]
public class PP_ColorGradient : FB_PostProcess
{
    public Color gradientColor;
    private Color currentColor;
    private ColorAdjustments colorAdjustments;

    public PP_ColorGradient(PP_ColorGradient postProcess) : base(postProcess)
    {
        gradientColor = postProcess.gradientColor;
    }

    public override void OnFeedbackActiavte()
    {
        base.OnFeedbackActiavte();
        if(globalVolume.profile.TryGet(out colorAdjustments))
        {
            currentColor=colorAdjustments.colorFilter.value;
           EvaluateTimeline();
        }
    }
    public override void PerformeEffect(float val)
    {
        
        colorAdjustments.colorFilter.value = Color.Lerp(currentColor,gradientColor,val);
    }

    public override FeedbackBase CloneMe()
    {
        return new PP_ColorGradient(this);
    }
}
