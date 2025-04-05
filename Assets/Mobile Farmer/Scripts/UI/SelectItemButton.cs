using System.Collections;
using System.Collections.Generic;
using jy_util;
using UnityEngine;
using UnityEngine.UI;


public class SelectItemButton : MonoBehaviour
{
    public Image image;
    public E_Inventory_Item_Type item_type;
    public GameEvent OnSelectShareItemButtonPressed;
    
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
        image.sprite = icon;
        item_type = e_Inventory_Item_Type;
    }

    public void OnButtonPressed()
    {
        OnSelectShareItemButtonPressed.Raise(this,item_type);
    }
}
