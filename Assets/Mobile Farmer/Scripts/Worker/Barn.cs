using System;
using System.Collections;
using System.Collections.Generic;
using jy_util;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BarnInventory))]
public class Barn : MonoBehaviour,IInteractable
{
    public Transform workerLoadOutPos;
    public List<BarnItem> barnCapableItem = new List<BarnItem>();
    public List<CropField> nearByFields = new List<CropField>();
    [SerializeField] List<workerStat> workerStats = new List<workerStat>();
    [Header("UI ref")]
    public InfoUI infoUI;
    public List<StorageUIStatus> storageUIStatuses = new List<StorageUIStatus>();


    private BarnInventory barnInventory;
    public Action<E_Inventory_Item_Type> OnBarnFull;
    public Action OnBarnCollected;
    private bool isBarnEmpty = true;
    








    void  Start()
    {
        barnInventory = GetComponent<BarnInventory>();
        Init();
    }

    void Init()
    {
        for(int i=0;i<storageUIStatuses.Count;i++)
            storageUIStatuses[i].slider.maxValue = barnCapableItem[i].maxLoadCapacity;
        
        
        
        Invoke(nameof(UpdateUiDisplay),1f);
    }
   
   public CropField GetUnlockedField(CropData cropData)
   {
        for(int i=0 ;i<nearByFields.Count;i++)
        {
            if(nearByFields[i].cropFieldDataHolder.chunk.IsUnclocked() && nearByFields[i].GetCropData()==cropData && !nearByFields[i].cropFieldDataHolder.cropField.IsOccupied)
                return nearByFields[i];
        }
       return nearByFields[0];
   }





    #region  INTERFACE
    public void InIntreactZone(GameObject interactingObject)
    {
        
    }

    public void Interact(GameObject interactingObject)
    {
        
    }

    public void OutIntreactZone(GameObject interactingObject)
    {
        SubcribeToUiButton(false);
    }

    public void ShowInfo(bool val)
    {
        infoUI.SetActivationStatus(val);
    }
    #endregion

    void UpdateUiDisplay()
    {
        Inventory temp_Inventory = barnInventory.GetInventory();
        int temp_ItemAmount = 0;

        for(int i=0;i<storageUIStatuses.Count;i++)
        {
            storageUIStatuses[i].icon.sprite = _GameAssets.Instance.GetCropData(storageUIStatuses[i].Item_Type).uiIconSprite;
            temp_ItemAmount = temp_Inventory.GetItemAmountInInventory(storageUIStatuses[i].Item_Type);

            if(temp_ItemAmount >= barnCapableItem[i].maxLoadCapacity)
            {
                //the item is on full capacity
                storageUIStatuses[i].text.text = "Full!";
            }else{
                storageUIStatuses[i].text.text = temp_ItemAmount+"/"+barnCapableItem[i].maxLoadCapacity;
            }
            
            storageUIStatuses[i].slider.value = temp_ItemAmount;

        }
    }
    void LoadInventoryToPlayer()
    {
        if(!isBarnEmpty)
        {
            InventoryManager.Instance.AddInventoryToInventory(barnInventory.GetInventory());
            barnInventory.ClearInventory();
            OnBarnCollected?.Invoke(); //Fire event
            
            isBarnEmpty = true;
            UpdateUiDisplay();
        }else{
            BarnUImanager.Instance.ShowWorkerData(workerStats);
            SubcribeToUiButton(true);
        }
    }
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Player"))
        {
            LoadInventoryToPlayer();
        }
    }

    public void AddItemInInventory(E_Inventory_Item_Type item_Type,int amount)
    {
        isBarnEmpty = false;
        int availableSpace = CheckForMaxload(item_Type);
        
        //check for spcae in barn
        if(availableSpace>=amount)
            barnInventory.AddItemToInventory(item_Type,amount);
        else if(availableSpace > 0) //Add only the amount of space available
            barnInventory.AddItemToInventory(item_Type,availableSpace);
        //Update slider UI
        UpdateUiDisplay();
    }

    [NaughtyAttributes.Button]
    void DebguCheckLoad()
    {
        CheckForMaxload(E_Inventory_Item_Type.Corn);
    }
    int CheckForMaxload(E_Inventory_Item_Type item_Type)
    {
        int maxCap = 1;
        foreach(BarnItem barnItem in barnCapableItem)
        {
            if(item_Type == barnItem.item_Type)
            {
                maxCap = barnItem.maxLoadCapacity;
            }
        }
        int availableSpace = maxCap-barnInventory.GetInventory().GetItemAmountInInventory(item_Type);
        if(availableSpace <= 0)
        {
           OnBarnFull?.Invoke(item_Type); //Fire Event when BarnFilled
        }

        return availableSpace;
    }



    void SubcribeToUiButton(bool val)
    {
        if(val)
        {
            BarnUImanager.Instance.hireButtonPressed += OnHireButtonPressed;
        }else
            BarnUImanager.Instance.hireButtonPressed -= OnHireButtonPressed;
    }

    void Hireworker(int index)
    {
        switch(index)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
        }
    }

    void OnHireButtonPressed(int index)
    {
        
    }
}

