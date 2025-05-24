using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using jy_util;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(BarnInventory))]
public class Barn : MonoBehaviour,IInteractable
{
    [Header("Visual")]
    [SerializeField] FeedBackManager barnFeedback;
    [SerializeField] List<GameObject> sacks = new List<GameObject>();
    [SerializeField] Transform workerSpawnPoint;
    public Transform workerLoadOutPos;
    public List<BarnItem> barnCapableItem = new List<BarnItem>();
    public List<CropField> nearByFields = new List<CropField>();
    [SerializeField] List<WorkerStat> workerStats = new List<WorkerStat>();
    private List<WorkerStat> deepCopyWorkerStats = new List<WorkerStat>();
    [Header("UI ref")]
    public InfoUI infoUI;
    public List<StorageUIStatus> storageUIStatuses = new List<StorageUIStatus>();
    [Header("Settings")]
    [SerializeField] Chunk myChunkRef;
    [SerializeField] string saveFileName = "/BarnData.txt";
    [SerializeField] bool isActive = false; //for testing in editor

    [Header("Barn")]
    [SerializeField] int maxBarnLevel = 5;
    [SerializeField] int barnLevel = 1;
    [SerializeField] int nextUpgradePrice = 2000;


    private List<Worker> workersUnderthisBarn = new List<Worker>();
    private Action WorkerinfoUpdateEvent;
    private string dataPath;
    private BarnInventory barnInventory;
    public Action<E_Inventory_Item_Type> OnBarnFull;
    public Action OnBarnCollected;
    [SerializeField]
    private bool isBarnEmpty = true;
    private WorkerData workerData;
    private bool listenigForInput = false;
    private int selectedWorkerIndex = -1;
    








    void  Start()
    {
        dataPath = Application.persistentDataPath + saveFileName;
        #if UNITY_EDITOR
        dataPath = Application.dataPath + saveFileName;
        #endif

        
        
        barnInventory = GetComponent<BarnInventory>();
        LoadWorker();
        Invoke(nameof(Init),4f);

        SetUpDeepCopyWorkerStats();

        //subcribe
        WorkerinfoUpdateEvent += SetUpDeepCopyWorkerStats;
        WorkerinfoUpdateEvent += SaveWorker;
        

        //initiaze the list first , when worker spawns it holds workers ref, macking sure all index is valid
        workersUnderthisBarn = Enumerable.Repeat<Worker>(null, workerStats.Count).ToList();

        
        
    }
    void SetUpDeepCopyWorkerStats()
    {
        for (int i = 0; i < workerData.workerStatSaves.Count; i++)
        {
            if (i < deepCopyWorkerStats.Count)
            {
                deepCopyWorkerStats[i] = GetDeepCopyWorkerStat(i);
            }
            else
            {
                deepCopyWorkerStats.Add(GetDeepCopyWorkerStat(i));
            }
        }
    }


    //called when chunk unlockes
    void MyChunkUnlocked()
    {
        myChunkRef.chunkUnlocked -= MyChunkUnlocked;
        isActive = true;
        Init();
    }

    void Init()
    {
        if(!myChunkRef.IsUnclocked())
        {
            isActive = false;
            myChunkRef.chunkUnlocked += MyChunkUnlocked;
            return;
        }
        

        //set up storage UI
        for(int i=0;i<storageUIStatuses.Count;i++)
            storageUIStatuses[i].slider.maxValue = barnCapableItem[i].maxLoadCapacity;
        
        UpdateUiDisplay();
        UpdateSackinBarn();



        if(!isActive) return;

        //spawn worker

        for(int i=0 ; i< workerData.workerStatSaves.Count ; i++ )
            if(workerData.workerStatSaves[i].isPurchased) SpawnWorkers(i);
    
        foreach(BarnItem item in barnCapableItem)
            if(barnInventory.GetInventory().GetItemAmountInInventory(item.item_Type) > 0)
            {
                isBarnEmpty = false;
                break;
            }
    
       
    }
  
