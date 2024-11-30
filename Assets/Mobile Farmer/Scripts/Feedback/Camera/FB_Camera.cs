using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class FB_Camera : FeedbackBase
{
    [HideInInspector]
    public CinemachineVirtualCamera cinemachineVirtualCamera;

    public override void OnFeedbackActiavte()
    {
        base.OnFeedbackActiavte();

        if(!cinemachineVirtualCamera)
        {
            Debug.Log("must assigen cinemachineVirtualCamera first");
            return;
        }
        evaluteAction+=PerformeEffect;
        feedbackFinishedExe+=OnFeedbackDeactivate;

    }

    public override void OnFeedbackDeactivate()
    {
        base.OnFeedbackDeactivate();

        feedbackFinishedExe-=OnFeedbackDeactivate;
        evaluteAction-=PerformeEffect;
    }

    public override void PushNeededComponent(Component comp)
    {
        if(cinemachineVirtualCamera) return;

        //when cam ref is not set get it from manager
        if(comp is CinemachineVirtualCamera)
        {
            cinemachineVirtualCamera=(CinemachineVirtualCamera)comp;
        }
    }
}
