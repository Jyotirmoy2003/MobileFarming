using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using jy_util;

[RequireComponent(typeof(PlayerAnimator))]
public class PlayerHarvestAbility : MonoBehaviour
{
   private PlayerAnimator playerAnimator;
    private PlayerToolSelector playerToolSelector;
    [SerializeField] Transform harvestSphere;


    private CropField currentCropField;
    private bool isShowing=false,insideCropField=false;

    void Start()
    {
        playerAnimator = _GameAssets.Instance.playerAnimator;
        playerToolSelector=_GameAssets.Instance.playerToolSelector;
        //suncribe to event
        CropField.OnFullyHarvested+=CropFieldFullyHarvested;
    }
    void OnDestroy()
    {
        //unsubscribe to events
        CropField.OnFullyHarvested-=CropFieldFullyHarvested;
    }




  





    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("CropField") )
        {
            currentCropField=other.GetComponent<CropField>();
            EnterCropfield(currentCropField);
        }
    }
  
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("CropField"))
        {
            playerAnimator.PlayerHarvestAnimation(false);
            currentCropField =null;
        }
    }


    private void CropFieldFullyHarvested(CropField cropField)
    {
        Debug.Log("Crop Field Fully Harvested");
        if(cropField ==  currentCropField)
            playerAnimator.PlayerHarvestAnimation(false);
    }

    private void EnterCropfield(CropField cropField)
    {

        if(playerToolSelector.CanHarvest() && cropField.IsWatered())
            playerAnimator.PlayerHarvestAnimation(true);
        
    }

   public void ListenToPlayerChangeTool(Component sender,object data)
   {
        E_Tool selectedTool=(E_Tool)data;

        if(currentCropField==null) return;
            playerAnimator.PlayerHarvestAnimation((playerToolSelector.CanHarvest() && currentCropField.IsWatered()));
   }










   public void ListenToStartHarvest()
   {
        if(currentCropField!=null & currentCropField.IsWatered())
        currentCropField.Harvest(harvestSphere);
   }

   public void ListenToStopHarvest()
   {

   }

}
