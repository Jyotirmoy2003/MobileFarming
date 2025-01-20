using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using jy_util;

using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] Transform world;
    Chunk[,] grid;

    [Header("Settings")]
    [SerializeField] int gridSize=20;
    [SerializeField] int gridSale=5;

    [Header("Data")]
    private WorldData worldData;
    string dataPath ;

    private bool shouldsave=false;

    [Header(" Chunk Meshes " )]
    [SerializeField] private Mesh[] chunkShapes;


    void Start()
    {
        dataPath = Application.dataPath + "/WorldData.txt";
        LoadWorld();
        Initialize();

        //Try to save after every 2s
        InvokeRepeating(nameof(TryToSave),2,2);

    }


    void Initialize()
    {
        
        for(int i=0;i<world.childCount;i++)
        {
            world.GetChild(i).GetComponent<Chunk>().Initialize(worldData.chunkPrices[i]);
        
        }
        
        InitializeGrid(); //creates grid
        UpdateGridWall(); //set up walls
        UpdateGridRenderer(); //set up chunk visibility
    }



    private void InitializeGrid()
    {
        grid = new Chunk [gridSize,gridSize];

        for(int i=0 ;i< world.childCount;i++)
        {
            Chunk chunk = world.GetChild(i).GetComponent<Chunk>();
            Vector2Int chunkGridPosition = new Vector2Int((int)chunk.transform.position.x/gridSale, (int)chunk.transform.position.z/gridSale);

            chunkGridPosition +=new Vector2Int(gridSize/2,gridSize/2);

            grid[chunkGridPosition.x,chunkGridPosition.y] = chunk;
        }
    }

    private void UpdateGridWall()
    {
        Chunk chunk=null,frontChunk=null,rightChunk=null,leftChunk=null,backChunk=null;
        for(int j=0; j<grid.GetLength(0); j++)
        {
            for(int i=0;i<grid.GetLength(1);i++)
            {
                chunk = grid[j,i];

                if(chunk==null) continue;

                if(IsValidGridPosition(j,i+1))frontChunk = grid[j,i+1];
                if(IsValidGridPosition(j+1,i))rightChunk = grid[j+1,i];
                if(IsValidGridPosition(j,i-1))backChunk = grid[j,i-1];
                if(IsValidGridPosition(j-1,i))leftChunk = grid[j-1,i];

                int configuration = 0;

                if(frontChunk!= null && frontChunk.IsUnclocked())
                    configuration = configuration +1;
                if(rightChunk!=null && rightChunk.IsUnclocked())
                    configuration = configuration +2;
                if(backChunk!=null && backChunk.IsUnclocked())
                    configuration = configuration +4;
                if(leftChunk!=null && leftChunk.IsUnclocked())
                    configuration = configuration +8;


                //we know the chunk wall configuration
                chunk.UpdateWall(configuration);

                //reset all chunks for next iteration
                frontChunk=rightChunk=leftChunk=backChunk=null;
            }
        }
    }

    private void UpdateGridRenderer()
    {
        
        for(int j=0; j<grid.GetLength(0); j++)
        {
            for(int i=0;i<grid.GetLength(1);i++)
            {
                Chunk chunk = grid[j,i];
                if(chunk==null) continue;

                if(chunk.IsUnclocked()) continue; //when chunk is already unlock skip

                Chunk frontChunk = (IsValidGridPosition(j,i+1))? grid[j,i+1]:null;
                Chunk rightChunk = (IsValidGridPosition(j+1,i))? grid[j+1,i]:null;
                Chunk backChunk = (IsValidGridPosition(j,i-1))? grid[j,i-1]:null;
                Chunk leftChunk = (IsValidGridPosition(j-1,i))? grid[j-1,i]:null;

                //if any of the neigoubring chunk is unlocked then show it
                if((frontChunk!=null && frontChunk.IsUnclocked() || (rightChunk!=null && rightChunk.IsUnclocked()) || 
                (leftChunk!=null && leftChunk.IsUnclocked()) || (backChunk!=null && backChunk.IsUnclocked())))
                    chunk.DisplayLockedElements(); 
            }

        }
    }

    private bool IsValidGridPosition(int x,int y)
    {
        if(x<0 || x>=gridSize || y<0 || y>=gridSize)
            return false;
        return true;
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

            if(worldData.chunkPrices.Count < world.childCount )
                UpdateData();
        }
    }

    private void UpdateData()
    {
        //calculate how many chunks are missing
        int missingData = world.childCount - worldData.chunkPrices.Count;
       
        for(int i=0; i<missingData;i++)
        {
            int chunkIndex = world.childCount - missingData + i;
            int chunkPrice = world.GetChild(chunkIndex).GetComponent<Chunk>().GetInitialPrice();
            worldData.chunkPrices.Add(chunkPrice);
        }
    }

    private void TryToSave()
    {
        if(shouldsave) SaveData();
    }

    private void SaveData()
    {
        shouldsave =false;
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
            if((bool)data) //Save world data called when new chunk unlocks
            {
                //Update walls and save new data
                UpdateGridWall();
                UpdateGridRenderer();
                SaveData();
            }
        }
    }
}
