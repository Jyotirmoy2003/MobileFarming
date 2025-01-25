using System.Collections;
using System.Collections.Generic;
using jy_util;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Apple : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] Renderer appleRenderer;
    [SerializeField] Rigidbody rig;

    private Vector3 initialPos,initalRotation;
    private E_Crop_Progess state;
    void Start()
    {
        initialPos = transform.position;
        initalRotation = transform.eulerAngles;
    }
    public void Shake(float magnitude)
    {
        appleRenderer.material.SetFloat("_Magnitude", magnitude);
    }

    public void Release()
    {
        rig.isKinematic = false;
        appleRenderer.material.SetFloat("_Magnitude", 0);
    }

    public bool IsFree()
    {
        return !rig.isKinematic;
    }
    public bool IsReady()
    {
        return state == E_Crop_Progess.Ready;
    }

    #region ResetApples
    internal void Reset()
    {
        LeanTween.scale(gameObject,Vector3.zero,1f).setDelay(2f).setOnComplete(ForceRest);
    }

    void ForceRest()
    {
        transform.position = initialPos;
        transform.eulerAngles = initalRotation;

        rig.isKinematic = true;

        //scale up
        float randomTime = Random.Range(5f,10f);
        state = E_Crop_Progess.Growing;
        LeanTween.scale(gameObject,Vector3.one,randomTime).setOnComplete(SetReady);
    }

    private void SetReady()
    {
        state = E_Crop_Progess.Ready;
    }

    #endregion
}
