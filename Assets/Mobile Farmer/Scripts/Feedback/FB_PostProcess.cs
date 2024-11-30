using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class FB_PostProcess : FeedbackBase
{
    [HideInInspector]
    public Volume globalVolume;
    public enum E_PP_Effect_Type
    {
        Bloom,
        Colorgradient,
    }
    public E_PP_Effect_Type effectType;








    public override void OnFeedbackActiavte()
    {
        base.OnFeedbackActiavte();

        if(!globalVolume) globalVolume= FindAnyObjectByType<Volume>();
        evaluteAction+=PerformeEffect;
        feedbackFinishedExe+=OnFeedbackDeactivate;

    }

    public override void OnFeedbackDeactivate()
    {
        base.OnFeedbackDeactivate();

        feedbackFinishedExe-=OnFeedbackDeactivate;
        evaluteAction-=PerformeEffect;
    }
}
