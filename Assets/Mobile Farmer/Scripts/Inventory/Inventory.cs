using System.Collections;
using System.Collections.Generic;
using jy_util;
using UnityEngine;

public class Inventory 
{
   [SerializeField] List<InventoryItem> items= new List<InventoryItem>();

   public void OnCropHervestedCallback(CropData cropData)
   {
        
        bool cropFound=false;

        foreach(InventoryItem item in items)
        {
            if(cropData.cropType==item.crop_Type)
            {
                cropFound=true;
                item.amount+=cropData.amountinSingleCrop;
                break;
            }
        }

        if(cropFound) return;

        //create new Item for inventroy as its a new type of crop added to inventory
        items.Add(new InventoryItem(cropData.cropType,cropData.amountinSingleCrop));

   }
    
    public void DebugInventory()
    {
        foreach(InventoryItem item in items)
        {
            Debug.Log("We have "+item.amount+" items in our "+item.crop_Type+"list");
        }
    }

    public InventoryItem[] GetInventoryItems()
    {
        return items.ToArray();
    }

    public void ClearInventory()
    {
        items.Clear();
    }
}
