using System;
using System.Collections;
using System.Collections.Generic;
using jy_util;
using UnityEngine;
using UnityEngine.Rendering;

public class Cow : AnimalBase, IShakeable,IInteractable
{
    [Header("Elements")]
    [SerializeField] GameObject cameraObject;
    [SerializeField] SkinnedMeshRenderer cowMesh;
    
    public E_ShakeType e_ShakeType { get { return needToShake;} set { needToShake = value;}}

    public E_NeedToperformTask_BeforeShake e_NeedToperformTask_BeforeShake { get {return taskBeforeShake;} set {taskBeforeShake = value;}} 

    [Header("Settings")]
    [SerializeField] E_ShakeType needToShake;
    [SerializeField] E_NeedToperformTask_BeforeShake taskBeforeShake;
    protected float shakeSliderValue = 0;
    private bool IsShaking = false;
    [SerializeField] float shakeIncreament;

    void Initialize()
    {
        StopMovement();
        MobileJoystick.Instance.SetControl(false); //turn off joystick
        SetCameraActivationStatus(true);
        UIManager.Instance.ShowShakeExit(true);
        shakeSliderValue = 0;
    }




    void SetCameraActivationStatus(bool isActive)
    {
         if(isActive)CameraManager.Instance.SwitchCamera(cowMesh.transform,new Vector3(0,5,0));
        else CameraManager.Instance.SwitchCamera();

        
        // if(isActive)
        // {
        //     UIManager.Instance.OnExitButtonPressed += ExitAnimal;
        // }else{
        //     UIManager.Instance.OnExitButtonPressed -= ExitAnimal;
        // }
    }



    void StartShake()
    {
        IsShaking = true;
        //TweenShake(maxShakeMagnitude);
        UpdateShakeSlider();
    }

    void TweenShake(float amount)
    {
        //cowMesh.SetBlendShapeWeight()
    }
    private void UpdateShakeSlider()
    {
        shakeSliderValue += shakeIncreament;
        UIManager.Instance.UpdateShakeSlider(shakeSliderValue);

         //stop when reached max
        if(shakeSliderValue >=1)
            ExitAnimal();
    }

    private void StopShake()
    {
        if(!IsShaking) return;

        IsShaking = false;
        //TweenShake(0);
    }

    private void ExitAnimal()
    {
        _GameAssets.Instance.OnViewChangeEvent.Raise(this,false);
        SetCameraActivationStatus(false);
        MobileJoystick.Instance.SetControl(true); //turn on joystick
        //TweenShake(0);

        //ResstFruits();
    }

    private bool IsReady()
    {
        return true;
    }


    #region  INTERFACE

    public void InIntreactZone(GameObject interactingObject)
    {
        if(!IsReady() || !canInteract) return;
        UIManager.Instance.UpdateShakeSlider(0);
        _GameAssets.Instance.OnViewChangeEvent.Raise(this,true);
        IntiateShake(this.gameObject);
    }

    

    public void Interact(GameObject interactingObject)
    {
        
    }


    public void OutIntreactZone(GameObject interactingObject)
    {
        
    }







    public GameObject IntiateShake(GameObject gameObject)
    {
        Initialize();
        return this.gameObject;
    }


    public void Shake(float magnitude)
    {
        StartShake();
    }

    public void StopShaking()
    {
        
    }

    public void ShowInfo(bool val)
    {
        
    }

    public void ReachedtoTarget()
    {
        throw new NotImplementedException();
    }







    #endregion
}
