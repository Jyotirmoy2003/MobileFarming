using System.Collections;
using System.Collections.Generic;
using System.Net;
using Cinemachine;
using UnityEngine;


[CreateAssetMenu(menuName ="GAME/Feedback/CM_Shake")]
public class CM_Shake : FB_Camera
{
    [SerializeField] float senetivity=1f;
    private CinemachineBasicMultiChannelPerlin perlinNoise;











    public override void OnFeedbackActiavte()
    {
        base.OnFeedbackActiavte();
        if(cinemachineVirtualCamera)
        {
            perlinNoise=cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
           EvaluateTimeline();
        }
    }
    public override void PerformeEffect(float val)
    {
        ShakeCurrentCamera(1,val*senetivity);

    }
    public override void OnFeedbackDeactivate()
    {
        base.OnFeedbackDeactivate();
        ShakeCurrentCamera(0,0);
    }
    public void ShakeCurrentCamera(float frquencyGain = 0.5f,float amplitudeGain = 1f)
    {
        if(perlinNoise != null)
        {
            perlinNoise.m_AmplitudeGain=amplitudeGain;
            perlinNoise.m_FrequencyGain=frquencyGain;
        }
    }
}