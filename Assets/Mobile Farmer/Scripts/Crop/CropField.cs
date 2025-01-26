using System.Collections;
using System.Collections.Generic;
using jy_util;
using UnityEngine;
using System;
using Unity.VisualScripting;


public class CropField : MonoBehaviour,IInteractable
{
    [Header("Elements")]
    [SerializeField] Transform tilesParent;
    private List<CropTile> cropTiles= new List<CropTile>();
    [SerializeField] ButtonInfo interactButtonData_Sow,interactButtonData_Water,interactButtonData_Hervest;

    [Header("Settings")]
    [SerializeField] CropData cropData;

    private int tileSown=0,tileWatered=0,tileHarvested=0;
    private E_Crop_State state;
    private GameObject interactingObject;
    private PlayerDataHolder playerDataHolder;

    [Header("Action")]
    public static Action<CropField> onFullySown,onFullyWatered,OnFullyHarvested;


    void Start()
    {
        state=E_Crop_State.Empty;
        StoreTile();
    }


    void StoreTile()
    {
        for(int i=0;i<tilesParent.childCount;i++)
            cropTiles.Add(tilesParent.GetChild(i).GetComponent<CropTile>());
    }
   
    #region Particel Callbacks
    public void SeedCollidedCallback(Vector3[] seedPos)
    {
        //go through all collided particel and sow seed in collided crop tile
        for(int i=0;i<seedPos.Length;i++)
        {
            
            CropTile closestCropTile=GetClosestCropTile(seedPos[i]);
            if(closestCropTile==null)
                continue;

            if(!closestCropTile.IsEmpty())
                continue;

            closestCropTile.Sow(cropData);
            tileSown++;

            if(tileSown>=cropTiles.Count)
                FieldFullySown();
            
        }
    }
    public void WaterCollidedCallBack(Vector3[] waterPos)
    {
        //go through all collided particel and water particel in collided crop tile
        for(int i=0;i<waterPos.Length;i++)
        {
            
            CropTile closestCropTile=GetClosestCropTile(waterPos[i]);
            if(closestCropTile==null)
                continue;

            if(!closestCropTile.IsSown())
                continue;

            closestCropTile.Water(cropData);
            tileWatered++;

            if(tileWatered>=cropTiles.Count)
                FieldFullyWatered();
            
        }
    }
    #endregion
    private CropTile GetClosestCropTile(Vector3 particelPos)
    {
        float minDistance=5000f;
        int closesCroptileIndex=-1;

        for(int i=0;i<cropTiles.Count;i++)
        {
            CropTile temp_holding_cropTile=cropTiles[i];
            float disanceTileSeed=Vector3.Distance(temp_holding_cropTile.transform.position,particelPos);
            if(disanceTileSeed < minDistance)
            {
                minDistance=disanceTileSeed;
                closesCroptileIndex=i;
            }
        }


        if(closesCroptileIndex==-1) return null;

        return cropTiles[closesCroptileIndex];
    }


    #region Operation on TILEs
    private void Sow(CropTile cropTile)
    {
        cropTile.Sow(cropData);
    }
    private void Water(CropTile cropTile)
    {
        cropTile.Water(cropData);
    }
    private void Harvest(CropTile cropTile)
    {
        cropTile.Harvest(cropData);

        tileHarvested++;

        if(tileHarvested>=cropTiles.Count)
            FieldFullyHarvested();
        
    }
    #endregion
    #region Operation on FIELD
    private  void FieldFullySown()
    {
        state= E_Crop_State.Sown;
        OnCompleteOnStep();
        onFullySown?.Invoke(this);
    }

    private void FieldFullyWatered()
    {
        state = E_Crop_State.Watered;
        OnCompleteOnStep();
        onFullyWatered?.Invoke(this);
    }

    private void FieldFullyHarvested()
    {
        state =  E_Crop_State.Empty;
        OnCompleteOnStep();
        OnFullyHarvested?.Invoke(this);

        tileSown = 0;
        tileWatered = 0;
        tileHarvested = 0;
        
    }

    public void Harvest(Transform harvestSphere)
    {
        float radius=harvestSphere.localScale.x;

        for(int i=0;i<cropTiles.Count;i++)
        {
            if(cropTiles[i].IsEmpty()) //already harvested
                continue;

            float distanceCropSphere = Vector3.Distance(cropTiles[i].transform.position,harvestSphere.position);
            if(distanceCropSphere < radius)
                Harvest(cropTiles[i]);
        }
    }
    #endregion
    public bool IsEmpty()
    {
        return state==E_Crop_State.Empty;
    }

