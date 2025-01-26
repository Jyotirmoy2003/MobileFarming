using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using System;

public class PlayerAnimationEvents : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] ParticleSystem seedParticle;
    [SerializeField] ParticleSystem waterParticle;

    public static Action startHarvestCallBackEvent, endHarvestCallBackEvent;


    void Start()
    {
        
    }

    private void PlaySeedParticle()
    {
        seedParticle.Play();
    }

    private void PlayWaterAnimation()
    {
        waterParticle.Play();
    }

    private void StartHarvestingCallback()
    {
        startHarvestCallBackEvent?.Invoke();
    }

    private void StopHervestingCallback()
    {
        endHarvestCallBackEvent?.Invoke();
    }
  
}
