using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class FB_Camera : FeedbackBase
{
    [HideInInspector]
    public CinemachineVirtualCamera cinemachineVirtualCamera;

    public FB_Camera(FB_Camera camera) : base(camera)
    {
        cinemachineVirtualCamera = camera.cinemachineVirtualCamera;
    }

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

    public override void PushNeededComponent(List<Component> comp)
    {
        if(cinemachineVirtualCamera) return;

        //when cam ref is not set get it from manager
        foreach(Component item in comp)
            if(item is CinemachineVirtualCamera)
                cinemachineVirtualCamera=(CinemachineVirtualCamera)item;
            
    }
}
