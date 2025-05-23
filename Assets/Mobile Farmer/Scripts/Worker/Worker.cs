using System;
using UnityEngine;
using UnityEngine.AI;
using jy_util;


public class Worker : MonoBehaviour
{
    #region STATES
    public WorkerBase currentState ;
    public WorkerIdle workerIdleState = new WorkerIdle();
    public AssignField assignFieldState = new AssignField();
    public WaitForBarnToClear waitForBarnToClearState = new WaitForBarnToClear();
    public PerformAction performActionState = new PerformAction();
    public HarvestField harvestFieldState = new HarvestField();
    public SowField sowFieldState = new SowField();
    public WaterField waterFieldState = new WaterField();
    public LoadoutToBarn loadoutToBarnState = new LoadoutToBarn();
    public ChangeVisual changeVisualState = new ChangeVisual();
    public E_Worker_State E_state;

    #endregion



    [HideInInspector] public Barn allocatedBarn;
    public WorkerStat workerStat;
    [HideInInspector] public CropField assignedCropField;
    [HideInInspector] public CropFieldDataHolder cropFieldDataHolder;

    [Header("Elements")]
    public NavMeshAgent navMeshAgent;
    public PlayerDataHolder playerDataHolder;
    public Billboard popupBillboard;
    public GameObject sackObject;



    private Action onDelayDone;
    public int carringCrop = 0;
    public bool isMyBarnFull = false;

    public DressSetup myDresssetup;

    void Start()
    {
        CacheNewVisual(myDresssetup);

        currentState = workerIdleState;
        currentState.EnterState(this);
        navMeshAgent.avoidancePriority = UnityEngine.Random.Range(50,100);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
        playerDataHolder.playerAnimator.ManageAnimation(navMeshAgent.velocity);
    }

    public void SwitchState( WorkerBase nextState)
    {
        currentState.ExitState(this);
        //Debug.Log("Last State Was:"+currentState);
        nextState.EnterState(this);
        currentState = nextState;
    }


    public void StartTimmer(float time, Action onComplete)
    {
        onDelayDone = onComplete;
        Invoke(nameof(Delay),time);
    }

    void Delay()=>onDelayDone?.Invoke();

    public void ListenToonCropHervested(Component sender,object data)
    {
        if((sender as PlayerDataHolder)== playerDataHolder) //when this worker hervested the crop
        {
            carringCrop += (data as CropData).amountinSingleCrop;

            if(carringCrop >= workerStat.maxLoadCapacity)
            {
                carringCrop = workerStat.maxLoadCapacity;
                currentState.ListenToEvent(sender,data);
            }
        }
    }

    public void ListenToOnBarnFilled(Component sender,object data)
    {
        if(sender as Barn == allocatedBarn)
        {
            if(data as CropData == workerStat.workableCorp)
            {
                //my my barn filled
                SwitchState(waitForBarnToClearState);
            }
        }
    }


    public void StartWorke()
    {
        SwitchState(assignFieldState);
    }

    public void CacheNewVisual(DressSetup dressSetup)
    {
        myDresssetup = Instantiate(dressSetup,transform);
        Destroy(playerDataHolder.playerAnimationEvents.gameObject,0.1f);

        sackObject = myDresssetup.sack;
        playerDataHolder.CacheNewVisual(myDresssetup);
        playerDataHolder.playerAnimator.CacheNewVisual(myDresssetup);
    }

    public void CallVisualChange(DressSetup dressSetup)
    {
        myDresssetup = dressSetup;
        SwitchState(changeVisualState);
    }

}






public abstract class WorkerBase 
{
    public abstract void EnterState(Worker wk);
    public abstract void UpdateState(Worker wk);
    public abstract void ExitState(Worker wk);
    public abstract void ListenToEvent(Component sender,object data, int id,Worker wk);
    public abstract void ListenToEvent(Component sender,object data);
    public abstract void StartTime(String coroutineName,Action OnComplete);

}

public class WorkerIdle : WorkerBase
{
    Worker worker;
    public override void EnterState(Worker wk)
    {
        worker = wk;
        worker.E_state = E_Worker_State.Idle;
        worker.StartTimmer(2f,StartWork);
    }

    public override void ExitState(Worker wk)
    {
        
    }

    public override void ListenToEvent(Component sender, object data, int id, Worker wk)
    {
        
    }

    public override void ListenToEvent(Component sender, object data)
    {
        
    }

    public override void StartTime(string coroutineName, Action OnComplete)
    {
        
    }

    public override void UpdateState(Worker wk)
    {
        
    }

    void StartWork()
    {
        worker.SwitchState(worker.assignFieldState);
    }
}




