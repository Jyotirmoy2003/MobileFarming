using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoSingleton<CameraManager>
{
    [SerializeField] GameObject playerCamReal;
    [SerializeField] CinemachineVirtualCamera transitionCamera;


    public void SwitchCamera(Transform lookAt,Vector3 bodyOffset)
    {
        

        transitionCamera.LookAt = lookAt;
        transitionCamera.Follow = lookAt;

        var transposer = transitionCamera.GetCinemachineComponent<CinemachineTransposer>();
        if (transposer != null)
        {
            //transposer.m_FollowOffset = Vector3.Lerp(transposer.m_FollowOffset, bodyOffset, Time.deltaTime * 2f);
            transposer.m_FollowOffset = bodyOffset;
        }

        transitionCamera.gameObject.SetActive(true);
    }

    public void SwitchCamera(Transform lookAt,Vector3 bodyOffset,Vector3 aim)
    {
        

        transitionCamera.LookAt = lookAt;
        transitionCamera.Follow = lookAt;

        var transposer = transitionCamera.GetCinemachineComponent<CinemachineTransposer>();
        if (transposer != null)
        {
            //transposer.m_FollowOffset = Vector3.Lerp(transposer.m_FollowOffset, bodyOffset, Time.deltaTime * 2f);
            transposer.m_FollowOffset = bodyOffset;
        }

        var composer = transitionCamera.GetCinemachineComponent<CinemachineComposer>();
        composer.m_TrackedObjectOffset = aim;

        transitionCamera.gameObject.SetActive(true);
    }

    public void SwitchCamera()
    {
        transitionCamera.gameObject.SetActive(false);
    }
}
