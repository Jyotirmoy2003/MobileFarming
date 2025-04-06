using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoSingleton<CameraManager>
{
    [SerializeField] GameObject playerCamReal;
    [SerializeField] CinemachineVirtualCamera transitionCamera;
    [SerializeField] Transform transitionCameraRig;
    private bool isRotating;
    [SerializeField] float rotationSpeed;
  

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


    #region Rotate Around object
    public void StartRotating(Transform target,Vector3 worldPosPivot)
    {
        if (!isRotating && transitionCameraRig != null && target != null)
        {
            transitionCamera.Follow = null;
            isRotating = true;
            StartCoroutine(RotateAroundTarget(target));
        }
    }

    public void StopRotating()
    {
        isRotating = false;
    }

    private IEnumerator RotateAroundTarget(Transform target)
    {
        while (isRotating)
        {
            transitionCameraRig.RotateAround(target.position, Vector3.up, rotationSpeed * Time.deltaTime);
            transitionCamera.transform.LookAt(target); // Keep looking at the target
            yield return null;
        }
    }


    #endregion
    
}
