using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerAnimator : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] Animator animator;
    [SerializeField] Transform rendererTransform;


    [Header("Settings")]
    [SerializeField] float moveSpeedMultiplier=1f;








    public void ManageAnimation(Vector3 moveVector)
    {
        if(moveVector.magnitude > 0)
        {
            animator.SetFloat("moveSpeed",moveVector.magnitude * moveSpeedMultiplier);
            PlayRunAnimation();

            rendererTransform.forward=moveVector.normalized;
        }else{
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
        if(sow) animator.SetLayerWeight(1,1);
        else animator.SetLayerWeight(1,0);
    }

    public void PlayeWaterAnimation(bool water)
    {
        //show watering can mesh
        _GameAssets.Instance.wateringCan.SetActive(water);
        
        if(water)
        {
            animator.SetLayerWeight(2,1);
        } 
        else{
            animator.SetLayerWeight(2,0);
            _GameAssets.Instance.waterParticel.Stop();
        }
    }

    public void PlayerHarvestAnimation(bool harvest)
    {
        //show Scythe mesh
        _GameAssets.Instance.harvestScythe.SetActive(harvest);

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


}