public class AssignField : WorkerBase
{
    Worker worker;
    Action updateAction;
    public override void EnterState(Worker wk)
    {
        worker = wk;
        worker.E_state = E_Worker_State.AssignField;
        if(worker.assignedCropField == null) worker.assignedCropField = worker.allocatedBarn.GetUnlockedField(worker.workerStat.workableCorp); //assign new field
        worker.cropFieldDataHolder = worker.assignedCropField.cropFieldDataHolder;
        worker.navMeshAgent.SetDestination(worker.assignedCropField.transform.position); //for testing let it be here
        //daly 1s given after setting destination the remaning distance sometimes giving zero for one frame,, and worker goes to reached field state
        worker.StartTimmer(1,Init);
    }
    void Init()
    {
        updateAction += CheckDest;
    }

    public override void ExitState(Worker wk)
    {
        updateAction -= CheckDest;
    }

    public override void ListenToEvent(Component sender, object data, int id, Worker wk)
    {
        if(id == 1) //Cached Event
        {
            worker.navMeshAgent.SetDestination(worker.assignedCropField.transform.position);
        }
    }

    public override void ListenToEvent(Component sender, object data)
    {
        
    }

    public override void StartTime(string coroutineName, Action OnComplete)
    {
        
    }

    public override void UpdateState(Worker wk)
    {
        updateAction?.Invoke();
    }

    void CheckDest()
    {

        if(worker.navMeshAgent.remainingDistance <= 0.1f)
        {
            worker.SwitchState(worker.performActionState);
        }
    }


}


public class PerformAction : WorkerBase
{
    private Worker worker;
    public override void EnterState(Worker wk)
    {
        worker = wk;
        worker.E_state= E_Worker_State.PerformAction;
        worker.StartTimmer(worker.workerStat.performActionDelay,DelayDone);
        
    }

    public override void ExitState(Worker wk)
    {
        
    }

    public override void ListenToEvent(Component sender, object data, int id, Worker wk)
    {
        
    }

    public override void ListenToEvent(Component sender, object data)
    {
        
    }

    public override void StartTime(string coroutineName, Action OnComplete)
    {
        
    }

    public override void UpdateState(Worker wk)
    {
        
    }
    void DelayDone()
    {
        SelectNextState();
    }

    void SelectNextState()
    {

        if(worker.isMyBarnFull)
        {
            worker.SwitchState(worker.waitForBarnToClearState);
            return;
        }
        if(worker.carringCrop >= worker.workerStat.maxLoadCapacity)
        {
            worker.SwitchState(worker.loadoutToBarnState);
            return;
        }
        switch(worker.assignedCropField.state)
        {
            case E_Crop_State.Empty:
            worker.SwitchState(worker.sowFieldState);
            break;
            case E_Crop_State.Sown:
            worker.SwitchState(worker.waterFieldState);
            break;
            case E_Crop_State.Watered:
            worker.SwitchState(worker.harvestFieldState);
            break;
        }
    }
}



public class SowField : WorkerBase
{
    Worker worker;
    CropFieldDataHolder dataHolder;
    int index = -1;
    bool isDone = false;


    public override void EnterState(Worker wk)
    {
        isDone = false;
        index = -1;
        worker = wk;
        worker.E_state= E_Worker_State.SowField;
        dataHolder = wk.cropFieldDataHolder;
        SetNextDest();
        worker.assignedCropField.onFullySown += onCompleteSow;
        worker.assignedCropField.Interact(worker.gameObject);
        worker.navMeshAgent.speed = worker.workerStat.moveSpeedWhileWorking;
    }

    public override void ExitState(Worker wk)
    {
        worker.navMeshAgent.speed = worker.workerStat.walkSpeed;
    }

    public override void ListenToEvent(Component sender, object data, int id, Worker wk)
    {
        
    }

    public override void ListenToEvent(Component sender, object data)
    {
        
    }

    public override void StartTime(string coroutineName, Action OnComplete)
    {
        
    }

    public override void UpdateState(Worker wk)
    {
        if(worker.navMeshAgent.remainingDistance <=0.1f)
        {
            SetNextDest();
        }
    }

    void SetNextDest()
    {
        if(isDone)
        {
            worker.SwitchState(worker.performActionState);
        }
        index=(index+1)%dataHolder.cropField.cropTiles.Count;

        //target next undone tile
        int maxTryCount = 10; //to avoid infinte loop

        
        while(dataHolder.cropField.cropTiles[index].IsSown() && (maxTryCount--  > 0))
        {
            index=(index+1)%dataHolder.cropField.cropTiles.Count;
        }
        
        if(dataHolder.cropField.cropTiles[index].IsSown())
        {
            index = UnityEngine.Random.Range(0,dataHolder.cropField.cropTiles.Count);
        }
        worker.navMeshAgent.SetDestination(dataHolder.cropField.cropTiles[index].transform.position);
    }

