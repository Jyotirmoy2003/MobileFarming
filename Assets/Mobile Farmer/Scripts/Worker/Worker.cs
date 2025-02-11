using System;
using UnityEngine;
using UnityEngine.AI;
using jy_util;


public class Worker : MonoBehaviour
{
    #region STATES
    public WorkerBase currentState ;
    public AssignField assignFieldState = new AssignField();
    public WaitForBarnToClear waitForBarnToClearState = new WaitForBarnToClear();
    public PerformAction performActionState = new PerformAction();
    public HarvestField harvestFieldState = new HarvestField();
    public SowField sowFieldState = new SowField();
    public WaterField waterFieldState = new WaterField();
    public LoadoutToBarn loadoutToBarnState = new LoadoutToBarn();
    public E_Worker_State E_state;

    #endregion



    public Barn allocatedBarn;
    public workerStat workerStat;
    [HideInInspector] public CropField assignedCropField;
    [HideInInspector] public CropFieldDataHolder cropFieldDataHolder;

    [Header("Elements")]
    public NavMeshAgent navMeshAgent;
    public PlayerDataHolder playerDataHolder;



    private Action onDelayDone;
    public int carringCrop = 0;
    public bool isMyBarnFull = false;
    void Start()
    {
        workerStat.allocatedBarn = allocatedBarn;
        currentState = assignFieldState;
        currentState.EnterState(this);
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
        Debug.Log("Last State Was:"+currentState);
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





public class AssignField : WorkerBase
{
    Worker worker;
    public override void EnterState(Worker wk)
    {
        worker = wk;
        worker.assignedCropField = worker.workerStat.allocatedBarn.GetUnlockedField(worker.workerStat.workableCorp); //assign new field
        worker.cropFieldDataHolder = worker.assignedCropField.cropFieldDataHolder;
        worker.navMeshAgent.SetDestination(worker.assignedCropField.transform.position); //for testing let it be here
    }

    public override void ExitState(Worker wk)
    {
        
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
        worker.StartTimmer(3f,DelayDone);
        
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
        
    }

    public override void ExitState(Worker wk)
    {
        worker.allocatedBarn.OnBarnFull -= OnBarnFullCallback;
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
        

        if(worker.navMeshAgent.remainingDistance <=0.1f )
        {
            timmer -= Time.deltaTime;
            if(timmer <= 0)
            {
                worker.carringCrop = 0;
                worker.SwitchState(worker.assignFieldState);
                worker.allocatedBarn.AddItemInInventory(worker.workerStat.workableCorp.item_type,worker.workerStat.maxLoadCapacity);
            }
        }

    }

    void GoToBarn()
    {
        worker.navMeshAgent.SetDestination(worker.workerStat.allocatedBarn.workerLoadOutPos.position);
    }
    void OnBarnFullCallback(E_Inventory_Item_Type item_Type)
    {
        worker.isMyBarnFull = true;
        worker.SwitchState(worker.waitForBarnToClearState);
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
    }

    public override void ExitState(Worker wk)
    {
        worker.allocatedBarn.OnBarnCollected -= OnBarnCollectedCallBack;
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