   public CropField GetUnlockedField(CropData cropData)
   {
        for(int i=0 ;i<nearByFields.Count;i++)
        {
            if(nearByFields[i] != null && nearByFields[i].cropFieldDataHolder.chunk.IsUnclocked() && nearByFields[i].GetCropData()==cropData )//&& !nearByFields[i].cropFieldDataHolder.cropField.IsOccupied)
            {
                nearByFields[i].IsOccupied = true;
                return nearByFields[i];
            }
        }
        Debug.Log("No Field found to assign");
       return nearByFields[0];
   }

    void UpdateSackinBarn()
    {
        int barnTotalCap = 0;
        foreach (BarnItem item in barnCapableItem)
            barnTotalCap += item.maxLoadCapacity;

        int numberOfSacktoShow = (barnInventory.totalItemsInInventory > 0) ? 
                                Mathf.Clamp(barnInventory.totalItemsInInventory * sacks.Count / barnTotalCap, 0, sacks.Count) 
                                : 0;

        for (int i = 0; i < sacks.Count; i++)
        {
            sacks[i].SetActive(i < numberOfSacktoShow);
        }
    }

    




    #region  INTERFACE
    public void InIntreactZone(GameObject interactingObject)
    {
        
    }

    public void Interact(GameObject interactingObject)
    {
        
    }

    public void OutIntreactZone(GameObject interactingObject)
    {
        SubcribeToUiButton(false);
        BarnUImanager.Instance.CloseButtonPressed();
    }

    public void ShowInfo(bool val)
    {
        infoUI.SetActivationStatus(val);
    }
    #endregion

   
    void UpdateUiDisplay()
    {
        Inventory temp_Inventory = barnInventory.GetInventory();
        int temp_ItemAmount = 0;

        for(int i=0;i<storageUIStatuses.Count;i++)
        {
            storageUIStatuses[i].icon.sprite = _GameAssets.Instance.GetCropData(storageUIStatuses[i].Item_Type).uiIconSprite;
            temp_ItemAmount = temp_Inventory.GetItemAmountInInventory(storageUIStatuses[i].Item_Type);

            if(temp_ItemAmount >= barnCapableItem[i].maxLoadCapacity)
            {
                //the item is on full capacity
                storageUIStatuses[i].text.text = "Full!";
            }else{
                storageUIStatuses[i].text.text = temp_ItemAmount+"/"+barnCapableItem[i].maxLoadCapacity;
            }
            
            storageUIStatuses[i].slider.value = temp_ItemAmount;

        }
    }
    void LoadInventoryToPlayer()
    {
        if(!isBarnEmpty)
        {
            PlayParticels();
            InventoryManager.Instance.AddInventoryToInventory(barnInventory.GetInventory());
            barnInventory.ClearInventory();
            OnBarnCollected?.Invoke(); //Fire event
            
            isBarnEmpty = true;
            //update visual
            UpdateUiDisplay();
            barnInventory.CalulateTotalItem();
            UpdateSackinBarn();
        }else{
            //show barn UI
            BarnUImanager.Instance.ShowWorkerData(deepCopyWorkerStats,nearByFields);
            BarnUImanager.Instance.OpenCloseBarnUI(true);
            CameraManager.Instance.SwitchCamera(workerLoadOutPos,new Vector3(0,8,-10),new Vector3(0,0,0));

            //off Info Ui
            infoUI.SetActivationStatus(false);
            SubcribeToUiButton(true);
            
        }
    }
    void PlayParticels()
    {
       
        Inventory temp_Inventory = barnInventory.GetInventory();


        for(int i=0;i<barnCapableItem.Count;i++)
        {
            int amount = temp_Inventory.GetItemAmountInInventory(barnCapableItem[i].item_Type);
            if(amount >0)
            {
                ParticleSystem.Burst burst = barnCapableItem[i].loadOutParticel.emission.GetBurst(0);
                burst.count = amount;
                barnCapableItem[i].loadOutParticel.emission.SetBurst(0, burst);
                barnCapableItem[i].loadOutParticel.Play();
            }

        }
    }
    #region Triggers
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Player"))
        {
            LoadInventoryToPlayer();
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if(collider.CompareTag("Player"))
        {
            OnCloseButtonPressed();
        }
    }

