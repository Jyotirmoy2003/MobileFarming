using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BarnInventory : MonoBehaviour
{
    [Tooltip("All barns must have differnt saveFilename, to read-write on correct file. Don't put '/' in name")]
    [SerializeField] string saveFileName ="BarnInventory.txt";
    private Inventory inventory;
    private string dataPath="";
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
                
            File.Create(dataPath);
            inventory = new Inventory();
        }
            
    }

    private void SaveInventory()
    {
        string data = JsonUtility.ToJson(inventory,true);
        File.WriteAllText(dataPath,data);
    }


    public void AddCropToInventory(CropData cropData)
    {
        //Update inventory data
        inventory.OnCropHervestedCallback(cropData);

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
