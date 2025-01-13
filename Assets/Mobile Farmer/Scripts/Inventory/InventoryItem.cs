using System.Collections;
using System.Collections.Generic;
using jy_util;
using UnityEngine;

[System.Serializable]
public class InventoryItem 
{
    public  E_Crop_Type crop_Type;
    public int amount;

    public InventoryItem(E_Crop_Type crop_Type, int amount)
    {
        this.crop_Type = crop_Type;
        this.amount = amount;
    }
}