    #endregion

    [NaughtyAttributes.Button]
    void DebguCheckLoad()
    {
        CheckForMaxload(E_Inventory_Item_Type.Corn);
    }
    int CheckForMaxload(E_Inventory_Item_Type item_Type)
    {
        int maxCap = 1;
        foreach(BarnItem barnItem in barnCapableItem)
        {
            if(item_Type == barnItem.item_Type)
            {
                maxCap = barnItem.maxLoadCapacity;
            }
        }
        int availableSpace = maxCap-barnInventory.GetInventory().GetItemAmountInInventory(item_Type);
        if(availableSpace <= 0)
        {
           OnBarnFull?.Invoke(item_Type); //Fire Event when BarnFilled
        }

        return availableSpace;
    }


    #region Worker
    void LoadWorker()
    {
        workerData = SaveAndLoad.Load<WorkerData>(dataPath);

        // Ensure workerData is initialized
        if (workerData == null){
            workerData = new WorkerData();
            
        
        }

        // Ensure workerStatSaves list is initialized
        if (workerData.workerStatSaves == null){
            workerData.workerStatSaves = new List<WorkerStatSave>();Debug.Log("worker List is null");}

        // Sync data between saved stats and existing workers
        for (int i = 0; i < workerStats.Count; i++)
        {
            if (i < workerData.workerStatSaves.Count) //not using as we are creating the data there is no list
            {
                // Load saved data
                Debug.Log("Loading saved data");
                // workerStats[i].isPurchased = workerData.workerStatSaves[i].isPurchased;
                // workerStats[i].level = workerData.workerStatSaves[i].level;
                // workerStats[i].price = workerData.workerStatSaves[i].price;
                // workerStats[i].maxLoadCapacity = workerData.workerStatSaves[i].maxLoadCapacity;
            }
            else
            {
                // Add missing workers to save file
                workerData.workerStatSaves.Add(new WorkerStatSave(
                    workerStats[i].isPurchased, 
                    workerStats[i].level, 
                    workerStats[i].price, 
                    workerStats[i].maxLoadCapacity
                ));
            }
        }

        // Only save if new data was added
        if (workerData.workerStatSaves.Count > workerStats.Count)
            SaveWorker();
    }
   
    void SaveWorker()
    {
        SaveAndLoad.Save<WorkerData>(dataPath,workerData);
    }
    void SpawnWorkers(int index)
    {
        
        Worker temp=Instantiate(workerStats[index].workerPrefab,workerSpawnPoint.position,workerSpawnPoint.rotation);
        temp.allocatedBarn = this;
        temp.workerStat = deepCopyWorkerStats[index];
        workerData.workerStatSaves[index].isPurchased = true;
        workersUnderthisBarn[index] = temp;
    }

    

    WorkerStat GetDeepCopyWorkerStat(int index)
    {
        WorkerStat result ;
        result = new WorkerStat(workerStats[index]);
        
        //Override Loaded data into new copy
        result.isPurchased = workerData.workerStatSaves[index].isPurchased;
        result.level = workerData.workerStatSaves[index].level;
        result.price = workerData.workerStatSaves[index].price;
        result.maxLoadCapacity = workerData.workerStatSaves[index].maxLoadCapacity;

        return result;
    }
    public int AddItemInInventory(E_Inventory_Item_Type item_Type,int amount)
    {
        isBarnEmpty = false;
        //show pop feedback
        barnFeedback.PlayFeedback();


        int availableSpace = CheckForMaxload(item_Type);
        
        //check for spcae in barn
        if(availableSpace>=amount)
            barnInventory.AddItemToInventory(item_Type,amount);
        else if(availableSpace > 0) //Add only the amount of space available
        {
            barnInventory.AddItemToInventory(item_Type,availableSpace);
            OnBarnFull?.Invoke(item_Type); //Fire Event when BarnFilled
        }

        //Update slider UI
        UpdateUiDisplay();
        UpdateSackinBarn();

        return availableSpace;
    }
    
