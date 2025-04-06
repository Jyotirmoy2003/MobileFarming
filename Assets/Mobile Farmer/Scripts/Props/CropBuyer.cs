using System.Collections;
using System.Collections.Generic;
using jy_util;
using UnityEngine;

public class CropBuyer : MonoBehaviour,IInteractable
{
    [Header("Ref")]
    [SerializeField] List<CropData> canBuyCrops = new List<CropData>();
    [SerializeField] GameEvent SoldCropEvent;
    [SerializeField] InfoUI infoUI;


    private void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Player"))
        {
            SellItems();
        }
    }


    void SellItems()
    {
        Inventory inventory = _GameAssets.Instance.inventoryManager.GetInventory();

        InventoryItem[] items = inventory.GetInventoryItems();
        int coinsEarned =0;
        int itemPrice = 0;
        foreach(InventoryItem item in items)
        {
            
            if(!CanBuy(item.item_type)) continue; //when current corp can not be sell to this seller
            //calculate Earning
            itemPrice = GetCropPrice(item.item_type);
            coinsEarned += itemPrice*item.amount;
            inventory.RemoveAllItem(item);
        }
        //only fire the event if player actually sold something
        if(coinsEarned <=0) return;
        SoldCropEvent.Raise(this,coinsEarned);
         
        TransactionEffectManager.Instance.PlayeCoinParticel(coinsEarned);
    }

    private int GetCropPrice(E_Inventory_Item_Type item_Type)
    {
        foreach(CropData item in _GameAssets.Instance.cropDatas)
            if(item.item_type==item_Type)
                return item.pricePerPice;

        
        Debug.LogError("No corrosponding crop price found!!");
        return 0;
        
    }

    private bool CanBuy(E_Inventory_Item_Type item_Type)
    {
        foreach(CropData item in canBuyCrops)
            if(item.item_type == item_Type) return true;
            return false;
    }

    #region INTERFACE
    public void Interact(GameObject interactingObject)
    {
        
    }

    public void InIntreactZone(GameObject interactingObject)
    {
        
    }

    public void OutIntreactZone(GameObject interactingObject)
    {
        
    }

    public void ShowInfo(bool val)
    {
        infoUI.SetActivationStatus(val);
    }
    #endregion
}
