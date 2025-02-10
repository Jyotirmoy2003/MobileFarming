using System.Collections;
using System.Collections.Generic;
using jy_util;
using UnityEngine;

public class InventoryDisplay : MonoBehaviour
{
   [Header("Elements")]
   [SerializeField] Transform cropContainerParent;
   [SerializeField] UICropContainer uICropContainerPrefab;
  



  

    public void Configure(Inventory inventory)
    {
        InventoryItem[] items = inventory.GetInventoryItems();


        foreach (InventoryItem item in items)
        {
            UICropContainer cropContainerInstance = Instantiate(uICropContainerPrefab, cropContainerParent);
            cropContainerInstance.Configure(GetItemIcon(item.item_type),item.amount);
        }
    }

    private Sprite GetItemIcon(E_Inventory_Item_Type item_type)
    {
        foreach(CropData item in _GameAssets.Instance.cropDatas)
        {
            if(item.item_type == item_type)
            {
                return item.uiIconSprite;

            }
        }

        return null;
    }


    public void UpdateDisplay(Inventory inventory)
    {
        InventoryItem[] items = inventory.GetInventoryItems();

        UICropContainer uICropContainerInstance;
        for(int i=0;i<items.Length;i++)
        {
            if(i < cropContainerParent.childCount)
            {
                //use preiviousely created containers
                uICropContainerInstance = cropContainerParent.GetChild(i).GetComponent<UICropContainer>();
                uICropContainerInstance.gameObject.SetActive(true);
                //Update just the amount text
                uICropContainerInstance.Configure(GetItemIcon(items[i].item_type),items[i].amount);

            }else{
                //when there is not enough container create new
                uICropContainerInstance = Instantiate(uICropContainerPrefab, cropContainerParent);
                uICropContainerInstance.Configure(GetItemIcon(items[i].item_type),items[i].amount);
            }
            
        }


        int remaningContainer = cropContainerParent.childCount - items.Length;

        if(remaningContainer<=0) return;


        //Deactivate all other active container
        for(int i=0;i<remaningContainer;i++)
            cropContainerParent.GetChild(items.Length+i).gameObject.SetActive(false);
    }
   
}
