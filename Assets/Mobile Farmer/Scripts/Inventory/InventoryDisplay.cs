using System.Collections;
using System.Collections.Generic;
using jy_util;
using UnityEngine;

public class InventoryDisplay : MonoBehaviour
{
   [Header("Elements")]
   [SerializeField] Transform cropContainerParent;
   [SerializeField] UICropContainer uICropContainerPrefab;
    void Start()
    {
        
    }

    public void Configure(Inventory inventory)
    {
        InventoryItem[] items = inventory.GetInventoryItems();


        foreach (InventoryItem item in items)
        {
            UICropContainer cropContainerInstance = Instantiate(uICropContainerPrefab, cropContainerParent);
            cropContainerInstance.Configure(GetCropIcon(item.crop_Type),item.amount);
        }
    }

    private Sprite GetCropIcon(E_Crop_Type e_Crop_Type)
    {
        foreach(CropData item in _GameAssets.Instance.cropDatas)
        {
            if(item.cropType == e_Crop_Type)
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
                uICropContainerInstance.Configure(GetCropIcon(items[i].crop_Type),items[i].amount);

            }else{
                //when there is not enough container create new
                uICropContainerInstance = Instantiate(uICropContainerPrefab, cropContainerParent);
                uICropContainerInstance.Configure(GetCropIcon(items[i].crop_Type),items[i].amount);
            }
            
        }


        int remaningContainer = cropContainerParent.childCount - items.Length;

        if(remaningContainer<=0) return;


        //Deactivate all other active container
        for(int i=0;i<remaningContainer;i++)
            cropContainerParent.GetChild(items.Length+i).gameObject.SetActive(false);
    }
   
}
