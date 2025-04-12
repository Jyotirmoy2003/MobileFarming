using System.Collections;
using System.Collections.Generic;
using jy_util;
using UnityEngine;
using System;




public class CropField : MonoBehaviour,IInteractable
{
    [Header("Elements")]
    public CropFieldDataHolder cropFieldDataHolder;
    [SerializeField] CropFieldInfoUI infoUI;

    [SerializeField] Transform tilesParent;
    internal List<CropTile> cropTiles= new List<CropTile>();
    [SerializeField] ButtonInfo interactButtonData_Sow,interactButtonData_Water,interactButtonData_Hervest;

    [Header("Settings")]
    [SerializeField] CropData cropData;
    //[HideInInspector]
    public bool IsOccupied= false;

    private int tileSown=0,tileWatered=0,tileHarvested=0;
    public E_Crop_State state{get;private set;}
    private List<GameObject> interactingObjects = new List<GameObject>();
    private List<PlayerDataHolder> playerDataHolders = new List<PlayerDataHolder>();

    [Header("Action")]
    public Action<CropField> onFullySown,onFullyWatered,OnFullyHarvested;
    private bool isJustMerged = false;


    void Start()
    {
        OnFullyHarvested = null;
        onFullySown = null;
        onFullyWatered = null;
        
        state=E_Crop_State.Empty;
        infoUI.Initialize(cropData);
        StoreTile();
    }


    void StoreTile()
    {
        cropTiles.Clear();
        for(int i=0;i<tilesParent.childCount;i++)
            cropTiles.Add(tilesParent.GetChild(i).GetComponent<CropTile>());

        
    }

    public void MergeDone(bool calledByChunkUnlocking)
    {
        StoreTile();
       if(calledByChunkUnlocking && playerDataHolders.Count>0)MergeStateSet();
        
    }
    void MergeStateSet()
    {
        isJustMerged = true;
        switch(state)
        {
            case E_Crop_State.Empty:
                InstantlySowTile();
                break;
            case E_Crop_State.Sown:
                InstantlySowTile();
                //tileSown += 6;
                break;
            case E_Crop_State.Watered:
                InstantlyWaterTile();
                //tileWatered += 6;
                break;

        }
    }

   


    private void OnDisable()
    {
        OnFullyHarvested = null;
        onFullySown = null;
        onFullyWatered = null;
    }
   
    #region Particel Callbacks
    public void SeedCollidedCallback(Vector3[] seedPos,PlayerDataHolder ownerData)
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
            if(ownerData.isPlayer) HapticManager.Instance.LightHaptic();

