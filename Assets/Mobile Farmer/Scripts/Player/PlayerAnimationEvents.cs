using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using System;

public class PlayerAnimationEvents : MonoBehaviour
{
    [Header("Elements")]
     ParticleSystem seedParticle;
     PlayerDataHolder owningPlayer;

    public Action<PlayerDataHolder> startHarvestCallBackEvent, endHarvestCallBackEvent;


    void Start()
    {
        if(!owningPlayer)
        {
            owningPlayer = GetComponentInParent<PlayerDataHolder>();
            if(owningPlayer)
                seedParticle = owningPlayer.seedParticle.GetComponent<ParticleSystem>();
        }
    }

    private void PlaySeedParticle()
    {
        seedParticle.Play();
    }

  
    private void StartHarvestingCallback()
    {
        startHarvestCallBackEvent?.Invoke(owningPlayer);
        //AudioManager.instance.PlaySound("Cut");
    }

    private void StopHervestingCallback()
    {
        endHarvestCallBackEvent?.Invoke(owningPlayer);
    }

    public void CacheData()
    {
        owningPlayer = GetComponentInParent<PlayerDataHolder>();
        if(owningPlayer)
            seedParticle = owningPlayer.seedParticle.GetComponent<ParticleSystem>();
    }
  
}
