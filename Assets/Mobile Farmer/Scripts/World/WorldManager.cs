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
    string dataPath ;

    private bool shouldsave=false;

    [Header(" Chunk Meshes " )]
    [SerializeField] private Mesh[] chunkShapes;


    void Start()
    {

        dataPath = Application.persistentDataPath + fileName;
        #if UNITY_EDITOR
        dataPath = Application.dataPath + fileName;
        #endif
        if(!isTutorial)LoadWorld();
        else LoadTutroalWord();
        
        Invoke(nameof(Initialize),1f);

        //Try to save after every 2s
        InvokeRepeating(nameof(TryToSave),2,5);

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

        SaveData();
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
                worldData.chunkPrices.Add(world.GetChild(i).GetComponent<Chunk>().GetInitialPrice());
            }
            SaveAndLoad.Save(dataPath,worldData);
        }else{
            if(worldData.chunkPrices.Count < world.childCount )
                UpdateData();
        }
    }

    private void LoadTutroalWord()
    {
        worldData = new WorldData();

        for(int i=0;i<world.childCount;i++)
        {
            worldData.chunkPrices.Add(world.GetChild(i).GetComponent<Chunk>().GetInitialPrice());
        }
    }

    private void UpdateData()
    {
        int savedChunkCount = worldData.chunkPrices.Count;
        int currentChunkCount = world.childCount;
        int missingData = currentChunkCount - savedChunkCount;

        Debug.Log($"<color=green>Missing data:</color> {missingData} | World Child Count: {currentChunkCount} | Saved Chunk Count: {savedChunkCount}");

        // If missing chunks exist, add their initial price
        if (missingData > 0)
        {
            for (int i = savedChunkCount; i < currentChunkCount; i++)
            {
                int chunkPrice = world.GetChild(i).GetComponent<Chunk>().GetInitialPrice();
                worldData.chunkPrices.Add(chunkPrice);
            }

            shouldsave = true; // Mark as needing save
        }
        
        // Optional: Ensure no extra old data lingers
        if (savedChunkCount > currentChunkCount)
        {
            Debug.LogWarning("Detected more saved chunks than present in the world. Trimming extra data...");
            worldData.chunkPrices.RemoveRange(currentChunkCount, savedChunkCount - currentChunkCount);
            shouldsave = true; // Mark as needing save
        }

        // Save only if changes were made
        if (shouldsave)
        {
            SaveData();
        }
    }


    private void TryToSave()
    {
        if(shouldsave) SaveData();
    }

    private void SaveData()
    {
        shouldsave =false;
        // if(worldData.chunkPrices.Count != world.childCount)
        //     worldData = new WorldData();

        // Ensure the list is large enough but don't wipe previous data
        while (worldData.chunkPrices.Count < world.childCount)
        {
            worldData.chunkPrices.Add(0); // Default price, or use a method to get new chunk prices
        }
        
        for(int i=0;i<world.childCount;i++)
        {
            if(worldData.chunkPrices.Count>i)
            {
                worldData.chunkPrices[i]=world.GetChild(i).GetComponent<Chunk>().GetCurrentPrice();
            }else{
                worldData.chunkPrices.Add(world.GetChild(i).GetComponent<Chunk>().GetCurrentPrice());
            }
        }
        SaveAndLoad.Save<WorldData>(dataPath,worldData);
        
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
