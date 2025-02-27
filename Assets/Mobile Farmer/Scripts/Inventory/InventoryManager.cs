using System;
using jy_util;
using UnityEngine;

[RequireComponent(typeof(InventoryDisplay))]
public class InventoryManager : MonoSingleton<InventoryManager>
{
   private Inventory inventory;
   private InventoryDisplay inventoryDisplay;
   private string dataPath="";
    void Start()
    {
        dataPath = Application.persistentDataPath + "/inventoryData.txt";
        #if UNITY_EDITOR
        dataPath = Application.dataPath + "/inventoryData.txt";
        #endif
        
        LoadInventory();
        ConfigureInventoryDisplay();

    }
  

   

    public Inventory GetInventory()
    {
        return inventory;
    }

    private void ConfigureInventoryDisplay()
    {
        inventoryDisplay = GetComponent<InventoryDisplay>();
        inventoryDisplay.Configure(inventory);
    }

    private void OnCropHervestedCallback(CropData cropData)
    {
        //Update inventory data
        inventory.OnCropHervestedCallback(cropData);
        inventoryDisplay.UpdateDisplay(inventory);

        SaveInventory();
    }

    

    
    [NaughtyAttributes.Button]
    public void ClearInventory()
    {
        inventory.ClearInventory();
        inventoryDisplay.UpdateDisplay(inventory);

        SaveInventory();
    }
   

   private void LoadInventory()
   {
       inventory = SaveAndLoad.Load<Inventory>(dataPath);
       if(inventory == null)
       {
            inventory = new Inventory();
            SaveInventory();
       }
   }

   private void SaveInventory()
   {
        SaveAndLoad.Save<Inventory>(dataPath,inventory);
   }


    public void ListenToOnHervested(Component sender,object data)
    {
        if(sender is PlayerDataHolder && (sender as PlayerDataHolder).isPlayer) //event fired when its realy player not worker
            OnCropHervestedCallback((CropData)data);
    }

    public void ListenToSoldCrop(Component sender,object data)
    {
        if(sender is CropBuyer)
        {
            inventoryDisplay.UpdateDisplay(inventory);
            SaveInventory();
        }
    }

    public void AddItemToInventory(E_Inventory_Item_Type item,int amount)
    {
        inventory.AddItemToInventory(item, amount);
        
        inventoryDisplay.UpdateDisplay(inventory);

        SaveInventory();
        
    }
    public void AddInventoryToInventory(Inventory inventory)
    {
        InventoryItem[] temp = inventory.GetInventoryItems();

        for(int i=0 ;i< temp.Length; i++)
        {
            this.inventory.AddItemToInventory(temp[i]);
        }
        inventoryDisplay.UpdateDisplay(this.inventory);
    }

}
