using System.Collections;
using System.Collections.Generic;
using jy_util;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimator))]
public class PlayerWaterAbility : MonoBehaviour
{
    
    private PlayerAnimator playerAnimator;
    private PlayerToolSelector playerToolSelector;


    private CropField currentCropField;
    private bool isShowing=false,insideCropField=false;

    void Start()
    {
        playerAnimator = GetComponent<PlayerAnimator>();
        playerToolSelector=GetComponent<PlayerToolSelector>();
        //suncribe to event
        WaterParticle.onWaterCollided+=WaterCollidedCallBack;
        //CropField.onFullySown+=CropFieldFullySownCallback;
    }
    void OnDestroy()
    {
        //unsubscribe to events
        WaterParticle.onWaterCollided -=WaterCollidedCallBack;
        //CropField.onFullySown-=CropFieldFullySownCallback;
    }




    private void WaterCollidedCallBack(Vector3[] waterPos)
    {
        if(currentCropField == null)
            return;
        
        currentCropField.WaterCollidedCallBack(waterPos);
    }





    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("CropField") && other.GetComponent<CropField>().IsSown())
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
            playerAnimator.PlayeWaterAnimation(false);
            currentCropField =null;
        }
    }


    private void CropFieldFullyWateredCallback(CropField cropField)
    {
        if(cropField ==  currentCropField)
            playerAnimator.PlayeWaterAnimation(false);
    }

    private void EnterCropfield(CropField cropField)
    {

        if(playerToolSelector.CanWater() && cropField.IsSown())
            playerAnimator.PlayeWaterAnimation(true);
        
    }

   public void ListenToPlayerChangeTool(Component sender,object data)
   {
        E_Tool selectedTool=(E_Tool)data;
        playerAnimator.PlayeWaterAnimation(playerToolSelector.CanWater());
   }

}