    public bool IsSown()
    {
        return state == E_Crop_State.Sown;
    }
    public bool IsWatered()
    {
        return state==E_Crop_State.Watered;
    }
   
    

    #region UTILITY INSPECTIOR BUTTONS
    [NaughtyAttributes.Button]
    private void InstantlySowTile()
    {
        for(int i=0;i<cropTiles.Count;i++)
            cropTiles[i].Sow(cropData);
        FieldFullySown();
    }
    [NaughtyAttributes.Button]
    private void InstantlyWaterTile()
    {
        for(int i=0;i<cropTiles.Count;i++)
            cropTiles[i].Water(cropData);
        FieldFullyWatered();
    }
    #endregion
    #region Interface
    public void Interact(GameObject interactingObject)
    {
        this.interactingObject = interactingObject;
        playerDataHolder = interactingObject.GetComponent<PlayerDataHolder>();
        UIManager.Instance.SetupIntreactButton(interactButtonData_Sow,false);

        switch(state)
        {
            case E_Crop_State.Empty:
                SeedParticle.onSeedCollided += SeedCollidedCallback;
                PlayerSowField(playerDataHolder.playerAnimator);
                break;
            case E_Crop_State.Sown:
                WaterParticle.onWaterCollided += WaterCollidedCallBack;
                PlayerWaterField(playerDataHolder.playerAnimator);
                break;
            case E_Crop_State.Watered:
                PlayerAnimationEvents.startHarvestCallBackEvent += StartHervest;
                PlayerHervestField(playerDataHolder.playerAnimator);
                
                break;
        }

        if(interactingObject.CompareTag("Player"))
            _GameAssets.Instance.OnPlayerInteractStatusChangeEvent.Raise(this,true);
    }

    public void InIntreactZone()
    {
        switch(state)
        {
            case E_Crop_State.Empty:
                UIManager.Instance.SetupIntreactButton(interactButtonData_Sow,true);
                break;
            case E_Crop_State.Sown:
                UIManager.Instance.SetupIntreactButton(interactButtonData_Water,true);
                break;
            case E_Crop_State.Watered:
                UIManager.Instance.SetupIntreactButton(interactButtonData_Hervest,true);
                break;
        }
    }

    public void OutIntreactZone()
    {
        UIManager.Instance.SetupIntreactButton(interactButtonData_Sow,false);
        if(interactingObject == null) return;

        
        if(interactingObject.CompareTag("Player"))
            _GameAssets.Instance.OnPlayerInteractStatusChangeEvent.Raise(this,false);
        interactingObject = null;

        switch(state)
        {
            case E_Crop_State.Empty:
                playerDataHolder?.playerAnimator.PlaySowAnimation(false);
                SeedParticle.onSeedCollided -= SeedCollidedCallback;
                break;
            case E_Crop_State.Sown:
                playerDataHolder.playerAnimator.PlayeWaterAnimation(false);
                WaterParticle.onWaterCollided -= WaterCollidedCallBack;
                break;
            case E_Crop_State.Watered:
                playerDataHolder.playerAnimator.PlayerHarvestAnimation(false);
                PlayerAnimationEvents.startHarvestCallBackEvent -= StartHervest;
                break;
        }
    }
    #region Player ANIMATIONS
    void PlayerSowField(PlayerAnimator animator)
    {
        if(animator == null)
        {
            Debug.LogError("Null Playeranimator faild to sow");
            return;
        }
        animator.PlaySowAnimation(true);
    }

    void PlayerWaterField(PlayerAnimator animator)
    {
        if(animator == null)
        {
            Debug.LogError("Null Playeranimator faild to water");
            return;
        }
        animator.PlayeWaterAnimation(true);
    }

    void PlayerHervestField(PlayerAnimator animator)
    {
        if(animator == null)
        {
            Debug.LogError("Null Playeranimator faild to Hervest");
            return;
        }
        animator.PlayerHarvestAnimation(true);
    }
    #endregion
    
    private void StartHervest()
    {
        Harvest(playerDataHolder.hervestSphere);
    }

    
    void OnCompleteOnStep()
    {
        if(interactingObject == null) return;
        InIntreactZone();
        if(interactingObject.CompareTag("Player"))
            _GameAssets.Instance.OnPlayerInteractStatusChangeEvent.Raise(this,false);
        playerDataHolder.playerAnimator.StopAllLayeredAnimation();
    }
    #endregion


}
