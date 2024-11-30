using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimator))]
public class PlayerSowAbility : MonoBehaviour
{
    private PlayerAnimator playerAnimator;


    private CropField currentCropField;

    void Start()
    {
        playerAnimator = GetComponent<PlayerAnimator>();
        SeedParticle.onSeedCollided+=SeedCollidedCallback;
    }
    void OnDestroy()
    {
        SeedParticle.onSeedCollided -=SeedCollidedCallback;
    }




    private void SeedCollidedCallback(Vector3[] seedPos)
    {
        if(currentCropField == null)
            return;
        
        currentCropField.SeedCollidedCallback(seedPos);
    }





    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("CropField"))
        {
            playerAnimator.PlaySowAnimation(true);
            currentCropField=other.GetComponent<CropField>();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("CropField"))
        {
            playerAnimator.PlaySowAnimation(false);
            currentCropField =null;
        }
    }




   

}