    void onCompleteSow(CropField cropField)
    {
        //when compete sowing unsubcribe
        worker.assignedCropField.onFullySown -= onCompleteSow;
        isDone = true;
    }
}

public class WaterField : WorkerBase
{
    Worker worker;
    CropFieldDataHolder dataHolder;
    int index = -1;
    bool isDone = false;
    public override void EnterState(Worker wk)
    {
        isDone = false;
        index = -1;
        worker = wk;
        worker.E_state= E_Worker_State.WaterField;
        dataHolder = wk.cropFieldDataHolder;
        SetNextDest();
        worker.assignedCropField.onFullyWatered += onCompleteWater;
        worker.assignedCropField.Interact(worker.gameObject);
        worker.navMeshAgent.speed = worker.workerStat.moveSpeedWhileWorking;
    }

    public override void ExitState(Worker wk)
    {
        worker.navMeshAgent.speed = worker.workerStat.walkSpeed;
    }

    public override void ListenToEvent(Component sender, object data, int id, Worker wk)
    {
        
    }

    public override void ListenToEvent(Component sender, object data)
    {
        
    }

    public override void StartTime(string coroutineName, Action OnComplete)
    {
        
    }

    public override void UpdateState(Worker wk)
    {
        if(worker.navMeshAgent.remainingDistance <=0.1f)
        {
            SetNextDest();
        }
    }

    void SetNextDest()
    {
        if(isDone)
        {
            worker.SwitchState(worker.performActionState);
        }
        index=(index+1)%dataHolder.cropField.cropTiles.Count;

        //target next undone tile
        int maxTryCount = 10; //to avoid infinte loop

        while(dataHolder.cropField.cropTiles[index].IsWatered() && (maxTryCount--  > 0))
        {
            index=(index+1)%dataHolder.cropField.cropTiles.Count;
        }
        worker.navMeshAgent.SetDestination(dataHolder.cropField.cropTiles[index].transform.position);
    }

    void onCompleteWater(CropField cropField)
    {
        //when compete sowing unsubcribe
        worker.assignedCropField.onFullyWatered -= onCompleteWater;
        isDone = true;
    }
}

public class HarvestField : WorkerBase
{
    Worker worker;
    CropFieldDataHolder dataHolder;
    int index = -1;
    bool isDone = false;
    public override void EnterState(Worker wk)
    {
        isDone = false;
        index = -1;
        worker = wk;
        worker.E_state= E_Worker_State.HervestField;
        dataHolder = wk.cropFieldDataHolder;
        SetNextDest();
        worker.assignedCropField.OnFullyHarvested += onCompleteHervest;
        worker.assignedCropField.Interact(worker.gameObject);
        worker.navMeshAgent.speed = worker.workerStat.walkSpeed+1;
    }

    public override void ExitState(Worker wk)
    {
        worker.assignedCropField.OnFullyHarvested -= onCompleteHervest;
        worker.navMeshAgent.speed = worker.workerStat.walkSpeed;
    }

    public override void ListenToEvent(Component sender, object data, int id, Worker wk)
    {
        
    }

    public override void ListenToEvent(Component sender, object data)
    {
        //worker on full load
        worker.SwitchState(worker.loadoutToBarnState);
    }

    public override void StartTime(string coroutineName, Action OnComplete)
    {
        
    }

    public override void UpdateState(Worker wk)
    {
        if(worker.navMeshAgent.remainingDistance <=0.1f)
        {
            SetNextDest();
        }
    }

    void SetNextDest()
    {
        if(isDone)
        {
            worker.SwitchState(worker.performActionState);
        }
        index=(index+1)%dataHolder.cropField.cropTiles.Count;
        //target next undone tile
        int maxTryCount = 10; //to avoid infinte loop

        while(dataHolder.cropField.cropTiles[index].IsEmpty() && (maxTryCount--  > 0))
        {
            index=(index+1)%dataHolder.cropField.cropTiles.Count;
        }
        worker.navMeshAgent.SetDestination(dataHolder.cropField.cropTiles[index].transform.position);
    }

    void onCompleteHervest(CropField cropField)
    {
        //when compete  unsubcribe
        worker.assignedCropField.OnFullyHarvested -= onCompleteHervest;
        isDone = true;
    }
}

public class LoadoutToBarn : WorkerBase
{
    private Worker worker;
    private float timmer = 5f;
    private bool stopIdle = false;
    public override void EnterState(Worker wk)
    {

        worker = wk;
        worker.E_state= E_Worker_State.LoadoutToBarn;
        timmer = 5f;
        GoToBarn();
        worker.playerDataHolder.playerAnimator.StopAllLayeredAnimation();
        worker.allocatedBarn.OnBarnFull += OnBarnFullCallback;
        worker.sackObject.SetActive(true);
        
    }

    public override void ExitState(Worker wk)
    {
        worker.allocatedBarn.OnBarnFull -= OnBarnFullCallback;
        worker.sackObject.SetActive(false);
    }

