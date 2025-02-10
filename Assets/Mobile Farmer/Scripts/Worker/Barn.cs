using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BarnInventory))]
public class Barn : MonoBehaviour,IInteractable
{
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
        InventoryManager.Instance.AddInventoryToInventory(barnInventory.GetInventory());
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






}
