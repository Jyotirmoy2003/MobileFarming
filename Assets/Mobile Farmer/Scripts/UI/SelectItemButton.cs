using System.Collections;
using System.Collections.Generic;
using jy_util;
using UnityEngine;


public class SelectItemButton : MonoBehaviour
{

    public E_Inventory_Item_Type item_type;
    
    public SelectItemButton(E_Inventory_Item_Type e_Inventory_Item_Type)
    {
        item_type = e_Inventory_Item_Type;
    }

    public SelectItemButton()
    {
        item_type = E_Inventory_Item_Type.None;
    }

    public void Configure(Sprite icon,E_Inventory_Item_Type e_Inventory_Item_Type)
    {
        item_type = e_Inventory_Item_Type;
    }

   
}
