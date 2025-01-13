using System.Collections;
using System.Collections.Generic;
using System.IO;
using jy_util;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
   private Inventory inventory;
   private string dataPath="";
    void Start()
    {
        dataPath = Application.dataPath + "/inventoryData.txt";
        LoadInventory();
        SubcribeEvent(true);
    }
    private void OnDestroy()
    {
        SubcribeEvent(false);
    }

    void SubcribeEvent(bool suncribe)
    {
        if(suncribe)
        {
            CropTile.OnCropHervestedEvent+=OnCropHervestedCallback;
        }else{
            CropTile.OnCropHervestedEvent-=OnCropHervestedCallback;
        }
    }



    private void OnCropHervestedCallback(CropData cropData)
    {
        inventory.OnCropHervestedCallback(cropData);
        SaveInventory();
    }

    [NaughtyAttributes.Button]
    public void DebugInventory()=>inventory.DebugInventory();

   

   private void LoadInventory()
   {
        
        string data;
        if(File.Exists(dataPath))
        {
            data = File.ReadAllText(dataPath);
            inventory = JsonUtility.FromJson<Inventory>(data);
            if(inventory == null)
                inventory = new Inventory();
        }else{
            File.Create(dataPath);
            inventory = new Inventory();
        }
        
   }

   private void SaveInventory()
   {
        string data = JsonUtility.ToJson(inventory,true);
        File.WriteAllText(dataPath,data);
   }
}
