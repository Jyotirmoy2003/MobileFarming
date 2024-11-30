using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(menuName ="GAME/Feedback/PP_ColorGradient")]
public class PP_ColorGradient : FB_PostProcess
{
    [SerializeField] Color gradientColor;
    private Color currentColor;
    private ColorAdjustments colorAdjustments;



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
}
