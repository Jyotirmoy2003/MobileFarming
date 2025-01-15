using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using jy_util;
using Unity.VisualScripting;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] Transform world;

    [Header("Data")]
    private WorldData worldData;
    string dataPath ;




    void Start()
    {
        dataPath = Application.dataPath + "/WorldData.txt";
        LoadWorld();
        Initialize();
    }


    void Initialize()
    {
        for(int i=0;i<world.childCount;i++)
        {
            world.GetChild(i).GetComponent<Chunk>().Initialize(worldData.chunkPrices[i]);
        }
    }


    private void LoadWorld()
    {
        string data= "";

        if(!File.Exists(dataPath))
        {
            FileStream fs = new FileStream(dataPath, FileMode.Create); //create new file 
            worldData = new WorldData();

            for(int i=0;i<world.childCount;i++)
            {
                worldData.chunkPrices.Add(world.GetChild(i).GetComponent<Chunk>().GetInitialPrice());
            }

            string worldDataString = JsonUtility.ToJson(worldData,true);
            byte[] worldDataByte = Encoding.UTF8.GetBytes(worldDataString);
            fs.Write(worldDataByte);

            //close the the file stream
            fs.Close();
        }else{
            data = File.ReadAllText(dataPath);
            worldData = JsonUtility.FromJson<WorldData>(data);
        }
    }

    private void SaveData()
    {
        if(worldData.chunkPrices.Count != world.childCount)
            worldData = new WorldData();
        
        for(int i=0;i<world.childCount;i++)
        {
            if(worldData.chunkPrices.Count>i)
            {
                worldData.chunkPrices[i]=world.GetChild(i).GetComponent<Chunk>().GetCurrentPrice();
            }else{
                worldData.chunkPrices.Add(world.GetChild(i).GetComponent<Chunk>().GetCurrentPrice());
            }
        }
        
        string data = JsonUtility.ToJson(worldData,true);

        File.WriteAllText(dataPath, data);
        Debug.Log("World data Saved!!");
    }
    

    public void ListenToSaveWorldDataEvent(Component sender,object data)
    {
        if(data is bool)
        {
            if((bool)data) //Save worl data
            {
                SaveData();
            }
        }
    }
}