            if(tileSown>=cropTiles.Count)
                FieldFullySown();
            
        }
    }
    public void WaterCollidedCallBack(Vector3[] waterPos,PlayerDataHolder ownerData)
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
            if(ownerData.isPlayer) HapticManager.Instance.LightHaptic();

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
            if(temp_holding_cropTile == null) continue; //Handel by any chance null ref
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
    private void Harvest(CropTile cropTile,PlayerDataHolder owningPlayer)
    {
        cropTile.Harvest(owningPlayer,cropData);

        tileHarvested++;

        if(tileHarvested>=cropTiles.Count)
            FieldFullyHarvested();
        
    }
    #endregion
    #region Operation on FIELD
    private  void FieldFullySown()
    {
        state= E_Crop_State.Sown;
        tileSown = cropTiles.Count;

        if(isJustMerged)foreach(CropTile cropTile in cropTiles) cropTile.ForceSow(cropData);
        OnCompleteOnStep();
        onFullySown?.Invoke(this);
    }

    private void FieldFullyWatered()
    {
        state = E_Crop_State.Watered;
        tileWatered = cropTiles.Count;

        if(isJustMerged)foreach(CropTile cropTile in cropTiles) cropTile.ForceWater();

        OnCompleteOnStep();
        onFullyWatered?.Invoke(this);
    }

    private void FieldFullyHarvested()
    {
        state =  E_Crop_State.Empty;

        if(isJustMerged)foreach(CropTile cropTile in cropTiles) cropTile.ForceHervest();

        OnCompleteOnStep();
        OnFullyHarvested?.Invoke(this);

        tileSown = 0;
        tileWatered = 0;
        tileHarvested = 0;

        isJustMerged = false;
        
    }

    public void Harvest(Transform harvestSphere,PlayerDataHolder owningPlayer)
    {
        float radius=harvestSphere.localScale.x;

        for(int i=0;i<cropTiles.Count;i++)
        {
            if(cropTiles[i].IsEmpty()) //already harvested
                continue;

            float distanceCropSphere = Vector3.Distance(cropTiles[i].transform.position,harvestSphere.position);
            if(distanceCropSphere < radius)
                Harvest(cropTiles[i],owningPlayer);
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
    public CropData GetCropData()
    {
        return cropData;
    }

    
    
    public void AnimateCropTileCallback()
    {
        StartCoroutine(CropTileFeedback());
    }
    IEnumerator CropTileFeedback()
    {
        foreach(CropTile item in cropTiles)
        {
            item.feedBackManager?.PlayFeedback();
            yield return new WaitForSeconds(0.1f);
        }
    }
    

    #region UTILITY INSPECTIOR BUTTONS
    [NaughtyAttributes.Button]
    public void InstantlySowTile()
    {
        for(int i=0;i<cropTiles.Count;i++)
            cropTiles[i].Sow(cropData);
        FieldFullySown();
    }
    [NaughtyAttributes.Button]
    public void InstantlyWaterTile()
    {
        for(int i=0;i<cropTiles.Count;i++)
            cropTiles[i].Water(cropData);
        FieldFullyWatered();
    }
    #endregion
    #region Interface
    public void Interact(GameObject interactingObject)
    {
        if(interactingObject.CompareTag("Player"))
            UIManager.Instance.SetupIntreactButton(interactButtonData_Sow,false);
        
        
        PlayerDataHolder temp_dataholder = interactingObject.GetComponent<PlayerDataHolder>();

        switch(state)
        {
            case E_Crop_State.Empty:
                temp_dataholder.seedParticle.onSeedCollided += SeedCollidedCallback;
                PlayerSowField(temp_dataholder.playerAnimator);
                break;
            case E_Crop_State.Sown:
                temp_dataholder.waterParticle.onWaterCollided += WaterCollidedCallBack;
                PlayerWaterField(temp_dataholder.playerAnimator);
                break;
            case E_Crop_State.Watered:
                temp_dataholder.playerAnimationEvents.startHarvestCallBackEvent += StartHervest;
                PlayerHervestField(temp_dataholder.playerAnimator);
                
                break;
        }

        if(interactingObject.CompareTag("Player"))
            _GameAssets.Instance.OnPlayerInteractStatusChangeEvent.Raise(this,true);
    }

    public void InIntreactZone(GameObject interactingObject)
    {
        // if(!(this.interactingObject !=null && interactingObject == this.interactingObject))
        //     if(IsOccupied) return; //when someone already working on this field
        //     IsOccupied = true;
        
        if(!interactingObjects.Contains(interactingObject)) 
        {
            interactingObjects.Add(interactingObject);
            playerDataHolders.Add(interactingObject.GetComponent<PlayerDataHolder>());
        }


        infoUI.SetActivationStatus(false);
        infoUI.canChangeStatus = false; //now no other can activate this panel until its not occupied

        if(interactingObject.CompareTag("Player"))
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
    }

    public void OutIntreactZone(GameObject interactingObject)
    {
        if(interactingObjects.Contains(interactingObject)) interactingObjects.Remove(interactingObject);
        PlayerDataHolder temp_Dataholder = interactingObject.GetComponent<PlayerDataHolder>();
       

        //if(this.interactingObject != interactingObject) return;

        // if(!IsOccupied) return;
        // IsOccupied = false;
        infoUI.canChangeStatus = true;
        if(interactingObject.CompareTag("Player"))
        {
            _GameAssets.Instance.OnPlayerInteractStatusChangeEvent.Raise(this,false);
            UIManager.Instance.SetupIntreactButton(interactButtonData_Sow,false);
        }

    
        if(playerDataHolders.Contains(temp_Dataholder))
        {

        
            switch(state)
            {
                case E_Crop_State.Empty:
                    temp_Dataholder?.playerAnimator.PlaySowAnimation(false);
                    temp_Dataholder.seedParticle.onSeedCollided -= SeedCollidedCallback;
                    break;
                case E_Crop_State.Sown:
                    temp_Dataholder.playerAnimator.PlayeWaterAnimation(false);
                    temp_Dataholder.waterParticle.onWaterCollided -= WaterCollidedCallBack;
                    AudioManager.instance.StopSound("Water",this.gameObject);
                    break;
                case E_Crop_State.Watered:
                    temp_Dataholder.playerAnimator.PlayerHarvestAnimation(false);
                    temp_Dataholder.playerAnimationEvents.startHarvestCallBackEvent -= StartHervest;
                    break;
            }
        
            playerDataHolders.Remove(temp_Dataholder);
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
        AudioManager.instance.PlaySound("Water",this.gameObject);
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
    
    private void StartHervest(PlayerDataHolder owningPlayer)
    {
        //if(playerDataHolder == null) playerDataHolder = interactingObject.GetComponent<PlayerDataHolder>();
        Harvest(owningPlayer.hervestSphere,owningPlayer);
    }

    
    void OnCompleteOnStep()
    {
        for(int i=0 ; i<interactingObjects.Count ; i++)
        {
            if(interactingObjects[i] == null) return;
            InIntreactZone(interactingObjects[i]);

            if(interactingObjects[i].CompareTag("Player"))
                _GameAssets.Instance.OnPlayerInteractStatusChangeEvent.Raise(this,false);
            
            playerDataHolders[i].playerAnimator.StopAllLayeredAnimation();
            AudioManager.instance.StopSound("Water",this.gameObject);
        }
    }

    public void ShowInfo(bool val)
    {
       infoUI.SetActivationStatus(val) ;
    }
    #endregion


}
