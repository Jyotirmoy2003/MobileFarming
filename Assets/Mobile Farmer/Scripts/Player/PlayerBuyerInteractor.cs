using System.Collections;
using System.Collections.Generic;
using jy_util;
using UnityEngine;

public class PlayerBuyerInteractor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Buyer"))
        {
            SellCrops();
        }
    }

    void SellCrops()
    {
        Inventory inventory = _GameAssets.Instance.inventoryManager.GetInventory();

        InventoryItem[] items = inventory.GetInventoryItems();

        int coinsEarned =0;
        int itemPrice = 0;
        foreach(InventoryItem item in items)
        {
            //calculate Earning
            itemPrice = GetCropPrice(item.crop_Type);
            coinsEarned += itemPrice*item.amount;
        }

        TransactionEffectManager.Instance.PlayeCoinParticel(coinsEarned);
        //CashManager.Instance.CreditCoins(coinsEarned);
        //clear inventory
        _GameAssets.Instance.inventoryManager.ClearInventory();;
    }

    private int GetCropPrice(E_Crop_Type e_Crop_Type)
    {
        foreach(CropData item in _GameAssets.Instance.cropDatas)
            if(item.cropType==e_Crop_Type)
                return item.pricePerPice;

        
        Debug.LogError("No corrosponding crop price found!!");
        return 0;
        
    }
}
