using System.Collections;
using System.Collections.Generic;
using jy_util;
using UnityEngine;

[System.Serializable]
public class InventoryItem 
{
    public  E_Inventory_Item_Type item_type;
    public int amount;

    public InventoryItem(E_Inventory_Item_Type item_type, int amount)
    {
        this.item_type = item_type;
        this.amount = amount;
    }
}
