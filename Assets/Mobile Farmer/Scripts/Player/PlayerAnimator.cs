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
        // Sequence s = DOTween.Sequence();
        // if(sow)
        //     s.Append(DOVirtual.Float(0, 1f, 1f, v => animator.SetLayerWeight(1,v)));
        // else
        //     s.Append(DOVirtual.Float(1, 0f, 1f, v => animator.SetLayerWeight(1,v)));
    }

    public void PlayeWaterAnimation(bool water)
    {
        if(water) animator.SetLayerWeight(2,1);
        else animator.SetLayerWeight(2,0);
    }


}
