using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerAnimator : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] Animator animator;
    [SerializeField] Transform rendererTransform;
    [SerializeField] Transform horseTranform;
    [SerializeField] ParticleSystem waterParticel,seedParticel;
    [SerializeField] GameObject wateringCan,harvestScythe,fishingRod;


    [Header("Settings")]
    [SerializeField] float moveSpeedMultiplier=1f;

    void Start()
    {
        // #if UNITY_EDITOR
        // moveSpeedMultiplier = 30f;
        // #endif
    }







    public void ManageAnimation(Vector3 moveVector)
    {
        if(moveVector.magnitude > 0)
        {
            animator.SetFloat("moveSpeed",moveVector.magnitude * moveSpeedMultiplier);
            PlayRunAnimation();

            rendererTransform.forward=moveVector.normalized;
            
        }else{
            animator.SetFloat("moveSpeed",0);
            PlayIdleAnimation();
        }
    }
    public void ManageAnimation(float speed)
    {
        if(speed>0.2){
            animator.SetFloat("moveSpeed",speed * moveSpeedMultiplier);
            PlayRunAnimation();
        }
        else {
            animator.SetFloat("moveSpeed",0);
            PlayIdleAnimation();
        }
    }

    void PlayRunAnimation()
    {
        animator.Play("Run");
    }
    
    void PlayIdleAnimation()
    {
        animator.Play("Idle");
    }

    public void PlaySowAnimation(bool sow)
    {
        seedParticel.Stop();
        if(sow) animator.SetLayerWeight(1,1);
        else animator.SetLayerWeight(1,0);
    }
    public void StopAllLayeredAnimation()
    {
        animator.SetLayerWeight(1,0);
        animator.SetLayerWeight(2,0);
        waterParticel.Stop();
        animator.SetLayerWeight(3,0);
        animator.SetLayerWeight(4,0);

        wateringCan.SetActive(false);
        harvestScythe.SetActive(false);
    }

    public void PlayeWaterAnimation(bool water)
    {
        //show watering can mesh
        wateringCan.SetActive(water);
        
        if(water)
        {
            animator.SetLayerWeight(2,1);
            waterParticel.Play();
        } 
        else{
            animator.SetLayerWeight(2,0);
            waterParticel.Stop();
        }
    }

    public void PlayerHarvestAnimation(bool harvest)
    {
        //show Scythe mesh
        harvestScythe.SetActive(harvest);

        if(harvest)
        {
            animator.SetLayerWeight(3,1);
        }else{
            animator.SetLayerWeight(3,0);
        }
    }

    public void PlayerShakeTreeAnimation(bool play)
    {
        if(play)
        {
            animator.SetLayerWeight(4,1);
            animator.Play("Shake Tree");
        }else{
            animator.SetLayerWeight(4,0);
        }
    }

    public void PlayFishingRod(bool isInitiate)
    {
        if(isInitiate)
        {
            animator.Play("Fishing Cast");
        }else{
            animator.Play("Idle");
        }

        fishingRod.SetActive(isInitiate);
    }

    public void PlayerReadyToShake()
    {
        animator.SetLayerWeight(4,1);
    }

    public void CacheNewVisual(DressSetup dressSetup)
    {
        animator = dressSetup.animator;
        rendererTransform = dressSetup.animator.transform;
        wateringCan = dressSetup.wateringCan;
        harvestScythe = dressSetup.Scythe;
    }


}