    public override void ListenToEvent(Component sender, object data, int id, Worker wk)
    {
        
    }

    public override void ListenToEvent(Component sender, object data)
    {
        
    }

    public override void StartTime(string coroutineName, Action OnComplete)
    {
        
    }

    public override void UpdateState(Worker wk)
    {
        

        if(worker.navMeshAgent.remainingDistance <=1f)
        {
            timmer -= Time.deltaTime;
            if(worker.carringCrop > 0)
            {
                int temp_avl_Space = worker.allocatedBarn.AddItemInInventory(worker.workerStat.workableCorp.item_type,worker.workerStat.maxLoadCapacity);
                
                if(temp_avl_Space < worker.carringCrop) 
                    worker.carringCrop -= temp_avl_Space;
                else
                    worker.carringCrop = 0;
            }
            if(timmer <= 0)
            {
                worker.SwitchState(worker.assignFieldState);
            }
        }

    }

    void GoToBarn()
    {
        worker.navMeshAgent.SetDestination(worker.allocatedBarn.workerLoadOutPos.position);
    }
    void OnBarnFullCallback(E_Inventory_Item_Type item_Type)
    {
        if(item_Type == worker.workerStat.workableCorp.item_type)
        {
            worker.isMyBarnFull = true;
            worker.SwitchState(worker.waitForBarnToClearState);
        }
    }
    
}

public class WaitForBarnToClear : WorkerBase
{
    Worker worker;
    public override void EnterState(Worker wk)
    {
        worker = wk;
        worker.E_state = E_Worker_State.WaitForBarnToClear;
        worker.allocatedBarn.OnBarnCollected +=OnBarnCollectedCallBack;
        worker.navMeshAgent.SetDestination(worker.assignedCropField.transform.position);
        worker.popupBillboard.gameObject.SetActive(true);
        worker.popupBillboard.IsActive = true;
    }

    public override void ExitState(Worker wk)
    {
        worker.allocatedBarn.OnBarnCollected -= OnBarnCollectedCallBack;
        worker.popupBillboard.IsActive = false;
        worker.popupBillboard.gameObject.SetActive(false);
    }

    public override void ListenToEvent(Component sender, object data, int id, Worker wk)
    {
        
    }

    public override void ListenToEvent(Component sender, object data)
    {
        
    }

    public override void StartTime(string coroutineName, Action OnComplete)
    {
        
    }

    public override void UpdateState(Worker wk)
    {
        

    }

    void OnBarnCollectedCallBack()
    {
        worker.isMyBarnFull = false;
        worker.SwitchState(worker.performActionState);
    }
}


public class ChangeVisual : WorkerBase 
{
   private Worker worker;
    private float timmer = 5f;
    private bool stopIdle = false;
    public override void EnterState(Worker wk)
    {

        worker = wk;
        worker.E_state= E_Worker_State.ChangeVisual;
        timmer = 5f;
        GoToBarn();
        worker.playerDataHolder.playerAnimator.StopAllLayeredAnimation();
        worker.allocatedBarn.OnBarnFull += OnBarnFullCallback;
        //worker.sackObject.SetActive(true);
        
    }

    public override void ExitState(Worker wk)
    {
        worker.CacheNewVisual(worker.myDresssetup);
        worker.allocatedBarn.OnBarnFull -= OnBarnFullCallback;
        worker.sackObject.SetActive(false);
    }

    public override void ListenToEvent(Component sender, object data, int id, Worker wk)
    {
        
    }

    public override void ListenToEvent(Component sender, object data)
    {
        
    }

    public override void StartTime(string coroutineName, Action OnComplete)
    {
        
    }

    public override void UpdateState(Worker wk)
    {
        

        if(worker.navMeshAgent.remainingDistance <=1f)
        {
            timmer -= Time.deltaTime;
            if(worker.carringCrop > 0)
            {
                int temp_avl_Space = worker.allocatedBarn.AddItemInInventory(worker.workerStat.workableCorp.item_type,worker.workerStat.maxLoadCapacity);
                
                if(temp_avl_Space < worker.carringCrop) 
                    worker.carringCrop -= temp_avl_Space;
                else
                    worker.carringCrop = 0;
            }
            if(timmer <= 0)
            {
                worker.SwitchState(worker.assignFieldState);
            }
        }

    }

    void GoToBarn()
    {
        worker.navMeshAgent.SetDestination(worker.allocatedBarn.workerLoadOutPos.position);
    }
    void OnBarnFullCallback(E_Inventory_Item_Type item_Type)
    {
        if(item_Type == worker.workerStat.workableCorp.item_type)
        {
            worker.isMyBarnFull = true;
            worker.SwitchState(worker.waitForBarnToClearState);
        }
    } 
}






