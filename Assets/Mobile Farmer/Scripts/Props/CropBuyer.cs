using System.Collections;
using System.Collections.Generic;
using jy_util;
using UnityEngine;

public class CropBuyer : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField] List<CropData> canBuyCrops = new List<CropData>();
    [SerializeField] GameEvent SoldCropEvent;


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
            
            if(!CanBuy(item.crop_Type)) continue; //when current corp can not be sell to this seller
            //calculate Earning
            itemPrice = GetCropPrice(item.crop_Type);
            coinsEarned += itemPrice*item.amount;
            inventory.RemoveItem(item);
        }
        //only fire the event if player actually sold something
        if(coinsEarned <=0) return;
        SoldCropEvent.Raise(this,coinsEarned);
        TransactionEffectManager.Instance.PlayeCoinParticel(coinsEarned);
    }

    private int GetCropPrice(E_Crop_Type e_Crop_Type)
    {
        foreach(CropData item in _GameAssets.Instance.cropDatas)
            if(item.cropType==e_Crop_Type)
                return item.pricePerPice;

        
        Debug.LogError("No corrosponding crop price found!!");
        return 0;
        
    }

    private bool CanBuy(E_Crop_Type e_Crop_Type)
    {
        foreach(CropData item in canBuyCrops)
            if(item.cropType == e_Crop_Type) return true;
            return false;
    }
}
