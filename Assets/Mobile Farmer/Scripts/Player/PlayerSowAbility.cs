using System.Collections;
using System.Collections.Generic;
using jy_util;

using UnityEngine;

[RequireComponent(typeof(PlayerAnimator))]
public class PlayerSowAbility : MonoBehaviour
{
    private PlayerAnimator playerAnimator;
    private PlayerToolSelector playerToolSelector;


    private CropField currentCropField;
    private bool isShowing=false,insideCropField=false;

    void Start()
    {
        playerAnimator = _GameAssets.Instance.playerAnimator;
        playerToolSelector=_GameAssets.Instance.playerToolSelector;
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
            currentCropField=other.GetComponent<CropField>();
            EnterCropfield(currentCropField);
        }
    }
    // private void OnTriggerStay(Collider other)
    // {
    //     if(other.CompareTag("CropField"))
    //         EnterCropfield(other.GetComponent<CropField>());
    // }
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

    private void EnterCropfield(CropField cropField)
    {

        if(playerToolSelector.CanSow() && cropField.IsEmpty())
            playerAnimator.PlaySowAnimation(true);
        
    }

   public void ListenToPlayerChangeTool(Component sender,object data)
   {
        E_Tool selectedTool=(E_Tool)data;

        // if(!playerToolSelector.CanSow())
        //     playerAnimator.PlaySowAnimation(false);
        if(currentCropField==null) return;
        playerAnimator.PlaySowAnimation(playerToolSelector.CanSow() && currentCropField.IsEmpty());
   }

}
