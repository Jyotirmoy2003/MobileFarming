using System.Collections.Generic;
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
    [SerializeField] bool isTutorial=false;

    [Header("Data")]
    [SerializeField] string fileName = "/WorldData.txt";
    private WorldData worldData;
    private List<Chunk> worldChunk = new List<Chunk>();
    string dataPath ;

    private bool shouldsave=false;
    [SerializeField]
    private bool isGameUpdated = true;

    [Header(" Chunk Meshes " )]
    [SerializeField] private Mesh[] chunkShapes;


    void Start()
    {

        dataPath = Application.persistentDataPath + fileName;
        #if UNITY_EDITOR
        dataPath = Application.dataPath + fileName;
        #endif

        CacheChunk();

        if(!isTutorial)LoadWorld();
        else LoadTutroalWord();
        
        Invoke(nameof(Initialize),1f);

        //Try to save after every 2s
        InvokeRepeating(nameof(TryToSave),2,5);

    }

    [NaughtyAttributes.Button]
    void UnlockAllChunks()
    {
        foreach(Chunk item in worldChunk) item.Initialize(0);

        SaveData();
    }

    void CacheChunk()
    {
        for(int i=0;i<world.childCount ; i++)
        {
            if(world.GetChild(i).TryGetComponent<Chunk>(out Chunk chunk)) worldChunk.Add(chunk);
        }
    }


    void Initialize()
    {
        
        
        for(int i=0;i<world.childCount;i++)
        {
            
            worldChunk[i].Initialize(GetChunkPricewithId(worldChunk[i].ChunkID));
        
        }
        
        InitializeGrid(); //creates grid
        UpdateGridWall(); //set up walls
        UpdateGridRenderer(); //set up chunk visibility

        SaveData();
    }

    int GetChunkPricewithId(int chunkId)
    {
        foreach(ChunkIdPricePair item in worldData.chunkPrices)
        {
            if(item.chunkId == chunkId) return item.chunkPrice;
        }
        // execution should not come here
        Debug.Log("Unable to Find Price for chunk :"+chunkId+"  Saved:=> "+ worldData.chunkPrices.Count + " inWold:=> "+worldChunk.Count);
        return 555;
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

        SetUpCropFieldDataHolder();
    }

    void SetUpCropFieldDataHolder()
    {
         for(int j=0; j<grid.GetLength(0); j++)
        {
            for(int i=0;i<grid.GetLength(1);i++)
            {
                if(grid[j,i] == null) continue;


                CropFieldDataHolder dataHolder = grid[j,i].cropFieldDataHolder;
                if(dataHolder == null) continue;

                if(dataHolder.right == null)
                {
                    dataHolder.right = (IsValidGridPosition(j+1,i))? grid[j+1,i]?.cropFieldDataHolder : null;
                    if(dataHolder.right)dataHolder.right.left = dataHolder;
                }

                if(dataHolder.left == null)
                {
                    dataHolder.left = (IsValidGridPosition(j-1,i))? grid[j-1,i]?.cropFieldDataHolder:null;
                    if(dataHolder.left)dataHolder.left.right = dataHolder;
                }

                if(dataHolder.above == null)
                {
                    dataHolder.above = (IsValidGridPosition(j,i+1))? grid[j,i+1]?.cropFieldDataHolder:null;
                    if(dataHolder.above)dataHolder.above.bottom = dataHolder;
                }

                if(dataHolder.bottom == null)
                {
                    dataHolder.bottom = (IsValidGridPosition(j,i-1))? grid[j,i-1]?.cropFieldDataHolder:null;
                    if(dataHolder.bottom)dataHolder.bottom.above = dataHolder;
                }

                //corners
                if(dataHolder.left_above == null)
                {
                    dataHolder.left_above = (IsValidGridPosition(j-1,i+1))? grid[j-1,i+1]?.cropFieldDataHolder:null;
                    if(dataHolder.left_above)dataHolder.left_above.right_bottom = dataHolder;
                }

                if(dataHolder.left_bottom == null)
                {
                    dataHolder.left_bottom = (IsValidGridPosition(j-1,i-1))? grid[j-1,i-1]?.cropFieldDataHolder:null;
                    if(dataHolder.left_bottom) dataHolder.left_bottom.right_above = dataHolder;
                }

                if(dataHolder.right_above == null)
                {
                    dataHolder.right_above = (IsValidGridPosition(j+1,i+1))? grid[j+1,i+1]?.cropFieldDataHolder:null;
                    if(dataHolder.right_above) dataHolder.right_above.left_bottom = dataHolder;
                }

                if(dataHolder.right_bottom == null)
                {
                    dataHolder.right_bottom = (IsValidGridPosition(j+1,i-1))? grid[j+1,i-1]?.cropFieldDataHolder:null;
                    if(dataHolder.right_bottom) dataHolder.right_bottom.left_above = dataHolder;
                }


            }

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
                //Debug.Log("Name: "+chunk.gameObject.name+"Lock status: "+chunk.IsUnclocked());
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

        worldData = SaveAndLoad.Load<WorldData>(dataPath);
        if(worldData == null)
        {
            worldData = new WorldData();

            for(int i=0;i<world.childCount;i++)
            {
                worldData.chunkPrices.Add(new ChunkIdPricePair(worldChunk[i].ChunkID,worldChunk[i].GetInitialPrice()));
            }

            SaveAndLoad.Save(dataPath,worldData);
        }else{
                UpdateData();
        }
    }

    private void LoadTutroalWord()
    {
        // worldData = new WorldData();

        // for(int i=0;i<world.childCount;i++)
        // {
        //     if(world.GetChild(i).TryGetComponent<Chunk>(out var chunk))
        //             worldData.chunkPrices.Add(new ChunkIdPricePair(chunk.GetInitialPrice(),chunk.ChunkID));
        // }
    }

    private void UpdateData()
    {
        int savedChunkCount = worldData.chunkPrices.Count;
        int currentChunkCount = world.childCount;
        int missingData = currentChunkCount - savedChunkCount;

        Debug.Log($"<color=green>Missing data:</color> {missingData} | World Child Count: {currentChunkCount} | Saved Chunk Count: {savedChunkCount}");

        if(missingData == 0 && isGameUpdated)
        {
            //when word has data that is not saved in file
            for(int i=0;i<worldChunk.Count;i++)
            {
                if(!ContainsChunkIdInSaved(worldChunk[i].ChunkID))
                {
                    worldData.chunkPrices.Add(new ChunkIdPricePair(worldChunk[i].ChunkID,worldChunk[i].GetInitialPrice()));
                }
            }

            //when file has data that is not needed in world
            List<int> NeedtoRemoveIndex = new List<int>();

            for(int i=0;i<worldData.chunkPrices.Count ; i++)
            {
                if(!ContainsChunkIdInWorld(worldData.chunkPrices[i].chunkId)) NeedtoRemoveIndex.Add(i);

            }

            for(int i=0; i<NeedtoRemoveIndex.Count ;i++)
            {
                worldData.chunkPrices.RemoveAt(NeedtoRemoveIndex[i]);
            }

        
            shouldsave = true;
        }
        
        // If missing chunks exist, add their initial price
        if (missingData > 0)
        {
            

           
            int index = 0;
            while(world.childCount > worldData.chunkPrices.Count)
            {
                
                if(!ContainsChunkIdInSaved(worldChunk[index].ChunkID))
                {
                    Debug.Log("<color=yellow> Making new Chunk to Save data </color>" +worldChunk[index].ChunkID);
                    //found a new Chunk add this
                    worldData.chunkPrices.Add(new ChunkIdPricePair(worldChunk[index].ChunkID,worldChunk[index].GetInitialPrice()));
                }
                index ++;
                
            }

           

            shouldsave = true; // Mark as needing save
        }
        
        // Optional: Ensure no extra old data lingers
        if (savedChunkCount > currentChunkCount)
        {
            Debug.LogWarning("Detected more saved chunks than present in the world. Trimming extra data...");
            // worldData.chunkPrices.RemoveRange(currentChunkCount, savedChunkCount - currentChunkCount);
            // shouldsave = true; // Mark as needing save

            List<int> NeedtoRemoveIndex = new List<int>();

            for(int i=0;i<worldData.chunkPrices.Count ; i++)
            {
                if(!ContainsChunkIdInWorld(worldData.chunkPrices[i].chunkId)) NeedtoRemoveIndex.Add(i);

            }

            for(int i=0; i<NeedtoRemoveIndex.Count ;i++)
            {
                worldData.chunkPrices.RemoveAt(NeedtoRemoveIndex[i]);
            }

        
            shouldsave = true;
        }



       
    }

    void DebugSavedChunkPrices()
    {
        Debug.Log("<color=green> =============Debuging prices============</color>");
        for(int i=0;i<worldData.chunkPrices.Count; i++)
            {
                Debug.Log(worldData.chunkPrices[i].chunkPrice);
            }
    }

    bool ContainsChunkIdInSaved(int chunkId)
    {
        foreach(ChunkIdPricePair item in worldData.chunkPrices)
        {
            if(item.chunkId == chunkId) return true;
        }

        return false;;
    }

    bool ContainsChunkIdInWorld(int chunkId)
    {
        for(int i = 0 ; i<world.childCount ;i++)
        {
            if(worldChunk[i].ChunkID == chunkId) return true;
        }

        return false;
    }


    private void TryToSave()
    {
        if(shouldsave) SaveData();
    }

    private void SaveData()
    {
        shouldsave =false;
       

       
        for(int i=0;i<world.childCount;i++)
        {
            SetNewPriceWithChunkID(worldChunk[i].ChunkID,worldChunk[i].GetCurrentPrice());
            
        }
        
        SaveAndLoad.Save<WorldData>(dataPath,worldData);
        
    }

    void SetNewPriceWithChunkID(int chunkId,int currentPrice)
    {
        foreach(ChunkIdPricePair item in worldData.chunkPrices)
        {
            if(item.chunkId == chunkId) item.chunkPrice = currentPrice;
        }
        
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
