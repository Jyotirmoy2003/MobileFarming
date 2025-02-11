using System.Collections;
using System.Collections.Generic;
using jy_util;
using Unity.Loading;
using UnityEngine;

[RequireComponent(typeof(BarnInventory))]
public class Barn : MonoBehaviour,IInteractable
{
    public Transform workerLoadOutPos;
    public List<CropField> nearByFields = new List<CropField>();
    private BarnInventory barnInventory;

    void  Start()
    {
        barnInventory = GetComponent<BarnInventory>();
    }
   
   public CropField GetUnlockedField(CropData cropData)
   {
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
        barnInventory.AddItemToInventory(item_Type,amount);
    }




}
