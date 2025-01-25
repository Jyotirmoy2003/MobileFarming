using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleTree : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] GameObject treeCam;
    [SerializeField] Renderer treeRendere;
    [SerializeField] Transform fruitParent;
    private AppleTreeManager treeManager;

    [Header("Settings")]
    [SerializeField] float maxShakeMagnitude = 0.005f;
    [SerializeField] float shakeIncreament = 0.2f;
    [SerializeField] float furitShakeMultiplayer;
    private float shakeSliderValue = 0f;
    private float shakeMagnitude = 0;
    private bool IsShaking = false;



    public void Initialize(AppleTreeManager treeManager)
    {
        SetTreeCamActivation(true);
        shakeSliderValue = 0;
        this.treeManager = treeManager;
    }



    public void SetTreeCamActivation(bool isActive)
    {
        treeCam.SetActive(isActive);
    }

    public void ShakeTree()
    {
        IsShaking = true;
        TweenShake(maxShakeMagnitude);
        UpdateShakeSlider();
    }

    public void StopShake()
    {
        if(!IsShaking) return;

        IsShaking = false;
        TweenShake(0);
    }

    private void TweenShake(float targetMagnitude)
    {
        LeanTween.cancel(treeRendere.gameObject);
       LeanTween.value(treeRendere.gameObject,UpdateMaterial,shakeMagnitude,targetMagnitude,1);
    }

    private void UpdateMaterial(float magnitude)
    {
        shakeMagnitude = magnitude;
        treeRendere.material.SetFloat("_Magnitude",shakeMagnitude);
        foreach(Transform fruit in fruitParent)
        {
            Apple apple = fruit.GetComponent<Apple>();

            apple.Shake(shakeMagnitude * furitShakeMultiplayer);
        }
    }

    private void UpdateShakeSlider()
    {
        shakeSliderValue += shakeIncreament;
        treeManager.UpdateShakeSlider(shakeSliderValue);

        

        
    }
}
