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
        //suncribe to event
        SeedParticle.onSeedCollided+=SeedCollidedCallback;
        CropField.onFullySown+=CropFieldFullySownCallback;
    }
    void OnDestroy()
    {
        //unsubscribe to events
        SeedParticle.onSeedCollided -=SeedCollidedCallback;
        CropField.onFullySown-=CropFieldFullySownCallback;
    }




    private void SeedCollidedCallback(Vector3[] seedPos)
    {
        if(currentCropField == null)
            return;
        
        currentCropField.SeedCollidedCallback(seedPos);
    }





    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("CropField") && other.GetComponent<CropField>().IsEmpty())
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


    private void CropFieldFullySownCallback(CropField cropField)
    {
        if(cropField ==  currentCropField)
            playerAnimator.PlaySowAnimation(false);
    }



   

}
