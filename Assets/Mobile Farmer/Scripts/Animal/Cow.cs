using System;
using System.Collections.Generic;
using DG.Tweening;
using jy_util;
using UnityEngine;


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


    [Header("COW")]
    [SerializeField] GameObject milkParent;
    [SerializeField] ParticleSystem milkParticel;
    [SerializeField] List<Transform> milkingPos = new List<Transform>();
    private int milkIndex = 0;
    private float milkProgress = 0;
    private bool listenToShake = true;

    [Header("Camera")]
    [SerializeField] Vector3 cameraAimOffset;
    [SerializeField] Vector3 cameraBodyOffSet;










    void Initialize()
    {
        StopMovement();
        MobileJoystick.Instance.SetControl(false); //turn off joystick
        SetCameraActivationStatus(true);
        _GameAssets.Instance.OnShakeInitiateEvent.Raise(this,"Milk the Cow!");
        shakeSliderValue = 0;
        milkParent.SetActive(true);
    }




    void SetCameraActivationStatus(bool isActive)
    {
        if(isActive)CameraManager.Instance.SwitchCamera(cowMesh.transform,cameraBodyOffSet,cameraAimOffset);
        else CameraManager.Instance.SwitchCamera();
    }



    void StartShake( float shake)
    {
        Debug.Log("shke amount: "+shake);
        IsShaking = true;
        TweenShake(shake);
        UpdateShakeSlider();
    }

    void TweenShake(float amount)
    {
        if(!listenToShake) return;

        listenToShake = false;
        DOTween.To(() => milkProgress, x => milkProgress = x, 100f, 1f)
               .OnUpdate(() =>
               {
                  cowMesh.SetBlendShapeWeight(milkIndex,milkProgress);
               })
               .OnComplete(OnOneMilkDone);
        
    }


    void OnOneMilkDone()
    {
        milkParticel.Play();
        listenToShake = true;

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
        StartShake(magnitude);
    }

    public void StopShaking()
    {
        
    }

    public void ShowInfo(bool val)
    {
        
    }

    public void ReachedtoTarget()
    {
       PlayerVisualManager.Instance.SetPlayerRendererShowStatus(false);
    }







    #endregion
}
