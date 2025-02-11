using System;
using System.Collections;
using System.Collections.Generic;
using jy_util;
using UnityEngine;

[RequireComponent(typeof(BarnInventory))]
public class Barn : MonoBehaviour,IInteractable
{
    public Transform workerLoadOutPos;
    public List<BarnItem> barnCapableItem = new List<BarnItem>();
    public List<CropField> nearByFields = new List<CropField>();
    private BarnInventory barnInventory;
    public Action<E_Inventory_Item_Type> OnBarnFull;
    public Action OnBarnCollected;

    void  Start()
    {
        barnInventory = GetComponent<BarnInventory>();
    }
   
   public CropField GetUnlockedField(CropData cropData)
   {
        for(int i=0 ;i<nearByFields.Count;i++)
        {
            if(nearByFields[i].cropFieldDataHolder.chunk.IsUnclocked() && nearByFields[i].GetCropData()==cropData &&!nearByFields[i].cropFieldDataHolder.cropField.IsOccupied)
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
        
    }

    public void ShowInfo(bool val)
    {
        
    }
    #endregion


    void LoadInventoryToPlayer()
    {
        InventoryManager.Instance.AddInventoryToInventory(barnInventory.GetInventory());
        barnInventory.ClearInventory();
        OnBarnCollected?.Invoke(); //Fire event
    }
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Player"))
        {
            Debug.Log("PlayerCollided");
            LoadInventoryToPlayer();
        }
    }

    public void AddItemInInventory(E_Inventory_Item_Type item_Type,int amount)
    {
        int availableSpace = CheckForMaxload(item_Type);
        
        //check for spcae in barn
        if(availableSpace>=amount)
            barnInventory.AddItemToInventory(item_Type,amount);
        else if(availableSpace > 0) //Add only the amount of space available
            barnInventory.AddItemToInventory(item_Type,availableSpace);
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
           OnBarnFull?.Invoke(item_Type); //Fire Event
        }

        return availableSpace;
    }


}

[System.Serializable]
public struct BarnItem{
    public E_Inventory_Item_Type item_Type;
    public int maxLoadCapacity;
}
