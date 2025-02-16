using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="GAME/Feedback/Camera/CM_Zoom")]
public class CM_Zoom : FB_Camera
{
   
    public float defaultFieldofView=60f;
    public float zoomedFieldofView=20f;

    public CM_Zoom(CM_Zoom camera) : base(camera)
    {
        zoomedFieldofView = camera.zoomedFieldofView;
    }

    public override void OnFeedbackActiavte()
    {
        base.OnFeedbackActiavte();
        if(cinemachineVirtualCamera)
        {
            //defaultFieldofView=cinemachineVirtualCamera.m_Lens.FieldOfView;
           EvaluateTimeline();
        }
    }
    public override void PerformeEffect(float val)
    {
        float value=Mathf.Lerp(defaultFieldofView,zoomedFieldofView,val);
        cinemachineVirtualCamera.m_Lens.FieldOfView=value;
    }

    public override FeedbackBase CloneMe()
    {
        return new CM_Zoom(this);
    }
}
