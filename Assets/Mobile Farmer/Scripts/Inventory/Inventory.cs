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
            if(cropData.item_type==item.item_type)
            {
                cropFound=true;
                item.amount+=cropData.amountinSingleCrop;
                break;
            }
        }

        if(cropFound) return;

        //create new Item for inventroy as its a new type of crop added to inventory
        items.Add(new InventoryItem(cropData.item_type,cropData.amountinSingleCrop));

   }

    //called when add more then one crop in a single call
   public void AddItemToInventory(E_Inventory_Item_Type itemType,int amount)
   {
        bool cropFound=false;

        foreach(InventoryItem item in items)
        {
            if(itemType==item.item_type)
            {
                cropFound=true;
                item.amount+=amount;
                break;
            }
        }

        if(cropFound) return;

        //create new Item for inventroy as its a new type of crop added to inventory
        items.Add(new InventoryItem(itemType,amount));
   }
    
   

    public InventoryItem[] GetInventoryItems()
    {
        return items.ToArray();
    }

    public void ClearInventory()
    {
        items.Clear();
    }

    public void RemoveAllItem(InventoryItem item)
    {
        items.Remove(item);
    }
}
