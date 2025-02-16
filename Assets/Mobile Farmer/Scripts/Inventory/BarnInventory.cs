using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using jy_util;
using UnityEngine;

public class BarnInventory : MonoBehaviour
{
    [Tooltip("All barns must have differnt saveFilename, to read-write on correct file. Don't put '/' in name")]
    [SerializeField] string saveFileName ="BarnInventory.txt";
    private Inventory inventory;
    private string dataPath="";
    public int totalItemsInInventory = 0;
    void Start()
    {
        dataPath = Application.persistentDataPath + "/"+saveFileName;
        #if UNITY_EDITOR
        dataPath = Application.dataPath + "/"+saveFileName;
        #endif

        LoadInventory();
    }

    public Inventory GetInventory()
    {
        return inventory;
    }

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
                
            FileStream fs = new FileStream(dataPath, FileMode.Create); //create new file 
            inventory = new Inventory();
            
            data = JsonUtility.ToJson(inventory,true);
            byte[] dataByte= Encoding.UTF8.GetBytes(data);
            fs.Write(dataByte);

            fs.Close();
        }
        CalulateTotalItem();
    }
    public void CalulateTotalItem()
    {
        totalItemsInInventory = 0;
        foreach(InventoryItem item in inventory.GetInventoryItems())
            totalItemsInInventory += item.amount;
    }

    private void SaveInventory()
    {
        string data = JsonUtility.ToJson(inventory,true);
        File.WriteAllText(dataPath,data);
        
    }


    public void AddItemToInventory(E_Inventory_Item_Type item_Type,int amount)
    {
        //Update inventory data
        inventory.AddItemToInventory(item_Type,amount);
        totalItemsInInventory += amount;
        SaveInventory();
    }

    public void ClearInventory()
    {
        inventory.ClearInventory();
        SaveInventory();
    }

    private void ListenToOnGameCached(Component sender,object data)
    {

    }
}