    #region UI
    void SubcribeToUiButton(bool val)
    {
        if(val)
        {
            BarnUImanager.Instance.hireButtonPressed += OnHireButtonPressed;
            BarnUImanager.Instance.closeButtonPressed += OnCloseButtonPressed;
            BarnUImanager.Instance.clothButtonPressed += OnClothButtonPressed;
        }else
        {
            BarnUImanager.Instance.hireButtonPressed -= OnHireButtonPressed;
            BarnUImanager.Instance.closeButtonPressed -= OnCloseButtonPressed;
            BarnUImanager.Instance.clothButtonPressed -= OnClothButtonPressed;
        }
    }

    void Hireworker(int index)
    {
       
        if(index >= workerData.workerStatSaves.Count)
        {
            if(barnLevel >= maxBarnLevel)
            {
                nextUpgradePrice = -1;
            }else
            //Barn upgrade button pressed
            if(CashManager.Instance.DebitCoin(nextUpgradePrice))
            {
                barnLevel +=1;
                UpgradeBarn();
                UpdateUiDisplay();
            }
        }else
        if(CashManager.Instance.DebitCoin(workerData.workerStatSaves[index].price))
        {
            barnFeedback.PlayFeedback();
            
            
            if(workerData.workerStatSaves[index].isPurchased)
            {
                workerData.workerStatSaves[index].Upgrade();
                SetUpDeepCopyWorkerStats();
                workersUnderthisBarn[index].workerStat = deepCopyWorkerStats[index];
            }
            else
                SpawnWorkers(index); 
            
            WorkerinfoUpdateEvent?.Invoke(); //fire event when worker data changes
        }
        //update UI
        BarnUImanager.Instance.ShowWorkerData(deepCopyWorkerStats,nearByFields);
        BarnUImanager.Instance.ShowBarnData(nextUpgradePrice,barnCapableItem[0].maxLoadCapacity);
    }

    void UpgradeBarn()
    {
        //cash debited now upgrade barn capacity
        for(int i=0; i<barnCapableItem.Count ; i++)
        {
            barnCapableItem[i].maxLoadCapacity += 50;
        }

        nextUpgradePrice += (int)(nextUpgradePrice * 0.20);
        
    }

    

   

    void OnHireButtonPressed(int index)
    {
        Hireworker(index-1);
    }

    void OnClothButtonPressed(int index)
    {
        listenigForInput = true;
        selectedWorkerIndex = -1;
        for(int i=0 ; i<workersUnderthisBarn.Count ; i++)
        {
            if(workersUnderthisBarn[i] != null && workersUnderthisBarn[i].workerStat.workableCorp == workerStats[index-1].workableCorp) {
                selectedWorkerIndex = i;
                break;
            }
        }

        Debug.Log("Selected index: "+selectedWorkerIndex);
        if(selectedWorkerIndex >= 0) //when its a valid index
        {
            SceneManager.LoadScene("DressingRoom", LoadSceneMode.Additive);
        }
    }

    void OnCloseButtonPressed()
    {
        listenigForInput = false;

        try{SceneManager.UnloadSceneAsync("DressingRoom");}
        catch{}

        //Revart back to main
        CameraManager.Instance.SwitchCamera();
        BarnUImanager.Instance.OpenCloseBarnUI(false); 
        SubcribeToUiButton(false);
        
    }
    #endregion

    #endregion

    public void ListnToOnNewDressSelected(Component sender, object data)
    {
        if(!listenigForInput) return;
        workersUnderthisBarn[selectedWorkerIndex].CallVisualChange(data as DressSetup);
        OnCloseButtonPressed();
    }
    
}

